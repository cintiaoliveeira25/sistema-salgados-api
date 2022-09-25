using Microsoft.AspNetCore.Mvc;
using SistemasSalgados.Controllers.Base;
using SistemasSalgados.Models.Auth;
using SistemasSalgados.Services.Interfaces.Auth;

namespace SistemasSalgados.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController<User>
    {
        public UsersController(IUserService baseService) : base(baseService)
        { }
    }
}
