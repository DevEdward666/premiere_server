using DeliveryRoomWatcher.Models.Doctors;
using DeliveryRoomWatcher.Models.User;
using DeliveryRoomWatcher.Repositories;
using DeliveryRoomWatcher.Repositories.Doctors_App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Controllers.Doctors
{
    [Authorize]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        DashboardRepo _dashbaord_doctor = new DashboardRepo();


        [HttpPost]
        [Route("api/dashboard/New_patients")]
        public ActionResult New_patients(DashboardModel.NewPatients newPatients)
        {
            return Ok(_dashbaord_doctor.New_patients(newPatients));
        }
        [HttpPost]
        [Route("api/dashboard/Active_patients")]
        public ActionResult Active_patients(DashboardModel.ActivePatients activePatients)
        {
            return Ok(_dashbaord_doctor.Active_patients(activePatients));
        }   
        [HttpPost]
        [Route("api/dashboard/search_patient")]
        public ActionResult search_patient(DashboardModel.filter_patients search)
        {
            return Ok(_dashbaord_doctor.search_patient(search));
        }    
        [HttpPost]
        [Route("api/dashboard/patient_medication")]
        public ActionResult patient_medication(DashboardModel.patient_info patno)
        {
            return Ok(_dashbaord_doctor.patient_medication(patno));
        }    
        [HttpPost]
        [Route("api/dashboard/patient_laboratories")]
        public ActionResult patient_laboratories(DashboardModel.patient_info patno)
        {
            return Ok(_dashbaord_doctor.patient_laboratories(patno));
        }    
        [HttpPost]
        [Route("api/dashboard/patient_general_info")]
        public ActionResult patient_general_info(DashboardModel.patient_info patno)
        {
            return Ok(_dashbaord_doctor.patient_general_info(patno));
        } 
        [HttpPost]
        [Route("api/dashboard/patient_admission_CareTeam")]
        public ActionResult patient_admission_CareTeam(DashboardModel.patient_info patno)
        {
            return Ok(_dashbaord_doctor.patient_admission_CareTeam(patno));
        }   
        [HttpPost]
        [Route("api/dashboard/patient_CourseOntheWard")]
        public ActionResult patient_CourseOntheWard(DashboardModel.patient_info patno)
        {
            return Ok(_dashbaord_doctor.patient_CourseOntheWard(patno));
        }   
        [HttpPost]
        [Route("api/dashboard/patient_admission_DietOrders")]
        public ActionResult patient_admission_DietOrders(DashboardModel.patient_info patno)
        {
            return Ok(_dashbaord_doctor.patient_admission_DietOrders(patno));
        }     
        [HttpPost]
        [Route("api/dashboard/patient_admission_history")]
        public ActionResult patient_admission_history(DashboardModel.patient_info hospitalno)
        {
            return Ok(_dashbaord_doctor.patient_admission_history(hospitalno));
        }
    }
}
