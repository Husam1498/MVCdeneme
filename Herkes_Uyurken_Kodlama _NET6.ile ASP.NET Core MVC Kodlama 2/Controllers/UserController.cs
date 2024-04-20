using AutoMapper;
using Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Entites;
using Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Helpers;
using Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Controllers
{

    [Authorize(Roles = "admin", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {

        private readonly DatabaseContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHasher _hasher;

        public UserController(DatabaseContext dbContext, IMapper mapper, IHasher hasher)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hasher = hasher;
        }

        public IActionResult Index()
        {

            //List<User> users = _dbContext.Users.ToList();
            //List<UserModel> model = users.Select(x => _mapper.Map<UserModel>(x)).ToList();//userslistesini al ve içerisini gez her birini usermodel e çevir

            List<UserModel> users=_dbContext.Users.ToList().Select(x =>_mapper.Map<UserModel>(x)).ToList();

            return View(users);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateUserModel model)
        {
            if(ModelState.IsValid)
            {

                if(_dbContext.Users.Any(x => x.UserName.ToLower()== model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username)," Database te username mevcut");
                    return View(model);
                }


                User user =_mapper.Map<User>(model);
                user.Password=_hasher.DoMd5HashedString(model.Password);

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));

            }

            return View(model);
        }

        public IActionResult Edit(Guid id)
        {
            User user =_dbContext.Users.Find(id);
            EditUserModel model =_mapper.Map<EditUserModel>(user);


            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Guid id, EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (_dbContext.Users.Any(x => x.UserName.ToLower() == model.Username.ToLower() && x.Id!=id))
                {
                    ModelState.AddModelError(nameof(model.Username), " Database te username mevcut lutfen başak bir username girin");
                    return View(model);
                }
                User user = _dbContext.Users.Find(id);
                
                _mapper.Map(model,user);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));

            }

            return View(model);
        }

       
        public IActionResult Delete(Guid id)
        {
            User user = _dbContext.Users.Find(id);

            if(user != null)
            {

                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();

            }
            return RedirectToAction(nameof(Index));
        }


    }
}
