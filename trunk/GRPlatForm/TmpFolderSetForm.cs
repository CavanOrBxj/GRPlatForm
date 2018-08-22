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
    public partial class TmpFolderSetForm : Form
    {
        private IniFiles inis;
        public TmpFolderSetForm()
        {
            InitializeComponent();
            inis = new IniFiles(@Application.StartupPath + "\\Config.ini");
        }

        private void btnRevTar_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtRevTar.Text = dialog.SelectedPath;
            }
        }

        private void TmpFolderSetForm_Load(object sender, EventArgs e)
        {
            txtRevTar.Text = inis.ReadValue("FolderSet", "RevTarFolder");
            txtTarBuild.Text = inis.ReadValue("FolderSet", "SndTarFolder");
            txtUnTar.Text = inis.ReadValue("FolderSet", "UnTarFolder");
            txtXMLBuild.Text = inis.ReadValue("FolderSet", "XmlBuildFolder");

            txtMedia.Text = inis.ReadValue("FolderSet", "AudioFileFolder");
            txtBeUnTar.Text = inis.ReadValue("FolderSet", "BeUnTarFolder");
            txtBeBuildXML.Text = inis.ReadValue("FolderSet", "BeXmlFileMakeFolder");
        }

        private void btnUnTar_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtUnTar.Text = dialog.SelectedPath;
            }
        }

        private void btnXMLBuild_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
               txtXMLBuild.Text = dialog.SelectedPath;
            }
        }

        private void btnTarBuild_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtTarBuild.Text = dialog.SelectedPath;
            }
        }

        private void btnSetFolder_Click(object sender, EventArgs e)
        {
            if (txtRevTar.Text == "")
            {
                MessageBox.Show("请选择接收Tar包文件夹路径！");
                return;
            }
            if (txtTarBuild.Text == "")
            {
                MessageBox.Show("请选择Tar包生成文件夹路径！");
                return;
            }
            if (txtUnTar.Text == "")
            {
                MessageBox.Show("请选择解压Tar包文件夹路径！");
                return;
            }
            if (txtXMLBuild.Text == "")
            {
                MessageBox.Show("请选择生成XML存放文件夹路径！");
                return;
            }
            if (txtBeUnTar.Text == "")
            {
                MessageBox.Show("请选择同步反馈解压Tar包文件夹路径！");
                return;
            }
            if (txtBeBuildXML.Text == "")
            {
                MessageBox.Show("请选择同步反馈生成XML文件夹路径！");
                return;
            }
            if (txtMedia.Text == "")
            {
                MessageBox.Show("请选择音频存放文件夹路径！");
                return;
            }

            inis.WriteValue("FolderSet", "RevTarFolder", txtRevTar.Text);
            inis.WriteValue("FolderSet", "UnTarFolder", txtUnTar.Text);
            inis.WriteValue("FolderSet", "SndTarFolder", txtTarBuild.Text);
            inis.WriteValue("FolderSet", "XmlBuildFolder", txtXMLBuild.Text);

            inis.WriteValue("FolderSet", "BeUnTarFolder", txtBeUnTar.Text);
            inis.WriteValue("FolderSet", "BeXmlFileMakeFolder", txtBeBuildXML.Text);
            inis.WriteValue("FolderSet", "AudioFileFolder", txtMedia.Text);

        }

        private void btnBeUnTar_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtBeUnTar.Text = dialog.SelectedPath;
            }
        }

        private void btnBeBulidXML_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtBeBuildXML.Text = dialog.SelectedPath;
            }
        }

        private void btnMedia_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtMedia.Text = dialog.SelectedPath;
            }
        }
    }
}
