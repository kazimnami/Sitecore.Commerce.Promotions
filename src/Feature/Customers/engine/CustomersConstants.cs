namespace SamplePromotions.Feature.Customers.Engine
{
    public static class CustomersConstants
    {
        public const string FeatureName = "Feature.Customers";

        public static class Pipelines
        {
            public static class Blocks
            {
                public const string PopulateCartContactBehaviourComponentBlockName = FeatureName + ":Block:" + nameof(PopulateCartContactBehaviourComponentBlock);
            }
        }

        public static class Conditions
        {
            public const string CurrentCustomerCampaignConditionName = nameof(CurrentCustomerCampaignCondition);
            public const string CurrentCustomerTotalVisitsConditionName = nameof(CurrentCustomerTotalVisitsCondition);
            public const string CurrentCustomerEngagementValueConditionName = nameof(CurrentCustomerEngagementValueCondition);
            public const string CurrentCustomerGoalConditionName = nameof(CurrentCustomerGoalCondition);
            public const string CurrentCustomerPageEventConditionName = nameof(CurrentCustomerPageEventCondition);
            public const string CurrentCustomerOutcomeConditionName = nameof(CurrentCustomerOutcomeCondition); 
        }
    }
}
