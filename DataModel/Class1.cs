namespace DataModel
{
    public class Item
    {
        public DateTime TimeStamp { get; set; }
        public ContentType ContentType { get; set; }
        public string? Name { get; set; }

        public byte[]? Data { get; set; }

    }

    public enum ContentType
    {
        Default = 0
    }
}