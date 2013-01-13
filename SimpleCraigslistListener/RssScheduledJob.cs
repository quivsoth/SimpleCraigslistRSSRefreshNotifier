using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCraigslistListener
{
    public class RSSScheduledJob : IJob
    {
        public RSSScheduledJob() { }

        public void Execute(IJobExecutionContext context)
        {
            Program.QueryRSSCollections();
        }
    } 
}
