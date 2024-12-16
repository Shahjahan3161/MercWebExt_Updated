using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using MercWebExt.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using MercWebExt.Services;
using MercWebExt.Models.ViewModels;
using MercWebExt.Models.DataBase;

namespace MercWebExt.Controllers
{
    public class AccountController : Controller
    {
        private readonly IContextService _context; 
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger, IContextService context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult LoginUser()
        {
            return View();
        }

        //GET: Login (Default Page)
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            try
            {
                if (Request.HttpContext.User.Identity.IsAuthenticated)
                {
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        returnUrl = "/Home/Index";
                    }

                    return View(returnUrl);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return View();
        }


        // POST: Validate
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ValidateAsync(ViewLogin user)
        {
            if (_context.GetDataBaseContext().UsersUser.Where(u => u.UserName.Equals(user.UserName)).Any())
            {
                var _user = _context.GetDataBaseContext().UsersUser.Where(u => u.UserName.Equals(user.UserName)).FirstOrDefault();
                
                if (CustomPasswordHasher.Verify(user.Password, _user.Password))
                {
                    if (_user.IsActived.Value)
                    {
                        if (_user.IsNonExpired.Value)
                        {
                            int roleId = _user.RoleId;
                            string roleName = _context.GetDataBaseContext().UsersRole.Where(r => r.Rid.Equals(roleId)).FirstOrDefault().RoleName;

                            var identity = new ClaimsIdentity(new[]
                           {new Claim(ClaimTypes.Name, _user.UserName), new Claim(ClaimTypes.Role, roleName)});

                            var principal = new ClaimsPrincipal(identity);

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                            _logger.LogError("Login by " + _user.UserName +"(" + roleName +")");

                            // Move to Index Page depend on Role
                            if(roleName.Equals("Job Admin"))
                            {
                                return RedirectToAction("Index", "Career");
                            }
                            else if((roleName.Equals("WineWood")))
                            {
                                return RedirectToAction("Index", "WineWood");
                            }
                            else if((roleName.Equals("Site Manager")))
                            {
                                return RedirectToAction("InductionList", "Induction");
                            }

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewBag.Color = "class=text-red";
                            ViewBag.Message = "Your Password is expired ";
                            return View("Login");
                        }
                    }
                    else
                    {
                        ViewBag.Color = "class=text-red";
                        ViewBag.Message = "Your Account is not activated";
                        return View("Login");
                    }
                }
                else
                {
                    ViewBag.Color = "class=text-red";
                    ViewBag.Message = "Please Check your Password or User Name";
                    return View("Login");
                }
            }
            else
            {
                ViewBag.Color = "class=text-red";
                ViewBag.Message = "Invalid User Name or Password";
                return View("Login");
            }

        }

        //GET: Register Page
        [Authorize(Roles = "Admin")]
        public ActionResult Register()
        {
            return View();
        }

        //POST: Register (Action)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult RegisterUser(UsersUser user)
        {
            var newUser = new UsersUser
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                //Default User = 2
                RoleId = 2,
                Password = CustomPasswordHasher.Hash(user.Password)
            };

            _context.GetDataBaseContext().UsersUser.Add(newUser);
            _context.GetDataBaseContext().SaveChanges();
            ViewBag.Message = "Succesful";

            return View("Login");
        }

        [AllowAnonymous]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _logger.LogError("Logout");
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    
    }

}
