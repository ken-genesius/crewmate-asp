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
    public class LeaveApplicationsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LeaveApplicationsAPIController> _logger;
        public LeaveApplicationsAPIController(ApplicationDbContext context, ILogger<LeaveApplicationsAPIController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/LeaveApplicationsAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveApplication>>> GetLeaveApplications()
        {
            return await _context.LeaveApplications.ToListAsync();
        }

        // GET: api/LeaveApplicationsAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveApplication>> GetLeaveApplication(int id)
        {
            var leaveApplication = await _context.LeaveApplications.FindAsync(id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            return leaveApplication;
        }

        // PUT: api/LeaveApplicationsAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeaveApplication(int id, LeaveApplication leaveApplication)
        {
            if (id != leaveApplication.Id)
            {
                return BadRequest();
            }

            _context.Entry(leaveApplication).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveApplicationExists(id))
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

        // POST: api/LeaveApplicationsAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LeaveApplication>> PostLeaveApplication(LeaveApplication leaveApplication)
        {
            _context.LeaveApplications.Add(leaveApplication);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLeaveApplication", new { id = leaveApplication.Id }, leaveApplication);
        }

        // DELETE: api/LeaveApplicationsAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaveApplication(int id)
        {
            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            _context.LeaveApplications.Remove(leaveApplication);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LeaveApplicationExists(int id)
        {
            return _context.LeaveApplications.Any(e => e.Id == id);
        }
    }
}
