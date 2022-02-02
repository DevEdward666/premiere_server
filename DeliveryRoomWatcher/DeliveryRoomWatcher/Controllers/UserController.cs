using DeliveryRoomWatcher.Filters;
using DeliveryRoomWatcher.Models;
using DeliveryRoomWatcher.Models.Common;
using DeliveryRoomWatcher.Models.Doctors;
using DeliveryRoomWatcher.Models.User;
using DeliveryRoomWatcher.Parameters;
using DeliveryRoomWatcher.Providers;
using DeliveryRoomWatcher.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DeliveryRoomWatcher.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJwtAuthManager _jwtAuthManager;
        UserRepository _user = new UserRepository();

        public UserController(IJwtAuthManager jwtAuthManager)
        {
            _jwtAuthManager = jwtAuthManager;
        }


        [HttpPost]
        [Route("api/user/login")]
        public ActionResult login(PAuthUser cred)
        {

            List<UserModel> user = _user.authenticateUser(cred);

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
        [Route("api/user/logindoctor")]
        public ActionResult logindoctor(PAuthUser cred)
        {

            List<UserModel> user = _user.authenticateDoctor(cred);

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
        [Route("api/user/loginAdmin")]
        public ActionResult loginAdmin(PAuthUser cred)
        {

            List<UserModel> user = _user.authenticateAdmin(cred);

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
        [Route("api/user/refresh-token")]
        public async Task<ActionResult> refreshToken(PRefreshToken request)
        {
            try
            {
                var userName = User.Identity.Name;

                if (string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    return Unauthorized();
                }


                var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
                var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);
                return Ok(new
                {
                    access_token = jwtResult.AccessToken,
                    refresh_token = jwtResult.RefreshToken.TokenString
                });
            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(e.Message);
            }
        }


        [HttpGet]
        [Route("api/user/logout")]
        public ActionResult logout()
        {
            var userName = User.Identity.Name;
            _jwtAuthManager.RemoveRefreshTokenByUserName(userName);

            return Ok();
        }



        [HttpPost]
        [Route("api/user/getDoctorInfo")]
        public ActionResult getDoctorInfo(Doctors_info.get_info info)
        {
            
            return Ok(_user.getDoctorInfo(info));
        }   
        [HttpPost]
        [Route("api/user/getUserinfoAdmin")]
        public ActionResult getUserinfoAdmin(PGetUsername username)
        {
            
            return Ok(_user.getUserInfoAdmin(username));
        }    
        [HttpPost]
        [Route("api/user/getUserInfo")]
        public ActionResult getUserInfo(PGetUsername username)
        {
            
            return Ok(_user.getUserInfo(username));
        }   
        [HttpPost]
        [Route("api/user/getusersqr")]
        public ActionResult getusersqr(PSingleString payload)
        {
            
            return Ok(_user.getusersqr(payload.value));
        }
        [HttpPost]
        [Route("api/user/getuserpin")]
        public ActionResult getuserpin(PGetUsername username)
        {
            return Ok(_user.getuserpin(username));
        }
         
        [HttpPost]
        [Route("api/user/getDoctorsExist")]
        public ActionResult getDoctorsExist(GetDoctorIdno idno)
        {
            return Ok(_user.getDoctorsExist(idno));
        }     
        [HttpPost]
        [Route("api/user/getDoctorsUsernameExist")]
        public ActionResult getDoctorsUsernameExist(PGetUsername username)
        {
            return Ok(_user.getDoctorsUsernameExist(username));
        }
           
        [HttpPost]
        [Route("api/user/getUsernameExist")]
        public ActionResult getUsernameExist(PGetUsername username)
        {
            return Ok(_user.getUsernameExist(username));
        }


        [HttpPost]
        [Route("api/user/InserNewDoctor")]
        public ActionResult InserNewDoctor(DoctorModel adduser)
        {
          
            return Ok(_user.InserNewDoctor(adduser));
        }   
        [HttpPost]
        [Route("api/user/getDoctorUserOTP")]
        public ActionResult getDoctorUserOTP(PGetUsername adduser)
        {
          
            return Ok(_user.getDoctorUserOTP(adduser));
        }    
        [HttpPost]
        [Route("api/user/addnewuser")]
        public ActionResult InserNewUser(PAddNewUsers adduser)
        {
          
            return Ok(_user.InserNewUser(adduser));
        }
         [HttpPost]
        [Route("api/user/InserNewOTP")]
        public ActionResult InserNewOTP(PAddNewOTP addotp)
        {
          
            return Ok(_user.InserNewOTP(addotp));
        }      
        [HttpPost]
        [Route("api/user/deletelastOTP")]
        public ActionResult deletelastOTP(mdlLocked username)
        {
          
            return Ok(_user.deletelastOTP(username));
        }
             
        [HttpPost]
        [Route("api/user/update_password")]
        public ActionResult update_password(mdlupdatepassword updatepassword)
        {
          
            return Ok(_user.update_password(updatepassword));
        }
             
        [HttpPost]
        [Route("api/user/updatelockeduser")]
        public ActionResult updatelockeduser(mdlLocked username)
        {
          
            return Ok(_user.updatelockeduser(username));
        }

        [HttpPost]
        [Route("api/user/getcurrentotp")]
        public ActionResult getcurrentotp(UserModel.getCurrentOtp currentOtp)
        {
            return Ok(_user.getcurrentotp(currentOtp));
        }      
        [HttpPost]
        [Route("api/user/getUserOTP")]
        public ActionResult getUserOTP(PGetUsername username)
        {
            return Ok(_user.getUserOTP(username));
        }      
        [HttpPost]
        [Route("api/user/OTP_for_UpdatePassword")]
        public ActionResult OTP_for_UpdatePassword(PAddNewOTP user_info)
        {
            return Ok(_user.OTP_for_UpdatePassword(user_info));
        }

        [HttpPost]
        [Route("api/user/getUserMobile")]
        public ActionResult getUserMobile(PGetUsername username)
        {
            return Ok(_user.getUserMobile(username));
        }     
        [HttpPost]
        [Route("api/user/UpdateUserImage")]
        public ActionResult UpdateUserImage([FromForm] UserModel.setUserImage userImage)
        {
            return Ok(_user.UpdateUserImage(userImage));
        }
        [HttpPost]
        [Route("api/user/getimage")]
 
        public ResponseModel getimage(string filename)
        {
         
              
                try
                {
                string path = Path.Combine(Directory.GetCurrentDirectory(), filename);
                using (Image images = Image.FromFile(path))
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            images.Save(m, images.RawFormat);
                            byte[] imageBytes = m.ToArray();



                            return new ResponseModel
                            {
                                success = true,
                                message = "data:image/png;base64," + Convert.ToBase64String(imageBytes)
                            };
                        }
                    }
                }
                catch (Exception e)
                {
                  return new ResponseModel
                            {
                                success = false,
                                message = "Error: "+e.ToString()
                            };

                }
            }
         
        

        [HttpPost]
        [Route("api/user/getimageDocs")]
  
        public ResponseModel getimageDocs(string filename)
        {
                try
                {
                string path = Path.Combine(Directory.GetCurrentDirectory(), filename);
                using (Image images = Image.FromFile(path))
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            images.Save(m, images.RawFormat);
                            byte[] imageBytes = m.ToArray();
                        



                        return new ResponseModel
                        {
                            success = true,
                            message = "data:image/png;base64," + Convert.ToBase64String(imageBytes)
                        };
                    }
                    }
                }
                catch (Exception e)
                {
                return new ResponseModel
                {
                    success = false,
                    message = "Error: " + e.ToString()
                };
            }
              

        }
        [Route("api/user/getResultsPDF")]
        [HttpPost]
        public  async Task<byte[]> DownloadFile(string url)
        {
            using (var client = new HttpClient())
            {

                using (var result = await client.GetAsync(url))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        return await result.Content.ReadAsByteArrayAsync();
                    }

                }
            }
            return null;
        }
        [HttpPost] 
        [Route("api/user/UploadFile")]
        public IActionResult UploadFile([FromForm] setNew_Docs file)
        {
            return Ok(_user.updatedocs(file));
          
        }


        [HttpGet]
        [Route("api/start")]
        public ActionResult start()
        {


            return Ok("The server has started successfully");
        }

        [HttpPost]
        [Route("api/user/UploadFileProfile")]
        public IActionResult UploadFileProfile([FromForm] setNew_profile file)
        {
            return Ok(_user.updateimg(file));
        
        }

    }
}
