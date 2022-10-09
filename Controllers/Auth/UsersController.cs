using Microsoft.AspNetCore.Mvc;
using SistemasSalgados.Controllers.Base;
using SistemasSalgados.Models.Auth;
using SistemasSalgados.Models.Base;
using SistemasSalgados.Services.Interfaces.Auth;

namespace SistemasSalgados.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController<User>
    {
        public UsersController(IUserService baseService) : base(baseService)
        { }

        protected override Task<PagedResult<User>> ApplyPaginationParams(QueryParams queryParams, IQueryable<User> query)
        {
            if (!string.IsNullOrEmpty(queryParams.Search))
                query = query.Where(p => p.Name.Contains(queryParams.Search));

            return base.ApplyPaginationParams(queryParams, query);
        }
    }
}
