using HotelManagement.Areas.Admin.Models;
using HotelManagement.Areas.Admin.Models.Account;
using HotelManagement.Common.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace HotelManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HotelController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<HotelController> _logger;
        private readonly IEmailSender _emailSender;
        public HotelController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<HotelController> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Register(string returnUrl = null)
        {
            var model = new RegisterModel();
            model.ReturnUrl = returnUrl;
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();



            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.ActionLink(
                        "/Account/ConfirmEmail",
                        values: new { userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToAction("Index", "Hotel", new { email = model.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [TempData]
        public string ErrorMessage { get; set; }


        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            var model = new LoginModel();
            model.ReturnUrl = returnUrl;

            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToAction("DashBoard");
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Hotel");
            }
        }




        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DashBoard()
        {
            return View();
        }

        public IActionResult ManageRoom()
        {
            ViewBag.SomeData = "Hello From Asp.Net";
            var model = new RoomListModel();
            return View(model);
        }
        public JsonResult GetRoomData()
        {
            var dataTablesModel = new DataTablesAjaxRequestModel(Request);
            var model = new RoomListModel();
            var data = model.GetRooms(dataTablesModel);
            return Json(data);
        }
        public IActionResult AddRoom()
        {
            var model = new AddRoomModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddRoom(AddRoomModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.AddRoom();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Failed to Add Room");
                    _logger.LogError(ex, "Add room Failed");
                }

            }
            return View(model);
        }
        public IActionResult EditRoom(int id)
        {
            var model = new EditRoomModel();
            model.LoadModelData(id);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditRoom(EditRoomModel model)
        {
            if (ModelState.IsValid)
            {
                model.Update();
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]

        public IActionResult Delete(int id)
        {
            var model = new RoomListModel();
            model.Delete(id);

            return RedirectToAction(nameof(ManageRoom));

        }




        public IActionResult UserLogin()
        {
            return View();
        }
        public IActionResult ManageUser()
        {
            ViewBag.SomeData = "Hello From Asp.Net";
            var model = new UserListModel();
            return View(model);
        }
        public JsonResult GetUserData()
        {
            var dataTablesModel = new DataTablesAjaxRequestModel(Request);
            var model = new UserListModel();
            var data = model.GetUsers(dataTablesModel);
            return Json(data);
        }
        public IActionResult AddUser()
        {
            var model = new AddUserModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddUser(AddUserModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.AddUser();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Failed to Add User");
                    _logger.LogError(ex, "Add user Failed");
                }

            }
            return View(model);
        }
        public IActionResult EditUser(int id)
        {
            var model = new EditUserModel();
            model.LoadModelData(id);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditUser(EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                model.Update();
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]

        public IActionResult DeleteUser(int id)
        {
            var model = new UserListModel();
            model.Delete(id);

            return RedirectToAction(nameof(ManageUser));

        }






        public IActionResult ManageBooking()
        {
            ViewBag.SomeData = "Hello From Asp.Net";
            var model = new BookingListModel();
            return View(model);
        }
        public JsonResult GetBookingData()
        {
            var dataTablesModel = new DataTablesAjaxRequestModel(Request);
            var model = new BookingListModel();
            var data = model.GetBookings(dataTablesModel);
            return Json(data);
        }
        public IActionResult AddBooking()
        {
            var model = new AddBookingModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddBooking(AddBookingModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.AddBooking();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Failed to Add Booking");
                    _logger.LogError(ex, "Add booking Failed");
                }

            }
            return View(model);
        }
        public IActionResult EditBooking(int id)
        {
            var model = new EditBookingModel();
            model.LoadModelData(id);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditBooking(EditBookingModel model)
        {
            if (ModelState.IsValid)
            {
                model.Update();
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]

        public IActionResult DeleteBooking(int id)
        {
            var model = new BookingListModel();
            model.Delete(id);

            return RedirectToAction(nameof(ManageBooking));

        }

        public IActionResult ViewRoom()
        {
            ViewBag.SomeData = "Hello From Asp.Net";
            var model = new RoomListModel();
            return View(model);
        }



    }
}
