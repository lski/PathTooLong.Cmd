namespace PathTooLong.Cmd.App {

	public interface IOutput {

		string Read();

		string Read(string message);

		Output Wait();

		Output Wait(string message);

		Output Write(string mess, params object[] args);

		Output WriteExtraHighlight(string mess, params object[] args);

		Output WriteHighlight(string mess, params object[] args);

		Output WriteWarning(string mess, params object[] args);
	}
}