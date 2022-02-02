﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Entities
{
    public class DepartmentEntity
    {
        public int? dept_pk { get; set; }
        public string dept_code { get; set; }
        public string dept_name { get; set; }
        public string notes { get; set; }
        public string is_active { get; set; }
        public DateTime? encoded_at { get; set; }
        public string encoder_pk { get; set; }

        public UserEntity user { get; set; }
    }
}
