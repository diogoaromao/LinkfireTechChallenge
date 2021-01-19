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

            RuleFor(model => model)
                .Must(model => model.Count > 0)
                .WithMessage("The number of musics to add to the playlist must be bigger than 0");

            RuleFor(model => model)
                .Must(model => model.Count < 101)
                .WithMessage("A maximum of 100 items can be added to the playlist in one request");
        }
    }
}
