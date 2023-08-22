using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalCoderHouse.EntityORM;
using ProyectoFinalCoderHouse.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinalCoderHouse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericCrudController<T> : ControllerBase where T : EntityBase
    {
        protected readonly SistemaGestionContext _context;
        public GenericCrudController(SistemaGestionContext context)
        {

            _context = context;
        }

        /// <summary>
        /// Traer una Lista de todos los T existentes.
        /// </summary>
        /// <returns></returns>
        [HttpGet("List")]
        public virtual async Task<IActionResult> List() {
            var list = await _context.Set<T>().ToListAsync();
            return Ok(list);
        }

        [HttpGet("Codigo{id}")]
        public virtual async Task<IActionResult> Codigo(long id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return NotFound(id);

            }
            return Ok(entity);
        }

        [HttpPost("Create")]
        public virtual async Task<IActionResult> Create(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Codigo", new { id = entity.Id }, entity);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(long id,[FromBody]T entity)
        {
            if (id != entity.Id)
            {
                return BadRequest();
            }
            if (!await EntityExists(id)) {
                return NotFound();
            }
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return NotFound(id);

            }
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private Task<bool> EntityExists(long id)
        {
            return _context.Set<T>().AnyAsync(e => e.Id == id);
        }
    }
}
