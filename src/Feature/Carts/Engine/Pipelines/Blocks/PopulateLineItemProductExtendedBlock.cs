using Microsoft.Extensions.DependencyInjection;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Feature.Carts.Engine
{
    [PipelineDisplayName(CartsConstants.Pipelines.Blocks.PopulateLineItemProductExtendedBlock)]
    public class PopulateLineItemProductExtendedBlock : PipelineBlock<CartLineComponent, CartLineComponent, CommercePipelineExecutionContext>
    {
        private CommerceCommander CommerceCommander { get; set; }

        public PopulateLineItemProductExtendedBlock(IServiceProvider serviceProvider)
        {
            CommerceCommander = serviceProvider.GetService<CommerceCommander>();
        }

        public override async Task<CartLineComponent> Run(CartLineComponent arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The CartLineComponent can not be null");

            if (arg.HasComponent<LineItemProductExtendedComponent>())
                return arg;

            var productArgument = ProductArgument.FromItemId(arg.ItemId);
            if (!productArgument.IsValid())
            {
                await RegisterLineIsNotPurchasableError(arg, context);
                return null;
            }

            var sellableItem = context.CommerceContext.GetEntity<SellableItem>(s => s.ProductId.Equals(productArgument.ProductId, StringComparison.OrdinalIgnoreCase));
            if (sellableItem == null)
            {
                await RegisterLineIsNotPurchasableError(arg, context);
                return null;
            }

            var component = arg.GetComponent<LineItemProductExtendedComponent>();
            component.Id = sellableItem.FriendlyId;
            component.Brand = sellableItem.Brand;
            component.ParentCategoryList = sellableItem.ParentCategoryList;

            return arg;
        }

        private static async Task RegisterLineIsNotPurchasableError(CartLineComponent arg, CommercePipelineExecutionContext context)
        {
            context.Abort(
                await context.CommerceContext.AddMessage(
                    context.GetPolicy<KnownResultCodes>().Error,
                    "LineIsNotPurchasable",
                    new object[1] { arg.ItemId },
                    $"Item '{arg.ItemId}' is not purchasable."),
                context);
        }
    }
}
