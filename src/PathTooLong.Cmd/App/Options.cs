using PathTooLong.Cmd.App.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PathTooLong.Cmd.App {

	public class Options : IOptions {

		readonly IEnumerable<string> _args;
		bool? _silent;

		public Options(IEnumerable<string> args) {

			_args = args;
		}

		public string Path {
			get {
				if (_args.Any()) {

					var arg = _args.ElementAt(0);

					if (!arg.StartsWith("-", StringComparison.OrdinalIgnoreCase)) {
						return arg;
					}

					if (Regex.IsMatch(arg, "^(-path|-delete)")) {
						return _args.ElementAt(1);
					}
				}

				throw new ArgumentNullException(nameof(Path));
			}
		}

		public string Source {
			get {
				var to = _args.Select((Arg, Index) => new { Arg, Index })
								.Where(x => Regex.IsMatch(x.Arg, "^-copy"))
								.FirstOrDefault();

				if (to != null) {

					var position = to.Index + 1;

					if (position <= _args.Count()) {

						return _args.ElementAt(position);
					}
				}

				throw new ArgumentNullException(nameof(Source));
			}
		}

		public string Destination {
			get {
				var to = _args.Select((Arg, Index) => new { Arg, Index })
												.Where(x => Regex.IsMatch(x.Arg, "^(-to|-dest)"))
												.FirstOrDefault();

				if (to != null) {

					var position = to.Index + 1;

					if (position <= _args.Count()) {

						return _args.ElementAt(position);
					}
				}

				throw new ArgumentNullException(nameof(Destination));
			}
		}

		public bool Silent {
			get {
				if (_silent == null) {
					_silent = _args.Any(x => Regex.IsMatch(x, "-silent", RegexOptions.IgnoreCase));
				}

				return _silent.Value;
			}
		}

		public IProcess Process {
			get {

				if (_args.Any()) {

					var arg = _args.ElementAt(0);

					if (Regex.IsMatch(arg, "^(-path|[^-]|-delete)")) {
						return new Delete(this);
					}

					if (Regex.IsMatch(arg, "^-copy")) {
						return new Copy(this);
					}
				}

				throw new ArgumentNullException(nameof(Process));
			}
		}
	}
}