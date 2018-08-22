using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace HttpModel
{
   public class CombineXML
    {
        IniFiles ini = null;

        public CombineXML(IniFiles ini)
        {
            this.ini = ini;
        }
        /// <summary>
        /// 通用反馈
        /// </summary>
        /// <param name="ebd">接收包关联数据</param>
        /// <param name="EBDstyle"></param>
        /// <param name="strEBDID"></param>
        /// <returns></returns>
        public XmlDocument CombineResponse(EBD ebd, string EBDstyle, string strEBDID)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //加入XML的声明段落,Save方法不再xml上写出独立属性GB2312
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            XmlElement xmlElem = xmlDoc.CreateElement("", "EBD", "");
            xmlDoc.AppendChild(xmlElem);
            xmlHead(xmlDoc, xmlElem, ebd, EBDstyle, strEBDID);

            XmlElement xmlRelatedEBD = xmlDoc.CreateElement("RelatedEBD");
            xmlElem.AppendChild(xmlRelatedEBD);

            XmlElement xmlReEBDID = xmlDoc.CreateElement("EBDID");
            xmlReEBDID.InnerText = ebd.EBDID;
            xmlRelatedEBD.AppendChild(xmlReEBDID);

            XmlElement xmlEBDResponse = xmlDoc.CreateElement("EBDResponse");
            xmlElem.AppendChild(xmlEBDResponse);

            XmlElement xmlResultCode = xmlDoc.CreateElement("ResultCode");
            xmlResultCode.InnerText = "1";
            xmlEBDResponse.AppendChild(xmlResultCode);

            XmlElement xmlResultDesc = xmlDoc.CreateElement("ResultDesc");
            xmlResultDesc.InnerText = "接收解析及数据校验成功";
            xmlEBDResponse.AppendChild(xmlResultDesc);
            return xmlDoc;
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
        public void CreateXML(XmlDocument XD, string Path)
        {
            CommonFunc ComX = new CommonFunc();
            ComX.SaveXmlWithUTF8NotBOM(XD, Path);
            if (ComX != null)
            {
                ComX = null;
            }
        }
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


            XmlElement xmlSRCEBRID = xmlDoc.CreateElement("EBRID");
            //  xmlSRCEBRID.InnerText = ini.ReadValue("INFOSET", "ADAPTERNO");
            xmlSRCEBRID.InnerText = ini.ReadValue("INFOSET", "HBRONO");
            xmlSRC.AppendChild(xmlSRCEBRID);




            //dest
            XmlElement xmlDEST = xmlDoc.CreateElement("DEST");
            xmlElem.AppendChild(xmlDEST);
        

            XmlElement xmlSRCAreaCode1 = xmlDoc.CreateElement("EBRID");
            xmlSRCAreaCode1.InnerText = ini.ReadValue("FORM", "Superior");
            xmlDEST.AppendChild(xmlSRCAreaCode1);



            XmlElement xmlEBDTime = xmlDoc.CreateElement("EBDTime");
            xmlEBDTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            xmlElem.AppendChild(xmlEBDTime);
            #endregion End
            return 0;
        }

    }
}
