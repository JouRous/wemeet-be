using System;
using System.IO;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Interfaces;
using API.Models;
using API.Types;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public AuthController(
          UserManager<AppUser> userManager,
          SignInManager<AppUser> signInManager,
          RoleManager<AppRole> roleManager,
          ITokenService tokenService,
          IUnitOfWork unitOfWork,
          IMapper mapper,
          IWebHostEnvironment hostEnvironment,
          IEmailService emailService
        )
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _hostEnvironment = hostEnvironment;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<AuthModel>>> Login(LoginModel loginModel)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == loginModel.Email.ToLower());

            if (user == null)
            {
                return Unauthorized("Invalid User");
            }

            if (!user.isActive)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    status = 403,
                    success = true,
                    message = "User had been deactivated"
                });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            var authDTO = new AuthModel
            {
                User = _mapper.Map<UserDTO>(user),
                token = await _tokenService.CreateToken(user),
                Role = await _userManager.GetRolesAsync(user)
            };

            return new Response<AuthModel>
            {
                Data = authDTO,
                success = true,
                status = 200
            };
        }

        [HttpPost("forget-request")]
        public async Task<ActionResult> ForgetRequest(ForgetRequest forgetRequest)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(user => user.Email == forgetRequest.email);

            if (user == null)
            {
                return Ok(new
                {
                    status = 404,
                    success = true,
                    message = "User not found!"
                });
            }

            var resetPasswordToken = _tokenService.CreateResetPasswordToken(user.Email);

            var resetPasswordLink = $"{forgetRequest.domain}?token={resetPasswordToken}";

            await _emailService.sendMailAsync(forgetRequest.email, "Forget password", $@"<a href=""http://{resetPasswordLink}"">http://{resetPasswordLink}</a>");
            return Ok(new
            {
                status = 200,
                success = true
            });
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var handler = new JwtSecurityTokenHandler();
            var email = handler.ReadJwtToken(model.token)
                   .Claims.Where(c => c.Type.Equals("email")).Select(c => c.Value).SingleOrDefault();

            var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Email == email.ToLower());

            if (user == null)
            {
                return NotFound(new
                {
                    status = 404,
                    success = true,
                    message = "User not found"
                });
            }

            var randomPassword = Utils.Utils.RandomString(9);

            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, model.password);

            return Ok(new
            {
                status = 200,
                success = true,
                message = "Reset password success"
            });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult> GetProfile()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var handler = new JwtSecurityTokenHandler();

            var email = handler.ReadJwtToken(token)
                   .Claims.Where(c => c.Type.Equals("email")).Select(c => c.Value).SingleOrDefault();
            var roles = handler.ReadJwtToken(token)
                   .Claims.Where(c => c.Type.Equals("role")).Select(c => c.Value).ToList();

            var User = await _unitOfWork.UserRepository.GetUserByEmailAsync(email);
            var profile = new
            {
                User = User,
                Roles = roles
            };

            return Ok(new Response<object>
            {
                success = true,
                status = 200,
                Data = profile
            });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] LoginModel model)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var handler = new JwtSecurityTokenHandler();

            var email = handler.ReadJwtToken(token)
                   .Claims.Where(c => c.Type.Equals("email")).Select(c => c.Value).SingleOrDefault();
            var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Email == email);

            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, model.Password);

            user.isFirstLogin = false;
            await _userManager.UpdateAsync(user);

            return Accepted(new
            {
                status = 202,
                success = true,
                message = "Password had changed"
            });
        }

        private async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = DateTime.Now.ToString("yyyymmssfff") + "_" + Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

            var uploadPath = Path.Combine(_hostEnvironment.ContentRootPath, "Uploads", "Avatars", imageName);

            using (var fileStream = new FileStream(uploadPath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return imageName;
        }
    }
}
