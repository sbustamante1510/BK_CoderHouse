using BK_CoderHouse.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BK_CoderHouse.Controllers
{
    [Route("api/tienda")]
    [ApiController]

    public class TiendaController : ControllerBase
    {
        private readonly ITiendaService _tiendaService;

        public TiendaController(ITiendaService tiendaService)
        {
            _tiendaService = tiendaService;
        }

        [HttpGet("listar")]
        public async Task<ActionResult> List([FromQuery] string? category) => Ok(await _tiendaService.GetTienda(category));
    }
}
