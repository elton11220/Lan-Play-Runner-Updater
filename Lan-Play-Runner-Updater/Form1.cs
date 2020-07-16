using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using MetroFramework.Forms;
using System.Threading;
using System.Diagnostics;

namespace Lan_Play_Runner_Updater
{
    public partial class Form1 : MetroForm 
    {
        private static string Server = "https://elton1122.top/App/status.html";
        private static string Down_Url = "";
        private static int Server_Version = 0;
        private static int Local_Version = 0;
        private static int Local_UpdaterVer = 2;
        public static string me_path = System.IO.Directory.GetCurrentDirectory(); //程序路径

        public Form1()
        {
            InitializeComponent();
        }

        private void GetVersion(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.Diagnostics.FileVersionInfo fv = System.Diagnostics.FileVersionInfo.GetVersionInfo(path);
                string ver = fv.FileVersion;
                string ver1 = ver.Substring(0, 1);
                Local_Version = Convert.ToInt32(ver1);
            }
            else
            {
                MessageBox.Show("文件目录损坏，请重新下载", "错误");
                Application.Exit();
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string status_page = webBrowser1.Document.Body.InnerHtml;
            JObject obj = JObject.Parse(status_page);
            Server_Version = (int)obj["version"];
            Down_Url = (string)obj["updurl"];
            int Down_Ver = (int)obj["updaterver"];
            if(Local_UpdaterVer <= Down_Ver)
            {
                //
            }
            else
            {
                MessageBox.Show("服务器版本为重大更新，请重新下载软件", "提示");
                Application.Exit();
                return;
            }
            metroLabel1.Text = "服务器版本：" + Convert.ToString(Server_Version);
            GetVersion(me_path + "\\Lan-Play-Runner.exe");
            if (Local_Version < Server_Version)
            {
                MessageBox.Show("检测到新版本：Version " + Server_Version + ".0.0", "注意");
                System.Diagnostics.Process.Start(Down_Url);
                Thread.Sleep(2000);
                Application.Exit();
            }
            else
            {
                MessageBox.Show("无需更新", "错误");
                Application.Exit();
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            webBrowser1.Navigate(Server);
        }
    }
}
