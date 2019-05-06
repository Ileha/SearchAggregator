using System;

namespace CommadInterfaces.Exceptions
{
    public class CommandNotFound : Exception {
        private string Message;

        public CommandNotFound(string comand_name) : base("comand " + comand_name + " was not found") {
            Message = "comand " + comand_name + " was not found";
        }

        public override string ToString() {
            return Message;
        }
    }
	//public class ArgumentCountExeption : Exception
	//{
	//	private string Message;

	//	public ArgumentCountExeption(int arg_count)
	//	{
	//		Message = "you need " + arg_count + " arguments";
	//	}

	//	public override string ToString()
	//	{
	//		return Message;
	//	} 
	//}
}
