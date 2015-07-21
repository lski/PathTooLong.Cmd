using PathTooLong.Cmd.App.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PathTooLong.Cmd.App {

	public class Options : IOptions {

		private readonly IEnumerable<string> _args;
		private bool? _silent;

		public Options(IEnumerable<string> args) {

			_args = args;
		}

		public string Path() {

			return _args
				.Where(x => Regex.IsMatch(x, "^-path:|[^-]", RegexOptions.IgnoreCase))
				.Select(x => Regex.Replace(x, "^-path:", ""))
				.FirstOrDefault();
		}

		public bool Silent() {

			if (_silent == null) {
				_silent = _args.Any(x => Regex.IsMatch(x, "-silent", RegexOptions.IgnoreCase));
			}

			return _silent.Value;
		}

		public IProcess Process {
			get {
				// Currently only supports delete
				return new Delete(this);
			}
		}
	}
}