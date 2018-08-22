using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessHelper
{
    interface ProcessInterface
    {
         bool StartProcess(string path);
        bool CloseProcess(string path);
        bool CheckProcess(string path);

            
    }
}
