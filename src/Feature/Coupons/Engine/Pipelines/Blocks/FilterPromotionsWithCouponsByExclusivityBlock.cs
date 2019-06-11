using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Coupons;
using Sitecore.Commerce.Plugin.Promotions;
using Sitecore.Framework.Pipelines;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SamplePromotions.Feature.Coupons.Engine.Pipelines.Blocks
{
    [PipelineDisplayName(CouponsConstants.Pipelines.Blocks.FilterPromotionsWithCouponsByExclusivity)]
    public class FilterPromotionsWithCouponsByExclusivityBlock : PipelineBlock<IEnumerable<Promotion>, IEnumerable<Promotion>, CommercePipelineExecutionContext>
    {
        public FilterPromotionsWithCouponsByExclusivityBlock()
          : base(null)
        {
        }
        
        public override Task<IEnumerable<Promotion>> Run(IEnumerable<Promotion> arg, CommercePipelineExecutionContext context)
        {
            if (arg == null)
            {
                return Task.FromResult(Enumerable.Empty<Promotion>());
            }

            var promotions = arg as List<Promotion> ?? arg.ToList();
            if (!promotions.Any(p => p.HasPolicy<ExclusivePromotionPolicy>()))
            {
                return Task.FromResult(promotions.AsEnumerable());
            }

            var promotionsArgument = context.CommerceContext.GetObject<EvaluatePromotionsArgument>();
            var cart = promotionsArgument?.Entity as Cart;
            if (cart == null || !cart.HasComponent<CartCouponsComponent>())
            {
                return Task.FromResult(promotions.AsEnumerable());
            }

            var qualifyingPromotions = promotions;
            var exclusivePromotions = promotions.Where(p => p.HasPolicy<ExclusivePromotionPolicy>())
                                                .OrderBy(p => p.ValidFrom)
                                                .ThenBy(p => p.DateCreated)
                                                .ToList();
            var groupedPromotions = exclusivePromotions.GroupBy(p => p.HasPolicy<CouponRequiredPolicy>()).ToList();
            var nonCouponPromotions = groupedPromotions.Where(g => !g.Key).SelectMany(g => g).ToList();
            
            if (nonCouponPromotions.Any())
            {
                qualifyingPromotions = nonCouponPromotions;
            }
            else
            {
                var couponPromotions = groupedPromotions.Where(g => g.Key).SelectMany(g => g).ToList();
                var coupons = cart.GetComponent<CartCouponsComponent>();
                var orderedCoupons = coupons?.List?
                                                .OrderBy(c => c.AddedDate)
                                                .Select(c => c.Promotion.EntityTarget)
                                                .ToList();
                if (orderedCoupons != null && orderedCoupons.Any())
                {
                    qualifyingPromotions = new List<Promotion>()
                    {
                        couponPromotions.OrderBy(p => orderedCoupons.IndexOf(p.Id)).FirstOrDefault()
                    };
                }
            }
            
            var messagesComponent = cart.GetComponent<MessagesComponent>();
            promotions.Select(p => p.Id)
                .Except(qualifyingPromotions.Select(p => p.Id))
                .ForEach(id => messagesComponent.AddMessage(
                    context.GetPolicy<KnownMessageCodePolicy>().Promotions,
                    $"PromotionExcluded: {id}"));

            return Task.FromResult(qualifyingPromotions.AsEnumerable());
        }
    }
}