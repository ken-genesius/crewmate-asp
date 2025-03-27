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
    public class SystemCodesAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SystemCodesAPIController> _logger;
        public SystemCodesAPIController(ApplicationDbContext context, ILogger<SystemCodesAPIController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/SystemCodesAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SystemCode>>> GetSystemCodes()
        {
            return await _context.SystemCodes.ToListAsync();
        }

        // GET: api/SystemCodesAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SystemCode>> GetSystemCode(int id)
        {
            var systemCode = await _context.SystemCodes.FindAsync(id);

            if (systemCode == null)
            {
                return NotFound();
            }

            return systemCode;
        }

        // PUT: api/SystemCodesAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSystemCode(int id, SystemCode systemCode)
        {
            if (id != systemCode.Id)
            {
                return BadRequest();
            }

            _context.Entry(systemCode).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SystemCodeExists(id))
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

        // POST: api/SystemCodesAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SystemCode>> PostSystemCode(SystemCode systemCode)
        {
            _context.SystemCodes.Add(systemCode);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSystemCode", new { id = systemCode.Id }, systemCode);
        }

        // DELETE: api/SystemCodesAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSystemCode(int id)
        {
            var systemCode = await _context.SystemCodes.FindAsync(id);
            if (systemCode == null)
            {
                return NotFound();
            }

            _context.SystemCodes.Remove(systemCode);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SystemCodeExists(int id)
        {
            return _context.SystemCodes.Any(e => e.Id == id);
        }
    }
}
