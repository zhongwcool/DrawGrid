namespace DrawGrid.Model
{
    public class Message
    {
        public Message(string key, object extra = null)
        {
            Key = key;
            Extra = extra;
        }

        public string Key { get; }
        public object Extra { get; }

        public const string GeneratePoint = nameof(GeneratePoint);
        public const string DrawCircle = nameof(DrawCircle);
        public const string InstantAdd = nameof(InstantAdd);
        public const string InstantRemove = nameof(InstantRemove);
    }
}