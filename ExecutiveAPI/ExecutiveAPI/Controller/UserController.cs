using ExecutiveAPI.Model;
using ExecutiveAPI.Model.Common;
using ExecutiveAPI.Model.PatientsModel;
using ExecutiveAPI.Providers;
using ExecutiveAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExecutiveAPI.Controller
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJwtAuthManager _jwtAuthManager;
        UserRepo _user = new UserRepo();
        public UserController(IJwtAuthManager jwtAuthManager)
        {
            _jwtAuthManager = jwtAuthManager;
        }
        [HttpPost]
        [Route("api/user/login")]
        public ActionResult login(UserModel.AuthUser cred)
        {

            List<UserModel.Users> user = _user.authenticateUser(cred);

            int userLength = user.Count;

            if (userLength > 0)
            {

                var claims = new Claim[user.Count + 1];



                for (int i = 0; i < user.Count; i++)
                {
                    claims[i] = new Claim(ClaimTypes.Role, user[i].username);
                }

                claims[user.Count] = new Claim(ClaimTypes.Name, user[userLength - 1].username);

                var jwtResult = _jwtAuthManager.GenerateTokens(user[userLength - 1].username, claims, DateTime.Now);
                //var jwtResult = _jwtAuthManager.GenerateTokens(user[userLength - 1].username, claims, DateTime.Now);

                return Ok(new ResponseModel
                {
                    success = true,
                    data = new
                    {
                        access_token = jwtResult.AccessToken,
                        refresh_token = jwtResult.RefreshToken.TokenString
                    }
                });

            }
            else
            {
                return Ok(new { success = false, message = "The username and/or password you entered is not correct. Please try again," });
            }
        }
        [HttpPost]
        [Route("api/user/getOTPforLab")]
        public ActionResult getOTPforLab(OTPModel otpModel)
        {

            return Ok(_user.OTP_For_Laboratory(otpModel));
        }
          
        [HttpPost]
        [Route("api/user/get_patient_lab_data")]
        public ActionResult get_patient_lab_data(OTPModel otpModel)
        {

            return Ok(_user.get_patient_lab_data(otpModel));
        }


    }
}
