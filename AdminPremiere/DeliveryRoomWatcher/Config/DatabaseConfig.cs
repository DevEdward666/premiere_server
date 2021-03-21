namespace DeliveryRoomWatcher.Config
{
    public class DatabaseConfig
    {
        public static string conStr;

        public static string GetConnection()
        {
            return conStr;
        }
    }
}
