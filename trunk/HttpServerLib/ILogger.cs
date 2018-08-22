using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServerLib
{
    public interface ILogger
    {
        void Log(object message);
        void Log(int type, object message);
        void Log(int type, string FileName, object message);
    }
}
