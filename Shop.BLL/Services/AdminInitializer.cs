﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Shop.DAL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BLL.Services
{
    public class AdminInitializer
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IConfiguration _configuration;
        public AdminInitializer(UserManager<User> userManamer, RoleManager<IdentityRole<Guid>> roleManager, IConfiguration configuration)
        {
            _userManager = userManamer;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task InitializeAdminAsync()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            }
            var cartId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            Cart cart = new Cart()
            {
                CartID = cartId
            };
            User user = new User()
            {
                Email = _configuration["Admin:Email"],
                UserName = _configuration["Admin:UserName"],
                EmailConfirmed = true,
                Id = userId,
                Cart = cart,
                CartID = cartId
            };

            var createAdmin_Result = await _userManager.CreateAsync(user, _configuration["Admin:Password"]);
            if (createAdmin_Result.Succeeded) await _userManager.AddToRoleAsync(user, "Admin");
        }
    }
}