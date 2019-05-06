using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using SearchAggregator.DataModel;
using System.Data.SqlClient;
using CommadInterfaces;
using SearchAggregator.Search;
using SearchAggregator.Search.google;
using System.Data.Entity;
using System.Text;

namespace SearchAggregator
{
    class Program
    {
        public const string CONFIG_PATH = "./config.json";

        public static Config config { get; private set; }
        private static CommandArray<CommandData> commandBank;
        private static bool execute = true;
        private static ISearch[] webSearchers;

        static void Main(string[] args)
        {
            config = readConfig(CONFIG_PATH);
            commandBank = configureCommands();
            webSearchers = createSearchers();

            Console.OutputEncoding = Encoding.UTF8;

            while (execute)
            {
                Console.WriteLine("Enter command");
                string cmd = Console.ReadLine();
                CommandData cmdData = new CommandData(cmd);
                try
                {
                    commandBank.Execute(cmdData.Cmd, cmdData);
                }
                catch (Exception err) {
                    Console.WriteLine(err);
                }
            }

            Console.WriteLine("done");
            Console.Read();
        }

        /*
         * функция для загрузки файла конфигурации
         * в котором записанны сторока подключения к БД и всякое нужное для работы с google custom search api
         */
        static Config readConfig(String path) {
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Config));
            Config result = null;

            try {
                using (FileStream fs = new FileStream(path, FileMode.Open)) {
                    result = jsonFormatter.ReadObject(fs) as Config;
                }
            }
            catch (IOException e) {
                throw e;
            }
            return result;
        }

        static ISearch[] createSearchers() {
            ISearch[] result = new ISearch[1];
            result[0] = new GoogleSearch();
            return result;
        }
        static CommandArray<CommandData> configureCommands() {
            CommandArray<CommandData> cmds = new CommandArray<CommandData>();

            cmds.AddCommand(cmd => {
                cmd.Name = "search";
                CommandDataPattern pattern = new CommandDataPattern();
                pattern.AddOption('l', false);
                pattern.AddOption('w', true);
                pattern.AddOption('a', false);
                cmd.Execute = (arguments) => {
                    CommandData argument = arguments[0];
                    argument.SetPattern(pattern);
                    Stream outStream = null;
                    if (argument.IsKey('w')) {
                        FileMode mode = FileMode.Create;
                        if (argument.IsKey('a')) {
                            mode = FileMode.Append;
                        }
                        outStream = new FileStream(argument.GetKeyValue('w'), mode);
                    }
                    else {
                        outStream = Console.OpenStandardOutput();
                    }
                    String searchPhrase = argument.args[0];
                    using (outStream) {
                        if (argument.IsKey('l'))
                        {//поиск в БД
                            writeToStream(outStream, DBRepository.findByQuery(searchPhrase), searchPhrase);
                        }
                        else
                        {//запрос к гуглу
                            Query searchResult = webSearchers[0].search(searchPhrase);
                            DBRepository.ConnectToDB(connect => {
                                Query query = (from q in connect.queries
                                         where q.title == searchPhrase
                                         select q).Include("links").FirstOrDefault();
                                if (query != null) {
                                    if (!searchResult.Equals(query)) {
                                        Console.WriteLine("update data base");
                                        connect.queries.Remove(query);
                                        connect.queries.Add(searchResult);
                                        connect.SaveChanges();
                                    }
                                }
                                else {
                                    Console.WriteLine("write to data base");
                                    connect.queries.Add(searchResult);
                                    connect.SaveChanges();
                                }
                            });
                            writeToStream(outStream, searchResult.links, searchPhrase);
                        }
                    }
                };
            });

            cmds.AddCommand(cmd => {
                cmd.Name = "exit";
                cmd.Execute = (arguments) => {
                    Console.WriteLine("exiting...");
                    execute = false;
                };
            });

            return cmds;
        }

        static void writeToStream(Stream outStream, ICollection<Link> links, String query) {
            using (StreamWriter sw = new StreamWriter(outStream)) {
                sw.WriteLine(String.Format("query: {0}\tcount: {1}\ttime {2}", query, links.Count, DateTime.Now));

                foreach (Link link in links) {
                    sw.WriteLine(link);
                }
                sw.WriteLine();
            }
        }
    }
}
