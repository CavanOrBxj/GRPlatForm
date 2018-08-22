using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GRPlatForm.model.LogicalModel;
using System.Data;

namespace GRPlatForm.LogicalCoding
{
   public  class LogicalHelper
    {
        public LogicalData LogicalData
        {
            get;set;
        }
        public LogicalHelper(LogicalData logicalData)
        {
            this.LogicalData = logicalData;
        }
        /// <summary>
        /// 组合逻辑码
        /// </summary>

        public string GetLogicalAndAddDataBase()
        {
            string logicalCode = GetGBCode();
   
           int number=  GetGBCodeCount(LogicalData.srvAreaID, null);

            if (number >99)
            {
                HttpServerFrom.SetManager("资源已满，查看是否要进行变更",System.Drawing.Color.Red);
            }
           return   CombinationalLogicCode(logicalCode,number.ToString());

        }
        /// <summary>
        /// 对比区域码
        /// </summary>
        /// <param name="logicalCode"></param>
        /// <returns></returns>
        public bool GetCombAreaCode(string logicalCode,string areaid)
        {
            string dGB_CODE = QueryDataByAreaCode(areaid);
            string AreaCode = GetAreaCode(logicalCode);
            if (AreaCode.Equals(dGB_CODE))
            {
            
                    return true;
            }
            else
            {
                if (!string.IsNullOrEmpty(dGB_CODE))
                    return false;
                return true;
            }
        }
        public string QueryDataByAreaCode(string areaid)
        {
            string sql = "select GB_CODE from Organization where ORG_ID = " + areaid;
            DataTable dt = mainForm.dba.getQueryInfoBySQL(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            return null;
        }
        private string GetAreaCode(string logicalCode)
        {
            return logicalCode.Substring(1,12);
        }
        public string GetLogicalCodel(string Code)
        {
            if (GetGBCodeCount(LogicalData.srvAreaID, Code) == 0)
            {
              string   sql = "select SRV_LOGICAL_CODE_GB from srv  where SRV_LOGICAL_CODE_GB='" + Code + "'";
                DataTable dt = mainForm.dba.getQueryInfoBySQL(sql);
                if (dt != null && dt.Rows.Count == 0)
                {
                    if (Code.Length != 23)
                    {
                        GetLogicalAndAddDataBase();
                    }
           
                }
                else
                {
                    Code = GetLogicalAndAddDataBase();
                }
             
                return Code;
            }
            string head = "";
            string foot = "";
            if (Code.Length == 23)
            {
                 head = Code.Substring(0, Code.Length - 2);

                foot = (Convert.ToInt32(Code.Substring(Code.Length - 2)) + 1).ToString();
            }

             else if (Code.Length != 23)
            {
                return GetLogicalAndAddDataBase();
            }
            if (foot.Length == 1)
            {
                foot = "0" + foot;
            }

            return GetLogicalCodel(head+foot);
        }
        #region 私有
        private string CombinationalLogicCode(string AreaCode, string number)
        {
         
            string builder = "";
            number = GetNuberToString(number);
            // 63301080000000314010401
            string level = "6";
            string ResourceType = "0314";
            string ResourceTypeNumber = "01";


            // 子类型
            string Subtype = "04";
            string logicCode = level + AreaCode + ResourceType + ResourceTypeNumber + Subtype + number;
            if (logicCode.Length != 23 && LogicalData.LogicalCode.Length == 23)
            {
                logicCode = LogicalData.LogicalCode;
            }
            logicCode =GetLogicalCodel(logicCode);
    
            if (logicCode.Length == 23)
            {
                if (!UpdateLogicalCode(LogicalData.srvID, logicCode))
                {
                    HttpServerFrom.SetManager("更改逻辑编码失败", System.Drawing.Color.Red);
                }
                return logicCode;
            }
            return null;
        }

        private string GetNuberToString(string number)
        {
            if (number == "0")
            {
                return  "0" + (Convert.ToInt32(number)+1);
            }
            if (number.Length == 1)
            {
                return "0" + number;
            }
            else if(number.Length ==2)
            {
                return number;
            }
            return null;
            
         }
        public bool UpdateLogicalCode(string Srvid, string LogicalCode)
        {
            bool flag = false;
            try
            {
                string sql = "update srv set SRV_LOGICAL_CODE_GB='" + LogicalCode + "' where srv_id=" + Srvid;
                int result = mainForm.dba.UpdateDbBySQL(sql);
                if (result > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            return false;

        }

        private string GetGBCode()
        {

            try
            {
                string areaId = LogicalData.srvAreaID;
                string sql = "select GB_CODE from organization where org_id =  " + areaId;

                DataTable dtMedia = mainForm.dba.getQueryInfoBySQL(sql);
                if (dtMedia != null && dtMedia.Rows.Count > 0)
                {
                    return dtMedia.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public int GetAreaByCodeCount(string areaId)
        {
            string sql = "select SRV_LOGICAL_CODE from srv  where areaId = " + areaId;
            DataTable dtMedia = mainForm.dba.getQueryInfoBySQL(sql);
            if (dtMedia != null && dtMedia.Rows.Count > 0)
            {
                return dtMedia.Rows.Count;
            }
            return 0;
        }
        /// <summary>
        /// 获取区域总数
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        private int GetGBCodeCount(string areaId,string LogicalCode)
        {
            try
            {
                string sql = "select SRV_LOGICAL_CODE_GB from srv  where areaId = " + areaId;
                if (!string.IsNullOrEmpty(LogicalCode))
                    sql += " and SRV_LOGICAL_CODE_GB='" + LogicalCode + "'";
                DataTable dtMedia = mainForm.dba.getQueryInfoBySQL(sql);
                if (dtMedia != null && dtMedia.Rows.Count > 0)
                {
     
                    return 1;
                }
                else
                {
                    sql = "";
                    sql = "select SRV_LOGICAL_CODE_GB from srv  where SRV_LOGICAL_CODE_GB='" + LogicalCode + "'";
                    DataTable dt = mainForm.dba.getQueryInfoBySQL(sql);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return -1;

        }
        #endregion
    }
}
