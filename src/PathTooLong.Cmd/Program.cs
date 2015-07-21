using PathTooLong.Cmd.App;
using System;
using System.Collections.Generic;

namespace PathTooLong.Cmd {

	internal class Program {

		private static void Main(string[] args) {

			AssemblyLoader.Register();

			Run(args);
		}

		private static void Run(IEnumerable<string> args) {

			new Options(args).Process.Run();
		}
	}
}