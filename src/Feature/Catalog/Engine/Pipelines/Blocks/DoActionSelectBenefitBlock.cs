// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoActionSelectBenefitBlock.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2019
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Feature.Catalog.Engine.Pipelines.Blocks
{
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Plugin.Promotions;
    using Sitecore.Framework.Pipelines;
    using CatalogConstants = Feature.Catalog.Engine.CatalogConstants;

    /// <inheritdoc />
    /// <summary>Defines the select benefit action pipeline block</summary>
    [PipelineDisplayName(CatalogConstants.Pipelines.Blocks.DoActionSelectBenefit)]
    public class DoActionSelectBenefitBlock : BaseApplyCategoryAutocompleteForSelectBlock
    {
        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="T:Sitecore.Framework.Pipelines.PipelineBlock" /> class.</summary>
        /// <param name="commander">The commerce commander.</param>
        public DoActionSelectBenefitBlock(CommerceCommander commander)
            : base(commander)
        {
        }

        /// <summary>Gets the action name.</summary>
        /// <param name="context">The context.</param
        /// <returns>The action name.</returns>
        protected override string GetActionName(CommercePipelineExecutionContext context)
        {
            return context.GetPolicy<KnownPromotionsActionsPolicy>().SelectBenefit;
        }
    }
}