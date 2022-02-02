using System;

namespace DeliveryRoomWatcher.Entities
{
    public class UserEntity
    {
        public string user_pk { get; set; }
        public string username { get; set; }
        public string full_name { get; set; }
        public string user_type { get; set; }
        public string clinic_pk { get; set; }
        public string dept_pk { get; set; }
        public string pass { get; set; }
        public string sts_pk { get; set; }
        public string is_active { get; set; }
        public DateTime? encoded_at { get; set; }
        public string encoder_pk { get; set; }

        public class UserImageFile
        {
            public string user_id { get; set; }
            public string file_dest { get; set; }
            public string file_name { get; set; }
            public string profile_image { get; set; }
            public string profile_image_name { get; set; }
            public DateTime? encoded_at { get; set; }
        }        
        public class UserDocsFile
        {
            public string user_id { get; set; }
            public string file_dest { get; set; }
            public string file_name { get; set; }
            public string profile_image { get; set; }
            public string profile_image_name { get; set; }
            public DateTime? encoded_at { get; set; }
        }
        public class UserImageFileEntity
        {
            public string profile_image { get; set; }
            public string profile_image_name { get; set; }
            public DateTime? dateEncoded { get; set; }
        }
    }
}
