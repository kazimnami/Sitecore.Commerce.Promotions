using System.Linq;
using Feature.Carts.Engine.Commands;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using Sitecore.Commerce.Core;

namespace Feature.Carts.Engine.Actions
{
    [EntityIdentifier(CartsConstants.Actions.CartItemTargetIdFreeGiftAction)]
    public class CartItemTargetIdFreeGiftAction : CartTargetItemId, ICartLineAction
    {
        public IRuleValue<bool> AutoAddToCart { get; set; }
        public IRuleValue<bool> AutoRemove { get; set; }

        protected const int QUANTITY_TO_ADD = 1;

        protected readonly IApplyFreeGiftDiscountCommand ApplyFreeGiftDiscountCommand;
        protected readonly IApplyFreeGiftEligibilityCommand ApplyFreeGiftEligibilityCommand;
        protected readonly IApplyFreeGiftAutoRemoveCommand ApplyFreeGiftAutoRemoveCommand;
        protected readonly AddCartLineCommand AddCartLineCommand;

        public CartItemTargetIdFreeGiftAction(IApplyFreeGiftDiscountCommand applyFreeGiftDiscountCommand, 
            IApplyFreeGiftEligibilityCommand applyFreeGiftEligibilityCommand,
            IApplyFreeGiftAutoRemoveCommand applyFreeGiftAutoRemoveCommand,
            AddCartLineCommand addCartLineCommand)
        {
            ApplyFreeGiftDiscountCommand = applyFreeGiftDiscountCommand;
            ApplyFreeGiftEligibilityCommand = applyFreeGiftEligibilityCommand;
            ApplyFreeGiftAutoRemoveCommand = applyFreeGiftAutoRemoveCommand;
            AddCartLineCommand = addCartLineCommand;
        }

        public async void Execute(IRuleExecutionContext context)
        {
            var commerceContext = context.Fact<CommerceContext>();
            var cart = commerceContext?.GetObject<Cart>();

            var totals = commerceContext?.GetObject<CartTotals>();
            if (cart == null || !cart.Lines.Any() || totals == null || !totals.Lines.Any())
                return;

            ApplyFreeGiftEligibilityCommand.Process(commerceContext, cart, this.GetType().Name);

            var matchingLines = this.MatchingLines(context);

            if (!matchingLines.Any() && AutoAddToCart.Yield(context))
            {
                await AddCartLineCommand.Process(commerceContext, cart, new CartLineComponent
                {
                    ItemId = this.TargetItemId.Yield(context),
                    Quantity = QUANTITY_TO_ADD
                });
            }

            foreach (var matchingLine in matchingLines)
            {
                ApplyFreeGiftDiscountCommand.Process(commerceContext, matchingLine, this.GetType().Name);
                ApplyFreeGiftAutoRemoveCommand.Process(commerceContext, matchingLine, AutoRemove.Yield(context));
            }
        }
    }
}
