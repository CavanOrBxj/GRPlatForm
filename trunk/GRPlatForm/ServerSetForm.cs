using System;
using System.Windows.Forms;

namespace GRPlatForm
{
    public partial class ServerSetForm : Form
    {
        private IniFiles inis;
        public ServerSetForm()
        {
            InitializeComponent();
            inis = new IniFiles(@Application.StartupPath + "\\Config.ini");
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            inis.WriteValue("INFOSET", "YXURL", txtYXPlat.Text);
            inis.WriteValue("INFOSET", "BJURL", txtZJPlat.Text);
        }

        private void ServerSetForm_Load(object sender, EventArgs e)
        {
            txtYXPlat.Text = inis.ReadValue("INFOSET", "YXURL");
            txtZJPlat.Text = inis.ReadValue("INFOSET", "BJURL");
        }
    }
}
