using FluentValidation;
using LinkfireTechChallenge.Core.Commands;

namespace LinkfireTechChallenge.Core.Validators
{
    public class UpdatePlaylistCommandValidator : BaseValidator<UpdatePlaylistCommand>
    {

        public UpdatePlaylistCommandValidator()
        {
            RuleFor(model => model)
               .Must(model => !string.IsNullOrWhiteSpace(model.ArtistId))
               .WithMessage("ArtistId is mandatory");

            RuleFor(model => model)
               .Must(model => !string.IsNullOrWhiteSpace(model.PlaylistId))
               .WithMessage("PlaylistId is mandatory");
        }
    }
}
