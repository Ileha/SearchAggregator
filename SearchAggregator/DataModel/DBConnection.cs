using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace SearchAggregator.DataModel
{
    public class DBConnection : DbContext
    {
        public DbSet<Link> links { get; set; }
        public DbSet<Query> queries { get; set; }

        public DBConnection(String connectionString)
            : base(connectionString)
        {
            
        }
    }
}
