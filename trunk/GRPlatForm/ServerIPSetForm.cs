using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace GRPlatForm
{
    public partial class ServerIPSetForm : Form
    {
        private IniFiles ini;
        public ServerIPSetForm()
        {
            InitializeComponent();
            ini = new IniFiles(@Application.StartupPath + "\\Config.ini");
        }

        private void ServerIPSetForm_Load(object sender, EventArgs e)
        {
            System.Net.IPAddress[] ipArr;
            ipArr = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName());
            //for (int i = 0; i < ipArr.Length; i++)
            //{
            //    if (ipArr[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            //    {
            //        cmbServerIP.Items.Add(ipArr[i].ToString());
            //    }
            //}
            cmbServerIP.Items.AddRange(ipArr);
            txtServerPort.Text = ini.ReadValue("INFOSET", "ServerPort");
            cmbServerIP.Text = ini.ReadValue("INFOSET", "ServerIP");
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            IPAddress ip;
            int iport = 0;
            if (cmbServerIP.Text == "")
            {
                MessageBox.Show("请选择或者填写服务IP！");
                cmbServerIP.Focus();
                return;
            }
            if (txtServerPort.Text == "")
            {
                MessageBox.Show("请填写服务端口号！");
                txtServerPort.Focus();
                return;
            }
            //回传接收IP
            if (!IPAddress.TryParse(cmbServerIP.Text.Trim(), out ip))
            {
                MessageBox.Show("服务IP为非IP类型，请重新输入！", "错误");
                cmbServerIP.Focus();
                return;
            }
            if (!int.TryParse(txtServerPort.Text.Trim(), out iport)
                || iport < 0 || iport > 65536)
            {
                MessageBox.Show("服务端口应是介于0至65536间的数值，请重新输入！", "错误");
                txtServerPort.Focus();
                return;
            }
            ini.WriteValue("INFOSET", "ServerPort", txtServerPort.Text);
            ini.WriteValue("INFOSET", "ServerIP", cmbServerIP.Text);

        }
    }
}
