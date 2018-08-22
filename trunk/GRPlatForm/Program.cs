using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GRPlatForm
{
    static class Program
    {
        static IniFiles Ini;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Ini = new IniFiles(@Application.StartupPath + "\\Config.ini");
            bool flag = Convert.ToBoolean(Ini.ReadValue("LOGIN", "LandingCompletion"));

            flag = false;//测试加入  20180812
            if (!flag)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmLogin());
            }
            else {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
      
                Application.Run(new mainForm(true));
      
            }
          
        }
    }
}
