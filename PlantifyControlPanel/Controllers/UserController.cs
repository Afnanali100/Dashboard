using AutoMapper;
using PlantifyControlPanel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlantifyApp.Core.Models;
using PlantifyApp.Apis.Dtos;

namespace PlantifyControlPanelL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IMapper mapper;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(UserManager<ApplicationUser> userManager ,SignInManager<ApplicationUser> signInManager,IMapper mapper,RoleManager<IdentityRole>roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.roleManager = roleManager;
        }

        public async Task<ActionResult> Index(string username)
       {

            var userName = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                var mappedUsers =  userManager.Users.Select( u => new UserDto()
                {
                    Id = u.Id,
                    DisplayName = u.DisplayName,
                    Email = u.Email,
                    Role = u.Role
                }).ToList();
                return View(mappedUsers);
            }
            else
            {
                var user=await userManager.FindByNameAsync(username);
                if (user is not null)
                {

                    var mappeduser = new UserDto()
                    {
                        Id = user.Id,
                        DisplayName = user.DisplayName,
                        Email = user.Email,
                        Role =user.Role
                    };
                    return View(new List<UserDto>() { mappeduser });
                }

                return View(new List<UserDto>() {  });

            }


            
            

        }

        [HttpGet]
        public ActionResult Create()
        {


            return View();
        }

        [HttpPost]

        public async Task<ActionResult> Create(UserDto userVM)
        {
           
            if (ModelState.IsValid)
            {
                var check = await userManager.FindByEmailAsync(userVM.Email);

                if (check is null)
                {
                  var NameCheck=await userManager.Users.FirstOrDefaultAsync(u=>u.DisplayName==userVM.DisplayName);
                    if (NameCheck is null)
                    {
                        var mappeduser = new ApplicationUser()
                        {
                            Email = userVM.Email,
                            DisplayName = userVM.DisplayName,
                            Role = "User",
                            UserName= userVM.Email.Split('@')[0]

                        };
                       

                        var result = await userManager.CreateAsync(mappeduser, userVM.Password);

                        if (result.Succeeded)
                        {
                            var role = roleManager.Roles.FirstOrDefault(r => r.Name == "User");
                          var checkRole=  await userManager.AddToRoleAsync(mappeduser, role.Name);

                            return RedirectToAction(nameof(Index));
                        }
                        

                        foreach (var error in result.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                    }
                    ModelState.AddModelError(String.Empty, "The Name is already taken");
                }
                ModelState.AddModelError(String.Empty, "The Email is already taken");
            }



            return View(userVM);
        }


        public async Task<ActionResult>Edit(string id, string name = "Update")
        {

            if (id is null)
                return BadRequest();

            var user = await userManager.FindByIdAsync(id);

            var mappeduser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Role = user.Role

            };

            return View(name, mappeduser);
        }


        [HttpGet]
        public async  Task<ActionResult> Update(string id)
        {
            
            
            return await Edit(id,"Update");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update([FromRoute] string id ,UserDto userVM)
        {
            if (id != userVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await userManager.FindByIdAsync(id);
                    user.DisplayName = userVM.DisplayName;
                    user.Email = userVM.Email;

                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var role = await userManager.IsInRoleAsync(user,"User");
                        if (!role )
                        {
                            var check = await userManager.AddToRoleAsync(user, "User");
                        }
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                       
                        ModelState.AddModelError(string.Empty,"Failed Updated ! please reverse your data if its match already exists if falid!");
                        return View(userVM);
                    }
                }catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(userVM);


        }
        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
           return await Edit(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< ActionResult> Delete(string id, UserDto userVM)
        {

            if(id !=userVM.Id)
                return BadRequest();
            if(ModelState.IsValid)
            {
                var user=await userManager.FindByIdAsync(id);
                var check =await userManager.IsInRoleAsync(user, "User");
                if (check)
                {
                    await userManager.RemoveFromRoleAsync(user, "User");
                }

               var  result= await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Faid To Update");

                }
            }

            return View(userVM);
        }

      

    }
}
