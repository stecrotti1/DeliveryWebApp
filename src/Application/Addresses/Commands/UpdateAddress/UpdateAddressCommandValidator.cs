﻿using FluentValidation;

namespace DeliveryWebApp.Application.Addresses.Commands.UpdateAddress
{
    public class UpdateAddressCommandValidator : AbstractValidator<UpdateAddressCommand>
    {
        public UpdateAddressCommandValidator()
        {
            RuleFor(a => a.Id).GreaterThan(0).NotEmpty();

            RuleFor(a => a.AddressLine1).MaximumLength(80);

            RuleFor(a => a.AddressLine2).MaximumLength(60);

            RuleFor(a => a.Number).MaximumLength(10);

            RuleFor(a => a.City).MaximumLength(15);

            RuleFor(a => a.Country).MaximumLength(40);

            RuleFor(a => a.StateProvince).MaximumLength(15);

            RuleFor(a => a.PostalCode).MaximumLength(10);

            RuleFor(a => a.Latitude).NotEmpty();

            RuleFor(a => a.Longitude).NotEmpty();
        }
    }
}