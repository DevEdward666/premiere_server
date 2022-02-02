using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExecutiveAPI.Config
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
