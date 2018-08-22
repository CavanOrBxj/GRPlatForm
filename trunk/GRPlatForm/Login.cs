using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace GRPlatForm
{
    public partial class frmLogin : Form
    {
        private Object oLock = null;
        private int formHeight = 0;
        IniFiles Ini;
        public frmLogin()
        {
            Ini=new IniFiles(@Application.StartupPath + "\\Config.ini");
            InitializeComponent();
            //bool flag = Convert.ToBoolean(Ini.ReadValue("LOGIN", "LandingCompletion"));
            //if (flag)
            //{
            //    mainForm sMain = new mainForm(true);
     
            //    sMain.Show();
            //    this.Hide();
            //}
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            this.Height = gbSetting.Visible ? formHeight - gbSetting.Height : formHeight;
            gbSetting.Visible = !gbSetting.Visible;
            btnSetting.Text = btnSetting.Text == ">>" ? "<<" : ">>";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtCode.Text.Trim() == "")
            {
                MessageBox.Show("用户编码不能为空，请输入！", "提示");
                txtCode.Focus();
                return;
            };
            if (txtUser.Text.Trim() == "")
            {
                MessageBox.Show("用户名称不能为空，请输入！", "提示");
                txtUser.Focus();
                return;
            };
            if (txtPass.Text.Trim() == "")
            {
                MessageBox.Show("用户密码不能为空，请输入！", "提示");
                txtPass.Focus();
                return;
            };
            dbAccess dA = new dbAccess();
            dA.conn.ConnectionString = GetConnectString();
            try
            {
                if (dA.OpenConn())
                {
                    if (dA.loginJudge(txtCode.Text.Trim(), txtUser.Text.Trim(), txtPass.Text.Trim(), dA) != 1)
                    {
                        MessageBox.Show("登录失败，请重新检查用户编码、用户姓名、登录密码！！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtCode.Focus();
                        return;
                    }
            
                   
                }
                else
                {
                    MessageBox.Show("数据库连接失败，请检查连接设置！", "提示");
                    return;
                }
                bool flag = Convert.ToBoolean(Ini.ReadValue("LOGIN", "LandingCompletion"));
                if (!flag)
                {
                    Ini.WriteValue("LOGIN", "LandingCompletion", "true");
                }
                mainForm sMain = new mainForm();
                mainForm.dba = dA;
                sMain.Show();
                this.Hide();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message + "\r\n请重新设置数据库连接！", "错误");
                return;
            }
            finally
            {
                if (dA.ConnState == ConnectionState.Open)
                    dA.CloseConn();
                dA = null;
            }
        }
        #region
        //获取数据库连接字符串
        private String GetConnectString() 
        {
            string sReturn;
            string sPass;
            sPass = txtDbPass.Text.Trim();
            if (sPass == "")
                 sPass = "tuners2012"; 
            sReturn = "server = " + txtServer.Text.Trim() +
                   ";UID = " + txtDbuser.Text.Trim() +
                    ";Password = " + sPass +
                     ";DataBase = " + txtDb.Text.Trim() + ";"
                     + "MultipleActiveResultSets=True";

            return sReturn;
        }
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            if (txtServer.Text.Trim() == "")
            {
                MessageBox.Show("服务器不能为空，请输入！", "提示");
                txtServer.Focus();
                return;
            };
            if (txtDb.Text.Trim() == "")
            {
                MessageBox.Show("数据库不能为空，请输入！", "提示");
                txtDb.Focus();
                return;
            };
            if (txtDbuser.Text.Trim() == "")
            {
                MessageBox.Show("数据库用户名不能为空，请输入！", "提示");
                txtDbuser.Focus();
                return;
            };
            //写INI文件
            IniFiles ini = new IniFiles(@Application.StartupPath + "\\Config.ini");
            ini.WriteValue("Database", "ServerName", txtServer.Text.Trim());
            ini.WriteValue("Database", "DataBase", txtDb.Text.Trim());
            ini.WriteValue("Database", "LogID", txtDbuser.Text.Trim());
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if ((sender as TextBox).Name == "txtPass")
                {
                    btnLogin_Click(null, EventArgs.Empty);
                    return;
                }
                //需设置textBox的TabIndex顺序属性
                this.SelectNextControl(this.ActiveControl, true, true, true, true);  
                
            }    
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            gbSetting.Visible = false;

            oLock = new Object();
            //读取INI文件
            formHeight = this.Height;
            IniFiles ini = new IniFiles(@Application.StartupPath + "\\Config.ini");
            txtServer.Text = ini.ReadValue("Database", "ServerName");
            txtDb.Text = ini.ReadValue("Database", "DataBase");
            txtDbuser.Text = ini.ReadValue("Database", "LogID");
            txtDbPass.Text = ini.ReadValue("Database", "LogPass");
            this.Height = formHeight - gbSetting.Height;

        }

    }
}
