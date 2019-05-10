using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using Imaging = System.Drawing.Imaging;
using System.Media;
using Psettings = GalgameScreenShoter.Properties.Settings;

namespace GalgameScreenShoter
{
    public partial class Form1 : Form
    {
        public Form2 settingForm;
        private static int RegistID = 8125;

        public Form1()
        {
            InitializeComponent();
            initializeProgramSettings();
            initializeHotKeys();
        }

        //初始化程序设置
        private void initializeProgramSettings()
        {
            if (String.IsNullOrEmpty(Psettings.Default.FolderPath1))
                Psettings.Default.FolderPath1 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (String.IsNullOrEmpty(Psettings.Default.FileName1))
                Psettings.Default.FileName1 = "screenshot1_scr";

            if (String.IsNullOrEmpty(Psettings.Default.FolderPath2))
                Psettings.Default.FolderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (String.IsNullOrEmpty(Psettings.Default.FileName2))
                Psettings.Default.FileName2 = "screenshot2_scr";

            radioButton1.Text = Psettings.Default.FileName1;
            radioButton2.Text = Psettings.Default.FileName2;

            if (Psettings.Default.ProfileFlag == 0) {
                Psettings.Default.ProfileFlag = 1;
                radioButton1.Checked = true;
            }
            else {
                switch (Psettings.Default.ProfileFlag) {
                    case 1: radioButton1.Checked = true; break;
                    case 2: radioButton2.Checked = true; break;
                    default: radioButton2.Checked = true; break;
                }
            } 
        }

        //截图处理
        private void exeScreenShot()
        {
            //创建图象，保存将来截取的图象
            Bitmap image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics imgGraphics = Graphics.FromImage(image);
            //已黑色填充画布，否则RGB(0,0,0)时会出现坏点
            imgGraphics.Clear(Color.Black);
            //设置截屏区域
            imgGraphics.CopyFromScreen(0, 0, 0, 0, new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));

            string fileName;
            string folderPath;

            switch (Psettings.Default.ProfileFlag) {
                case 1: fileName = Psettings.Default.FileName1;
                        folderPath = Psettings.Default.FolderPath1;
                        break;
                case 2: fileName = Psettings.Default.FileName2;
                        folderPath = Psettings.Default.FolderPath2;
                        break;
                default: fileName = Psettings.Default.FileName1;
                        folderPath = Psettings.Default.FolderPath1;
                        break;
            }

            //检测是否有重名文件
            string[] filesName = Directory.GetFiles(folderPath, fileName + "???" + ".png");

                //若没有文件
                if (filesName.Length == 0)
                {
                
                    image.Save(folderPath + "\\" + fileName + "001" + ".png", Imaging.ImageFormat.Png);
                    SystemSounds.Beep.Play();
                    return;
                }

                //选取最大序号
                Array.Sort(filesName);
                int lastNum = 0;

                for (int i = 0; i < filesName.Length; i++)
                {
                    lastNum = int.Parse(filesName[filesName.Length - (i + 1)].Substring(filesName[filesName.Length - (i + 1)].Length - 7, 3)); //.Substring(userData.FileName.Length, 3));
                    if (lastNum < 900)
                    {
                        image.Save(folderPath + "\\" + fileName + (lastNum + 1).ToString("D3") + ".png", Imaging.ImageFormat.Png);
                        SystemSounds.Beep.Play();
                        break;
                    }
                    else
                        continue;
                }
            
            image.Dispose();
            imgGraphics.Dispose();
        } 

        //热键响应
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0312:
                    if (m.WParam.ToInt32() == RegistID)
                    {
                        exeScreenShot();
                    }
                break;
            }
            base.WndProc(ref m);
        }

        //注册系统热键
        private void initializeHotKeys()
        {
            try
            {
                HotKeyControl.RegisterHotKey(this.Handle, RegistID, HotKeyControl.KeyModifiers.Alt, Keys.D);
            }
            catch (Exception e)
            {
                MessageBox.Show("注册系统热键出错\n" + e.Message, "Oops", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
        }

        //注销系统热键
        private void unregistHotKeys()
        {
            HotKeyControl.UnregisterHotKey(this.Handle, RegistID);
        }

        //设置按钮点击
        private void button1_Click(object sender, EventArgs e)
        {
            if (settingForm != null) {
                //settingForm.ShowDialog();
                settingForm.Activate();
                settingForm.Update();
            }
            else {
                settingForm = new Form2(radioButton1, radioButton2);
                settingForm.ShowDialog();
                settingForm.Activate();
                settingForm.FormClosed += onsettingformclosed;
            }

            /*
            if (settingForm == null || settingForm.IsDisposed)
            {
                settingForm = new Form2(radioButton1, radioButton2);
                settingForm.ShowDialog();
                settingForm.Activate();
                settingForm.FormClosed += onsettingformclosed;
            }
            else
                settingForm.Activate();
             */
        }

        void onsettingformclosed(object sender, FormClosedEventArgs e) {
            settingForm = null;
        }

        //双击任务栏图标
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.Activate();
            /*
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                //this.ShowInTaskbar = true;
                this.Activate();
            }
             */
        }

        //菜单点击退出
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            unregistHotKeys();
            this.Dispose();
            this.Close();
        }

        //菜单点击Setting
        private void SettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (settingForm == null || settingForm.IsDisposed)
            {
                settingForm = new Form2(radioButton1, radioButton2);
                settingForm.ShowDialog();
                settingForm.Activate();
            }
            else
                settingForm.Activate();
        }

        //菜单点击About
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 newAboutForm = new Form3();
            newAboutForm.ShowDialog();
        }

        //点击关闭
        private void closeConfirm(object sender, FormClosingEventArgs e)
        {

            if (MessageBox.Show("Are you sure to exit?", "Exit Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                unregistHotKeys();
                this.Dispose();
                this.Close();
            }
            else
            {
                e.Cancel = true;
            }
             
        }

        //Hide按钮点击
        private void button3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        //Shot按钮
        private void button2_Click(object sender, EventArgs e)
        {
            exeScreenShot();
        }

        //radioButton1选中
        private void radioButton1_CheckedChanged(object sender, EventArgs e) {
            Psettings.Default.ProfileFlag = 1;
            Psettings.Default.Save();
        }
        //radioButton2选中
        private void radioButton2_CheckedChanged(object sender, EventArgs e) {
            Psettings.Default.ProfileFlag = 2;
            Psettings.Default.Save();
        }
    }
}
