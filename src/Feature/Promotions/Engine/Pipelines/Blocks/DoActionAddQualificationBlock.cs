using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Promotions;
using Sitecore.Commerce.Plugin.Rules;
using Sitecore.Framework.Pipelines;
using Sitecore.Framework.Rules;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SamplePromotions.Feature.Promotions.Engine.Pipelines.Blocks
{
    [PipelineDisplayName("Promotions.block.doactionaddqualification")]
    public class DoActionAddQualificationBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly AddQualificationCommand _addQualificationCommand;
        private readonly GetConditionsCommand _getConditionsCommand;

        public DoActionAddQualificationBlock(AddQualificationCommand addQualificationCommand, GetConditionsCommand getConditionsCommand)
          : base(null)
        {
            this._addQualificationCommand = addQualificationCommand;
            this._getConditionsCommand = getConditionsCommand;
        }

        public override async Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            if (string.IsNullOrEmpty(entityView?.Action)
                || !entityView.Action.Equals(context.GetPolicy<KnownPromotionsActionsPolicy>().AddQualification, StringComparison.OrdinalIgnoreCase)
                || (!entityView.Name.Equals(context.GetPolicy<KnownPromotionsViewsPolicy>().QualificationDetails, StringComparison.OrdinalIgnoreCase)
                || string.IsNullOrEmpty(entityView.EntityId)))
            {
                return entityView;
            }

            var promotion = context.CommerceContext.GetObject<Promotion>(p => p.Id.Equals(entityView.EntityId, StringComparison.OrdinalIgnoreCase));
            if (promotion == null)
            {
                return entityView;
            }

            var selectedCondition = entityView.GetProperty("Condition");
            if (string.IsNullOrEmpty(selectedCondition?.Value))
            {
                await context.CommerceContext.AddMessage(
                    context.GetPolicy<KnownResultCodes>().ValidationError,
                    "InvalidOrMissingPropertyValue",
                    new object[1] { selectedCondition?.DisplayName ?? "Condition" },
                    "Invalid or missing value for property 'Condition'.");

                return entityView;
            }

            var conditions = await this._getConditionsCommand.Process(context.CommerceContext, typeof(ICondition));
            var condition = conditions?.ToList()?.FirstOrDefault(c => c.LibraryId.Equals(selectedCondition.Value, StringComparison.OrdinalIgnoreCase));
            if (condition == null)
            {
                await context.CommerceContext.AddMessage(
                    context.GetPolicy<KnownResultCodes>().ValidationError,
                    "InvalidOrMissingPropertyValue",
                    new object[1] { selectedCondition.DisplayName },
                    "Invalid or missing value for property 'Condition'.");

                return entityView;
            }

            var conditionOperator = entityView.GetProperty("ConditionOperator");
            if (string.IsNullOrEmpty(conditionOperator?.Value) && promotion.HasPolicy<PromotionQualificationsPolicy>())
            {
                await context.CommerceContext.AddMessage(
                    context.GetPolicy<KnownResultCodes>().ValidationError,
                    "InvalidOrMissingPropertyValue",
                    new object[1] { conditionOperator?.DisplayName ?? "ConditionOperator" },
                    "Invalid or missing value for property 'ConditionOperator'.");

                return entityView;
            }
            
            condition.ConditionOperator = conditionOperator?.Value;
            var hasValidationError = false;
            foreach (var property in condition.Properties)
            {
                var viewProperty = entityView.GetProperty(property.Name);
                if (Foundation.Rules.Engine.ModelExtensions.SetPropertyValue(condition, property.Name, viewProperty?.Value))
                {
                    await context.CommerceContext.AddMessage(
                        context.GetPolicy<KnownResultCodes>().ValidationError,
                        "InvalidOrMissingPropertyValue",
                        new object[1] { viewProperty.DisplayName ?? property.Name },
                        $"Invalid or missing value for property '{property.Name}'.");
                    hasValidationError = true;

                    break;
                }
            }

            if (hasValidationError)
            {
                return entityView;
            }

            await this._addQualificationCommand.Process(context.CommerceContext, promotion, condition);

            return entityView;
        }
    }
}
