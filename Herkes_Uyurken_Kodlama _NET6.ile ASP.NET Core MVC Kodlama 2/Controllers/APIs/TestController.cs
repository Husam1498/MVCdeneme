using Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Helpers;
using Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Controllers.APIs
{
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITokenHelper _tokenHelper;

        public TestController(ITokenHelper tokenHelper)
        {
            _tokenHelper = tokenHelper;
        }


        //[HttpGet("get-data")]
        [HttpGet]
        public IActionResult GetData()
        {

            return Ok(new {Name="muratt", Surname="djanf"});
        }
        [HttpPost]
        public IActionResult PostData([FromBody]PostDataApiModel model)
        {
            return Ok(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody]LoginViewModel model)
        {
            if(model.Username=="husam"&&model.Password=="123")
            {
                string token = _tokenHelper.GenerateToken(model.Username,0);
                return Ok( new {Error=false,Message="token başarılı",Data=token} );
            }
            else
            {

                return BadRequest(new {Error=true,Message="hatallı giriş"});
            }
        }

    }
}
