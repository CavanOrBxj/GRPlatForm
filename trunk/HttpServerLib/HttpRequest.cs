using ConsumptionQueue;
using HttpModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HttpModel;
using System.Windows.Forms;

using System.Web;
using System.Net;

namespace HttpServerLib
{
  public  class HttpRequest: HttpPostedFileBase
    {
        

        /// <summary>
        /// URL参数
        /// </summary>
        public Dictionary<string, string> Params { get; private set; }

        /// <summary>
        /// HTTP请求方式
        /// </summary>
        public string Method { get; private set; }

        /// <summary>
        /// HTTP(S)地址
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// HTTP协议版本
        /// </summary>
        public string ProtocolVersion { get; set; }


        private HttpListenerRequest request;


        /// <summary>
        /// 定义缓冲区
        /// </summary>
        private const int MAX_POST_SIZE = 1024 * 1024 * 100;

        private string sSeparateString = string.Empty;
        private int BUF_SIZE = 4096;
        public Hashtable httpHeaders = new Hashtable();
        public ILogger Logger { get; set; }
        public HttpServer httpServer { get; set; }
        private Stream handler;
        string http_method = "";
        string http_url="";
        string http_protocol_versionstring = "";


        public HttpRequest(Stream stream)
        {
            this.handler = stream;
            //    var data = GetRequestData(handler);
            //var rows = Regex.Split(data, Environment.NewLine);

            ////Request URL & Method & Version
            //var first = Regex.Split(rows[0], @"(\s+)")
            //    .Where(e => e.Trim() != string.Empty)
            //    .ToArray();
            //if (first.Length > 0) this.Method = first[0];
            //if (first.Length > 1) this.URL = Uri.UnescapeDataString(first[1]).Split('?')[0];
            //if (first.Length > 2) this.ProtocolVersion = first[2];

            //Request Headers
            // this.Headers = GetRequestHeaders(rows);
         
       
            readHeaders();
            if (this.Method == "GET")
            {
            }
            else if(this.Method == "POST")
            {
              //  GetRequestData(stream,null,null);
            }
        }
        public HttpRequest(Stream stream,string SaveFile)
        {
            this.handler = stream;


            //Request Headers
            // this.Headers = GetRequestHeaders(rows);
            byte []data = new byte[MAX_POST_SIZE];
          //  SetMethod(handler,ref data);


            readHeaders();
            string fileName = "";
            if (this.Method == "GET")
            {
            }
            else if (this.Method == "POST")
            {
              
                //["Content-Disposition"] = "form-data; name=\"file\"; filename=\"EBDT_10434152300000001030101010000000000001546.tar\""
               // GetRequestData(handler, SaveFile, fileName = GetFileName(httpHeaders["Content-Disposition"].ToString()));
            }
        }
        /// <summary>
        /// 验证处理请求
        /// </summary>
        /// <returns>处理成功标志</returns>
        public bool parseRequest(Stream inputStream)
        {
            string request = streamReadLine(inputStream);
            string[] tokens = request.Split(' ');
            if (tokens.Length != 3)
            {
                Log.Instance.LogWrite("头部验证错误，无法解析，丢弃处理！");
                Console.WriteLine("头部验证错误，无法解析，丢弃处理！");
                return false;
            }
            Method = tokens[0].ToUpper();
            http_method = tokens[0].ToUpper();
            http_url = tokens[1];
            http_protocol_versionstring = tokens[2];
          //  Console.WriteLine("头部验证字符串：" + request);
            return true;
        }
        public HttpRequest(Stream stream, string SaveFile,HttpServer httpServer)
        {


            this.handler = stream;
            if (parseRequest(stream) == false)
            {
                this.handler = null;
            }

            //Request Headers
            // this.Headers = GetRequestHeaders(rows);

            string[] builder;
       

            readHeaders();
         //   SetMethod(handler, ref data);


      
            string fileName = "";
            if (http_method == "GET")
            {
            }
            else if (http_method == "POST")
            {
                GetRequestData(SaveFile);


            }
        }
        //public HttpRequest(Stream stream, string SaveFile, HttpServer httpServer,ref byte[] Readdata)
        //{
        //    this.handler = stream;


        //    //Request Headers
        //    // this.Headers = GetRequestHeaders(rows);
        //    byte[] data = new byte[MAX_POST_SIZE];
        //    SetMethod(handler, ref data);

            
        //    readHeaders(byte2stream(data));
        //    string fileName = "";
        //    if (this.Method == "GET")
        //    {
        //    }
        //    else if (this.Method == "POST")
        //    {

        //        //["Content-Disposition"] = "form-data; name=\"file\"; filename=\"EBDT_10434152300000001030101010000000000001546.tar\""
        //        GetRequestData(byte2stream(data), SaveFile, fileName = GetFileName(httpHeaders["Content-Disposition"].ToString()));
        //    }
        //}
        #region 公有方法

        #endregion
        #region 私有方法
       public string GetFileName(string Content_Disposition)
        {
            var first = Regex.Split(Content_Disposition.Replace("\"",""), ";");
            foreach (var item in first)
            {
                if (item.ToUpper().IndexOf("FILENAME")>-1)
                {
                    return item.Split('=')[1];
                }
            }

            return first[0];
        }

        private Stream byte2stream(byte[] buffer)
        {
            Stream stream = new MemoryStream(buffer);
            stream.Seek(0, SeekOrigin.Begin);
            //设置stream的position为流的开始
            return stream;
        }
        //public void SetMethod(Stream stream,ref byte[] readData)
        //{
        //    var data = GetRequestData(stream,ref readData);
        //    string [] rows = Regex.Split(data, Environment.NewLine);
        //    readData = Encoding.UTF8.GetBytes(rows[14]);
        //    //Request URL & Method & Version
        //    var first = Regex.Split(rows[0], @"(\s+)")
        //        .Where(e => e.Trim() != string.Empty)
        //        .ToArray();
        //    if (first.Length > 0) this.Method = first[0];
        //    if (first.Length > 1) this.URL = Uri.UnescapeDataString(first[1]).Split('?')[0];
        //    if (first.Length > 2) this.ProtocolVersion = first[2];
        //}
        //public void SetMethod(Stream stream, ref byte[] readData, ref string[] ReadRows)
        //{
        //    var data = GetRequestData(stream, ref readData);
        //    var rows = Regex.Split(data, Environment.NewLine);

        //    //Request URL & Method & Version
        //    var first = Regex.Split(rows[0], @"(\s+)")
        //        .Where(e => e.Trim() != string.Empty)
        //        .ToArray();
        //    if (first.Length > 0) this.Method = first[0];
        //    if (first.Length > 1) this.URL = Uri.UnescapeDataString(first[1]).Split('?')[0];
        //    if (first.Length > 2) this.ProtocolVersion = first[2];
        //}
        public List<string> RemoveArrayNullRows(string [] data)
        {
            List<string> strArray = new List<string>();
            for (int i = 0; i < data.Length; i++)
            {
                if (string.IsNullOrEmpty(data[i].Trim()))
                {
                    continue;
                }
                else
                {
                    strArray.Add(data[i]);
                }
            }
            return strArray;
        }
        /// <summary>
        /// 获取http头 2018-06-26
        /// </summary>
        /// <param name="sr"></param>
        public  void readHeaders()
        {
      //      Console.WriteLine("readHeaders()");
            string line;
            sSeparateString = string.Empty;//初始化接收
            while ((line = streamReadLine(handler)) != null)
            {
                if (line.Equals(""))
                {
              //      Console.WriteLine("got headers");
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
               //             Console.WriteLine("头部验证出错!");
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
      //          Console.WriteLine("头部: {0}:{1}", name, value);
                httpHeaders[name] = value;
            }
        }
        //public void readHeaders(Stream sr)
        //{

        //    Console.WriteLine("readHeaders()");
        //    string line;
        //    sSeparateString = string.Empty;//初始化接收
        //    while ((line = streamReadLine(sr)) != null)
        //    {
        //        if (httpHeaders.ContainsKey("Content - Disposition")&&line.Equals("") )
        //        {
        //            Console.WriteLine("got headers");
        //            return;
        //        }

        //        int separator = line.IndexOf(':');
        //        if (separator == -1)
        //        {
        //            if (line != "platformtype" && (sSeparateString != "" && !line.Contains(sSeparateString)))
        //            {
        //                if (!httpHeaders.ContainsKey("Content-Disposition"))
        //                {
        //                    continue;
        //                }
        //                if (line == "" || line == string.Empty)
        //                {
        //                    return;//结束头部
        //                }
        //                else
        //                {
        //                    Console.WriteLine("头部验证出错!");
        //                    return;
        //                }
        //            }
        //            else
        //            {
        //                //Console.WriteLine(line);
        //                continue;
        //            }
        //        }
        //        String name = line.Substring(0, separator);

        //        int pos = separator + 1;
        //        while ((pos < line.Length) && (line[pos] == ' '))
        //        {
        //            pos++; // strip any spaces
        //        }
        //        string value = line.Substring(pos, line.Length - pos);
        //        if (name == "Content-Type" && sSeparateString == "")
        //        {
        //            string[] sSeparateVaule = value.Split('=');
        //            if (sSeparateVaule.Length > 1)
        //            {
        //                if (sSeparateVaule[1].IndexOf(";") > -1)
        //                {
        //                    sSeparateString = sSeparateVaule[1].Split(';')[0];
        //                }
        //                else
        //                {
        //                    sSeparateString = sSeparateVaule[1];
        //                }
        //            }
        //        }
        //        Console.WriteLine("头部: {0}:{1}", name, value);
        //        httpHeaders[name] = value;
        //    }
        //}

        /// <summary>
        ///单行读取 2018-06-26更新
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>

        private string streamReadLine(Stream inputStream)
        {
            string data = "";
            int next_char;
            while (true)
            {
                next_char = inputStream.ReadByte();
                if (next_char == '\n') { break; }
                if (next_char == '\r') { continue; }
                if (next_char == -1) { Thread.Sleep(1); continue; };
                data += Convert.ToChar(next_char);
            }
            return data;
        }
        private void PostRequestDeal(StreamReader sr, string sFileForldPath)
        {
            //直接使用StreamReader为导致接收文件数据缺失，直接用Stream可接收所有数据，但需自行处理分行和结尾，
            //有其他更好方法请自行修改
            try
            {
                InfoModel info = new InfoModel();
                Stream stream = sr.BaseStream;
                string sFilePath = string.Empty;
                int charData = 0;
                List<byte> data = new List<byte>();
                List<byte[]> dataArray = new List<byte[]>();
                while (stream.Position != stream.Length && charData != -1)
                {
                    charData = stream.ReadByte();
                    data.Add((byte)charData);
                }
                if (data.Count < 200) return;
                int index = data.IndexOf((byte)'\n');
                while (index >= 0)
                {
                    dataArray.Add(data.Take(index + 1).ToArray());
                    data.RemoveRange(0, index + 1);
                    index = data.IndexOf((byte)'\n');
                }
                int startIndex = 0;
                int endIndex = 0;
                int length = 0;//作用？？
                bool flag = false;//是否需要特殊处理  20180108
                for (int j = 0; j < dataArray.Count; j++)
                {
                    string str = "";
                    if (dataArray[j].Length > 2)
                        str = Encoding.UTF8.GetString(dataArray[j], 0, dataArray[j].Length - 2);
                    else
                        str = Encoding.UTF8.GetString(dataArray[j]);
                    #region 解析Content-Disposition
                    if (str.Contains("Content-Disposition"))
                    {
                        string[] sSeparateVaule = str.Split('=');
                        if (sSeparateVaule.Length > 1)
                        {
                            string revfilename = sSeparateVaule[sSeparateVaule.Length - 1];//文件名
                            
                            if (revfilename != "")
                            {
                                revfilename = revfilename.Replace("\"", "");
                                info.FileName = revfilename;
                                sFilePath = sFileForldPath + "\\" + revfilename;
                            }
                            else
                            {
                                sFilePath = sFileForldPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".tar";
                            }
                            revfilename = string.Empty;
                        }
                    }
                    #endregion

                 
                    //判断是数据开头则退出循环
                    if (dataArray[j][0] == 69 && dataArray[j][1] == 66 && dataArray[j][2] == 68 && (dataArray[j][3] == 66 || dataArray[j][3] == 82 || dataArray[j][3] == 84 || dataArray[j][3] == 83))
                    {
                        string tstr = Encoding.UTF8.GetString(dataArray[j - 1]);
                        if (dataArray[j][3] == 82 || dataArray[j][3] == 84 || dataArray[j][3] == 83)
                        {
                            flag = true;
                        }
                        startIndex = j;
                        break;
                    }
                    length += dataArray[j].Length;
                }
                //     S = "---Ek_Z4Yct_SfSfiUn29bpQTy517Pv-nTec6--\r\n\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\...
                string S = "";
                for (int j = dataArray.Count - 1; j > 1; j--)
                {
                    length += dataArray[j].Length;

                  //  string ss = Encoding.Unicode.GetString(dataArray[j-1]);
                    string str = Encoding.UTF8.GetString(dataArray[j]);

      
                    S += str;
                    //判断是http结尾则退出循环
                    if (str.Contains("--" + sSeparateString + "--") && sSeparateString != "") ///这句没成立 所以没有推出循环
                    {
                        if (dataArray[j - 1].Length == 2 && dataArray[j - 1][0] == '\r' && dataArray[j - 1][1] == '\n')
                        {
            
                             if (dataArray[j][3] == 83)
                            {
                                endIndex = j;
                                length -= dataArray[j].Length;  //ceshi
                            }
                            else
                            {
                                endIndex = j - 1;
                                length += 2;
                            }
                        }
                        else if (str.IndexOf("--" + sSeparateString + "--")>0)
                        {
                            int num = str.IndexOf("--" + sSeparateString + "--");
                            if(num >0)
                            { 
                            dataArray[j]= GetEndByteArray(str, num);
                                length -= dataArray[j].Length;  
                          endIndex = j;
                            }
                            break;
                        }
                        else
                        {
                            endIndex = j;
                            if (flag)
                            {
                                length -= dataArray[j].Length;  //ceshi
                            }
                        }
                        break;
                    }
                }
           //     if (startIndex < 2) return;
                var bodyData = new byte[stream.Length - length]; //文件数据
                int dstLength = 0;
                if (flag)
                {
                    for (int j = startIndex; j < endIndex + 1; j++)
                    {
                        Array.Copy(dataArray[j], 0, bodyData, dstLength, dataArray[j].Length);
                        dstLength += dataArray[j].Length;
                    }
                }
                else
                {
                    for (int j = startIndex; j < endIndex; j++)
                    {
                        Array.Copy(dataArray[j], 0, bodyData, dstLength, dataArray[j].Length);
                        dstLength += dataArray[j].Length;
                    }
                }
                //存储文件
                File.WriteAllBytes(sFilePath, bodyData);
           
                Random rd = new Random(int.Parse(DateTime.Now.ToString("HHmmssfff")));
                info.id = rd.Next(1000, 9999);
           
                info.IsTime = DateTime.Now.ToString("yy-MM-dd hh:mm:ss");
                info.FullPath = sFilePath;
                info.IsType = IsType.Receive;
                info.OfCompletion = OfCompletion.Untreated;
                Queueinterface<InfoModel>.queue.Enqueue(info);
                //处理接收的文件
               // bool verifySuccess = false;
           //     DealTarBack(sFilePath, out verifySuccess);
             //完成接收文件后把文件增加到处理列表上去
            }
            catch (Exception em)
            {
                Console.WriteLine("HS422：" + em.Message);
                MessageBox.Show(em.ToString());
            }
            Console.WriteLine("接收Tar文件成功！");
        }

        public byte[] GetEndByteArray(string str,int EndNumber)
        {
            string endString = str.Substring(0,EndNumber);
            int number = endString.Length;
            byte[] ByteData = Encoding.UTF8.GetBytes(endString);
          
            //  
        
            ByteData[ByteData.Length - 2] = 13;
            ByteData[ByteData.Length -1] = 10;
            return ByteData;
            
        }
        public void handlePOSTRequest(Stream sr, string path, string tarName)
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
            //byte[] byteData = new byte[ms.Length];
            //ms.Read(byteData, 0, byteData.Length);
            //ms.Seek(0, SeekOrigin.Begin);
            //// 把 byte[] 写入文件
            //FileStream fsd = new FileStream(path + "\\" + tarName, FileMode.Create);
            //BinaryWriter bw = new BinaryWriter(fsd);
            //bw.Write(byteData);
            //bw.Close();
            //fsd.Close();

             PostRequestDeal(new StreamReader(ms), path);
            return;
        }
       

        private void GetRequestData( string path)
        {
            try
            {
                InfoModel info = new InfoModel();
                Console.WriteLine("get post data start");
                int content_len = 0;
                MemoryStream ms = new MemoryStream();
                if (this.httpHeaders.ContainsKey("Content-Length"))
                {
                    content_len = Convert.ToInt32(this.httpHeaders["Content-Length"]);
                    if (content_len > MAX_POST_SIZE)
                    {
                        Console.WriteLine(string.Format("POST Content-Length({0}) too big for this simple server", content_len));
                        return;
                    }
                    byte[] buf = new byte[BUF_SIZE];
                    int to_read = content_len;
                    while (to_read > 0)
                    {
                        int numread = this.handler.Read(buf, 0, Math.Min(BUF_SIZE, to_read));
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
                 
                        string str = Encoding.UTF8.GetString(buf);
                        if (str.ToUpper().IndexOf("filename".ToUpper()) > -1)
                        {
                            string pattern = @"[a-zA-Z]{8}\S{2}[A-Z]{4}[_]\w+[\.]\w{3}\S";

                            MatchCollection result =Regex.Matches(str, pattern);
                            httpHeaders["Content-Disposition"] = result[0].ToString() ;
                        }
                        to_read -= numread;
                        ms.Write(buf, 0, numread);
                    }
                    ms.Seek(0, SeekOrigin.Begin);
                }
                PostRequestDeal(new StreamReader(ms), path);
              
            }
            catch (Exception ex)
            {

                //   httpServer.Log(2,"Error",ex.Message);

            }
        }

        private Dictionary<string, string> GetRequestHeaders(IEnumerable<string> rows)
        {
            if (rows == null || rows.Count() <= 0) return null;
            var target = rows.Select((v, i) => new { Value = v, Index = i }).FirstOrDefault(e => e.Value.Trim() == string.Empty);
            var length = target == null ? rows.Count() - 1 : target.Index;
            if (length <= 1) return null;
            var range = Enumerable.Range(1, length - 1);
            return range.Select(e => rows.ElementAt(e)).ToDictionary(e => e.Split(':')[0], e => e.Split(':')[1].Trim());
        }

        /// <summary>
        /// 获取数据结果
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private string GetRequestBody(IEnumerable<string> rows)
        {
            var target = rows.Select((v, i) => new { Value = v, Index = i }).FirstOrDefault(e => e.Value.Trim() == string.Empty);
            if (target == null) return null;
            var range = Enumerable.Range(target.Index + 1, rows.Count() - target.Index - 1);
            return string.Join(Environment.NewLine, range.Select(e => rows.ElementAt(e)).ToArray());
        }
        #endregion
    }
}
