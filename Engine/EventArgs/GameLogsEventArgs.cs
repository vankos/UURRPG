namespace Engine.EventArgs
{
    public class GameLogsEventArgs: System.EventArgs
    {
        public string Message { get; }

        public GameLogsEventArgs(string message)=>Message = message;
    }
}
