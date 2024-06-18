using AutoMapper;
using PlantifyControlPanel.Helper;
using PlantifyControlPanel.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlantifyApp.Core.Models;
using System.Security.Claims;
using PlantifyApp.Apis.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace PlantifyControlPanel.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
		private readonly IEmailSetting emailSetting;

		public AdminController(IMapper mapper,UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser>signInManager ,RoleManager<IdentityRole> roleManager,IEmailSetting emailSetting)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
			this.emailSetting = emailSetting;
		}



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                string email = model.Email;

                var user = await userManager.FindByEmailAsync(email);

                if (user is not null && user.Email == "admin@gmail.com")
                {
                    bool flag = await userManager.CheckPasswordAsync(user, model.Password);

                    if (flag)
                    {
                        var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                        if (result.Succeeded)
                        {
                            var claims = new List<Claim>
                            {
                               new Claim(ClaimTypes.Name, user.UserName),
                   
                             };

                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var principal = new ClaimsPrincipal(identity);

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                             HttpContext.Session.SetString("LastLoginTime", DateTime.Now.ToString());


                            return RedirectToAction("Index", "Home",user);
                        }
                    }
                    ModelState.AddModelError(string.Empty, "The Email Or Password is invalid");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "You are not Authorized To Access Admin Dashboard");
                }
            }
            return View(model);
        }





        public async Task<ActionResult> Signout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }



        public ActionResult ForgetPassword() {
            var model = new ForgetPasswordViewModel();
        return View(model);
        }

        [HttpGet]
        public ActionResult SendEmail( )
        {
            
            return View();
        }



        public async Task<ActionResult> SendEmail(SendEmails model)
        {
            if (ModelState.IsValid)
            {
             
                    var email = new Email()
                    {
                        To = model.UserEmail,
                        Subject = model.Subject,
                        Body = model.Body
                    };
                    await emailSetting.SendEmail(email);
   

                return RedirectToAction("Index", "Home");
            }

            return View(model);

        }



        //   public async Task<ActionResult> SendEmail(ForgetPasswordViewModel model)
        //   {
        //       if (ModelState.IsValid)
        //       {
        //           var user =await  userManager.FindByEmailAsync(model.Email);

        //           if(user is not null)
        //           {
        //var token = await userManager.GeneratePasswordResetTokenAsync(user);
        //var ResetLink = Url.Action("ResetPassword", "Admin", new { email = user.Email, Token = token }, Request.Scheme);

        //var email = new Email()
        //               {
        //                   To = model.Email,
        //                   Subject = "Reset Password",
        //                   Body = ResetLink
        //               };
        //              await emailSetting.SendEmail(email);
        //               return RedirectToAction(nameof(CheckInbox));
        //           }
        //           ModelState.AddModelError(string.Empty,"The Email Is not Exist");

        //       }

        //       return View("ForgetPassword",model);

        //   }
        [HttpGet]
        public ActionResult CheckInbox()
        {
            
            return View();
        }

		[HttpGet]
		public ActionResult ResetPassword(string email, string token)
		{
			TempData["email"] = email;
			TempData["token"] = token;

			return View();
		}

		[HttpPost]
		public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				string Email = TempData["email"] as string;
				string Token = TempData["token"] as string;
				var user = await userManager.FindByEmailAsync(Email);
				var result = await userManager.ResetPasswordAsync(user, Token, model.NewPassword);
				if (result.Succeeded)
				{
				return	RedirectToAction(nameof(Login));
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}

			}
			return View(model);

		}




	}
}
