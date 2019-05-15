// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CouponsConstants.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2019
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Feature.Coupons.Engine
{
    /// <summary>
    /// The CouponsConstants.
    /// </summary>
    public class CouponsConstants
    {
        /// <summary>
        /// The names of the pipelines.
        /// </summary>
        public static class Pipelines
        {
            /// <summary>
            /// The names of the pipeline blocks.
            /// </summary>
            public static class Blocks
            {
				/// <summary>
				/// The filter promotions with coupons by exclusivity block name.
				/// </summary>
				public const string FilterPromotionsWithCouponsByExclusivity = "Coupons.Block.FilterPromotionsWithCouponsByExclusivity";
			}
        }
    }
}