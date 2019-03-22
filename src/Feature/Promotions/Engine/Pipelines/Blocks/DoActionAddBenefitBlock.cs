using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Promotions;
using Sitecore.Commerce.Plugin.Rules;
using Sitecore.Framework.Pipelines;
using Sitecore.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feature.Promotions.Engine.Pipelines.Blocks
{
    [PipelineDisplayName("Promotions.block.doactionaddbenefit")]
    public class DoActionAddBenefitBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly AddBenefitCommand _addBenefitCommand;
        private readonly GetActionsCommand _getActionsCommand;

        public DoActionAddBenefitBlock(AddBenefitCommand addBenefitCommand, GetActionsCommand getActionsCommand)
          : base((string)null)
        {
            this._addBenefitCommand = addBenefitCommand;
            this._getActionsCommand = getActionsCommand;
        }

        public override async Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            if (string.IsNullOrEmpty(entityView?.Action)
                || !entityView.Action.Equals(context.GetPolicy<KnownPromotionsActionsPolicy>().AddBenefit, StringComparison.OrdinalIgnoreCase)
                || (!entityView.Name.Equals(context.GetPolicy<KnownPromotionsViewsPolicy>().BenefitDetails, StringComparison.OrdinalIgnoreCase)
                || string.IsNullOrEmpty(entityView.EntityId)))
            {
                return entityView;
            }

            var promotion = context.CommerceContext.GetObject<Promotion>(p => p.Id.Equals(entityView.EntityId, StringComparison.OrdinalIgnoreCase));
            if (promotion == null)
            {
                return entityView;
            }

            var selectedAction = entityView.Properties.FirstOrDefault(p => p.Name.Equals("Action", StringComparison.OrdinalIgnoreCase));
            if (string.IsNullOrEmpty(selectedAction?.Value))
            {
                await context.CommerceContext.AddMessage(
                    context.GetPolicy<KnownResultCodes>().ValidationError,
                    "InvalidOrMissingPropertyValue",
                    new object[1] { selectedAction?.DisplayName ?? null },
                    "Invalid or missing value for property 'Action'.");

                return entityView;
            }

            var benefits = await this._getActionsCommand.Process(context.CommerceContext, typeof(IAction));
            var benefit = benefits?.ToList().FirstOrDefault(c => c.LibraryId.Equals(selectedAction.Value, StringComparison.OrdinalIgnoreCase));
            if (benefit == null)
            {
                await context.CommerceContext.AddMessage(
                    context.GetPolicy<KnownResultCodes>().ValidationError,
                    "InvalidOrMissingPropertyValue",
                    new object[1] { selectedAction.DisplayName },
                    "Invalid or missing value for property 'Action'.");

                return entityView;
            }

            var hasValidationError = false;
            foreach (var property in benefit.Properties)
            {
                var propertyValue = entityView.Properties.FirstOrDefault(evp => evp.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase))?.Value;
                if (Foundation.Rules.Engine.ModelExtensions.SetPropertyValue(benefit, property.Name, propertyValue))
                {
                    await context.CommerceContext.AddMessage(
                        context.GetPolicy<KnownResultCodes>().ValidationError,
                        "InvalidOrMissingPropertyValue",
                        new object[1] { property.Name },
                        $"Invalid or missing value for property '{property.Name}'.");
                    hasValidationError = true;

                    break;
                }
            }

            if (hasValidationError)
            {
                return entityView;
            }

            await this._addBenefitCommand.Process(context.CommerceContext, promotion, benefit);

            return entityView;
        }
    }
}
