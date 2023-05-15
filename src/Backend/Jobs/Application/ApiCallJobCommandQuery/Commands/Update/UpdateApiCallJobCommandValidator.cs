using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Application.ApiCallJobCommandQuery.Commands.Update;

public class UpdateApiCallJobCommandValidator : AbstractValidator<UpdateApiCallJobCommand>
{
    public UpdateApiCallJobCommandValidator()
    {

        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

    }
}
