using PathTooLong.Exceptions;
using System;

namespace PathTooLong.Cmd.App.Processes {

	public class Delete : IProcess {

		readonly Options _options;
		readonly IOutput _output;
		readonly IFileSystemScanner _scanner;
		readonly IFileSystemManager _manager;
		readonly IPathUtility _paths;

		public Delete(Options options) {

			_options = options;
			_output = new Output();
			_paths = new PathUtility();
			_scanner = new FileSystemScanner();
			_manager = new FileSystemManager();
		}

		public Delete(Options options, IOutput output, IPathUtility paths, IFileSystemScanner collector, IFileSystemManager manager) {

			_output = output;
			_options = options;
			_scanner = collector;
			_manager = manager;
			_paths = paths;
		}

		public void Run() {

			var path = _options.Path;

			try {

				if (string.IsNullOrEmpty(path)) {

					_output.Write("You did not supply a file/directory path\n");
					return;
				}

				if (!_scanner.Exists(path)) {

					// Only attempt to check in the current directory if not silent (to avoid accidently deletes)
					if (!_options.Silent && !_paths.IsRooted(path)) {

						path = _paths.Combine(_paths.CurrentDirectory, path);

						if (!_scanner.Exists(path)) {

							_output.Write("Sorry that file/directory does not exist\n");
							return;
						}
					}

					_output.Write("Sorry that file/directory does not exist\n");
				}

				// Do a catch in case the item has been deleted since
				try {

					// Do a search on the file/folder
					var search = _scanner.GetFileSystemData(path);

					if (search.IsDirectory) {
						ProcessDirectory(search);
					}
					else {
						ProcessFile(search);
					}
				}
				catch (PathNotFoundException) {

					// Swallow it, as it means something else has done the delete
					return;
				}
				catch (InvalidFileSearchException ipsex) {

					_output.Write(ipsex.Message);
					return;
				}
			}
			finally {

				if (!_options.Silent) {
					_output.Wait("\nPress any key to continue\n");
				}
			}
		}

		void ProcessFile(FileSystemData data) {

			// Double check they actually want to delete that file/directory
			if (!_options.Silent) {

				_output.WriteWarning("WARNING ")
					  .WriteHighlight("This process can not be undone\n")
					  .Write("Are you sure you want to delete the following file? \n\n")
					  .WriteExtraHighlight(data.Path)
					  .WriteHighlight("\n\nPlease select y/n: ");

				if (!_output.Read().StartsWith("y", StringComparison.OrdinalIgnoreCase)) {
					return;
				}
			}

			_output.Write("\nProcessing, please wait\n\n");

			try {

				_manager.Delete((FileData)data);
				_output.Write("Successfully deleted\n");
			}
			catch (UnauthorizedAccessException) {

				_output.Write("You were not authorised to delete that file. Try running the program again as an Administrator.\n");
			}
			catch (Exception ex) {

				_output.Write(ex.Message);
			}
		}

		void ProcessDirectory(FileSystemData data) {

			DirectoryDataSnapshot dir;

			try {

				_output.Write("Please wait scanning directory...\n");

				dir = (DirectoryDataSnapshot)_scanner.GetFileSystemDataDeep(data.Path);
			}
			catch (PathNotFoundException) {

				// Swallow it, as it means something else has done the delete
				return;
			}
			catch (InvalidFileSearchException ipsex) {

				_output.Write(ipsex.Message);
				return;
			}

			// Double check they actually want to delete that file/directory
			if (!_options.Silent) {

				_output.WriteWarning("WARNING ")
					   .WriteHighlight("This process can not be undone\n")
					   .Write("Are you sure you want to delete the following directory?\n")
					   .WriteExtraHighlight("\n{0}\n", data.Path)
					   .WriteHighlight("Directories: {0}\n", dir.DirectoryCount)
					   .WriteHighlight("Files: {0}\n", dir.FileCount)
					   .WriteHighlight("Size: {0:0.00} Mb\n", dir.Size == 0 ? 0 : (decimal)dir.Size / 1048576)
					   .WriteHighlight("\nPlease select y/n: ");

				if (!_output.Read().StartsWith("y", StringComparison.OrdinalIgnoreCase)) {
					return;
				}
			}

			_output.Write("\nProcessing...\n\n");

			try {

				_manager.Delete(dir);
				_output.Write("Successfully deleted\n");
			}
			catch (UnauthorizedAccessException) {

				_output.Write("You were not authorised to delete one or more of the files/directories. Try running the program again as an Administrator.\n");
			}
			catch (Exception ex) {

				_output.Write(ex.Message);
			}
		}
	}
}