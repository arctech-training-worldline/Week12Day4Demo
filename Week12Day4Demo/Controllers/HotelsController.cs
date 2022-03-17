using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Week12Day4Demo.Data;
using Week12Day4Demo.Data.Models;
using Week12Day4DemoApi.Dtos;

namespace Week12Day4Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HotelsController> _logger;

        public HotelsController(ApplicationDbContext context, ILogger<HotelsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
        {
            var hotels =
                await _context.Hotels
                .Select(h => new HotelDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Address = h.Address,
                    PhotoUrl = h.PhotoUrl,
                    CostPerNight = h.CostPerNight
                })
                .ToListAsync();

            return hotels;
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            return new HotelDto
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Address = hotel.Address,
                PhotoUrl = hotel.PhotoUrl,
                CostPerNight = hotel.CostPerNight
            };
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelDto hotel)
        {
            if (id != hotel.Id)
            {
                return BadRequest();
            }

            var newHotel = new Hotel
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Address = hotel.Address,
                PhotoUrl = hotel.PhotoUrl,
                CostPerNight = hotel.CostPerNight
            };

            _context.Entry(newHotel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(id))
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

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HotelDto>> PostHotelAsync(HotelDto hotel)
        {
            try
            {
                var newHotel = new Hotel
                {
                    Name = hotel.Name,
                    Address = hotel.Address,
                    PhotoUrl = hotel.PhotoUrl,
                    CostPerNight = hotel.CostPerNight
                };

                _context.Hotels.Add(newHotel);
                await _context.SaveChangesAsync();

                hotel.Id = newHotel.Id;

                return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Adding Hotel {Id}-{Name}", hotel.Id, hotel.Name);
            }

            return BadRequest();
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HotelExists(int id)
        {
            return _context.Hotels.Any(e => e.Id == id);
        }
    }
}
