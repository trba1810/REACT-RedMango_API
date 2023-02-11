using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using RedMango_API.Data;
using RedMango_API.Models;
using System.Net;

namespace RedMango_API.Controllers
{
    [Route("api/MenuItem")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private ApiResponse _response;

        public MenuItemController(ApplicationDbContext context)
        {
            _context = context;
            _response = new ApiResponse(); 
        }

        [HttpGet]
        public async Task<IActionResult> GetMenuItems()
        {
            _response.Result = _context.MenuItemUsers;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}
