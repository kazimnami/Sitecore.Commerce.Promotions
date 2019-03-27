using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Pipelines;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Feature.Customers.Engine
{
    [PipelineDisplayName(CustomersConstants.Pipelines.Blocks.PopulateCartContactBehaviourComponentBlockName)]
    public class PopulateCartContactBehaviourComponentBlock : PipelineBlock<Cart, Cart, CommercePipelineExecutionContext>
    {
        public override Task<Cart> Run(Cart arg, CommercePipelineExecutionContext context)
        {
            var commerceContext = context.CommerceContext;
            if (commerceContext.Headers == null)
            {
                return Task.FromResult(arg);
            }

            var component = arg.GetComponent<CartContactBehaviourComponent>();
            if (commerceContext.Headers["TotalVisits"].Any<string>() &&
                int.TryParse(commerceContext.Headers["TotalVisits"], out int totalVisits))
            {
                component.TotalVisits = totalVisits;
            }
            if (commerceContext.Headers["EngagementValue"].Any<string>() &&
                int.TryParse(commerceContext.Headers["EngagementValue"], out int engagementValue))
            {
                component.EngagementValue = engagementValue;
            }
            if (commerceContext.Headers["CampaignId"].Any<string>())
            {
                component.AddCampaignId(commerceContext.Headers["CampaignId"].ToString().ToLower());
            }
            if (commerceContext.Headers["Goals"].Any<string>())
            {
                component.AddGoals(commerceContext.Headers["Goals"].ToString());
            }
            if (commerceContext.Headers["PageEvents"].Any<string>())
            {
                component.AddPageEvents(commerceContext.Headers["PageEvents"].ToString());
            }
            if (commerceContext.Headers["Outcomes"].Any<string>())
            {
                component.AddOutcomes(commerceContext.Headers["Outcomes"].ToString());
            }

            return Task.FromResult(arg);
        }
    }
}
