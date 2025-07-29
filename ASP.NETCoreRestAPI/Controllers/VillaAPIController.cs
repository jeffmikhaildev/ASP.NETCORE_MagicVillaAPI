using ASP.NETCoreRestAPI.Data;
using ASP.NETCoreRestAPI.Models;
using ASP.NETCoreRestAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCoreRestAPI.Controllers
{
    // *** [Route("api/[controller]")] -- also works, but using a specific route for clarity
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;
        private readonly ApplicationDbContext _db;

        public VillaAPIController(ApplicationDbContext db, ILogger<VillaAPIController> logger)
        {
            _logger = logger;
            _db = db;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation("Getting all villas");

            return Ok(_db.Villas.ToList());
        }


        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {

            if (id == 0)
            {
                _logger.LogError("Invalid villa ID: {Id}", id);
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                _logger.LogWarning("Villa with ID {Id} not found", id);
                return NotFound();
            }


            _logger.LogInformation("Returning villa with ID {Id}", id);
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
        {
            // *** Custom validation in ModelState
            if (_db.Villas.FirstOrDefault(v => v.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already exists!");

                _logger.LogError("Villa with name {Name} already exists", villaDTO.Name);
                return BadRequest(ModelState);
            }

            if (villaDTO == null)
            {
                _logger.LogError("Villa data is null");
                return BadRequest(villaDTO);
            }

            if (villaDTO.Id > 0)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Villa model = new Villa()
            {
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                ImageUrl = villaDTO.ImageUrl,
                Occupancy = villaDTO.Occupancy,
                Sqft = villaDTO.Sqft,
                Rate = villaDTO.Rate,
                Amenity = villaDTO.Amenity,

            };

            _db.Villas.Add(model);
            _db.SaveChanges();

            _logger.LogInformation("Villa created with ID {Id}", villaDTO.Id);

            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Invalid villa ID: {Id}", id);
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                _logger.LogWarning("Villa with ID {Id} not found", id);
                return NotFound();
            }

            _db.Villas.Remove(villa);
            _db.SaveChanges();

            _logger.LogInformation("Villa with ID {Id} deleted", id);

            return NoContent();
        }


        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            _logger.LogInformation("PUT request received for ID {Id}", id);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Model state is invalid.");
                return BadRequest(ModelState);
            }

            if (villaDTO == null)
            {
                _logger.LogError("Villa DTO is null or ID mismatch: {Id}", id);
                return BadRequest();
            }

            var villaFromDb = _db.Villas.FirstOrDefault(v => v.Id == id);

            if (villaFromDb == null)
            {
                _logger.LogWarning("Villa with ID {Id} not found", id);
                return NotFound();
            }

            // Update the fields
            villaFromDb.Name = villaDTO.Name;
            villaFromDb.Details = villaDTO.Details;
            villaFromDb.ImageUrl = villaDTO.ImageUrl;
            villaFromDb.Occupancy = villaDTO.Occupancy;
            villaFromDb.Sqft = villaDTO.Sqft;
            villaFromDb.Rate = villaDTO.Rate;
            villaFromDb.Amenity = villaDTO.Amenity;

            _db.SaveChanges();

            _logger.LogInformation("Villa with ID {Id} updated successfully", id);

            return NoContent();
        }



        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                _logger.LogError("Invalid patch data or ID: {Id}", id);
                return BadRequest();
            }

            var villaFromDb = _db.Villas.FirstOrDefault(v => v.Id == id);

            if (villaFromDb == null)
            {
                _logger.LogWarning("Villa with ID {Id} not found for patch update", id);
                return NotFound();
            }

            // Map to DTO
            VillaDTO villaDTO = new()
            {
                Id = villaFromDb.Id,
                Name = villaFromDb.Name,
                Details = villaFromDb.Details,
                ImageUrl = villaFromDb.ImageUrl,
                Occupancy = villaFromDb.Occupancy,
                Sqft = villaFromDb.Sqft,
                Rate = villaFromDb.Rate,
                Amenity = villaFromDb.Amenity
            };

            // Apply patch
            patchDTO.ApplyTo(villaDTO, ModelState);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Model state is invalid for villa with ID {Id}", id);
                return BadRequest(ModelState);
            }

            // Update the tracked entity manually
            villaFromDb.Name = villaDTO.Name;
            villaFromDb.Details = villaDTO.Details;
            villaFromDb.ImageUrl = villaDTO.ImageUrl;
            villaFromDb.Occupancy = villaDTO.Occupancy;
            villaFromDb.Sqft = villaDTO.Sqft;
            villaFromDb.Rate = villaDTO.Rate;
            villaFromDb.Amenity = villaDTO.Amenity;

            _db.SaveChanges();

            _logger.LogInformation("Villa with ID {Id} partially updated", id);

            return NoContent();
        }


    }
}
