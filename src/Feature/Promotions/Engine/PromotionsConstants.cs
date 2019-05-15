namespace Feature.Promotions.Engine
{
    public static class PromotionsConstants
    {
        public static class Conditions
        {
            public const string ItemsCollectionCondition = nameof(ItemsCollectionCondition);
        }

        public static class Actions
        {
            public const string CartItemTargetItemsCollectionSubtotalPercentOffAction = nameof(CartItemTargetItemsCollectionSubtotalPercentOffAction);
		}

		public static class Pipelines
		{
			public static class Blocks
			{
				public const string EditPromotion = "Promotions.Block.EditPromotion";
				public const string GetPromotionDetailsView = "Promotions.Block.GetPromotionDetailsView";
				public const string PopulatePromotionViewActions = "Promotions.Block.PopulatePromotionViewActions";
			}
		}
	}
}
