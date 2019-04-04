// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditPromotionBlock.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2019
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Feature.Promotions.Engine.Pipelines.Blocks
{
	using Sitecore.Commerce.Core;
	using Sitecore.Commerce.Plugin.ManagedLists;
	using Sitecore.Commerce.Plugin.Promotions;
	using Sitecore.Framework.Conditions;
	using Sitecore.Framework.Pipelines;
	using System;
	using System.Threading.Tasks;
	using PromotionsConstants = Feature.Promotions.Engine.PromotionsConstants;

	/// <summary>Edits the promotion entity.</summary>
	/// <seealso>
	///     <cref>
	///         Sitecore.Framework.Pipelines.PipelineBlock{Sitecore.Commerce.Plugin.Promotions.EditPromotionArgument,
	///         Sitecore.Commerce.Plugin.Promotions.Promotion, Sitecore.Commerce.Core.CommercePipelineExecutionContext}
	///     </cref>
	/// </seealso>
	[PipelineDisplayName(PromotionsConstants.Pipelines.Blocks.EditPromotion)]
	public class EditPromotionBlock : PipelineBlock<EditPromotionArgument, Promotion, CommercePipelineExecutionContext>
	{
		/// <inheritdoc />
		/// <summary>Initializes a new instance of the <see cref="T:Sitecore.Framework.Pipelines.PipelineBlock" /> class.</summary>
		public EditPromotionBlock()
		  : base(null)
		{
		}

		/// <summary>The run.</summary>
		/// <param name="EditPromotionArgument">The <see cref="EditPromotionArgument"/>.</param>
		/// <param name="context">The context.</param>
		/// <returns>The <see cref="Promotion"/>.</returns>
		public override async Task<Promotion> Run(EditPromotionArgument arg, CommercePipelineExecutionContext context)
		{
			Condition.Requires(arg).IsNotNull($"{Name}: The block's argument cannot be null.");
			Condition.Requires(arg.Promotion).IsNotNull($"{Name}: The promotion cannot be null.");
			Condition.Requires(arg.ValidFrom).IsNotNull($"{Name}: The promotion's valid from date cannot be null.");
			Condition.Requires(arg.ValidTo).IsNotNull($"{Name}: The promotion's valid to date cannot be null.");
			Condition.Requires(arg.Text).IsNotNullOrEmpty($"{Name}: The promotion's text cannot be null or empty.");
			Condition.Requires(arg.CartText).IsNotNullOrEmpty($"{Name}: The promotion's cart text cannot be null or empty.");

			var promotion = arg.Promotion;
			var effectiveDate = context.CommerceContext.CurrentEffectiveDate();
			if (arg.ValidTo.CompareTo(effectiveDate) <= 0)
			{
				context.Abort(
					await context.CommerceContext.AddMessage(
						context.GetPolicy<KnownResultCodes>().Error,
						"ValidToDateInPast",
						new object[] { promotion.FriendlyId },
						$"'{promotion.FriendlyId}' cannot be edited because Valid To date is in the past."),
					context);

				return null;
			}

			if (!promotion.IsDraft(context.CommerceContext) && promotion.ValidTo.CompareTo(effectiveDate) <= 0)
			{
				context.Abort(
					await context.CommerceContext.AddMessage(
						context.GetPolicy<KnownResultCodes>().Error,
						"EntityIsApproved",
						new object[] { promotion.FriendlyId },
						$"'{promotion.FriendlyId}' cannot be edited or deleted because is approved and has past the promotion's Valid To date."),
					context);

				return null;
			}

			if (promotion.HasPolicy<DisabledPolicy>())
			{
				context.Abort(
					await context.CommerceContext.AddMessage(
						context.GetPolicy<KnownResultCodes>().Error,
						"EntityIsDisabled",
						new object[] { promotion.FriendlyId },
						$"'{promotion.FriendlyId}' cannot be edited or deleted because is disabled."),
					context);

				return null;
			}

			if (DateTimeOffset.Compare(arg.ValidTo, arg.ValidFrom) <= 0)
			{
				context.Abort(
					await context.CommerceContext.AddMessage(
						context.GetPolicy<KnownResultCodes>().Error,
						"InvalidDateRange",
						null,
						$"Invalid date range.'{arg.ValidTo}' cannot be earlier than '{arg.ValidFrom}'"),
					context);

				return promotion;
			}

			promotion.ValidFrom = arg.ValidFrom;
			promotion.ValidTo = arg.ValidTo;
			promotion.Description = arg.Description;
			promotion.DisplayName = arg.DisplayName;
			promotion.DisplayCartText = arg.CartText;
			promotion.DisplayText = arg.Text;

			if (arg.IsExclusive)
			{
				promotion.SetPolicy(new ExclusivePromotionPolicy());
			}
			else if (promotion.HasPolicy<ExclusivePromotionPolicy>())
			{
				promotion.Policies.Remove(promotion.GetPolicy<ExclusivePromotionPolicy>());
			}

			var component = promotion.GetComponent<TransientListMembershipsComponent>();
			if (component != null)
			{
				component.Memberships.Add(context.GetPolicy<KnownPromotionsListsPolicy>().PromotionsIndex);
			}

			return promotion;
		}
	}
}
