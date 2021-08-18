using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models
{
    public class Queue
    {
        public queues queue { get; set; }
        public counter counters { get; set; }
        public generatecounternumber generatecounternumbers { get; set; }

        public waiting waitings { get; set; }
        public counterlist counterlists { get; set; }
        public generatenumber generatenumbers { get; set; }
        public UpdateQueue UpdateQueues { get; set; }
        public getMaxNUmber getMaxNUmbers { get; set; }
        public getqueues getqueue { get; set; }

        public class getqueues
        {
            public string countertype { get; set; }
            public string countername { get; set; }
        }
        public class addlobby
        {
            public string location { get; set; }
        }
        public class getcountertype
        {
            public string typename { get; set; }
        }
        public class addlobbydtls
        {
            public int lobbyno { get; set; }
            public string countername { get; set; }
            public string counterno { get; set; }
        }
        public class updatephonenumber
        {
            public string phonenumber { get; set; }
            public string queueno { get; set; }
            public string countername { get; set; }
        }
        public class nextclienttotext
        {
            public string phonenumber { get; set; }
            public string queueno { get; set; }
            public string counter { get; set; }

        }
        public class message
        {
            public string msg { get; set; }
        }
        public class insertmessage
        {
            public string phonenumber { get; set; }
            public string msg { get; set; }
        }
        public class generatecounterno
        {
            public string counterno { get; set; }
            public string displayedto { get; set; }
            public string type { get; set; }
        }

        public class queues
        {
            public string status { get; set; }
            public string queueno { get; set; }
            public string countertype { get; set; }
            public string countername { get; set; }
            public string phonenumber { get; set; }
            public string docdate { get; set; }
            public string maxnumber { get; set; }
            public string id { get; set; }
        }
        public class counter
        {
            public string status { get; set; }
            public string queueno { get; set; }
            public string countername { get; set; }
            public string displayedto { get; set; }
            public string type { get; set; }
            public string maincountername { get; set; }
            public string maincountertype { get; set; }
            public string generated_counter { get; set; }
            public string generated_countertype { get; set; }
            public string phonenumber { get; set; }
            public string countertype { get; set; }


        }
        public class generatecounternumber
        {
            public string counter { get; set; }
            public string queueno { get; set; }
            public string generated_counter { get; set; }
            public string maxnumber { get; set; }
            public string generated_countertype { get; set; }
            public string prem_id { get; set; }
        }     
        public class getuserqueuenumber
        {
            public string prem_id { get; set; }
            public string countername { get; set; }
        }
        public class counters_table
        {
            public string counter_name { get; set; }
            public string displayedto { get; set; }
            public string type { get; set; }

        }
        public class addnewcounternumber
        {
            public int counter_name { get; set; }
            public string displayedto { get; set; }
            public string type { get; set; }

        }
        public class waiting
        {
            public string queueno { get; set; }
            public string countername { get; set; }
            public string countertype { get; set; }
            public string docdate { get; set; }
            public string status { get; set; }
        }
        public class counterlist
        {
            public string queueno { get; set; }
            public string countername { get; set; }
            public string counter { get; set; }
            public string lobbyno { get; set; }
        }
        public class getlobbynos
        {

            public int lobbyno { get; set; }
        }
        public class generatenumber
        {
            public string queueno { get; set; }
            public string maxnumber { get; set; }
            public string counter { get; set; }
        }
        public class getcounterno
        {
            public string displayedto { get; set; }
        }
        public class UpdateQueue
        {
            public string queueno { get; set; }
            public string countername { get; set; }
            public string counter { get; set; }

        }
        public class getMaxNUmber
        {
            public string maxnumber { get; set; }
            public string counter { get; set; }

        }
    }
}
