using IdentityModel;
using Mango.Services.Identity.DBContexts;
using Mango.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mango.Services.Identity.Initializer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly ApplicationDBContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DBInitializer(ApplicationDBContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (_roleManager.FindByNameAsync(SD.Admin).Result == null)
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
            }
            else
                return;

            ApplicationUser adminUser = new ApplicationUser()
            {
                UserName = "admin@correo.com",
                Email = "admin@correo.com",
                EmailConfirmed = true,
                PhoneNumber = "8711252524",
                FirstName = "Alberto",
                LastName = "Martinez"
            };

            _userManager.CreateAsync(adminUser, "Admin123456*").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(adminUser, SD.Admin).GetAwaiter().GetResult();

            var adminClaims = _userManager.AddClaimsAsync(adminUser, new Claim[] 
            {
                new Claim(JwtClaimTypes.Name, $"{adminUser.FirstName} {adminUser.LastName}"),
                new Claim(JwtClaimTypes.GivenName, adminUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName, adminUser.LastName),
                new Claim(JwtClaimTypes.Role, SD.Admin)
            }).Result;

            ApplicationUser customerUser = new ApplicationUser()
            {
                UserName = "customer1@correo.com",
                Email = "customer1@correo.com",
                EmailConfirmed = true,
                PhoneNumber = "8711252524",
                FirstName = "Juan",
                LastName = "Diaz"
            };

            _userManager.CreateAsync(customerUser, "Admin123456*").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(customerUser, SD.Customer).GetAwaiter().GetResult();

            var customerClaims = _userManager.AddClaimsAsync(customerUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{customerUser.FirstName} {customerUser.LastName}"),
                new Claim(JwtClaimTypes.GivenName, customerUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName, customerUser.LastName),
                new Claim(JwtClaimTypes.Role, SD.Customer)
            }).Result;
        }
    }
}
