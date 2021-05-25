using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace google_map_scraper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

           


            



        }

        void threadMethod()
        {

            var chromeDriver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), new ChromeOptions(), TimeSpan.FromSeconds(180));

            chromeDriver.Navigate().GoToUrl("https://www.google.lk/maps/");

            chromeDriver.FindElementById("gs_lc50").FindElement(By.TagName("input")).SendKeys("colombo hospitals");

            chromeDriver.FindElementById("searchbox-searchbutton").Click();

            while (true)
            {
                var nodes = chromeDriver.FindElementsByCssSelector("div[class = 'V0h1Ob-haAclf gd9aEe-LQLjdd OPZbO-KE6vqe']");

                foreach (var node in nodes)
                {
                    node.Click();
                    string name = chromeDriver.FindElementByClassName("section-hero-header-title-title gm2-headline-5").Text;
                    string address = chromeDriver.FindElementsByCssSelector("div[jstcache='1392']")[0].Text;
                    string webssite = chromeDriver.FindElementsByCssSelector("div[jstcache='1392']")[1].Text;
                    string phone = chromeDriver.FindElementsByCssSelector("div[jstcache='1392']")[2].Text;

                    string line = string.Format("{0},{1},{2},{3}", name, address, webssite, phone);
                    List<string> list = new List<string>();
                    list.Add(line);

                    File.WriteAllLines("data.txt", list.ToArray());
                    chromeDriver.Navigate().Back();
                }

                try
                {
                    chromeDriver.FindElementById("ppdPk-Ej1Yeb-LgbsSe-tJiF1e").Click();
                }
                catch
                {

                    break;
                }
            }

        }


        Thread thread = null;

        //stop
        private void button2_Click(object sender, EventArgs e)
        {
            thread.Abort();
        }

        //start
        private void button1_Click(object sender, EventArgs e)
        {
            thread = new Thread(threadMethod);
            thread.Start();
        }
    }
}
