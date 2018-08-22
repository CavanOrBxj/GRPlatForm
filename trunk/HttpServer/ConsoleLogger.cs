using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpServerLib;
using HttpModel;

namespace HttpServer
{
    public class ConsoleLogger : ILogger
    {
        public void Log(object message)
        {
            HttpModel. Log.Instance.LogWrite(message.ToString());
  
        }

        public void Log(int type, object message)
        {
            if (type == 0)
            {
                HttpModel.Log.Instance.LogWrite(message.ToString());
            }
            else if (type == 1)
            {
                HttpModel.Log.Instance.LogWrite(message.ToString());
            }
            else if (type == 2)
            {
                HttpModel.Log.Instance.LogWrite(message.ToString());
            }
        }
        public void Log(int type,string FileName, object message)
        {
            if (type == 0)
            {
                HttpModel.Log.Instance.LogWrite(message.ToString());
            }
            else if (type == 1)
            {
                HttpModel.Log.Instance.LogWrite(message.ToString());
            }
            else if (type == 2)
            {
                HttpModel.Log.Instance.LogWrite(message.ToString());
            }
        }
    }
}
