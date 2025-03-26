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
    public class LeaveTypesAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LeaveTypesAPIController> _logger;
        public LeaveTypesAPIController(ApplicationDbContext context, ILogger<LeaveTypesAPIController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/LeaveTypesAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveType>>> GetLeaveTypes()
        {
            return await _context.LeaveTypes.ToListAsync();
        }

        // GET: api/LeaveTypesAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveType>> GetLeaveType(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);

            if (leaveType == null)
            {
                return NotFound();
            }

            return leaveType;
        }

        // PUT: api/LeaveTypesAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeaveType(int id, LeaveType leaveType)
        {
            if (id != leaveType.Id)
            {
                return BadRequest();
            }

            _context.Entry(leaveType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveTypeExists(id))
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

        // POST: api/LeaveTypesAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LeaveType>> PostLeaveType(LeaveType leaveType)
        {
            _context.LeaveTypes.Add(leaveType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLeaveType", new { id = leaveType.Id }, leaveType);
        }

        // DELETE: api/LeaveTypesAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaveType(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType == null)
            {
                return NotFound();
            }

            _context.LeaveTypes.Remove(leaveType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LeaveTypeExists(int id)
        {
            return _context.LeaveTypes.Any(e => e.Id == id);
        }
    }
}
