using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemasSalgados.Extensions;
using SistemasSalgados.Models.Base;
using SistemasSalgados.Services.Interfaces.Base;

namespace SistemasSalgados.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<TEntity> : ControllerBase where TEntity : BaseEntity
    {
        protected readonly IBaseService<TEntity> _baseService;
        private readonly IHttpContextAccessor _contextAccessor;
        protected int _userId;

        public BaseController(IBaseService<TEntity> baseService)
        {
            _baseService = baseService;
        }

        public BaseController(IBaseService<TEntity> baseService, IHttpContextAccessor contextAccessor)
        {
            this._baseService = baseService;
            _contextAccessor = contextAccessor;
            GetUserId();
        }

        private void GetUserId()
        {
            string _userId = _contextAccessor.HttpContext.User?.Identity.GetUserId();
            int.TryParse(_userId, out this._userId);
        }

        [HttpGet("{id:int}")]
        public virtual async Task<IActionResult> GetOne([FromRoute] int id)
        {
            TEntity entity = await _baseService.SelectById(id);

            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAll([FromQuery] QueryParams queryParams)
        {
            IQueryable<TEntity> query = _baseService.Select();

            var pagedResult = await ApplyPaginationParams(queryParams, query);

            if (pagedResult == null)
                return NotFound();

            return Ok(pagedResult);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post(TEntity entity)
        {
            var result = await _baseService.Insert(entity);

            if (result.HasError)
                return BadRequest(result);

            return Created("", entity);
        }

        [HttpPut("{id:int}")]
        public virtual async Task<IActionResult> Put([FromRoute] int id, TEntity entity)
        {
            var result = await _baseService.Update(id, entity);

            if (result.HasError)
                return BadRequest(result);

            return Ok(entity);
        }

        protected virtual async Task<PagedResult<TEntity>> ApplyPaginationParams(QueryParams queryParams, IQueryable<TEntity> query)
        {
            if (queryParams.ActiveItems)
                query = query.Where(p => p.IsActive);

            int count = await query.CountAsync();

            if (!queryParams.BringAll)
            {
                query = query.Skip(queryParams.Offset)
                                .Take(queryParams.PageSize);
            }

            List<TEntity> data = await query.ToListAsync();

            if (data == null || data.Count == 0)
                return null;

            return new(queryParams)
            {
                Count = count,
                Data = data,
            };
        }
    }
}
