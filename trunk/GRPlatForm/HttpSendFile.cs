using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Collections;

namespace GRPlatForm
{
    public class HttpSendFile
    {
        static string sSeparateString = "";
        public Thread HttpSendFieTh = null;
        SendFileSon FSon = new SendFileSon();
        static int MAX_POST_SIZE = 100 * 1024 * 1024;
        static int BUF_SIZE = 4096;
        private static Hashtable httpHeaders = new Hashtable();

        public class SendFileSon
        {
            public string AddressUrl { get; set; }
            public string FileName { get; set; }
        }

        /// <summary>   
        /// 将本地文件上传到指定的服务器(HttpWebRequest方法)   
        /// </summary>   
        /// <param name="address">文件上传到的服务器</param>   
        /// <param name="fileNamePath">要上传的本地文件（全路径）</param>   
        /// <param name="saveName">文件上传后的名称</param>    
        /// <returns>成功返回1，失败返回0</returns>   
        public static string UploadFilesByPost(string address, string fileNamePath)
        {
            try
            {
                int u = ServicePointManager.DefaultConnectionLimit;
                ServicePointManager.DefaultConnectionLimit = 200;

                string returnValue = "0";     // 要上传的文件     
                WebResponse webRespon = null;

                FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(fs);     //时间戳

                string sguidSplit = Guid.NewGuid().ToString();
                string filename = fileNamePath.Substring(fileNamePath.LastIndexOf("\\") + 1);

                StringBuilder sb = new StringBuilder(300);

                string strPostHeader = sb.ToString();
                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(address));

                httpReq.ServicePoint.Expect100Continue = false;
                httpReq.Method = "POST";     //对发送的数据不使用缓存
                httpReq.AllowWriteStreamBuffering = false;     //设置获得响应的超时时间（300秒）
                httpReq.Timeout = 60000;
                httpReq.ContentType = "multipart/form-data; boundary=" + sguidSplit;//"text/xml";
                httpReq.Accept = "text/plain, */*";
                httpReq.UserAgent = "WinHttpClient";

                httpReq.Headers["Accept-Language"] = "zh-cn";

                sb.Append("--" + sguidSplit + "\r\n");
                sb.Append("Content-Disposition: form-data; name=\"file\"; filename=\"" + filename + "\"\r\n");
                sb.Append("Content-Type: application/octet-stream;Charset=UTF-8\r\n");
                sb.Append("\r\n");

                byte[] boundaryBytes = Encoding.ASCII.GetBytes(sb.ToString());     //请求头部信息  
                byte[] bEndBytes = Encoding.ASCII.GetBytes("\r\n--" + sguidSplit + "--\r\n");
                long length = fs.Length + boundaryBytes.Length + bEndBytes.Length;
                long fileLength = fs.Length;
                httpReq.ContentLength = length;

                try
                {
                    int bufferLength = 4096;//每次上传4k  
                    byte[] buffer = new byte[bufferLength]; //已上传的字节数   
                    long offset = 0;         //开始上传时间   
                    DateTime startTime = DateTime.Now;

                    int size = r.Read(buffer, 0, bufferLength);
                    Stream postStream = httpReq.GetRequestStream();         //发送请求头部消息   
                    postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                    while (size > 0)
                    {
                        postStream.Write(buffer, 0, size);
                        offset += size;
                        TimeSpan span = DateTime.Now - startTime;
                        double second = span.TotalSeconds;
                        Application.DoEvents();
                        size = r.Read(buffer, 0, bufferLength);
                    }
                    //添加尾部的时间戳 
                    postStream.Write(bEndBytes, 0, bEndBytes.Length);
                    postStream.Close();
                    //获取服务器端的响应   
                    webRespon = httpReq.GetResponse();   //提示操作超时  20180105
                    Stream s = webRespon.GetResponseStream();
                    //读取服务器端返回的消息  
                    StreamReader sr = new StreamReader(s);
                    String sReturnString = sr.ReadLine();
                    //while (true)
                    //{

                    //}
                    s.Close();
                    sr.Close();
                    returnValue = "1";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    returnValue = "0";
                }
                finally
                {
                    fs.Close();
                    r.Close();
                    if (httpReq != null)
                    {
                        httpReq.Abort();
                    }
                    if (webRespon != null)
                    {
                        webRespon.Close();
                    }
                    GC.Collect();
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Thread.Sleep(500);
            }
            return "0";
        }

        public static model.EBDResponse UploadFilesByPost(string address, string fileNamePath, int state)
        {
            try
            {
                model.EBDResponse Res = new model.EBDResponse();
                int u = ServicePointManager.DefaultConnectionLimit;
                ServicePointManager.DefaultConnectionLimit = 200;

                string returnValue = "0";     // 要上传的文件     
                WebResponse webRespon = null;

                FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(fs);     //时间戳

                string sguidSplit = Guid.NewGuid().ToString();
                string filename = fileNamePath.Substring(fileNamePath.LastIndexOf("\\") + 1);

                StringBuilder sb = new StringBuilder(300);

                string strPostHeader = sb.ToString();
                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(address));

                httpReq.ServicePoint.Expect100Continue = false;
                httpReq.Method = "POST";     //对发送的数据不使用缓存
                httpReq.AllowWriteStreamBuffering = false;     //设置获得响应的超时时间（300秒）
                httpReq.Timeout = 80000;
                httpReq.ContentType = "multipart/form-data; boundary=" + sguidSplit;//"text/xml";
                httpReq.Accept = "text/plain, */*";
                httpReq.UserAgent = "WinHttpClient";

                httpReq.Headers["Accept-Language"] = "zh-cn";

                sb.Append("--" + sguidSplit + "\r\n");
                sb.Append("Content-Disposition: form-data; name=\"file\"; filename=\"" + filename + "\"\r\n");
                sb.Append("Content-Type: application/octet-stream;Charset=UTF-8\r\n");
                sb.Append("\r\n");

                byte[] boundaryBytes = Encoding.ASCII.GetBytes(sb.ToString());     //请求头部信息  
                byte[] bEndBytes = Encoding.ASCII.GetBytes("\r\n--" + sguidSplit + "--\r\n");
                long length = fs.Length + boundaryBytes.Length + bEndBytes.Length;
                long fileLength = fs.Length;
                httpReq.ContentLength = length;

                try
                {
                    int bufferLength = 4096;//每次上传4k  
                    byte[] buffer = new byte[bufferLength]; //已上传的字节数   
                    long offset = 0;         //开始上传时间   
                    DateTime startTime = DateTime.Now;

                    int size = r.Read(buffer, 0, bufferLength);
                    Stream postStream = httpReq.GetRequestStream();         //发送请求头部消息   
                    postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                    while (size > 0)
                    {
                        postStream.Write(buffer, 0, size);
                        offset += size;
                        TimeSpan span = DateTime.Now - startTime;
                        double second = span.TotalSeconds;
                        Application.DoEvents();
                        size = r.Read(buffer, 0, bufferLength);
                    }
                    //添加尾部的时间戳 
                    postStream.Write(bEndBytes, 0, bEndBytes.Length);
                    postStream.Close();
                    //获取服务器端的响应   
                    Encoding encoding = Encoding.UTF8;//根据网站的编码自定义
                    webRespon = httpReq.GetResponse();   //提示操作超时  20180105
                    Stream responseStream = webRespon.GetResponseStream();
                    string str = streamReadLine(responseStream);

                    if (str != "")
                    {
                        string xmlInfo = GetResponseXml(str);

                        xmlInfo = XmlSerialize.GetLowOrderASCIICharacters(xmlInfo);
                        model.EBD ebd = XmlSerialize.DeserializeXML<model.EBD>(xmlInfo);
                        if (ebd != null)
                        {

                            Res.ResultCode = ebd.EBDResponse.ResultCode;

                            Res.ResultDesc = ebd.EBDResponse.ResultDesc;
                        }

                        responseStream.Close();
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                  
                }
                finally
                {
                    fs.Close();
                    r.Close();
                    if (httpReq != null)
                    {
                        httpReq.Abort();
                    }
                    if (webRespon != null)
                    {
                        webRespon.Close();
                    }
                    GC.Collect();
                }
                return Res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Thread.Sleep(500);
            }
            return null;
        }
        /// <summary>
        ///单行读取 2018-06-26更新
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        private static string streamReadLine(Stream inputStream)
        {
            string data = "";
            int next_char;
            byte[] readdata;
            while (true)
            {
                readdata = new byte[2048];
                int number = inputStream.Read(readdata, 0, readdata.Length);
                //next_char = inputStream.ReadByte();
                //if (next_char == '\n') { break; }
                //if (next_char == '\r') { continue; }
                //if (next_char == -1) { Thread.Sleep(1); continue; };
                //data += Convert.ToChar(next_char);
                if (number > 0)
                {
                    data += Encoding.UTF8.GetString(readdata);
                }
                else
                {
                    break;
                }
            }

            return data;
        }

        private static string GetResponseXml(string str)
        {
            string[] Array = str.Split('\0');
            StringBuilder builder = new StringBuilder();
            int count = 0;
            foreach (var item in Array)
            {
                if (item.IndexOf(".xml") > -1)
                {
                    count++;
                }
                if (count > 1)
                {
                    break;
                }

                if (!string.IsNullOrEmpty(item))
                {
                    if (item.IndexOf("<?") > -1)

                        builder.Append(item);
                }

            }
            return builder.ToString();
        }
        /// <summary>
        /// 获取http头 2018-06-26
        /// </summary>
        /// <param name="sr"></param>
        public static void readHeaders(Stream sr)
        {
            Console.WriteLine("readHeaders()");
            string line;
            sSeparateString = string.Empty;//初始化接收
            while ((line = streamReadLine(sr)) != null)
            {
                if (line.Equals(""))
                {
                    Console.WriteLine("got headers");
                    return;
                }

                int separator = line.IndexOf(':');
                if (separator == -1)
                {
                    if (line != "platformtype" && (sSeparateString != "" && !line.Contains(sSeparateString)))
                    {
                        if (line == "" || line == string.Empty)
                        {
                            return;//结束头部
                        }
                        else
                        {
                            Console.WriteLine("头部验证出错!");
                            return;
                        }
                    }
                    else
                    {
                        //Console.WriteLine(line);
                        continue;
                    }
                }
                String name = line.Substring(0, separator);

                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++; // strip any spaces
                }
                string value = line.Substring(pos, line.Length - pos);
                if (name == "Content-Type" && sSeparateString == "")
                {
                    string[] sSeparateVaule = value.Split('=');
                    if (sSeparateVaule.Length > 1)
                    {
                        if (sSeparateVaule[1].IndexOf(";") > -1)
                        {
                            sSeparateString = sSeparateVaule[1].Split(';')[0];
                        }
                        else
                        {
                            sSeparateString = sSeparateVaule[1];
                        }
                    }
                }
                Console.WriteLine("头部: {0}:{1}", name, value);
                httpHeaders[name] = value;
            }
        }
        public static string UploadFilesByPostNoSplit(string address, string fileNamePath)
        {
            string returnValue = "0";     // 要上传的文件   
            FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);     //时间戳   

            string sguidSplit = Guid.NewGuid().ToString();
            string filename = fileNamePath.Substring(fileNamePath.LastIndexOf("\\") + 1);
            StringBuilder sb = new StringBuilder(300);
            string strPostHeader = sb.ToString();
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(address));
            httpReq.Method = "POST";     //对发送的数据不使用缓存   
            httpReq.AllowWriteStreamBuffering = false;     //设置获得响应的超时时间（300秒）   
            httpReq.Timeout = 30000;
            httpReq.ContentType = "multipart/form-data; boundary=" + sguidSplit;//"text/xml";// 
            httpReq.Accept = "text/plain, */*";
            httpReq.UserAgent = "WinHttpClient";

            httpReq.Headers["Accept-Language"] = "zh-cn";

            sb.Append("--" + sguidSplit + "\r\n");
            sb.Append("Content-Disposition: form-data;filename=\"" + filename + "\"\r\n");
            sb.Append("Content-Type: application/x-tar\r\n");
            sb.Append("\r\n");

            byte[] boundaryBytes = Encoding.ASCII.GetBytes(sb.ToString());     //请求头部信息  
            byte[] bEndBytes = Encoding.ASCII.GetBytes("\r\n--" + sguidSplit + "--\r\n");
            long length = fs.Length + boundaryBytes.Length + bEndBytes.Length;
            long fileLength = fs.Length;
            httpReq.ContentLength = length;
            try
            {
                int bufferLength = 4096;//每次上传4k  
                byte[] buffer = new byte[bufferLength]; //已上传的字节数   
                long offset = 0;         //开始上传时间   
                DateTime startTime = DateTime.Now;

                int size = r.Read(buffer, 0, bufferLength);
                Stream postStream = httpReq.GetRequestStream();         //发送请求头部消息   
                postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                while (size > 0)
                {
                    postStream.Write(buffer, 0, size);
                    offset += size;
                    TimeSpan span = DateTime.Now - startTime;
                    double second = span.TotalSeconds;
                    Application.DoEvents();
                    size = r.Read(buffer, 0, bufferLength);
                }
                //添加尾部的时间戳 
                postStream.Write(bEndBytes, 0, bEndBytes.Length);
                postStream.Close();         //获取服务器端的响应   
                WebResponse webRespon = httpReq.GetResponse();
                Stream s = webRespon.GetResponseStream();
                //读取服务器端返回的消息  
                StreamReader sr = new StreamReader(s);
                String sReturnString = sr.ReadLine();
                s.Close();
                sr.Close();
                returnValue = "1";
                Console.WriteLine(sReturnString);

            }
            catch
            {
                returnValue = "0";
            }
            finally
            {
                fs.Close();
                r.Close();
            }
            return returnValue;
        }
        //
        /// <summary>
        /// 解析tar2018-06-26
        /// </summary>
        /// <param name="sr">数据流</param>
        /// <param name="path">文件路径</param>
        /// <param name="tarName">tar包名称</param>
        public static void handlePOSTRequest(Stream sr, string path, string tarName)
        {
            Console.WriteLine("get post data start");
            int content_len = 0;
            MemoryStream ms = new MemoryStream();
            if (httpHeaders.ContainsKey("Content-Length"))
            {
                content_len = Convert.ToInt32(httpHeaders["Content-Length"]);
                if (content_len > MAX_POST_SIZE)
                {
                    Console.WriteLine(string.Format("POST Content-Length({0}) too big for this simple server", content_len));
                    return;
                }
                byte[] buf = new byte[BUF_SIZE];
                int to_read = content_len;
                while (to_read > 0)
                {
                    int numread = sr.Read(buf, 0, Math.Min(BUF_SIZE, to_read));
                    if (numread == 0)
                    {
                        if (to_read == 0)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("client disconnected during post");
                        }
                    }
                    to_read -= numread;
                    ms.Write(buf, 0, numread);
                }
                ms.Seek(0, SeekOrigin.Begin);
            }
            Console.WriteLine("get post data end");
            byte[] byteData = new byte[ms.Length];
            ms.Read(byteData, 0, byteData.Length);
            ms.Seek(0, SeekOrigin.Begin);
            // 把 byte[] 写入文件
            FileStream fsd = new FileStream(path + "\\" + tarName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fsd);
            bw.Write(byteData);
            bw.Close();
            fsd.Close();

            //  PostRequestDeal(new StreamReader(ms), ServerForm.sRevTarPath);
            return;

            #region xx
            //Console.WriteLine("获取POST数据开始:get post data start");
            //string sFileForldPath = ServerForm.sRevTarPath;// ServerForm.sTarPathName.Substring(0, ServerForm.sTarPathName.LastIndexOf("\\"));//接收文件夹路径
            //Console.WriteLine("204:" + sFileForldPath);
            //string sFilePath = string.Empty;
            //string revfilename = string.Empty;//接收到的文件名
            //int content_len = 0;
            //int iFileLen = -1;
            //if (this.httpHeaders.ContainsKey("Content-Length"))
            //{
            //    content_len = Convert.ToInt32(this.httpHeaders["Content-Length"]);
            //    if (content_len > MAX_POST_SIZE)
            //    {
            //        Console.WriteLine(String.Format("POST Content-Length({0}) too big for this simple server", content_len));
            //    }
            //    byte[] buf = new byte[BUF_SIZE];
            //    List<byte> lListData = new List<byte>();
            //    string dealFilePath = "";
            //    int to_read = content_len;
            //    if (to_read > 0)
            //    {
            //        string DataLine;
            //        FileStream fs = null;
            //        try
            //        {
            //            while ((DataLine = streamDataReadLine(inputStream, ref lListData)) != null)
            //            {
            //                if (DataLine.Equals("--" + sSeparateString) && sSeparateString != "")
            //                {
            //                    continue;
            //                }
            //                else if (DataLine.Equals("--" + sSeparateString + "--") && sSeparateString != "")
            //                {
            //                    if (fs != null)
            //                        fs.Close();
            //                    bool verifySuccess = false;
            //                    DealTarBack(dealFilePath, out verifySuccess);//处理接收文件      2016-04-11 与下一句调换顺序
            //                    if (verifySuccess)
            //                        ServerForm.lRevFiles.Add(sFilePath);//完成接收文件后把文件增加到处理列表上去
            //                }
            //                else if (DataLine.Contains("Content-Disposition"))
            //                {
            //                    string[] sSeparateVaule = DataLine.Split('=');
            //                    if (sSeparateVaule.Length > 1)
            //                    {
            //                        revfilename = sSeparateVaule[sSeparateVaule.Length - 1];//文件名
            //                        if (revfilename != "")
            //                        {
            //                            revfilename = revfilename.Replace("\"", "");
            //                            sFilePath = sFileForldPath + "\\" + revfilename;
            //                        }
            //                        else
            //                        {
            //                            sFilePath = sFileForldPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".tar";
            //                        }
            //                        if (fs != null)
            //                        {
            //                            fs.Close();
            //                        }
            //                        else
            //                        {
            //                            fs = new FileStream(sFilePath, FileMode.Create, FileAccess.Write); //打开一个写入流
            //                            iFileLen = 0;
            //                        }
            //                        dealFilePath = sFilePath;
            //                        revfilename = string.Empty;
            //                    }
            //                    continue;
            //                }
            //                else if (DataLine.Contains("Content-Type"))
            //                {
            //                    continue;
            //                }
            //                else //if (DataLine.Length > 0 || lListData.Count> 0)
            //                {
            //                    //为数据内容
            //                    if (DataLine.Length == 0 && iFileLen == 0)
            //                        continue;
            //                    iFileLen += lListData.Count;
            //                    fs.Write(lListData.ToArray(), 0, lListData.Count);
            //                    fs.Flush();
            //                }
            //            }
            //        }
            //        catch (Exception em)
            //        {
            //            Console.WriteLine("295行：" + em.Message);
            //        }
            //        finally
            //        {
            //            if (fs != null)
            //            {
            //                fs.Close();
            //            }
            //        }
            //    }
            //    Console.WriteLine("接收Tar文件成功！");
            //    //writeSuccess();
            //}
            //Console.WriteLine("get post data end");
            #endregion
        }
    }
}
