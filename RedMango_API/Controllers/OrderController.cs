using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedMango_API.Data;
using RedMango_API.Models;
using System.Net;

namespace RedMango_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private ApiResponse _response;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
            _response = new ApiResponse();

        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetOrders(string? userId)
        {
            try
            {
                var orderHeaders = _context.OrderHeaders.Include(x => x.OrderDetails).ThenInclude(x => x.MenuItem).OrderByDescending(x => x.OrderHeaderId);
                if (!string.IsNullOrEmpty(userId))
                {
                    _response.Result = orderHeaders.Where(x => x.ApplicationUserId == userId);

                }
                else
                {
                    _response.Result = orderHeaders;
                }
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetOrders(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var orderHeaders = _context.OrderHeaders.Include(x => x.OrderDetails).ThenInclude(x => x.MenuItem).Where(x => x.OrderHeaderId == id);
                if (orderHeaders == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);

                }
                _response.Result = orderHeaders;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
    }
}
