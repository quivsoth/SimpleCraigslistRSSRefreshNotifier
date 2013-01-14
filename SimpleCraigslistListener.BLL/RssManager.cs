using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PK.SimpleCraigslistListener.BLL
{

    /// <summary>
    /// RSS manager to read rss feeds
    /// </summary>
    public static class RssManager
    {
        /// <summary>
        /// Reads the relevant Rss feed and returns a list off RssFeedItems
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static List<RssFeedItem> ReadFeed(string url)
        {
            //create a new list of the rss feed items to return
            List<RssFeedItem> rssFeedItems = new List<RssFeedItem>();

            //create an http request which will be used to retrieve the rss feed
            HttpWebRequest rssFeed = (HttpWebRequest)WebRequest.Create(url);

            //use a dataset to retrieve the rss feed
            using (DataSet rssData = new DataSet())
            {
                //read the xml from the stream of the web request
                rssData.ReadXml(rssFeed.GetResponse().GetResponseStream());

                //loop through the rss items in the dataset and populate the list of rss feed items
                foreach (DataRow dataRow in rssData.Tables["item"].Rows)
                {
                    rssFeedItems.Add(new RssFeedItem
                    {
                        ChannelId = Convert.ToInt32(dataRow["channel_Id"]),
                        Description = Convert.ToString(dataRow["description"]),
                        ItemId = Convert.ToInt32(dataRow["item_Id"]),
                        Link = Convert.ToString(dataRow["link"]),
                        PublishDate = Convert.ToDateTime(dataRow["pubDate"]),
                        Title = Convert.ToString(dataRow["title"])
                    });
                }
            }

            //return the rss feed items
            return rssFeedItems;
        }

        public static List<RssFeedItem> GetItemFeed(string url)
        {
            List<RssFeedItem> feed = new List<RssFeedItem>();

            XmlDocument doc = new XmlDocument();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            nsmgr.AddNamespace("rss", "http://purl.org/rss/1.0/");
            nsmgr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
            XmlTextReader reader = new XmlTextReader(url);
            doc.Load(reader);
            XmlNodeList nodes = doc.SelectNodes("/rdf:RDF//rss:item", nsmgr);

            foreach (XmlNode node in nodes)
            {
                XmlNodeList titleNode = node.SelectNodes("rss:title", nsmgr);
                XmlNodeList descNode = node.SelectNodes("rss:description", nsmgr);
                XmlNodeList linkNode = node.SelectNodes("rss:link", nsmgr);
                XmlNodeList dateNode = node.SelectNodes("dc:date", nsmgr);

                string title = titleNode.Count == 0 ? "" : titleNode[0].InnerText;
                string desc = descNode.Count == 0 ? "" : descNode[0].InnerText;
                string link = linkNode.Count == 0 ? "" : linkNode[0].InnerText;
                string date = dateNode.Count == 0 ? "" : dateNode[0].InnerText;

                RssFeedItem item = new RssFeedItem()
                {
                    Description = desc,
                    Title = title,
                    Link = link,
                    PublishDate = DateTime.Parse(date)
                };
                feed.Add(item);
            }
            return feed;
        }
    }
}