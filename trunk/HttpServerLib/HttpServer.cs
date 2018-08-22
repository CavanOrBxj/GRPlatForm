using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Sockets;
using System.IO; 
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Net.Security;
using System.Security.Authentication;


namespace HttpServerLib
{
    public class HttpServer : IService
    {

        /// <summary>
        /// 服务器IP
        /// </summary>
        public string ServerIP { get; private set; }

        /// <summary>
        /// 服务器端口
        /// </summary>
        public int ServerPort { get; private set; }

        /// <summary>
        /// 启动路径
        /// </summary>
        public string StartupPath { get; set; }
        /// <summary>
        /// 服务器目录
        /// </summary>
        public string ServerRoot { get; private set; }

        ////设置文件tar包保存路径
        public string SavePath { get; set; }

        /// 是否运行
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// 服务器协议
        /// </summary>
        public Protocols Protocol { get; private set; }
        /// <summary>
        /// SSL证书
        /// </summary>
        private X509Certificate serverCertificate = null;
        /// <summary>
        /// 服务端Socet
        /// </summary>
        private TcpListener serverListener;

        /// <summary>
        /// 日志接口
        /// </summary>
        public ILogger Logger { get; set; }
        ///
        public int ReceiveTimeout { get; set; }
        public int SendTimeout { get; set; }

        public bool IsReusable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="port">端口号</param>
        /// <param name="root">根目录</param>
        private HttpServer(IPAddress ipAddress, int port, string root)
        {
            this.ServerIP = ipAddress.ToString();
            this.ServerPort = port;

            //如果指定目录不存在则采用默认目录
            if (!Directory.Exists(root))
                this.ServerRoot = AppDomain.CurrentDomain.BaseDirectory;
            else
            {
                this.ServerRoot = root;
            }
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="port">端口号</param>
        /// <param name="root">根目录</param>
        public HttpServer(string ipAddress, int port, string root) :
            this(IPAddress.Parse(ipAddress), port, root)
        { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="port">端口号</param>
        public HttpServer(string ipAddress, int port) :
            this(IPAddress.Parse(ipAddress), port, AppDomain.CurrentDomain.BaseDirectory)
        { }





        public void Stop()
        {
            IsRunning = false;
            serverListener.Stop();
            serverListener = null;

        }

        /// <summary>
        /// 开启服务器
        /// </summary>
        public void Start()
        {
            if (IsRunning) return;

            //创建服务端Socket
            serverListener = new TcpListener(IPAddress.Parse(ServerIP), ServerPort);
            this.Protocol = serverCertificate == null ? Protocols.Http : Protocols.Https;
            this.IsRunning = true;
          
            this.serverListener.Start();
            this.Log(string.Format("Sever is running at {0}://{1}:{2}", Protocol.ToString().ToLower(), ServerIP, ServerPort));

            try
            {
                while (IsRunning)
                {
                   
                    TcpClient client = serverListener.AcceptTcpClient();
                    if (this.ReceiveTimeout > 0)
                    {
                        client.ReceiveTimeout = this.ReceiveTimeout;
                    }
                    if (this.SendTimeout > 0)
                    {
                        client.SendTimeout = this.SendTimeout;
                    }
                    Thread requestThread = null;
                    requestThread = new Thread(() => { ProcessRequest(client, ref requestThread); });
                    requestThread.Start();
                }
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }
        #region 内部方法
        /// <summary>
        /// 处理ssl加密请求
        /// </summary>
        /// <param name="clientStream"></param>
        /// <returns></returns>
        /// 
        private Stream ProcessSSL(Stream clientStream)
        {
            try
            {
                SslStream sslStream = new SslStream(clientStream);
                sslStream.AuthenticateAsServer(serverCertificate, false, SslProtocols.Tls, true);
                sslStream.ReadTimeout = 10000;
                sslStream.WriteTimeout = 10000;
                return sslStream;
            }
            catch (Exception e)
            {
                Log(e.Message);
                clientStream.Close();
            }

            return null;
        }

        /// <summary>
        /// 处理客户端请求
        /// </summary>
        /// <param name="handler">客户端Socket</param>
        /// 
        //public void ProcessRequest(HttpContext context)
        //{
        //    context.Response.ContentType = "text/plain";
        //    //接收参数
        //    string time = context.Request["time"];

        //    if (!string.IsNullOrEmpty(time))
        //    {
        //        //调用方法
        //    }

        //    context.Response.Write("2");//返回参数
        //}


        private void ProcessRequest(TcpClient handler, ref Thread th)
        {
            //处理请求
            Stream clientStream = handler.GetStream();


            //处理SSL
            if (serverCertificate != null) clientStream = ProcessSSL(clientStream);
           
            if (clientStream == null) return;

         //   var context = httpListener.EndGetContext(ar);
            //构造HTTP请求
            HttpRequest request = new HttpRequest(clientStream, SavePath, this);
            request.Logger = Logger;

            //构造HTTP响应
            HttpResponse response = new HttpResponse(clientStream);
            response.Logger = Logger;

            //处理请求类型
            switch (request.Method)
            {
                case "GET":
                    OnGet(request, response);
                    break;
                case "POST":
                    try
                    {
                        OnPost(request, response, SavePath, request.GetFileName(request.httpHeaders["Content-Disposition"].ToString()));
                    }
                    catch (Exception ex)
                    {
                        this.Log(string.Format(DateTime.Now.ToString("yy-MM-dd HH:mm:ss")+ "收到非法文件"));
                    }
                    break;
                default:
                    OnDefault(request, response);
                    break;
            }
            clientStream.Flush();
            request = null; response = null;
            handler.Close();
            GC.Collect();
            th.Abort();

        }


        #endregion
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">日志消息</param>


        #region 公开方法
        public void Log(object message)
        {
            if (Logger != null) Logger.Log(message);
        }
        public void Log(int type,string FileName, object message)
        {
            if (Logger != null) Logger.Log(type,FileName,message);
        }
        public void Log(int type,object message)
        {
            if (Logger != null) Logger.Log(type,message);
        }
        public virtual void OnDefault(HttpRequest request, HttpResponse response)
        {
         
        }

        public virtual void OnGet(HttpRequest request, HttpResponse response)
        {
          
        }

        public virtual void OnPost(HttpRequest request, HttpResponse response)
        {
          
        }
        public virtual void OnPost(HttpRequest request, HttpResponse response,string SavePath, string FileName)
        {

        }


        #endregion
    }
}
