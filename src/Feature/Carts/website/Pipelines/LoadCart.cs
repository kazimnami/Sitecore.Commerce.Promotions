using Sitecore.Analytics;
using Sitecore.Analytics.Model;
using Sitecore.Commerce.Engine;
using System;
using System.Linq;

namespace SamplePromotions.Feature.Carts.Pipelines
{
    public class LoadCart : Sitecore.Commerce.Engine.Connect.Pipelines.Carts.LoadCart
    {
        protected override Container GetContainer(string shopName, string userId, string customerId = "", string language = "", string currency = "", DateTime? effectiveDate = null)
        {
            var container = base.GetContainer(shopName, userId, customerId, language, currency, effectiveDate);
            container.BuildingRequest += ((s, e) =>
            {
                if (Tracker.IsActive)
                {
                    if (Tracker.Current.Interaction.CampaignId.HasValue)
                    {
                        var campaignId = Tracker.Current.Interaction.CampaignId.Value;
                        e.Headers.Add("CampaignId", campaignId.ToString());
                    }

                    var totalVisits = Tracker.Current.Contact?.System?.VisitCount ?? 1;
                    e.Headers.Add("TotalVisits", totalVisits.ToString());

                    var engagementValue = Tracker.Current.Contact?.System?.Value ?? 0;
                    e.Headers.Add("EngagementValue", engagementValue.ToString());

                    //foreach (var profileName in Tracker.Current.Interaction?.Profiles?.GetProfileNames())
                    //{
                    //    var userPattern = Tracker.Current.Interaction.Profiles[profileName];
                    //    //return userPattern != null && userPattern.Count != 0;
                    //}

                    var goals = Tracker.Current?.Interaction?.GetPages().SelectMany(page => page.PageEvents.Where(pe => pe.IsGoal)).Select(pe => pe.PageEventDefinitionId.ToString());
                    if (goals != null && goals.Any())
                    {
                        e.Headers.Add("Goals", string.Join(",", goals));
                    }

                    var pageEvents = Tracker.Current?.Interaction?.GetPages().SelectMany(page => page.PageEvents.Where(pe => !pe.IsGoal)).Select(pe => pe.PageEventDefinitionId.ToString());
                    if (pageEvents != null && pageEvents.Any())
                    {
                        e.Headers.Add("PageEvents", string.Join(",", pageEvents));
                    }

                    var interactionOutcomes = Tracker.Current?.Interaction?.Outcomes ?? Enumerable.Empty<OutcomeData>();
                    var pageOutcomes = Tracker.Current?.Interaction?.Pages.SelectMany(p => p.Outcomes) ?? Enumerable.Empty<OutcomeData>();
                    var outcomes = interactionOutcomes.Union(pageOutcomes).Select(o => o.OutcomeDefinitionId);
                    if (outcomes != null && outcomes.Any())
                    {
                        e.Headers.Add("Outcomes", string.Join(",", outcomes));
                    }
                }
            });

            return container;
        }
    }
}