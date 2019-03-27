namespace Feature.Carts.Engine.Tests
{
    using System;
    using System.Linq;
    using AutoFixture;
    using AutoFixture.Xunit2;
    using Carts.Engine;
    using Carts.Engine.Components;
    using Carts.Engine.Tests.Utilities;
    using FluentAssertions;
    using NSubstitute;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Plugin.Carts;
    using Xunit;

    public class AutoRemoveFreeGiftBlockBlockFixture
    {
        public class Boundary : CommerceUnitTestBase
        {
            [Theory, AutoNSubstituteData]
            public async void Run_NullCart_ShouldThrowArgumentNullException(
                AutoRemoveFreeGiftBlock sut)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                var commercePipelineContext = CreateCommercePipelineExecutionContext();

                /**********************************************
                 * Act
                 **********************************************/
                var exception = await Record.ExceptionAsync(() => sut.Run(null, commercePipelineContext));

                /**********************************************
                 * Assert
                 **********************************************/
                exception.Should().NotBe(null);
                exception.Should().BeOfType<ArgumentNullException>();
            }

            [Theory, AutoNSubstituteData]
            public async void Run_NoAutoRemoveFreeGiftBlock_ShouldReturn(
                Cart cart,
                AutoRemoveFreeGiftBlock sut)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                var commercePipelineContext = CreateCommercePipelineExecutionContext();

                /**********************************************
                 * Act
                 **********************************************/
                var result = await sut.Run(cart, commercePipelineContext);

                /**********************************************
                 * Assert
                 **********************************************/
                result.Should().NotBeNull();
            }

            [Theory, AutoNSubstituteData]
            public async void Run_NoAdjustments_ShouldReturn(
                Cart cart,
                FreeGiftAutoRemoveComponent freeGiftAutoRemoveComponent,
                AutoRemoveFreeGiftBlock sut)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                var commercePipelineContext = CreateCommercePipelineExecutionContext();
                cart.SetComponent(freeGiftAutoRemoveComponent);

                /**********************************************
                 * Act
                 **********************************************/
                var result = await sut.Run(cart, commercePipelineContext);

                /**********************************************
                 * Assert
                 **********************************************/
                result.Should().NotBeNull();
            }
        }

        public class Functional : CommerceUnitTestBase
        {
            [Theory, InlineAutoNSubstituteData("Entity-Promotion-Habitat_PromotionBook_2-Free Camera with Laptop 3")]
            public async void Run_ValidFreeGiftAutoRemoveComponent_ShouldRemoveCartLine(
                string promotionId,
                Cart cart,
                FreeGiftAutoRemoveComponent freeGiftAutoRemoveComponent,
                [Frozen] IServiceProvider serviceProvider)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                var commercePipelineContext = CreateCommercePipelineExecutionContext();

                var cartLineToBeRemoved = cart.Lines.FirstOrDefault();
                cartLineToBeRemoved.UnitListPrice.Amount = 0;
                cartLineToBeRemoved.Adjustments.FirstOrDefault().AwardingBlock = promotionId;
                cartLineToBeRemoved.SetComponent(freeGiftAutoRemoveComponent);
                freeGiftAutoRemoveComponent.PromotionId = promotionId;

                var removeCartPipeline = Substitute.For<IRemoveCartLinePipeline>();
                var commerceCommander = Substitute.For<CommerceCommander>(serviceProvider);
                commerceCommander.Pipeline<IRemoveCartLinePipeline>().ReturnsForAnyArgs(removeCartPipeline);

                var sut = new AutoRemoveFreeGiftBlock(commerceCommander);

                /**********************************************
                 * Act
                 **********************************************/
                await sut.Run(cart, commercePipelineContext);

                /**********************************************
                 * Assert
                 **********************************************/
                await removeCartPipeline.ReceivedWithAnyArgs().Run(new CartLineArgument(cart, cartLineToBeRemoved), commercePipelineContext);
            }

            [Theory, AutoNSubstituteData]
            public async void Run_NoFreeGiftAutoRemoveComponent_ShouldNotRemoveCartLine(
                decimal unitListPriceAmount,
                string promotionId,
                Cart cart,
                [Frozen] IServiceProvider serviceProvider)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                var commercePipelineContext = CreateCommercePipelineExecutionContext();

                var cartLineToBeRemoved = cart.Lines.FirstOrDefault();
                cartLineToBeRemoved.UnitListPrice.Amount = unitListPriceAmount;
                cartLineToBeRemoved.Adjustments.FirstOrDefault().AwardingBlock = promotionId;

                var removeCartPipeline = Substitute.For<IRemoveCartLinePipeline>();
                var commerceCommander = Substitute.For<CommerceCommander>(serviceProvider);
                commerceCommander.Pipeline<IRemoveCartLinePipeline>().ReturnsForAnyArgs(removeCartPipeline);

                var sut = new AutoRemoveFreeGiftBlock(commerceCommander);

                /**********************************************
                 * Act
                 **********************************************/
                await sut.Run(cart, commercePipelineContext);

                /**********************************************
                 * Assert
                 **********************************************/
                await removeCartPipeline.DidNotReceiveWithAnyArgs().Run(Arg.Any<CartLineArgument>(), commercePipelineContext);
            }

            [Theory]
            [InlineAutoNSubstituteData(0, "Promotion A", "Promotion B")]
            [InlineAutoNSubstituteData(0, "Entity-Promotion-Habitat_PromotionBook_2-Free Camera with Laptop 1", "Entity-Promotion-Habitat_PromotionBook_2-Free Camera with Laptop 2")]
            public async void Run_NoMatchingAdjustments_ShouldNotRemoveCartLine(
                decimal unitListPriceAmount,
                string promotionId,
                string otherPromotionId,
                Cart cart,
                FreeGiftAutoRemoveComponent freeGiftAutoRemoveComponent,
                [Frozen] IServiceProvider serviceProvider)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                var commercePipelineContext = CreateCommercePipelineExecutionContext();

                var cartLineToBeRemoved = cart.Lines.FirstOrDefault();
                cartLineToBeRemoved.UnitListPrice.Amount = unitListPriceAmount;
                cartLineToBeRemoved.Adjustments.FirstOrDefault().AwardingBlock = otherPromotionId;
                cartLineToBeRemoved.SetComponent(freeGiftAutoRemoveComponent);
                freeGiftAutoRemoveComponent.PromotionId = promotionId;

                var removeCartPipeline = Substitute.For<IRemoveCartLinePipeline>();
                var commerceCommander = Substitute.For<CommerceCommander>(serviceProvider);
                commerceCommander.Pipeline<IRemoveCartLinePipeline>().ReturnsForAnyArgs(removeCartPipeline);

                var sut = new AutoRemoveFreeGiftBlock(commerceCommander);

                /**********************************************
                 * Act
                 **********************************************/
                await sut.Run(cart, commercePipelineContext);

                /**********************************************
                 * Assert
                 **********************************************/
                await removeCartPipeline.DidNotReceiveWithAnyArgs().Run(Arg.Any<CartLineArgument>(), commercePipelineContext);
            }
        }
    }
}