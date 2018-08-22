using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace HttpModel
{
  public   class Attestation
    {
        public USBE usb = new USBE();
        public IntPtr phDeviceHandle = (IntPtr)1;
        public void GenerateSignatureFile(string signature, string strPath, string strEBDID,string sysPath)
        {
            if (signature != "1")
            {
                return;
            }

            string sSignFileName = "\\EBDB_" + strEBDID + ".xml";

            using (FileStream SignFs = new FileStream(strPath + sSignFileName, FileMode.Open))
            {
                StreamReader signsr = new StreamReader(SignFs, Encoding.UTF8);
                string xmlsign = signsr.ReadToEnd();
                signsr.Close();
               CombineXML  signrp = new CombineXML(new IniFiles(sysPath));
                XmlDocument xmlSignDoc = new XmlDocument();
                try
                {
                    //对文件进行签名
                    int nDeviceHandle = (int)phDeviceHandle;
                    byte[] pucSignature = Encoding.UTF8.GetBytes(xmlsign);

                    string strSignture = "";
                    string strpucCounter = "";
                    string strpucSignCerSn = "";
                    string nReturn = usb.Platform_CalculateSingature_String(nDeviceHandle, 1, pucSignature, pucSignature.Length, ref strSignture);
                    //生成签名文件
                    string xmlSIGNFileName = "\\EBDS_EBDB_" + strEBDID + ".xml";
                    xmlSignDoc = signrp.SignResponse(strEBDID, strpucCounter, strpucSignCerSn, nReturn);
                    CommonFunc cm = new CommonFunc();
                    cm.SaveXmlWithUTF8NotBOM(xmlSignDoc, strPath + xmlSIGNFileName);
                    if (cm != null)
                    {
                        cm = null;
                    }
                }
                catch (Exception ex)
                {
                    Log.Instance.LogWrite("签名文件错误：" + ex.Message);
                }
            }
        }
    }
}
