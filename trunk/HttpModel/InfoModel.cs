using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpModel
{

    public enum OfCompletion
    {
        Untreated = 0,//未处理
        processing = 1,//处理中
        processed = 2//处理完
    }
    public enum IsType
    {
        Receive=1,
        send=2
    }
    public class InfoModel
    {

        //处理标识
        public int id { get; set; }
        //文件名称
        public string FileName { get; set; }

        /// <summary>
        /// 保存路径
        /// </summary>
        public string FullPath { get; set; }
        //（收/发）时间
        public string IsTime { get; set; }

        public IsType IsType { get; set; }
        //处理结果
        public OfCompletion OfCompletion { get; set; }
    }
}
