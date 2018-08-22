using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ProcessHelper
{
    public class ProcessManager : ProcessInterface
    {
        public string ExeProcessName
        {
            get;set;
        }
        public string ProcessPath
        {
            get;
            set;
        }
        public ProcessManager(string ExeProcessName,string ProcessPath)
        {
            this.ExeProcessName = ExeProcessName;
            this.ProcessPath = ProcessPath;


        }
        public virtual bool CheckProcess(string path)
        {

            Process[] processes = Process.GetProcesses();
            foreach (var item in processes)
            {
                try
                {
                    if (item.MainModule.FileName.ToLower() == path.ToLower())
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }

            }
            return false;
        }

        public virtual bool CloseProcess(string path)
        {
            throw new NotImplementedException();
        }

        public virtual bool StartProcess(string path)
        {
            throw new NotImplementedException();
        }
    }
}
