using Feature.Carts.Engine.Commands;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using System.Linq;

namespace Feature.Carts.Engine.Actions
{
    [EntityIdentifier(CartsConstants.Actions.CartItemTargetTagFreeGiftAction)]
    public class CartItemTargetTagFreeGiftAction : ICartLineAction
    {
        public IRuleValue<string> TargetTag { get; set; }
        public IRuleValue<bool> AutoRemove { get; set; }

        private readonly CommerceCommander Commander;

        public CartItemTargetTagFreeGiftAction(CommerceCommander commerceCommander)
        {
            Commander = commerceCommander;
        }

        public void Execute(IRuleExecutionContext context)
        {
            var commerceContext = context.Fact<CommerceContext>();
            var cart = commerceContext?.GetObject<Cart>();

            var totals = commerceContext?.GetObject<CartTotals>();
            if (cart == null || !cart.Lines.Any() || totals == null || !totals.Lines.Any())
                return;

            Commander.Command<ApplyFreeGiftEligibilityCommand>().Process(commerceContext, cart, GetType().Name);

            var matchingLines = TargetTag.YieldCartLinesWithTag(context);

            foreach (var matchingLine in matchingLines)
            {
                Commander.Command<ApplyFreeGiftDiscountCommand>().Process(commerceContext, matchingLine, GetType().Name);
                Commander.Command<ApplyFreeGiftAutoRemoveCommand>().Process(commerceContext, matchingLine, AutoRemove.Yield(context));
            }
        }
    }
}
