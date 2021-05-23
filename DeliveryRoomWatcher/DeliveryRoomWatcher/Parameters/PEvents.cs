using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Parameters
{
    public class PEvents
    {
        public class PEventsInfo
        {
            [Required]
            [StringLength(30)]
            [Display(Name = "EventID")]
            public string evid { get; set; }
            [Required]
            [StringLength(30)]
            [Display(Name = "EventTitle")]
            public string evtitle { get; set; }
            [Required]
            [StringLength(30)]
            [Display(Name = "EventDesc")]
            public string evdesc { get; set; }
            [Required]
            [StringLength(30)]
            [Display(Name = "EventVenue")]
            public string evvenue { get; set; }
            [Required]
            [StringLength(30)]
            [Display(Name = "EventStartDate")]
            public string evstartdate { get; set; }
            [Required]
            [StringLength(30)]
            [Display(Name = "EventStartTime")]
            public string evstarttime { get; set; }
            [Required]
            [StringLength(30)]
            [Display(Name = "EventEndDate")]
            public string evenddate { get; set; }
            [Required]
            [StringLength(30)]
            [Display(Name = "EventEndTime")]
            public string evendtime { get; set; }
            [Required]
            [StringLength(50)]
            [Display(Name = "EventsTimestamp")]
            public string eventts { get; set; }
            [Required]
            [StringLength(50)]
            [Display(Name = "EventsStatus")]
            public string evstatus { get; set; }
        }
        public class PGetEvent
        {
            [Required]
            [Display(Name = "EventID")]
            public long evid { get; set; }
           
        }
        public class PEvent
        {
            [Required]
            [Display(Name = "EventID")]
            public long evid { get; set; }
            [Required]
            [StringLength(30)]
            [Display(Name = "EventTitle")]
            public string evtitle { get; set; }
            [Required]
            [StringLength(30)]
            [Display(Name = "EventDesc")]
            public string evdesc { get; set; }

            [Display(Name = "EventImage")]
            public string evimage { get; set; }

            [Display(Name = "base64")]
            public string base64 { get; set; }
     
            [Required]
            [StringLength(30)]
            [Display(Name = "EventStartDate")]
            public string evstartdate { get; set; }
            [Required]
            [StringLength(30)]
            [Display(Name = "EventStartTime")]
            public string evstarttime { get; set; }
            [Required]
            [StringLength(30)]
            [Display(Name = "EventEndDate")]
            public string evenddate { get; set; }
            [Required]
            [StringLength(30)]
            [Display(Name = "EventEndTime")]
            public string evendtime { get; set; }
            [Required]
            [StringLength(30)]
            [Display(Name = "EventColor")]
            public string evcolor { get; set; }
        }
        public class PEventByMonth
        {
            public string month { get; set; }
            public string year { get; set; }

        }
        public class searchableDate { 
        
        public string searchdate { get; set; }
        }

        }
}
