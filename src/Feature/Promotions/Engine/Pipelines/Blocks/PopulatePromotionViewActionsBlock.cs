// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PopulatePromotionViewActionsBlock.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2019
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Feature.Promotions.Engine.Pipelines.Blocks
{
	using Sitecore.Commerce.Core;
	using Sitecore.Commerce.EntityViews;
	using Sitecore.Commerce.Plugin.BusinessUsers;
	using Sitecore.Commerce.Plugin.Promotions;
	using Sitecore.Framework.Pipelines;
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using PromotionsConstants = Feature.Promotions.Engine.PromotionsConstants;

	/// <summary>Enables the 'Edit' and 'Localize' view actions if the promotion is active (current
	/// date prior to 'Valid To' date and promotion has not been disabled.)</summary>
	/// <seealso>
	///     <cref>
	///         Sitecore.Framework.Pipelines.PipelineBlock{Sitecore.Commerce.EntityViews.EntityView,
	///         Sitecore.Commerce.EntityViews.EntityView, Sitecore.Commerce.Core.CommercePipelineExecutionContext}
	///     </cref>
	/// </seealso>
	[PipelineDisplayName(PromotionsConstants.Pipelines.Blocks.PopulatePromotionViewActions)]
	public class PopulatePromotionViewActionsBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
	{
		/// <inheritdoc />
		/// <summary>Initializes a new instance of the <see cref="T:Sitecore.Framework.Pipelines.PipelineBlock" /> class.</summary>
		public PopulatePromotionViewActionsBlock()
		  : base(null)
		{
		}

		/// <summary>The run.</summary>
		/// <param name="entityView">The <see cref="EntityView"/>.</param>
		/// <param name="context">The context.</param>
		/// <returns>The <see cref="EntityView"/>.</returns>
		public override Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
		{
			if (string.IsNullOrEmpty(entityView?.Name)
				|| !entityView.Name.Equals(context.GetPolicy<KnownPromotionsViewsPolicy>().Details, StringComparison.OrdinalIgnoreCase)
				|| !string.IsNullOrEmpty(entityView.Action))
			{
				return Task.FromResult(entityView);
			}

			var entityViewArgument = context.CommerceContext.GetObjects<EntityViewArgument>().FirstOrDefault();
			var promotion = entityViewArgument?.Entity as Promotion;
			if (promotion == null)
			{
				return Task.FromResult(entityView);
			}

			var effectiveDate = context.CommerceContext.CurrentEffectiveDate();
			var actionsPolicy = entityView.GetPolicy<ActionsPolicy>();
			var editAction = actionsPolicy.Actions.FirstOrDefault(p => p.Name.Equals(context.GetPolicy<KnownPromotionsActionsPolicy>().EditPromotion, StringComparison.OrdinalIgnoreCase));
			if (editAction != null)
			{
				editAction.IsEnabled = promotion.ValidTo.CompareTo(effectiveDate) > 0 && !promotion.HasPolicy<DisabledPolicy>();
			}
			
			var localizeAction = actionsPolicy.Actions.FirstOrDefault(a => a.Name.Equals(context.GetPolicy<KnownBusinessUsersActionsPolicy>().LocalizeProperty, StringComparison.OrdinalIgnoreCase));
			if (localizeAction != null)
			{
				localizeAction.IsEnabled = promotion.ValidTo.CompareTo(effectiveDate) > 0 && !promotion.HasPolicy<DisabledPolicy>();
				if (localizeAction.HasPolicy<MultiStepActionPolicy>())
				{
					localizeAction.GetPolicy<MultiStepActionPolicy>().FirstStep.IsEnabled = promotion.ValidTo.CompareTo(effectiveDate) > 0 && !promotion.HasPolicy<DisabledPolicy>();
				}
			}

			return Task.FromResult(entityView);
		}
	}
}
