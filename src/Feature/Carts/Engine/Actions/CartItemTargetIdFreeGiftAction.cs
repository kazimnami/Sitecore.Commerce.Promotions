using Feature.Carts.Engine.Commands;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using System.Linq;

namespace Feature.Carts.Engine.Actions
{
    [EntityIdentifier(CartsConstants.Actions.CartItemTargetIdFreeGiftAction)]
    public class CartItemTargetIdFreeGiftAction : CartTargetItemId, ICartLineAction
    {
        public IRuleValue<bool> AutoAddToCart { get; set; }
        public IRuleValue<bool> AutoRemove { get; set; }

        protected const int QUANTITY_TO_ADD = 1;

        private readonly CommerceCommander Commander;

        public CartItemTargetIdFreeGiftAction(CommerceCommander commerceCommander)
        {
            Commander = commerceCommander;
        }

        public async void Execute(IRuleExecutionContext context)
        {
            var commerceContext = context.Fact<CommerceContext>();
            var cart = commerceContext?.GetObject<Cart>();

            var totals = commerceContext?.GetObject<CartTotals>();
            if (cart == null || !cart.Lines.Any() || totals == null || !totals.Lines.Any())
                return;

            Commander.Command<ApplyFreeGiftEligibilityCommand>().Process(commerceContext, cart, GetType().Name);

            var matchingLines = MatchingLines(context);

            if (!matchingLines.Any() && AutoAddToCart.Yield(context))
            {
                await Commander.Command<AddCartLineCommand>().Process(commerceContext, cart, new CartLineComponent
                {
                    ItemId = TargetItemId.Yield(context),
                    Quantity = QUANTITY_TO_ADD
                });
            }

            foreach (var matchingLine in matchingLines)
            {
                Commander.Command<ApplyFreeGiftDiscountCommand>().Process(commerceContext, matchingLine, GetType().Name);
                Commander.Command<ApplyFreeGiftAutoRemoveCommand>().Process(commerceContext, matchingLine, AutoRemove.Yield(context));
            }
        }
    }
}
