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
    public class ItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItem()
        {
            return await _context.Item.ToListAsync();
        }

        // GET: api/Items/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Item>> GetItem(int id)
        //{
        //    var item = await _context.Item.FindAsync(id);

        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    return item;
        //}


        //items at a certain geocache
        [HttpGet("{geocache}")]
        public async Task<ActionResult<Item>> GetItem(int geocache)
        {
            var item = await _context.Item.FindAsync(geocache);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // PUT: api/Items/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, [FromBody] Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
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

        // POST: api/Items
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem([FromQuery] string name, int? location, DateTime? active, DateTime? inactive)
        {
            //validate name
            String newname = Regex.Replace(name, @"[^0-9a-zA-Z ]+", "");

            //validate gclocation is real
            //if assigned a location at creation, make sure there's not too many in that location
            if(location != null)
            {
                var geocache = await _context.Geocache.FindAsync(location);

                if (geocache == null)
                {
                    return NotFound();
                }
                List<Item> allItems = await _context.Item.ToListAsync();
                int count = 0;
                foreach (Item it in allItems)
                {
                    if (it.Geocache == location) count++;
                }
                if (count >= 3) return BadRequest();
                active = DateTime.Today;
            }
            

            Item item = new Item()
            {
                Name = newname,
                Geocache = location,
                Active = active,
                Inactive = inactive

            };

            _context.Item.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.Id }, item);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Item>> DeleteItem(int id)
        {
            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Item.Remove(item);
            await _context.SaveChangesAsync();

            return item;
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.Id == id);
        }
    }
}
