using FluentFTP;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace viewcovidcomp
{
    public partial class Form1 : Form
    {
        public static string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CTI_settings.xml");
        public static Config.ConfigData config = Config.GetConfigData();
        //public static config = Config.GetConfigData();
        public FtpClient client = new FtpClient(config.local_gate_ip)
        {
            Credentials = new NetworkCredential(config.ftp_username, config.ftp_pass)
        };
        static System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        static DateTime dt = DateTime.Now;
        //public var timer = new System.Windows.Forms.Timer();
        public Form1()
        {
            InitializeComponent();

            if (!File.Exists(fileName)) // create config file with default values
            {
                Form2 f = new Form2();
                f.ShowDialog();
                f.Focus();
            }

            try
            {
                client.Connect();
            }
            catch
            {
                string message = "Connection Failed!" + Environment.NewLine + "Check server information and try again" + Environment.NewLine + "Err: 04";
                string title = "Error";
                MessageBox.Show(message, title,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1.Enabled = false;
                Form2 f = new Form2();
                f.ShowDialog();
                f.Focus();
            }


            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = 50; //10 seconds
            timer.Start();
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.ShowDialog();
            f.Focus();

            //Application.Run(f);
            //var fileName = Path.Combine(Environment.GetFolderPath(
            //Environment.SpecialFolder.ApplicationData), "DateLinks.xml");
        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        async void Timer_Tick(object sender, EventArgs e)
        {


            Thread.CurrentThread.IsBackground = true;
            foreach (var item in client.GetListing("/CTI/", FtpListOption.Recursive))
            {
                switch (item.Type)
                {

                    case FtpFileSystemObjectType.File:

                        //listBox1.Items +=  item.FullName + Environment.NewLine;
                        if (!listBox1.Items.Contains(item.FullName.Remove(0, 5))) // case sensitive is not important
                            listBox1.Items.Add(item.FullName.Remove(0, 5));
                        //str = str.Remove(0, 10);
                        break;

                    case FtpFileSystemObjectType.Link:
                        break;
                }
            }
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /* FtpClient client = new FtpClient(config.local_gate_ip);
             client.Credentials = new NetworkCredential(config.ftp_username, config.ftp_pass);
             client.Connect();
             foreach (var item in client.GetListing("/CTI/", FtpListOption.Recursive))
             {
                 switch (item.Type)
                 {

                     case FtpFileSystemObjectType.File:

                         //listBox1.Items +=  item.FullName + Environment.NewLine;

                         listBox1.Items.Add(item.FullName);

                         break;

                     case FtpFileSystemObjectType.Link:
                         break;
                 }
             }

             */
            // if this is a file

            // richTextBox1.Text += Environment.NewLine + textBox1.Text;
            // get the file size


        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            Config.ConfigData config = Config.GetConfigData();
            config = Config.GetConfigData();
            FtpClient client = new FtpClient(config.local_gate_ip);
            client.Credentials = new NetworkCredential(config.ftp_username, config.ftp_pass);

            try
            {
                client.Connect();
            }
            catch
            {
                string message = "Connection Failed!" + Environment.NewLine + "Check server information and try again" + Environment.NewLine + "Err: 04";
                string title = "Error";
                MessageBox.Show(message, title,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1.Enabled = false;
                Form2 f = new Form2();
                f.ShowDialog();
                f.Focus();
            }
            //Stream toimg = client.Download(toimg, "");
            System.IO.Stream toimg = new System.IO.MemoryStream();
            if (client.Download(toimg, "/CTI/" + listBox1.SelectedItem.ToString()))
            {
                var image = Image.FromStream(toimg);
                pictureBox1.Image = image;
            }
            else
            {
                string message = "File Not Found!" + Environment.NewLine + "Err: 03";
                string title = "Error";
                MessageBox.Show(message, title,
                MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            //client.DeleteFile(listBox1.SelectedItem.ToString());

        }



        private void button1_Click(object sender, EventArgs e)
        {



            if (button1.Text == "Turn Refresh On")
            {
                timer.Start();
                button1.Text = "Turn Refresh Off";
            }
            else if (button1.Text == "Turn Refresh Off")
            {
                timer.Stop();
                button1.Text = "Turn Refresh On";
            }

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 f = new AboutBox1();
            f.ShowDialog();
            f.Focus();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
