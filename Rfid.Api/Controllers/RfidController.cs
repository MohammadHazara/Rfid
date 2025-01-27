using Microsoft.AspNetCore.Mvc;
using Rfid.Core.Interfaces.Services;
using Rfid.Core.Models;

namespace Rfid.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RfidController : ControllerBase
    {
        private IRfidService _rfidService;

        public RfidController(IRfidService rfidService)
        {
            _rfidService = rfidService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(RfidTokenDTO))]
        [ProducesResponseType(204)]
        [ProducesResponseType(500, Type = typeof(string))]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            try
            {
                var token = await _rfidService.GetByIdAsync(id);
                if (token == null)
                {
                    return NoContent();
                }
                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
