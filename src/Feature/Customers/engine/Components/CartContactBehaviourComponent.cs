using Sitecore.Commerce.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Customers.Engine
{
    public class CartContactBehaviourComponent : Component
    {
        public int TotalVisits { get; set; } = 1;
        public int EngagementValue { get; set; } = 0;
        public List<string> CampaignIds { get; set; } = new List<string>();
        public List<string> Goals { get; set; } = new List<string>();
        public List<string> PageEvents { get; set; } = new List<string>();
        public List<string> Outcomes { get; set; } = new List<string>();

        public void AddCampaignId(string campaignId)
        {
            if (!CampaignIds.Contains(campaignId))
            {
                CampaignIds.Add(campaignId);
            }
        }

        public void AddGoals(string goalsCommaSeparated)
        {
            AddCommaSeperatedValueToList(goalsCommaSeparated, Goals);
        }

        public void AddPageEvents(string pageEventsCommaSeparated)
        {
            AddCommaSeperatedValueToList(pageEventsCommaSeparated, PageEvents);
        }

        public void AddOutcomes(string outcomesCommaSeparated)
        {
            AddCommaSeperatedValueToList(outcomesCommaSeparated, Outcomes);
        }

        private void AddCommaSeperatedValueToList(string commaSeparatedValue, List<string> existingList)
        {
            if (string.IsNullOrWhiteSpace(commaSeparatedValue))
            {
                return;
            }

            var newList = commaSeparatedValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            existingList.AddRange(newList.Except(existingList));
        }
    }
}
