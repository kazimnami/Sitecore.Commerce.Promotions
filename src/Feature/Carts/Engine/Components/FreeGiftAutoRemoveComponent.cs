using Sitecore.Commerce.Core;

namespace Feature.Carts.Engine.Components
{
    public class FreeGiftAutoRemoveComponent : Component
    {
        public FreeGiftAutoRemoveComponent()
        {
            AwardingAction = string.Empty;
            PromotionId = string.Empty;
        }

        public string AwardingAction { get; set; }

        public string PromotionId { get; set; }
    }
}

