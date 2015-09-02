using PathTooLong.Cmd.App.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PathTooLong.Cmd.App {

	public class Options : IOptions {

		bool? _silent;
		bool? _overwrite;
		string _path;
		string _to;
		readonly Dictionary<string, IEnumerable<string>> _args;

		public Options(IEnumerable<string> args) {

			if (args == null) {
				throw new ArgumentException(nameof(args));
			}

			_args = SplitArgs(args, ParseKey);
		}

		public string Path {
			get {

				if (_path != null) {
					return _path;
				}

				// Cache it
				_path = (from a in _args where Regex.IsMatch(a.Key, "^(-delete|-copy)") select a.Value.First()).FirstOrDefault();

				if (_path == null) {
					throw new ArgumentNullException(nameof(Path));
				}

				return _path;
			}
		}

		public string Destination {
			get {
				
				if (_to != null) {
					return _to;
				}

				// Cache it
				_to = (from a in _args where a.Key == "-dest" select a.Value.First()).FirstOrDefault();

				if (_to == null) {
					throw new ArgumentNullException(nameof(Destination));
				}

				return _to;
			}
		}

		public bool Silent {
			get {
				if (_silent == null) {
					_silent = _args.Any(arg => arg.Key == "-silent");
				}

				return _silent.Value;
			}
		}

		public bool Overwrite {
			get {

				if (_overwrite == null) {
					_overwrite = _args.Any(arg => arg.Key == "-overwrite");
				}

				return _overwrite.Value;
			}
		}

		public IProcess Process {
			get {

				if (_args.Any(arg => arg.Key == "-delete")) {

					return new Delete(this);
				}

				if (_args.Any(arg => arg.Key == "-copy")) {

					return new Copy(this);
				}

				return new Help(this);
			}
		}



		string ParseKey(string key) {

			key = key.ToLowerInvariant();

			if (Regex.IsMatch(key, "^(^$|^-path)")) {
				return "-delete";
			}

			if (Regex.IsMatch(key, "^(-to|-dest)")) {
				return "-dest";
			}

			return key;
		}

		Dictionary<string, IEnumerable<string>> SplitArgs(IEnumerable<string> args, Func<string, string> keyNameParser) {

			var splitArgs = new Dictionary<string, IEnumerable<string>>();

			var i = 0;
			List<string> values = null;

			foreach (var item in args) {
				
				if(i++ == 0 && !item.StartsWith("-", StringComparison.OrdinalIgnoreCase)) {

					values = new List<string>() {
						item
					};

					splitArgs.Add(keyNameParser(""), values);

					continue;
				}

				if (item.StartsWith("-", StringComparison.OrdinalIgnoreCase)) {

					values = new List<string>();

					splitArgs.Add(keyNameParser(item), values);

					continue;
				}

				values.Add(item);
			}

			return splitArgs;
		}
	}
}