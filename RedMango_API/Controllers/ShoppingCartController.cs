﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedMango_API.Data;
using RedMango_API.Models;
using System.Net;

namespace RedMango_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        protected ApiResponse _response;
        private readonly ApplicationDbContext _context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            _response = new();
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddorUpdateItemInCart(string userId,int menuItemId,int updateQuantityBy)
        {
            ShoppingCart shoppingCart = _context.ShoppingCarts.Include(x=>x.CartItems).FirstOrDefault(x => x.UserId == userId);
            MenuItem menuItem = _context.MenuItemUsers.FirstOrDefault(x => x.Id == menuItemId);
            if (menuItem == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess= false;
                return BadRequest();
            }
            if(shoppingCart == null && updateQuantityBy > 0)
            {
                ShoppingCart newCart = new() { UserId = userId};
                _context.ShoppingCarts.Add(newCart);
                _context.SaveChanges();

                CartItem newCartItem = new()
                {
                    MenuItemId = menuItemId,
                    quantity = updateQuantityBy,
                    ShoppingCartId = newCart.Id,
                    MenuItem = null
                };
                _context.CartItems.Add(newCartItem);
                _context.SaveChanges();
            }
            else
            {
                CartItem cartItemInCart = shoppingCart.CartItems.FirstOrDefault(x => x.MenuItemId == menuItemId);
                if(cartItemInCart == null)
                {
                    CartItem newCartItem = new()
                    {
                        MenuItemId = menuItemId,
                        quantity = updateQuantityBy,
                        ShoppingCartId = shoppingCart.Id,
                        MenuItem = null
                    };
                    _context.CartItems.Add(newCartItem);
                    _context.SaveChanges(); 
                }
                else
                {
                    int newQuantity = cartItemInCart.quantity += updateQuantityBy;
                    if(updateQuantityBy == 0 || newQuantity <= 0)
                    {
                        _context.CartItems.Remove(cartItemInCart);
                        if(shoppingCart.CartItems.Count() == 1)
                        {
                            _context.ShoppingCarts.Remove(shoppingCart);
                        }
                        _context.SaveChanges();
                    }
                    else
                    {
                        cartItemInCart.quantity = newQuantity;
                        _context.SaveChanges();
                    }
                }
            }
            return _response;
        }
    }
}
