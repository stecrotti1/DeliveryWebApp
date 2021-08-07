﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Products.Commands.UpdateProducts
{
    public class UpdateProductCommand : IRequest<Product>
    {
        public Product Product { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            //var entity = await _context.Products.FindAsync(request.Id);

            //if (entity == null)
            //{
            //    throw new NotFoundException(nameof(Product), request.Id);
            //}

            //if (request.Discount != null)
            //{
            //    entity.Discount = (int) request.Discount;
            //}

            //if (request.Quantity != null)
            //{
            //    entity.Quantity = (int) request.Quantity;
            //}

            //if (request.Price != null)
            //{
            //    entity.Price = (decimal) request.Price;
            //}

            //if (!string.IsNullOrEmpty(request.Category))
            //{
            //    entity.Category = request.Category;
            //}

            //if (!string.IsNullOrEmpty(request.Name))
            //{
            //    entity.Name = request.Name;
            //}

            //if (request.Image != null)
            //{
            //    entity.Image = request.Image;
            //}

            var entity = _mapper.Map<Product>(request);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
