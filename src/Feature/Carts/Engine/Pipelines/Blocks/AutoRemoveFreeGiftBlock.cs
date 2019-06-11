using Sitecore.Commerce.Core;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System.Linq;
using System.Threading.Tasks;

namespace SamplePromotions.Feature.Carts.Engine
{
    using Components;
    using Sitecore.Commerce.Plugin.Carts;

    [PipelineDisplayName(CartsConstants.Pipelines.Blocks.DoActionSelectBenefitBlock)]
    public class AutoRemoveFreeGiftBlock : PipelineBlock<Cart, Cart, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commander;

        public AutoRemoveFreeGiftBlock(CommerceCommander commander)
        {
            _commander = commander;
        }

        public override async Task<Cart> Run(Cart cart, CommercePipelineExecutionContext context)
        {
            Condition.Requires(cart).IsNotNull($"{Name}: The cart cannot be null.");

            foreach (var cartLine in cart.Lines)
            {
                if (cartLine.HasComponent<FreeGiftAutoRemoveComponent>())
                {
                    var freeGiftAutoRemoveComponent = cartLine.GetComponent<FreeGiftAutoRemoveComponent>();
                    var adjustments = cartLine.Adjustments.Where(x =>
                        x.AwardingBlock.Equals(freeGiftAutoRemoveComponent.PromotionId));
                    if (adjustments.Any())
                    {
                        await _commander.Pipeline<IRemoveCartLinePipeline>().Run(new CartLineArgument(cart, cartLine), context);
                    }
                }
            }

            return cart;
        }
    }
}
