using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Application.Models.Campaign
{
    public class CampaignCreateRequestValidator : AbstractValidator<CampaignCreateRequest>
    {
        public CampaignCreateRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        }
    }
}
