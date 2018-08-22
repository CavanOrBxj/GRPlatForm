using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using HttpServerLib;
using System.Xml;
using System.Net.Mime;
using HttpModel;
using static HttpServerLib.HttpRequest;

namespace HttpServer
{


   public class TnHttpServser:HttpServerLib.HttpServer
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="port">端口号</param>
        public TnHttpServser(string ipAddress, int port)
            : base(ipAddress, port)
        {

        }


        ////设置tar包解压路径
        public string DecompressionPath { get; private set; }
        ///设置发送文件保存路径
        public string SendFilePath { get; private set; }
        ///设置心跳保存路径
        public string SaveHeartbeatPath { get; private set; }
        ///设置心跳解压路径
        /// <summary>
        public string DecmpressionHertbeat { get; private set; }

        //                public string sSendTarName = "";//发送Tar包名字
        //public static string sRevTarPath = "";//接收Tar包存放路径
        //public static string sSendTarPath = "";//发送Tar包存放路径
        public  string sSourcePath = "";//需压缩文件路径
        public  string sUnTarPath = "";//Tar包解压缩路径
        public  string sAudioFilesFolder = "";//音频文件存放位置
        //public static string heartbeatPacketStoragePath = "";//心跳包存放路径
        //public static string heartbeatDecompressionPath = "";//心跳包解压路径
        public override void OnPost(HttpRequest request, HttpResponse response)
        {
            IniFiles serverini = new IniFiles(StartupPath + "\\Config.ini");
            string     strHBRONO = serverini.ReadValue("INFOSET", "HBRONO");  //实体编号
            //获取客户端传递的参数
            //string data = request.Params == null ? "" : string.Join(";", request.Params.Select(x => x.Key + "=" + x.Value).ToArray());

            ////设置返回信息
            //string content = string.Format("这是通过Post方式返回的数据:{0}", data);
            Random rd = new Random();
            // string fName = "10" + rp.sHBRONO + "00000000000" + rd.Next(100, 999).ToString();
            string fName = "10" + strHBRONO + "0000000000000" + rd.Next(100, 999).ToString();
            ////构造响应报文

            
         //   response.SetContent(content);
         //response.Content_Encoding = "utf-8";
         //response.StatusCode = "200";
         //response.Content_Type = "text/html; charset=UTF-8";
         //response.Headers["Server"] = "ExampleServer";





            //发送响应
            response.Send();
        }






        public override void OnPost(HttpRequest request, HttpResponse response, string SavePath, string FileName)
        {
            string UnpackTarPath = "";
            string FileSavePath = SavePath + "\\" + FileName;
            UnpackTarPath = sUnTarPath + "\\" + FileName.Split('.')[0];
            if (File.Exists(FileSavePath))
            {
                if (!Directory.Exists(UnpackTarPath))
                {
                    Directory.CreateDirectory(UnpackTarPath);
                }
                TarHelper tar = new TarHelper();
                tar.DeleteFolder(UnpackTarPath);

                tar.UnpackTarFiles(FileSavePath, UnpackTarPath);


                string[] xmlfilenames = Directory.GetFiles(UnpackTarPath, "*.xml");//从解压XML文件夹下获取解压的XML文件名
                string sTmpFile = string.Empty;
                string sAnalysisFileName = "";
                string sSignFileName = "";
                for (int i = 0; i < xmlfilenames.Length; i++)
                {
                    sTmpFile = Path.GetFileName(xmlfilenames[i]);
                    if (sTmpFile.ToUpper().IndexOf("EBDB") > -1 && sTmpFile.ToUpper().IndexOf("EBDS_EBDB") < 0)
                    {
                        sAnalysisFileName = xmlfilenames[i];
                    }
                    //else if (sTmpFile.ToUpper().IndexOf("EBDS_EBDB") > -1)//签名文件
                    //{
                    //    sSignFileName = xmlfilenames[i];//签名文件
                    //}
                }
                EBD ebd = null;
                try
                {
                    using (FileStream fstream = new FileStream(sAnalysisFileName, FileMode.Open))
                    {
                        StreamReader sr = new StreamReader(fstream, System.Text.Encoding.UTF8);
                        String xmlInfo = sr.ReadToEnd();
                        xmlInfo = xmlInfo.Replace("xmlns:xs", "xmlns");
                        sr.Close();
                        xmlInfo = XmlSerialize.ReplaceLowOrderASCIICharacters(xmlInfo);
                        xmlInfo = XmlSerialize.GetLowOrderASCIICharacters(xmlInfo);
                        ebd = XmlSerialize.DeserializeXML<EBD>(xmlInfo);
                    }

                    IniFiles serverini = new IniFiles(StartupPath + "\\Config.ini");
                    CombineXML cx = new CombineXML(serverini);
                    string strHBRONO = serverini.ReadValue("INFOSET", "HBRONO");  //实体编号
                    Random rd = new Random();
                    // string fName = "10" + rp.sHBRONO + "00000000000" + rd.Next(100, 999).ToString();
                    string fName = "10" + strHBRONO + "0000000000000" + rd.Next(100, 999).ToString();
                    XmlDocument xmlDoc = cx.CombineResponse(ebd, "EBDResponse", fName);
                    string xmlSignFileName = "\\EBDB_" + fName + ".xml";


                    string BeXmlFilesPath = serverini.ReadValue("FolderSet", "BeXmlFileMakeFolder");
                    tar.DeleteFolder(BeXmlFilesPath);//新增20180816


                    cx.CreateXML(xmlDoc, BeXmlFilesPath + xmlSignFileName);

                    //进行签名

                    string m_UsbPwsSupport = serverini.ReadValue("USBPSW", "USBPSWSUPPART");
                    Attestation Attestation = new Attestation();
                    // TarHelper tar = new TarHelper();
                    //     ServerForm.mainFrm.AudioGenerateSignatureFile(ServerForm.strBeSendFileMakeFolder,"EBDI",fName);
                    //   Attestation.GenerateSignatureFile(m_UsbPwsSupport, SendFilePath, fName, StartupPath + "\\Config.ini");  测试注释 20180814
                    tar.CreatTar(serverini.ReadValue("FolderSet", "BeXmlFileMakeFolder"), SendFilePath, fName);//使用新TAR
                    string sSendTarName = SendFilePath + "\\EBDT_" + fName + ".tar";


                    response.SendNew(sSendTarName);
                }
                catch (Exception ex)
                {
                    HttpModel.Log.Instance.LogWrite("异常：" + ex.Message);
                }
            }
        }


        public byte[] StreamToBytes(Stream stream)

        {

            byte[] bytes = new byte[stream.Length];

            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始 

            stream.Seek(0, SeekOrigin.Begin);

            return bytes;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=" SavePath">tar包保存路径</param>
        /// <param name="SaveFilePath">设置发送文件保存路径</param>
        /// <param name="SaveHeartbeatPath">设置心跳保存路径</param>
        /// <param name="DecompressionPath">设置tar包解压路径</param>
        /// <param name="DecmpressionHertbeat">设置心跳解压路径></param>
        public void SetPath(string SavePath, string SendFilePath, string SaveHeartbeatPath, string DecompressionPath, string DecmpressionHertbeat, string sSourcePath,//需压缩文件路径
            string sUnTarPath,//Tar包解压缩路径
            string sAudioFilesFolder)//音频文件存放位置)
        {
            this.SavePath = SavePath;
            ////设置tar包解压路径
            this.DecompressionPath = DecompressionPath;
            ///设置发送文件保存路径
            this.SendFilePath = SendFilePath;
            ///设置心跳保存路径
            this.SaveHeartbeatPath = SaveHeartbeatPath;
            ///设置心跳解压路径
            /// <summary>
            this.DecmpressionHertbeat = DecmpressionHertbeat;
  
            this.sSourcePath = sSourcePath;//需压缩文件路径
            this.sUnTarPath = sUnTarPath;//Tar包解压缩路径
            this.sAudioFilesFolder = sAudioFilesFolder;//音频文件存放位置
        }
        public override void OnGet(HttpRequest request, HttpResponse response)
        {

            ///链接形式1:"http://localhost:4050/assets/styles/style.css"表示访问指定文件资源，
            ///此时读取服务器目录下的/assets/styles/style.css文件。

            ///链接形式1:"http://localhost:4050/assets/styles/"表示访问指定页面资源，
            ///此时读取服务器目录下的/assets/styles/style.index文件。

            //当文件不存在时应返回404状态码
            string requestURL = request.URL;
            requestURL = requestURL.Replace("/", @"\").Replace("\\..", "").TrimStart('\\');
            string requestFile = Path.Combine(ServerRoot, requestURL);

            //判断地址中是否存在扩展名
            string extension = Path.GetExtension(requestFile);

            //根据有无扩展名按照两种不同链接进行处
            //if (extension != "")
            //{
            //    //从文件中返回HTTP响应
            //    response = response.FromFile(requestFile);
            //}
            //else
            //{
            //    //目录存在且不存在index页面时时列举目录
            //    if (Directory.Exists(requestFile) && !File.Exists(requestFile + "\\index.html"))
            //    {
            //        requestFile = Path.Combine(ServerRoot, requestFile);
            //        var content = ListDirectory(requestFile, requestURL);
            //        response = response.SetContent(content, Encoding.UTF8);
            //        response.Content_Type = "text/html; charset=UTF-8";
            //    }
            //    else
            //    {
            //        //加载静态HTML页面
            //        requestFile = Path.Combine(requestFile, "index.html");
            //        response = response.FromFile(requestFile);
            //        response.Content_Type = "text/html; charset=UTF-8";
            //    }
            //}

            //发送HTTP响应
            response.Send();
        }

        public override void OnDefault(HttpRequest request, HttpResponse response)
        {

        }

        private string ConvertPath(string[] urls)
        {
            string html = string.Empty;
            int length = ServerRoot.Length;
            foreach (var url in urls)
            {
                var s = url.StartsWith("..") ? url : url.Substring(length).TrimEnd('\\');
                html += String.Format("<li><a href=\"{0}\">{0}</a></li>", s);
            }

            return html;
        }

        private string ListDirectory(string requestDirectory, string requestURL)
        {
            //列举子目录
            var folders = requestURL.Length > 1 ? new string[] { "../" } : new string[] { };
            folders = folders.Concat(Directory.GetDirectories(requestDirectory)).ToArray();
            var foldersList = ConvertPath(folders);

            //列举文件
            var files = Directory.GetFiles(requestDirectory);
            var filesList = ConvertPath(files);

            //构造HTML
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("<html><head><title>{0}</title></head>", requestDirectory));
            builder.Append(string.Format("<body><h1>{0}</h1><br/><ul>{1}{2}</ul></body></html>",
                 requestURL, filesList, foldersList));

            return builder.ToString();
        }
    }
}
