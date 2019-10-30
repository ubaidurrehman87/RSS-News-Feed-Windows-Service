using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using System.Configuration;
using System.Xml.Serialization;
using System.ServiceModel.Syndication;
using System.IO;

namespace K164037_Q4
{
    public partial class Service1 : ServiceBase
    {
        public readonly Timer _timer;
        public Service1()
        {
            InitializeComponent();
            _timer = new Timer(300000) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;

        }
       
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            //The News
            // XmlSerializer serialize = new XmlSerializer(typeof(NewsItem));
            string news1_path = ConfigurationManager.AppSettings["news1"];
            string news2_path = ConfigurationManager.AppSettings["news2"];
            string filePath = ConfigurationManager.AppSettings["filepath"];

            using (XmlReader read = XmlReader.Create(news1_path))
            {
                SyndicationFeed feed = SyndicationFeed.Load(read);
                DateTime[] date = new DateTime[feed.Items.Count()];


                DateTime temp;
                using (StreamWriter NewsPaper = File.AppendText(filePath + "/NewFeed.txt"))
                {
                    int counter = 0;
                    foreach (SyndicationItem item in feed.Items)
                    {
                        date[counter] = item.PublishDate.DateTime;

                        counter++;
                        for (int i = 0; i < counter - 1; i++)
                        {
                            for (int j = i + 1; j < counter; j++)
                            {
                                int result = DateTime.Compare(date[i], date[j]);
                                if (result > 0)
                                {
                                    temp = date[i];
                                    date[i] = date[j];
                                    date[j] = temp;
                                }

                            }
                        }
                    }
                    counter = 0;
                    //XmlWriter Newsfile = XmlWriter.Create(filePath + "/NewFeed.xml");
                    //Newsfile.WriteStartDocument();
                    XmlSerializer serializer = new XmlSerializer(typeof(NewsItem));
                    NewsItem news = new NewsItem();

                    foreach (SyndicationItem item in feed.Items)
                    {
                        // Newsfile.WriteStartAttribute("NewsItem");
                        NewsPaper.WriteLine(date[counter]);

                        foreach (SyndicationItem item2 in feed.Items)
                        {
                            int result = DateTime.Compare(date[counter], item2.PublishDate.DateTime);
                            if (result == 0)
                            {
                                NewsPaper.WriteLine(item2.Title.Text);
                                news.Title = item2.Title.Text;
                                // Newsfile.WriteElementString("Title", item2.Title.Text);
                            }
                        }
                        news.PublishedDate = date[counter].ToString();
                        news.NewsChannel = "The News";
                        // Newsfile.WriteElementString("PublishedDate", date[counter].ToString());
                        //Newsfile.WriteElementString("NewsChannel", item.Copyright.Text);
                        counter++;
                        using (FileStream fs = new FileStream(filePath + "NewsFeed2.xml", FileMode.Create, FileAccess.Write))
                        {
                            serializer.Serialize(fs, news);
                            Console.WriteLine("File Created Successfully");

                        }
                        //Newsfile.WriteEndAttribute();
                    }
                    //Newsfile.WriteEndDocument();

                }
            }

            using (XmlReader read = XmlReader.Create(news2_path))
            {
                SyndicationFeed feed = SyndicationFeed.Load(read);
                DateTime[] date = new DateTime[feed.Items.Count()];


                DateTime temp;
                using (StreamWriter NewsPaper = File.AppendText(filePath + "/NewFeed.txt"))
                {
                    int counter = 0;
                    foreach (SyndicationItem item in feed.Items)
                    {
                        date[counter] = item.PublishDate.DateTime;

                        counter++;
                        for (int i = 0; i < counter - 1; i++)
                        {
                            for (int j = i + 1; j < counter; j++)
                            {
                                int result = DateTime.Compare(date[i], date[j]);
                                if (result > 0)
                                {
                                    temp = date[i];
                                    date[i] = date[j];
                                    date[j] = temp;
                                }

                            }
                        }
                    }
                    counter = 0;
                    //XmlWriter Newsfile = XmlWriter.Create(filePath + "/NewFeed.xml");
                    //Newsfile.WriteStartDocument();
                    XmlSerializer serializer = new XmlSerializer(typeof(NewsItem));
                    NewsItem news = new NewsItem();

                    foreach (SyndicationItem item in feed.Items)
                    {
                        // Newsfile.WriteStartAttribute("NewsItem");
                        NewsPaper.WriteLine(date[counter]);

                        foreach (SyndicationItem item2 in feed.Items)
                        {
                            int result = DateTime.Compare(date[counter], item2.PublishDate.DateTime);
                            if (result == 0)
                            {
                                NewsPaper.WriteLine(item2.Title.Text);
                                news.Title = item2.Title.Text;
                                // Newsfile.WriteElementString("Title", item2.Title.Text);
                            }
                        }
                        news.PublishedDate = date[counter].ToString();
                        news.NewsChannel = "The News";
                        // Newsfile.WriteElementString("PublishedDate", date[counter].ToString());
                        //Newsfile.WriteElementString("NewsChannel", item.Copyright.Text);
                        counter++;
                        using (FileStream fs = new FileStream(filePath + "NewsFeed.xml", FileMode.Create, FileAccess.Write))
                        {
                            serializer.Serialize(fs, news);
                            Console.WriteLine("File Created Successfully");

                        }
                        //Newsfile.WriteEndAttribute();
                    }
                    //Newsfile.WriteEndDocument();

                }
            }
        }

        protected override void OnStart(string[] args)
        {
            _timer.Start();
        }

        protected override void OnStop()
        {
            _timer.Stop();
        }
    }
    public class NewsItem{

        public string Title { get; set; }
        public string PublishedDate { get; set; }
        public string NewsChannel { get; set; }
    }
}
