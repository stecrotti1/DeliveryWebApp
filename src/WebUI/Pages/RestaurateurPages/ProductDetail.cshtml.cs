using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Products.Commands.UpdateProducts;
using DeliveryWebApp.Application.Restaurateurs.Extensions;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.RestaurateurPages
{
    [Authorize(Policy = PolicyName.IsRestaurateur)]
    public class ProductDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductDetailModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Product Product { get; set; }
        public Restaurateur Restaurateur { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound("Unable to load product with this ID.");
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            await LoadAsync(id, user);

            return Page();
        }

        private async Task LoadAsync(int? id, ApplicationUser user)
        {
            Product = await _context.Products.FindAsync(id);
            Restaurateur = await _context.GetRestaurateurByApplicationUserFkAsync(user.Id);
        }
    }
}