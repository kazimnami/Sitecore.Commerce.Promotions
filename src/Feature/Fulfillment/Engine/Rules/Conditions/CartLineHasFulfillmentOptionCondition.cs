using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Fulfillment;
using Sitecore.Framework.Rules;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Feature.Fulfillment.Engine.Rules.Conditions
{
    [EntityIdentifier(nameof(CartLineHasFulfillmentOptionCondition))]
    public class CartLineHasFulfillmentOptionCondition : IFulfillmentCondition
    {
        protected CommerceCommander Commander { get; set; }

        public CartLineHasFulfillmentOptionCondition(CommerceCommander commander)
        {
            this.Commander = commander;
        }

        public IRuleValue<string> FulfillmentOptionName { get; set; }

        public bool Evaluate(IRuleExecutionContext context)
        {
            var commerceContext = context.Fact<CommerceContext>(null);
            var cart = commerceContext?.GetObject<Cart>();

            var optionName = FulfillmentOptionName.Yield(context);
            if (cart == null || !cart.Lines.Any() || !cart.HasComponent<SplitFulfillmentComponent>() || string.IsNullOrEmpty(optionName))
            {
                return false;
            }

            var methods = Task.Run(() => Commander.Command<GetFulfillmentMethodsCommand>().Process(commerceContext)).Result
                .Where(o => o.FulfillmentType.Equals(optionName, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!methods.Any())
            {
                return false;
            }

            var lineHasMethod = false;
            foreach (var cartLineComponent in cart.Lines.Where(l => l.HasComponent<FulfillmentComponent>()))
            {
                var fulfillment = cartLineComponent.GetComponent<FulfillmentComponent>();
                if (!string.IsNullOrEmpty(fulfillment.FulfillmentMethod?.EntityTarget)
                    && !string.IsNullOrEmpty(fulfillment.FulfillmentMethod?.Name))
                {
                    lineHasMethod = methods.Any(m =>
                    {
                        if (m.Id.Equals(fulfillment.FulfillmentMethod.EntityTarget, StringComparison.OrdinalIgnoreCase))
                        {
                            return m.Name.Equals(fulfillment.FulfillmentMethod.Name, StringComparison.OrdinalIgnoreCase);
                        }

                        return false;
                    });
                    if (lineHasMethod)
                    {
                        break;
                    }
                }
            }

            return lineHasMethod;
        }
    }
}