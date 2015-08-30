namespace PathTooLong.Cmd.App {

	public interface IOptions {

		string Path { get; }

		/// <summary>
		/// States whether the program should just run without human intervention or warning
		/// </summary>
		bool Silent { get; }

		/// <summary>
		/// Returns the process desired based on the options
		/// </summary>
		IProcess Process { get; }
	}
}