using Sitecore.Commerce.Core;

namespace Feature.Carts.Engine.Components
{
    public class FreeGiftEligibilityComponent : Component
    {
        public FreeGiftEligibilityComponent()
        {
            AwardingAction = string.Empty;
            PromotionId = string.Empty;
        }

        public string AwardingAction { get; set; }

        public string PromotionId { get; set; }
    }
}

