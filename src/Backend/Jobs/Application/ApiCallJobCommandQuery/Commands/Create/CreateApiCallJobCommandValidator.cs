using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Application.ApiCallJobCommandQuery.Commands.Create;

public class CreateApiCallJobCommandValidator : AbstractValidator<CreateApiCallJobCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateApiCallJobCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

    }
}
