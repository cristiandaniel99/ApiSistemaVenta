using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utility;
using SistemaVenta.BLL.Services;
using SistemaVenta.BLL.Services.Contract;
using SistemaVenta.DTO;

namespace SistemaVenta.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {


        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }


        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var rsp = new Response<List<ProductoDTO>>();

            try
            {
                rsp.status = true;
                rsp.value =await _productoService.Lista();





            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] ProductoDTO producto)
        {
            var rsp = new Response<ProductoDTO>();


            try
            {
                rsp.status = true;
                rsp.value = await _productoService.CrearProducto(producto);

            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);
        }



        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] ProductoDTO producto)
        {
            var rsp = new Response<bool>();


            try
            {
                rsp.status = true;
                rsp.value = await _productoService.Editar(producto);

            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);
        }


        [HttpDelete]
        [Route("Eliminar")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var rsp = new Response<bool>();


            try
            {
                rsp.status = true;
                rsp.value = await _productoService.Eliminar(id);

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
