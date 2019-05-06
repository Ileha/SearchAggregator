using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchAggregator.DataModel
{
    public class DBRepository
    {
        public static readonly char[] SPLITTER = new char[] { ' ' };

        /*
         * функции для добавления ссылок
         */
        public static void addLink(Link link) {
            ConnectToDB((connect) =>
            {
                connect.links.Add(link);
                connect.SaveChanges();
            });
        }
        public static void addLink(params Link[] link) {
            ConnectToDB((connect) => {
                connect.links.AddRange(link);
                connect.SaveChanges();
            });
        }
        public static void addLink(Query query) {
            ConnectToDB((connect) =>
            {
                connect.queries.Add(query);
                connect.SaveChanges();
            });
        }

        /*
         * ищет вхождение слов из запроса в базе данных
         */
        public static ICollection<Link> findByQuery(String query) {
            List<Link> links = null;
            String[] words = query.Trim().Split(SPLITTER, StringSplitOptions.RemoveEmptyEntries);
            ConnectToDB((connect) => {
                string word = words[0];
                IQueryable<Link> queryToDb = connect.links.Where(l => l.title.Contains(word));
                for (int i = 1; i < words.Length; i++)
                {
                    string wordsearch = words[i];
                    queryToDb = queryToDb.Intersect(connect.links.Where(l => l.title.Contains(wordsearch)));
                }
                links = queryToDb.ToList().Distinct().ToList();
            });
            return links;
        }

        /*
         * функция для взаимодействия с базой данных
         * создаёт контекст и вызывает action передавая в неё созданный контекст
         */
        public static void ConnectToDB(Action<DBConnection> action) {
            using (DBConnection connect = new DBConnection(Program.config.DBConnectionString)) {
                //connect.Database.Log = (s => Console.WriteLine(s)); //лог
                action(connect);
            }
        }
    }
}
