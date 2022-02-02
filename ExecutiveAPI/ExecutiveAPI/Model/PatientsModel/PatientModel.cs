using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExecutiveAPI.Model.PatientsModel
{
    public class PatientModel
    {
        public class GetPatientOptions
        {
            public string year { get; set; }
            public string month { get; set; }
        } 
    }
}
