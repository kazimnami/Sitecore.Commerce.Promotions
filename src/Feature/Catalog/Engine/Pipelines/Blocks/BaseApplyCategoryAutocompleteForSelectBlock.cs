// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseApplyCategoryAutocompleteForSelectBlock.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2019
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Feature.Catalog.Engine.Pipelines.Blocks
{
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;
    using Sitecore.Commerce.Plugin.Catalog;
    using Sitecore.Commerce.Plugin.Promotions;
    using Sitecore.Commerce.Plugin.Search;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CatalogConstants = Feature.Catalog.Engine.CatalogConstants;

    /// <summary>Replaces the TargetCategorySitecoreId field with a CategoryId autocomplete field</summary>
    /// <seealso>
    ///     <cref>
    ///         Sitecore.Framework.Pipelines.PipelineBlock{Sitecore.Commerce.EntityViews.EntityView,
    ///         Sitecore.Commerce.EntityViews.EntityView, Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///     </cref>
    /// </seealso>
    [PipelineDisplayName(CatalogConstants.Pipelines.Blocks.BaseApplyCategoryAutocompleteForSelect)]
    public abstract class BaseApplyCategoryAutocompleteForSelectBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        /// <summary>Gets or sets the commander.</summary>
        /// <value>The commander.</value>
        protected CommerceCommander Commander { get; set; }

        /// <inheritdoc />
        /// <summary>Initializes a new instance of the <see cref="T:Sitecore.Framework.Pipelines.PipelineBlock" /> class.</summary>
        /// <param name="commander">The commerce commander.</param>
        public BaseApplyCategoryAutocompleteForSelectBlock(CommerceCommander commander)
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
            Condition.Requires(entityView).IsNotNull($"{Name}: The argument cannot be null.");

            if (string.IsNullOrEmpty(entityView?.Action)
                    || !entityView.Action.Equals(this.GetActionName(context), StringComparison.OrdinalIgnoreCase))
            {
                return await Task.FromResult(entityView);
            }

            var promotion = context.CommerceContext.GetObjects<Promotion>().FirstOrDefault(p => p.Id.Equals(entityView.EntityId, StringComparison.OrdinalIgnoreCase));
            if (promotion == null)
            {
                return await Task.FromResult(entityView);
            }

            var viewProperty = entityView.GetProperty("TargetCategorySitecoreId");
            if (viewProperty == null)
            {
                return await Task.FromResult(entityView);
            }

            viewProperty.IsHidden = true;
            viewProperty.IsRequired = false;
            PopulateItemDetails(entityView, context);

            return await Task.FromResult(entityView);
        }

        /// <summary>Sets the CategoryId view property to autocomplete.</summary>
        /// <param name="view">The <see cref="EntityView"/>.</param>
        /// <param name="context">The context.</param>
        protected virtual void PopulateItemDetails(EntityView view, CommercePipelineExecutionContext context)
        {
            if (view == null)
            {
                return;
            }

            var categoryId = view.GetProperty("CategoryId");
            if (categoryId == null)
            {
                return;
            }
            
            var policyByType = SearchScopePolicy.GetPolicyByType(
                context.CommerceContext,
                context.CommerceContext.Environment,
                typeof(Category));
            if (policyByType != null)
            {
                var policy = new Policy()
                {
                    PolicyId = "EntityType",
                    Models = new List<Model>()
                    {
                        new Model() { Name = "Category" }
                    }
                };
                categoryId.UiType = "Autocomplete";
                categoryId.Policies.Add(policy);
                categoryId.Policies.Add(policyByType);
            }
        }

        /// <summary>Gets the action name.</summary>
        /// <param name="context">The context.</param
        /// <returns>The action name.</returns>
        protected abstract string GetActionName(CommercePipelineExecutionContext context);
    }
}