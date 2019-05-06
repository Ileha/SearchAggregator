using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAggregator.DataModel
{
    public class Link : IComparable<Link>
    {
        public static readonly char[] SPLITTER = new char[] { ' ' };

        [Key]
        public int linkId { get; set; }
        public String link { get; set; }
        public String title { get; set; }

        public Link() {}
        public Link(String title, String link)
        {
            this.title = title;
            this.link = link;
        }

        public override bool Equals(object obj) {
            if (obj == null) { return false; }
            if (obj == this) { return true; }

            Link other = obj as Link;
            if (other == null) {
                return false;
            }
            else {
                return other.link == link && other.title == title;
            }
        }

        public override int GetHashCode() {
            return title.GetHashCode() + 7 * link.GetHashCode();
        }

        public override string ToString() {
            return String.Format("title: {0},\tlink: {1}", title, link);
        }

        public int CompareTo(Link other) {
            return String.Compare(title, other.title);
        }
    }
}
