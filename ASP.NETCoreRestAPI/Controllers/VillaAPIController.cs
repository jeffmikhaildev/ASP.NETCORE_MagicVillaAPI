using ASP.NETCoreRestAPI.Data;
using ASP.NETCoreRestAPI.Models;
using ASP.NETCoreRestAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NETCoreRestAPI.Controllers
{
    // *** [Route("api/[controller]")] -- also works, but using a specific route for clarity
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogger<VillaAPIController> _logger;
        public VillaAPIController(ILogger<VillaAPIController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation("Getting all villas");

            return Ok(VillaStore.villaList);
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

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

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
            if (VillaStore.villaList.FirstOrDefault(v => v.Name.ToLower() == villaDTO.Name.ToLower()) != null)
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

            villaDTO.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;


            VillaStore.villaList.Add(villaDTO);
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

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                _logger.LogWarning("Villa with ID {Id} not found", id);
                return NotFound();
            }

            VillaStore.villaList.Remove(villa);
            _logger.LogInformation("Villa with ID {Id} deleted", id);

            return NoContent();
        }


        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
            {

                _logger.LogError("Villa data is null or ID mismatch: {Id}", id);
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                _logger.LogWarning("Villa with ID {Id} not found for update", id);
                return NotFound();
            }

            villa.Name = villaDTO.Name;
            villa.Occupancy = villaDTO.Occupancy;
            villa.Sqft = villaDTO.Sqft;

            _logger.LogInformation("Villa with ID {Id} updated", id);

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


            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                _logger.LogWarning("Villa with ID {Id} not found for patch update", id);
                return NotFound();
            }

            if (villa.Id != id)
            {
                _logger.LogError("ID mismatch: expected {ExpectedId}, got {ActualId}", id, villa.Id);
                return BadRequest();
            }

            patchDTO.ApplyTo(villa, ModelState);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Model state is invalid after patching villa with ID {Id}", id);
                return BadRequest(ModelState);
            }

             
            return NoContent();
        }

    }
}
