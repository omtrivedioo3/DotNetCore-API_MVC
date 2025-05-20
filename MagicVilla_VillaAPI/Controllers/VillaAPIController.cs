using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    // ControllerBase - use for API model
    // Controller - Use for MVC structure to return view
    [Route("api/VillaAPI")]
    [ApiController] // to enable API controller features and validation
    public class VillaAPIController : ControllerBase
    {
        public ApplicationDbContext _db;

        public VillaAPIController(ApplicationDbContext db)
        {
            _db = db;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] // 200 OK
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(_db.Villas.ToList());
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)] // 200 OK
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // 400 Bad Request
        [ProducesResponseType(StatusCodes.Status404NotFound)] // 404 Not Found
        public ActionResult<VillaDTO> GetVillas(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var vill = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (vill == null)
            {
                return NotFound();
            }
            return Ok(vill);

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)] // 201 Created
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // 400 Bad Request
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            if (_db.Villas.FirstOrDefault(u => u.Id == villaDTO.Id) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already exists!");
                return BadRequest(ModelState);
            }
            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
           Villa model = new()
           {
               Name = villaDTO.Name,
               Details = villaDTO.Details,
               ImageUrl = villaDTO.ImageUrl,
               Rate = villaDTO.Rate,
               Amenity = villaDTO.Amenity,
               Occupancy = villaDTO.Occupancy,
               Sqft = villaDTO.Sqft,
               CreatedDate = DateTime.Now,
               UpdatedDate = DateTime.Now
           };
            _db.Villas.Add(model);
            _db.SaveChanges();
            return CreatedAtRoute("GetVilla",new {id= villaDTO.Id} ,villaDTO);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // 204 No Content
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // 400 Bad Request

        [ProducesResponseType(StatusCodes.Status404NotFound)] // 404 Not Found
        public ActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            _db.Villas.Remove(villa);
            _db.SaveChanges();
            return NoContent();
        }
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // 204 No Content
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // 400 Bad Request
        [ProducesResponseType(StatusCodes.Status404NotFound)] // 404 Not Found
        public ActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            //if (villa == null)
            //{
            //    return NotFound();
            //}
            //villa.Name = villaDTO.Name;
            //villa.Occupancy = villaDTO.Occupancy;
            //villa.Sqft = villaDTO.Sqft;

            Villa model = new()
            {
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Rate = villaDTO.Rate,
                Amenity = villaDTO.Amenity,
                Occupancy = villaDTO.Occupancy,
                Sqft = villaDTO.Sqft,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            _db.Villas.Update(model);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(u => u.Id == id);

            VillaDTO villaDTO = new()
            {
                Name = villa.Name,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Rate = villa.Rate,
                Amenity = villa.Amenity,
                Occupancy = villa.Occupancy,
                Sqft = villa.Sqft
              
            };

            if (villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDTO, ModelState);
            Villa model = new()
            {
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Rate = villaDTO.Rate,
                Amenity = villaDTO.Amenity,
                Occupancy = villaDTO.Occupancy,
                Sqft = villaDTO.Sqft,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            _db.Villas.Update(model);
            _db.SaveChanges();
            //Villa model = _mapper.Map<Villa>(villaDTO);

            //await _dbVilla.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
