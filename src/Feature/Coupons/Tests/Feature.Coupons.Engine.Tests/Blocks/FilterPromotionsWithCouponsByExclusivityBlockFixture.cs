namespace Feature.Coupons.Engine.Tests.Blocks
{
	using FluentAssertions;
	using Sitecore.Commerce.Core;
	using Sitecore.Commerce.Plugin.Carts;
	using Sitecore.Commerce.Plugin.Coupons;
	using Sitecore.Commerce.Plugin.Promotions;
	using System;
	using System.Collections.Generic;
	using Xunit;
	using FilterPromotionsWithCouponsByExclusivityBlock = Feature.Coupons.Engine.Pipelines.Blocks.FilterPromotionsWithCouponsByExclusivityBlock;
	
	public class FilterPromotionsWithCouponsByExclusivityBlockFixture
	{
		private readonly CommercePipelineExecutionContext _context;

		public FilterPromotionsWithCouponsByExclusivityBlockFixture()
		{
			this._context = TestHelper.GetPipelineExecutionContext();
		}

		[Theory, AutoNSubstituteData]
		public async void Run_NullArgument(FilterPromotionsWithCouponsByExclusivityBlock block)
		{
			/**********************************************
			 * Arrange
			 **********************************************/

			/**********************************************
             * Act
             **********************************************/
			var result = await block.Run(null, this._context);

			/**********************************************
             * Assert
             **********************************************/
			result.Should().NotBeNull();
			result.Should().BeEmpty();
			this._context.CommerceContext.AnyMessage(m => m.Code.Equals(this._context.GetPolicy<KnownResultCodes>().Warning)).Should().BeFalse();
		}

		[Theory, AutoNSubstituteData]
		public async void Run_EmptyArgument(FilterPromotionsWithCouponsByExclusivityBlock block)
		{
			/**********************************************
			 * Arrange
			 **********************************************/

			/**********************************************
             * Act
             **********************************************/
			var result = await block.Run(new List<Promotion>(), this._context);

			/**********************************************
             * Assert
             **********************************************/
			result.Should().NotBeNull();
			result.Should().BeEmpty();
			this._context.CommerceContext.AnyMessage(m => m.Code.Equals(this._context.GetPolicy<KnownResultCodes>().Warning)).Should().BeFalse();
		}

		[Theory, AutoNSubstituteData]
		public async void Run_NoCart(List<Promotion> promotions, FilterPromotionsWithCouponsByExclusivityBlock block)
		{
			/**********************************************
			 * Arrange
			 **********************************************/

			/**********************************************
             * Act
             **********************************************/
			var result = await block.Run(promotions, this._context);

			/**********************************************
             * Assert
             **********************************************/
			result.Should().NotBeNull();
			result.Should().NotBeEmpty();
			this._context.CommerceContext.AnyMessage(m => m.Code.Equals(this._context.GetPolicy<KnownResultCodes>().Warning)).Should().BeFalse();
		}

		[Theory, AutoNSubstituteData]
		public async void Run_NoCartCoupons(List<Promotion> promotions, Cart cart, FilterPromotionsWithCouponsByExclusivityBlock block)
		{
			/**********************************************
			 * Arrange
			 **********************************************/
			this._context.CommerceContext.AddUniqueObjectByType(new EvaluatePromotionsArgument(cart));
			cart.Components.Clear();

			/**********************************************
             * Act
             **********************************************/
			var result = await block.Run(promotions, this._context);

			/**********************************************
             * Assert
             **********************************************/
			result.Should().NotBeNull();
			result.Should().NotBeEmpty();
			result.Should().Contain(promotions);
			this._context.CommerceContext.AnyMessage(m => m.Code.Equals(this._context.GetPolicy<KnownResultCodes>().Warning)).Should().BeFalse();
		}

		[Theory, AutoNSubstituteData]
		public async void Run_NotExclusive(
			Cart cart,
			CartCouponsComponent coupons,
			CartCoupon coupon1,
			CartCoupon coupon2,
			Promotion promotion1,
			Promotion promotion2,
			Promotion promotion3,
			CouponRequiredPolicy policy,
			FilterPromotionsWithCouponsByExclusivityBlock block)
		{
			/**********************************************
			 * Arrange
			 **********************************************/
			this._context.CommerceContext.AddUniqueObjectByType(new EvaluatePromotionsArgument(cart));
			coupon1.AddedDate = DateTimeOffset.UtcNow;
			coupon1.Promotion = new EntityReference { EntityTarget = promotion1.Id };
			coupon2.AddedDate = DateTimeOffset.UtcNow.AddMinutes(-5);
			coupon2.Promotion = new EntityReference { EntityTarget = promotion2.Id };
			coupons.List.Clear();
			coupons.List.AddRange(new List<CartCoupon> { coupon1, coupon2 });
			cart.Components.Add(coupons);
			promotion1.Policies.Add(policy);
			promotion2.Policies.Add(policy);
			var promotions = new List<Promotion> { promotion1, promotion2, promotion3 };

			/**********************************************
             * Act
             **********************************************/
			var result = await block.Run(promotions, this._context);

			/**********************************************
             * Assert
             **********************************************/
			result.Should().NotBeNull();
			result.Should().NotBeEmpty();
			result.Should().Contain(promotions);
			this._context.CommerceContext.AnyMessage(m => m.Code.Equals(this._context.GetPolicy<KnownResultCodes>().Warning)).Should().BeFalse();
		}

		[Theory, AutoNSubstituteData]
		public async void Run_ExclusivePromotion(
			Cart cart,
			CartCouponsComponent coupons,
			CartCoupon coupon1,
			CartCoupon coupon2,
			Promotion promotion1,
			Promotion promotion2,
			Promotion promotion3,
			CouponRequiredPolicy policy,
			ExclusivePromotionPolicy exclusive,
			FilterPromotionsWithCouponsByExclusivityBlock block)
		{
			/**********************************************
			 * Arrange
			 **********************************************/
			this._context.CommerceContext.AddUniqueObjectByType(new EvaluatePromotionsArgument(cart));
			coupon1.AddedDate = DateTimeOffset.UtcNow;
			coupon1.Promotion = new EntityReference { EntityTarget = promotion1.Id };
			coupon2.AddedDate = DateTimeOffset.UtcNow.AddMinutes(-5);
			coupon2.Promotion = new EntityReference { EntityTarget = promotion2.Id };
			coupons.List.Clear();
			coupons.List.AddRange(new List<CartCoupon> { coupon1, coupon2 });
			cart.Components.Add(coupons);
			promotion1.Policies.Add(policy);
			promotion2.Policies.Add(policy);
			promotion3.Policies.Add(exclusive);
			var promotions = new List<Promotion> { promotion1, promotion2, promotion3 };

			/**********************************************
             * Act
             **********************************************/
			var result = await block.Run(promotions, this._context);

			/**********************************************
             * Assert
             **********************************************/
			result.Should().NotBeNull();
			result.Should().NotBeEmpty();
			result.Should().Contain(promotion3);
			result.Should().NotContain(p => p.Id.Equals(promotion2.Id) || p.Id.Equals(promotion1.Id));
			this._context.CommerceContext.AnyMessage(m => m.Code.Equals(this._context.GetPolicy<KnownResultCodes>().Warning)).Should().BeFalse();
		}

		[Theory, AutoNSubstituteData]
		public async void Run_ExclusiveCouponPromotion(
			Cart cart,
			CartCouponsComponent coupons,
			CartCoupon coupon1,
			CartCoupon coupon2,
			Promotion promotion1,
			Promotion promotion2,
			Promotion promotion3,
			CouponRequiredPolicy policy,
			ExclusivePromotionPolicy exclusive,
			FilterPromotionsWithCouponsByExclusivityBlock block)
		{
			/**********************************************
			 * Arrange
			 **********************************************/
			this._context.CommerceContext.AddUniqueObjectByType(new EvaluatePromotionsArgument(cart));
			coupon1.AddedDate = DateTimeOffset.UtcNow;
			coupon1.Promotion = new EntityReference { EntityTarget = promotion1.Id };
			coupon2.AddedDate = DateTimeOffset.UtcNow.AddMinutes(-5);
			coupon2.Promotion = new EntityReference { EntityTarget = promotion2.Id };
			coupons.List.Clear();
			coupons.List.AddRange(new List<CartCoupon> { coupon1, coupon2 });
			cart.Components.Add(coupons);
			promotion1.Policies.Add(policy);
			promotion2.Policies.Add(policy);
			promotion1.Policies.Add(exclusive);
			promotion2.Policies.Add(exclusive);
			var promotions = new List<Promotion> { promotion1, promotion2, promotion3 };

			/**********************************************
             * Act
             **********************************************/
			var result = await block.Run(promotions, this._context);

			/**********************************************
             * Assert
             **********************************************/
			result.Should().NotBeNull();
			result.Should().NotBeEmpty();
			result.Should().Contain(promotion2);
			result.Should().NotContain(p => p.Id.Equals(promotion1.Id) || p.Id.Equals(promotion3.Id));
			this._context.CommerceContext.AnyMessage(m => m.Code.Equals(this._context.GetPolicy<KnownResultCodes>().Warning)).Should().BeFalse();
		}

		[Theory, AutoNSubstituteData]
		public async void Run_ExclusiveCouponAndAutomaticPromotion(
			Cart cart,
			CartCouponsComponent coupons,
			CartCoupon coupon1,
			CartCoupon coupon2,
			Promotion promotion1,
			Promotion promotion2,
			Promotion promotion3,
			Promotion promotion4,
			CouponRequiredPolicy policy,
			ExclusivePromotionPolicy exclusive,
			FilterPromotionsWithCouponsByExclusivityBlock block)
		{
			/**********************************************
			 * Arrange
			 **********************************************/
			this._context.CommerceContext.AddUniqueObjectByType(new EvaluatePromotionsArgument(cart));
			coupon1.AddedDate = DateTimeOffset.UtcNow;
			coupon1.Promotion = new EntityReference { EntityTarget = promotion1.Id };
			coupons.List.Clear();
			coupons.List.AddRange(new List<CartCoupon> { coupon1, coupon2 });
			cart.Components.Add(coupons);
			promotion1.ValidFrom = DateTimeOffset.UtcNow.AddMinutes(-5);
			promotion1.Policies.Add(policy);
			promotion1.Policies.Add(exclusive);
			promotion2.ValidFrom = DateTimeOffset.UtcNow.AddMinutes(-1);
			promotion2.Policies.Add(exclusive);
			promotion3.ValidFrom = DateTimeOffset.UtcNow;
			promotion3.Policies.Add(exclusive);
			var promotions = new List<Promotion> { promotion1, promotion2, promotion3, promotion4 };

			/**********************************************
             * Act
             **********************************************/
			var result = await block.Run(promotions, this._context);

			/**********************************************
             * Assert
             **********************************************/
			result.Should().NotBeNull();
			result.Should().NotBeEmpty();
			var expectedPromotions = new List<Promotion> { promotion2, promotion3 };
			result.Should().Contain(expectedPromotions);
			var unexpectedPromotions = new List<Promotion> { promotion1, promotion4 };
			result.Should().NotContain(unexpectedPromotions);
			this._context.CommerceContext.AnyMessage(m => m.Code.Equals(this._context.GetPolicy<KnownResultCodes>().Warning)).Should().BeFalse();
		}
	}
}