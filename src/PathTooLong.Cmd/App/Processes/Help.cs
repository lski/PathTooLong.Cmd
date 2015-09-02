using System;

namespace PathTooLong.Cmd.App.Processes {

	public class Help : IProcess {

		IOptions _options;
		IOutput _output;

		public Help(IOptions options) {

			_options = options;
			_output = new Output();
		}

		public Help(IOptions options, IOutput output) {

			_options = options;
			_output = output;
		}

		public void Run() {

			_output.Write("You need to supply details for the action you want");
		}
	}
}