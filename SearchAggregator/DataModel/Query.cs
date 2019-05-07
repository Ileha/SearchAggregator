using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SearchAggregator.DataModel
{
    public class Query
    {
        [Key]
        public int queyId { get; set; }
        public virtual ICollection<Link> links { get; set; }
        [Required]
        public string title { get; set; }

        public Query() {}

        public Query(String title, ICollection<Link> queryContent) {
            this.title = title;
            links = queryContent;
        }

        public override bool Equals(object obj) {
            if (obj == null) { return false; }
            if (obj == this) { return true; }

            Query other = obj as Query;
            if (other == null) {
                return false;
            }
            else {
                if (other.links.Count != links.Count) { return false; }
                Link[] res = (from l in other.links
                              orderby l.title
                              select l).ToArray();
                foreach (Link l in links) {
                    int index = Array.BinarySearch<Link>(res, l);
                    if (index < 0) { return false; }
                }
                return true;
            }
        }
    }
}
