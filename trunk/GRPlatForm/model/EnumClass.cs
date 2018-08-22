using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GRPlatForm.model
{
    class EnumClass
    {
        public enum RecordptType
        {
            Full,//全量
            Incremental, // 增量 
        }
        public enum PlaybackRecordType
        {

            Report,// 上报        
            request//请求
        }
    }
}
