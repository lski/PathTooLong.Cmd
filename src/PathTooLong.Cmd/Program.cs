using PathTooLong.Cmd.App;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PathTooLong.Cmd {

	internal class Program {

		private static void Main(string[] args) {

			AssemblyLoader.Register();

			Run(args);
		}

		private static void Run(IEnumerable<string> args) {

			var options = new Options(args);

			options.Process.Run();
		}
	}
}