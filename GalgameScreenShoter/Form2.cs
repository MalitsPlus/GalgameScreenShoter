using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Psettings = GalgameScreenShoter.Properties.Settings;

namespace GalgameScreenShoter
{
    public partial class Form2 : Form
    {
        public Form2(RadioButton r1, RadioButton r2)
        {
            InitializeComponent();
            _r1 = r1;
            _r2 = r2;
            BoxFloderPath1.Text = Psettings.Default.FolderPath1;
            BoxFileName1.Text = Psettings.Default.FileName1;
            BoxFloderPath2.Text = Psettings.Default.FolderPath2;
            BoxFileName2.Text = Psettings.Default.FileName2;
            switch (Psettings.Default.HotKeyFlag)
            {
                case 1:
                    radioButton1.Checked = true;
                    break;
                case 2:
                    radioButton2.Checked = true;
                    break;
                default:
                    radioButton1.Checked = true;
                    break;
            }
        }

        private static RadioButton _r1;
        private static RadioButton _r2;

        //点击浏览按钮
        private void brwoserBtn1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                BoxFloderPath1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void brwoserBtn2_Click(object sender, EventArgs e) {
            if (folderBrowserDialog2.ShowDialog() == DialogResult.OK) {
                BoxFloderPath2.Text = folderBrowserDialog2.SelectedPath;
            }
        }

        //检测信息是否完整
        private bool illegalCheck()
        {
            if (!String.IsNullOrEmpty(BoxFloderPath1.Text.Trim())
                && !String.IsNullOrEmpty(BoxFileName1.Text.Trim())
                && !String.IsNullOrEmpty(BoxFloderPath2.Text.Trim())
                && !String.IsNullOrEmpty(BoxFileName2.Text.Trim())
                )
                return true;
            else
                return false;
        }

        //点击保存按钮
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (illegalCheck() == false)
            {
                MessageBox.Show("请将信息填写完整！", "拒绝操作", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Psettings.Default.FolderPath1 = BoxFloderPath1.Text.Trim();
            Psettings.Default.FileName1 = BoxFileName1.Text.Trim();
            Psettings.Default.FolderPath2 = BoxFloderPath2.Text.Trim();
            Psettings.Default.FileName2 = BoxFileName2.Text.Trim();
            Psettings.Default.Save();
            _r1.Text = Psettings.Default.FileName1;
            _r2.Text = Psettings.Default.FileName2;
            Dispose();
            Close();
        }

        //点击取消按钮
        private void CancelBtn_Click(object sender, EventArgs e)
        {
            _r1.Dispose();
            _r2.Dispose();
            Dispose();
            Close();
        }

    }
}
