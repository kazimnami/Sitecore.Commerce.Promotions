using System;
using System.Collections.Generic;
using System.Linq;
using Feature.Carts.Engine.Actions;
using Feature.Carts.Engine.Commands;
using FluentAssertions;
using NSubstitute;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using Xunit;

namespace Feature.Carts.Engine.Tests
{
    public class CartItemTargetTagFreeGiftActionFixture
    {
        public class Boundary
        {
            [Theory, AutoNSubstituteData]
            public void Execute_01_NoCommerceContext(
                IRuleValue<string> targetTag,
                Cart cart,
                CartItemTargetTagFreeGiftAction action,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());
                action.TargetTag = targetTag;
                
                context.Fact(Arg.Any<IFactIdentifier>()).Returns((CommerceContext) null);

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
                cart.Adjustments.Should().BeEmpty();
            }

            [Theory, AutoNSubstituteData]
            public void Execute_02_NoCart(
                IRuleValue<string> targetTag,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                CartItemTargetTagFreeGiftAction action,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                commerceContext.AddObject(cartTotals);
                action.TargetTag = targetTag;

                context.Fact(Arg.Any<IFactIdentifier>()).Returns(commerceContext);

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
            }

            [Theory, AutoNSubstituteData]
            public void Execute_03_NoCartLines(
                IRuleValue<string> targetTag,
                Cart cart,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                CartItemTargetTagFreeGiftAction action,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.Clear();

                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);

                action.TargetTag = targetTag;
                
                context.Fact(Arg.Any<IFactIdentifier>()).Returns(commerceContext);

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
                cart.Adjustments.Should().BeEmpty();
            }

            [Theory, AutoNSubstituteData]
            public void Execute_04_NoCartTotals(
                IRuleValue<string> targetTag,
                Cart cart,
                CommerceContext commerceContext,
                CartItemTargetTagFreeGiftAction action,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());
                commerceContext.AddObject(cart);

                action.TargetTag = targetTag;

                context.Fact(Arg.Any<IFactIdentifier>()).Returns(commerceContext);

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
                cart.Adjustments.Should().BeEmpty();
            }

            [Theory, AutoNSubstituteData]
            public void Execute_05_NoCartTotalLines(
                IRuleValue<string> targetTag,
                Cart cart,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                CartItemTargetTagFreeGiftAction action,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());
                cartTotals.Lines.Clear();

                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);

                action.TargetTag = targetTag;

                context.Fact(Arg.Any<IFactIdentifier>()).Returns(commerceContext);

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
                cart.Adjustments.Should().BeEmpty();
            }
        }

        public class Functional
        {
            [Theory, InlineAutoNSubstituteData("freegift")]
            public void Execute_HasMatchingLines_ShouldApplyCartLineAdjustment(
                string targetTag,
                bool autoRemove,
                Cart cart,
                CartProductComponent cartProductComponent,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                ApplyFreeGiftDiscountCommand discountCommand;
                ApplyFreeGiftEligibilityCommand eligibilityCommand;
                ApplyFreeGiftAutoRemoveCommand autoRemoveCommand;
                CartItemTargetTagFreeGiftAction action = BuildAction(out discountCommand, out eligibilityCommand, out autoRemoveCommand);


                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);

                cartProductComponent.Tags.Add(new Tag(targetTag));

                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());
                cart.Lines[0].SetComponent(cartProductComponent);

                action.TargetTag = Substitute.For<IRuleValue<string>>();
                action.TargetTag.Yield(context).ReturnsForAnyArgs(targetTag);

                action.AutoRemove = Substitute.For<IRuleValue<bool>>();
                action.AutoRemove.Yield(context).ReturnsForAnyArgs(autoRemove);

                context.Fact(Arg.Any<IFactIdentifier>()).Returns(commerceContext);

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                eligibilityCommand.Received().Process(Arg.Any<CommerceContext>(), cart, Arg.Any<string>());
                discountCommand.Received().Process(Arg.Any<CommerceContext>(), Arg.Any<CartLineComponent>(), Arg.Any<string>());
            }

            [Theory, InlineAutoNSubstituteData("freegift")]
            public void Execute_NoMatchingLines_ShouldNotApplyCartLineAdjustment(
                string targetItemId,
                Cart cart,
                CartTotals cartTotals,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                ApplyFreeGiftDiscountCommand discountCommand;
                ApplyFreeGiftEligibilityCommand eligibilityCommand;
                ApplyFreeGiftAutoRemoveCommand autoRemoveCommand;
                CartItemTargetTagFreeGiftAction action = BuildAction(out discountCommand, out eligibilityCommand, out autoRemoveCommand);


                commerceContext.AddObject(cartTotals);
                commerceContext.AddObject(cart);

                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());

                action.TargetTag = Substitute.For<IRuleValue<string>>();
                action.TargetTag.Yield(context).ReturnsForAnyArgs(targetItemId);

                context.Fact(Arg.Any<IFactIdentifier>()).Returns(commerceContext);

                /**********************************************
                 * Act
                 **********************************************/
                Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
                eligibilityCommand.Received().Process(Arg.Any<CommerceContext>(), cart, Arg.Any<string>());
                discountCommand.DidNotReceive().Process(Arg.Any<CommerceContext>(), Arg.Any<CartLineComponent>(), Arg.Any<string>());
            }

            private static CartItemTargetTagFreeGiftAction BuildAction(out ApplyFreeGiftDiscountCommand discountCommand, out ApplyFreeGiftEligibilityCommand eligibilityCommand, out ApplyFreeGiftAutoRemoveCommand autoRemoveCommand)
            {
                discountCommand = Substitute.For<ApplyFreeGiftDiscountCommand>(Substitute.For<IServiceProvider>());
                eligibilityCommand = Substitute.For<ApplyFreeGiftEligibilityCommand>(Substitute.For<IServiceProvider>());
                autoRemoveCommand = Substitute.For<ApplyFreeGiftAutoRemoveCommand>();

                var commerceCommander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
                commerceCommander.Command<ApplyFreeGiftDiscountCommand>().Returns(discountCommand);
                commerceCommander.Command<ApplyFreeGiftEligibilityCommand>().Returns(eligibilityCommand);
                commerceCommander.Command<ApplyFreeGiftAutoRemoveCommand>().Returns(autoRemoveCommand);

                return new CartItemTargetTagFreeGiftAction(commerceCommander);
            }
        }
    }
}
