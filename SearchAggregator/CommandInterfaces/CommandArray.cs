using System;
using System.Collections.Generic;
using CommadInterfaces.Exceptions;

namespace CommadInterfaces
{
    public class CommandArray<T> {
        private Dictionary<string, Command<T>> Repository;

        public CommandArray() {
            Repository = new Dictionary<string, Command<T>>();
        }

        public void AddCommand(Action<Command<T>> commad_config) {
            Command<T> for_add = new Command<T>();
            commad_config.Invoke(for_add);
            Repository.Add(for_add.Name, for_add);
        }

        public Command<T> GetCommand(string name) {
            Command<T> exe;
            try {
                exe = Repository[name];
            }
            catch (Exception err) {
                throw new CommandNotFound(name);
            }
            return exe;
        }

        public void Execute(string name, params T[] arguments) {
            Command<T> exe = GetCommand(name);
            exe.OnExecute(arguments);
        }
    }
}
