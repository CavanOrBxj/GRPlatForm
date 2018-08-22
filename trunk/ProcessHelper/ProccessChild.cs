using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ProcessHelper
{
    public class ProccessChild : ProcessManager
    {
        public ProccessChild(string ExeProcessName, string ProcessPath) : base(ExeProcessName, ProcessPath)
        {
        }
        public override bool CloseProcess(string path)
        {
            return base.CloseProcess(path);
        }

    }
}
