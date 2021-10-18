using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Models.Clinic
{
    public class ClinicModel
    {
        public IFormFile ProfileImage { get; set; }
        public string imagename { get; set; }
        public string imagpath { get; set; }
        public string hash_key { get; set; }
        public string consult_req_pk { get; set; }
        public string appointment_id { get; set; }
        public string premid { get; set; }
        public string prefix { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public string suffix { get; set; }
        public string gender { get; set; }
        public string civil_status_key { get; set; }
        public string civil_status_desc { get; set; }
        public string nationality_code { get; set; }
        public string religion_code { get; set; }
        public string birthdate { get; set; }
        public string email { get; set; }

        public string mobile { get; set; }
        public string fulladdress { get; set; }
        public string fulladdress2 { get; set; }
        public string barangay { get; set; }
        public string line1 { get; set; }
        public string line2 { get; set; }

        public string province_code { get; set; }
        public string city_code { get; set; }
        public string citymundesc { get; set; }
        public string region_code { get; set; }
        public string regiondesc { get; set; }

        public string psgc_address { get; set; }
        public string req_total { get; set; }

        public string zipcode { get; set; }
        public string complaint { get; set; }
        public string symptomps { get; set; }
        public string remarks { get; set; }
        public string requested_at { get; set; }

        //paymongo
        public string discount_type { get; set; }
        public string discount_desc { get; set; }
        public string discount_rate { get; set; }
        public string discount_id_num { get; set; }
        public decimal? discount_amount { get; set; }
        public decimal? consult_cost { get; set; }
        public DateTime? request_at { get; set; }
        public DateTime? accept_at { get; set; }
        public DateTime? soa_sent_at { get; set; }
        public DateTime? pay_at { get; set; }
        public DateTime? consult_at { get; set; }
        public DateTime? finish_at { get; set; }
        public string sts_pk { get; set; }
        public string pay_sts_pk { get; set; }
        public string payment_method { get; set; }
        public string payment_receipt_no { get; set; }
        public DateTime? last_update_at { get; set; }
        public DateTime? last_update_by { get; set; }
        public string payment_source_id { get; set; }
        public int? pay_link_sent_count { get; set; }
        public int? soa_sent_count { get; set; }
        public string paymongo_src_id { get; set; }
        public DateTime? paymongo_src_id_enc_at { get; set; }
        public DateTime? paymongo_paid_at { get; set; }
        public string assign_dept_pk { get; set; }
        public string assign_res_pk { get; set; }
        public DateTime? assign_dept_consult_date { get; set; }
        public DateTime? assign_dept_at { get; set; }
        public ConsultRequestFileEntity consult_req_file { get; set; }
        public StatusMasterEntity status { get; set; }
        public List<IFormFile> attach_req_files { set; get; }
    }
    public class consultationDetails
    {
        public ClinicModel details { get; set; }
    }
    public class consultation_table
    {
        public string premid { get; set; }
        public string consult_req_pk { get; set; }
        public string status { get; set; }
        public int offset{ get; set; }
    }
}
