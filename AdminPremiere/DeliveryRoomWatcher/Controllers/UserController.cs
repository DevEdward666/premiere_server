using DeliveryRoomWatcher.Filters;
using DeliveryRoomWatcher.Models.Common;
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
        public IActionResult login(PAuthUser cred)
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
        [Route("api/user/refresh-token")]
        public async Task<IActionResult> refreshToken(PRefreshToken request)
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
        public IActionResult logout()
        {
            var userName = User.Identity.Name;
            _jwtAuthManager.RemoveRefreshTokenByUserName(userName);

            return Ok();
        }



        [HttpPost]
        [Route("api/user/getUserInfo")]
        public IActionResult getUserInfo(PGetUsername username)
        {
            return Ok(_user.getUserInfo(username));
        }
         
        [HttpPost]
        [Route("api/user/getUsernameExist")]
        public IActionResult getUsernameExist(PGetUsername username)
        {
            return Ok(_user.getUsernameExist(username));
        }


        [HttpPost]
        [Route("api/user/addnewuser")]
        public IActionResult InserNewUser(PAddNewUsers adduser)
        {
          
            return Ok(_user.InserNewUser(adduser));
        }
         [HttpPost]
        [Route("api/user/InserNewOTP")]
        public IActionResult InserNewOTP(PAddNewOTP addotp)
        {
          
            return Ok(_user.InserNewOTP(addotp));
        }

        [HttpPost]
        [Route("api/user/getUserOTP")]
        public IActionResult getUserOTP(PGetUsername username)
        {
            return Ok(_user.getUserOTP(username));
        }

        [HttpPost]
        [Route("api/user/getUserMobile")]
        public IActionResult getUserMobile(PGetUsername username)
        {
            return Ok(_user.getUserMobile(username));
        }

        //[Route("api/user/getimage")]
        //[HttpPost]
        //public IActionResult Post(string username)
        //{
        //    if (username=="undefined" || username == null)
        //    {
        //        username = "test";
        //        string path = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\Images\test\test.jpg");
        //        var image = System.IO.File.OpenRead(path);
        //        return File(image, "image/jpeg");
        //    }
        //    else
        //    {
        //        string path = Path.Combine(Directory.GetCurrentDirectory(), username);
        //        var image = System.IO.File.OpenRead(path);
        //        return File(image, "image/jpeg");
        //    }
         
        //}
        //[Route("api/user/getimageDocs")]
        //[HttpPost]
        //public IActionResult getimageDocs(string username)
        //{
        //    if (username == "undefined" || username==null)
        //    {
        //        username = "test";
        //        string path = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\Images\test\test_VerificationDocs.jpg");
        //        var image = System.IO.File.OpenRead(path );
        //        return File(image, "image/jpeg");
        //    }
        //    else
        //    {
        //        string path = Path.Combine(Directory.GetCurrentDirectory(), username);
        //        var image = System.IO.File.OpenRead(path );
        //        return File(image, "image/jpeg");
        //    }

        //}
    
        //[HttpPost("api/user/UploadFile")]
        //public async Task<string> UploadFile([FromForm] IFormFile file)
        //{
        //    string fName = file.FileName;
        //    string path = Path.Combine(Directory.GetCurrentDirectory(), "Resources\\Images\\" + file.FileName);
        //    using (var stream = new FileStream(path, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }
        //    return file.FileName;
        //}
        //[HttpPost("api/user/UploadFile"), DisableRequestSizeLimit]
        //public IActionResult Upload()
        //{
        //    try
        //    {
        //        var file = Request.Form.Files[0];
        //        var folderName = Path.Combine("Resources", "Images");
        //        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        //        if (file.Length > 0)
        //        {
        //            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //            var fullPath = Path.Combine(pathToSave, fileName);
        //            var dbPath = Path.Combine(folderName, fileName);

        //            using (var stream = new FileStream(fullPath, FileMode.Create))
        //            {
        //                file.CopyTo(stream);
        //            }

        //            return Ok(new { dbPath });
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex}");
        //    }
        //}

        [HttpPost] 
        [Route("api/user/UploadFile")]
        public ActionResult Post([FromForm] FileModel file)
        {
            try
            {
             
                string path = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\\Images\\{file.FolderName}");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    string filepath = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\\Images\\{file.FolderName}\\", file.FileName);
                    using (Stream stream = new FileStream(filepath, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                    }
                }
                else {
                    string filepath = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\\Images\\{file.FolderName}\\", file.FileName);
                    using (Stream stream = new FileStream(filepath, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                    }
                   
                }

                return StatusCode(StatusCodes.Status201Created);



            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [Route("api/user/UploadFileProfile")]
        public ActionResult UploadFileProfile([FromForm] FileModelProfile file)
        {
            try
            {

                string path = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\\Images\\{file.FolderName}");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    string filepath = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\\Images\\{file.FolderName}\\", file.FileName);
                    using (Stream stream = new FileStream(filepath, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                    }
                }
                else
                {
                    string filepath = Path.Combine(Directory.GetCurrentDirectory(), $@"Resources\\Images\\{file.FolderName}\\", file.FileName);
                    using (Stream stream = new FileStream(filepath, FileMode.Create))
                    {
                        file.FormFile.CopyTo(stream);
                    }

                }

                return StatusCode(StatusCodes.Status201Created);

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
