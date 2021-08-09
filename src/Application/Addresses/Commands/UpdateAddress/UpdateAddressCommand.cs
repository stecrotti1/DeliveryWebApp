﻿using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Addresses.Commands.UpdateAddress
{
    public class UpdateAddressCommand : IRequest<Address>
    {
        public int Id { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
    }

    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, Address>
    {
        private readonly IApplicationDbContext _context;

        public UpdateAddressCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Address> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Addresses.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Address), request.Id);
            }

            if (!string.IsNullOrEmpty(request.AddressLine1))
            {
                entity.AddressLine1 = request.AddressLine1;
            }

            if (!string.IsNullOrEmpty(request.AddressLine2))
            {
                entity.AddressLine2 = request.AddressLine2;
            }

            if (!string.IsNullOrEmpty(request.Number))
            {
                entity.Number = request.Number;
            }

            if (!string.IsNullOrEmpty(request.City))
            {
                entity.City = request.City;
            }

            if (!string.IsNullOrEmpty(request.PostalCode))
            {
                entity.PostalCode = request.PostalCode;
            }

            if (!string.IsNullOrEmpty(request.StateProvince))
            {
                entity.StateProvince = request.StateProvince;
            }


            if (!string.IsNullOrEmpty(request.Country))
            {
                entity.Country = request.Country;
            }

            if (request.Latitude != null)
            {
                entity.Latitude = (decimal)request.Latitude;
            }

            if (request.Longitude != null)
            {
                entity.Longitude = (decimal)request.Longitude;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}