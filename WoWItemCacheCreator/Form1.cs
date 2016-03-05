using WoWItemCacheCreator.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace WoWItemCacheCreator
{
    public partial class wowitemcachecreator : Form
    {
        public wowitemcachecreator()
        {
            InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            textBox3.Text = "wow_item_search_map";

            
        }

        mytimer mt = new mytimer();

        private void button2_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string path = dialog.SelectedPath;
                    textBox2.Text = path;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length == 0)
            {
                MessageBox.Show("Please select a directory to write files to.");
                return ;
            }

            progressBar1.Maximum = 100;
            progressBar1.Step = 1;
            progressBar1.Value = 0;

            // start timer
            mt.StartClock(ref label4);               

            button1.Enabled = false;
            textBox3.ReadOnly = true;
            //start the animation
            progressBar1.Style = ProgressBarStyle.Marquee;
            backgroundWorker1.RunWorkerAsync();
        }

        private string getJson(int id)
        {
            try {
                string url = "https://us.api.battle.net/wow/item/" + id + "?locale=en_US&apikey=" + textBox4.Text;
                dynamic wc = new WebClient();
                string json = wc.DownloadString(url);
                string path = textBox2.Text;
                // create insert file
                // INSERT INTO tbl_name (col1,col2) VALUES(15,col1*2);
                JObject jo = JObject.Parse(json);
                string jid = (string)jo["id"];
                string jname = (string)jo["name"];
                jname = jname.Replace("'", "''");
                string insert = "INSERT INTO " + textBox3.Text + " (id, name) VALUES('" + jid + "', '"+ jname +"')";
                SaveFile sfm = new SaveFile();
                string sfmr = sfm.writeToLogFile(insert, "mysql", path, "sql");
                // save json to file
                
                SaveFile sf = new SaveFile();
                string sfr = sf.writeToLogFile(json, id.ToString(), path, "json");
                //textBox1.Text += json;
                return "ID: " + id + " - " + sfr + " - " + json;
            }
            catch(Exception ex)
            {
                //textBox1.Text += ex.ToString();
                return "ID: " + id + " - " + ex.Message;
            }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker1.CancellationPending == true)
            {
                e.Cancel = true;
                return;
            }
            else
            {
                // here we start calling the wow servers to request item data.
                // set the end point item number. 
                int start = 101153;
                int end = 130000;
                //end = 100;
                string json = "";
                // call the class that does the searching.
                for (int i = start; i <= end; i++)
                {
                    //label1.Text = "ID: " + i;
                    json = getJson(i);
                    backgroundWorker1.ReportProgress(1, json);
                    start++;
                    //Thread.Sleep(750);
                }
            }
            
        }
        

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var info = e.Result;            

            // stop animation
            progressBar1.Style = ProgressBarStyle.Continuous;
            button1.Enabled = true;
            textBox3.ReadOnly = false;
            // stop timer
            mt.StartClock(ref label4);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressBar1.Value = e.ProgressPercentage;
            string val = e.UserState as string + Environment.NewLine;
            textBox1.AppendText(val);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
