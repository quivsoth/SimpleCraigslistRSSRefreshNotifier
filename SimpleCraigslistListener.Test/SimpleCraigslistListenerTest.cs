using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCraigslistListener.Test
{
    [TestClass]
    public class SimpleCraigslistListenerTest
    {

        public List<RssFeedItem> preFeed = new List<RssFeedItem>();
        List<RssFeedItem> postFeed = new List<RssFeedItem>();

        public SimpleCraigslistListenerTest()
        {
            RssFeedItem item1 = new RssFeedItem() { Title = "test item 1", Description = "first item", Link = "phoenix.craigslist.org/1", PublishDate = new DateTime(2012, 01, 01) };
            RssFeedItem item2 = new RssFeedItem() { Title = "test item 2", Description = "second item", Link = "phoenix.craigslist.org/2", PublishDate = new DateTime(2012, 02, 01) };
            RssFeedItem item3 = new RssFeedItem() { Title = "test item 3", Description = "third item", Link = "phoenix.craigslist.org/3", PublishDate = new DateTime(2012, 03, 01) };
            RssFeedItem item4 = new RssFeedItem() { Title = "test item 4", Description = "fourth item", Link = "phoenix.craigslist.org/4", PublishDate = new DateTime(2012, 04, 01) };
            RssFeedItem item5 = new RssFeedItem() { Title = "test item 5", Description = "fifth item", Link = "phoenix.craigslist.org/5", PublishDate = new DateTime(2012, 05, 01) };
            RssFeedItem item6 = new RssFeedItem() { Title = "test item 6", Description = "6 item", Link = "phoenix.craigslist.org/6", PublishDate = new DateTime(2012, 06, 01) };
            RssFeedItem item7 = new RssFeedItem() { Title = "test item 7", Description = "7 item", Link = "phoenix.craigslist.org/7", PublishDate = new DateTime(2012, 07, 01) };
            RssFeedItem item8 = new RssFeedItem() { Title = "test item 8", Description = "8 item", Link = "phoenix.craigslist.org/8", PublishDate = new DateTime(2012, 08, 01) };
            RssFeedItem item9 = new RssFeedItem() { Title = "test item 9", Description = "9 item", Link = "phoenix.craigslist.org/9", PublishDate = new DateTime(2012, 09, 01) };
            RssFeedItem item10 = new RssFeedItem() { Title = "test item 10", Description = "10 item", Link = "phoenix.craigslist.org/10", PublishDate = new DateTime(2012, 010, 01) };

            preFeed.Add(item1);
            preFeed.Add(item2);
            preFeed.Add(item3);
            preFeed.Add(item4);
            preFeed.Add(item5);

            postFeed.Add(item1);
            postFeed.Add(item2);
            postFeed.Add(item3);
            postFeed.Add(item4);
            postFeed.Add(item6);
        }

        [TestMethod]
        public void SendEmail() {
            RssFeedItem sampleItem = new RssFeedItem() { Title = "test item 6", Description = "6 item", Link = "phoenix.craigslist.org/6", PublishDate = new DateTime(2012, 06, 01) };

            string subject = "Craigslist notification, a new post detected - " + sampleItem.Link;
            string body = "A new item has been detected!! <br><br> <a href=\"" + sampleItem.Link + "\">" + sampleItem.Title + "</a><br>" + sampleItem.Description + "";

            SimpleCraigslistListener.Utilities.SendMail(subject, body);
            //SimpleCraigslistListener.Program.SendMail(sampleItem, "custom body");
        }

        [TestMethod]
        public void CompareRssFeedItem()
        {
            RssFeedItem item6 = new RssFeedItem() { Title = "test item 6", Description = "6 item", Link = "phoenix.craigslist.org/6", PublishDate = new DateTime(2012, 06, 01) };
            List<RssFeedItem> result = postFeed.Except(preFeed).ToList();
            RssFeedItem resultDif = result.First();
            Assert.AreEqual(item6.Link, resultDif.Link);
            Assert.AreEqual(item6.Description, resultDif.Description);
            Assert.AreEqual(item6.PublishDate, resultDif.PublishDate);
        }
    }
}