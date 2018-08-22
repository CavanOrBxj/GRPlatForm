using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GRPlatForm.AudioMessage;
using System.Threading;

namespace GRPlatForm.AudioMessage.MQAudio
{
    public class MQAudioHelper : AudioHelper
    {
        
        public MQAudioHelper()
        {
            PlayStateInterface= new AudioPlayState();
        }
        public MQAudioHelper(AudioModel audio)
        {
            this.AudioModel = audio;

            this.PlayStateInterface = new AudioPlayState();

            if (!AudioModel.TextState)
            {
                AudioModel.PlayingTime = DateTime.Now.AddMinutes(1);
                AudioModel.PlayEndTime = DateTime.Now.AddMinutes(5);
            }
        }

        public static string get_uft8(string unicodeString)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            Byte[] encodedBytes = utf8.GetBytes(unicodeString);
            String decodedString = utf8.GetString(encodedBytes);
            return decodedString;
        }
        protected override string InsertTsCmdStore(string TsCmd_ValueID, string m_AreaCode, string Content, string sDateTime, string sEndDateTime)
        {
            try
            {

                string sqlstr = "";

                if (!string.IsNullOrEmpty(AudioModel.PlayingContent))
                {
                    sqlstr = "insert into TsCmdStore(TsCmd_Type, TsCmd_Mode, TsCmd_UserID, TsCmd_ValueID, TsCmd_Params, TsCmd_Date,TsCmd_ExcuteTime,TsCmd_SaveTime, TsCmd_Status,TsCmd_EndTime,TsCmd_Note,Ebm_ID,AreaCode,MsgTitle,TsCmd_PlayCount)" +
    "values('播放视频', '区域', '"+SingletonInfo.GetInstance().TsCmd_UserID+"','" + TsCmd_ValueID + "', '1~" + AudioModel.PlayingContent + "~0~1200~192~0~1~1', " + "'" + sDateTime + "'" + ",'" + sDateTime + "','" + sDateTime + "', 0,'" + sEndDateTime + "'," + "'-1'" + "," + AudioModel.EBMID + "," + AudioModel.AeraCodeReal + ",'" + AudioModel.MsgTitleNew + "','20'" + ")";
                }
                else
                {
                    sqlstr = "insert into TsCmdStore(TsCmd_Type, TsCmd_Mode, TsCmd_UserID, TsCmd_ValueID, TsCmd_Params, TsCmd_Date,TsCmd_ExcuteTime,TsCmd_SaveTime, TsCmd_Status,TsCmd_EndTime,TsCmd_Note,Ebm_ID,AreaCode,MsgTitle,TsCmd_PlayCount)" +
"values('文本播放', '区域', '"+SingletonInfo.GetInstance().TsCmd_UserID+"','" + TsCmd_ValueID + "', '" + AudioModel.PlayingContent + "~向上移动~10~12~0', " + "'" + sDateTime + "'" + ",'" + sDateTime + "','" + sDateTime + "', 0,'" + sEndDateTime + "'," + "'-1'" + "," + AudioModel.EBMID + "," + AudioModel.AeraCodeReal + ",'" + AudioModel.MsgTitleNew + "','20'" + ")";
                }
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
        public AudioModel PlayReady()
        {
            try
            {
                string MQInstruction = "";
                if (!string.IsNullOrEmpty(AudioModel.PlayingContent))
                {
                    MQInstruction= GetAudioContent(AudioModel.PlayingContent);
                    base.PlayReady(1, MQInstruction);
                }
                else
                {
                    MQInstruction = GetTTSContent(AudioModel.MsgDesc);
                    base.PlayReady(2, MQInstruction);
                }
            
              
            }
            catch (Exception ex)
            {
                return AudioModel; 
            }
            return null;
       }
        private string GetAudioContent(string content)
        {
            string paramValue1 = "";
            if (!string.IsNullOrEmpty(content))
                paramValue1 = "1~" + content + "~0~1000~128~0~1~1";
            return paramValue1;
        }

        private string GetTTSContent(string content)
        {
            string paramValue1 = "";
            if (!string.IsNullOrEmpty(content))
                paramValue1 = content + "~向上移动~10~12~0";
            return paramValue1;
        }
      
        /// <summary>
        /// 取消播放
        /// </summary>
        public override bool CancelPlay()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            return true;
        }



    }
}
