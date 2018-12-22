namespace Feature.Carts.Engine
{
    public static class Constants
    {
        public static class Pipelines
        {
            public static class Blocks
            {
                public const string PopulateLineItemProductExtendedBlock = "Feature.Carts:Block:" + nameof(PopulateLineItemProductExtendedBlock);
                public const string DoActionSelectBenefitBlock = "Feature.Carts:Block:" + nameof(DoActionSelectBenefitBlock);
            }
        }

        public const string CartItemTargetCategorySubtotalPercentOffAction = nameof(CartItemTargetCategorySubtotalPercentOffAction);
        public const string CartItemTargetCategorySubtotalAmountOffAction = nameof(CartItemTargetCategorySubtotalAmountOffAction);
        public const string CartItemTargetBrandSubtotalPercentOffAction = nameof(CartItemTargetBrandSubtotalPercentOffAction);
        public const string CartItemTargetBrandSubtotalAmountOffAction = nameof(CartItemTargetBrandSubtotalAmountOffAction);
        public const string CartItemTargetTagSubtotalPercentOffAction = nameof(CartItemTargetTagSubtotalPercentOffAction);
        public const string CartItemTargetTagSubtotalAmountOffAction = nameof(CartItemTargetTagSubtotalAmountOffAction);
    }
}
