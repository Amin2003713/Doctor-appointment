using App.Applications.Users.Requests.ToggleUsers;
using FluentValidation;

namespace App.Applications.Users.Validators.ToggleUsers;


public class ToggleUserRequestValidator : AbstractValidator<ToggleUserRequest>
{
   public ToggleUserRequestValidator()
   {
      RuleFor(x => x.UserId)
         .NotEmpty()
         .WithMessage("شناسه کاربر الزامی است.")
         .Must(id => long.TryParse(id, out _))
         .WithMessage("شناسه کاربر باید عدد صحیح معتبر باشد.");
   }
}