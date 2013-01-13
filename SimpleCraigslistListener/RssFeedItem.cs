using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCraigslistListener
{

    /// <summary>
    /// RSS feed item entity
    /// </summary>
    public class RssFeedItem : IEquatable<RssFeedItem>
    {

        /// <summary>
        /// Gets or sets the rss target url
        /// </summary>
        public string TargetUrl { get; set; }
        
        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the link
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the item id
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets the publish date
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// Gets or sets the channel id
        /// </summary>
        public int ChannelId { get; set; }

        /// <summary>
        /// Compares the RSS published date between objects
        /// </summary>
        /// <returns>Difference true if same</returns>
        public bool Equals(RssFeedItem other)
        {
            if (other == null)
                return false;

            if (this.Title == other.Title && this.Link == other.Link) return true;
            else return false;
        }

        /// <summary>
        /// Compares the RSS published date between objects
        /// </summary>
        /// <returns>Difference true if same</returns>
        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            RssFeedItem RssFeedItemObj = obj as RssFeedItem;
            if (RssFeedItemObj == null)
                return false;
            else
                return Equals(RssFeedItemObj);
        }

        /// <summary>
        /// default override for iequatable
        /// </summary>
        /// <returns>hash value</returns>
        public override int GetHashCode()
        {
            return this.Title.GetHashCode();
        }
    }
}
