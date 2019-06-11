namespace SamplePromotions.Feature.Carts.Engine.Commands
{
    using Components;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Core.Commands;
    using Sitecore.Commerce.Plugin.Carts;

    public class ApplyFreeGiftAutoRemoveCommand : CommerceCommand
    {
        public virtual void Process(CommerceContext commerceContext, CartLineComponent cartLineComponent, bool autoRemove)
        {

            if (!autoRemove && cartLineComponent.UnitListPrice.Amount != 0.0m)
            {
                return;
            }

            var propertiesModel = commerceContext.GetObject<PropertiesModel>();
            var freeGiftEligibilityComponent = cartLineComponent.GetComponent<FreeGiftAutoRemoveComponent>();

            freeGiftEligibilityComponent.PromotionId = propertiesModel?.GetPropertyValue("PromotionId") as string;
        }
    }
}