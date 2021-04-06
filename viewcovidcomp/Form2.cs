using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Net;
using FluentFTP;

namespace viewcovidcomp
{
    public partial class Form2 : Form
    {
        public static string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CTI_settings.xml");

        public Form2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {




            if (!File.Exists(fileName)) // create config file with default values
            {

                string message = "You need to verify server connection information!" + Environment.NewLine + "Err: 01";
                string title = "Error";
                MessageBox.Show(message, title,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.Close();
            }

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // get configuration
            Config.ConfigData config = Config.GetConfigData();


            // change data
            config.local_gate_ip = textBox1.Text;
            config.ftp_username = textBox3.Text;
            config.ftp_pass = textBox4.Text;
            // save config
            Config.SaveConfigData(config);
            this.Close();
            Application.Restart();
            Environment.Exit(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Config.ConfigData config = Config.GetConfigData();
            config = Config.GetConfigData();

            // create an FTP client
            FtpClient client = new FtpClient(textBox1.Text);

            // specify the login credentials, unless you want to use the "anonymous" user account
            client.Credentials = new NetworkCredential(textBox3.Text, textBox4.Text);

            // begin connecting to the server

            try
            {
                client.Connect();
                string message = "Connection Sucessful!" + Environment.NewLine + "Click Confirm to save the settings" + Environment.NewLine + "and you can proceed with the program";
                string title = "Information";
                MessageBox.Show(message, title,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                button1.Enabled = true;
            }
            catch
            {
                string message = "Connection Failed!" + Environment.NewLine + "Check server information and try again" + Environment.NewLine + "Err: 02";
                string title = "Error";
                MessageBox.Show(message, title,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1.Enabled = false;
            }




        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = false;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = false;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = false;
        }
    }
    }

