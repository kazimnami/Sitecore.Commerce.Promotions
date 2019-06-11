namespace SamplePromotions.Feature.Carts.Engine
{
    public static class CartsConstants
    {
        public static class Pipelines
        {
            public static class Blocks
            {
                public const string PopulateLineItemProductExtendedBlock = "Feature.Carts:Block:" + nameof(PopulateLineItemProductExtendedBlock);
                public const string DoActionSelectBenefitBlock = "Feature.Carts:Block:" + nameof(DoActionSelectBenefitBlock);
                public const string AutoRemoveFreeGiftBlock = "Feature.Carts:Block:" + nameof(AutoRemoveFreeGiftBlock);
            }
        }

        public static class Conditions
        {
            public const string CartAnyItemHasBrandCondition = nameof(CartAnyItemHasBrandCondition);
            public const string CartAnyItemHasCategoryCondition = nameof(CartAnyItemHasCategoryCondition);
        }

            public static class Actions
        {
            public const string CartItemTargetCategorySubtotalPercentOffAction = nameof(CartItemTargetCategorySubtotalPercentOffAction);
            public const string CartItemTargetCategorySubtotalAmountOffAction = nameof(CartItemTargetCategorySubtotalAmountOffAction);
            public const string CartItemTargetBrandSubtotalPercentOffAction = nameof(CartItemTargetBrandSubtotalPercentOffAction);
            public const string CartItemTargetBrandSubtotalAmountOffAction = nameof(CartItemTargetBrandSubtotalAmountOffAction);
            public const string CartItemTargetTagSubtotalPercentOffAction = nameof(CartItemTargetTagSubtotalPercentOffAction);
            public const string CartItemTargetTagSubtotalAmountOffAction = nameof(CartItemTargetTagSubtotalAmountOffAction);

            // "Get [Target Item Id] Gift With Purchase"
            public const string CartItemTargetIdFreeGiftAction = nameof(CartItemTargetIdFreeGiftAction);

            // "Get [Target Tag] Gift With Purchase"
            public const string CartItemTargetTagFreeGiftAction = nameof(CartItemTargetTagFreeGiftAction);
        }
    }
}
