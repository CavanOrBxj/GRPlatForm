using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GRPlatForm
{
    public partial class InfoSetForm : Form
    {
        private IniFiles inis;
        public InfoSetForm()
        {
            InitializeComponent();
            inis = new IniFiles(@Application.StartupPath + "\\Config.ini");
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            inis.WriteValue("INFOSET", "SourceID", txtSourceID.Text);
            inis.WriteValue("INFOSET", "SourceName", txtSourceName.Text);
            inis.WriteValue("INFOSET", "SourceType", txtSourceType.Text);
        }

        private void InfoSetForm_Load(object sender, EventArgs e)
        {
            txtSourceID.Text = inis.ReadValue("INFOSET", "SourceID");
            txtSourceName.Text = inis.ReadValue("INFOSET", "SourceName");
            txtSourceType.Text = inis.ReadValue("INFOSET", "SourceType");
        }
    }
}
