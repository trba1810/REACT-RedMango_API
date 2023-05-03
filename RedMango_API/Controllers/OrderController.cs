using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedMango_API.Data;
using RedMango_API.Models;
using RedMango_API.Models.DTO;
using RedMango_API.Utility;
using System.Net;
using System.Text.Json;

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
        public async Task<ActionResult<ApiResponse>> GetOrders(string? userId, string searchString, string status, int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                IEnumerable<OrderHeader> orderHeaders = _context.OrderHeaders.Include(x => x.OrderDetails).ThenInclude(x => x.MenuItem).OrderByDescending(x => x.OrderHeaderId);
                if (!string.IsNullOrEmpty(userId))
                {
                    orderHeaders = orderHeaders.Where(x => x.ApplicationUserId == userId);

                }
                if (!string.IsNullOrEmpty(searchString))
                {
                    orderHeaders = orderHeaders
                        .Where(u => u.PickupPhoneNumber.ToLower().Contains(searchString.ToLower()) ||
                    u.PickupEmail.ToLower().Contains(searchString.ToLower())
                    || u.PickupName.ToLower().Contains(searchString.ToLower()));
                }
                if (!string.IsNullOrEmpty(status))
                {
                    orderHeaders = orderHeaders.Where(u => u.Status.ToLower() == status.ToLower());
                }
                Pagination pagination = new()
                {
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = orderHeaders.Count(),
                };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));

                _response.Result = orderHeaders.Skip((pageNumber - 1) * pageSize).Take(pageSize);
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

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateOrder([FromBody] OrderHeaderCreateDTO orderHeaderDTO)
        {
            try
            {
                OrderHeader order = new()
                {
                    ApplicationUserId = orderHeaderDTO.ApplicationUserId,
                    PickupEmail = orderHeaderDTO.PickupEmail,
                    PickupName = orderHeaderDTO.PickupName,
                    PickupPhoneNumber = orderHeaderDTO.PickupPhoneNumber,
                    OrderTotal = orderHeaderDTO.OrderTotal,
                    OrderDate = DateTime.Now,
                    StripePaymentIntentId = orderHeaderDTO.StripePaymentIntentId,
                    TotalItems = orderHeaderDTO.TotalItems,
                    Status = String.IsNullOrEmpty(orderHeaderDTO.Status) ? SD.status_pending : orderHeaderDTO.Status,
                };
                if (ModelState.IsValid)
                {
                    _context.OrderHeaders.Add(order);
                    _context.SaveChanges();
                    foreach (var orderDetailDTO in orderHeaderDTO.OrderDetailsDTO)
                    {
                        OrderDetails orderDetails = new()
                        {
                            OrderHeaderId = order.OrderHeaderId,
                            ItemName = orderDetailDTO.ItemName,
                            MenuItemId = orderDetailDTO.MenuItemId,
                            Price = orderDetailDTO.Price,
                            Quantity = orderDetailDTO.Quantity,
                        };
                        _context.OrderDetails.Add(orderDetails);
                    }
                    _context.SaveChanges();
                    _response.Result = order;
                    order.OrderDetails = null;
                    _response.StatusCode = HttpStatusCode.Created;
                    return Ok(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateOrderHeader(int id, [FromBody] OrderHeaderUpdateDTO orderHeaderUpdateDTO)
        {
            try
            {
                if (orderHeaderUpdateDTO == null || id != orderHeaderUpdateDTO.OrderHeaderId)
                {
                    return BadRequest();
                }
                OrderHeader orderFromDB = _context.OrderHeaders.FirstOrDefault(x => x.OrderHeaderId == id);
                if (orderFromDB == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest();
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.PickupName))
                {
                    orderFromDB.PickupName = orderHeaderUpdateDTO.PickupName;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.PickupEmail))
                {
                    orderFromDB.PickupEmail = orderHeaderUpdateDTO.PickupEmail;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.PickupPhoneNumber))
                {
                    orderFromDB.PickupPhoneNumber = orderHeaderUpdateDTO.PickupPhoneNumber;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.Status))
                {
                    orderFromDB.Status = orderHeaderUpdateDTO.Status;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdateDTO.StripePaymentIntentId))
                {
                    orderFromDB.StripePaymentIntentId = orderHeaderUpdateDTO.StripePaymentIntentId;
                }
                _context.SaveChanges();
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
