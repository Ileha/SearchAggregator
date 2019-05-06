using System;

namespace CommadInterfaces {
	

    public class Command<T> {
		public delegate void DCommand(params T[] arguments);

        public string Name;
        public DCommand Execute;

        public void OnExecute(params T[] arguments) {
            Execute(arguments);
        }
    }
}
