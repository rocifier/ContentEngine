namespace ContentEngine.Persistence.Azure
{
    public static class Constants
    {
        public static readonly string LINKS_TABLE = "links";
        public static readonly string CONTENT_TABLE = "content";
        public static readonly string WRITE_QUEUE_TABLE = "writequeue";
        public static readonly string ETAG_CLIENT = "etag";
        public static readonly string BUS_TOPIC_NAME = "ContentEngine/WriteQueue";
    }
}
