using System;

namespace PathTooLong.Cmd.App {

	public class Output : IOutput {

		public Output WriteExtraHighlight(string mess, params object[] args) {

			Console.ForegroundColor = ConsoleColor.Yellow;
			Write(mess, args);
			Console.ResetColor();

			return this;
		}

		public Output WriteHighlight(string mess, params object[] args) {

			Console.ForegroundColor = ConsoleColor.White;
			Write(mess, args);
			Console.ResetColor();

			return this;
		}

		public Output WriteWarning(string mess, params object[] args) {

			Console.ForegroundColor = ConsoleColor.Red;
			Write(mess, args);
			Console.ResetColor();

			return this;
		}

		public Output Write(string mess, params object[] args) {

			Console.Out.Write(mess, args);

			return this;
		}

		public Output Wait() {

			Console.ReadKey(true);

			return this;
		}

		public Output Wait(string message) {

			Write(message);
			Wait();

			return this;
		}

		public string Read() {
			return Console.ReadLine();
		}

		public string Read(string message) {

			Write(message);
			return Read();
		}
	}
}