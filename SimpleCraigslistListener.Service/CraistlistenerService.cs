using PK.SimpleCraigslistListener.BLL;
using PK.SimpleCraigslistListener.Utilities;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using config = PK.SimpleCraigslistListener.BLL.Properties.application;

namespace SimpleCraigslistListener.Service
{
    public partial class CraistlistenerService : ServiceBase
    {
        public static List<RssFeedItem> Feed = new List<RssFeedItem>();

        public CraistlistenerService()
        {
            InitializeComponent();
            Feed = RssManager.GetItemFeed(config.Default.rssLocation1).Take(config.Default.searchResult).ToList();
        }

        protected override void OnStart(string[] args)
        {
            Scheduler();
        }

        protected override void OnStop()
        {
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
                .StartAt(DateBuilder.FutureDate(config.Default.jobStartUpTime, IntervalUnit.Second))
                .WithSimpleSchedule(x => x.RepeatForever().WithIntervalInSeconds(config.Default.jobIntervalScan))
                .Build();

            sched.ScheduleJob(job, trigger);
            sched.Start();
        }

        public static void Scan(List<RssFeedItem> feed)
        {
            List<RssFeedItem> newFeed = RssManager.GetItemFeed(config.Default.rssLocation1).Take(config.Default.searchResult).ToList();
            List<RssFeedItem> result = newFeed.Except(feed).ToList();
            CraistlistenerService.Feed = newFeed;

            if (result.Count == 0) Console.Write(".");
            else
            {
                Console.WriteLine("New feed(s) sent!");
                Utilities.SendRssManifest(result);
            }
        }

        public class RSSScheduledJob : IJob
        {
            public RSSScheduledJob() { }

            public void Execute(IJobExecutionContext context)
            {
                Scan(CraistlistenerService.Feed);
            }
        }
    }
}
