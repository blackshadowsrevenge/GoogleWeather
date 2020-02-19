using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoogleWeather
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Icon baseIcon;

        private void Form1_Load(object sender, EventArgs e)
        {
            baseIcon = notifyIcon1.Icon;
            updateWeather();
            
            }



        public string findTemp(string strSource, string strStart, string strEnd)
        {
            string rtString = "--";
            if (strSource.Contains(strEnd))
            {
                int end = strSource.IndexOf(strEnd);
                rtString = strSource.Remove(0, (end - 5));
                rtString = rtString.Remove(5);
                end = rtString.LastIndexOf(strStart);
                rtString = rtString.Remove(0, end + 1);
            }

            return rtString;
        }



        public string findWeatherPicURL(string strSource, string keyStr, string strEnd)
        {
            //System.Console.WriteLine("****findWeatherPicURL");
            string urlStr = "//ssl.gstatic.com/onebox/weather/64/partly_cloudy.png";
            if (strSource.Contains(keyStr))
            {
                int start = strSource.IndexOf(keyStr);
                urlStr = strSource.Remove(0, start);
                urlStr = urlStr.Remove(70);
            //    System.Console.WriteLine("****findWeatherPicURL: " + urlStr);
                int end = urlStr.IndexOf(strEnd);
                urlStr = urlStr.Remove(end + strEnd.Length);
            //    System.Console.WriteLine("****findWeatherPicURL: " + urlStr);
            }
            //return "https://ssl.gstatic.com/onebox/weather/64/partly_cloudy.png";
            return ("https:" + urlStr);
        }



        public void updateWeather()
        {
            System.Console.WriteLine("updateWeather()");
            string webData = "??";
            System.Net.WebClient wc = new System.Net.WebClient();

            Task t = Task.Run(() =>
            {
                try {
                    byte[] raw = wc.DownloadData("https://www.google.com/search?q=weather+&ie=utf-8&oe=utf-8");
                    webData = System.Text.Encoding.UTF8.GetString(raw);
                    pictureBox1.Load(findWeatherPicURL(webData, "//ssl.gstatic.com/onebox/weather/", ".png"));
                }
                catch {; }
            });
            t.Wait();
            label1.Text = findTemp(webData, ">", "°F") + "°F";
            notifyIcon1.Text = "Temp: " + label1.Text;

            

            Graphics canvas = null;
            Bitmap iconBitmap = new Bitmap(16, 16);
            canvas = Graphics.FromImage(iconBitmap);
            //Icon icon = new Icon(notifyIcon1.Icon, 16 , 16);
            canvas.DrawIcon(baseIcon , -12, -12);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;

            
            canvas.DrawString(label1.Text, 
                new Font("Calibri", 10, FontStyle.Bold),
                new SolidBrush(Color.FromArgb(40, 40, 40)),
                new RectangleF(-4, 0, 26, 23),
                format
            );

            notifyIcon1.Icon = Icon.FromHandle(iconBitmap.GetHicon());

        }


        private void timer1_Tick_1(object sender, EventArgs e)
        {
            updateWeather();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            updateWeather();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Hide();
            this.Show();
            this.Focus();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            updateWeather();
        }
    }
}

