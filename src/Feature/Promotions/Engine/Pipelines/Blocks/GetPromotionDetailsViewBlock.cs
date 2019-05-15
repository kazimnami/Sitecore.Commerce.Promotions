// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetPromotionDetailsViewBlock.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2019
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Feature.Promotions.Engine.Pipelines.Blocks
{
	using Sitecore.Commerce.Core;
	using Sitecore.Commerce.EntityViews;
	using Sitecore.Commerce.Plugin.Promotions;
	using Sitecore.Framework.Conditions;
	using Sitecore.Framework.Pipelines;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using PromotionsConstants = Feature.Promotions.Engine.PromotionsConstants;

	/// <summary>Updates the promotion details view to restrict certain view properties from being editable.</summary>
	/// <seealso>
	///     <cref>
	///         Sitecore.Framework.Pipelines.PipelineBlock{Sitecore.Commerce.EntityViews.EntityView,
	///         Sitecore.Commerce.EntityViews.EntityView, Sitecore.Commerce.Core.CommercePipelineExecutionContext}
	///     </cref>
	/// </seealso>
	[PipelineDisplayName(PromotionsConstants.Pipelines.Blocks.GetPromotionDetailsView)]
	public class GetPromotionDetailsViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
	{
		/// <inheritdoc />
		/// <summary>Initializes a new instance of the <see cref="T:Sitecore.Framework.Pipelines.PipelineBlock" /> class.</summary>
		public GetPromotionDetailsViewBlock()
		  : base(null)
		{
		}

		/// <summary>The run.</summary>
		/// <param name="entityView">The <see cref="EntityView"/>.</param>
		/// <param name="context">The context.</param>
		/// <returns>The <see cref="EntityView"/>.</returns>
		public override Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
		{
			Condition.Requires(entityView).IsNotNull($"{Name}: The argument cannot be null");

			var entityViewArgument = context.CommerceContext.GetObject<EntityViewArgument>();
			if (string.IsNullOrEmpty(entityViewArgument?.ViewName)
					|| !entityViewArgument.ViewName.Equals(context.GetPolicy<KnownPromotionsViewsPolicy>().Details, StringComparison.OrdinalIgnoreCase)
					&& !entityViewArgument.ViewName.Equals(context.GetPolicy<KnownPromotionsViewsPolicy>().Master, StringComparison.OrdinalIgnoreCase))
			{
				return Task.FromResult(entityView);
			}

			var isEditAction = entityViewArgument.ForAction.Equals(context.GetPolicy<KnownPromotionsActionsPolicy>().EditPromotion, StringComparison.OrdinalIgnoreCase);
			if (!(entityViewArgument.Entity is Promotion) || !isEditAction)
			{
				return Task.FromResult(entityView);
			}

			var promotion = (Promotion)entityViewArgument.Entity;
			var effectiveDate = context.CommerceContext.CurrentEffectiveDate();
			if (promotion.IsDraft(context.CommerceContext)
					|| promotion.ValidTo.CompareTo(effectiveDate) <= 0
					|| promotion.HasPolicy<DisabledPolicy>())
			{
				return Task.FromResult(entityView);
			}

			entityView.GetProperty("ValidFrom").IsHidden = true;
			entityView.GetProperty("IsExclusive").IsReadOnly = true;

			return Task.FromResult(entityView);
		}
	}
}
