using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrewMate.Data;
using CrewMate.Models;

namespace CrewMate.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemCodeDetailsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SystemCodeDetailsAPIController> _logger;

        public SystemCodeDetailsAPIController(ApplicationDbContext context, ILogger<SystemCodeDetailsAPIController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/SystemCodeDetailsAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SystemCodeDetail>>> GetSystemCodeDetails()
        {
            return await _context.SystemCodeDetails.ToListAsync();
        }

        // GET: api/SystemCodeDetailsAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SystemCodeDetail>> GetSystemCodeDetail(int id)
        {
            var systemCodeDetail = await _context.SystemCodeDetails.FindAsync(id);

            if (systemCodeDetail == null)
            {
                return NotFound();
            }

            return systemCodeDetail;
        }

        // PUT: api/SystemCodeDetailsAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSystemCodeDetail(int id, SystemCodeDetail systemCodeDetail)
        {
            if (id != systemCodeDetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(systemCodeDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SystemCodeDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SystemCodeDetailsAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SystemCodeDetail>> PostSystemCodeDetail(SystemCodeDetail systemCodeDetail)
        {
            _context.SystemCodeDetails.Add(systemCodeDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSystemCodeDetail", new { id = systemCodeDetail.Id }, systemCodeDetail);
        }

        // DELETE: api/SystemCodeDetailsAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSystemCodeDetail(int id)
        {
            var systemCodeDetail = await _context.SystemCodeDetails.FindAsync(id);
            if (systemCodeDetail == null)
            {
                return NotFound();
            }

            _context.SystemCodeDetails.Remove(systemCodeDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SystemCodeDetailExists(int id)
        {
            return _context.SystemCodeDetails.Any(e => e.Id == id);
        }
    }
}
