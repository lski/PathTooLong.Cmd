using System;

namespace PathTooLong.Cmd.App.Processes {

	public class Copy : IProcess {

		readonly Options _options;
		readonly IOutput _output;
		readonly IFileSystemScanner _scanner;
		readonly IFileSystemManager _manager;
		readonly IPathUtility _paths;

		public Copy(Options options) {

			_options = options;
			_output = new Output();
			_paths = new PathUtility();
			_scanner = new FileSystemScanner();
			_manager = new FileSystemManager();
		}

		public Copy(Options options, IOutput output, IPathUtility paths, IFileSystemScanner collector, IFileSystemManager manager) {

			_output = output;
			_options = options;
			_scanner = collector;
			_manager = manager;
			_paths = paths;
		}

		public void Run() {

			try {

				_manager.Copy(_options.Source, _options.Destination, _options.Overwrite);

			}
			catch (Exception ex) {

				_output.Write(ex.Message);
				return;
			}
			finally {

				if (!_options.Silent) {
					_output.Wait("\nPress any key to continue\n");
				}
			}
			
		}
	}
}