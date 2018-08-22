using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;

namespace HttpServerLib
{
 public   class HttpResponse:BaseHeader
    {
       
        public string StatusCode { get; set; }

        public string Protocols { get; set; }

        public string ProtocolsVersion { get; set; }

        public byte[] Content { get; private set; }
        public string FileName { get;private set; }
        private Stream handler;
        
        public ILogger Logger { get; set; }
        public string sEndLine = "\r\n";
        public HttpServer httpServer { get; set; }
        public HttpResponse(Stream stream)
        {
            this.handler = stream;
        }
        public HttpResponse(Stream stream,HttpServer httpServer)
        {
            this.handler = stream;
            this.httpServer = httpServer;
        }
        public HttpResponse SetContent(byte[] content, Encoding encoding = null)
        {

            this.Content = content;
            this.Encoding = encoding != null ? encoding : Encoding.UTF8;
            this.Content_Length = content.Length.ToString();
            return this;
        }
        public HttpResponse SetContent(byte[] content, string FileName, Encoding encoding = null)
        {

            this.Content = content;
            this.Encoding = encoding != null ? encoding : Encoding.UTF8;
            this.Content_Length = content.Length.ToString();
            return this;
        }
        public HttpResponse SetContent(string content, Encoding encoding = null)
        {
            //初始化内容
            encoding = encoding != null ? encoding : Encoding.UTF8;
            return SetContent(encoding.GetBytes(content), encoding);
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        public void Send()
        {
            if (!handler.CanWrite) return;

            try
            {
                
                //发送响应头
                var header = BuildHeader(FileName,Content.Length);
                byte[] headerBytes = this.Encoding.GetBytes(header);
                handler.Write(headerBytes, 0, headerBytes.Length);

                //发送空行
                byte[] lineBytes = this.Encoding.GetBytes(System.Environment.NewLine);
                handler.Write(lineBytes, 0, lineBytes.Length);

                //发送内容
                handler.Write(Content, 0, Content.Length);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
            finally
            {
                handler.Close();
            }
        }
        public void SendNew(string FullTarFileName)
        {
            FileStream fsSnd = new FileStream(FullTarFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(fsSnd);     //时间戳
            int datalen = (int)fsSnd.Length + 2;
            int bufferLength = 4096;
            long offset = 0; //开始上传时间

            string[] tmp = FullTarFileName.Split('\\');
            string fName = tmp[tmp.Length - 1];
            writeHeader(datalen.ToString(), fName);


            byte[] buffer = new byte[4096]; //已上传的字节数
            int size = br.Read(buffer, 0, bufferLength);
            while (size > 0)
            {
                handler.Write(buffer, 0, size);
                offset += size;
                size = br.Read(buffer, 0, bufferLength);
            }
            handler.Write(Encoding.UTF8.GetBytes(sEndLine), 0, 2);
            Thread.Sleep(500);//太快提交会导致提交失败 所以这里加入延时   20180816
            handler.Flush();//提交写入的数据
            fsSnd.Close(); 
        }


        public void writeHeader(string strDataLen, string strTarName)//,ref FileStream fsave
        {
            StringBuilder sbHeader = new StringBuilder(200);

            sbHeader.Append("HTTP/1.1 200 OK" + sEndLine);//HTTP/1.1 200 OK
            sbHeader.Append("Content-Disposition:attachment;name=\"file\";filename=" + "\"" + strTarName + "\"" + sEndLine);
            sbHeader.Append("Content-Type:application/x-tar" + sEndLine);
            sbHeader.Append("Server:WinHttpClient" + sEndLine);
            sbHeader.Append("Content-Length:" + strDataLen + sEndLine);
            sbHeader.Append("Date:" + DateTime.Now.ToString("r") + sEndLine);
            sbHeader.Append(sEndLine);
            byte[] bTmp = Encoding.UTF8.GetBytes(sbHeader.ToString());
            handler.Write(bTmp, 0, bTmp.Length);
        }

        /// <summary>
        /// 构建响应头部
        /// </summary>
        /// <returns></returns>
        protected string BuildHeader()
        {
            StringBuilder builder = new StringBuilder();

            if (!string.IsNullOrEmpty(StatusCode))
                builder.Append("HTTP/1.1 " + StatusCode + "\r\n");

            if (!string.IsNullOrEmpty(this.Content_Type))
                builder.AppendLine("Content-Type:" + this.Content_Type);
            return builder.ToString();
        }

        /// <summary>
        /// 构建响应头部
        /// </summary>
        /// <returns></returns>
        protected string BuildHeader(string strTarName, int strDataLen)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("HTTP/1.1 200 OK" + sEndLine);//HTTP/1.1 200 OK
            builder.Append("Content-Disposition:attachment;filename=" + "\"" + strTarName + "\"" + sEndLine);
            builder.Append("Content-Type:application/x-tar" + sEndLine);
            builder.Append("Server:WinHttpClient" + sEndLine);
            builder.Append("Content-Length:" + strDataLen + sEndLine);
            builder.Append("Date:" + DateTime.Now.ToString("r") + sEndLine);
            builder.Append(sEndLine);
            return builder.ToString();
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">日志消息</param>
        private void Log(object message)
        {
            if (Logger != null)
                Logger.Log(message);
        }

    }
}
