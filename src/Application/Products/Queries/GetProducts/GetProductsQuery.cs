﻿using System;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DeliveryWebApp.Application.Products.Queries.GetProducts
{
    public class GetProductsQuery : IRequest<List<Product>>
    {
        public int? RestaurateurId { get; set; }
        public int? OrderId { get; set; }
    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<Product>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<GetProductsQuery> _logger;

        public GetProductsQueryHandler(IApplicationDbContext context, ILogger<GetProductsQuery> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.RestaurateurId != null)
                {
                    return await (from p in _context.Products
                        where p.RestaurateurId == request.RestaurateurId
                        select p).ToListAsync(cancellationToken);
                }

                if (request.OrderId != null)
                {
                    // TODO: complete
                }

                return null;
            }
            catch (InvalidOperationException e)
            {
                _logger.LogWarning($"{nameof(Product)}, {e.Message}");
                return null;
            }
        }
    }
}
