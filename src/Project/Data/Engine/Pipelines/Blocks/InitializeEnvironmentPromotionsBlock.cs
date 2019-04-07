// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InitializeEnvironmentPromotionsBlock.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Project.SamplePromotions.Engine.Pipelines.Blocks
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Core.Commands;
    using Sitecore.Commerce.Plugin.Carts;
    using Sitecore.Commerce.Plugin.Catalog;
    using Sitecore.Commerce.Plugin.Coupons;
    using Sitecore.Commerce.Plugin.Promotions;
    using Sitecore.Commerce.Plugin.Rules;
    using Sitecore.Framework.Pipelines;

    /// <summary>
    /// Defines a block which bootstraps promotions.
    /// </summary>
    /// <seealso>
    ///     <cref>
    ///         Sitecore.Framework.Pipelines.PipelineBlock{System.String, System.String,
    ///         Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///     </cref>
    /// </seealso>
    [PipelineDisplayName("SamplePromotions.Blocks.InitializeEnvironmentPromotionsBlock")]
    public class InitializeEnvironmentPromotionsBlock : PipelineBlock<string, string, CommercePipelineExecutionContext>
    {
        protected CommerceCommander Commander { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializeEnvironmentPromotionsBlock"/> class.
        /// </summary>
        /// <param name="persistEntityPipeline">The persist entity pipeline.</param>
        public InitializeEnvironmentPromotionsBlock(CommerceCommander commander)
        {
            this.Commander = commander;
        }

        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="arg">
        /// The argument.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override async Task<string> Run(string arg, CommercePipelineExecutionContext context)
        {
            var artifactSet = "Environment.Sample.Promotions-1.0";

            // Check if this environment has subscribed to this Artifact Set
            if (!arg.Equals("override") && !context.GetPolicy<EnvironmentInitializationPolicy>().InitialArtifactSets.Contains(artifactSet))
            {
                return arg;
            }

            context.Logger.LogInformation($"{this.Name}.InitializingArtifactSet: ArtifactSet={artifactSet}");

            var promotionBookName = "Habitat_PromotionBook";
            var entityId = promotionBookName.ToEntityId<PromotionBook>();
            var book = await Commander.GetEntity<PromotionBook>(context.CommerceContext, entityId);

            if (book == null)
            {
                return arg;
            }

            await this.CreateCartShippingOptionDiscountPromotion(book, context);
            await this.CreateCartLineQuantityRangePercentageOffPromotion(book, context);
            await this.CreateBuyXQuantityForYQuantityPricePromotion(book, context);
            await this.CreateBuyXQuantityForSellPricePromotion(book, context);
            await this.CreateAmountOffAudioCategoryPromotion(book, context);
            await this.CreatePercentOffAudioCategoryPromotion(book, context);
            await this.CreateAmountOffEarbudsTagPromotion(book, context);
            await this.CreatePercentOffEarbudsTagPromotion(book, context);
            await this.CreateAmountOffKickbudsBrandPromotion(book, context);
            await this.CreatePercentOffKickbudsBrandPromotion(book, context);

            return arg;
        }

        #region Cart's Promotions

        /// <summary>
        /// Creates the cart shipping option discount promotion.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Task"/></returns>
        private async Task CreateCartShippingOptionDiscountPromotion(PromotionBook book, CommercePipelineExecutionContext context)
        {
            /* #16 Shipping option specific discount */
            var promotionId = "_Cart5OffDeliveryShippingOptionPromotion";
            var entityId = $"{book.Name}-{promotionId}".ToEntityId<Promotion>();
            var promotion = await Commander.GetEntity<Promotion>(context.CommerceContext, entityId);
            if (promotion != null)
            {
                return;
            }

            var promotionName = "$5 Off Delivery Shipping";
            promotion =
                await Commander.Pipeline<AddPromotionPipeline>().Run(
                    new AddPromotionArgument(book, promotionId, DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddYears(1), promotionName, promotionName)
                        {
                            DisplayName = "$5 Off Delivery Shipping",
                            Description = "$5 Off Delivery Shipping Option on $200 or more"
                        },
                    context);

            promotion =
                await Commander.Pipeline<AddQualificationPipeline>().Run(
                    new PromotionConditionModelArgument(
                        promotion,
                        new ConditionModel
                        {
                            ConditionOperator = "And",
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = CartsConstants.Conditions.CartSubtotalCondition,
                            Name = CartsConstants.Conditions.CartSubtotalCondition,
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("Operator", Constants.Operators.DecimalGreaterThanEqualToOperator, Constants.DisplayType.BinaryOperatorDec, true),
                                this.AddProperty("Subtotal", "200", Constants.DisplayType.Decimal)
                            }
                        }),
                    context);

            promotion =
                await Commander.Pipeline<AddQualificationPipeline>().Run(
                    new PromotionConditionModelArgument(
                        promotion,
                        new ConditionModel
                        {
                            ConditionOperator = "And",
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = "CartHasFulfillmentOptionCondition",
                            Name = "CartHasFulfillmentOptionCondition",
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("FulfillmentOptionName", "ShipToMe", "System.String")
                            }
                        }),
                    context);

            promotion =
                await Commander.Pipeline<AddQualificationPipeline>().Run(
                    new PromotionConditionModelArgument(
                        promotion,
                        new ConditionModel
                        {
                            ConditionOperator = "Or",
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = "CartLineHasFulfillmentOptionCondition",
                            Name = "CartLineHasFulfillmentOptionCondition",
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("FulfillmentOptionName", "ShipToMe", "System.String")
                            }
                        }),
                    context);

            promotion =
                await Commander.Pipeline<AddBenefitPipeline>().Run(
                    new PromotionActionModelArgument(
                        promotion,
                        new ActionModel
                        {
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = "CartShippingOptionAmountOffAction",
                            Name = "CartShippingOptionAmountOffAction",
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("FulfillmentOptionName", "ShipToMe", "System.String"),
                                this.AddProperty("AmountOff", "5", Constants.DisplayType.Decimal)
                            }
                        }),
                    context);

			promotion =
				await Commander.Pipeline<AddBenefitPipeline>().Run(
					new PromotionActionModelArgument(
						promotion,
						new ActionModel
						{
							Id = Guid.NewGuid().ToString(),
							LibraryId = "CartLineShippingOptionAmountOffAction",
							Name = "CartLineShippingOptionAmountOffAction",
							Properties = new List<PropertyModel>
							{
								this.AddProperty("FulfillmentOptionName", "ShipToMe", "System.String"),
								this.AddProperty("AmountOff", "5", Constants.DisplayType.Decimal)
							}
						}),
					context);

			promotion = await Commander.Pipeline<AddPublicCouponPipeline>().Run(new AddPublicCouponArgument(promotion, "SHIPPINGDISCOUNT"), context);
            promotion.SetComponent(new ApprovalComponent(context.GetPolicy<ApprovalStatusPolicy>().Approved));
            await Commander.Pipeline<PersistEntityPipeline>().Run(new PersistEntityArgument(promotion), context);
        }

        /// <summary>
        /// Creates cart line quantity range percentage discount promotion.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Task"/></returns>
        private async Task CreateCartLineQuantityRangePercentageOffPromotion(PromotionBook book, CommercePipelineExecutionContext context)
        {
            /* #16 Buy X Sellable-Items in range get Y % off */
            /* FM-X Wireless FM Over-the-Ear (Habitat_Master|6042060|)*/
            var promotionId = "_LineQuantityPctOffPromotion";
            var promotion = await Commander.GetEntity<Promotion>(context.CommerceContext, $"Entity-Promotion-{book.Name}-{promotionId}");
            if (promotion != null)
            {
                return;
            }

            var promotionName = "The more you buy the more you save!";
            promotion =
                await Commander.Pipeline<AddPromotionPipeline>().Run(
                    new AddPromotionArgument(book, promotionId, DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddYears(1), promotionName, promotionName)
                    {
                        IsExclusive = true,
                        DisplayName = promotionName,
                        Description = "Buy Sellable-Item in Quantity Range to get Y % Off"
                    },
                    context);

            promotion =
                await Commander.Pipeline<AddQualificationPipeline>().Run(
                    new PromotionConditionModelArgument(
                        promotion,
                        new ConditionModel
                        {
                            ConditionOperator = "And",
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = CartsConstants.Conditions.CartItemQuantityRangeCondition,
                            Name = CartsConstants.Conditions.CartItemQuantityRangeCondition,
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("TargetItemId", "Habitat_Master|6042060|", Constants.DisplayType.String),
                                this.AddProperty("MinQuantity", "3", Constants.DisplayType.Decimal),
                                this.AddProperty("MaxQuantity", "8", Constants.DisplayType.Decimal)
                            }
                        }),
                    context);

            promotion =
                await Commander.Pipeline<AddBenefitPipeline>().Run(
                    new PromotionActionModelArgument(
                        promotion,
                        new ActionModel
                        {
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = "CartItemQuantityRangePercentOffAction",
                            Name = "CartItemQuantityRangePercentOffAction",
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("TargetItemId", "Habitat_Master|6042060|", Constants.DisplayType.String),
                                this.AddProperty("MinQuantity", "3", Constants.DisplayType.Decimal),
                                this.AddProperty("MaxQuantity", "4", Constants.DisplayType.Decimal),
                                this.AddProperty("PercentOff", "5", Constants.DisplayType.Decimal)
                            }
                        }),
                    context);

            promotion =
                await Commander.Pipeline<AddBenefitPipeline>().Run(
                    new PromotionActionModelArgument(
                        promotion,
                        new ActionModel
                        {
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = "CartItemQuantityRangePercentOffAction",
                            Name = "CartItemQuantityRangePercentOffAction",
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("TargetItemId", "Habitat_Master|6042060|", Constants.DisplayType.String),
                                this.AddProperty("MinQuantity", "5", Constants.DisplayType.Decimal),
                                this.AddProperty("MaxQuantity", "6", Constants.DisplayType.Decimal),
                                this.AddProperty("PercentOff", "10", Constants.DisplayType.Decimal)
                            }
                        }),
                    context);

            promotion =
                await Commander.Pipeline<AddBenefitPipeline>().Run(
                    new PromotionActionModelArgument(
                        promotion,
                        new ActionModel
                        {
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = "CartItemQuantityRangePercentOffAction",
                            Name = "CartItemQuantityRangePercentOffAction",
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("TargetItemId", "Habitat_Master|6042060|", Constants.DisplayType.String),
                                this.AddProperty("MinQuantity", "7", Constants.DisplayType.Decimal),
                                this.AddProperty("MaxQuantity", "8", Constants.DisplayType.Decimal),
                                this.AddProperty("PercentOff", "15", Constants.DisplayType.Decimal)
                            }
                        }),
                    context);

            promotion = await Commander.Pipeline<AddPublicCouponPipeline>().Run(new AddPublicCouponArgument(promotion, "RANGEPCTOFF"), context);
            promotion.SetComponent(new ApprovalComponent(context.GetPolicy<ApprovalStatusPolicy>().Approved));
            await Commander.Pipeline<PersistEntityPipeline>().Run(new PersistEntityArgument(promotion), context);
        }

        /// <summary>
        /// Creates buy X quantity for Y quantity price promotion.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Task"/></returns>
        private async Task CreateBuyXQuantityForYQuantityPricePromotion(PromotionBook book, CommercePipelineExecutionContext context)
        {
            /* #1 Buy X Quantity for Y Quantity */
            /* Wireless Skinnybuds Headphones (Habitat_Master|6042061|)*/
            var promotionId = "_LineXQuantityForYQuantityPromotion";
            var promotion = await Commander.GetEntity<Promotion>(context.CommerceContext, $"Entity-Promotion-{book.Name}-{promotionId}");
            if (promotion != null)
            {
                return;
            }

            var promotionName = "Buy 4 For the Price of 3";
            promotion =
                await Commander.Pipeline<AddPromotionPipeline>().Run(
                    new AddPromotionArgument(book, promotionId, DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddYears(1), promotionName, promotionName)
                    {
                        DisplayName = promotionName,
                        Description = promotionName
                    },
                    context);

            promotion =
                await Commander.Pipeline<AddQualificationPipeline>().Run(
                    new PromotionConditionModelArgument(
                        promotion,
                        new ConditionModel
                        {
                            ConditionOperator = "And",
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = CartsConstants.Conditions.CartItemQuantityCondition,
                            Name = CartsConstants.Conditions.CartItemQuantityCondition,
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("TargetItemId", "Habitat_Master|6042061|", Constants.DisplayType.String),
                                this.AddProperty("Operator", Constants.Operators.DecimalGreaterThanEqualToOperator, Constants.DisplayType.BinaryOperatorDec, true),
                                this.AddProperty("Quantity", "4", Constants.DisplayType.Decimal)
                            }
                        }),
                    context);

            promotion =
                await Commander.Pipeline<AddBenefitPipeline>().Run(
                    new PromotionActionModelArgument(
                        promotion,
                        new ActionModel
                        {
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = "CartItemQuantityXForQuantityYAction",
                            Name = "CartItemQuantityXForQuantityYAction",
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("TargetItemId", "Habitat_Master|6042061|", Constants.DisplayType.String),
                                this.AddProperty("QuantityX", "4", Constants.DisplayType.Integer),
                                this.AddProperty("QuantityY", "3", Constants.DisplayType.Integer),
                                this.AddProperty("MaximumApplications", "0", Constants.DisplayType.Integer)
                            }
                        }),
                    context);

            promotion = await Commander.Pipeline<AddPublicCouponPipeline>().Run(new AddPublicCouponArgument(promotion, "4FOR3"), context);
            promotion.SetComponent(new ApprovalComponent(context.GetPolicy<ApprovalStatusPolicy>().Approved));
            await Commander.Pipeline<PersistEntityPipeline>().Run(new PersistEntityArgument(promotion), context);
        }

        /// <summary>
        /// Creates buy X quantity for sell price promotion.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Task"/></returns>
        private async Task CreateBuyXQuantityForSellPricePromotion(PromotionBook book, CommercePipelineExecutionContext context)
        {
            /* #2 Buy X Quantity For Y Price */
            /* Athletica SportClip Standard Earbuds (Habitat_Master|6042062|)*/
            var promotionId = "_LineXQuantityForPricePromotion";
            var promotion = await Commander.GetEntity<Promotion>(context.CommerceContext, $"Entity-Promotion-{book.Name}-{promotionId}");
            if (promotion != null)
            {
                return;
            }

            var promotionName = "Buy 3 For $50";
            promotion =
                await Commander.Pipeline<AddPromotionPipeline>().Run(
                    new AddPromotionArgument(book, promotionId, DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddYears(1), promotionName, promotionName)
                    {
                        DisplayName = promotionName,
                        Description = promotionName
                    },
                    context);

            promotion =
                await Commander.Pipeline<AddQualificationPipeline>().Run(
                    new PromotionConditionModelArgument(
                        promotion,
                        new ConditionModel
                        {
                            ConditionOperator = "And",
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = CartsConstants.Conditions.CartItemQuantityCondition,
                            Name = CartsConstants.Conditions.CartItemQuantityCondition,
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("TargetItemId", "Habitat_Master|6042062|", Constants.DisplayType.String),
                                this.AddProperty("Operator", Constants.Operators.DecimalGreaterThanEqualToOperator, Constants.DisplayType.BinaryOperatorDec, true),
                                this.AddProperty("Quantity", "3", Constants.DisplayType.Integer)
                            }
                        }),
                    context);

            promotion =
                await Commander.Pipeline<AddBenefitPipeline>().Run(
                    new PromotionActionModelArgument(
                        promotion,
                        new ActionModel
                        {
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = "CartItemQuantityXSellPriceAction",
                            Name = "CartItemQuantityXSellPriceAction",
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("TargetItemId", "Habitat_Master|6042062|", Constants.DisplayType.String),
                                this.AddProperty("QuantityX", "3", Constants.DisplayType.Integer),
                                this.AddProperty("SellPrice", "50", Constants.DisplayType.Decimal),
                                this.AddProperty("MaximumApplications", "0", Constants.DisplayType.Integer)
                            }
                        }),
                    context);

            promotion = await Commander.Pipeline<AddPublicCouponPipeline>().Run(new AddPublicCouponArgument(promotion, "3FOR50"), context);
            promotion.SetComponent(new ApprovalComponent(context.GetPolicy<ApprovalStatusPolicy>().Approved));
            await Commander.Pipeline<PersistEntityPipeline>().Run(new PersistEntityArgument(promotion), context);
        }

        /// <summary>
        /// Creates $ discount on Audio category promotion.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Task"/></returns>
        private async Task CreateAmountOffAudioCategoryPromotion(PromotionBook book, CommercePipelineExecutionContext context)
        {
            /* #12 $ off category */
            /* Audio (a0860c27-2a32-841f-7014-75b163b9471e)*/
            var promotionId = "_LineCategoryAmountDiscountPromotion";
            var promotion = await Commander.GetEntity<Promotion>(context.CommerceContext, $"Entity-Promotion-{book.Name}-{promotionId}");
            if (promotion != null)
            {
                return;
            }

            var promotionName = "$10 off Audio category when over $100 spent";
            promotion =
                await Commander.Pipeline<AddPromotionPipeline>().Run(
                    new AddPromotionArgument(book, promotionId, DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddYears(1), promotionName, promotionName)
                    {
                        DisplayName = promotionName,
                        Description = promotionName
                    },
                    context);

            promotion =
                await Commander.Pipeline<AddQualificationPipeline>().Run(
                    new PromotionConditionModelArgument(
                        promotion,
                        new ConditionModel
                        {
                            ConditionOperator = "And",
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = "CartAnyItemHasCategoryCondition",
                            Name = "CartAnyItemHasCategoryCondition",
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("TargetCategorySitecoreId", "a0860c27-2a32-841f-7014-75b163b9471e", Constants.DisplayType.String),
                                this.AddProperty("CategoryId", "Entity-Category-Habitat_Master-Audio", Constants.DisplayType.String)
                            }
                        }),
                    context);

            promotion =
                await Commander.Pipeline<AddBenefitPipeline>().Run(
                    new PromotionActionModelArgument(
                        promotion,
                        new ActionModel
                        {
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = "CartItemTargetCategorySubtotalAmountOffAction",
                            Name = "CartItemTargetCategorySubtotalAmountOffAction",
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("TargetCategorySitecoreId", "a0860c27-2a32-841f-7014-75b163b9471e", Constants.DisplayType.String),
                                this.AddProperty("CategoryId", "Entity-Category-Habitat_Master-Audio", Constants.DisplayType.String),
                                this.AddProperty("SubtotalOperator", Constants.Operators.DecimalGreaterThanEqualToOperator, Constants.DisplayType.BinaryOperatorDec, true),
                                this.AddProperty("Subtotal", "100", Constants.DisplayType.Decimal),
                                this.AddProperty("AmountOff", "10", Constants.DisplayType.Decimal)
                            }
                        }),
                    context);

            promotion = await Commander.Pipeline<AddPublicCouponPipeline>().Run(new AddPublicCouponArgument(promotion, "10OFFAUDIO"), context);
            promotion.SetComponent(new ApprovalComponent(context.GetPolicy<ApprovalStatusPolicy>().Approved));
            await Commander.Pipeline<PersistEntityPipeline>().Run(new PersistEntityArgument(promotion), context);
        }

        /// <summary>
        /// Creates % discount on Audio category promotion.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Task"/></returns>
        private async Task CreatePercentOffAudioCategoryPromotion(PromotionBook book, CommercePipelineExecutionContext context)
        {
            /* #11 % off category */
            /* Audio (a0860c27-2a32-841f-7014-75b163b9471e)*/
            var promotionId = "_LineCategoryPercentDiscountPromotion";
            var promotion = await Commander.GetEntity<Promotion>(context.CommerceContext, $"Entity-Promotion-{book.Name}-{promotionId}");
            if (promotion != null)
            {
                return;
            }

            var promotionName = "10% off Audio category when over $100 spent";
            promotion =
                await Commander.Pipeline<AddPromotionPipeline>().Run(
                    new AddPromotionArgument(book, promotionId, DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddYears(1), promotionName, promotionName)
                    {
                        DisplayName = promotionName,
                        Description = promotionName
                    },
                    context);

            promotion =
                await Commander.Pipeline<AddQualificationPipeline>().Run(
                    new PromotionConditionModelArgument(
                        promotion,
                        new ConditionModel
                        {
                            ConditionOperator = "And",
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = "CartAnyItemHasCategoryCondition",
                            Name = "CartAnyItemHasCategoryCondition",
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("TargetCategorySitecoreId", "a0860c27-2a32-841f-7014-75b163b9471e", Constants.DisplayType.String),
                                this.AddProperty("CategoryId", "Entity-Category-Habitat_Master-Audio", Constants.DisplayType.String)
                            }
                        }),
                    context);

            promotion =
                await Commander.Pipeline<AddBenefitPipeline>().Run(
                    new PromotionActionModelArgument(
                        promotion,
                        new ActionModel
                        {
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = "CartItemTargetCategorySubtotalPercentOffAction",
                            Name = "CartItemTargetCategorySubtotalPercentOffAction",
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("TargetCategorySitecoreId", "a0860c27-2a32-841f-7014-75b163b9471e", Constants.DisplayType.String),
                                this.AddProperty("CategoryId", "Entity-Category-Habitat_Master-Audio", Constants.DisplayType.String),
                                this.AddProperty("SubtotalOperator", Constants.Operators.DecimalGreaterThanEqualToOperator, Constants.DisplayType.BinaryOperatorDec, true),
                                this.AddProperty("Subtotal", "100", Constants.DisplayType.Decimal),
                                this.AddProperty("PercentOff", "10", Constants.DisplayType.Decimal)
                            }
                        }),
                    context);

            promotion = await Commander.Pipeline<AddPublicCouponPipeline>().Run(new AddPublicCouponArgument(promotion, "10PCTOFFAUDIO"), context);
            promotion.SetComponent(new ApprovalComponent(context.GetPolicy<ApprovalStatusPolicy>().Approved));
            await Commander.Pipeline<PersistEntityPipeline>().Run(new PersistEntityArgument(promotion), context);
        }

        /// <summary>
        /// Creates $ discount on earbuds tag promotion.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Task"/></returns>
        private async Task CreateAmountOffEarbudsTagPromotion(PromotionBook book, CommercePipelineExecutionContext context)
        {
            /* #17 $ off Tag */
            /* Tag: earbuds */
            var promotionId = "_LineTagAmountDiscountPromotion";
            var promotion = await Commander.GetEntity<Promotion>(context.CommerceContext, $"Entity-Promotion-{book.Name}-{promotionId}");
            if (promotion != null)
            {
                return;
            }

            var promotionName = "$10 off products with earbuds tag when over $100 spent";
            promotion =
                await Commander.Pipeline<AddPromotionPipeline>().Run(
                    new AddPromotionArgument(book, promotionId, DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddYears(1), promotionName, promotionName)
                    {
                        DisplayName = promotionName,
                        Description = promotionName
                    },
                    context);

            promotion =
                await Commander.Pipeline<AddQualificationPipeline>().Run(
                    new PromotionConditionModelArgument(
                        promotion,
                        new ConditionModel
                        {
                            ConditionOperator = "And",
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = CartsConstants.Conditions.CartAnyItemHasTagCondition,
                            Name = CartsConstants.Conditions.CartAnyItemHasTagCondition,
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("Tag", "earbuds", Constants.DisplayType.String)
                            }
                        }),
                    context);

            promotion =
                await Commander.Pipeline<AddBenefitPipeline>().Run(
                    new PromotionActionModelArgument(
                        promotion,
                        new ActionModel
                        {
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = "CartItemTargetTagSubtotalAmountOffAction",
                            Name = "CartItemTargetTagSubtotalAmountOffAction",
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("TargetTag", "earbuds", Constants.DisplayType.String),
                                this.AddProperty("SubtotalOperator", Constants.Operators.DecimalGreaterThanEqualToOperator, Constants.DisplayType.BinaryOperatorDec, true),
                                this.AddProperty("Subtotal", "100", Constants.DisplayType.Decimal),
                                this.AddProperty("AmountOff", "10", Constants.DisplayType.Decimal)
                            }
                        }),
                    context);

            promotion = await Commander.Pipeline<AddPublicCouponPipeline>().Run(new AddPublicCouponArgument(promotion, "10OFFEARBUDS"), context);
            promotion.SetComponent(new ApprovalComponent(context.GetPolicy<ApprovalStatusPolicy>().Approved));
            await Commander.Pipeline<PersistEntityPipeline>().Run(new PersistEntityArgument(promotion), context);
        }

        /// <summary>
        /// Creates % discount on earbuds tag promotion.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Task"/></returns>
        private async Task CreatePercentOffEarbudsTagPromotion(PromotionBook book, CommercePipelineExecutionContext context)
        {
            /* #18 % off tag */
            /* Tag: earbuds */
            var promotionId = "_LineTagPercentDiscountPromotion";
            var promotion = await Commander.GetEntity<Promotion>(context.CommerceContext, $"Entity-Promotion-{book.Name}-{promotionId}");
            if (promotion != null)
            {
                return;
            }

            var promotionName = "10% off products with earbuds tag when over $100 spent";
            promotion =
                await Commander.Pipeline<AddPromotionPipeline>().Run(
                    new AddPromotionArgument(book, promotionId, DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddYears(1), promotionName, promotionName)
                    {
                        DisplayName = promotionName,
                        Description = promotionName
                    },
                    context);

            promotion =
                await Commander.Pipeline<AddQualificationPipeline>().Run(
                    new PromotionConditionModelArgument(
                        promotion,
                        new ConditionModel
                        {
                            ConditionOperator = "And",
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = CartsConstants.Conditions.CartAnyItemHasTagCondition,
                            Name = CartsConstants.Conditions.CartAnyItemHasTagCondition,
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("Tag", "earbuds", Constants.DisplayType.String)
                            }
                        }),
                    context);

            promotion =
                await Commander.Pipeline<AddBenefitPipeline>().Run(
                    new PromotionActionModelArgument(
                        promotion,
                        new ActionModel
                        {
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = "CartItemTargetTagSubtotalPercentOffAction",
                            Name = "CartItemTargetTagSubtotalPercentOffAction",
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("TargetTag", "earbuds", Constants.DisplayType.String),
                                this.AddProperty("SubtotalOperator", Constants.Operators.DecimalGreaterThanEqualToOperator, Constants.DisplayType.BinaryOperatorDec, true),
                                this.AddProperty("Subtotal", "100", Constants.DisplayType.Decimal),
                                this.AddProperty("PercentOff", "10", Constants.DisplayType.Decimal)
                            }
                        }),
                    context);

            promotion = await Commander.Pipeline<AddPublicCouponPipeline>().Run(new AddPublicCouponArgument(promotion, "10PCTOFFEARDBUDS"), context);
            promotion.SetComponent(new ApprovalComponent(context.GetPolicy<ApprovalStatusPolicy>().Approved));
            await Commander.Pipeline<PersistEntityPipeline>().Run(new PersistEntityArgument(promotion), context);
        }

		/// <summary>
		/// Creates $ discount on Kickbuds brand promotion.
		/// </summary>
		/// <param name="book">The book.</param>
		/// <param name="context">The context.</param>
		/// <returns>A <see cref="Task"/></returns>
		private async Task CreateAmountOffKickbudsBrandPromotion(PromotionBook book, CommercePipelineExecutionContext context)
        {
			/* #9 $ off Brand */
			/* Brand: Kickbuds */
			var promotionId = "_LineBrandAmountDiscountPromotion";
            var promotion = await Commander.GetEntity<Promotion>(context.CommerceContext, $"Entity-Promotion-{book.Name}-{promotionId}");
            if (promotion != null)
            {
                return;
            }

            var promotionName = "$10 off Kickbuds brand products when over $100 spent";
            promotion =
                await Commander.Pipeline<AddPromotionPipeline>().Run(
                    new AddPromotionArgument(book, promotionId, DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddYears(1), promotionName, promotionName)
                    {
                        DisplayName = promotionName,
                        Description = promotionName
                    },
                    context);

            promotion =
                await Commander.Pipeline<AddQualificationPipeline>().Run(
                    new PromotionConditionModelArgument(
                        promotion,
                        new ConditionModel
                        {
                            ConditionOperator = "And",
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = "CartAnyItemHasBrandCondition",
                            Name = "CartAnyItemHasBrandCondition",
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("Brand", "Kickbuds", Constants.DisplayType.String)
                            }
                        }),
                    context);

            promotion =
                await Commander.Pipeline<AddBenefitPipeline>().Run(
                    new PromotionActionModelArgument(
                        promotion,
                        new ActionModel
                        {
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = "CartItemTargetBrandSubtotalAmountOffAction",
                            Name = "CartItemTargetBrandSubtotalAmountOffAction",
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("TargetBrand", "Kickbuds", Constants.DisplayType.String),
                                this.AddProperty("SubtotalOperator", Constants.Operators.DecimalGreaterThanEqualToOperator, Constants.DisplayType.BinaryOperatorDec, true),
                                this.AddProperty("Subtotal", "100", Constants.DisplayType.Decimal),
                                this.AddProperty("AmountOff", "10", Constants.DisplayType.Decimal)
                            }
                        }),
                    context);

            promotion = await Commander.Pipeline<AddPublicCouponPipeline>().Run(new AddPublicCouponArgument(promotion, "10OFFKICKBUDS"), context);
            promotion.SetComponent(new ApprovalComponent(context.GetPolicy<ApprovalStatusPolicy>().Approved));
            await Commander.Pipeline<PersistEntityPipeline>().Run(new PersistEntityArgument(promotion), context);
        }

		/// <summary>
		/// Creates % discount on Kickbuds brand promotion.
		/// </summary>
		/// <param name="book">The book.</param>
		/// <param name="context">The context.</param>
		/// <returns>A <see cref="Task"/></returns>
		private async Task CreatePercentOffKickbudsBrandPromotion(PromotionBook book, CommercePipelineExecutionContext context)
        {
			/* #8 % off brand */
			/* Brand: Kickbuds */
			var promotionId = "_LineBrandPercentDiscountPromotion";
            var promotion = await Commander.GetEntity<Promotion>(context.CommerceContext, $"Entity-Promotion-{book.Name}-{promotionId}");
            if (promotion != null)
            {
                return;
            }

            var promotionName = "10% off products with earbuds tag when over $100 spent";
            promotion =
                await Commander.Pipeline<AddPromotionPipeline>().Run(
                    new AddPromotionArgument(book, promotionId, DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddYears(1), promotionName, promotionName)
                    {
                        DisplayName = promotionName,
                        Description = promotionName
                    },
                    context);

            promotion =
                await Commander.Pipeline<AddQualificationPipeline>().Run(
                    new PromotionConditionModelArgument(
                        promotion,
                        new ConditionModel
                        {
                            ConditionOperator = "And",
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = "CartAnyItemHasBrandCondition",
                            Name = "CartAnyItemHasBrandCondition",
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("Brand", "Kickbuds", Constants.DisplayType.String)
                            }
                        }),
                    context);

            promotion =
                await Commander.Pipeline<AddBenefitPipeline>().Run(
                    new PromotionActionModelArgument(
                        promotion,
                        new ActionModel
                        {
                            Id = Guid.NewGuid().ToString(),
                            LibraryId = "CartItemTargetBrandSubtotalPercentOffAction",
                            Name = "CartItemTargetBrandSubtotalPercentOffAction",
                            Properties = new List<PropertyModel>
                            {
                                this.AddProperty("TargetBrand", "Kickbuds", Constants.DisplayType.String),
                                this.AddProperty("SubtotalOperator", Constants.Operators.DecimalGreaterThanEqualToOperator, Constants.DisplayType.BinaryOperatorDec, true),
                                this.AddProperty("Subtotal", "100", Constants.DisplayType.Decimal),
                                this.AddProperty("PercentOff", "10", Constants.DisplayType.Decimal)
                            }
                        }),
                    context);

            promotion = await Commander.Pipeline<AddPublicCouponPipeline>().Run(new AddPublicCouponArgument(promotion, "10PCTOFFKICKBUDS"), context);
            promotion.SetComponent(new ApprovalComponent(context.GetPolicy<ApprovalStatusPolicy>().Approved));
            await Commander.Pipeline<PersistEntityPipeline>().Run(new PersistEntityArgument(promotion), context);
        }

        #endregion

        protected PropertyModel AddProperty(string name, string value, string displayType, bool isOperator = false)
        {
            return new PropertyModel { IsOperator = isOperator, Name = name, Value = value, DisplayType = displayType };
        }
    }
}
