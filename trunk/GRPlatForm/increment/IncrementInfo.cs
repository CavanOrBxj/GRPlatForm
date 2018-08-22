using GRPlatForm.model.LogicalModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;

namespace GRPlatForm.increment
{
    public class IncrementInfo
    {
        public SendInfo send = new SendInfo();
        /*---------------------2018-06-10添加终端主动上报变更信息-------------------------------*/

        public string GetSequenceCodes()
        {
            SingletonInfo.GetInstance().SequenceCodes += 1;
            return SingletonInfo.GetInstance().SequenceCodes.ToString().PadLeft(16, '0');
        }


        public void TimingTerminalInfo(int type)
        {



            DateTime INow = DateTime.Now;
            XmlDocument xmlHeartDoc = new XmlDocument();
            responseXML rHeart = new responseXML();
            rHeart.SourceAreaCode = ServerForm.strSourceAreaCode;
            rHeart.SourceType = ServerForm.strSourceType;
            rHeart.SourceName = ServerForm.strSourceName;
            rHeart.SourceID = ServerForm.strSourceID;
            rHeart.sHBRONO = ServerForm.strHBRONO;
            string MediaSql = "";
            string strSRV_ID = "";
            string strSRV_CODE = "";
            ServerForm.DeleteFolder(ServerForm.sHeartSourceFilePath);//删除原有XML发送文件的文件夹下的XML
            string frdStateName = "";
            DataTable dtMedia = null;
            DataTable dtSrvMedia = null;
            List<Device> lDev = new List<Device>();
            try
            {
                MediaSql = "select SRV_ID, SRV_CODE, SRV_GOOGLE, SRV_PHYSICAL_CODE, srv_detail, SRV_LOGICAL_CODE, SRV_RMT_STATUS, SRV_MFT_DATE,  srv_updateDate,Terminal_SRV_MFT_DATE,terminal_updateDate from terminalMaintenanceView where deviceTypeId = '" + type + "' ";
                dtMedia = mainForm.dba.getQueryInfoBySQL(MediaSql);

                if (dtMedia.Rows.Count == 0)
                {
                    MediaSql = "select  SRV.SRV_ID,SRV.SRV_CODE,SRV_GOOGLE, SRV_PHYSICAL_CODE,srv_detail,SRV_LOGICAL_CODE,SRV_MFT_DATE,updateDate,SRV_RMT_STATUS  FROM SRV  left join Srvtype on   SRV.DeviceTypeId= Srvtype .srv_id where  Srvtype.srv_id= " + type;
                    dtSrvMedia = mainForm.dba.getQueryInfoBySQL(MediaSql);
                    for (int idtM = 0; idtM < dtSrvMedia.Rows.Count; idtM++)
                    {
                        int srvId = Convert.ToInt32(dtSrvMedia.Rows[idtM]["SRV_ID"].ToString());
                        string TerminalState = dtSrvMedia.Rows[0]["SRV_RMT_STATUS"].ToString();


                        Device DV = new Device();
                        DV.SRV_ID = dtSrvMedia.Rows[idtM][0].ToString();
                        strSRV_CODE = dtSrvMedia.Rows[idtM][1].ToString();
                        #region 自动添加逻辑编码 2018-01-10
                        string SRV_LOGICAL_CODE = dtMedia.Rows[idtM]["SRV_LOGICAL_CODE"].ToString();
                        string areaId = dtMedia.Rows[idtM]["areaId"].ToString();
                        string SRV_LOGICAL_CODE_GB = dtMedia.Rows[idtM]["SRV_LOGICAL_CODE_GB"].ToString();
                        int number = GetGBCodeCount(areaId, SRV_LOGICAL_CODE_GB);

                        LogicalData logicaldata = new LogicalData();
                        logicaldata.srvID = dtMedia.Rows[idtM]["SRV_ID"].ToString();
                        logicaldata.srvAreaID = areaId;
                        logicaldata.LogicalCode = SRV_LOGICAL_CODE;
                        LogicalCoding.LogicalHelper logical = new LogicalCoding.LogicalHelper(logicaldata);

                        if (number > 1)
                        {
                            SRV_LOGICAL_CODE_GB = logical.GetLogicalCodel(SRV_LOGICAL_CODE_GB);

                            logical.UpdateLogicalCode(logicaldata.srvID, SRV_LOGICAL_CODE_GB);
                        }

                        if (string.IsNullOrEmpty(SRV_LOGICAL_CODE_GB) || SRV_LOGICAL_CODE_GB.Length != 23)
                        {


                            SRV_LOGICAL_CODE_GB = logical.GetLogicalAndAddDataBase();
                            if (string.IsNullOrEmpty(SRV_LOGICAL_CODE_GB))
                            {
                               HttpServerFrom. SetManager("区域码有误请认真核对区域码", Color.Red);
                                continue;
                            }
                        }
                        if (!string.IsNullOrEmpty(SRV_LOGICAL_CODE_GB))
                        {
                            if (!(SRV_LOGICAL_CODE_GB.Length == 23 && logical.GetCombAreaCode(SRV_LOGICAL_CODE_GB, areaId)))
                            {
                                SRV_LOGICAL_CODE_GB = logical.GetLogicalAndAddDataBase();
                            }
                        }
                        DV.DeviceID = SRV_LOGICAL_CODE_GB;
                        #endregion

                        DV.DeviceName = dtSrvMedia.Rows[idtM][4].ToString();

                        DV.Latitude = dtSrvMedia.Rows[idtM][2].ToString().Split(',')[0].Substring(0, 6);
                        DV.Longitude = dtSrvMedia.Rows[idtM][2].ToString().Split(',')[1].Substring(0, 6); ;
                        DV.Srv_Mft_Date = dtSrvMedia.Rows[idtM]["SRV_MFT_DATE"].ToString();
                        DV.UpdateDate = dtSrvMedia.Rows[idtM]["updateDate"].ToString();
                        if (string.IsNullOrEmpty(DV.UpdateDate))
                            DV.UpdateDate = "null";

                        rHeart.AddTerminalMaintenance(DV);
                        lDev.Add(DV);
                    }
                }
                else
                {
                    // MediaSql = "select top(99) SRV_ID,SRV_CODE,SRV_GOOGLE from SRV";

                    if (dtMedia != null && dtMedia.Rows.Count > 0)
                    {
                        for (int idtM = 0; idtM < dtMedia.Rows.Count; idtM++)
                        {
                            int srvId = Convert.ToInt32(dtMedia.Rows[idtM]["SRV_ID"].ToString());
                            string TerminalState = dtMedia.Rows[idtM]["SRV_RMT_STATUS"].ToString();


                            Device DV = new Device();
                            DV.SRV_ID = dtMedia.Rows[idtM][0].ToString();
                            strSRV_CODE = dtMedia.Rows[idtM][1].ToString();
                            #region 自动添加逻辑编码 2018-01-10
                            string SRV_LOGICAL_CODE = dtMedia.Rows[idtM]["SRV_LOGICAL_CODE"].ToString();
                            string areaId = dtMedia.Rows[idtM]["areaId"].ToString();
                            string SRV_LOGICAL_CODE_GB = dtMedia.Rows[idtM]["SRV_LOGICAL_CODE_GB"].ToString();
                            int number = GetGBCodeCount(areaId, SRV_LOGICAL_CODE_GB);

                            LogicalData logicaldata = new LogicalData();
                            logicaldata.srvID = dtMedia.Rows[idtM]["SRV_ID"].ToString();
                            logicaldata.srvAreaID = areaId;
                            logicaldata.LogicalCode = SRV_LOGICAL_CODE;
                            LogicalCoding.LogicalHelper logical = new LogicalCoding.LogicalHelper(logicaldata);

                            if (number > 1)
                            {
                                SRV_LOGICAL_CODE_GB = logical.GetLogicalCodel(SRV_LOGICAL_CODE_GB);

                                logical.UpdateLogicalCode(logicaldata.srvID, SRV_LOGICAL_CODE_GB);
                            }

                            if (string.IsNullOrEmpty(SRV_LOGICAL_CODE_GB) || SRV_LOGICAL_CODE_GB.Length != 23)
                            {


                                SRV_LOGICAL_CODE_GB = logical.GetLogicalAndAddDataBase();
                                if (string.IsNullOrEmpty(SRV_LOGICAL_CODE_GB))
                                {
                                    HttpServerFrom.SetManager("区域码有误请认真核对区域码", Color.Red);
                                    continue;
                                }
                            }
                            if (!string.IsNullOrEmpty(SRV_LOGICAL_CODE_GB))
                            {
                                if (!(SRV_LOGICAL_CODE_GB.Length == 23 && logical.GetCombAreaCode(SRV_LOGICAL_CODE_GB, areaId)))
                                {
                                    SRV_LOGICAL_CODE_GB = logical.GetLogicalAndAddDataBase();
                                }
                            }
                            DV.DeviceID = SRV_LOGICAL_CODE_GB;
                            #endregion

                            DV.DeviceName = dtMedia.Rows[idtM][4].ToString();

                            DV.Latitude = dtMedia.Rows[idtM][2].ToString().Split(',')[0];
                            if (DV.Latitude.Split('.')[1].Length > 6)
                                DV.Latitude = DV.Latitude.Split('.')[0] + "." + DV.Latitude.Split('.')[1].Substring(0, 6);
                            DV.Longitude = dtMedia.Rows[idtM][2].ToString().Split(',')[1];
                            if (DV.Longitude.Split('.')[1].Length > 6)
                                DV.Longitude = DV.Latitude.Split('.')[0] + "." + DV.Longitude.Split('.')[1].Substring(0, 6);
                            DV.Srv_Mft_Date = dtMedia.Rows[idtM]["SRV_MFT_DATE"].ToString();
                            DV.UpdateDate = dtMedia.Rows[idtM]["srv_updateDate"].ToString();
                            /// Terminal_SRV_MFT_DATE,terminal_updateDate
                            DV.Old_Srv_Mft_Date = dtMedia.Rows[idtM]["Terminal_SRV_MFT_DATE"].ToString();
                            DV.Old_UpdateDate = dtMedia.Rows[idtM]["terminal_updateDate"].ToString();
                            if (!(DV.Srv_Mft_Date.IndexOf(DV.Old_Srv_Mft_Date) > -1))
                            {
                                rHeart.UpdateTerminalMaintenance(DV);
                                lDev.Add(DV);
                            }
                            else if (!string.IsNullOrEmpty(DV.UpdateDate) && !string.IsNullOrEmpty(DV.Old_UpdateDate))
                            {
                                if (!(DV.UpdateDate.IndexOf(DV.Old_UpdateDate) > -1))
                                {
                                    rHeart.UpdateTerminalMaintenance(DV);
                                    lDev.Add(DV);

                                }



                            }
                            else
                            {
                                continue;
                            }



                        }

                        //string TrLL = dtMedia.Rows[idtM][2].ToString();
                        //Device DV = new Device();
                        //if (idtM < 10)
                        //{
                        //    DV.DeviceID = "0" + idtM;
                        //}
                        //else { DV.DeviceID = idtM.ToString(); }
                        //strSRV_CODE = dtMedia.Rows[idtM][1].ToString();
                        //DV.DeviceID = strSRV_ID;
                        //DV.DeviceName = strSRV_ID;
                        //if (TrLL != "")
                        //{
                        //    string[] str = TrLL.Split(',');
                        //    if (str.Length >= 2)
                        //    {
                        //        DV.Longitude = str[1];
                        //        DV.Latitude = str[0];
                        //    }
                        //    else
                        //    {
                        //        DV.Longitude = "118.33";
                        //        DV.Latitude = "33.95";
                        //    }
                        //}
                        //else
                        //{
                        //    DV.Longitude = "118.33";
                        //    DV.Latitude = "33.95";
                        //}

                    }



                }
                if (lDev.Count > 0)
                {
                    Random rdState = new Random();
                    frdStateName = "10" + rHeart.sHBRONO + GetSequenceCodes();
                    string xmlEBMStateFileName = "\\EBDB_" + frdStateName + ".xml";
                    xmlHeartDoc = rHeart.DeviceInfoResponse(lDev, frdStateName);
                    CreateXML(xmlHeartDoc, ServerForm.sHeartSourceFilePath + xmlEBMStateFileName);
                    ServerForm.mainFrm.GenerateSignatureFile(ServerForm.sHeartSourceFilePath, frdStateName);
                    ServerForm.tar.CreatTar(ServerForm.sHeartSourceFilePath, ServerForm.sSendTarPath, frdStateName);//使用新TAR
                    string sHeartBeatTarName = ServerForm.sSendTarPath + "\\" + "EBDT_" + frdStateName + ".tar";
                    send.address = ServerForm.sZJPostUrlAddress;
                    send.fileNamePath = sHeartBeatTarName;
                    SendTar.SendTarOrder.sendHelper.AddPostQueue(ServerForm.sZJPostUrlAddress, sHeartBeatTarName);
                }
            }

            catch
            {
            }
            Console.WriteLine((INow - DateTime.Now));

        }
        public int GetGBCodeCount(string areaID, string logicalCode)
        {
            string sql = "select count(*)from srv  where SRV_LOGICAL_CODE='" + logicalCode + "'";
            DataTable dt = mainForm.dba.getQueryInfoBySQL(sql);
            return Convert.ToInt32(dt.Rows[0][0].ToString());
        }
        public void TimingTerminalInfo(int type, string MediaSql)
        {



            DateTime INow = DateTime.Now;
            XmlDocument xmlHeartDoc = new XmlDocument();
            responseXML rHeart = new responseXML();
            rHeart.SourceAreaCode = ServerForm.strSourceAreaCode;
            rHeart.SourceType = ServerForm.strSourceType;
            rHeart.SourceName = ServerForm.strSourceName;
            rHeart.SourceID = ServerForm.strSourceID;
            rHeart.sHBRONO = ServerForm.strHBRONO;

            string strSRV_ID = "";
            string strSRV_CODE = "";
            ServerForm.DeleteFolder(ServerForm.sHeartSourceFilePath);//删除原有XML发送文件的文件夹下的XML
            string frdStateName = "";
            DataTable dtMedia = null;
            DataTable dtSrvMedia = null;
            List<Device> lDev = new List<Device>();
            try
            {

                dtMedia = mainForm.dba.getQueryInfoBySQL(MediaSql);

                if (dtMedia.Rows.Count > 0)
                {

                    // MediaSql = "select top(99) SRV_ID,SRV_CODE,SRV_GOOGLE from SRV";

                    if (dtMedia != null && dtMedia.Rows.Count > 0)
                    {
                        for (int idtM = 0; idtM < dtMedia.Rows.Count; idtM++)
                        {
                            int srvId = Convert.ToInt32(dtMedia.Rows[idtM]["SRV_ID"].ToString());
                            string TerminalState = dtMedia.Rows[idtM]["SRV_RMT_STATUS"].ToString();


                            Device DV = new Device();
                            DV.SRV_ID = dtMedia.Rows[idtM][0].ToString();
                            strSRV_CODE = dtMedia.Rows[idtM][1].ToString();
                            DV.DeviceID = dtMedia.Rows[idtM]["SRV_LOGICAL_CODE"].ToString();

                            DV.DeviceName = dtMedia.Rows[idtM][4].ToString();

                            DV.Latitude = dtMedia.Rows[idtM][2].ToString().Split(',')[0];
                            if (DV.Latitude.Split('.')[1].Length > 6)
                                DV.Latitude = DV.Latitude.Split('.')[0] + "." + DV.Latitude.Split('.')[1].Substring(0, 6);
                            DV.Longitude = dtMedia.Rows[idtM][2].ToString().Split(',')[1];
                            if (DV.Longitude.Split('.')[1].Length > 6)
                                DV.Longitude = DV.Latitude.Split('.')[0] + "." + DV.Longitude.Split('.')[1].Substring(0, 6);
                            DV.Srv_Mft_Date = dtMedia.Rows[idtM]["SRV_MFT_DATE"].ToString();
                            DV.UpdateDate = dtMedia.Rows[idtM]["srv_updateDate"].ToString();
                            /// Terminal_SRV_MFT_DATE,terminal_updateDate
                            DV.Old_Srv_Mft_Date = dtMedia.Rows[idtM]["Terminal_SRV_MFT_DATE"].ToString();
                            DV.Old_UpdateDate = dtMedia.Rows[idtM]["terminal_updateDate"].ToString();
                            if (!(DV.Srv_Mft_Date.IndexOf(DV.Old_Srv_Mft_Date) > -1))
                            {
                                rHeart.UpdateTerminalMaintenance(DV);
                                lDev.Add(DV);
                            }
                            else if (!string.IsNullOrEmpty(DV.UpdateDate) && !string.IsNullOrEmpty(DV.Old_UpdateDate))
                            {
                                if (!(DV.UpdateDate.IndexOf(DV.Old_UpdateDate) > -1))
                                {
                                    rHeart.UpdateTerminalMaintenance(DV);
                                    lDev.Add(DV);

                                }



                            }
                            else
                            {
                                continue;
                            }



                        }

                        //string TrLL = dtMedia.Rows[idtM][2].ToString();
                        //Device DV = new Device();
                        //if (idtM < 10)
                        //{
                        //    DV.DeviceID = "0" + idtM;
                        //}
                        //else { DV.DeviceID = idtM.ToString(); }
                        //strSRV_CODE = dtMedia.Rows[idtM][1].ToString();
                        //DV.DeviceID = strSRV_ID;
                        //DV.DeviceName = strSRV_ID;
                        //if (TrLL != "")
                        //{
                        //    string[] str = TrLL.Split(',');
                        //    if (str.Length >= 2)
                        //    {
                        //        DV.Longitude = str[1];
                        //        DV.Latitude = str[0];
                        //    }
                        //    else
                        //    {
                        //        DV.Longitude = "118.33";
                        //        DV.Latitude = "33.95";
                        //    }
                        //}
                        //else
                        //{
                        //    DV.Longitude = "118.33";
                        //    DV.Latitude = "33.95";
                        //}

                    }



                }
                if (lDev.Count > 0)
                {
                    Random rdState = new Random();
                    frdStateName = "10" + rHeart.sHBRONO + GetSequenceCodes();
                    string xmlEBMStateFileName = "\\EBDB_" + frdStateName + ".xml";
                    xmlHeartDoc = rHeart.DeviceInfoResponse(lDev, frdStateName);
                    CreateXML(xmlHeartDoc, ServerForm.sHeartSourceFilePath + xmlEBMStateFileName);
                    ServerForm.mainFrm.GenerateSignatureFile(ServerForm.sHeartSourceFilePath, frdStateName);
                    ServerForm.tar.CreatTar(ServerForm.sHeartSourceFilePath, ServerForm.sSendTarPath, frdStateName);//使用新TAR
                    string sHeartBeatTarName = ServerForm.sSendTarPath + "\\" + "EBDT_" + frdStateName + ".tar";
                    send.address = ServerForm.sZJPostUrlAddress;
                    send.fileNamePath = sHeartBeatTarName;
                    SendTar.SendTarOrder.sendHelper.AddPostQueue(ServerForm.sZJPostUrlAddress, sHeartBeatTarName);
                }
            }

            catch
            {
            }
            Console.WriteLine((INow - DateTime.Now));

        }
        public void AdapterPost(string BrdStateDesc, string BrdStateCode)
        {
            XmlDocument xmlHeartDoc = new XmlDocument();
            responseXML rHeart = new responseXML();
            rHeart.SourceAreaCode = ServerForm.strSourceAreaCode;
            rHeart.SourceType = ServerForm.strSourceType;
            rHeart.SourceName = ServerForm.strSourceName;
            rHeart.SourceID = ServerForm.strSourceID;
            rHeart.sHBRONO = ServerForm.strHBRONO;

            ServerForm.DeleteFolder(ServerForm.sHeartSourceFilePath);//删除原有XML发送文件的文件夹下的XML
            string frdStateName = "";
            try
            {
                Random rdState = new Random();
                frdStateName = "10" + rHeart.sHBRONO + GetSequenceCodes();
                string xmlEBMStateFileName = "\\EBDB_" + frdStateName + ".xml";

                xmlHeartDoc = rHeart.AdapterStateRequestResponse(frdStateName,BrdStateDesc,BrdStateCode);

                CreateXML(xmlHeartDoc, ServerForm.sHeartSourceFilePath + xmlEBMStateFileName);
                ServerForm.mainFrm.GenerateSignatureFile(ServerForm.sHeartSourceFilePath, frdStateName);
                ServerForm.tar.CreatTar(ServerForm.sHeartSourceFilePath, ServerForm.sSendTarPath, frdStateName);//使用新TAR
                string sHeartBeatTarName = ServerForm.sSendTarPath + "\\" + "EBDT_" + frdStateName + ".tar";
                SendTar.SendTarOrder.sendHelper.AddPostQueue(ServerForm.sZJPostUrlAddress, sHeartBeatTarName);
            }
            catch (Exception ex)
            {
            }
        }
        private void CreateXML(XmlDocument XD, string Path)
        {
            CommonFunc ComX = new CommonFunc();
            ComX.SaveXmlWithUTF8NotBOM(XD, Path);
            if (ComX != null)
            {
                ComX = null;
            }
        }
    }
}
