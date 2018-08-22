using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GRPlatForm.AudioMessage;
using System.Data;
using System.IO;
using System.Xml;
using System.Drawing;

namespace GRPlatForm.AudioMessage.MQAudio
{
    //播放状态反馈类
    public class AudioPlayState : IPlayState
    {
        private delegate bool HandlingDelegate(string TmcId, string path, string BrdStateDesc, string BrdStateCode);
        private event HandlingDelegate HandlingEvent;
        //未播放

        //播放中

        /// <summary>
        /// 播放完成
        /// </summary>
        /// <returns></returns>
        public bool NotPlay(string TmcId, string path, string BrdStateDesc, string BrdStateCode)
        {


            try
            {

                bool Radio= EmergencyBroadcast(TmcId, path, BrdStateDesc, BrdStateCode,null);
                return Radio;
            }
            catch (Exception ex)
            {
                throw new Exception("未播放:" +ex.Message);
            }
            return false;

        }
        public bool FeedbackFunction(EBD ebdsr, string BrdStateDesc, string BrdStateCode, string TimingTerminalState)
        {
            bool flag = false;
            try
            {
                if (string.IsNullOrEmpty(TimingTerminalState))
                {
                    bool eb = sendEBMStateResponse(ebdsr, BrdStateDesc, BrdStateCode);
                    if (eb)
                    {
                        flag = true;
                    }
                }
                else
                {
                    bool eb= sendEBMStateResponse(ebdsr, BrdStateDesc, BrdStateCode);
                    bool Up = UpdateState(TimingTerminalState);
                    if (eb && Up)
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return flag;
        }
        private bool EmergencyBroadcast(string TmcId, string path, string BrdStateDesc, string BrdStateCode,string TimingTerminalState)
        {
            EBD ebd;
            DataTable dt;
            bool flag = false;
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8);
                    String xmlInfo = sr.ReadToEnd();
                    xmlInfo = xmlInfo.Replace("xmlns:xs", "xmlns");
                    sr.Close();
                    xmlInfo = XmlSerialize.ReplaceLowOrderASCIICharacters(xmlInfo);
                    xmlInfo = XmlSerialize.GetLowOrderASCIICharacters(xmlInfo);
                    ebd = XmlSerialize.DeserializeXML<EBD>(xmlInfo);
                }
                if (Convert.ToInt32(BrdStateCode) != 0)
                {
                    if (Convert.ToInt32(TmcId) < 0)
                    {
                        return flag;
                    }
                    else
                    {
                        dt = ViewDataTsCmdStore(TmcId);
                        if (dt != null && dt.Rows.Count > 0)
                        {

                            flag = FeedbackFunction(ebd, BrdStateDesc, BrdStateCode, TimingTerminalState);
                            return flag;
                        }

                    }
                }
                else
                {

                    flag = FeedbackFunction(ebd, BrdStateDesc, BrdStateCode, TimingTerminalState);
                    return flag;
                }
                return false;

            }
            catch (Exception ex)
            {
                throw new Exception("应急消息回馈:" + ex.Message);
            }
        }

        public string GetSequenceCodes()
        {
            SingletonInfo.GetInstance().SequenceCodes += 1;
            return SingletonInfo.GetInstance().SequenceCodes.ToString().PadLeft(16, '0');
        }

        private bool sendEBMStateResponse(EBD ebdsr, string BrdStateDesc, string BrdStateCode)
        {
            //*反馈
            #region 先删除解压缩包中的文件

            bool flag = false;
            foreach (string xmlfiledel in Directory.GetFileSystemEntries(HttpServerFrom.sEBMStateResponsePath))
            {
                if (File.Exists(xmlfiledel))
                {
                    FileInfo fi = new FileInfo(xmlfiledel);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(xmlfiledel);//直接删除其中的文件  
                }
            }
            #endregion End
            XmlDocument xmlHeartDoc = new XmlDocument();
            responseXML rHeart = new responseXML();
            rHeart.SourceAreaCode = HttpServerFrom. strSourceAreaCode;
            rHeart.SourceType = HttpServerFrom.strSourceType;
            rHeart.SourceName = HttpServerFrom.strSourceName;
            rHeart.SourceID = HttpServerFrom.strSourceID;
            rHeart.sHBRONO = HttpServerFrom.strHBRONO;
            //try
            //{
            //.HeartBeatResponse();  // rState.EBMStateResponse(ebd);
            Random rd = new Random();
            string fName = HttpServerFrom.ebd.EBDID.ToString();

            Random rdState = new Random();
            string frdStateName = "10" + rHeart.sHBRONO + GetSequenceCodes();
            string xmlEBMStateFileName = "\\EBDB_" + frdStateName + ".xml";


            //  string xmlEBMStateFileName = "\\EBDB_" + ebd.EBDID.ToString() + ".xml";
            // string xmlSignFileName = "\\EBDI_" + ebd.EBDID.ToString() + ".xml";
            //xmlHeartDoc = rHeart.EBMStateResponse(ebd, "EBMStateResponse", fName, BrdStateDesc, BrdStateCode);

            xmlHeartDoc = rHeart.EBMStateRequestResponse(ebdsr, fName, BrdStateDesc, BrdStateCode);
            //string xmlStateFileName = "\\EBDB_000000000001.xml";
           TarXml.AudioResponseXml.CreateXML(xmlHeartDoc, HttpServerFrom.sEBMStateResponsePath + xmlEBMStateFileName);
            //  ServerForm.mainFrm.AudioGenerateSignatureFile(sEBMStateResponsePath, "EBDI",ebd.EBDID.ToString());
            HttpServerFrom.mainFrm.GenerateSignatureFile(HttpServerFrom.sEBMStateResponsePath, frdStateName);

            //string pp= frdStateName
            HttpServerFrom.tar.CreatTar(HttpServerFrom.sEBMStateResponsePath, HttpServerFrom.sSendTarPath, frdStateName);// "HB000000000001");//使用新TAR
            //}
            //catch (Exception ec)
            //{
            //    Log.Instance.LogWrite("应急消息播发状态反馈组包错误：" + ec.Message);
            //}
            //string sHeartBeatTarName = sSendTarPath + "\\" + "HB000000000001" + ".tar";
            string sHeartBeatTarName = HttpServerFrom.sSendTarPath + "\\EBDT_" + frdStateName + ".tar";
            try
            {
               string result=  HttpSendFile.UploadFilesByPost(HttpServerFrom.sZJPostUrlAddress, sHeartBeatTarName);
                if (result != "0")
                {
                    return true;
                }
            }
            catch (Exception w)
            {
                Log.Instance.LogWrite("应急消息播发状态反馈发送平台错误：" + w.Message);
            }
            return flag;
        }

        private bool UpdateState(string TimingTerminalState)
        {
            bool flag = false;
            XmlDocument xmlHeartDoc = new XmlDocument();
            responseXML rHeart = new responseXML();
            rHeart.SourceAreaCode = HttpServerFrom.strSourceAreaCode;
            rHeart.SourceType = HttpServerFrom.strSourceType;
            rHeart.SourceName = HttpServerFrom.strSourceName;
            rHeart.SourceID = HttpServerFrom.strSourceID;
            rHeart.sHBRONO = HttpServerFrom.strHBRONO;
            string MediaSql = "";
            string strSRV_ID = "";
            string strSRV_CODE = "";
            HttpServerFrom.DeleteFolder(HttpServerFrom.sHeartSourceFilePath);//删除原有XML发送文件的文件夹下的XML
            string frdStateName = "";
            List<Device> lDev = new List<Device>();
            try
            {
                //  MediaSql = "select  SRV.SRV_ID,SRV.SRV_CODE,SRV_GOOGLE, SRV_PHYSICAL_CODE,srv_detail  FROM SRV  left join Srvtype on   SRV.DeviceTypeId= Srvtype .srv_id where  Srvtype.srv_id=1";
                MediaSql = "select  SRV.SRV_ID,SRV.SRV_CODE,SRV_GOOGLE, SRV_PHYSICAL_CODE,srv_detail,SRV_LOGICAL_CODE_GB,SRV_MFT_DATE,updateDate,SRV_RMT_STATUS  FROM SRV  left join Srvtype on   SRV.DeviceTypeId= Srvtype .srv_id where  Srvtype.srv_id=1";
                DataTable dtMedia = mainForm.dba.getQueryInfoBySQL(MediaSql);
                if (dtMedia != null && dtMedia.Rows.Count > 0)
                {
                    if (dtMedia.Rows.Count > 100)
                    {
                        int mod = dtMedia.Rows.Count / 100 + 1;
                        for (int i = 0; i < mod; i++)
                        {
                            for (int idtM = 0; idtM < dtMedia.Rows.Count; idtM++)
                            {
                                Device DV = new Device();
                                DV.SRV_ID = dtMedia.Rows[idtM][0].ToString();
                                strSRV_CODE = dtMedia.Rows[idtM][1].ToString();
                                DV.DeviceID = dtMedia.Rows[idtM]["SRV_LOGICAL_CODE_GB"].ToString();//修改于20180819 把资源码换成23位

                                DV.DeviceName = dtMedia.Rows[idtM][4].ToString();

                                DV.Latitude = dtMedia.Rows[idtM][2].ToString().Split(',')[0];
                                DV.Longitude = dtMedia.Rows[idtM][2].ToString().Split(',')[1];
                                DV.Srv_Mft_Date = dtMedia.Rows[idtM]["SRV_MFT_DATE"].ToString();
                                DV.UpdateDate = dtMedia.Rows[idtM]["updateDate"].ToString();



                                DV.DeviceState = TimingTerminalState;

                                lDev.Add(DV);
                            }
                            Random rdState = new Random();
                            frdStateName = "10" + rHeart.sHBRONO + GetSequenceCodes();
                            string xmlEBMStateFileName = "\\EBDB_" + frdStateName + ".xml";

                            xmlHeartDoc = rHeart.DeviceStateResponse(lDev, frdStateName);
                            TarXml.AudioResponseXml. CreateXML(xmlHeartDoc, HttpServerFrom.sHeartSourceFilePath + xmlEBMStateFileName);
                            HttpServerFrom.mainFrm.GenerateSignatureFile(HttpServerFrom.sHeartSourceFilePath, frdStateName);
                            HttpServerFrom.tar.CreatTar(HttpServerFrom.sHeartSourceFilePath, HttpServerFrom.sSendTarPath, frdStateName);//使用新TAR
                            string sHeartBeatTarName = HttpServerFrom.sSendTarPath + "\\" + "EBDT_" + frdStateName + ".tar";
                           string result =   SendTar.SendTarOrder.sendHelper.AddPostQueue (HttpServerFrom.sZJPostUrlAddress, sHeartBeatTarName);
                            if (result == "1")
                            {
                                flag = true;
                            }
                        }
                    }
                    else
                    {
                        for (int idtM = 0; idtM < dtMedia.Rows.Count; idtM++)
                        {
                            Device DV = new Device();
                            DV.SRV_ID = dtMedia.Rows[idtM][0].ToString();
                            strSRV_CODE = dtMedia.Rows[idtM][1].ToString();
                            DV.DeviceID = dtMedia.Rows[idtM]["SRV_LOGICAL_CODE_GB"].ToString();

                            DV.DeviceName = dtMedia.Rows[idtM][4].ToString();

                            DV.Latitude = dtMedia.Rows[idtM][2].ToString().Split(',')[0];
                            DV.Longitude = dtMedia.Rows[idtM][2].ToString().Split(',')[1];
                            DV.Srv_Mft_Date = dtMedia.Rows[idtM]["SRV_MFT_DATE"].ToString();
                            DV.UpdateDate = dtMedia.Rows[idtM]["updateDate"].ToString();
                            DV.DeviceState = TimingTerminalState;
                            lDev.Add(DV);
                        }
                        Random rdState = new Random();
                        frdStateName = "10" + rHeart.sHBRONO + GetSequenceCodes();
                        string xmlEBMStateFileName = "\\EBDB_" + frdStateName + ".xml";

                        xmlHeartDoc = rHeart.DeviceStateResponse(lDev, frdStateName);
                        TarXml.AudioResponseXml. CreateXML(xmlHeartDoc, HttpServerFrom. sHeartSourceFilePath + xmlEBMStateFileName);
                        HttpServerFrom.mainFrm.GenerateSignatureFile(HttpServerFrom.sHeartSourceFilePath, frdStateName);
                        HttpServerFrom.tar.CreatTar(HttpServerFrom.sHeartSourceFilePath, HttpServerFrom.sSendTarPath, frdStateName);//使用新TAR
                        string sHeartBeatTarName = HttpServerFrom.sSendTarPath + "\\" + "EBDT_" + frdStateName + ".tar";
                      string result = SendTar.SendTarOrder.sendHelper.AddPostQueue(HttpServerFrom. sZJPostUrlAddress, sHeartBeatTarName);
                        if (result == "1")
                        {
                            flag = true;
                        }
                    }
                }
                else
                {
                    Random rdState = new Random();
                    frdStateName = "10" + rHeart.sHBRONO + GetSequenceCodes();
                    string xmlEBMStateFileName = "\\EBDB_" + frdStateName + ".xml";

                    xmlHeartDoc = rHeart.DeviceStateResponse(lDev, frdStateName);
                   TarXml.AudioResponseXml. CreateXML(xmlHeartDoc, HttpServerFrom. sHeartSourceFilePath + xmlEBMStateFileName);
                    HttpServerFrom.mainFrm.GenerateSignatureFile(HttpServerFrom.sHeartSourceFilePath, frdStateName);
                    HttpServerFrom.tar.CreatTar(HttpServerFrom.sHeartSourceFilePath, HttpServerFrom.sSendTarPath, frdStateName);//使用新TAR
                    string sHeartBeatTarName = HttpServerFrom.sSendTarPath + "\\" + "EBDT_" + frdStateName + ".tar";
                   string result = SendTar.SendTarOrder.sendHelper.AddPostQueue(HttpServerFrom.sZJPostUrlAddress, sHeartBeatTarName);
                    if (result == "1")
                    {
                        flag = true;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("终端状态变更:" + ex.Message);
            }
            return flag;
        }

        public DataTable ViewDataTsCmdStore(string TsCmd_ID)
        {
            string MediaSql;
            try
            {

                MediaSql = "select TsCmd_ID,TsCmd_ExCute from  TsCmdStore where TsCmd_ID='" + TsCmd_ID + "'";
                //  MediaSql = "select top(1)TsCmd_ID,TsCmd_XmlFile from  TsCmdStore where TsCmd_ValueID = '" + ebd.EBMStateRequest.EBM.EBMID + "' order by TsCmd_Date desc";
                DataTable dtMedia = mainForm.dba.getQueryInfoBySQL(MediaSql);

                return dtMedia != null && dtMedia.Rows.Count > 0 ? dtMedia : null;
            }
            catch (Exception ex)
            {
                throw new Exception("查询TsCmdStore出现异常:" + ex.Message);
            }
            return null;
        }

        public bool Playing(string TmcId, string path, string BrdStateDesc, string BrdStateCode,string TimingTerminalState)
        {
            try
            {
                bool Radio = EmergencyBroadcast(TmcId, path, BrdStateDesc, BrdStateCode, TimingTerminalState);
                return Radio;
            }
            catch (Exception ex)
            {
                throw new Exception("未播放:" + ex.Message);
            }
            return false;
        }

        public bool PlayOver(string TmcId, string path, string BrdStateDesc, string BrdStateCode, string TimingTerminalState)
        {
            try
            {
                bool Radio = EmergencyBroadcast(TmcId, path, BrdStateDesc, BrdStateCode, TimingTerminalState);
                return Radio;
            }
            catch (Exception ex)
            {
                throw new Exception("未播放:" + ex.Message);
            }
            return false;
        }

        public bool Untreated(string path, string BrdStateDesc, string BrdStateCode)
        {
            try
            {
                bool Radio = EmergencyBroadcast("-1", path, BrdStateDesc, BrdStateCode,null);
                return Radio;
            }
            catch (Exception ex)
            {
                throw new Exception("未播放:" + ex.Message);
            }
            return false;
        }
    }
}
