using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeocacheAPI.DAL;
using GeocacheAPI.Models;
using System.Text.RegularExpressions;

namespace GeocacheAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeocachesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GeocachesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Geocaches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Geocache>>> GetGeocache()
        {
            return await _context.Geocache.ToListAsync();
        }

        // GET: api/Geocaches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Geocache>> GetGeocache(int id)
        {
            var geocache = await _context.Geocache.FindAsync(id);

            if (geocache == null)
            {
                return NotFound("Geocache does not exist");
            }

            return geocache;
        }

        // PUT: api/Geocaches/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGeocache(int id, [FromBody] Geocache geocache)
        {
            if (id != geocache.Id)
            {
                return BadRequest();
            }

            _context.Entry(geocache).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeocacheExists(id))
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

        // POST: api/Geocaches
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Geocache>> PostGeocache([FromQuery] string name, string location)
        {
            //need to add format validation for coordinates

            String newname = Regex.Replace(name, @"[^0-9a-zA-Z ]+", "");
            Geocache geocache = new Geocache
            {
                Name = newname,
                Coordinates = location
            };

            _context.Geocache.Add(geocache);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeocache", new { id = geocache.Id }, geocache);
        }

        // DELETE: api/Geocaches/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Geocache>> DeleteGeocache(int id)
        {
            var geocache = await _context.Geocache.FindAsync(id);
            if (geocache == null)
            {
                return NotFound();
            }

            _context.Geocache.Remove(geocache);
            await _context.SaveChangesAsync();

            return geocache;
        }

        private bool GeocacheExists(int id)
        {
            return _context.Geocache.Any(e => e.Id == id);
        }
    }
}
