using Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Entites;
using Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Helpers;
using Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETCore.Encrypt.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Controllers
{
    [Authorize(AuthenticationSchemes= CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AccountController : Controller
    {
        private readonly DatabaseContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHasher _hasher;

        public AccountController(DatabaseContext dbContext, IConfiguration configuration, IHasher hasher)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _hasher = hasher;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string hashed = _hasher.DoMd5HashedString(model.Password);

                User user = _dbContext.Users.SingleOrDefault(x => x.UserName.ToLower() == model.Username.ToLower() && x.Password == hashed);


                if (user != null)
                {
                    if (user.Locked)
                    {
                        ModelState.AddModelError("", "Kullanıcıaktif değil lütfen yetkili kişi ile konuşun");
                        return View(model);
                    }
                    else
                    {
                        List<Claim> claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                        claims.Add(new Claim(ClaimTypes.Name, user.Fullname ?? string.Empty));
                        claims.Add(new Claim(ClaimTypes.Role, user.Role));
                        claims.Add(new Claim("Username", user.UserName));
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                            CookieAuthenticationDefaults.AuthenticationScheme);
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifreniz hatalı");
                }



            }
            return View(model);
        }

        

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_dbContext.Users.Any(x => x.UserName.ToLower() == model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username), "Kaydınız var login yapınız");

                    View(model);
                }
                else
                {

                    string hashed = _hasher.DoMd5HashedString(model.Password);

                    User user = new User()
                    {
                        Fullname = model.Fullname,
                        UserName = model.Username,
                        Password = hashed
                    };
                    _dbContext.Users.Add(user);
                }
            }
            int affectedRowCount = _dbContext.SaveChanges();
            if (affectedRowCount == 0)
            {
                ModelState.AddModelError("Username", "Veri Tabanına kayıt Başarısız");
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }
            return View(model);
        }


        public IActionResult Profile()
        {
            ProfileInfoLoader();

            return View();
        }

        private void ProfileInfoLoader()
        {
            Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            User user = _dbContext.Users.SingleOrDefault(u => u.Id == userid);

            ViewData["Fullname"] = user.Fullname;
            ViewData["ProfileImage"] = user.ProfileImageFileName;

        }

        [HttpPost]
        public IActionResult ProfileChangeFullName([Required][StringLength(30)] string? fullname)
        {
            if (ModelState.IsValid)
            {
                Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

                User user = _dbContext.Users.SingleOrDefault(u => u.Id == userid);
                user.Fullname = fullname;
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Profile));
            }
            ProfileInfoLoader();//fullname hatalarının gözükmesi için
            return View("Profile");
        }


        [HttpPost]
        public IActionResult ProfileChangePassword([Required][MinLength(3)][MaxLength(8)] string password)
        {
            if (ModelState.IsValid)
            {
                Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

                User user = _dbContext.Users.SingleOrDefault(u => u.Id == userid);
                string hashedPassword = _hasher.DoMd5HashedString(password);

                user.Password = hashedPassword;
                _dbContext.SaveChanges();

                ViewData["result"] = "PasswordChange";
            }
            ProfileInfoLoader();//fullname hatalarının gözükmesi için
            return View("Profile");
        }

        [HttpPost]
        public IActionResult ProfileChangeImage([Required] IFormFile file)
        {
            if (ModelState.IsValid)
            {
                Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = _dbContext.Users.SingleOrDefault(u => u.Id == userid);
                //p_guid.png
                string filename = $"p_{userid}.jpg";
               // string filenameCoklu = $"p_{userid}.{file.ContentType.Split('/')[1]}"; //image/png  image/jpg

                Stream stream = new FileStream($"wwwroot/upload/{filename}",FileMode.OpenOrCreate);


                file.CopyTo(stream);
                stream.Close();
                stream.Dispose();
                
                user.ProfileImageFileName=filename;
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Profile));


            }
            ProfileInfoLoader();//fullname hatalarının gözükmesi için
            return View("Profile");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }



    }
}
