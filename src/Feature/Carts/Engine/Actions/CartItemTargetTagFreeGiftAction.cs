using System.Linq;
using Feature.Carts.Engine.Commands;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using Sitecore.Commerce.Core;

namespace Feature.Carts.Engine.Actions
{
    [EntityIdentifier(CartsConstants.Actions.CartItemTargetTagFreeGiftAction)]
    public class CartItemTargetTagFreeGiftAction : ICartLineAction
    {
        public IRuleValue<string> TargetTag { get; set; }
        public IRuleValue<bool> AutoRemove { get; set; }

        protected readonly IApplyFreeGiftDiscountCommand ApplyFreeGiftDiscountCommand;
        protected readonly IApplyFreeGiftEligibilityCommand ApplyFreeGiftEligibilityCommand;
        protected readonly IApplyFreeGiftAutoRemoveCommand ApplyFreeGiftAutoRemoveCommand;

        public CartItemTargetTagFreeGiftAction(IApplyFreeGiftDiscountCommand applyFreeGiftDiscountCommand, 
            IApplyFreeGiftEligibilityCommand applyFreeGiftEligibilityCommand,
            IApplyFreeGiftAutoRemoveCommand applyFreeGiftAutoRemoveCommand)
        {
            ApplyFreeGiftDiscountCommand = applyFreeGiftDiscountCommand;
            ApplyFreeGiftEligibilityCommand = applyFreeGiftEligibilityCommand;
            ApplyFreeGiftAutoRemoveCommand = applyFreeGiftAutoRemoveCommand;
        }

        public void Execute(IRuleExecutionContext context)
        {
            var commerceContext = context.Fact<CommerceContext>();
            var cart = commerceContext?.GetObject<Cart>();

            var totals = commerceContext?.GetObject<CartTotals>();
            if (cart == null || !cart.Lines.Any() || totals == null || !totals.Lines.Any())
                return;

            ApplyFreeGiftEligibilityCommand.Process(commerceContext, cart, this.GetType().Name);

            var matchingLines = TargetTag.YieldCartLinesWithTag(context);

            foreach (var matchingLine in matchingLines)
            {
                ApplyFreeGiftDiscountCommand.Process(commerceContext, matchingLine, this.GetType().Name);
                ApplyFreeGiftAutoRemoveCommand.Process(commerceContext, matchingLine, AutoRemove.Yield(context));
            }
        }
    }
}
