using System;
using System.Collections.Generic;
using System.Xml;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using GRPlatForm.model;
using System.Data;

namespace GRPlatForm
{
    public class responseXML
    {
        private IniFiles serverini = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\Config.ini");
        public string SourceAreaCode = "";
        public string SourceType = "";
        public string SourceName = "";
        public string SourceID = "";
        public string sHBRONO = "0000000000000";//"010332132300000001";//实体编号

        //通用反馈的xml头
        public int xmlHead(XmlDocument xmlDoc, XmlElement xmlElem, EBD ebdsr, string EBDstyle, string strebdid)
        {
            #region 标准头部
            XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            xmlElem.Attributes.Append(xmlns);

            //Version
            XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            xmlProtocolVer.InnerText = "1.0";
            xmlElem.AppendChild(xmlProtocolVer);
            //EBDID
            XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            xmlEBDID.InnerText = strebdid;
            xmlElem.AppendChild(xmlEBDID);

            //EBDType
            XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            xmlEBDType.InnerText = EBDstyle;
            xmlElem.AppendChild(xmlEBDType);

            //Source
            XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            xmlElem.AppendChild(xmlSRC);

            //dest
            XmlElement xmlDEST = xmlDoc.CreateElement("DEST");
            xmlElem.AppendChild(xmlDEST);

            XmlElement xmlSRCEBRID = xmlDoc.CreateElement("EBRID");
            xmlSRCEBRID.InnerText = serverini.ReadValue("INFOSET", "ADAPTERNO");
            xmlSRC.AppendChild(xmlSRCEBRID);


            XmlElement xmlSRCAreaCode1 = xmlDoc.CreateElement("EBRID");
            xmlSRCAreaCode1.InnerText = ServerForm.strHBRONO;
            xmlDEST.AppendChild(xmlSRCAreaCode1);



            XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");
            xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlElem.AppendChild(xmlEBDTime);
            #endregion End
            return 0;
        }

        /// <summary>
        /// －－接收回馈数据包-通用反馈
        /// </summary>
        /// <returns></returns>
        public XmlDocument EBDResponse(EBD ebdsr, string EBDstyle, string strEBDID)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //加入XML的声明段落,Save方法不再xml上写出独立属性GB2312
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);
            xmlHead(xmlDoc, xmlElem, ebdsr, EBDstyle, strEBDID);

            XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            xmlElem.AppendChild(xmlRelatedEBD);

            XmlElement xmlReEBDID = xmlDoc.CreateElement("EBDID");
            xmlReEBDID.InnerText = ebdsr.EBDID;
            xmlRelatedEBD.AppendChild(xmlReEBDID);

            XmlElement xmlEBDResponse = xmlDoc.CreateElement("EBDResponse");
            xmlElem.AppendChild(xmlEBDResponse);

            XmlElement xmlResultCode = xmlDoc.CreateElement("ResultCode");
            xmlResultCode.InnerText = "1";
            xmlEBDResponse.AppendChild(xmlResultCode);

            XmlElement xmlResultDesc = xmlDoc.CreateElement("ResultDesc");
            xmlResultDesc.InnerText = "执行成功";
            xmlEBDResponse.AppendChild(xmlResultDesc);

            return xmlDoc;
        }

        public XmlDocument VerifySignatureError(EBD ebdsr, string EBDstyle, string strEBDID)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //加入XML的声明段落,Save方法不再xml上写出独立属性GB2312
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);
            xmlHead(xmlDoc, xmlElem, ebdsr, EBDstyle, strEBDID);

            XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            xmlElem.AppendChild(xmlRelatedEBD);

            XmlElement xmlReEBDID = xmlDoc.CreateElement("EBDID");
            xmlReEBDID.InnerText = ebdsr.EBDID;
            xmlRelatedEBD.AppendChild(xmlReEBDID);

            XmlElement xmlEBDResponse = xmlDoc.CreateElement("EBDResponse");
            xmlElem.AppendChild(xmlEBDResponse);

            XmlElement xmlResultCode = xmlDoc.CreateElement("ResultCode");
            xmlResultCode.InnerText = "4";
            xmlEBDResponse.AppendChild(xmlResultCode);

            XmlElement xmlResultDesc = xmlDoc.CreateElement("ResultDesc");
            xmlResultDesc.InnerText = "签名验证失败";
            xmlEBDResponse.AppendChild(xmlResultDesc);

            return xmlDoc;
        }


        public XmlDocument CurrencyReback(EBD ebdsr, string EBDstyle, string strEBDID)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //加入XML的声明段落,Save方法不再xml上写出独立属性GB2312
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);
            xmlHead(xmlDoc, xmlElem, ebdsr, EBDstyle, strEBDID);

            XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            xmlElem.AppendChild(xmlRelatedEBD);

            XmlElement xmlReEBDID = xmlDoc.CreateElement("EBDID");
            xmlReEBDID.InnerText = ebdsr.EBDID;
            xmlRelatedEBD.AppendChild(xmlReEBDID);

            XmlElement xmlEBDResponse = xmlDoc.CreateElement("EBDResponse");
            xmlElem.AppendChild(xmlEBDResponse);

            XmlElement xmlResultCode = xmlDoc.CreateElement("ResultCode");
            xmlResultCode.InnerText = "1";
            xmlEBDResponse.AppendChild(xmlResultCode);

            XmlElement xmlResultDesc = xmlDoc.CreateElement("ResultDesc");
            xmlResultDesc.InnerText = "已完成接收";
            xmlEBDResponse.AppendChild(xmlResultDesc);

            return xmlDoc;
        }

        public XmlDocument EBDResponseyunweierror(EBD ebdsr, string EBDstyle, string strEBDID)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //加入XML的声明段落,Save方法不再xml上写出独立属性GB2312
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);
            xmlHead(xmlDoc, xmlElem, ebdsr, EBDstyle, strEBDID);

            XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            xmlElem.AppendChild(xmlRelatedEBD);

            XmlElement xmlReEBDID = xmlDoc.CreateElement("EBDID");
            xmlReEBDID.InnerText = ebdsr.EBDID;
            xmlRelatedEBD.AppendChild(xmlReEBDID);

            XmlElement xmlEBDResponse = xmlDoc.CreateElement("EBDResponse");
            xmlElem.AppendChild(xmlEBDResponse);

            XmlElement xmlResultCode = xmlDoc.CreateElement("ResultCode");
            xmlResultCode.InnerText = "3";
            xmlEBDResponse.AppendChild(xmlResultCode);

            XmlElement xmlResultDesc = xmlDoc.CreateElement("ResultDesc");
            xmlResultDesc.InnerText = "该接口暂不支持";
            xmlEBDResponse.AppendChild(xmlResultDesc);

            return xmlDoc;
        }

        public XmlDocument EBDResponseerror(EBD ebdsr, string EBDstyle, string strEBDID)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //加入XML的声明段落,Save方法不再xml上写出独立属性GB2312
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);
            xmlHead(xmlDoc, xmlElem, ebdsr, EBDstyle, strEBDID);

            XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            xmlElem.AppendChild(xmlRelatedEBD);

            XmlElement xmlReEBDID = xmlDoc.CreateElement("EBDID");
            xmlReEBDID.InnerText = ebdsr.EBDID;
            xmlRelatedEBD.AppendChild(xmlReEBDID);

            XmlElement xmlEBDResponse = xmlDoc.CreateElement("EBDResponse");
            xmlElem.AppendChild(xmlEBDResponse);

            XmlElement xmlResultCode = xmlDoc.CreateElement("ResultCode");
            xmlResultCode.InnerText = "5";
            xmlEBDResponse.AppendChild(xmlResultCode);

            XmlElement xmlResultDesc = xmlDoc.CreateElement("ResultDesc");
            xmlResultDesc.InnerText = "查找不到该EBMID";
            xmlEBDResponse.AppendChild(xmlResultDesc);

            return xmlDoc;
        }

        /// <summary>
        /// 河北－－应急消息播发状态反馈
        /// </summary>
        /// <returns>返回XML文档</returns>
        public XmlDocument EBMStateResponse(EBD ebdsr, string EBDstyle, string strebdid)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //加入XML的声明段落,Save方法不再xml上写出独立属性GB2312
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);
            xmlHead(xmlDoc, xmlElem, ebdsr, EBDstyle, strebdid);

            XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            xmlElem.AppendChild(xmlRelatedEBD);

            XmlElement xmlReEBDID = xmlDoc.CreateElement("EBDID");
            xmlRelatedEBD.AppendChild(xmlReEBDID);
            XmlElement xmlEBMStateResponse = xmlDoc.CreateElement("EBMStateResponse");
            xmlElem.AppendChild(xmlEBMStateResponse);

            //反馈数据的时间
            XmlElement xmlRptTime = xmlDoc.CreateElement("RptTime");
            xmlRptTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlEBMStateResponse.AppendChild(xmlRptTime);
            //应急消息内容信息
            XmlElement xmlEBM = xmlDoc.CreateElement("EBM");
            xmlEBMStateResponse.AppendChild(xmlEBM);
            {
                //发布该应急广播消息的应急广播平台ID
                XmlElement xmlEBEID = xmlDoc.CreateElement("EBEID");
                xmlEBEID.InnerText = ebdsr.SRC.EBRID;
                xmlEBM.AppendChild(xmlEBEID);

                //应急消息ID通过应急广播平台ID和应急广播消息ID区别其他的应急广播消息
                XmlElement xmlEBMID = xmlDoc.CreateElement("EBMID");
                xmlEBMID.InnerText = ebdsr.EBM.EBMID;
                xmlEBM.AppendChild(xmlEBMID);
            }

            //播发状态标志，0：播发失败 1：正在播发 2：播发完成，该字段表明当前的应急广播消息播发是否已完成
            XmlElement xmlBrdStateCode = xmlDoc.CreateElement("BrdStateCode");
            xmlBrdStateCode.InnerText = "2";
            xmlEBMStateResponse.AppendChild(xmlBrdStateCode);

            //播发状态描述
            XmlElement xmlBrdStateDesc = xmlDoc.CreateElement("BrdStateDesc");
          //  xmlBrdStateDesc.InnerText = ebdsr.EBMStateRequest.EBM.s;
            xmlEBMStateResponse.AppendChild(xmlBrdStateDesc);

            //实际覆盖行政区域,该数据元素为可选
            XmlElement xmlCoverage = xmlDoc.CreateElement("Coverage");
            xmlEBMStateResponse.AppendChild(xmlCoverage);
            {
                //实际覆盖区域百分比
                XmlElement xmlCoveragePercent = xmlDoc.CreateElement("CoveragePercent");
                xmlCoveragePercent.InnerText = "90%";
                xmlCoverage.AppendChild(xmlCoveragePercent);

                //区域代码，格式为：（区域编码1，区域编码2）
                XmlElement xmlAreaCode = xmlDoc.CreateElement("AreaCode");
                if (ebdsr.EBM.MsgContent.AreaCode != null)
                {
                    xmlAreaCode.InnerText = ebdsr.EBM.MsgContent.AreaCode;
                }
                xmlCoverage.AppendChild(xmlAreaCode);
            }

            //播发数据详情，可选
            XmlElement xmlResBrdInfo = xmlDoc.CreateElement("ResBrdInfo");
            xmlEBMStateResponse.AppendChild(xmlResBrdInfo);
            {
                //播出情况，可为多个，元素关系参见资源信息数据上报
                XmlElement xmlResBrdItem = xmlDoc.CreateElement("ResBrdItem");
                xmlResBrdInfo.AppendChild(xmlResBrdItem);
                {
                    //反馈时间
                    XmlElement xmlARptTime = xmlDoc.CreateElement("RptTime");
                    xmlARptTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    xmlResBrdItem.AppendChild(xmlARptTime);

                    XmlElement xmlEBEST = xmlDoc.CreateElement("EBEST");
                    xmlEBEST.InnerText = "";
                    xmlResBrdItem.AppendChild(xmlEBEST);
                    {
                        XmlElement xmlEBESTEBEID = xmlDoc.CreateElement("EBEID");
                        xmlEBESTEBEID.InnerText = "";
                        xmlEBEST.AppendChild(xmlEBESTEBEID);
                    }

                    XmlElement xmlEBEAS = xmlDoc.CreateElement("EBEAS");
                    xmlEBEAS.InnerText = "";
                    xmlResBrdItem.AppendChild(xmlEBEAS);
                    {
                        XmlElement xmlEBEASEBEID = xmlDoc.CreateElement("EBEID");
                        xmlEBEASEBEID.InnerText = "";
                        xmlEBEAS.AppendChild(xmlEBEASEBEID);
                    }

                    XmlElement xmlEBEBS = xmlDoc.CreateElement("EBEBS");
                    xmlEBEAS.InnerText = "";
                    xmlResBrdItem.AppendChild(xmlEBEBS);
                    {
                        XmlElement xmlEBEBSEBEID = xmlDoc.CreateElement("EBEID");
                        xmlEBEBSEBEID.InnerText = "";
                        xmlEBEBS.AppendChild(xmlEBEBSEBEID);

                        XmlElement xmlStartTime = xmlDoc.CreateElement("StartTime");
                        xmlStartTime.InnerText = "";
                        xmlEBEBS.AppendChild(xmlStartTime);

                        XmlElement xmlEndTime = xmlDoc.CreateElement("EndTime");
                        xmlEndTime.InnerText = "";
                        xmlEBEBS.AppendChild(xmlEndTime);

                        XmlElement xmlFileURL = xmlDoc.CreateElement("FileURL");
                        xmlEBEBS.AppendChild(xmlFileURL);

                        XmlElement xmlResultCode = xmlDoc.CreateElement("ResultCode");
                        xmlResultCode.InnerText = "2";
                        xmlEBEBS.AppendChild(xmlResultCode);

                        XmlElement xmlResultDesc = xmlDoc.CreateElement("ResultDesc");
                        xmlEBEBS.AppendChild(xmlResultDesc);
                    }
                }
            }
            return xmlDoc;
        }
        public XmlDocument EBMStateResponse(EBD ebdsr, string EBDstyle, string strebdid,string BrdStateDesc, string BrdStateCode)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //加入XML的声明段落,Save方法不再xml上写出独立属性GB2312
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);
            xmlHead(xmlDoc, xmlElem, ebdsr, EBDstyle, strebdid);

            XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            xmlElem.AppendChild(xmlRelatedEBD);

            XmlElement xmlReEBDID = xmlDoc.CreateElement("EBDID");
            xmlRelatedEBD.AppendChild(xmlReEBDID);
            XmlElement xmlEBMStateResponse = xmlDoc.CreateElement("EBMStateResponse");
            xmlElem.AppendChild(xmlEBMStateResponse);

            //反馈数据的时间
            XmlElement xmlRptTime = xmlDoc.CreateElement("RptTime");
            xmlRptTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlEBMStateResponse.AppendChild(xmlRptTime);
            //应急消息内容信息
            XmlElement xmlEBM = xmlDoc.CreateElement("EBM");
            xmlEBMStateResponse.AppendChild(xmlEBM);
            {
                //发布该应急广播消息的应急广播平台ID
                XmlElement xmlEBEID = xmlDoc.CreateElement("EBEID");
                xmlEBEID.InnerText = ebdsr.SRC.EBRID;
                xmlEBM.AppendChild(xmlEBEID);

                //应急消息ID通过应急广播平台ID和应急广播消息ID区别其他的应急广播消息
                XmlElement xmlEBMID = xmlDoc.CreateElement("EBMID");
                xmlEBMID.InnerText = ebdsr.EBM.EBMID;
                xmlEBM.AppendChild(xmlEBMID);
            }

            //播发状态标志，0：播发失败 1：正在播发 2：播发完成，该字段表明当前的应急广播消息播发是否已完成
            XmlElement xmlBrdStateCode = xmlDoc.CreateElement("BrdStateCode");
            xmlBrdStateCode.InnerText = BrdStateCode;
            xmlEBMStateResponse.AppendChild(xmlBrdStateCode);

            //播发状态描述
            XmlElement xmlBrdStateDesc = xmlDoc.CreateElement("BrdStateDesc");
            xmlBrdStateDesc.InnerText = BrdStateDesc;
            xmlEBMStateResponse.AppendChild(xmlBrdStateDesc);

            //实际覆盖行政区域,该数据元素为可选
            XmlElement xmlCoverage = xmlDoc.CreateElement("Coverage");
            xmlEBMStateResponse.AppendChild(xmlCoverage);
            {
                //实际覆盖区域百分比
                XmlElement xmlCoveragePercent = xmlDoc.CreateElement("CoveragePercent");
                xmlCoveragePercent.InnerText = "90%";
                xmlCoverage.AppendChild(xmlCoveragePercent);

                //区域代码，格式为：（区域编码1，区域编码2）
                XmlElement xmlAreaCode = xmlDoc.CreateElement("AreaCode");
                if (ebdsr.EBM.MsgContent.AreaCode != null)
                {
                    xmlAreaCode.InnerText = ebdsr.EBM.MsgContent.AreaCode;
                }
                xmlCoverage.AppendChild(xmlAreaCode);
            }

            //播发数据详情，可选
            XmlElement xmlResBrdInfo = xmlDoc.CreateElement("ResBrdInfo");
            xmlEBMStateResponse.AppendChild(xmlResBrdInfo);
            {
                //播出情况，可为多个，元素关系参见资源信息数据上报
                XmlElement xmlResBrdItem = xmlDoc.CreateElement("ResBrdItem");
                xmlResBrdInfo.AppendChild(xmlResBrdItem);
                {
                    //反馈时间
                    XmlElement xmlARptTime = xmlDoc.CreateElement("RptTime");
                    xmlARptTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    xmlResBrdItem.AppendChild(xmlARptTime);

                    XmlElement xmlEBEST = xmlDoc.CreateElement("EBEST");
                    xmlEBEST.InnerText = "";
                    xmlResBrdItem.AppendChild(xmlEBEST);
                    {
                        XmlElement xmlEBESTEBEID = xmlDoc.CreateElement("EBEID");
                        xmlEBESTEBEID.InnerText = "";
                        xmlEBEST.AppendChild(xmlEBESTEBEID);
                    }

                    XmlElement xmlEBEAS = xmlDoc.CreateElement("EBEAS");
                    xmlEBEAS.InnerText = "";
                    xmlResBrdItem.AppendChild(xmlEBEAS);
                    {
                        XmlElement xmlEBEASEBEID = xmlDoc.CreateElement("EBEID");
                        xmlEBEASEBEID.InnerText = "";
                        xmlEBEAS.AppendChild(xmlEBEASEBEID);
                    }

                    XmlElement xmlEBEBS = xmlDoc.CreateElement("EBEBS");
                    xmlEBEAS.InnerText = "";
                    xmlResBrdItem.AppendChild(xmlEBEBS);
                    {
                        XmlElement xmlEBEBSEBEID = xmlDoc.CreateElement("EBEID");
                        xmlEBEBSEBEID.InnerText = "";
                        xmlEBEBS.AppendChild(xmlEBEBSEBEID);

                        XmlElement xmlStartTime = xmlDoc.CreateElement("StartTime");
                        xmlStartTime.InnerText = "";
                        xmlEBEBS.AppendChild(xmlStartTime);

                        XmlElement xmlEndTime = xmlDoc.CreateElement("EndTime");
                        xmlEndTime.InnerText = "";
                        xmlEBEBS.AppendChild(xmlEndTime);

                        XmlElement xmlFileURL = xmlDoc.CreateElement("FileURL");
                        xmlEBEBS.AppendChild(xmlFileURL);

                        XmlElement xmlResultCode = xmlDoc.CreateElement("ResultCode");
                        xmlResultCode.InnerText = "2";
                        xmlEBEBS.AppendChild(xmlResultCode);

                        XmlElement xmlResultDesc = xmlDoc.CreateElement("ResultDesc");
                        xmlEBEBS.AppendChild(xmlResultDesc);
                    }
                }
            }
            return xmlDoc;
        }
        public XmlDocument EBMStateRequestResponse(EBD ebdsr, string strebdid)
        {

            model.EDB_EBRBS.EBD EDB = new EDB_EBRBS.EBD();



            //加入XML的声明段落,Save方法不再xml上写出独立属性
            EDB.EBDVersion = "1.0";
            EDB.EBDID = strebdid;
            EDB.EBDType = "EBMStateResponse";
            EDB.SRC = new model.SRC();
            EDB.SRC.EBRID = sHBRONO;
            EDB.DEST = new model.DEST();
            EDB.DEST.EBRID = serverini.ReadValue("FORM", "Superior");
            EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
          
            EDB.SRC.URL = serverini.ReadValue("PLATFORMINFO", "URL");

            EDB.EBMStateResponse = new model.EBMStateResponse();
            EDB.EBMStateResponse.RptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            EDB.EBMStateResponse.EBM = new model.EBM();

            if (ebdsr.EBMStateRequest != null)
                EDB.EBMStateResponse.EBM.EBMID = ebdsr.EBMStateRequest.EBM.EBMID;//从100000000000开始编号
            else
                EDB.EBMStateResponse.EBM.EBMID = ebdsr.EBM.EBMID;
     //       EDB.EBMStateResponse.BrdStateCode = BrdStateCode;
      //      EDB.EBMStateResponse.BrdStateDesc = BrdStateDesc;
            //EDB.EBMStateResponse.Coverage = new model.Coverage();

            //EDB.EBMStateResponse.Coverage.CoverageRate = "0.99";
            //if (ebdsr.EBM != null)
            //    if (ebdsr.EBM.MsgContent != null)
            //    {
            //        EDB.EBMStateResponse.Coverage.AreaCode  = ebdsr.EBM.MsgContent.AreaCode;//"003609810101AA"
            //    }
            //EDB.EBMStateResponse.Coverage.ResBrdStat = "5,10,10,1000";
            EDB.EBMStateResponse.ResBrdInfo = new ResBrdInfo();
            EDB.EBMStateResponse.ResBrdInfo.ResBrdItem = new List<ResBrdItems>();

            ResBrdItems ResBrdItem = new ResBrdItems();

            ResBrdItem.EBRBS = new EBRBS();
            ResBrdItem.EBRBS.RptTime = DateTime.Now.AddHours(1).ToString();
            ResBrdItem.EBRBS.BrdSysInfo = serverini.ReadValue("PLATFORMINFO", "EBRName");
            ResBrdItem.EBRBS.StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ResBrdItem.EBRBS.FileURL = "http://10.131.58.121/ftp/" + "XXX" + ".mp3";
            ResBrdItem.EBRBS.EndTime = DateTime.Now.AddHours(1).ToString();
            //ResBrdItem.EBRBS.BrdStateCode = BrdStateCode;
            //ResBrdItem.EBRBS.BrdStateDesc = BrdStateDesc;
            EDB.EBMStateResponse.ResBrdInfo.ResBrdItem.Add(ResBrdItem);
            string strXML = XmlSerialize<model.EDB_EBRBS.EBD>(EDB).Replace("<ResBrdItem>", "").Replace("</ResBrdItem>", "").Replace("ResBrdItems", "ResBrdItem");
            //string strXML = XmlSerialize<model.EDB_EBRBS.EBD>(EDB).Replace("<EBRDT>", "").Replace("</EBRDT>", "").Replace("Information", "EBRDT");
            XmlDocument document = new XmlDocument();
            document.LoadXml(strXML);

            // XmlDocument xmlDoc = new XmlDocument();
            // #region 标准头部
            // //加入XML的声明段落,Save方法不再xml上写出独立属性
            // xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            // //加入根元素
            // XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            // xmlDoc.AppendChild(xmlElem);
            // XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            // xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            // xmlElem.Attributes.Append(xmlns);

            // //Version
            // XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            // xmlProtocolVer.InnerText = "1.0";
            // xmlElem.AppendChild(xmlProtocolVer);
            // //EBDID
            // XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            // xmlEBDID.InnerText = strebdid;
            // xmlElem.AppendChild(xmlEBDID);

            // //EBDType
            // XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            // xmlEBDType.InnerText = "EBMStateResponse";
            // xmlElem.AppendChild(xmlEBDType);

            // //Source
            // XmlElement xmlSRC = xmlDoc.CreateElement("SRC");

            // xmlElem.AppendChild(xmlSRC);

            // XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            // xmlSRCAreaCode.InnerText = sHBRONO;//ebdsr.SRC.EBEID;
            // xmlSRC.AppendChild(xmlSRCAreaCode);


            // //EBDTime
            // XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");
            // xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // xmlElem.AppendChild(xmlEBDTime);
            // #endregion End

            // #region EBMStateResponse
            // XmlElement xmlEBMStateResponse = xmlDoc.CreateElement("EBMStateResponse");
            // xmlElem.AppendChild(xmlEBMStateResponse);
            // //数据操作生成时间
            // XmlElement xmlRptTime = xmlDoc.CreateElement("RptTime");
            // xmlRptTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // xmlEBMStateResponse.AppendChild(xmlRptTime);
            // XmlElement xmlEBM = xmlDoc.CreateElement("EBM");
            // xmlEBMStateResponse.AppendChild(xmlEBM);

            // XmlElement xmlEBMID = xmlDoc.CreateElement("EBMID");
            // if (ebdsr.EBMStateRequest != null)
            //     xmlEBMID.InnerText = ebdsr.EBMStateRequest.EBM.EBMID;//从100000000000开始编号
            // else
            //     xmlEBMID.InnerText = ebdsr.EBM.EBMID;
            // xmlEBM.AppendChild(xmlEBMID);

            // //XmlElement xmlRPTTime = xmlDoc.CreateElement("RptTime");
            // //xmlRPTTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // //xmlEBMStateResponse.AppendChild(xmlRPTTime);

            // XmlElement xmlBRDState = xmlDoc.CreateElement("BrdStateCode");
            // xmlBRDState.InnerText = "2";
            // xmlEBMStateResponse.AppendChild(xmlBRDState);

            // XmlElement BrdStateDesc = xmlDoc.CreateElement("BrdStateDesc");
            // BrdStateDesc.InnerText = "播发完成";
            // xmlEBMStateResponse.AppendChild(BrdStateDesc);

            // #region Coverage

            // // if (lEBMState.Count > 0)
            // {
            //     XmlElement xmlCoverage = xmlDoc.CreateElement("Coverage");
            //     xmlEBMStateResponse.AppendChild(xmlCoverage);

            //     XmlElement xmlCoveragePercent = xmlDoc.CreateElement("CoverageRate");
            //     xmlCoveragePercent.InnerText = "0.99";
            //     xmlCoverage.AppendChild(xmlCoveragePercent);

            //     // string[] AreaValue = lEBMState[0].BRDCoverageArea.Split('|');
            //     XmlElement xmlAreaCode = xmlDoc.CreateElement("AreaCode");
            //     if (ebdsr.EBM != null)
            //         if (ebdsr.EBM.MsgContent != null)
            //         {
            //             xmlAreaCode.InnerText = ebdsr.EBM.MsgContent.AreaCode;//"003609810101AA"
            //         }
            //     xmlCoverage.AppendChild(xmlAreaCode);
            //     XmlElement xmlResBrdStat = xmlDoc.CreateElement("ResBrdStat");
            //     xmlCoveragePercent.InnerText = "5,10,10,1000";
            //     xmlCoverage.AppendChild(xmlCoveragePercent);
            // }
            // #endregion End

            // #region ResBrdInfo

            // XmlElement ResBrdInfo = xmlDoc.CreateElement("ResBrdInfo");

            // xmlEBMStateResponse.AppendChild(ResBrdInfo);
            // XmlElement EBRPS = xmlDoc.CreateElement("EBRPS");
            // EBRPS.InnerText = sHBRONO;
            //ResBrdInfo.AppendChild(EBRPS);
            // #endregion ResBrdInfo
            // #endregion End

            return document;
        }
        public XmlDocument AdapterStateRequestResponse(string strebdid, string BrdStateDesc, string BrdStateCode)
        {

            model.EDB_EBRBS.EBD EDB = new EDB_EBRBS.EBD();



            //加入XML的声明段落,Save方法不再xml上写出独立属性
            EDB.EBDVersion = "1.0";
            EDB.EBDID = strebdid;
            EDB.EBDType = "EBRASState";
            EDB.SRC = new model.SRC();
            //2018-06-13
            EDB.SRC.EBRID =sHBRONO ;
            // EDB.SRC.EBRID = sHBRONO;
            //EDB.DEST = new model.DEST();
            //EDB.DEST.EBRID = serverini.ReadValue("FORM", "SuperiorPlatform");
            EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //EDB.RelatedEBD = new model.RelatedEBD();//合版本
            //EDB.RelatedEBD.EBDID = ebdsr.EBDID;//合版本
            //      EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
      
            EDB.SRC.URL = serverini.ReadValue("PLATFORMINFO", "URL");
            EDB.EBRASState = new EBRASState();
            EDB.EBRASState.EBRAS = new EDB_EBRBS.EBRAS();
            EDB.EBRASState.EBRAS.RptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //   EDB.EBRASState.EBRAS.EBRID = serverini.ReadValue("INFOSET", "ADAPTERNO");

            EDB.EBRASState.EBRAS.StateCode = BrdStateCode;
            EDB.EBRASState.EBRAS.StateDesc = BrdStateDesc;
            EDB.EBRASState.EBRAS.EBRID = serverini.ReadValue("INFOSET", "ADAPTERNO");
            //EDB.EBMStateResponse = new model.EBMStateResponse();
            //EDB.EBMStateResponse.RptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //EDB.EBMStateResponse.EBM = new model.EBM();

            //if (ebdsr.EBMStateRequest != null)
            //    EDB.EBMStateResponse.EBM.EBMID = ebdsr.EBMStateRequest.EBM.EBMID;//从100000000000开始编号
            //else
            //    EDB.EBMStateResponse.EBM.EBMID = ebdsr.EBM.EBMID;
            //EDB.EBMStateResponse.BrdStateCode = BrdStateCode;
            //EDB.EBMStateResponse.BrdStateDesc = BrdStateDesc;
            ////EDB.EBMStateResponse.Coverage = new model.Coverage();

            ////EDB.EBMStateResponse.Coverage.CoverageRate = "0.99";
            ////if (ebdsr.EBM != null)
            ////    if (ebdsr.EBM.MsgContent != null)
            ////    {
            ////        EDB.EBMStateResponse.Coverage.AreaCode  = ebdsr.EBM.MsgContent.AreaCode;//"003609810101AA"
            ////    }
            ////EDB.EBMStateResponse.Coverage.ResBrdStat = "5,10,10,1000";
            //EDB.EBMStateResponse.ResBrdInfo = new ResBrdInfo();
            //EDB.EBMStateResponse.ResBrdInfo.ResBrdItem = new List<ResBrdItems>();

            //ResBrdItems ResBrdItem = new ResBrdItems();

            //ResBrdItem.EBRBS = new EBRBS();

            //ResBrdItem.EBRBS.RptTime = DateTime.Now.AddHours(1).ToString();
            //ResBrdItem.EBRBS.BrdSysInfo = serverini.ReadValue("PLATFORMINFO", "EBRName");
            ////  ResBrdItem.EBRBS.StartTime = DateTime.Now.ToString();
            //ResBrdItem.EBRBS.StartTime = ebdsr.EBM.MsgBasicInfo.StartTime;//合版本
            //                                                              //  ResBrdItem.EBRBS.FileURL = "http://10.131.58.121/ftp/"+"XXX"+".mp3";
            //ResBrdItem.EBRBS.FileURL = "http://10.131.58.121/ftp/" + ebdsr.EBM.MsgContent.Auxiliary.AuxiliaryDesc;//合版本 
            //                                                                                                      //  ResBrdItem.EBRBS.EndTime = DateTime.Now.AddHours(1).ToString();
            //ResBrdItem.EBRBS.EndTime = ebdsr.EBM.MsgBasicInfo.EndTime;//合版本
            //ResBrdItem.EBRBS.BrdStateCode = BrdStateCode;
            //ResBrdItem.EBRBS.BrdStateDesc = BrdStateDesc;
            //EDB.EBMStateResponse.ResBrdInfo.ResBrdItem.Add(ResBrdItem);
            string strXML = XmlSerialize<model.EDB_EBRBS.EBD>(EDB);
            //string strXML = XmlSerialize<model.EDB_EBRBS.EBD>(EDB).Replace("<EBRDT>", "").Replace("</EBRDT>", "").Replace("Information", "EBRDT");
            XmlDocument document = new XmlDocument();
            document.LoadXml(strXML);

            // XmlDocument xmlDoc = new XmlDocument();
            // #region 标准头部
            // //加入XML的声明段落,Save方法不再xml上写出独立属性
            // xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            // //加入根元素
            // XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            // xmlDoc.AppendChild(xmlElem);
            // XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            // xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            // xmlElem.Attributes.Append(xmlns);

            // //Version
            // XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            // xmlProtocolVer.InnerText = "1.0";
            // xmlElem.AppendChild(xmlProtocolVer);
            // //EBDID
            // XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            // xmlEBDID.InnerText = strebdid;
            // xmlElem.AppendChild(xmlEBDID);

            // //EBDType
            // XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            // xmlEBDType.InnerText = "EBMStateResponse";
            // xmlElem.AppendChild(xmlEBDType);

            // //Source
            // XmlElement xmlSRC = xmlDoc.CreateElement("SRC");

            // xmlElem.AppendChild(xmlSRC);

            // XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            // xmlSRCAreaCode.InnerText = sHBRONO;//ebdsr.SRC.EBEID;
            // xmlSRC.AppendChild(xmlSRCAreaCode);


            // //EBDTime
            // XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");
            // xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // xmlElem.AppendChild(xmlEBDTime);
            // #endregion End

            // #region EBMStateResponse
            // XmlElement xmlEBMStateResponse = xmlDoc.CreateElement("EBMStateResponse");
            // xmlElem.AppendChild(xmlEBMStateResponse);
            // //数据操作生成时间
            // XmlElement xmlRptTime = xmlDoc.CreateElement("RptTime");
            // xmlRptTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // xmlEBMStateResponse.AppendChild(xmlRptTime);
            // XmlElement xmlEBM = xmlDoc.CreateElement("EBM");
            // xmlEBMStateResponse.AppendChild(xmlEBM);

            // XmlElement xmlEBMID = xmlDoc.CreateElement("EBMID");
            // if (ebdsr.EBMStateRequest != null)
            //     xmlEBMID.InnerText = ebdsr.EBMStateRequest.EBM.EBMID;//从100000000000开始编号
            // else
            //     xmlEBMID.InnerText = ebdsr.EBM.EBMID;
            // xmlEBM.AppendChild(xmlEBMID);

            // //XmlElement xmlRPTTime = xmlDoc.CreateElement("RptTime");
            // //xmlRPTTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // //xmlEBMStateResponse.AppendChild(xmlRPTTime);

            // XmlElement xmlBRDState = xmlDoc.CreateElement("BrdStateCode");
            // xmlBRDState.InnerText = "2";
            // xmlEBMStateResponse.AppendChild(xmlBRDState);

            // XmlElement BrdStateDesc = xmlDoc.CreateElement("BrdStateDesc");
            // BrdStateDesc.InnerText = "播发完成";
            // xmlEBMStateResponse.AppendChild(BrdStateDesc);

            // #region Coverage

            // // if (lEBMState.Count > 0)
            // {
            //     XmlElement xmlCoverage = xmlDoc.CreateElement("Coverage");
            //     xmlEBMStateResponse.AppendChild(xmlCoverage);

            //     XmlElement xmlCoveragePercent = xmlDoc.CreateElement("CoverageRate");
            //     xmlCoveragePercent.InnerText = "0.99";
            //     xmlCoverage.AppendChild(xmlCoveragePercent);

            //     // string[] AreaValue = lEBMState[0].BRDCoverageArea.Split('|');
            //     XmlElement xmlAreaCode = xmlDoc.CreateElement("AreaCode");
            //     if (ebdsr.EBM != null)
            //         if (ebdsr.EBM.MsgContent != null)
            //         {
            //             xmlAreaCode.InnerText = ebdsr.EBM.MsgContent.AreaCode;//"003609810101AA"
            //         }
            //     xmlCoverage.AppendChild(xmlAreaCode);
            //     XmlElement xmlResBrdStat = xmlDoc.CreateElement("ResBrdStat");
            //     xmlCoveragePercent.InnerText = "5,10,10,1000";
            //     xmlCoverage.AppendChild(xmlCoveragePercent);
            // }
            // #endregion End

            // #region ResBrdInfo

            // XmlElement ResBrdInfo = xmlDoc.CreateElement("ResBrdInfo");

            // xmlEBMStateResponse.AppendChild(ResBrdInfo);
            // XmlElement EBRPS = xmlDoc.CreateElement("EBRPS");
            // EBRPS.InnerText = sHBRONO;
            //ResBrdInfo.AppendChild(EBRPS);
            // #endregion ResBrdInfo
            // #endregion End

            return document;
        }
        public XmlDocument AdapterStateRequestResponse(EBD ebdsr, string strebdid, string BrdStateDesc, string BrdStateCode)
        {

            model.EDB_EBRBS.EBD EDB = new EDB_EBRBS.EBD();



            //加入XML的声明段落,Save方法不再xml上写出独立属性
            EDB.EBDVersion = "1.0";
            EDB.EBDID = strebdid;
            EDB.EBDType = "EBRASState";
            EDB.SRC = new model.SRC();
            //2018-06-13
            // EDB.SRC.EBRID = serverini.ReadValue("INFOSET", "ADAPTERNO");
            EDB.SRC.EBRID = sHBRONO;
            //EDB.DEST = new model.DEST();
            //EDB.DEST.EBRID = serverini.ReadValue("FORM", "SuperiorPlatform");
            EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //EDB.RelatedEBD = new model.RelatedEBD();//合版本
            //EDB.RelatedEBD.EBDID = ebdsr.EBDID;//合版本
            //      EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
          
            EDB.SRC.URL = serverini.ReadValue("PLATFORMINFO", "URL");
            EDB.EBRASState = new EBRASState();
            EDB.EBRASState.EBRAS = new EDB_EBRBS.EBRAS();
            EDB.EBRASState.EBRAS.RptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //   EDB.EBRASState.EBRAS.EBRID = serverini.ReadValue("INFOSET", "ADAPTERNO");

            EDB.EBRASState.EBRAS.StateCode = BrdStateCode;
            EDB.EBRASState.EBRAS.StateDesc = BrdStateDesc;
            EDB.EBRASState.EBRAS.EBRID = serverini.ReadValue("INFOSET", "ADAPTERNO");
            //EDB.EBMStateResponse = new model.EBMStateResponse();
            //EDB.EBMStateResponse.RptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //EDB.EBMStateResponse.EBM = new model.EBM();

            //if (ebdsr.EBMStateRequest != null)
            //    EDB.EBMStateResponse.EBM.EBMID = ebdsr.EBMStateRequest.EBM.EBMID;//从100000000000开始编号
            //else
            //    EDB.EBMStateResponse.EBM.EBMID = ebdsr.EBM.EBMID;
            //EDB.EBMStateResponse.BrdStateCode = BrdStateCode;
            //EDB.EBMStateResponse.BrdStateDesc = BrdStateDesc;
            ////EDB.EBMStateResponse.Coverage = new model.Coverage();

            ////EDB.EBMStateResponse.Coverage.CoverageRate = "0.99";
            ////if (ebdsr.EBM != null)
            ////    if (ebdsr.EBM.MsgContent != null)
            ////    {
            ////        EDB.EBMStateResponse.Coverage.AreaCode  = ebdsr.EBM.MsgContent.AreaCode;//"003609810101AA"
            ////    }
            ////EDB.EBMStateResponse.Coverage.ResBrdStat = "5,10,10,1000";
            //EDB.EBMStateResponse.ResBrdInfo = new ResBrdInfo();
            //EDB.EBMStateResponse.ResBrdInfo.ResBrdItem = new List<ResBrdItems>();

            //ResBrdItems ResBrdItem = new ResBrdItems();

            //ResBrdItem.EBRBS = new EBRBS();

            //ResBrdItem.EBRBS.RptTime = DateTime.Now.AddHours(1).ToString();
            //ResBrdItem.EBRBS.BrdSysInfo = serverini.ReadValue("PLATFORMINFO", "EBRName");
            ////  ResBrdItem.EBRBS.StartTime = DateTime.Now.ToString();
            //ResBrdItem.EBRBS.StartTime = ebdsr.EBM.MsgBasicInfo.StartTime;//合版本
            //                                                              //  ResBrdItem.EBRBS.FileURL = "http://10.131.58.121/ftp/"+"XXX"+".mp3";
            //ResBrdItem.EBRBS.FileURL = "http://10.131.58.121/ftp/" + ebdsr.EBM.MsgContent.Auxiliary.AuxiliaryDesc;//合版本 
            //                                                                                                      //  ResBrdItem.EBRBS.EndTime = DateTime.Now.AddHours(1).ToString();
            //ResBrdItem.EBRBS.EndTime = ebdsr.EBM.MsgBasicInfo.EndTime;//合版本
            //ResBrdItem.EBRBS.BrdStateCode = BrdStateCode;
            //ResBrdItem.EBRBS.BrdStateDesc = BrdStateDesc;
            //EDB.EBMStateResponse.ResBrdInfo.ResBrdItem.Add(ResBrdItem);
            string strXML = XmlSerialize<model.EDB_EBRBS.EBD>(EDB);
            //string strXML = XmlSerialize<model.EDB_EBRBS.EBD>(EDB).Replace("<EBRDT>", "").Replace("</EBRDT>", "").Replace("Information", "EBRDT");
            XmlDocument document = new XmlDocument();
            document.LoadXml(strXML);

            // XmlDocument xmlDoc = new XmlDocument();
            // #region 标准头部
            // //加入XML的声明段落,Save方法不再xml上写出独立属性
            // xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            // //加入根元素
            // XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            // xmlDoc.AppendChild(xmlElem);
            // XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            // xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            // xmlElem.Attributes.Append(xmlns);

            // //Version
            // XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            // xmlProtocolVer.InnerText = "1.0";
            // xmlElem.AppendChild(xmlProtocolVer);
            // //EBDID
            // XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            // xmlEBDID.InnerText = strebdid;
            // xmlElem.AppendChild(xmlEBDID);

            // //EBDType
            // XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            // xmlEBDType.InnerText = "EBMStateResponse";
            // xmlElem.AppendChild(xmlEBDType);

            // //Source
            // XmlElement xmlSRC = xmlDoc.CreateElement("SRC");

            // xmlElem.AppendChild(xmlSRC);

            // XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            // xmlSRCAreaCode.InnerText = sHBRONO;//ebdsr.SRC.EBEID;
            // xmlSRC.AppendChild(xmlSRCAreaCode);


            // //EBDTime
            // XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");
            // xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // xmlElem.AppendChild(xmlEBDTime);
            // #endregion End

            // #region EBMStateResponse
            // XmlElement xmlEBMStateResponse = xmlDoc.CreateElement("EBMStateResponse");
            // xmlElem.AppendChild(xmlEBMStateResponse);
            // //数据操作生成时间
            // XmlElement xmlRptTime = xmlDoc.CreateElement("RptTime");
            // xmlRptTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // xmlEBMStateResponse.AppendChild(xmlRptTime);
            // XmlElement xmlEBM = xmlDoc.CreateElement("EBM");
            // xmlEBMStateResponse.AppendChild(xmlEBM);

            // XmlElement xmlEBMID = xmlDoc.CreateElement("EBMID");
            // if (ebdsr.EBMStateRequest != null)
            //     xmlEBMID.InnerText = ebdsr.EBMStateRequest.EBM.EBMID;//从100000000000开始编号
            // else
            //     xmlEBMID.InnerText = ebdsr.EBM.EBMID;
            // xmlEBM.AppendChild(xmlEBMID);

            // //XmlElement xmlRPTTime = xmlDoc.CreateElement("RptTime");
            // //xmlRPTTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // //xmlEBMStateResponse.AppendChild(xmlRPTTime);

            // XmlElement xmlBRDState = xmlDoc.CreateElement("BrdStateCode");
            // xmlBRDState.InnerText = "2";
            // xmlEBMStateResponse.AppendChild(xmlBRDState);

            // XmlElement BrdStateDesc = xmlDoc.CreateElement("BrdStateDesc");
            // BrdStateDesc.InnerText = "播发完成";
            // xmlEBMStateResponse.AppendChild(BrdStateDesc);

            // #region Coverage

            // // if (lEBMState.Count > 0)
            // {
            //     XmlElement xmlCoverage = xmlDoc.CreateElement("Coverage");
            //     xmlEBMStateResponse.AppendChild(xmlCoverage);

            //     XmlElement xmlCoveragePercent = xmlDoc.CreateElement("CoverageRate");
            //     xmlCoveragePercent.InnerText = "0.99";
            //     xmlCoverage.AppendChild(xmlCoveragePercent);

            //     // string[] AreaValue = lEBMState[0].BRDCoverageArea.Split('|');
            //     XmlElement xmlAreaCode = xmlDoc.CreateElement("AreaCode");
            //     if (ebdsr.EBM != null)
            //         if (ebdsr.EBM.MsgContent != null)
            //         {
            //             xmlAreaCode.InnerText = ebdsr.EBM.MsgContent.AreaCode;//"003609810101AA"
            //         }
            //     xmlCoverage.AppendChild(xmlAreaCode);
            //     XmlElement xmlResBrdStat = xmlDoc.CreateElement("ResBrdStat");
            //     xmlCoveragePercent.InnerText = "5,10,10,1000";
            //     xmlCoverage.AppendChild(xmlCoveragePercent);
            // }
            // #endregion End

            // #region ResBrdInfo

            // XmlElement ResBrdInfo = xmlDoc.CreateElement("ResBrdInfo");

            // xmlEBMStateResponse.AppendChild(ResBrdInfo);
            // XmlElement EBRPS = xmlDoc.CreateElement("EBRPS");
            // EBRPS.InnerText = sHBRONO;
            //ResBrdInfo.AppendChild(EBRPS);
            // #endregion ResBrdInfo
            // #endregion End

            return document;
        }
        public XmlDocument EBMStateRequestResponse(EBD ebdsr, string strebdid, string BrdStateDesc, string BrdStateCode)
        {

            model.EDB_EBRBS.EBD EDB = new EDB_EBRBS.EBD();



            //加入XML的声明段落,Save方法不再xml上写出独立属性
            EDB.EBDVersion = "1.0";
            EDB.EBDID = strebdid;
            EDB.EBDType = "EBMStateResponse";
            EDB.SRC = new model.SRC();
            EDB.SRC.EBRID = serverini.ReadValue("INFOSET", "HBRONO");
            EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            EDB.EBMStateResponse = new model.EBMStateResponse();
            EDB.EBMStateResponse.RptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            EDB.EBMStateResponse.EBM = new model.EBM();
            //EDB.RelatedEBD = new model.RelatedEBD();
            //EDB.RelatedEBD.EBDID = sHBRONO; ;
            if (ebdsr.EBMStateRequest != null)
                EDB.EBMStateResponse.EBM.EBMID = ebdsr.EBMStateRequest.EBM.EBMID;//从100000000000开始编号
            else
                EDB.EBMStateResponse.EBM.EBMID = ebdsr.EBM.EBMID;
            EDB.EBMStateResponse.BrdStateCode = BrdStateCode;
            EDB.EBMStateResponse.BrdStateDesc = BrdStateDesc;
            EDB.EBMStateResponse.Coverage = new model.Coverage();

            EDB.EBMStateResponse.Coverage.CoverageRate = "1";
         //需要修
            EDB.EBMStateResponse.Coverage.AreaCode = "341523000000";//"003609810101AA"
           
            EDB.EBMStateResponse.Coverage.ResBrdStat = "1,1,1,1";
            string strXML = XmlSerialize<model.EDB_EBRBS.EBD>(EDB);
            XmlDocument document = new XmlDocument();
            document.LoadXml(strXML);
            return document;
        }

        public XmlDocument PassiveEBMStateRequestResponse(EBD ebdsr, string strebdid, string BrdStateDesc, string BrdStateCode)
        {

            model.EDB_EBRBS.EBD EDB = new EDB_EBRBS.EBD();
            //加入XML的声明段落,Save方法不再xml上写出独立属性
            EDB.EBDVersion = "1.0";
            EDB.EBDID = strebdid;
            EDB.EBDType = "EBMStateResponse";
            EDB.SRC = new model.SRC();
            EDB.SRC.EBRID = serverini.ReadValue("FORM", "Superior");

            EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            EDB.EBMStateResponse = new model.EBMStateResponse();
            EDB.EBMStateResponse.RptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            EDB.EBMStateResponse.EBM = new model.EBM();
            EDB.RelatedEBD = new model.RelatedEBD();
            EDB.RelatedEBD.EBDID = ebdsr.EBDID;
            if (ebdsr.EBMStateRequest != null)
                EDB.EBMStateResponse.EBM.EBMID = ebdsr.EBMStateRequest.EBM.EBMID;//从100000000000开始编号
            else
                EDB.EBMStateResponse.EBM.EBMID = ebdsr.EBM.EBMID;
            EDB.EBMStateResponse.BrdStateCode = BrdStateCode;
            EDB.EBMStateResponse.BrdStateDesc = BrdStateDesc;
            EDB.EBMStateResponse.Coverage = new model.Coverage();

            EDB.EBMStateResponse.Coverage.CoverageRate = "1";
            //需要修
            EDB.EBMStateResponse.Coverage.AreaCode = "341523000000";//"003609810101AA"  后期需要修改  20180818

            EDB.EBMStateResponse.Coverage.ResBrdStat = "1,1,1,1";
            string strXML = XmlSerialize<model.EDB_EBRBS.EBD>(EDB);
            XmlDocument document = new XmlDocument();
            document.LoadXml(strXML);
            return document;
        }
        /// <summary>
        /// 心跳包 
        /// </summary>
        /// <returns></returns>
        public XmlDocument HeartBeatResponse()
        {
             IniFiles serverini = new IniFiles(@Application.StartupPath + "\\Config.ini");
            model.EBD EDB = new model.EBD();
     
            EDB.EBDVersion = "1.0";
            EDB.EBDID = "01" + sHBRONO + "0000000000000000";
            EDB.EBDType = "ConnectionCheck";
            EDB.SRC = new model.SRC();
            EDB.SRC.EBRID = serverini.ReadValue("INFOSET", "HBRONO");
            EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
           
            EDB.SRC.URL = serverini.ReadValue("PLATFORMINFO", "URL");

            EDB.DEST = new model.DEST();
            EDB.DEST.EBRID =serverini.ReadValue("FORM", "Superior");
            EDB.ConnectionCheck = new ConnectionCheck();
            EDB.ConnectionCheck.RptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
      

            string strXML = XmlSerialize<model.EBD>(EDB);
            XmlDocument document = new XmlDocument();
            document.LoadXml(strXML);
            //2018-06-01修改成注释
            //XmlDocument xmlDoc = new XmlDocument();
            ////加入XML的声明段落,Save方法不再xml上写出独立属性GB2312
            ////xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null));
            ////XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            ////xmlDoc.AppendChild(xmlElem);
            //xmlDoc.CreateAttribute(strXML);
            //   #region 标准头部

            //XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            //xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            //xmlElem.Attributes.Append(xmlns);
            //XmlElement xmlCentent = xmlDoc.CreateElement(strXML);
            //xmlElem.AppendChild(xmlCentent);
            // //Version
            // XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            // xmlProtocolVer.InnerText = "1.0";
            // xmlElem.AppendChild(xmlProtocolVer);
            // //EBDID
            // XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            // xmlEBDID.InnerText = "01" + sHBRONO + "0000000000000000";
            // xmlElem.AppendChild(xmlEBDID);

            // //EBDType
            // XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            // xmlEBDType.InnerText = "ConnectionCheck";
            // xmlElem.AppendChild(xmlEBDType);

            // //Source
            // XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            // xmlElem.AppendChild(xmlSRC);
            // XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            // xmlSRCAreaCode.InnerText = sHBRONO;// "010334152300000002";// ebdsr.SRC.EBEID;
            // xmlSRC.AppendChild(xmlSRCAreaCode);
            // XmlElement xmlSRCUrl = xmlDoc.CreateElement("URL");
            // // "010334152300000002";// ebdsr.SRC.EBEID;
            //string serverIP = serverini.ReadValue("INFOSET", "ServerIP");
            //string serverPort = serverini.ReadValue("INFOSET", "ServerPort");
            // //   合并
            // string mergeString = serverIP + ":" + serverPort;
            // xmlSRCUrl.InnerText = mergeString;
            // xmlSRC.AppendChild(xmlSRCUrl);

            // XmlElement xmlDEST = xmlDoc.CreateElement("DEST");
            // xmlElem.AppendChild(xmlDEST);

            // XmlElement eebtEE = xmlDoc.CreateElement("EBRID");
            // eebtEE.InnerText = sHBRONO;// "010334152300000002";// ebdsr.SRC.EBEID;   尼玛   干嘛要写死？？20180104
            // xmlDEST.AppendChild(eebtEE);

            // XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");
            // xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // xmlElem.AppendChild(xmlEBDTime);

            // XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            // xmlElem.AppendChild(xmlRelatedEBD);

            // XmlElement xmlRelatedEBDID = xmlDoc.CreateElement("EBDID");

            // xmlRelatedEBD.AppendChild(xmlRelatedEBDID);


            // #endregion End

            // XmlElement xmlEBDResponse = xmlDoc.CreateElement("ConnectionCheck");
            // xmlElem.AppendChild(xmlEBDResponse);

            // XmlElement xmlResultCode = xmlDoc.CreateElement("RptTime");
            // xmlResultCode.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // xmlEBDResponse.AppendChild(xmlResultCode);

            return document;
        }
        public  string XmlSerialize<T>(T obj)
        {
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    Type t = obj.GetType();
                    XmlSerializer serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(sw, obj);
                    sw.Close();
                    string[] array = sw.ToString().Split('\n');
                    string xmlString = "<?xml version='1.0' encoding='utf-8' standalone='yes'?>" + '\n';

                    for (int i = 1; i < array.Length; i++)
                    {
                        xmlString += array[i] + '\n';
                    }

                    return xmlString;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 实时流
        /// </summary>
        /// <returns></returns>
        public XmlDocument EBMStreamResponse(string strEBMID, string strUrl)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //加入XML的声明段落,Save方法不再xml上写出独立属性GB2312
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);

            //xmlHead(xmlDoc, xmlElem, ebdsr, EBDstyle);

            #region 标准头部

            XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            xmlElem.Attributes.Append(xmlns);

            //Version
            XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            xmlProtocolVer.InnerText = "1.0";
            xmlElem.AppendChild(xmlProtocolVer);
            //EBDID
            XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            xmlEBDID.InnerText = "01" + sHBRONO + "0000000000000000";
            xmlElem.AppendChild(xmlEBDID);

            //EBDType
            XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            xmlEBDType.InnerText = "EBMStreamPortRequest";
            xmlElem.AppendChild(xmlEBDType);

            //Source
            XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            xmlElem.AppendChild(xmlSRC);

            XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            xmlSRCAreaCode.InnerText = sHBRONO;
            xmlSRC.AppendChild(xmlSRCAreaCode);
            XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");
            xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlElem.AppendChild(xmlEBDTime);
            #endregion End

            XmlElement xmlDevice = xmlDoc.CreateElement("EBMStream");
            xmlElem.AppendChild(xmlDevice);

            XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBM");
            xmlDevice.AppendChild(xmlRelatedEBD);
            XmlElement xmlReEBDID = xmlDoc.CreateElement("EBMID");
            xmlReEBDID.InnerText = strEBMID;//与EBDID一致就用这个写
            xmlRelatedEBD.AppendChild(xmlReEBDID);

            XmlElement xmlParams = xmlDoc.CreateElement("Params");
            xmlDevice.AppendChild(xmlParams);
            XmlElement xmlUrl = xmlDoc.CreateElement("Url");
            xmlUrl.InnerText = strUrl;//与EBDID一致就用这个写
            xmlParams.AppendChild(xmlUrl);

            return xmlDoc;
        }

        /// <summary>
        /// 平台播发记录数据数据
        /// </summary>
        /// <param name="ebdsr"></param>
        /// <returns></returns>
        public XmlDocument PlatformBRDResponse(EBD ebdsr, List<PlatformBRD> lP)
        {
            XmlDocument xmlDoc = new XmlDocument();
            #region 标准头部
            //加入XML的声明段落,Save方法不再xml上写出独立属性
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            //加入根元素
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);
            XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            xmlElem.Attributes.Append(xmlns);

            //Version
            XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            xmlProtocolVer.InnerText = "1.0";
            xmlElem.AppendChild(xmlProtocolVer);
            //EBDID
            XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            xmlEBDID.InnerText = ebdsr.EBDID;//
            xmlElem.AppendChild(xmlEBDID);

            //EBDType
            XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            xmlEBDType.InnerText = "EBDResponse";
            xmlElem.AppendChild(xmlEBDType);

            //Source
            XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            xmlElem.AppendChild(xmlSRC);

            XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            xmlSRCAreaCode.InnerText = ebdsr.SRC.EBRID;
            xmlSRC.AppendChild(xmlSRCAreaCode);

            XmlElement xmlDEST = xmlDoc.CreateElement("DEST");
            xmlElem.AppendChild(xmlDEST);
            XmlElement xmlDESTEBEID = xmlDoc.CreateElement("EBEID");
            //try
            //{
            //    xmlDESTEBEID.InnerText = ebdsr.DEST.EBEID;
            //}
            //catch
            //{
            //}
            xmlSRC.AppendChild(xmlDESTEBEID);
            //XmlElement xmlSourceID = xmlDoc.CreateElement("EBEID");
            //xmlSourceID.InnerText = SourceID;//
            //xmlSRC.AppendChild(xmlSourceID);

            //EBDTime
            XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");
            xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlElem.AppendChild(xmlEBDTime);
            #endregion End
            //RelatedEBD
            XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            xmlElem.AppendChild(xmlRelatedEBD);
            XmlElement xmlReEBDID = xmlDoc.CreateElement("EBDID");
            xmlReEBDID.InnerText = ebdsr.EBDID.ToString();//与EBDID一致就用这个写
            xmlRelatedEBD.AppendChild(xmlReEBDID);

            #region PlatformBRDReport

            //XmlElement xmlPlatformBRDReport = xmlDoc.CreateElement("PlatformBRDReport");
            //xmlElem.AppendChild(xmlPlatformBRDReport);

            //XmlElement xmlRPTStartTime = xmlDoc.CreateElement("RPTStartTime");//RPTStartTime
            //xmlRPTStartTime.InnerText = ebdsr.DataRequest.StartTime;// ebdsr.DataRequest.StartTime;//
            //xmlPlatformBRDReport.AppendChild(xmlRPTStartTime);
            //XmlElement xmlRPTEndTime = xmlDoc.CreateElement("RPTEndTime");//RPTEndTime
            //xmlRPTEndTime.InnerText = ebdsr.DataRequest.EndTime; //ebdsr.DataRequest.EndTime;//DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            //xmlPlatformBRDReport.AppendChild(xmlRPTEndTime);

            //#region PlatformBRD
            //if (lP.Count > 0)
            //{
            //    for (int i = 0; i < lP.Count; i++)
            //    {
            //        XmlElement xmlPlatformBRD = xmlDoc.CreateElement("PlatformBRD");//PlatformBRD
            //        xmlPlatformBRDReport.AppendChild(xmlPlatformBRD);

            //        XmlElement xmlPlatformBRDID = xmlDoc.CreateElement("PlatformBRDID");//PlatformBRDID
            //        xmlPlatformBRDID.InnerText = lP[i].PlatformBRDID;//数据库ID字段值
            //        xmlPlatformBRD.AppendChild(xmlPlatformBRDID);
            //        XmlElement xmlBRDPSourceType = xmlDoc.CreateElement("SourceType");//SourceType
            //        xmlBRDPSourceType.InnerText =lP[i].SourceType;
            //        xmlPlatformBRD.AppendChild(xmlBRDPSourceType);
            //        XmlElement xmlBRDPSourceID = xmlDoc.CreateElement("SourceID");//SourceType
            //        xmlBRDPSourceID.InnerText = lP[i].SourceID;
            //        xmlPlatformBRD.AppendChild(xmlBRDPSourceID);
            //        XmlElement xmlMsgID = xmlDoc.CreateElement("MsgID");//
            //        xmlMsgID.InnerText = lP[i].MsgID;
            //        xmlPlatformBRD.AppendChild(xmlMsgID);
            //        XmlElement xmlSender = xmlDoc.CreateElement("Sender");//
            //        xmlSender.InnerText = lP[i].Sender;//播发部门：气象局，应急办，公安局
            //        xmlPlatformBRD.AppendChild(xmlSender);
            //        XmlElement xmlUnitId = xmlDoc.CreateElement("UnitId");//
            //        xmlUnitId.InnerText = lP[i].UnitId;//播发部门ID
            //        xmlPlatformBRD.AppendChild(xmlUnitId);
            //        XmlElement xmlUnitName = xmlDoc.CreateElement("UnitName");//
            //        xmlUnitName.InnerText = lP[i].UnitName;//播发部门名称
            //        xmlPlatformBRD.AppendChild(xmlUnitName);
            //        XmlElement xmlPersonID = xmlDoc.CreateElement("PersonID");//
            //        xmlPersonID.InnerText = lP[i].PersonID;//发布人员ID
            //        xmlPlatformBRD.AppendChild(xmlPersonID);
            //        XmlElement xmlPersonName = xmlDoc.CreateElement("PersonName");//
            //        xmlPersonName.InnerText = lP[i].PersonName;//播发人员姓名
            //        xmlPlatformBRD.AppendChild(xmlPersonName);
            //        XmlElement xmlBRDStartTime = xmlDoc.CreateElement("BRDStartTime");//
            //        xmlBRDStartTime.InnerText =lP[i].BRDStartTime ;//播发起始时间 DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            //        xmlPlatformBRD.AppendChild(xmlBRDStartTime);
            //        XmlElement xmlBRDEndTime = xmlDoc.CreateElement("BRDEndTime");//
            //        xmlBRDEndTime.InnerText =lP[i].BRDEndTime ;//DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            //        xmlPlatformBRD.AppendChild(xmlBRDEndTime);
            //        XmlElement xmlAudioFileURL = xmlDoc.CreateElement("AudioFileURL");//
            //        xmlAudioFileURL.InnerText = lP[i].AudioFileURL;//
            //        xmlPlatformBRD.AppendChild(xmlAudioFileURL);
            //    }
            //}
            #endregion End

            //#endregion End
            return xmlDoc;
        }

        /// <summary>
        /// 终端播发记录数据
        /// </summary>
        /// <param name="ebdsr"></param>
        /// <returns></returns>
        public XmlDocument TermBRDResponse(EBD ebdsr, List<TermBRD> lt)
        {
            XmlDocument xmlDoc = new XmlDocument();
            #region 标准头部
            //加入XML的声明段落,Save方法不再xml上写出独立属性
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            //加入根元素
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);
            XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            xmlElem.Attributes.Append(xmlns);

            //Version
            XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            xmlProtocolVer.InnerText = "1.0";
            xmlElem.AppendChild(xmlProtocolVer);
            //EBDID
            XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            xmlEBDID.InnerText = ebdsr.EBDID;//
            xmlElem.AppendChild(xmlEBDID);

            //EBDType
            XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            xmlEBDType.InnerText = "EBDResponse";
            xmlElem.AppendChild(xmlEBDType);

            //Source
            XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            xmlElem.AppendChild(xmlSRC);

            XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBEID");
            xmlSRCAreaCode.InnerText = ebdsr.SRC.EBRID;
            xmlSRC.AppendChild(xmlSRCAreaCode);

            XmlElement xmlDEST = xmlDoc.CreateElement("DEST");
            xmlElem.AppendChild(xmlDEST);
            XmlElement xmlDESTEBEID = xmlDoc.CreateElement("EBEID");
            //try
            //{
            //    xmlDESTEBEID.InnerText = ebdsr.DEST.EBEID;
            //}
            //catch
            //{
            //}
            xmlSRC.AppendChild(xmlDESTEBEID);
            //XmlElement xmlSourceID = xmlDoc.CreateElement("EBEID");
            //xmlSourceID.InnerText = SourceID;//
            //xmlSRC.AppendChild(xmlSourceID);

            //EBDTime
            XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");
            xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlElem.AppendChild(xmlEBDTime);
            #endregion End
            //RelatedEBD
            XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            xmlElem.AppendChild(xmlRelatedEBD);
            XmlElement xmlReEBDID = xmlDoc.CreateElement("EBDID");
            xmlReEBDID.InnerText = ebdsr.EBDID.ToString();//与EBDID一致就用这个写
            xmlRelatedEBD.AppendChild(xmlReEBDID);

            #region TermBRD
            XmlElement xmlTermBRDReport = xmlDoc.CreateElement("TermBRDReport");
            xmlElem.AppendChild(xmlTermBRDReport);

            XmlElement xmlRPTStartTime = xmlDoc.CreateElement("RPTStartTime");//RPTStartTime
            xmlRPTStartTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// ebdsr.DataRequest.StartTime;//
            xmlTermBRDReport.AppendChild(xmlRPTStartTime);
            XmlElement xmlRPTEndTime = xmlDoc.CreateElement("RPTEndTime");//RPTEndTime
            xmlRPTEndTime.InnerText = DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss"); //ebdsr.DataRequest.EndTime;//
            xmlTermBRDReport.AppendChild(xmlRPTEndTime);
            #region Term
            if (lt.Count > 0)
            {
                for (int l = 0; l < lt.Count; l++)
                {
                    XmlElement xmlTerm = xmlDoc.CreateElement("TermBRD");//TermBRD
                    xmlTermBRDReport.AppendChild(xmlTerm);

                    XmlElement xmlTermBRDID = xmlDoc.CreateElement("TermBRDID");
                    xmlTermBRDID.InnerText = lt[l].TermBRDID;//
                    xmlTerm.AppendChild(xmlTermBRDID);
                    XmlElement xmlTSourceType = xmlDoc.CreateElement("SourceType");
                    xmlTSourceType.InnerText = lt[l].SourceType;//
                    xmlTerm.AppendChild(xmlTSourceType);
                    XmlElement xmlTSourceID = xmlDoc.CreateElement("SourceID");
                    xmlTSourceID.InnerText = lt[l].SourceID;//
                    xmlTerm.AppendChild(xmlTSourceID);

                    XmlElement xmlMsgID = xmlDoc.CreateElement("MsgID");//
                    xmlMsgID.InnerText = lt[l].MsgID;//广播ID
                    xmlTerm.AppendChild(xmlMsgID);
                    XmlElement xmlDeviceID = xmlDoc.CreateElement("DeviceID");//
                    xmlDeviceID.InnerText = lt[l].DeviceID;
                    xmlTerm.AppendChild(xmlDeviceID);
                    XmlElement xmlBRDTime = xmlDoc.CreateElement("BRDTime");//
                    xmlBRDTime.InnerText = lt[l].BRDTime;//
                    xmlTerm.AppendChild(xmlBRDTime);
                    XmlElement xmlResultCode = xmlDoc.CreateElement("ResultCode");//
                    xmlResultCode.InnerText = "1";//播发结果
                    xmlTerm.AppendChild(xmlResultCode);
                    XmlElement xmlResultDesc = xmlDoc.CreateElement("ResultDesc");//
                    xmlResultDesc.InnerText = "播出正常";//播发结果描述
                    xmlTerm.AppendChild(xmlResultDesc);
                }
            }
            #endregion End
            #endregion End
            return xmlDoc;
        }

        /// <summary>
        /// 设备基础数据
        /// </summary>
        /// <param name="ebdsr"></param>
        /// <returns></returns>
        public XmlDocument DeviceInfoResponse(EBD ebdsr, List<Device> lDev, string strebdid)
        {
            model.EBD EDB = new model.EBD();
            EDB.EBRDTInfo = new EBRPSInfo();


            //加入XML的声明段落,Save方法不再xml上写出独立属性
            EDB.EBDVersion = "1.0";
            EDB.EBDID = strebdid;
            EDB.EBDType = "EBRDTInfo";
            EDB.SRC = new model.SRC();
            EDB.SRC.EBRID = sHBRONO;
            EDB.RelatedEBD = new model.RelatedEBD();
            EDB.RelatedEBD.EBDID = ebdsr.EBDID;
            EDB.EBRDTInfo.Params = new model.Params();
            EDB.EBRDTInfo.Params.RptEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            EDB.EBRDTInfo.Params.RptStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            EDB.EBRDTInfo.Params.RptType = "Full";
            EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        
            EDB.SRC.URL = serverini.ReadValue("PLATFORMINFO", "URL");
            EDB.EBRDTInfo.EBRDT = new List<Information>();
            EDB.DEST = new model.DEST();
            EDB.DEST.EBRID = serverini.ReadValue("FORM", "Superior");

            string DeviEBRID = sHBRONO.Substring(4, sHBRONO.Length - 6);
            if (lDev.Count > 0)
            {
                for (int l = 0; l < lDev.Count; l++)
                {
                    Information EBRDT = new Information();
                    EBRDT.RelatedEBRPS = new RelatedEBRPS();
                    EBRDT.RelatedEBRPS.EBRID = sHBRONO;

             
            
                        EBRDT.EBRID = lDev[l].DeviceID;
          

                    EBRDT.RptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    EBRDT.RptType = "Sync";
                    if (lDev[l].Longitude.Split('.')[1].Length > 6)
                    {
                        EBRDT.Longitude = lDev[l].Longitude.Split('.')[0] + "." + lDev[l].Longitude.Split('.')[1].Substring(0, 6);
                    }
                    if (lDev[l].Latitude.Split('.')[1].Length > 6)
                    {
                        EBRDT.Latitude = lDev[l].Latitude.Split('.')[0] + "." + lDev[l].Latitude.Split('.')[1].Substring(0, 6);

                    }

                    EBRDT.EBRName = lDev[l].DeviceName;

                    //EBRPS.Latitude = serverini.ReadValue("PLATFORMINFO", "Latitude");
                    //if (EBRPS.Latitude.Split('.')[1].Length > 6)
                    //{
                    //    EBRPS.Latitude = EBRPS.Latitude.Split('.')[0] + "." + EBRPS.Latitude.Split('.')[1].Substring(0, 6);
                    //}
                    //EBRPS.URL = "http://" + mergeString;

                    EDB.EBRDTInfo.EBRDT.Add(EBRDT);

                }
            }

            string strXML = XmlSerialize<model.EBD>(EDB).Replace("<EBRDT>", "").Replace("</EBRDT>", "").Replace("Information", "EBRDT");
            XmlDocument document = new XmlDocument();
            document.LoadXml(strXML);
            //XmlDocument xmlDoc = new XmlDocument();
            //#region 标准头部
            ////加入XML的声明段落,Save方法不再xml上写出独立属性
            //xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            ////加入根元素
            //XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            //xmlDoc.AppendChild(xmlElem);
            //XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            //xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            //xmlElem.Attributes.Append(xmlns);

            ////Version
            //XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            //xmlProtocolVer.InnerText = "1";
            //xmlElem.AppendChild(xmlProtocolVer);
            ////EBDID
            //XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            //xmlEBDID.InnerText = strebdid;//
            //xmlElem.AppendChild(xmlEBDID);

            ////EBDType
            //XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            //xmlEBDType.InnerText = "EBRDTInfo";
            //xmlElem.AppendChild(xmlEBDType);

            ////Source
            //XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            //xmlElem.AppendChild(xmlSRC);

            //XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            //xmlSRCAreaCode.InnerText = sHBRONO;
            //xmlSRC.AppendChild(xmlSRCAreaCode);


            ////XmlElement xmlDEST = xmlDoc.CreateElement("DEST");
            ////xmlElem.AppendChild(xmlDEST);

            ////XmlElement xmlSRCAreaEBRID = xmlDoc.CreateElement("EBRID");
            ////xmlSRCAreaEBRID.InnerText = "010232000000000001";
            ////xmlDEST.AppendChild(xmlSRCAreaEBRID);

            ////EBDTime
            //XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");

            //xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //xmlElem.AppendChild(xmlEBDTime);

            //XmlElement RelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            //xmlElem.AppendChild(RelatedEBD);

            //#endregion End
            ////RelatedEBD
            ////if (ebdsr != null)
            ////{
            ////    XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            ////    xmlElem.AppendChild(xmlRelatedEBD);
            ////    XmlElement xmlReEBDID = xmlDoc.CreateElement("EBDID");
            ////    xmlReEBDID.InnerText = ebdsr.EBDID;//与EBDID一致就用这个写
            ////    xmlRelatedEBD.AppendChild(xmlReEBDID);
            ////}
            //#region DeviceInfoReport
            //XmlElement xmlDeviceInfoReport = xmlDoc.CreateElement("EBRDTInfo");
            //xmlElem.AppendChild(xmlDeviceInfoReport);

            //XmlElement xmlParams = xmlDoc.CreateElement("Params");
            //xmlDeviceInfoReport.AppendChild(xmlParams);

            //XmlElement xmlRPTStartTime = xmlDoc.CreateElement("RptStartTime");//RPTStartTime
            //xmlRPTStartTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// ebdsr.DataRequest.StartTime;
            //xmlParams.AppendChild(xmlRPTStartTime);

            //XmlElement xmlRPTEndTime = xmlDoc.CreateElement("RptEndTime");//RPTEndTime
            //xmlRPTEndTime.InnerText = DateTime.Now.AddDays(5).ToString("yyyy-MM-dd HH:mm:ss"); //ebdsr.DataRequest.EndTime;
            //xmlParams.AppendChild(xmlRPTEndTime);

            //XmlElement xmlRptType = xmlDoc.CreateElement("RptType");//RPTEndTime
            //xmlRptType.InnerText = "Full"; //ebdsr.DataRequest.EndTime;
            //xmlParams.AppendChild(xmlRptType);

            //string DeviEBRID = sHBRONO.Substring(4, sHBRONO.Length - 6);
            //Console.WriteLine(DeviEBRID);

            //#region Device
            //if (lDev.Count > 0)
            //{
            //    for (int l = 1; l < lDev.Count; l++)
            //    {
            //        XmlElement xmlDevice = xmlDoc.CreateElement("EBRDT");//Term
            //        xmlDeviceInfoReport.AppendChild(xmlDevice);



            //        XmlElement xmlRptTime = xmlDoc.CreateElement("RptTime");
            //        xmlRptTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //        xmlDevice.AppendChild(xmlRptTime);

            //        XmlElement xmlRptType2 = xmlDoc.CreateElement("RptType");
            //        xmlRptType2.InnerText = "Sync";
            //        xmlDevice.AppendChild(xmlRptType2);


            //        //XmlElement xmlRelatedEEBRBS = xmlDoc.CreateElement("EBRPS");
            //        //xmlDeviceInfoReport.AppendChild(xmlRelatedEEBRBS);

            //        //XmlElement xmlEBRID = xmlDoc.CreateElement("RptTime");
            //        //xmlEBRID.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //        //xmlRelatedEEBRBS.AppendChild(xmlEBRID);

            //        XmlElement xmlRelatedEBRPS = xmlDoc.CreateElement("RelatedEBRPS");
            //        xmlDevice.AppendChild(xmlRelatedEBRPS);

            //        XmlElement xmlERelatedEBRPSD = xmlDoc.CreateElement("EBRID");
            //        xmlERelatedEBRPSD.InnerText = sHBRONO;
            //        xmlRelatedEBRPS.AppendChild(xmlERelatedEBRPSD);


            //        XmlElement xmlDeviceID = xmlDoc.CreateElement("EBRID");
            //        xmlDeviceID.InnerText = "0601" + DeviEBRID + lDev[l].DeviceID; //"0699" + "321323000000" + lDev[l].DeviceID;
            //        xmlDevice.AppendChild(xmlDeviceID);


            //        XmlElement xmlDeviceName = xmlDoc.CreateElement("EBRName");
            //        xmlDeviceName.InnerText = l + "号";
            //        xmlDevice.AppendChild(xmlDeviceName);

            //        XmlElement xmlLongitude = xmlDoc.CreateElement("Longitude");
            //        xmlLongitude.InnerText = "118.33";
            //        xmlDevice.AppendChild(xmlLongitude);

            //        XmlElement xmlLatitude = xmlDoc.CreateElement("Latitude");
            //        xmlLatitude.InnerText = "33.95";
            //        xmlDevice.AppendChild(xmlLatitude);

            //        XmlElement xmlLatitudParamse = xmlDoc.CreateElement("Params");
            //        xmlLatitudParamse.InnerText = "";
            //        xmlDevice.AppendChild(xmlLatitudParamse);




            //    }
            //}
            ////XmlElement xmlRelatedEEBRBS = xmlDoc.CreateElement("EBRPS");
            ////xmlDeviceInfoReport.AppendChild(xmlRelatedEEBRBS);

            ////XmlElement xmlEBRID = xmlDoc.CreateElement("RptTime");
            ////xmlEBRID.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ////xmlRelatedEEBRBS.AppendChild(xmlEBRID);

            ////XmlElement xmlRelatedEBRPS = xmlDoc.CreateElement("RelatedEBRPS");
            ////xmlRelatedEEBRBS.AppendChild(xmlRelatedEBRPS);

            ////XmlElement xmlERelatedEBRPSD = xmlDoc.CreateElement("EBRID");
            ////xmlERelatedEBRPSD.InnerText = sHBRONO;
            ////xmlRelatedEBRPS.AppendChild(xmlERelatedEBRPSD);
            //#endregion End
            //#endregion End
            return document;
        }

        public XmlDocument DeviceInfoResponse(List<Device> lDev, string strebdid)
        {
            model.EBD EDB = new model.EBD();
            EDB.EBRDTInfo = new EBRPSInfo();

      
            //加入XML的声明段落,Save方法不再xml上写出独立属性
            EDB.EBDVersion = "1.0";
            EDB.EBDID = strebdid;
            EDB.EBDType = "EBRDTInfo";
            EDB.SRC = new model.SRC();
            //  EDB.SRC.EBRID = lDev[0].DeviceID;

            EDB.SRC.EBRID = serverini.ReadValue("INFOSET", "HBRONO");
            //EDB.EBRDTInfo.Params = new model.Params();
            //EDB.EBRDTInfo.Params.RptEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //EDB.EBRDTInfo.Params.RptStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //EDB.EBRDTInfo.Params.RptType = "";
            EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //      EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
       
      
       
            EDB.SRC.URL = serverini.ReadValue("PLATFORMINFO", "URL");
            EDB.EBRDTInfo.EBRDT = new List<Information>();
            EDB.DEST = new model.DEST();
            EDB.DEST.EBRID = serverini.ReadValue("FORM", "Superior");

            string DeviEBRID = sHBRONO.Substring(4, sHBRONO.Length - 6);
            if (lDev.Count > 0)
            {
                for (int l = 0; l < lDev.Count; l++)
                {
                    /*---------------------2018-06-10添加终端主动上报变更信息-------------------------------*/
                    //if (string.IsNullOrEmpty(lDev[l].Old_Srv_Mft_Date))
                    //{
                    //    AddTerminalMaintenance(lDev[l]);
                    //}
                    //else if (lDev[l].Srv_Mft_Date.IndexOf(lDev[l].Old_Srv_Mft_Date) <=-1)
                    //{
                    //    UpdateTerminalMaintenance(lDev[l]);
                    //}
                    //else if (!string.IsNullOrEmpty(lDev[l].UpdateDate) && !string.IsNullOrEmpty(lDev[l].Old_UpdateDate))
                    //{
                    //    if (lDev[l].UpdateDate.IndexOf(lDev[l].Old_UpdateDate) > -1)
                    //    {

                    //        continue;
                    //    }
                    //}
                    Information EBRDT = new Information();
            

                    EBRDT.EBRID = lDev[l].DeviceID;
                
                    EBRDT.RptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
                    EBRDT.RptType = "Sync";
                    if (lDev[l].Longitude.Split('.')[1].Length > 6)
                    {
                        EBRDT.Longitude = lDev[l].Longitude.Split('.')[0] + "." + lDev[l].Longitude.Split('.')[1].Substring(0, 6);
                    }
                    if (lDev[l].Latitude.Split('.')[1].Length > 6)
                    {
                        EBRDT.Latitude = lDev[l].Latitude.Split('.')[0] + "." + lDev[l].Latitude.Split('.')[1].Substring(0, 6);

                    }

                    EBRDT.EBRName = lDev[l].DeviceName;

                    //EBRPS.Latitude = serverini.ReadValue("PLATFORMINFO", "Latitude");
                    //if (EBRPS.Latitude.Split('.')[1].Length > 6)
                    //{
                    //    EBRPS.Latitude = EBRPS.Latitude.Split('.')[0] + "." + EBRPS.Latitude.Split('.')[1].Substring(0, 6);
                    //}
                    //EBRPS.URL = "http://" + mergeString;

                    EDB.EBRDTInfo.EBRDT.Add(EBRDT);

                }
            }

            string strXML = XmlSerialize<model.EBD>(EDB).Replace("<EBRDT>","").Replace("</EBRDT>","").Replace("Information", "EBRDT");
            XmlDocument document = new XmlDocument();
            document.LoadXml(strXML);
            //XmlDocument xmlDoc = new XmlDocument();
            //#region 标准头部
            ////加入XML的声明段落,Save方法不再xml上写出独立属性
            //xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            ////加入根元素
            //XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            //xmlDoc.AppendChild(xmlElem);
            //XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            //xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            //xmlElem.Attributes.Append(xmlns);

            ////Version
            //XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            //xmlProtocolVer.InnerText = "1.0";
            //xmlElem.AppendChild(xmlProtocolVer);

            ////EBDID
            //XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            //xmlEBDID.InnerText = strebdid;//
            //xmlElem.AppendChild(xmlEBDID);

            ////EBDType
            //XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            //xmlEBDType.InnerText = "EBRDTInfo";
            //xmlElem.AppendChild(xmlEBDType);

            ////Source
            //XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            //xmlElem.AppendChild(xmlSRC);

            //XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            //xmlSRCAreaCode.InnerText = sHBRONO;
            //xmlSRC.AppendChild(xmlSRCAreaCode);

            ////EBDTime
            //XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");

            //xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //xmlElem.AppendChild(xmlEBDTime);
            //#endregion End
            ////RelatedEBD
            ////XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            ////xmlElem.AppendChild(xmlRelatedEBD);
            ////XmlElement xmlReEBDID = xmlDoc.CreateElement("EBDID");
            ////xmlReEBDID.InnerText = strebdid;//与EBDID一致就用这个写
            ////xmlRelatedEBD.AppendChild(xmlReEBDID);
            //#region DeviceInfoReport
            //XmlElement xmlDeviceInfoReport = xmlDoc.CreateElement("EBRDTInfo");
            //xmlElem.AppendChild(xmlDeviceInfoReport);

            //XmlElement xmlParams = xmlDoc.CreateElement("Params");
            //xmlElem.AppendChild(xmlParams);

            //XmlElement xmlRPTStartTime = xmlDoc.CreateElement("RPTStartTime");//RPTStartTime
            //xmlRPTStartTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// ebdsr.DataRequest.StartTime;
            //xmlParams.AppendChild(xmlRPTStartTime);

            //XmlElement xmlRPTEndTime = xmlDoc.CreateElement("RPTEndTime");//RPTEndTime
            //xmlRPTEndTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //ebdsr.DataRequest.EndTime;
            //xmlParams.AppendChild(xmlRPTEndTime);

            //XmlElement xmlRptType = xmlDoc.CreateElement("RptType");//RPTEndTime
            //xmlRptType.InnerText = ""; //ebdsr.DataRequest.EndTime;
            //xmlParams.AppendChild(xmlRptType);

            //string DeviEBRID = sHBRONO.Substring(4, sHBRONO.Length - 6);
            //Console.WriteLine(DeviEBRID);

            //#region Device
            //if (lDev.Count > 0)
            //{
            //    for (int l = 0; l < lDev.Count; l++)
            //    {
            //        XmlElement xmlDevice = xmlDoc.CreateElement("EBRDT");//Term
            //        xmlDeviceInfoReport.AppendChild(xmlDevice);

            //        XmlElement xmlRptTime = xmlDoc.CreateElement("RptTime");
            //        xmlRptTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //        xmlDevice.AppendChild(xmlRptTime);

            //        XmlElement xmlRptType2 = xmlDoc.CreateElement("RptType");
            //        xmlRptType2.InnerText = "Sync";
            //        xmlDevice.AppendChild(xmlRptType2);

            //        XmlElement xmlRelatedEBRPS = xmlDoc.CreateElement("RelatedEBRPS");
            //        xmlDevice.AppendChild(xmlRelatedEBRPS);

            //        XmlElement xmlEBRID = xmlDoc.CreateElement("EBRID");
            //        xmlEBRID.InnerText = sHBRONO;
            //        xmlRelatedEBRPS.AppendChild(xmlEBRID);

            //        XmlElement xmlDeviceID = xmlDoc.CreateElement("EBRID");
            //        xmlDeviceID.InnerText = "0601" + DeviEBRID + lDev[l].DeviceID;
            //        xmlDevice.AppendChild(xmlDeviceID);

            //        XmlElement xmlDeviceName = xmlDoc.CreateElement("EBRName");
            //        xmlDeviceName.InnerText = l + "号";
            //        xmlDevice.AppendChild(xmlDeviceName);

            //        XmlElement xmlLongitude = xmlDoc.CreateElement("Longitude");
            //        xmlLongitude.InnerText = lDev[l].Longitude;
            //        xmlDevice.AppendChild(xmlLongitude);

            //        XmlElement xmlLatitude = xmlDoc.CreateElement("Latitude");
            //        xmlLatitude.InnerText = lDev[l].Latitude;
            //        xmlDevice.AppendChild(xmlLatitude);

            //        XmlElement xmlParams2 = xmlDoc.CreateElement("Params");
            //        xmlLatitude.InnerText = lDev[l].Latitude;
            //        xmlDevice.AppendChild(xmlParams2);
            //    }
            //}
            //#endregion End
            //#endregion End
            return document;
        }

        /// <summary>
        /// 平台信息
        /// </summary>
        /// <param name="ebdsr"></param>
        /// <returns></returns>
        public XmlDocument platformInfoResponse(EBD ebdsr, List<Device> lDev, string strebdid)
        {
            model.EBD EDB = new model.EBD();
            //   EDB.EBMStateResponse = new model.EBMStateResponse();

            //加入XML的声明段落,Save方法不再xml上写出独立属性
            EDB.EBDVersion = "1.0";
            EDB.EBDID = strebdid;
            EDB.EBDType = "EBRPSInfo";
            EDB.SRC = new model.SRC();
            EDB.SRC.EBRID = sHBRONO;
            EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        
            EDB.SRC.URL = serverini.ReadValue("PLATFORMINFO", "URL"); ;
            EDB.RelatedEBD = new model.RelatedEBD();
            EDB.RelatedEBD.EBDID = ebdsr.EBDID;

            EDB.DEST = new model.DEST();
            EDB.DEST.EBRID = serverini.ReadValue("FORM", "Superior");

            EDB.EBRPSInfo = new EBRPSInfo();
            EDB.EBRPSInfo.EBRPS = new List<model.EBRPSS>();
            EDB.EBRPSInfo.Params = new model.Params();
            EDB.EBRPSInfo.Params.RptEndTime =  DateTime.Now.AddDays(5).ToString("yyyy-MM-dd HH:mm:ss"); //ebd
            EDB.EBRPSInfo.Params.RptStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
            EDB.EBRPSInfo.Params.RptType = ebdsr.OMDRequest.Params.RptType;
           

          
            if (ebdsr.OMDRequest.Params.RptType.ToUpper() == "FULL")
            {
                EBRPSS EBRPS = new EBRPSS();
                EBRPS.RptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                EBRPS.RptType = "Sync";
                EBRPS.RelatedEBRPS = new RelatedEBRPS();
                EBRPS.RelatedEBRPS.EBRID = serverini.ReadValue("FORM", "Superior");
                EBRPS.PhoneNumber = serverini.ReadValue("PLATFORMINFO", "PhoneNumber");
                EBRPS.Longitude = serverini.ReadValue("PLATFORMINFO", "Longitude");
                if (EBRPS.Longitude.Split('.')[1].Length > 6)
                {
                    EBRPS.Longitude = EBRPS.Longitude.Split('.')[0] + "." + EBRPS.Longitude.Split('.')[1].Substring(0, 6);
                }
                EBRPS.Latitude = serverini.ReadValue("PLATFORMINFO", "Latitude");
                if (EBRPS.Latitude.Split('.')[1].Length > 6)
                {
                    EBRPS.Latitude = EBRPS.Latitude.Split('.')[0] + "." + EBRPS.Latitude.Split('.')[1].Substring(0, 6);
                }
                EBRPS.URL = serverini.ReadValue("PLATFORMINFO", "URL");
                EBRPS.Address = serverini.ReadValue("PLATFORMINFO", "Address");
                EBRPS.Contact = serverini.ReadValue("PLATFORMINFO", "Contact");
                EBRPS.EBRID = sHBRONO;
                EBRPS.EBRName = serverini.ReadValue("PLATFORMINFO", "EBRName");

                EDB.EBRPSInfo.EBRPS.Add(EBRPS);

            }

            //  string strXML = XmlSerialize<model.EBMStateResponse>(EBMStateResponse);
            string strXML = XmlSerialize<model.EBD>(EDB).Replace("<EBRPS>", "").Replace("</EBRPS>", "").Replace("EBRPSS", "EBRPS");


            XmlDocument document = new XmlDocument();
            document.LoadXml(strXML);
            //XmlDocument xmlDoc = new XmlDocument();

            ////加入XML的声明段落,Save方法不再xml上写出独立属性
            //xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            ////加入根元素
            //XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            //xmlDoc.AppendChild(xmlElem);
            //XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            //xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            //xmlElem.Attributes.Append(xmlns);

            ////Version
            //XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            //xmlProtocolVer.InnerText = "1.0";
            //xmlElem.AppendChild(xmlProtocolVer);

            ////EBDID
            //XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            //xmlEBDID.InnerText = strebdid;//
            //xmlElem.AppendChild(xmlEBDID);

            ////EBDType
            //XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            //xmlEBDType.InnerText = "EBRPSInfo";
            //xmlElem.AppendChild(xmlEBDType);

            ////Source
            //XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            //xmlElem.AppendChild(xmlSRC);

            //XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            //xmlSRCAreaCode.InnerText = sHBRONO;
            //xmlSRC.AppendChild(xmlSRCAreaCode);

            ////EBDTime
            //XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");

            //xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //xmlElem.AppendChild(xmlEBDTime);
            //if (ebdsr != null)
            //{
            //    //RelatedEBD
            //    XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            //    xmlElem.AppendChild(xmlRelatedEBD);
            //    XmlElement xmlReEBDID = xmlDoc.CreateElement("EBDID");
            //    xmlReEBDID.InnerText = ebdsr.EBDID;//与EBDID一致就用这个写
            //    xmlRelatedEBD.AppendChild(xmlReEBDID);
            //}

            //XmlElement xmlDeviceInfoReport = xmlDoc.CreateElement("EBRPSInfo");
            //xmlElem.AppendChild(xmlDeviceInfoReport);

            //XmlElement xmlParams = xmlDoc.CreateElement("Params");
            //xmlDeviceInfoReport.AppendChild(xmlParams);

            //XmlElement xmlRPTStartTime = xmlDoc.CreateElement("RptStartTime");//RPTStartTime
            //xmlRPTStartTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// ebdsr.DataRequest.StartTime;
            //xmlParams.AppendChild(xmlRPTStartTime);

            //XmlElement xmlRPTEndTime = xmlDoc.CreateElement("RptEndTime");//RPTEndTime
            //xmlRPTEndTime.InnerText = DateTime.Now.AddDays(5).ToString("yyyy-MM-dd HH:mm:ss"); //ebdsr.DataRequest.EndTime;
            //xmlParams.AppendChild(xmlRPTEndTime);

            //XmlElement xmlRptType = xmlDoc.CreateElement("RptType");//RPTEndTime
            //xmlRptType.InnerText = "Full"; //ebdsr.DataRequest.EndTime;
            //xmlParams.AppendChild(xmlRptType);

            //XmlElement xmlDevice = xmlDoc.CreateElement("EBRPS");//Term
            //xmlDeviceInfoReport.AppendChild(xmlDevice);

            //XmlElement xmlRptTime = xmlDoc.CreateElement("RptTime");
            //xmlRptTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //xmlDevice.AppendChild(xmlRptTime);

            //XmlElement xmlRptType2 = xmlDoc.CreateElement("RptType");
            //xmlRptType2.InnerText = "Sync";
            //xmlDevice.AppendChild(xmlRptType2);

            //XmlElement xmlRelatedEBRPS = xmlDoc.CreateElement("RelatedEBRPS");
            //xmlDevice.AppendChild(xmlRelatedEBRPS);

            //XmlElement xmlEBRID = xmlDoc.CreateElement("EBRID");
            //xmlEBRID.InnerText = sHBRONO;
            //xmlRelatedEBRPS.AppendChild(xmlEBRID);

            //XmlElement xmlDeviceID = xmlDoc.CreateElement("EBRID");
            //xmlDeviceID.InnerText = sHBRONO;
            //xmlDevice.AppendChild(xmlDeviceID);

            //XmlElement xmlDeviceName = xmlDoc.CreateElement("EBRName");
            //xmlDeviceName.InnerText = serverini.ReadValue("PLATFORMINFO", "EBRName");//"丹阳县应急广播平台";
            //xmlDevice.AppendChild(xmlDeviceName);

            //XmlElement Address = xmlDoc.CreateElement("Address");
            //Address.InnerText = serverini.ReadValue("PLATFORMINFO", "Address"); //"丹阳县广电";
            //xmlDevice.AppendChild(Address);

            //XmlElement Contact = xmlDoc.CreateElement("Contact");
            //Contact.InnerText = serverini.ReadValue("PLATFORMINFO", "Contact"); //"刘先生";
            //xmlDevice.AppendChild(Contact);

            //XmlElement PhoneNumber = xmlDoc.CreateElement("PhoneNumber");
            //PhoneNumber.InnerText = serverini.ReadValue("PLATFORMINFO", "PhoneNumber"); //"12345678901";
            //xmlDevice.AppendChild(PhoneNumber);

            //XmlElement Longitude = xmlDoc.CreateElement("Longitude");
            //Longitude.InnerText = "118.33";
            //xmlDevice.AppendChild(Longitude);

            //XmlElement Latitude = xmlDoc.CreateElement("Latitude");
            //Latitude.InnerText = "33.95";
            //xmlDevice.AppendChild(Latitude);

            //XmlElement URL = xmlDoc.CreateElement("URL");
            //URL.InnerText = "htttp://192.168.34.98:7000";
            //xmlDevice.AppendChild(URL);

            return document;
        }

        public XmlDocument platformInfoResponse(string strebdid)
        {
      
            model.EBD EDB = new model.EBD();
         //   EDB.EBMStateResponse = new model.EBMStateResponse();

            //加入XML的声明段落,Save方法不再xml上写出独立属性
            EDB.EBDVersion = "1.0";
            EDB.EBDID = strebdid;
            EDB.EBDType = "EBRPSInfo";
            EDB.SRC = new model.SRC();
            EDB.SRC.EBRID = sHBRONO;
            EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
          
            EDB.SRC.URL = serverini.ReadValue("PLATFORMINFO", "URL"); 


            EDB.DEST = new model.DEST();
            EDB.DEST.EBRID = serverini.ReadValue("FORM", "Superior");

            EDB.EBRPSInfo = new EBRPSInfo();
            EDB.EBRPSInfo.EBRPS = new List<model.EBRPSS>();
            //EDB.EBRPSInfo.Params = new model.Params();
            //EDB.EBRPSInfo.Params.RptEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //EDB.EBRPSInfo.Params.RptStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // EDB.EBRPSInfo.Params.RptType = "Full";
            EBRPSS EBRPS = new EBRPSS();
           EBRPS.RptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
           EBRPS.RptType = "Sync";
            EBRPS.RelatedEBRPS = new RelatedEBRPS();
           EBRPS.RelatedEBRPS.EBRID = serverini.ReadValue("FORM", "Superior");
            EBRPS.PhoneNumber = serverini.ReadValue("PLATFORMINFO", "PhoneNumber");
            EBRPS.Longitude = serverini.ReadValue("PLATFORMINFO", "Longitude");
            if (EBRPS.Longitude.Split('.')[1].Length > 6)
            {
                EBRPS.Longitude = EBRPS.Longitude.Split('.')[0] + "." + EBRPS.Longitude.Split('.')[1].Substring(0,6);
            }
            EBRPS.Latitude = serverini.ReadValue("PLATFORMINFO", "Latitude");
            if (EBRPS.Latitude.Split('.')[1].Length > 6)
            {
                EBRPS.Latitude = EBRPS.Latitude.Split('.')[0] +"."+ EBRPS.Latitude.Split('.')[1].Substring(0, 6);
            }
            EBRPS.URL = "http://";
            EBRPS.Address = serverini.ReadValue("PLATFORMINFO", "Address");
            EBRPS.Contact = serverini.ReadValue("PLATFORMINFO", "Contact");
            EBRPS.EBRID = sHBRONO;
            EBRPS.EBRName = serverini.ReadValue("PLATFORMINFO", "EBRName");
    
            EDB.EBRPSInfo.EBRPS.Add(EBRPS);



            //  string strXML = XmlSerialize<model.EBMStateResponse>(EBMStateResponse);
            string strXML = XmlSerialize<model.EBD>(EDB).Replace("<EBRPS>","").Replace("</EBRPS>","").Replace("EBRPSS", "EBRPS") ;


            XmlDocument document = new XmlDocument();
            document.LoadXml(strXML);



            //2018-06-02修改

            //xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            ////加入根元素
            //XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            //xmlDoc.AppendChild(xmlElem);
            //XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            //xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            //xmlElem.Attributes.Append(xmlns);

            ////Version
            //XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            //xmlProtocolVer.InnerText = "1.0";
            //xmlElem.AppendChild(xmlProtocolVer);
            ////EBDID
            //XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            //xmlEBDID.InnerText = strebdid;//strebdid;//自己的ID前面一长串
            //xmlElem.AppendChild(xmlEBDID);

            ////EBDType
            //XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            //xmlEBDType.InnerText = "EBRPSInfo";
            //xmlElem.AppendChild(xmlEBDType);

            ////Source
            //XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            //xmlElem.AppendChild(xmlSRC);

            //XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            //xmlSRCAreaCode.InnerText = sHBRONO;// sHBRONO;单独的ID
            //xmlSRC.AppendChild(xmlSRCAreaCode);


            //XmlElement xmlDEST = xmlDoc.CreateElement("DEST");
            //xmlElem.AppendChild(xmlDEST);

            //XmlElement xmlDESTEBRID = xmlDoc.CreateElement("EBRID");
            //xmlDESTEBRID.InnerText = "010232000000000001";// sHBRONO;单独的ID
            //xmlDEST.AppendChild(xmlDESTEBRID);

            ////EBDTime
            //XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");

            //xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //xmlElem.AppendChild(xmlEBDTime);

            ////RelatedEBD
            ////XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            ////xmlElem.AppendChild(xmlRelatedEBD);
            ////XmlElement xmlReEBDID = xmlDoc.CreateElement("EBDID");
            ////xmlReEBDID.InnerText = strebdid;//与EBDID一致就用这个写
            ////xmlRelatedEBD.AppendChild(xmlReEBDID);

            //XmlElement xmlDeviceInfoReport = xmlDoc.CreateElement("EBRPSInfo");
            //xmlElem.AppendChild(xmlDeviceInfoReport);

            //XmlElement xmlParams = xmlDoc.CreateElement("Params");
            //xmlElem.AppendChild(xmlParams);

            //XmlElement xmlRPTStartTime = xmlDoc.CreateElement("RPTStartTime");//RPTStartTime
            //xmlRPTStartTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// ebdsr.DataRequest.StartTime;
            //xmlParams.AppendChild(xmlRPTStartTime);

            //XmlElement xmlRPTEndTime = xmlDoc.CreateElement("RPTEndTime");//RPTEndTime
            //xmlRPTEndTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //ebdsr.DataRequest.EndTime;
            //xmlParams.AppendChild(xmlRPTEndTime);

            //XmlElement xmlRptType = xmlDoc.CreateElement("RptType");//RPTEndTime
            //xmlRptType.InnerText = "Full"; //ebdsr.DataRequest.EndTime;
            //xmlParams.AppendChild(xmlRptType);

            //XmlElement xmlDevice = xmlDoc.CreateElement("EBRPS");//Term
            //xmlDeviceInfoReport.AppendChild(xmlDevice);

            //XmlElement xmlRptTime = xmlDoc.CreateElement("RptTime");
            //xmlRptTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //xmlDevice.AppendChild(xmlRptTime);

            //XmlElement xmlRptType2 = xmlDoc.CreateElement("RptType");
            //xmlRptType2.InnerText = "Sync";
            //xmlDevice.AppendChild(xmlRptType2);

            //XmlElement xmlRelatedEBRPS = xmlDoc.CreateElement("RelatedEBRPS");
            //xmlDevice.AppendChild(xmlRelatedEBRPS);

            //XmlElement xmlEBRID = xmlDoc.CreateElement("EBRID");
            //xmlEBRID.InnerText = sHBRONO;
            //xmlRelatedEBRPS.AppendChild(xmlEBRID);

            //XmlElement xmlDeviceID = xmlDoc.CreateElement("EBRID");
            //xmlDeviceID.InnerText = sHBRONO;
            //xmlDevice.AppendChild(xmlDeviceID);

            //XmlElement xmlDeviceName = xmlDoc.CreateElement("EBRName");
            //xmlDeviceName.InnerText = serverini.ReadValue("PLATFORMINFO", "EBRName");//"丹阳县应急广播平台";
            //xmlDevice.AppendChild(xmlDeviceName);

            //XmlElement Address = xmlDoc.CreateElement("Address");
            //Address.InnerText = serverini.ReadValue("PLATFORMINFO", "Address");//"丹阳县广电";
            //xmlDevice.AppendChild(Address);

            //XmlElement Contact = xmlDoc.CreateElement("Contact");
            //Contact.InnerText = serverini.ReadValue("PLATFORMINFO", "Contact");//"老铁";
            //xmlDevice.AppendChild(Contact);

            //XmlElement PhoneNumber = xmlDoc.CreateElement("PhoneNumber");
            //PhoneNumber.InnerText = serverini.ReadValue("PLATFORMINFO", "PhoneNumber");//"12345678901";
            //xmlDevice.AppendChild(PhoneNumber);

            //XmlElement Longitude = xmlDoc.CreateElement("Longitude");
            //Longitude.InnerText = "118.33"; // "113.7747551274";
            //xmlDevice.AppendChild(Longitude);

            //XmlElement Latitude = xmlDoc.CreateElement("Latitude");
            //Latitude.InnerText = "33.95"; //  "34.6328783614";
            //xmlDevice.AppendChild(Latitude);

            //XmlElement URL = xmlDoc.CreateElement("URL");
            //URL.InnerText = "192.168.34.98";
            //xmlDevice.AppendChild(URL);

            return document;
        }

        public XmlDocument platformInfoResponse(string strebdid,int num)
        {
       
            XmlDocument document = new XmlDocument();
            model.EBD EDB = new model.EBD();

            //加入XML的声明段落,Save方法不再xml上写出独立属性
            EDB.EBDVersion = "1.0";
            EDB.EBDID = strebdid;
            EDB.EBDType = "EBRPSInfo";
            EDB.SRC = new model.SRC();
            EDB.SRC.EBRID = sHBRONO;
            EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            EDB.SRC.URL = serverini.ReadValue("PLATFORMINFO", "URL"); 
            EDB.RelatedEBD = new model.RelatedEBD();
            EDB.RelatedEBD.EBDID = "空";
            EDB.EBRPSInfo = new EBRPSInfo();
            EDB.EBRPSInfo.EBRPS = new List<model.EBRPSS>();
            EBRPSS EBRPS = new EBRPSS();
            EBRPS.RelatedEBRPS = new RelatedEBRPS();
            EBRPS.RelatedEBRPS.EBRID = serverini.ReadValue("FORM", "Superior");//省平台的资源编码
            EBRPS.RptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            EBRPS.RptType = "Sync";
            EBRPS.PhoneNumber = serverini.ReadValue("PLATFORMINFO", "PhoneNumber");
            EBRPS.Longitude = serverini.ReadValue("PLATFORMINFO", "Longitude");
            if (EBRPS.Longitude.Split('.')[1].Length > 6)
            {
                EBRPS.Longitude = EBRPS.Longitude.Split('.')[0] + "." + EBRPS.Longitude.Split('.')[1].Substring(0, 6);
            }
            EBRPS.Latitude = serverini.ReadValue("PLATFORMINFO", "Latitude");
            if (EBRPS.Latitude.Split('.')[1].Length > 6)
            {
                EBRPS.Latitude = EBRPS.Latitude.Split('.')[0] + "." + EBRPS.Latitude.Split('.')[1].Substring(0, 6);
            }
            EBRPS.URL = serverini.ReadValue("PLATFORMINFO", "URL");
            EBRPS.Address = serverini.ReadValue("PLATFORMINFO", "Address");
            EBRPS.Contact = serverini.ReadValue("PLATFORMINFO", "Contact");
            EBRPS.EBRID = sHBRONO;
            EBRPS.EBRName = serverini.ReadValue("PLATFORMINFO", "EBRName");
            //2018-06-09添加唯一上传
            if (num == 1)
            {
                EDB.EBRPSInfo.EBRPS.Add(EBRPS);
                string strXML = XmlSerialize<model.EBD>(EDB).Replace("<EBRPS>", "").Replace("</EBRPS>", "").Replace("EBRPSS", "EBRPS");
                document.LoadXml(strXML);
                return document;
            }
            else
            {
                //状态2自动上传

                //是否记录
                if (ServerForm.EbrpssInfo.ContainsKey(sHBRONO) && ServerForm.EbrpssInfo.Count == 1)
                {
                    if (!ContrastEBRPS(ServerForm.EbrpssInfo[sHBRONO], EBRPS))
                    {
                        UpdateEbrpssInfo(EBRPS);
                        ServerForm.EbrpssInfo[sHBRONO] = EBRPS;
                        EDB.EBRPSInfo.EBRPS.Add(EBRPS);
                        string strXML = XmlSerialize<model.EBD>(EDB).Replace("<EBRPS>", "").Replace("</EBRPS>", "").Replace("EBRPSS", "EBRPS");
                        document.LoadXml(strXML);
                        return document;
                    }
                }
                else if (ServerForm.EbrpssInfo.Count > 1)

                {
                    if (ServerForm.EbrpssInfo.ContainsKey(sHBRONO))
                    {
                        if (!ContrastEBRPS(ServerForm.EbrpssInfo[sHBRONO], EBRPS))
                        {
                            // 有变动
                            ServerForm.EbrpssInfo.Clear();
                            UpdateEbrpssInfo(EBRPS);
                            ServerForm.EbrpssInfo.Add(sHBRONO, EBRPS);
                            EDB.EBRPSInfo.EBRPS.Add(EBRPS);
                            string strXML = XmlSerialize<model.EBD>(EDB).Replace("<EBRPS>", "").Replace("</EBRPS>", "").Replace("EBRPSS", "EBRPS");
                            document.LoadXml(strXML);
                            return document;
                        }
                        else
                        {
                          //  有包含未变动
                            ServerForm.EbrpssInfo.Clear();
                            ServerForm.EbrpssInfo.Add(sHBRONO, EBRPS);
               
                        }
                    }
                }
                else if (!ServerForm.EbrpssInfo.ContainsKey(sHBRONO))
                {
                    //第一次载入
                    EBRPSS ebrpss = GetEbrpssInfo(sHBRONO);

                    if (ebrpss == null)
                    {
                        AddEbrpssInfo(EBRPS);
                        ServerForm.EbrpssInfo.Add(sHBRONO, EBRPS);
                        EDB.EBRPSInfo.EBRPS.Add(EBRPS);
                        string strXML = XmlSerialize<model.EBD>(EDB).Replace("<EBRPS>", "").Replace("</EBRPS>", "").Replace("EBRPSS", "EBRPS");
                        document.LoadXml(strXML);
                        return document;
                    }
                    else
                    {
                        if (!ContrastEBRPS(ebrpss, EBRPS) && ServerForm.EbrpssInfo.Count == 0)
                        {

                            ServerForm.EbrpssInfo.Add(sHBRONO, EBRPS);
                            EDB.EBRPSInfo.EBRPS.Add(EBRPS);
                            string strXML = XmlSerialize<model.EBD>(EDB).Replace("<EBRPS>", "").Replace("</EBRPS>", "").Replace("EBRPSS", "EBRPS");
                            document.LoadXml(strXML);
                            return document;
                        }
                        else if (ServerForm.EbrpssInfo.Count == 0)
                        {
                            ServerForm.EbrpssInfo.Add(sHBRONO, EBRPS);
                        }
                        else if(!ContrastEBRPS(ebrpss, EBRPS))
                        {
                            UpdateEbrpssInfo(EBRPS);
                            ServerForm.EbrpssInfo[sHBRONO]=EBRPS;

                        }
                    }
                }

            }

            return null;
        }


        public bool UpdateEbrpssInfo(EBRPSS EBRPSS)
        {
            string EbrpssSql = "update ebrpss set RptTime='"+ EBRPSS.RptTime + "',EBRID='"+ EBRPSS.EBRID +
                "',EBRName='"+ EBRPSS.EBRName + "',Address='"+ EBRPSS.Address + "',Contact='"+ EBRPSS.Contact

                + "',PhoneNumber='"+ EBRPSS.PhoneNumber

                + "',Longitude='"+ EBRPSS.Longitude+"',Latitude='"+EBRPSS.Latitude+"',URL='"+EBRPSS.URL+ "' where EBRID='" + EBRPSS.EBRID+"'";
            string ID = mainForm.dba.UpdateDbBySQLRetID(EbrpssSql).ToString();
            if (string.IsNullOrEmpty(ID))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public Device QueryTerminalMaintenance(string id)
        {
            string QueryTerminalsql = "select * from TerminalMaintenance where SRV_ID="+id;
            DataTable dtUser = mainForm.dba.getQueryInfoBySQL(QueryTerminalsql);
            foreach (DataRow item in dtUser.Rows)
            {
                Device DV = new Device();
                DV.Old_Srv_Mft_Date = item["SRV_MFT_DATE"].ToString();
                DV.Old_UpdateDate = item["updateDate"].ToString();
                DV.SRV_ID = item["SRV_ID"].ToString();
                return DV;
            }
            return null;

        }
        public bool UpdateTerminalMaintenance(Device dev)
        {
            string TerminalUpdateSql = "Update TerminalMaintenance set SRV_ID='"
     + dev.SRV_ID + "',"
     + dev.Srv_Mft_Date + "',"
      + dev.UpdateDate + "'";
            string ID = mainForm.dba.UpdateDbBySQLRetID(TerminalUpdateSql).ToString();
            if (string.IsNullOrEmpty(ID))
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public bool AddTerminalMaintenance(Device dev)
        {
            // select SRV_ID,SRV_MFT_DATE,updateDate from TerminalMaintenance

            string EbrpssSql = "insert into TerminalMaintenance(SRV_ID,SRV_MFT_DATE,updateDate) values('"
     + dev.SRV_ID + "',"
     + dev.Srv_Mft_Date + "',"
     + dev.UpdateDate + "'"
     + ")";
            string ID = mainForm.dba.UpdateDbBySQLRetID(EbrpssSql).ToString();
            if (string.IsNullOrEmpty(ID))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool AddEbrpssInfo(EBRPSS EBRPSS)
        {
            string EbrpssSql = "insert into Ebrpss (RptTime,EBRID,EBRName,Address,Contact,PhoneNumber,Longitude,Latitude,URL) values('"
                +EBRPSS.RptTime+"',"
                + EBRPSS.EBRID + "',"
                + EBRPSS.EBRName + "',"
                + EBRPSS.Address + "',"
                + EBRPSS.Contact + "',"
                + EBRPSS.PhoneNumber + "',"
                + EBRPSS.Longitude + "',"
                + EBRPSS.Latitude
                + EBRPSS.URL 
                + ")";
       
           string ID = mainForm.dba.UpdateDbBySQLRetID(EbrpssSql).ToString();
            if (string.IsNullOrEmpty(ID))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 查询数据库平台记录
        /// </summary>
        public void GetEbrpssInfo()
        {

            string EbrpssSql = "select * from Ebrpss";

            DataTable dtUser = mainForm.dba.getQueryInfoBySQL(EbrpssSql);
            if (dtUser.Rows.Count > 0)
            {
                foreach (DataRow item in dtUser.Rows)
                {
                    EBRPSS ebrpss = new EBRPSS();
                    /// <summary>
                    /// 数据操作生成时间
                    /// </summary>
                    ebrpss.RptTime = item["RptTime"].ToString();

                    /// <summary>
                    /// 应急平台编码
                    /// </summary>
                    ebrpss.EBRID = item["EBRID"].ToString();

                    /// <summary>
                    /// 应急平台名称
                    /// </summary>
                    ebrpss.EBRName = item["EBRName"].ToString();

                    /// <summary>
                    /// 应急平台地址
                    /// </summary>
                    ebrpss.Address = item["Address"].ToString();

                    /// <summary>
                    /// 联系人
                    /// </summary>
                    ebrpss.Contact = item["Contact"].ToString();

                    /// <summary>
                    /// 联系电话
                    /// </summary>
                    ebrpss.PhoneNumber = item["PhoneNumber"].ToString();

                    /// <summary>
                    /// 经度
                    /// </summary>
                    ebrpss.Longitude = item["Longitude"].ToString();

                    /// <summary>
                    /// 纬度
                    /// </summary>
                    ebrpss.Latitude = item["Latitude"].ToString();
                    //网络地址
                    ebrpss.URL = item["URL"].ToString();
                    ServerForm.EbrpssInfo.Add(ebrpss.EBRID, ebrpss);
                }

            }
        }
        public model.EBRPSS GetEbrpssInfo(string EBRID)
        {

            string EbrpssSql = "select * from Ebrpss where EBRID='" + EBRID + "'";

            DataTable dtUser = mainForm.dba.getQueryInfoBySQL(EbrpssSql);
            if (dtUser.Rows.Count > 0)
            {
                foreach (DataRow item in dtUser.Rows)
                {
                    EBRPSS ebrpss = new EBRPSS();
                    /// <summary>
                    /// 数据操作生成时间
                    /// </summary>
                    ebrpss.RptTime = item["RptTime"].ToString();

                    /// <summary>
                    /// 应急平台编码
                    /// </summary>
                    ebrpss.EBRID = item["EBRID"].ToString();

                    /// <summary>
                    /// 应急平台名称
                    /// </summary>
                    ebrpss.EBRName = item["EBRName"].ToString();

                    /// <summary>
                    /// 应急平台地址
                    /// </summary>
                    ebrpss.Address = item["Address"].ToString();

                    /// <summary>
                    /// 联系人
                    /// </summary>
                    ebrpss.Contact = item["Contact"].ToString();

                    /// <summary>
                    /// 联系电话
                    /// </summary>
                    ebrpss.PhoneNumber = item["PhoneNumber"].ToString();

                    /// <summary>
                    /// 经度
                    /// </summary>
                    ebrpss.Longitude = item["Longitude"].ToString();

                    /// <summary>
                    /// 纬度
                    /// </summary>
                    ebrpss.Latitude = item["Latitude"].ToString();
                    //网络地址
                    ebrpss.URL = item["URL"].ToString();
                    return ebrpss;
                }
            
            }
            return null;
        }
        public bool ContrastEBRPS(model.EBRPSS E1,model.EBRPSS E2 )
        {

            if (!E1.EBRID.Contains(E2.EBRID))
            {
                return false;
            }
            if (!E1.EBRName.Contains(E2.EBRName))
            {
                return false;
            }
            if (!E1.Address.Contains(E2.Address))
            {
                return false;
            }


            /// <summary>
            /// 联系人
            /// </summary>
            if (!E1.Contact.Contains(E2.Contact))
            {
                return false;
            }



            /// <summary>
            /// 联系电话
            /// </summary>
            if (!E1.PhoneNumber.Contains(E2.PhoneNumber))
            {
                return false;
            }

            /// <summary>
            /// 经度
            /// </summary>
            if (!E1.Longitude.Contains(E2.Longitude))


            {
                return false;
            }

            /// <summary>
            /// 纬度
            /// </summary>
            if (!E1.Latitude.Contains(E2.Longitude))


            {
                return false;
            }

            //网络地址
            if (!E1.URL.Contains(E2.URL))


            {
                return false;
            }
            return true;

        }
        /// <summary>
        /// 播发状态上报
        /// </summary>
        /// <param name="ebdsr"></param>
        /// <returns></returns>
        public XmlDocument PlayRecordResponse(EBD ebdsr, string strebdid, Params param)
        {

            string StartTime = param.RptStartTime;
            string EndTime = param.RptEndTime;
            string rtptype = param.RptType;
            model.EDB_EBRBS.EBD EDB = new EDB_EBRBS.EBD();
            //   EDB.EBMStateResponse = new model.EBMStateResponse();

            //加入XML的声明段落,Save方法不再xml上写出独立属性
            EDB.EBDVersion = "1.0";
            EDB.EBDID = strebdid;
            EDB.EBDType = "EBMBrdLog";
            EDB.SRC = new model.SRC();
            EDB.SRC.EBRID = sHBRONO;
            EDB.RelatedEBD = new model.RelatedEBD();
            EDB.RelatedEBD.EBDID = ebdsr.EBDID;
            EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            EDB.RelatedEBD = new model.RelatedEBD();//合版本
            EDB.RelatedEBD.EBDID = ebdsr.EBDID;//合版本
       
            EDB.EBMBrdLog = new EBMBrdLog();
            EDB.EBMBrdLog.EBMBrdItem = new List<EBMBrdItems>();

            if (rtptype == "Full")
            {
                string MediaSQL = "select * from TSCMDSTORE where TsCmd_Type ='播放视频' and TsCmd_Date between '" + StartTime + "' and '" + EndTime + "'order by TsCmd_Date desc";


                DataTable dtMedia = mainForm.dba.getQueryInfoBySQL(MediaSQL);

                if (dtMedia.Rows.Count>0)
                {
                    SingletonInfo.GetInstance().PlayRecordLastTime = dtMedia.Rows[0]["TsCmd_Date"].ToString();
                }
             

                foreach (DataRow item in dtMedia.Rows)
                {
                    //EBMBrdItem
                    EBMBrdItems EBItem = new EBMBrdItems();
                    EBItem.EBM = new model.EBM();

                    EBItem.EBM.EBMID = item["Ebm_ID"].ToString();
                    EBItem.EBM.MsgContent =new  model.MsgContent();
                    EBItem.EBM.MsgContent.LanguageCode = "zho";
                    EBItem.EBM.MsgContent.MsgTitle = item["MsgTitle"].ToString();
                    EBItem.EBM.MsgContent.MsgDesc = "";
                    EBItem.EBM.MsgContent.AreaCode = item["AreaCode"].ToString();

                    string StateCodetmp = item["TsCmd_Status"].ToString();
                    switch (StateCodetmp)
                    {
                        case "0":
                            EBItem.BrdStateCode = "0";
                            EBItem.BrdStateDesc = "未处理";


                            break;
                        case "1":
                            string flag = item["TsCmd_ID"].ToString();

                            string SQLSTR = "select * from playRecord where PR_SourceID =" + flag;

                            DataTable dtMediaTMP = mainForm.dba.getQueryInfoBySQL(SQLSTR);

                            if (dtMediaTMP.Rows.Count > 0)
                            {
                                EBItem.BrdStateCode = "2";
                                EBItem.BrdStateDesc = "播发中";//合版本
                            }
                            else
                            {
                                EBItem.BrdStateCode = "3";
                                EBItem.BrdStateDesc = "播发成功";//合版本
                            }

                            break;
                    }
                    EDB.EBMBrdLog.EBMBrdItem.Add(EBItem);
                }
            }
            else
            {
                string MediaSQL = "";
                if (SingletonInfo.GetInstance().PlayRecordLastTime == "")
                {
                     MediaSQL = "select * from TSCMDSTORE where TsCmd_Type ='播放视频' and TsCmd_Date between '" + StartTime + "' and '" + EndTime + "'order by TsCmd_Date desc";

                }
                else
                {
                     MediaSQL = "select * from TSCMDSTORE where TsCmd_Type ='播放视频' and TsCmd_Date between '" + StartTime + "' and '" + EndTime + "' and  TsCmd_Date >'" + SingletonInfo.GetInstance().PlayRecordLastTime + "' order by TsCmd_Date desc";

                }

                DataTable dtMedia = mainForm.dba.getQueryInfoBySQL(MediaSQL);
                if (dtMedia.Rows.Count >0)
                {
                    SingletonInfo.GetInstance().PlayRecordLastTime = dtMedia.Rows[0]["TsCmd_Date"].ToString();

                    foreach (DataRow item in dtMedia.Rows)
                    {
                        //EBMBrdItem
                        EBMBrdItems EBItem = new EBMBrdItems();
                        EBItem.EBM = new model.EBM();

                        EBItem.EBM.EBMID = item["Ebm_ID"].ToString();
                        EBItem.EBM.MsgContent = new model.MsgContent();
                        EBItem.EBM.MsgContent.LanguageCode = "zho";
                        EBItem.EBM.MsgContent.MsgTitle = item["MsgTitle"].ToString();
                        EBItem.EBM.MsgContent.MsgDesc = "";
                        EBItem.EBM.MsgContent.AreaCode = item["AreaCode"].ToString();

                        string StateCodetmp = item["TsCmd_Status"].ToString();
                        switch (StateCodetmp)
                        {
                            case "0":
                                EBItem.BrdStateCode = "0";
                                EBItem.BrdStateDesc = "未处理";


                                break;
                            case "1":
                                string flag = item["TsCmd_ID"].ToString();

                                string SQLSTR = "select * from playRecord where PR_SourceID =" + flag;

                                DataTable dtMediaTMP = mainForm.dba.getQueryInfoBySQL(SQLSTR);

                                if (dtMediaTMP.Rows.Count > 0)
                                {
                                    EBItem.BrdStateCode = "2";
                                    EBItem.BrdStateDesc = "播发中";//合版本
                                }
                                else
                                {
                                    EBItem.BrdStateCode = "3";
                                    EBItem.BrdStateDesc = "播发成功";//合版本
                                }

                                break;
                        }
                        EDB.EBMBrdLog.EBMBrdItem.Add(EBItem);
                    }
                }


            }


       
           
            string strXML = XmlSerialize<EDB_EBRBS.EBD>(EDB).Replace("<EBMBrdItem>", "").Replace("</EBMBrdItem>", "").Replace("EBMBrdItems", "EBMBrdItem");


            XmlDocument document = new XmlDocument();
            document.LoadXml(strXML);
            return document;
        }


        /// <summary>
        /// 平台状态信息
        /// </summary>
        /// <param name="ebdsr"></param>
        /// <returns></returns>
        public XmlDocument platformstateInfoResponse(EBD ebdsr, List<Device> lDev, string strebdid)
        {
            model.EBD EDB = new model.EBD();
            EDB.EBRDTState = new List<EBRDT>();

            //加入XML的声明段落,Save方法不再xml上写出独立属性
            EDB.EBDVersion = "1.0";
            EDB.EBDID = strebdid;
            EDB.EBDType = "EBRPSState";
            EDB.SRC = new model.SRC();
            EDB.SRC.EBRID = sHBRONO;
            EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
      
            EDB.SRC.URL = serverini.ReadValue("PLATFORMINFO", "URL");

            EDB.DEST = new model.DEST();
            EDB.DEST.EBRID = serverini.ReadValue("FORM", "Superior");
            EDB.RelatedEBD = new model.RelatedEBD();
            EDB.RelatedEBD.EBDID = ebdsr.EBDID;

            EDB.EBRDTState = new List<EBRDT>();
            EBRDT EBRDT = new EBRDT();
            EBRDT.RptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            EBRDT.StateCode = 1;
            EBRDT.StateDesc = "运行正常";
            EBRDT.EBRID = sHBRONO;
            EDB.EBRDTState.Add(EBRDT);
            XmlDocument document = new XmlDocument();
            string strXML = XmlSerialize<model.EBD>(EDB);
            document.LoadXml(strXML);
            //XmlDocument xmlDoc = new XmlDocument();

            ////加入XML的声明段落,Save方法不再xml上写出独立属性
            //xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            ////加入根元素
            //XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            //xmlDoc.AppendChild(xmlElem);
            //XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            //xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            //xmlElem.Attributes.Append(xmlns);

            ////Version
            //XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            //xmlProtocolVer.InnerText = "1.0";
            //xmlElem.AppendChild(xmlProtocolVer);
            ////EBDID
            //XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            //xmlEBDID.InnerText = strebdid;//
            //xmlElem.AppendChild(xmlEBDID);

            ////EBDType
            //XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            //xmlEBDType.InnerText = "EBRPSState";
            //xmlElem.AppendChild(xmlEBDType);

            ////Source
            //XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            //xmlElem.AppendChild(xmlSRC);

            //XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            //xmlSRCAreaCode.InnerText = sHBRONO;
            //xmlSRC.AppendChild(xmlSRCAreaCode);

            ////EBDTime
            //XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");

            //xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //xmlElem.AppendChild(xmlEBDTime);

            //if (ebdsr != null)
            //{
            //    //RelatedEBD
            //    XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            //    xmlElem.AppendChild(xmlRelatedEBD);
            //    XmlElement xmlReEBDID = xmlDoc.CreateElement("EBDID");
            //    xmlReEBDID.InnerText = ebdsr.EBDID;//与EBDID一致就用这个写
            //    xmlRelatedEBD.AppendChild(xmlReEBDID);
            //}


            //XmlElement xmlDeviceInfoReport = xmlDoc.CreateElement("EBRPSState");
            //xmlElem.AppendChild(xmlDeviceInfoReport);

            //XmlElement xmlDevice = xmlDoc.CreateElement("EBRPS");//Term
            //xmlDeviceInfoReport.AppendChild(xmlDevice);

            //XmlElement xmlRptTime = xmlDoc.CreateElement("RptTime");
            //xmlRptTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //xmlDevice.AppendChild(xmlRptTime);

            //XmlElement xmlDeviceID = xmlDoc.CreateElement("EBRID");
            //xmlDeviceID.InnerText = sHBRONO;
            //xmlDevice.AppendChild(xmlDeviceID);

            //XmlElement StateCode = xmlDoc.CreateElement("StateCode");
            //StateCode.InnerText = "1";
            //xmlDevice.AppendChild(StateCode);

            //XmlElement StateDesc = xmlDoc.CreateElement("StateDesc");
            //StateDesc.InnerText = "系统运行正常";
            //xmlDevice.AppendChild(StateDesc);

            return document;
        }

        public XmlDocument platformstateInfoResponse(string strebdid)
        {
            model.EBD EDB = new model.EBD();
           // EDB.EBRDTState = new List<EBRDT>();

            //加入XML的声明段落,Save方法不再xml上写出独立属性
            EDB.EBDVersion = "1.0";
            EDB.EBDID = strebdid;
            EDB.EBDType = "EBRPSState";
            EDB.SRC = new model.SRC();
            EDB.SRC.EBRID = sHBRONO;
            EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            EDB.SRC.URL = serverini.ReadValue("PLATFORMINFO", "URL"); ;

            EDB.DEST = new model.DEST();
            EDB.DEST.EBRID = serverini.ReadValue("FORM", "Superior");


            EDB.EBRPSState = new List<EBRPS>();
            EBRPS ebrps = new EBRPS();
            ebrps.RptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
           
            ebrps.StateDesc = serverini.ReadValue("PlatformState", "State");
            if (ebrps.StateDesc=="故障")
            {
                ebrps.StateCode = 3;
            }

            if (ebrps.StateDesc == "正常")
            {
                ebrps.StateCode = 1;
            }

            if (ebrps.StateDesc == "停止")
            {
                ebrps.StateCode = 2;
            }


            if (ebrps.StateDesc == "故障恢复")
            {
                ebrps.StateCode = 4;
            }

            if (ebrps.StateDesc == "播发中")
            {
                ebrps.StateCode = 5;
            }

            ebrps.EBRID = sHBRONO;
            EDB.EBRPSState.Add(ebrps);
            
         
           XmlDocument document = new XmlDocument();
            string strXML = XmlSerialize<model.EBD>(EDB);
            document.LoadXml(strXML);
            return document;
        }
        public XmlDocument DevicePlayback(string strebdid,DataTable E_ID)
        {
            model.EDB_EBRBS.EBD EDB = new EDB_EBRBS.EBD();
            //   EDB.EBMStateResponse = new model.EBMStateResponse();

            //加入XML的声明段落,Save方法不再xml上写出独立属性
            EDB.EBDVersion = "1.0";
            EDB.EBDID = strebdid;
            EDB.EBDType = "EBMBrdLog";
            EDB.SRC = new model.SRC();
            EDB.SRC.EBRID = sHBRONO;
            EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
          
            EDB.SRC.URL = serverini.ReadValue("PLATFORMINFO", "URL");
            EDB.EBMBrdLog = new EBMBrdLog();
            EDB.EBMBrdLog.EBMBrdItem = new List<EBMBrdItems>();
            foreach (DataRow item in E_ID.Rows)
            {
                EBMBrdItems EBItem = new EBMBrdItems();
                EBItem.EBM = new model.EBM();
                EBItem.EBM.EBMID = item["EBM_ID"].ToString();
                EBItem.EBM.MsgContent = new model.MsgContent();
                EBItem.EBM.MsgContent.LanguageCode = "zho";
                EBItem.EBM.MsgContent.MsgTitle = item["MsgTitle"].ToString();
                EBItem.EBM.MsgContent.MsgDesc = "";
                EBItem.EBM.MsgContent.AreaCode = item["AreaCode"].ToString();
                string StateCodetmp = item["TsCmd_Status"].ToString();
                switch (StateCodetmp)
                {
                    case "0":
                        EBItem.BrdStateCode = "0";
                        EBItem.BrdStateDesc = "未处理";


                        break;
                    case "1":
                        string flag = item["TsCmd_ID"].ToString();

                        string SQLSTR = "select * from playRecord where PR_SourceID =" + flag;

                        DataTable dtMediaTMP = mainForm.dba.getQueryInfoBySQL(SQLSTR);

                        if (dtMediaTMP.Rows.Count > 0)
                        {
                            EBItem.BrdStateCode = "2";
                            EBItem.BrdStateDesc = "播发中";//合版本
                        }
                        else
                        {
                            EBItem.BrdStateCode = "3";
                            EBItem.BrdStateDesc = "播发成功";//合版本
                        }

                        break;
                }

                EDB.EBMBrdLog.EBMBrdItem.Add(EBItem);
            }
            //EBMBrdItem
           
            string strXML = XmlSerialize<EDB_EBRBS.EBD>(EDB).Replace("<EBMBrdItem>", "").Replace("</EBMBrdItem>", "").Replace("EBMBrdItems", "EBMBrdItem");


            XmlDocument document = new XmlDocument();
            document.LoadXml(strXML);
            return document;
        }
        /// <summary>
        /// 设备状态数据
        /// </summary>
        /// <param name="ebdsr"></param>
        /// <returns></returns>
        public XmlDocument DeviceStateResponse(EBD ebdsr, List<Device> lDevState, string strebdid)
        {
            model.EBD EDB = new model.EBD();
            EDB.EBRDTState = new List<EBRDT>();

            //加入XML的声明段落,Save方法不再xml上写出独立属性
            EDB.EBDVersion = "1.0";
            EDB.EBDID = strebdid;
            EDB.EBDType = "EBRDTState";
            EDB.SRC = new model.SRC();
            EDB.SRC.EBRID = sHBRONO;
            EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            EDB.DEST = new model.DEST();
            EDB.DEST.EBRID = serverini.ReadValue("FORM", "Superior");

            EDB.RelatedEBD = new model.RelatedEBD();//合版本
            EDB.RelatedEBD.EBDID = ebdsr.EBDID;//合版本
         
            EDB.SRC.URL = serverini.ReadValue("PLATFORMINFO", "URL"); 

            EDB.RelatedEBD = new model.RelatedEBD();
            EDB.RelatedEBD.EBDID = ebdsr.EBDID;

            string DeviEBRID = sHBRONO.Substring(4, sHBRONO.Length - 6);
            if (lDevState.Count > 0)
            {
                for (int l = 0; l < lDevState.Count; l++)
                {
                    EBRDT EBRDT = new EBRDT();
               
                 
                        EBRDT.EBRID = lDevState[l].DeviceID;
               
                    EBRDT.RptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    if (lDevState[l].DeviceState == "离线")
                    {
                        EBRDT.StateCode = 3;
                    }
                    else
                    {
                        EBRDT.StateCode = 1;
                    }
                  
                    EBRDT.StateDesc = lDevState[l].DeviceState;
                    EDB.DEST.EBRID = serverini.ReadValue("FORM", "SuperiorPlatform");

                    EDB.EBRDTState.Add(EBRDT);

                }
            }

            string strXML = XmlSerialize<model.EBD>(EDB);
            XmlDocument document = new XmlDocument();
            document.LoadXml(strXML);
            return document;
    }

        public XmlDocument DeviceStateResponse(List<Device> lDevState, string strebdid)
        {
            model.EBD EDB = new model.EBD();
            EDB.EBRDTState = new List<EBRDT>();
     
            //加入XML的声明段落,Save方法不再xml上写出独立属性
            EDB.EBDVersion = "1.0";
            EDB.EBDID = strebdid;
            EDB.EBDType = "EBRDTState";
            EDB.SRC = new model.SRC();
            EDB.SRC.EBRID = serverini.ReadValue("INFOSET", "HBRONO");
            EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
         
            EDB.SRC.URL = serverini.ReadValue("PLATFORMINFO", "URL"); 

       
       
            string DeviEBRID = sHBRONO.Substring(4, sHBRONO.Length - 6);
            if (lDevState.Count > 0)
            {
                for (int l = 0; l < lDevState.Count; l++)
                {
                    EBRDT EBRDT = new EBRDT();

                
                    EBRDT.EBRID = lDevState[l].DeviceID;
//                    1：开机 / 运行正常
//2：关机 / 停止运行
//3：故障
//4：故障恢复
//5：播发中
                    EBRDT.RptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    if (lDevState[l].DeviceState == "离线")
                    {
                        EBRDT.StateCode = 3;
                    }
                    else if (lDevState[l].DeviceState == "播发中")
                    {
                        EBRDT.StateCode = 5;
                    }
                    else if (lDevState[l].DeviceState == "故障恢复")
                    {
                        EBRDT.StateCode = 4;
                    }
                    else if (lDevState[l].DeviceState == "开机/运行中"|| lDevState[l].DeviceState== "在线")
                    {
                        EBRDT.StateCode = 1;
                    }
                    else
                    {
                        EBRDT.StateCode = 2;
                    }
                    EBRDT.StateDesc = lDevState[l].DeviceState;
              //      EDB.DEST.EBRID = serverini.ReadValue("FORM", "SuperiorPlatform");

                    EDB.EBRDTState.Add(EBRDT);
              
                }
            }
            
                    string strXML = XmlSerialize<model.EBD>(EDB);
            XmlDocument document = new XmlDocument();
            document.LoadXml(strXML);
            //2018-06-02修改
            //XmlDocument xmlDoc = new XmlDocument();
            //#region 标准头部
            ////加入XML的声明段落,Save方法不再xml上写出独立属性
            //xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            ////加入根元素
            //XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            //xmlDoc.AppendChild(xmlElem);
            //XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            //xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            //xmlElem.Attributes.Append(xmlns);

            ////Version
            //XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            //xmlProtocolVer.InnerText = "1.0";
            //xmlElem.AppendChild(xmlProtocolVer);
            ////EBDID
            //XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            //xmlEBDID.InnerText = strebdid;//
            //xmlElem.AppendChild(xmlEBDID);

            ////EBDType
            //XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            //xmlEBDType.InnerText = "EBRDTState";
            //xmlElem.AppendChild(xmlEBDType);

            ////Source
            //XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            //xmlElem.AppendChild(xmlSRC);

            //XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            //xmlSRCAreaCode.InnerText = sHBRONO;
            //xmlSRC.AppendChild(xmlSRCAreaCode);

            ////EBDTime
            //XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");
            //xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //xmlElem.AppendChild(xmlEBDTime);
            //#endregion End

            ////RelatedEBD
            ////XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            ////xmlElem.AppendChild(xmlRelatedEBD);

            ////XmlElement xmlReEBDID = xmlDoc.CreateElement("EBDID");
            ////xmlReEBDID.InnerText = sHBRONO;//与EBDID一致就用这个写
            ////xmlRelatedEBD.AppendChild(xmlReEBDID);

            //#region DeviceInfoReport
            //XmlElement xmlDeviceStateReport = xmlDoc.CreateElement("EBRDTState");
            //xmlElem.AppendChild(xmlDeviceStateReport);


            //Console.WriteLine(DeviEBRID);
            //#region Device
            //if (lDevState.Count > 0)
            //{
            //    for (int l = 0; l < lDevState.Count; l++)
            //    {
            //        XmlElement xmlDevice = xmlDoc.CreateElement("EBRDT");
            //        xmlDeviceStateReport.AppendChild(xmlDevice);

            //        XmlElement xmlDeviceCategory = xmlDoc.CreateElement("RptTime");
            //        xmlDeviceCategory.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //        xmlDevice.AppendChild(xmlDeviceCategory);

            //        XmlElement xmlDeviceID = xmlDoc.CreateElement("EBRID");
            //        xmlDeviceID.InnerText = "0601" + DeviEBRID + lDevState[l].DeviceID;
            //        xmlDevice.AppendChild(xmlDeviceID);

            //        XmlElement xmlDeviceType = xmlDoc.CreateElement("StateCode");
            //        xmlDeviceType.InnerText = "1";
            //        xmlDevice.AppendChild(xmlDeviceType);
            //        XmlElement xmlDeviceName = xmlDoc.CreateElement("StateDesc");
            //        xmlDeviceName.InnerText = "正常";//
            //        xmlDevice.AppendChild(xmlDeviceName);
            //    }
            //}

            return document;
        }


        /// <summary>
        /// 适配器数据 
        /// </summary>
        /// <param name="lAdapterState"></param>
        /// <param name="strebdid"></param>
        /// <param name="RptType"> 数据类型  Full:全量数据，即需要当前数据的副本  Incremental:增量数据</param>
        /// <returns></returns>
        public XmlDocument EBRASInfoResponse(List<Device> lAdapterState, string strebdid,string RptType)
        {
            model.EBD EDB = new model.EBD();
            //加入XML的声明段落,Save方法不再xml上写出独立属性
            EDB.EBDVersion = "1.0";
            EDB.EBDID = strebdid;
            EDB.EBDType = "EBRASInfo";
            EDB.SRC = new model.SRC();
            EDB.SRC.EBRID = serverini.ReadValue("INFOSET", "HBRONO");
            EDB.SRC.URL = serverini.ReadValue("PLATFORMINFO", "URL");
            EDB.EBDTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            EDB.EBRASInfo = new EBRASInfo();
            EDB.EBRASInfo.EBRAS = new List<EBRAS>();
           // EDB.EBRASInfo.EBRPS = new List<EBRAS>();
            if (lAdapterState.Count > 0)
            {
                for (int l = 0; l < lAdapterState.Count; l++)
                {
                    EBRAS EBRAS = new EBRAS();
                    EBRAS.RptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    EBRAS.RptType = RptType;
                    EBRAS.EBRID = lAdapterState[l].EBRID;
                    EBRAS.EBRName = lAdapterState[l].DeviceName;
                    EBRAS.Longitude = lAdapterState[l].Longitude;
                    EBRAS.Latitude = lAdapterState[l].Latitude;
                    EBRAS.URL = lAdapterState[l].URL;
                    EDB.EBRASInfo.EBRAS.Add(EBRAS);

                }
            }

            string strXML = XmlSerialize<model.EBD>(EDB);
            XmlDocument document = new XmlDocument();
            document.LoadXml(strXML);
            return document;
        }


        public XmlDocument SignResponse(string refbdid, string strIssuerID, string strCertSN, string strSignatureValue)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));

            XmlElement xmlElem = xmlDoc.CreateElement("Signature");
            xmlDoc.AppendChild(xmlElem);

            //Version
            XmlElement xmlVersion = xmlDoc.CreateElement("Version");
            xmlVersion.InnerText = "1.0";
            xmlElem.AppendChild(xmlVersion);

            //RelatedEBD
            XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            xmlElem.AppendChild(xmlRelatedEBD);

            XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            xmlEBDID.InnerText = refbdid;
            xmlRelatedEBD.AppendChild(xmlEBDID);

            // SignatureCert
            XmlElement xmlSignatureCert = xmlDoc.CreateElement("SignatureCert");
            xmlElem.AppendChild(xmlSignatureCert);

            XmlElement xmlCertType = xmlDoc.CreateElement("CertType");
            xmlCertType.InnerText = "01";
            xmlSignatureCert.AppendChild(xmlCertType);

            XmlElement xmlIssuerID = xmlDoc.CreateElement("IssuerID");
            xmlIssuerID.InnerText = strIssuerID;
            xmlSignatureCert.AppendChild(xmlIssuerID);

            //CertSN
            XmlElement xmlCertSN = xmlDoc.CreateElement("CertSN");
            xmlCertSN.InnerText = strCertSN;
            xmlSignatureCert.AppendChild(xmlCertSN);


            //SignatureTime
            XmlElement xmlSignatureTime = xmlDoc.CreateElement("SignatureTime");

            double D = DateTime.Now.ToOADate();
            Byte[] Bytes = BitConverter.GetBytes(D);
            String S = BitConverter.ToString(Bytes);

            xmlSignatureTime.InnerText = S;
            xmlElem.AppendChild(xmlSignatureTime);
            //DigestAlgorithm
            XmlElement xmlDigestAlgorithm = xmlDoc.CreateElement("DigestAlgorithm");
            xmlDigestAlgorithm.InnerText = "SM3";
            xmlElem.AppendChild(xmlDigestAlgorithm);
            //SignatureAlgorithm
            XmlElement xmlSignatureAlgorithm = xmlDoc.CreateElement("SignatureAlgorithm");
            xmlSignatureAlgorithm.InnerText = "SM2";
            xmlElem.AppendChild(xmlSignatureAlgorithm);

            XmlElement xmlSignatureValue = xmlDoc.CreateElement("SignatureValue");
            xmlSignatureValue.InnerText = strSignatureValue;
            xmlElem.AppendChild(xmlSignatureValue);



            return xmlDoc;
        }

        public XmlDocument ResponeEBMStateRequrest(string EBMID, string strebdid)
        {
            XmlDocument xmlDoc = new XmlDocument();
            #region 标准头部
            //加入XML的声明段落,Save方法不再xml上写出独立属性
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            //加入根元素
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);
            XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            xmlElem.Attributes.Append(xmlns);

            //Version
            XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            xmlProtocolVer.InnerText = "1.0";
            xmlElem.AppendChild(xmlProtocolVer);
            //EBDID
            XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            xmlEBDID.InnerText = strebdid;
            xmlElem.AppendChild(xmlEBDID);

            //EBDType
            XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            xmlEBDType.InnerText = "EBMStateResponse";
            xmlElem.AppendChild(xmlEBDType);

            //Source
            XmlElement xmlSRC = xmlDoc.CreateElement("SRC");

            xmlElem.AppendChild(xmlSRC);

            XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            xmlSRCAreaCode.InnerText = sHBRONO;//ebdsr.SRC.EBEID;
            xmlSRC.AppendChild(xmlSRCAreaCode);


            //EBDTime
            XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");
            xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlElem.AppendChild(xmlEBDTime);
            #endregion End

            XmlElement xmlEBMStateResponse = xmlDoc.CreateElement("EBMStateResponse");
            xmlElem.AppendChild(xmlEBMStateResponse);

            XmlElement RptTime = xmlDoc.CreateElement("RptTime");
            RptTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//从100000000000开始编号
            xmlEBMStateResponse.AppendChild(RptTime);

            XmlElement xmlEBM = xmlDoc.CreateElement("EBM");
            xmlEBMStateResponse.AppendChild(xmlEBM);

            XmlElement xmlEBMID = xmlDoc.CreateElement("EBMID");
            xmlEBMID.InnerText = EBMID;//从100000000000开始编号
            xmlEBM.AppendChild(xmlEBMID);

            //不加
            //XmlElement xmlRPTTime = xmlDoc.CreateElement("RptTime");
            //xmlRPTTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //xmlEBMStateResponse.AppendChild(xmlRPTTime);

            XmlElement xmlBRDState = xmlDoc.CreateElement("BrdStateCode");
            xmlBRDState.InnerText = "2";
            xmlEBMStateResponse.AppendChild(xmlBRDState);

            XmlElement BrdStateDesc = xmlDoc.CreateElement("BrdStateDesc");
            BrdStateDesc.InnerText = "完成";
            xmlEBMStateResponse.AppendChild(BrdStateDesc);

            return xmlDoc;
        }

        public XmlDocument SendEBM(string EBMID, string MusicName, string MusicDescName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            #region 标准头部
            //加入XML的声明段落,Save方法不再xml上写出独立属性
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            //加入根元素
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);
            XmlAttribute xmlns = xmlDoc.CreateAttribute("xmlns:xs");
            xmlns.Value = "http://www.w3.org/2001/XMLSchema";
            xmlElem.Attributes.Append(xmlns);

            //Version
            XmlElement xmlProtocolVer = xmlDoc.CreateElement("EBDVersion");
            xmlProtocolVer.InnerText = "1.0";
            xmlElem.AppendChild(xmlProtocolVer);
            //EBDID
            XmlElement xmlEBDID = xmlDoc.CreateElement("EBDID");
            xmlEBDID.InnerText = EBMID;
            xmlElem.AppendChild(xmlEBDID);

            //EBDType
            XmlElement xmlEBDType = xmlDoc.CreateElement("EBDType");
            xmlEBDType.InnerText = "EBM";
            xmlElem.AppendChild(xmlEBDType);

            //Source
            XmlElement xmlSRC = xmlDoc.CreateElement("SRC");
            xmlElem.AppendChild(xmlSRC);

            XmlElement xmlSRCAreaCode = xmlDoc.CreateElement("EBRID");
            xmlSRCAreaCode.InnerText = sHBRONO;//ebdsr.SRC.EBEID;
            xmlSRC.AppendChild(xmlSRCAreaCode);


            XmlElement DEST = xmlDoc.CreateElement("DEST");
            xmlElem.AppendChild(DEST);

            XmlElement EBRID = xmlDoc.CreateElement("EBRID");
            EBRID.InnerText = sHBRONO;//ebdsr.SRC.EBEID;
            DEST.AppendChild(EBRID);


            //EBDTime
            XmlElement EBDTime = xmlDoc.CreateElement("EBDTime");
            EBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlElem.AppendChild(EBDTime);
            #endregion End

            XmlElement xmlEBM = xmlDoc.CreateElement("EBM");
            xmlElem.AppendChild(xmlEBM);

            XmlElement EBMVersion = xmlDoc.CreateElement("EBMVersion");
            xmlEBM.AppendChild(EBMVersion);

            XmlElement xmlEBMID = xmlDoc.CreateElement("EBMID");
            xmlEBMID.InnerText = sHBRONO + DateTime.Now.ToString("yyyyMMddHHmm");
            xmlEBM.AppendChild(xmlEBMID);

            XmlElement xmlMesg = xmlDoc.CreateElement("MsgBasicInfo");
            xmlEBM.AppendChild(xmlMesg);

            XmlElement MsgType = xmlDoc.CreateElement("MsgType");
            MsgType.InnerText = "1";
            xmlMesg.AppendChild(MsgType);

            XmlElement SenderName = xmlDoc.CreateElement("SenderName");
            SenderName.InnerText = "江苏省应急平台";
            xmlMesg.AppendChild(SenderName);

            XmlElement SenderCode = xmlDoc.CreateElement("SenderCode");
            SenderCode.InnerText = "010232000000000001";
            xmlMesg.AppendChild(SenderCode);

            XmlElement SentTime = xmlDoc.CreateElement("SentTime");
            SentTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlMesg.AppendChild(SentTime);

            XmlElement EventType = xmlDoc.CreateElement("EventType");
            EventType.InnerText = "11000";
            xmlMesg.AppendChild(EventType);

            XmlElement Severity = xmlDoc.CreateElement("Severity");
            Severity.InnerText = "4";
            xmlMesg.AppendChild(Severity);

            XmlElement StartTime = xmlDoc.CreateElement("StartTime");
            StartTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlMesg.AppendChild(StartTime);

            XmlElement EndTime = xmlDoc.CreateElement("EndTime");
            EndTime.InnerText = DateTime.Now.AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss");
            xmlMesg.AppendChild(EndTime);

            XmlElement LinkTypeSel = xmlDoc.CreateElement("EndTime");
            LinkTypeSel.InnerText = "0";
            xmlMesg.AppendChild(LinkTypeSel);

            XmlElement MsgContent = xmlDoc.CreateElement("MsgContent");
            xmlEBM.AppendChild(MsgContent);

            XmlElement LanguageCode = xmlDoc.CreateElement("LanguageCode");
            LanguageCode.InnerText = "zho";
            MsgContent.AppendChild(LanguageCode);

            XmlElement MsgTitle = xmlDoc.CreateElement("MsgTitle");
            MsgTitle.InnerText = "图南点歌台";
            MsgContent.AppendChild(MsgTitle);

            XmlElement MsgDesc = xmlDoc.CreateElement("MsgDesc");
            MsgDesc.InnerText = MusicName;
            MsgContent.AppendChild(MsgDesc);

            XmlElement AreaCode = xmlDoc.CreateElement("AreaCode");
            AreaCode.InnerText = "320102000000";
            MsgContent.AppendChild(AreaCode);

            XmlElement Auxiliary = xmlDoc.CreateElement("Auxiliary");
            MsgContent.AppendChild(Auxiliary);

            XmlElement AuxiliaryType = xmlDoc.CreateElement("AuxiliaryType");
            AuxiliaryType.InnerText = "2";
            Auxiliary.AppendChild(AuxiliaryType);

            XmlElement AuxiliaryDesc = xmlDoc.CreateElement("AuxiliaryDesc");
            AuxiliaryDesc.InnerText = MusicDescName;
            Auxiliary.AppendChild(AuxiliaryDesc);

            XmlElement Size = xmlDoc.CreateElement("Size");
            Size.InnerText = "0204286";
            Auxiliary.AppendChild(Size);

            XmlElement Digest = xmlDoc.CreateElement("Digest");
            Digest.InnerText = "";
            Auxiliary.AppendChild(Digest);

            return xmlDoc;
        }

    }
}
