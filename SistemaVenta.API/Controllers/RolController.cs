using Microsoft.AspNetCore.Mvc;

namespace SistemaVenta.API.Controllers
{

    using SistemaVenta.BLL.Services.Contract;
    using SistemaVenta.DTO;
    using SistemaVenta.API.Utility;
    using SistemaVenta.BLL.Services;



    [Route("api/v1/[controller]")]
    [ApiController]
    public class RolController : Controller
    {
        private readonly IRolService _rolService;

        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Lista()
        {
            var rsp = new Response<List<RolDTO>>();

            try
            {
                rsp.status = true;
                rsp.value =  await _rolService.Lista();
            }
            catch (Exception ex)
            {

                rsp.status = false;
                rsp.msg = ex.Message;

            }

            return Ok(rsp);
        }
    }
}
