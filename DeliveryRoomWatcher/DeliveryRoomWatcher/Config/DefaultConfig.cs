using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Config
{
    public static class DefaultConfig
    {
        public static string app_name;
        public static string _providerEmailAddress = "tuoitsolutions@gmail.com";
        public static string _providerEmailPass = "thisismy name@2015";
        public static string _clientBaseUrl = "http://89.107.58.254:60315/payment/";

        public static string ftp_ip;
        public static string ftp_port;
        public static string ftp_user;
        public static string ftp_pass;

        public static string paymongo_secret_key;
        public static string paymongo_public_key;
        public static string paymongo_payment_url;
        public static string paymongo_source_url;
        public static string paymongo_pay_intent_url;


        public static string passbase_public_key;
        public static string passbase_secret_key;
        public static string passbase_verification_url;

    }
}
