﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Common.Security;
using MediatR;

namespace DeliveryWebApp.Application.Baskets.Commands.PurgeBasket
{
    [Authorize(Roles = "Administrator")]
    [Authorize(Policy = "IsCustomer")]
    public class PurgeBasketCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class PurgeBasketCommandHandler : IRequestHandler<PurgeBasketCommand>
    {
        private IApplicationDbContext _context;

        public PurgeBasketCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(PurgeBasketCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Baskets.FindAsync(request.Id);

            entity.Products.Clear();
            entity.TotalPrice = 0.00M;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
