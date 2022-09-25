using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemasSalgados.Models.Base;
using SistemasSalgados.Services.Interfaces.Base;

namespace SistemasSalgados.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<TEntity> : ControllerBase where TEntity : BaseEntity
    {
        protected readonly IBaseService<TEntity> _baseService;

        public BaseController(IBaseService<TEntity> baseService)
        {
            _baseService = baseService;
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
        public virtual async Task<IActionResult> GetAll()
        {
            List<TEntity> items = await _baseService.Select().ToListAsync();

            if (items == null || items.Count == 0)
                return NotFound();

            return Ok(items);
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
    }
}
