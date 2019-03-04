// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoActionAddBenefitBlock.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2019
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Feature.Catalog.Engine.Pipelines.Blocks
{
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;
    using Sitecore.Commerce.Plugin.Catalog;
    using Sitecore.Commerce.Plugin.Promotions;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;
    using System;
    using System.Threading.Tasks;
    using CatalogConstants = Feature.Catalog.Engine.CatalogConstants;

    /// <summary> Defines the add benefit action pipeline block</summary>
    /// <seealso>
    ///     <cref>
    ///         Sitecore.Framework.Pipelines.PipelineBlock{Sitecore.Commerce.EntityViews.EntityView,
    ///         Sitecore.Commerce.EntityViews.EntityView, Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///     </cref>
    /// </seealso>
    [PipelineDisplayName(CatalogConstants.Pipelines.Blocks.DoActionAddBenefit)]
    public class DoActionAddBenefitBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        /// <summary>Gets or sets the commander.</summary>
        /// <value>The commander.</value>
        protected CommerceCommander Commander { get; set; }

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="T:Sitecore.Framework.Pipelines.PipelineBlock" /> class.</summary>
        /// <param name="commander">The commerce commander.</param>
        public DoActionAddBenefitBlock(CommerceCommander commander)
            : base(null)
        {
            this.Commander = commander;
        }

        /// <summary>The execute.</summary>
        /// <param name="entityView">The <see cref="EntityView"/>.</param>
        /// <param name="context">The context.</param>
        /// <returns>The <see cref="EntityView"/>.</returns>
        public override async Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            Condition.Requires(entityView).IsNotNull($"{this.Name}: The entity view can not be null");

            if (string.IsNullOrEmpty(entityView?.Action)
                    || !entityView.Action.Equals(context.GetPolicy<KnownPromotionsActionsPolicy>().AddBenefit, StringComparison.OrdinalIgnoreCase)
                    || (!entityView.Name.Equals(context.GetPolicy<KnownPromotionsViewsPolicy>().BenefitDetails, StringComparison.OrdinalIgnoreCase)
                    || string.IsNullOrEmpty(entityView.EntityId))
                    || context.CommerceContext.GetObject<Promotion>(p => p.Id.Equals(entityView.EntityId, StringComparison.OrdinalIgnoreCase)) == null)
            {
                return entityView;
            }

            var targetCategorySitecoreId = entityView.GetProperty("TargetCategorySitecoreId");
            if (targetCategorySitecoreId == null || !string.IsNullOrEmpty(targetCategorySitecoreId.Value))
            {
                return entityView;
            }
            
            var categoryId = entityView.GetProperty("CategoryId");
            if (string.IsNullOrEmpty(categoryId?.Value))
            {
                await context.CommerceContext.AddMessage(
                    context.GetPolicy<KnownResultCodes>().ValidationError,
                    "InvalidOrMissingPropertyValue",
                    new object[1] { "CategoryId" },
                    "Invalid or missing value for property 'CategoryId'.");

                return entityView;
            }

            var category = await Commander.Command<GetCategoryCommand>().Process(context.CommerceContext, categoryId.Value);
            if (category == null)
            {
                context.Abort($"{Name} Category {categoryId.Value} was not found", context);

                return entityView;
            }
            
            targetCategorySitecoreId.Value = category.SitecoreId;

            return entityView;
        }
    }
}