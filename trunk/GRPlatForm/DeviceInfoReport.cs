using System;

namespace GRPlatForm
{
    [Serializable]
    public class DeviceInfoReport
    {
        public string RPTStartTime;

        public string RPTEndTime;

        public Device Device;
    }

    public class Device
    {
        public string SRV_ID;
        public string DeviceID;

        public string DeviceCategory;

        public string DeviceType;

        public string DeviceName;

        public string AreaCode;

        public string AdminLevel;

        /// <summary>
        /// 
        /// </summary>
        /*-------------2018-06-10添加-----------------*/
        public string Srv_Mft_Date;
        public string UpdateDate;
        public string Old_Srv_Mft_Date;
        public string Old_UpdateDate;
        /*------------------------------*/

        /// <summary>
        /// 应急广播适配器资源编码
        /// </summary>
        public string EBRID;

        /// <summary>
        /// 
        /// </summary>
        public string DeviceState;
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude;
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude;

        /// <summary>
        /// url  20180815新增
        /// </summary>
        public string URL;
    }

}
