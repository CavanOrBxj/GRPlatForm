using System.Threading;
using System.Collections.Generic;
using System.Data;

namespace GRPlatForm
{
    public class SingletonInfo
    {
        private static SingletonInfo _singleton;


        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude;
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude;


        public string PlayRecordLastTime;

        public string TsCmd_ValueID_;

        public string sZJPostUrlAddress;
        public string TerminalLastTime;

        public bool PlatformInformationFirst;

        public int SequenceCodes;//顺序码

        public Dictionary<string, string> DicTsCmd_ID;
        public Dictionary<string, List<Thread>> DicPlayingThread;


        public string USER_PRIORITY;
        public string TsCmd_UserID;
        public string USER_ORG_CODE;

        public int TerminalCount;//数据库中的终端数量
        private SingletonInfo()                                                                 
        {
            Longitude = "";
            Latitude = "";
            PlayRecordLastTime = "";

            //终端
            TerminalLastTime = "";
            TsCmd_ValueID_ = "";
            sZJPostUrlAddress = "";
            PlatformInformationFirst = false;
            SequenceCodes = 0;
            DicTsCmd_ID = new Dictionary<string, string>();
            DicPlayingThread = new Dictionary<string, List<Thread>>();

            USER_PRIORITY = "";
            TsCmd_UserID = "";
            USER_ORG_CODE = "";
            TerminalCount = 0;
        }
        public static SingletonInfo GetInstance()
        {
            if (_singleton == null)
            {
                Interlocked.CompareExchange(ref _singleton, new SingletonInfo(), null);
            }
            return _singleton;
        }
    }
}