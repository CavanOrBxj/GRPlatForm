using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GRPlatForm.AudioMessage.MQAudio;
using System.Data;
using System.Threading;
using System.IO;
using System.Drawing;
using GRPlatForm.AudioMessage.SendMQ;

namespace GRPlatForm.AudioMessage
{
    public enum AudioType
    {
        Text = 0,
        speech = 1,
    }
    
    public enum AudioPlayState
    {
        NotPlay = 0,
        Playing = 1,
        PlayingOver = 2,
        error = 3
    }
    //播放类
    public class AudioHelper : IAudioHelper
    {
        public readonly Dictionary<string,Thread> th = new  Dictionary<string,Thread>();
        /// <summary>
        /// 播放状态
        /// </summary>
        public AudioPlayState AudioPlayState
        {
            get; set;
        }

        public EBD EBD
        {
            get; set;
        }

        public AudioModel AudioModel
        {
            get;
            set;
        }


        /// <summary>
        /// 播放状态接口
        /// </summary>
        public IPlayState PlayStateInterface { get; set; }




 
        public virtual AudioModel PlayReady(int type,string MQIns)
        {
            try
            {
                if (MoreTime())
                {
                    PlayStateInterface.Untreated(AudioModel.XmlFilaPath, "未处理", "0");
                }
                else
                {
                    bool res= false;
                    string MQInstruction = MQIns;

                    string AreaString = CombinationArea();
                    ///获取TsCmd_ValueID
                    string TsCmd_ValueID = GetTmcValue(AreaString);
                    if (!string.IsNullOrEmpty(TsCmd_ValueID))
                    {
                        string result = InsertTsCmdStore(TsCmd_ValueID, AreaString, MQInstruction, AudioModel.PlayingTime.ToString(), AudioModel.PlayEndTime.ToString());

                        SingletonInfo.GetInstance().DicTsCmd_ID.Add(AreaString, result);
                        if (!string.IsNullOrEmpty(result))
                        {
                            Thread thread;
                            string uuid= Guid.NewGuid().ToString("N");
                            thread = new Thread(delegate () {
                               AudioPlay(type, MQInstruction, result, TsCmd_ValueID);
                             
                                }
                            );
                            SingletonInfo.GetInstance().DicPlayingThread[AudioModel.AeraCodeReal].Add(thread);
                           // th.Add(uuid,thread);
                            thread.IsBackground = true;
                            thread.Start();
                            while (true)
                            {
                                Thread.Sleep(1000);
                                if (thread.ThreadState == ThreadState.Stopped)
                                {
                                    thread.Abort();
                                   // th.Remove(uuid);
                                    SingletonInfo.GetInstance().DicPlayingThread.Remove(AudioModel.AeraCodeReal);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return AudioModel;
            }
            return null;
        }

        public virtual bool MoreTime()
        {
            if (Convert.ToDateTime(AudioModel.PlayEndTime) < DateTime.Now)
                return true;
            return false;
        }

        public EBD GetEBD(string path)
        {
            try
            {
                EBD ebd;
                using (FileStream fs = new FileStream(AudioModel.XmlFilaPath, FileMode.Open))
                {
                    StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8);
                    String xmlInfo = sr.ReadToEnd();
                    xmlInfo = xmlInfo.Replace("xmlns:xs", "xmlns");
                    sr.Close();
                    xmlInfo = XmlSerialize.ReplaceLowOrderASCIICharacters(xmlInfo);
                    xmlInfo = XmlSerialize.GetLowOrderASCIICharacters(xmlInfo);
                    ebd = XmlSerialize.DeserializeXML<EBD>(xmlInfo);
                }
                return ebd;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("文转语读取文件{0},XML失败，错误原因:" + ex.Message));
            }
        }
        /// <summary>
        /// 播放
        /// </summary>
        /// <returns></returns>
        public  bool AudioPlay(int type, string ParamValue, string TsCmd_ID, string TsCmd_ValueID)
        {
            try
            {
                HttpServerFrom.SetManager("EBM开始时间: " + AudioModel.PlayingTime + "===>EBM结束时间: " + AudioModel.PlayEndTime, Color.Green);
                HttpServerFrom.SetManager("播放开始时间: " + AudioModel.PlayingTime + "===>播放结束时间: " + AudioModel.PlayEndTime, Color.Green);
                HttpServerFrom.SetManager("等待播放"+AudioModel.PlayingContent, Color.Green);
                EBD ebd = GetEBD(AudioModel.XmlFilaPath);

                string AreaString = CombinationArea();

                ///未播放
                AudioPlayState = AudioMessage.AudioPlayState.NotPlay;
                lock (HttpServerFrom.PlayBackObject)
                {
                    HttpServerFrom.PlayBack = HttpServerFrom.PlaybackStateType.NotBroadcast;
                }
                #region 未播放
                PlayStateInterface.NotPlay(TsCmd_ID, AudioModel.XmlFilaPath, "未播放", "1");
                #endregion 未播放代码
                //播放中
                #region 播放中

                while (true)
                {
                    DateTime current = DateTime.Now;
                    Thread.Sleep(500);
                    if (DateTime.Compare(current, AudioModel.PlayingTime) > 0)
                    {
                        HttpServerFrom.SetManager("播放开始", Color.Green);
                        lock (HttpServerFrom.PlayBackObject)
                        {
                            HttpServerFrom.PlayBack = HttpServerFrom.PlaybackStateType.Playback;

                        }
                        AudioPlayState = AudioMessage.AudioPlayState.Playing;
                        bool result = SendMQ.MqSendOrder.sendOrder.SendMq(ebd, type, ParamValue, TsCmd_ID, TsCmd_ValueID);
                        PlayStateInterface.Playing(TsCmd_ID, AudioModel.XmlFilaPath, "播放中", "2", "播发中");
                        break;
                    }
                }
                #endregion 播放中代码
                //播放完成
                #region 播放完
                while (true)
                {
                    Thread.Sleep(500);
                    if (DateTime.Compare(DateTime.Now, AudioModel.PlayEndTime)< 0)//结束时间大于当前时间  
                    {
                        string MediaSql = "select TsCmd_ID,TsCmd_ExCute from  TsCmdStore where TsCmd_ID='" + TsCmd_ID + "'";
                        DataTable dtMedia = mainForm.dba.getQueryInfoBySQL(MediaSql);
                        if (dtMedia.Rows[0]["TsCmd_ExCute"].ToString().Contains("播放完毕"))
                        {
                            HttpServerFrom.SetManager("播放结束", Color.Green);
                            lock (HttpServerFrom.PlayBackObject)
                            {
                                HttpServerFrom.PlayBack = HttpServerFrom.PlaybackStateType.PlayOut;

                            }
                            AudioPlayState = AudioMessage.AudioPlayState.PlayingOver;
                            PlayStateInterface.PlayOver(TsCmd_ID, AudioModel.XmlFilaPath, "播放完成", "3", "开机/运行中");
                            SingletonInfo.GetInstance().DicTsCmd_ID.Remove(AreaString);
                            break;
                        }
                    }
                    else
                    {
                        lock (HttpServerFrom.PlayBackObject)
                        {
                            HttpServerFrom.PlayBack = HttpServerFrom.PlaybackStateType.PlayOut;
                        }
                        HttpServerFrom.SetManager("播放结束", Color.Green);
                        //没播放完 但是文件时间到了
                        string strSql = "";
                        if (type == 1)
                        {
                            strSql = string.Format("update PLAYRECORD set PR_REC_STATUS = '{0}' where PR_SourceID='{1}'", "删除", TsCmd_ID);
                            mainForm.dba.UpdateDbBySQL(strSql);
                            string strSqlTsCmdStore = string.Format("update TsCmdStore set TsCmd_ExCute = '{0}' where TsCmd_ID='{1}'", "播放完毕", TsCmd_ID);
                            mainForm.dba.UpdateDbBySQL(strSqlTsCmdStore);
                        }
                        else if (type == 2)
                        {
                            strSql = string.Format("update PLAYRECORD set PR_REC_STATUS = '{0}' ", "删除");
                            mainForm.dba.UpdateDbBySQL(strSql);
                        }
                        SingletonInfo.GetInstance().DicTsCmd_ID.Remove(AreaString);
                        break;
                    }
                }
                #endregion 播放完代码
                GC.Collect();
             
                return true;
            }
            catch (Exception ex)
            {
                AudioPlayState = AudioMessage.AudioPlayState.error;
                Log.Instance.LogWrite(ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 组合
        /// </summary>
        /// <param name="PlayContent"></param>
        /// <returns></returns>
        public virtual string CombinationInstruction()
        {
            return null;
        }
        /// <summary>
        /// 停止播放
        /// </summary>
        /// <returns></returns>
        public virtual bool CancelPlay()
        {
            throw new NotImplementedException();
        }

        protected string CombinationArea()
        {

            string AreaCodeValue = "";
            if (AudioModel.PlayArea.Length > 1)
            {
                for (int i = 0; i < AudioModel.PlayArea.Length; i++)
                {
                    if (i == AudioModel.PlayArea.Length - 1)
                    {
                        AreaCodeValue += "'" + AudioModel.PlayArea[0] + "'";
                    }
                    else
                    {
                        AreaCodeValue += "'" + AudioModel.PlayArea[i] + "',";
                    }
                }
            }
            else
            {
                AreaCodeValue += "'" + AudioModel.PlayArea[0] + "'";
            }
            return AreaCodeValue;
        }
        /// <summary>
        /// 获取TMC区域代码
        /// </summary>
        /// <param name="AreaCode"></param>
        /// <returns></returns>
       protected string GetTmcValue(string AreaCode)
        {
            string sqlQueryTsCmd_ValueID = "select ORG_ID from Organization where GB_CODE in (" + AreaCode + ")";
            DataTable dtMedia = mainForm.dba.getQueryInfoBySQL(sqlQueryTsCmd_ValueID);
            string TsCmd_ValueID = "";
            if (dtMedia!=null&& dtMedia.Rows.Count > 0)
            {
                foreach (DataRow item in dtMedia.Rows)
                {
                    TsCmd_ValueID += item["ORG_ID"].ToString() + ",";
                }

            }
            if (string.IsNullOrEmpty(TsCmd_ValueID))
            {
                return null;
            }
            TsCmd_ValueID = TsCmd_ValueID.Substring(0, TsCmd_ValueID.Length - 1);
            return TsCmd_ValueID;
        }
        protected virtual string InsertTsCmdStore(string TsCmd_ValueID, string m_AreaCode, string Content, string sDateTime, string sEndDateTime)
        {
            try
            {
                string sqlstr = "insert into TsCmdStore(TsCmd_Type, TsCmd_Mode, TsCmd_UserID, TsCmd_ValueID, TsCmd_Params, TsCmd_Date, TsCmd_Status,TsCmd_EndTime,TsCmd_Note,TsCmd_PlayCount)" +
                                                                                 "values('音源播放', '区域', 1,'" + TsCmd_ValueID + "', '" + Content + "', '" + sDateTime + "', 0,'" + sEndDateTime + "'," + "'-1'" + ",'20'" +")";

                string TsCmdStoreID = mainForm.dba.UpdateDbBySQLRetID(sqlstr).ToString();
                if (Convert.ToInt32(TsCmdStoreID) > 0)
                {
                    return TsCmdStoreID;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("插入TsCmdStore失败:" + ex.Message);
            }
            return null;
        }


    }
}
