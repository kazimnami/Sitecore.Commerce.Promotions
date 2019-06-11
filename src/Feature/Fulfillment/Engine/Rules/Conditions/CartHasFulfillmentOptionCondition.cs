using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Fulfillment;
using Sitecore.Framework.Rules;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SamplePromotions.Feature.Fulfillment.Engine.Rules.Conditions
{
    [EntityIdentifier(nameof(CartHasFulfillmentOptionCondition))]
    public class CartHasFulfillmentOptionCondition : IFulfillmentCondition
    {
        protected CommerceCommander Commander { get; set; }

        public CartHasFulfillmentOptionCondition(CommerceCommander commander)
        {
            this.Commander = commander;
        }

        public IRuleValue<string> FulfillmentOptionName { get; set; }

        public bool Evaluate(IRuleExecutionContext context)
        {
            var commerceContext = context.Fact<CommerceContext>(null);
            var cart = commerceContext?.GetObject<Cart>();

            var optionName = FulfillmentOptionName.Yield(context);
            if (cart == null || !cart.Lines.Any() || (!cart.HasComponent<FulfillmentComponent>() || string.IsNullOrEmpty(optionName)))
            {
                return false;
            }

            var fulfillment = cart.GetComponent<FulfillmentComponent>();
            if (string.IsNullOrEmpty(fulfillment.FulfillmentMethod?.EntityTarget) || string.IsNullOrEmpty(fulfillment.FulfillmentMethod?.Name))
            {
                return false;
            }

            var methods = Task.Run(() => Commander.Command< GetFulfillmentMethodsCommand>().Process(commerceContext)).Result;
            var optionMethods = methods.Where(o => o.FulfillmentType.Equals(optionName, StringComparison.OrdinalIgnoreCase)).ToList();
            var hasMethod = optionMethods.Any(m =>
            {
                if (m.Id.Equals(fulfillment.FulfillmentMethod.EntityTarget, StringComparison.OrdinalIgnoreCase))
                {
                    return m.Name.Equals(fulfillment.FulfillmentMethod.Name, StringComparison.OrdinalIgnoreCase);
                }

                return false;
            });

            return hasMethod;
        }
    }
}
