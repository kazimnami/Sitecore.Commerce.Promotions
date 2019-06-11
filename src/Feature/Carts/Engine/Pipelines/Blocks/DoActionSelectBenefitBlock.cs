using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Promotions;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SamplePromotions.Feature.Carts.Engine
{
    [PipelineDisplayName(CartsConstants.Pipelines.Blocks.DoActionSelectBenefitBlock)]
    public class DoActionSelectBenefitBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commander;

        public DoActionSelectBenefitBlock(CommerceCommander commander)
        {
            _commander = commander;
        }

        public override Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            Condition.Requires(entityView).IsNotNull($"{Name}: The argument cannot be null.");

            if (string.IsNullOrEmpty(entityView?.Action) || !entityView.Action.Equals(context.GetPolicy<KnownPromotionsActionsPolicy>().SelectBenefit, StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(entityView);

            Promotion promotion = context.CommerceContext.GetObjects<Promotion>().FirstOrDefault(p => p.Id.Equals(entityView.EntityId, StringComparison.OrdinalIgnoreCase));
            if (promotion == null)
                return Task.FromResult(entityView);

            ViewProperty viewProperty = entityView.Properties.FirstOrDefault(p => p.Name.Equals("Action", StringComparison.OrdinalIgnoreCase));
            if (viewProperty == null)
                return Task.FromResult(entityView);

            if (!(viewProperty.Value.Equals(nameof(CartItemTargetBrandSubtotalAmountOffAction))
                || viewProperty.Value.Equals(nameof(CartItemTargetBrandSubtotalPercentOffAction))
                || viewProperty.Value.Equals(nameof(CartItemTargetCategorySubtotalAmountOffAction))
                || viewProperty.Value.Equals(nameof(CartItemTargetCategorySubtotalPercentOffAction))
                || viewProperty.Value.Equals(nameof(CartItemTargetTagSubtotalAmountOffAction))
                || viewProperty.Value.Equals(nameof(CartItemTargetTagSubtotalPercentOffAction))))
            {
                return Task.FromResult(entityView);
            }

            var propertiesFromBaseClass = (new List<ViewProperty>()
            {
                entityView.Properties.FirstOrDefault(p => p.Name.Equals("Subtotal", StringComparison.OrdinalIgnoreCase)),
                entityView.Properties.FirstOrDefault(p => p.Name.Equals("SubtotalOperator", StringComparison.OrdinalIgnoreCase)),
                entityView.Properties.FirstOrDefault(p => p.Name.Equals("AmountOff", StringComparison.OrdinalIgnoreCase)),
                entityView.Properties.FirstOrDefault(p => p.Name.Equals("PercentOff", StringComparison.OrdinalIgnoreCase))
            }).Where(p => p != null).ToList();

            // Of the two base classes that exist ($Off, %Off) we have 3 feilds that need to be moved

            if (!propertiesFromBaseClass.Count.Equals(3))
                return Task.FromResult(entityView);

            entityView.Properties = entityView.Properties.Except(propertiesFromBaseClass).ToList();
            entityView.Properties.AddRange(propertiesFromBaseClass);

            return Task.FromResult(entityView);
        }
    }
}
