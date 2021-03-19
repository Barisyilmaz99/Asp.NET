using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Shopapp.Bussiness.Abstract;
using Shopapp.WebUI.Identity;
using Shopapp.WebUI.Models;

namespace Shopapp.WebUI.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailSender _emailSender;
        private ICartService _cartService;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, ICartService cartService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _cartService = cartService;
        }


        public IActionResult Register()
        {
            return View(new RegisterModel());
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new ApplicationUser
            { FullName = model.FullName,
                UserName = model.Username,
                Email = model.Email
            };
            _cartService.InitializeCart(user.Id);//cartı burada oluşturdum
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ComfirmEmail","Account",new {
                userId=user.Id,
                token=code
                });
                await _emailSender.SendEmailAsync(model.Email, "Hesap onay", $"Onaylamak icin Linke <a href='http://localhost:64426{callbackUrl}'>Tıklayınız.</a>");
                return RedirectToAction("login", "account");
            }
            ModelState.AddModelError("", "Bilinmeyen bir hata oluştu");
            return RedirectToAction("login","account");
        }

        public IActionResult Login(string ReturnUrl=null)
        {
            return View(new LoginModel() { 
            ReturnUrl=ReturnUrl
            
            });
        }

        
        [HttpPost]
        
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı Bulunamadı");
                return View(model);
            }

          /*  if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lütfen Giriş Yapmadan Önce Hesabınızı Onaylayınız.");
                return View(model);
            }*/
            //Mail onay API sıkıntılı diye devre dışı burası da

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);//lockout 4. şimdilik kapattım

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl ?? "~/");
            }
            else
            {
                ModelState.AddModelError("", "Hatalı Email ve Şifre Kombinasyonu.");
                return View(model);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }
        public async Task<IActionResult> ConfirmEmail(string userId,string token )
        {
            if (userId==null || token==null)
            {
                TempData["message"] = "Geçersiz token.";
                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user!=null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    //create cart object

                    TempData["message"] = "Hesabınız Onaylandı";
                    return View();
                }
                else
                {
                    TempData["message"] = "Onaylama başarısız.";
                    return View();
                }

            }
            else
            {
                TempData["message"] = "böyle bir kullanıcı yok.";
                return View();
            }
            


        }
    }
}