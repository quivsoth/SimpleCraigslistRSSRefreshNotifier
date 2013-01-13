using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using config = SimpleCraigslistListener.Properties.application;

namespace SimpleCraigslistListener
{
    class Program
    {
        public static List<RssFeedItem> Feed = new List<RssFeedItem>();

        static void Main(string[] args)
        {
            try
            {
                ConsoleMessages();
                Feed = RssManager.GetItemFeed(config.Default.rssLocation1).Take(config.Default.searchResult).ToList();
                //Utilities.SendRssManifest(Feedx);
                Scheduler();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ConsoleMessages()
        {
            String configSettings = "Craigslist RSS Listener\n--\nWith Config:\nJob Start up (Seconds): " +
            "{0}\nJob Scan Interval (Seconds): {1}\nMail Server: {2}\nMail Server Username: {3}\n" +
            "Mail Server Password: *************\nDestination Email: {4}\n# of Search results: {5}\nRSS Feed: {5}\n--";
            
            Console.WriteLine(String.Format(configSettings, 
                                                config.Default.jobStartUpTime.ToString(), 
                                                config.Default.jobIntervalScan.ToString(),
                                                config.Default.smtp,
                                                config.Default.smtpUser,
                                                config.Default.destinationEmail,
                                                config.Default.searchResult,
                                                config.Default.rssLocation1));
        }

        //private static void BuildRSSCollection(string location)
        //{
        //    if (!String.IsNullOrEmpty((location)))
        //    {
        //        var feed = new List<RssFeedItem>();
        //        feed = RssManager.GetItemFeed(config.Default.rssLocation1).Take(config.Default.searchResult).ToList();
        //        Feed.Add(feed, config.Default.rssLocation1);
        //    }
        //}

        public static void QueryRSSCollections()
        {
            Scan(Program.Feed);
        }

        public static void Scan(List<RssFeedItem> feed)
        {
            List<RssFeedItem> newFeed = RssManager.GetItemFeed(config.Default.rssLocation1).Take(config.Default.searchResult).ToList();
            List<RssFeedItem> result = newFeed.Except(feed).ToList();
            Program.Feed = newFeed;

            if (result.Count == 0)  Console.Write(".");
            else
            {
                Console.WriteLine("New feed(s) sent!");
                Utilities.SendRssManifest(result);
            }
        }

        public static void Scheduler()
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sched = sf.GetScheduler();
            IJobDetail job = JobBuilder.Create<RSSScheduledJob>()
                                        .WithIdentity("craigslistFeedJob", "pk.cg.org")
                                        .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("craigslistRssTrigger", "pk.cg.org")
                .StartAt(DateBuilder.FutureDate(Properties.application.Default.jobStartUpTime, IntervalUnit.Second))
                .WithSimpleSchedule(x => x.RepeatForever().WithIntervalInSeconds(Properties.application.Default.jobIntervalScan))
                .Build();

            sched.ScheduleJob(job, trigger);
            sched.Start();
        }
    }
}