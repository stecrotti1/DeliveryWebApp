using DeliveryWebApp.Application.Customers.Extensions;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Application.Riders.Commands.CreateRider;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using DeliveryWebApp.Infrastructure.Services.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.Admin
{
    [Authorize(Roles = RoleName.Admin)]
    public class RequestDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RequestDetailModel> _logger;
        private readonly IMediator _mediator;

        public RequestDetailModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<RequestDetailModel> logger, IMediator mediator)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _mediator = mediator;
        }

        public Request UserRequest { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ClaimValue { get; set; }
        public bool IsRider { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Currency)]
            [Display(Name = "Delivery Credit")]
            public double DeliveryCredit { get; set; } // TODO: Culture
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // get client by request id
            var client = await _context.GetCustomerByRequestIdAsync(id);

            // get request instance
            UserRequest = await (from r in _context.Requests
                                 where r.Id == id
                                 select r).FirstOrDefaultAsync();

            var appUserFk = client.ApplicationUserFk;

            ApplicationUser = await _userManager.FindByIdAsync(appUserFk);
            ClaimValue = await _userManager.GetRoleAsync(ApplicationUser);

            IsRider = UserRequest.Role.Equals(RoleName.Rider);

            if (UserRequest == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAcceptAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var customer = await _context.GetCustomerByRequestIdAsync(id);
            var appUserFk = customer.ApplicationUserFk;

            ApplicationUser = await _userManager.FindByIdAsync(appUserFk);

            // TODO MediatR

            var oldClaim = await _userManager.GetClaimByTypeAsync(ApplicationUser, ClaimName.Role);

            UserRequest = await (from r in _context.Requests
                                 where r.Id == id
                                 select r).FirstOrDefaultAsync();

            IsRider = UserRequest.Role.Equals(RoleName.Rider);

            // update tables
            if (IsRider)
            {
                var entityId = await _mediator.Send(new CreateRiderCommand()
                {
                    Customer = customer,
                    DeliveryCredit = Input.DeliveryCredit
                });

                _logger.LogInformation($"Created new rider with id: {entityId}");

                // change claim
                await _userManager.ReplaceClaimAsync(ApplicationUser, oldClaim,
                    new Claim(ClaimName.Role, RoleName.Rider));
            }
            else
            {
                var entityId = await _mediator.Send(new CreateRestaurateurCommand()
                {
                    Customer = customer,

                });

                _logger.LogInformation($"Created resturateur with id: {entityId}");

                await _userManager.ReplaceClaimAsync(ApplicationUser, oldClaim,
                    new Claim(ClaimName.Role, RoleName.Restaurateur));
            }

            UserRequest.Status = RequestStatus.Accepted;

            // TODO MediatR -> update request table
            _context.Requests.Update(UserRequest);

            // TODO push notification to client

            return RedirectToPage("/Admin/Requests");
        }

        public async Task<IActionResult> OnPostRejectAsync(int? id)
        {
            UserRequest.Status = RequestStatus.Rejected;
            Input.DeliveryCredit = 0.00;

            // update request table
            _context.Requests.Update(UserRequest);

            await _context.SaveChangesAsync();

            // TODO push notification to client
            return RedirectToPage("/Admin/Requests");
        }
    }
}
