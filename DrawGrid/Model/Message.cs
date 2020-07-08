namespace DrawGrid.Model
{
    public class Message
    {
        public Message(string key, string msg = "", string extra = "")
        {
            Key = key;
            Msg = msg;
            Extra = extra;
        }

        public string Key { get; }

        public string Msg { get; }
        public string Extra { get; }

        public static class Main
        {
            public const string GeneratePoint = nameof(GeneratePoint);
            public const string DrawCircle = nameof(DrawCircle);
        }
    }
}