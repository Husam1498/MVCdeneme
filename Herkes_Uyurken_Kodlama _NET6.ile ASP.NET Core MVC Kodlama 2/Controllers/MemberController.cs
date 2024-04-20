using AutoMapper;
using Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Entites;
using Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Helpers;
using Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Controllers
{
    [Authorize(Roles ="admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class MemberController : Controller
    {
        private readonly DatabaseContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHasher _hasher;

        public MemberController(DatabaseContext dbContext, IMapper mapper, IHasher hasher)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hasher = hasher;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MemberListPartial()
        {
            List<UserModel> users = _dbContext.Users.ToList().Select(x => _mapper.Map<UserModel>(x)).ToList();
            return PartialView("_MemberListPartial", users);

        }
        public IActionResult AddNewUserPartial()
        {
           
            return PartialView("_AddNewUserPartial", new CreateUserModel());

        }

        [HttpPost]
        public IActionResult AddNewUser(CreateUserModel model)
        {
            if(ModelState.IsValid)
            {
                if (_dbContext.Users.Any(x => x.UserName.ToLower() == model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username), " Database te username mevcut");
                    return PartialView("_AddNewUserPartial", model);
                }


                User user = _mapper.Map<User>(model);
                user.Password = _hasher.DoMd5HashedString(model.Password);
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                return PartialView("_AddNewUserPartial", new CreateUserModel { Done="User aded."} );

            }
            return PartialView("_AddNewUserPartial", model);

        }

        public IActionResult  EditUserPartial(Guid id)
        {
            User user = _dbContext.Users.Find(id);
            EditUserModel model = _mapper.Map<EditUserModel>(user);

            return PartialView("_EditUserPartial", model);

        }

        [HttpPost]
        public IActionResult EditUser(Guid id, EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (_dbContext.Users.Any(x => x.UserName.ToLower() == model.Username.ToLower() && x.Id != id))
                {
                    ModelState.AddModelError(nameof(model.Username), " Database te username mevcut lutfen başak bir username girin");
                    return PartialView("_EditUserPartial", model);
                }
                User user = _dbContext.Users.Find(id);

                _mapper.Map(model, user);
                _dbContext.SaveChanges();
                return PartialView("_EditUserPartial", new EditUserModel { Done = "User Updated." });

            }

            return PartialView("_EditUserPartial", model);
        }

        public IActionResult DeleteUser(Guid id)
        {
            User user=_dbContext.Users.Find(id);
            
            if(user != null)
            {
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
            }
            return MemberListPartial();
        }
    }
}
