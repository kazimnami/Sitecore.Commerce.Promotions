using Feature.Fulfillment.Engine.Rules.Actions;
using FluentAssertions;
using NSubstitute;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Fulfillment;
using Sitecore.Commerce.Plugin.Pricing;
using Sitecore.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Feature.Fulfillment.Engine.Tests
{
	public class CartShippingOptionAmountOffActionFixture
    {
		const decimal MINIMUM_AMOUNT_OFF = 0.01m;

		public class Boundary
        {
            [Theory, AutoNSubstituteData]
            public void Execute_01_NoCommerceContext(
                CartShippingOptionAmountOffAction action,
                IRuleValue<string> fulfillmentOptionName,
                IRuleValue<decimal> amountOff,
                Cart cart,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());

                context.Fact<CommerceContext>().ReturnsForAnyArgs((CommerceContext)null);
                action.FulfillmentOptionName = fulfillmentOptionName;
                action.AmountOff = amountOff;

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
                CartShippingOptionAmountOffAction action,
                IRuleValue<string> fulfillmentOptionName,
                IRuleValue<decimal> amountOff,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                action.FulfillmentOptionName = fulfillmentOptionName;
                action.AmountOff = amountOff;

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
                CartShippingOptionAmountOffAction action,
                IRuleValue<string> fulfillmentOptionName,
                IRuleValue<decimal> amountOff,
                Cart cart,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.Clear();

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
                commerceContext.AddObject(cart);

				action.FulfillmentOptionName = fulfillmentOptionName;
                action.AmountOff = amountOff;

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
			public void Execute_04_NoFulfillmentOptionName(
				CartShippingOptionAmountOffAction action,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CommerceContext commerceContext,
				IRuleExecutionContext context)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				action.FulfillmentOptionName = null;
				action.AmountOff = amountOff;

				/**********************************************
                 * Act
                 **********************************************/
				Action executeAction = () => action.Execute(context);

				/**********************************************
                 * Assert
                 **********************************************/
				executeAction.Should().Throw<Exception>();
				cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
				cart.Adjustments.Should().BeEmpty();
			}

			[Theory, AutoNSubstituteData]
			public void Execute_05_NoAmountOff(
				CartShippingOptionAmountOffAction action,
				IRuleValue<string> fulfillmentOptionName,
				Cart cart,
				CommerceContext commerceContext,
				IRuleExecutionContext context)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				action.FulfillmentOptionName = fulfillmentOptionName;
				action.AmountOff = null;

				/**********************************************
                 * Act
                 **********************************************/
				Action executeAction = () => action.Execute(context);

				/**********************************************
                 * Assert
                 **********************************************/
				executeAction.Should().Throw<Exception>();
				cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
				cart.Adjustments.Should().BeEmpty();
			}

			[Theory, AutoNSubstituteData]
			public void Execute_06_EmptyFulfillmentOptionName(
				CartShippingOptionAmountOffAction action,
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CommerceContext commerceContext,
				IRuleExecutionContext context)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				fulfillmentOptionName.Yield(context).Returns(string.Empty);
				action.FulfillmentOptionName = fulfillmentOptionName;
				action.AmountOff = amountOff;
				
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
            public void Execute_07_ZeroAmountOff(
                CartShippingOptionAmountOffAction action,
                IRuleValue<string> fulfillmentOptionName,
                IRuleValue<decimal> amountOff,
                Cart cart,
                CommerceContext commerceContext,
                IRuleExecutionContext context)
            {
                /**********************************************
                 * Arrange
                 **********************************************/
                cart.Adjustments.Clear();
                cart.Lines.ForEach(l => l.Adjustments.Clear());

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				amountOff.Yield(context).ReturnsForAnyArgs(0);

                action.FulfillmentOptionName = fulfillmentOptionName;
                action.AmountOff = amountOff;

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
			public void Execute_08_NoFulfillmentMethods(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				string fulfillmentOptionNameValue)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				fulfillmentOptionName.Yield(context).ReturnsForAnyArgs(fulfillmentOptionNameValue);
				amountOff.Yield(context).ReturnsForAnyArgs(1);

				var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
				var command = Substitute.For<GetFulfillmentMethodsCommand>(
					Substitute.For<IFindEntityPipeline>(),
					Substitute.For<IGetCartFulfillmentMethodsPipeline>(),
					Substitute.For<IGetCartLineFulfillmentMethodsPipeline>(),
					Substitute.For<IGetFulfillmentMethodsPipeline>(),
					Substitute.For<IServiceProvider>());

				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>().AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var action = new CartShippingOptionAmountOffAction(commander)
				{
					FulfillmentOptionName = fulfillmentOptionName,
					AmountOff = amountOff
				};
				
				/**********************************************
                 * Act
                 **********************************************/
				Action executeAction = () => action.Execute(context);

                /**********************************************
                 * Assert
                 **********************************************/
                executeAction.Should().NotThrow<Exception>();
				command.Received().Process(commerceContext);
				cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
                cart.Adjustments.Should().BeEmpty();
			}

			[Theory, AutoNSubstituteData]
			public void Execute_09_NoCartFulfillmentComponent(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				string fulfillmentOptionNameValue)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				fulfillmentOptionName.Yield(context).ReturnsForAnyArgs(fulfillmentOptionNameValue);
				amountOff.Yield(context).ReturnsForAnyArgs(MINIMUM_AMOUNT_OFF);

				var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
				var command = Substitute.For<GetFulfillmentMethodsCommand>(
					Substitute.For<IFindEntityPipeline>(),
					Substitute.For<IGetCartFulfillmentMethodsPipeline>(),
					Substitute.For<IGetCartLineFulfillmentMethodsPipeline>(),
					Substitute.For<IGetFulfillmentMethodsPipeline>(),
					Substitute.For<IServiceProvider>());

				var fulfillmentMethod = new FulfillmentMethod() { FulfillmentType = fulfillmentOptionNameValue };
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var action = new CartShippingOptionAmountOffAction(commander)
				{
					FulfillmentOptionName = fulfillmentOptionName,
					AmountOff = amountOff
				};
				
				/**********************************************
                 * Act
                 **********************************************/
				Action executeAction = () => action.Execute(context);

				/**********************************************
                 * Assert
                 **********************************************/
				executeAction.Should().NotThrow<Exception>();
				command.Received().Process(commerceContext);
				cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
				cart.Adjustments.Should().BeEmpty();
			}

			[Theory, AutoNSubstituteData]
			public void Execute_10_EmptyFulfillmentMethodEntityTarget(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				string fulfillmentOptionNameValue,
				FulfillmentComponent fulfillmentComponent)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				fulfillmentComponent.FulfillmentMethod.EntityTarget = null;
				cart.SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				fulfillmentOptionName.Yield(context).ReturnsForAnyArgs(fulfillmentOptionNameValue);
				amountOff.Yield(context).ReturnsForAnyArgs(MINIMUM_AMOUNT_OFF);

				var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
				var command = Substitute.For<GetFulfillmentMethodsCommand>(
					Substitute.For<IFindEntityPipeline>(),
					Substitute.For<IGetCartFulfillmentMethodsPipeline>(),
					Substitute.For<IGetCartLineFulfillmentMethodsPipeline>(),
					Substitute.For<IGetFulfillmentMethodsPipeline>(),
					Substitute.For<IServiceProvider>());

				var fulfillmentMethod = new FulfillmentMethod() { FulfillmentType = fulfillmentOptionNameValue };
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var action = new CartShippingOptionAmountOffAction(commander)
				{
					FulfillmentOptionName = fulfillmentOptionName,
					AmountOff = amountOff
				};

				/**********************************************
                 * Act
                 **********************************************/
				Action executeAction = () => action.Execute(context);

				/**********************************************
                 * Assert
                 **********************************************/
				executeAction.Should().NotThrow<Exception>();
				command.Received().Process(commerceContext);
				cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
				cart.Adjustments.Should().BeEmpty();
			}

			[Theory, AutoNSubstituteData]
			public void Execute_11_EmptyFulfillmentMethodName(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				string fulfillmentOptionNameValue,
				FulfillmentComponent fulfillmentComponent)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				fulfillmentComponent.FulfillmentMethod.Name = null;
				cart.SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				fulfillmentOptionName.Yield(context).ReturnsForAnyArgs(fulfillmentOptionNameValue);
				amountOff.Yield(context).ReturnsForAnyArgs(MINIMUM_AMOUNT_OFF);

				var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
				var command = Substitute.For<GetFulfillmentMethodsCommand>(
					Substitute.For<IFindEntityPipeline>(),
					Substitute.For<IGetCartFulfillmentMethodsPipeline>(),
					Substitute.For<IGetCartLineFulfillmentMethodsPipeline>(),
					Substitute.For<IGetFulfillmentMethodsPipeline>(),
					Substitute.For<IServiceProvider>());

				var fulfillmentMethod = new FulfillmentMethod() { FulfillmentType = fulfillmentOptionNameValue };
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var action = new CartShippingOptionAmountOffAction(commander)
				{
					FulfillmentOptionName = fulfillmentOptionName,
					AmountOff = amountOff
				};

				/**********************************************
                 * Act
                 **********************************************/
				Action executeAction = () => action.Execute(context);

				/**********************************************
                 * Assert
                 **********************************************/
				executeAction.Should().NotThrow<Exception>();
				command.Received().Process(commerceContext);
				cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
				cart.Adjustments.Should().BeEmpty();
			}

			[Theory, AutoNSubstituteData]
			public void Execute_12_NoFulfillmentFee(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				string fulfillmentOptionNameValue,
				FulfillmentComponent fulfillmentComponent)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				fulfillmentOptionName.Yield(context).ReturnsForAnyArgs(fulfillmentOptionNameValue);
				amountOff.Yield(context).ReturnsForAnyArgs(MINIMUM_AMOUNT_OFF);

				var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
				var command = Substitute.For<GetFulfillmentMethodsCommand>(
					Substitute.For<IFindEntityPipeline>(),
					Substitute.For<IGetCartFulfillmentMethodsPipeline>(),
					Substitute.For<IGetCartLineFulfillmentMethodsPipeline>(),
					Substitute.For<IGetFulfillmentMethodsPipeline>(),
					Substitute.For<IServiceProvider>());

				var fulfillmentMethod = new FulfillmentMethod() { FulfillmentType = fulfillmentOptionNameValue };
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var action = new CartShippingOptionAmountOffAction(commander)
				{
					FulfillmentOptionName = fulfillmentOptionName,
					AmountOff = amountOff
				};

				/**********************************************
                 * Act
                 **********************************************/
				Action executeAction = () => action.Execute(context);

				/**********************************************
                 * Assert
                 **********************************************/
				executeAction.Should().NotThrow<Exception>();
				command.Received().Process(commerceContext);
				cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
				cart.Adjustments.Should().BeEmpty();
			}

			[Theory, AutoNSubstituteData]
			public void Execute_13_ZeroDiscount(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				string fulfillmentOptionNameValue,
				FulfillmentComponent fulfillmentComponent)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Adjustments.Add(new AwardedAdjustment() { Name = "FulfillmentFee" });
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				fulfillmentOptionName.Yield(context).ReturnsForAnyArgs(fulfillmentOptionNameValue);
				amountOff.Yield(context).ReturnsForAnyArgs(MINIMUM_AMOUNT_OFF);

				var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
				var command = Substitute.For<GetFulfillmentMethodsCommand>(
					Substitute.For<IFindEntityPipeline>(),
					Substitute.For<IGetCartFulfillmentMethodsPipeline>(),
					Substitute.For<IGetCartLineFulfillmentMethodsPipeline>(),
					Substitute.For<IGetFulfillmentMethodsPipeline>(),
					Substitute.For<IServiceProvider>());

				var fulfillmentMethod = new FulfillmentMethod() { FulfillmentType = fulfillmentOptionNameValue };
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var action = new CartShippingOptionAmountOffAction(commander)
				{
					FulfillmentOptionName = fulfillmentOptionName,
					AmountOff = amountOff
				};

				/**********************************************
                 * Act
                 **********************************************/
				Action executeAction = () => action.Execute(context);

				/**********************************************
                 * Assert
                 **********************************************/
				executeAction.Should().NotThrow<Exception>();
				command.Received().Process(commerceContext);
				cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
				cart.Adjustments.Count.Should().Be(1);
			}
		}

		public class Functional
		{
			[Theory, AutoNSubstituteData]
			public void Execute_AmountOffLessThanFulfillmentFee_True(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				string fulfillmentOptionNameValue,
				FulfillmentComponent fulfillmentComponent)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				const decimal FULFILLMENT_FEE = MINIMUM_AMOUNT_OFF + 10m;
				cart.Adjustments.Clear();
				var awardedAdjustment = new AwardedAdjustment() { Name = "FulfillmentFee" };
				awardedAdjustment.Adjustment = new Money(FULFILLMENT_FEE);
				cart.Adjustments.Add(awardedAdjustment);
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				fulfillmentOptionName.Yield(context).ReturnsForAnyArgs(fulfillmentOptionNameValue);
				amountOff.Yield(context).ReturnsForAnyArgs(MINIMUM_AMOUNT_OFF);

				var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
				var command = Substitute.For<GetFulfillmentMethodsCommand>(
					Substitute.For<IFindEntityPipeline>(),
					Substitute.For<IGetCartFulfillmentMethodsPipeline>(),
					Substitute.For<IGetCartLineFulfillmentMethodsPipeline>(),
					Substitute.For<IGetFulfillmentMethodsPipeline>(),
					Substitute.For<IServiceProvider>());

				var fulfillmentMethod = new FulfillmentMethod() { FulfillmentType = fulfillmentOptionNameValue };
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var action = new CartShippingOptionAmountOffAction(commander)
				{
					FulfillmentOptionName = fulfillmentOptionName,
					AmountOff = amountOff
				};

				/**********************************************
                 * Act
                 **********************************************/
				Action executeAction = () => action.Execute(context);

				/**********************************************
                 * Assert
                 **********************************************/
				executeAction.Should().NotThrow<Exception>();
				command.Received().Process(commerceContext);
				cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
				cart.Adjustments.Should().HaveCount(2, "The cart adjustments should only contain the fulfillment fee and shipping discount adjustments.");
				var adjustment = cart.Adjustments.LastOrDefault();
				adjustment.AwardingBlock.Should().Be(nameof(CartShippingOptionAmountOffAction), "Adjustment should be awarded by CartShippingOptionAmountOffAction.");
				adjustment.Adjustment.Amount.Should().Be(-MINIMUM_AMOUNT_OFF, "Adjustment amount should match the AmountOff rule.");
			}

			[Theory, AutoNSubstituteData]
			public void Execute_AmountOffLessThanFulfillmentFee_False(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				string fulfillmentOptionNameValue,
				FulfillmentComponent fulfillmentComponent)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				const decimal FULFILLMENT_FEE = 10m;
				const decimal AMOUNT_OFF = FULFILLMENT_FEE + 1m;
				cart.Adjustments.Clear();
				var awardedAdjustment = new AwardedAdjustment() { Name = "FulfillmentFee" };
				awardedAdjustment.Adjustment = new Money(FULFILLMENT_FEE);
				cart.Adjustments.Add(awardedAdjustment);
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				fulfillmentOptionName.Yield(context).ReturnsForAnyArgs(fulfillmentOptionNameValue);
				amountOff.Yield(context).ReturnsForAnyArgs(AMOUNT_OFF);

				var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
				var command = Substitute.For<GetFulfillmentMethodsCommand>(
					Substitute.For<IFindEntityPipeline>(),
					Substitute.For<IGetCartFulfillmentMethodsPipeline>(),
					Substitute.For<IGetCartLineFulfillmentMethodsPipeline>(),
					Substitute.For<IGetFulfillmentMethodsPipeline>(),
					Substitute.For<IServiceProvider>());

				var fulfillmentMethod = new FulfillmentMethod() { FulfillmentType = fulfillmentOptionNameValue };
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var action = new CartShippingOptionAmountOffAction(commander)
				{
					FulfillmentOptionName = fulfillmentOptionName,
					AmountOff = amountOff
				};

				/**********************************************
                 * Act
                 **********************************************/
				Action executeAction = () => action.Execute(context);

				/**********************************************
                 * Assert
                 **********************************************/
				executeAction.Should().NotThrow<Exception>();
				command.Received().Process(commerceContext);
				cart.Lines.SelectMany(l => l.Adjustments).Should().BeEmpty();
				cart.Adjustments.Should().HaveCount(2, "The cart adjustments should only contain the fulfillment fee and shipping discount adjustments.");
				var adjustment = cart.Adjustments.LastOrDefault();
				adjustment.Adjustment.Amount.Should().Be(-FULFILLMENT_FEE, "Adjustment amount should match the AmountOff rule.");
			}
			
			[Theory, AutoNSubstituteData]
			public void Execute_ShouldRoundPriceCalc_True(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				string fulfillmentOptionNameValue,
				FulfillmentComponent fulfillmentComponent)
			{
				/**********************************************
				 * Arrange
				 **********************************************/
				const decimal AMOUNT_OFF = 0.006m;
				const decimal FULFILLMENT_FEE = AMOUNT_OFF + 10m;
				cart.Adjustments.Clear();
				var awardedAdjustment = new AwardedAdjustment() { Name = "FulfillmentFee" };
				awardedAdjustment.Adjustment = new Money(FULFILLMENT_FEE);
				cart.Adjustments.Add(awardedAdjustment);
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				fulfillmentOptionName.Yield(context).ReturnsForAnyArgs(fulfillmentOptionNameValue);
				amountOff.Yield(context).ReturnsForAnyArgs(AMOUNT_OFF);

				var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
				var command = Substitute.For<GetFulfillmentMethodsCommand>(
					Substitute.For<IFindEntityPipeline>(),
					Substitute.For<IGetCartFulfillmentMethodsPipeline>(),
					Substitute.For<IGetCartLineFulfillmentMethodsPipeline>(),
					Substitute.For<IGetFulfillmentMethodsPipeline>(),
					Substitute.For<IServiceProvider>());

				var fulfillmentMethod = new FulfillmentMethod() { FulfillmentType = fulfillmentOptionNameValue };
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var action = new CartShippingOptionAmountOffAction(commander)
				{
					FulfillmentOptionName = fulfillmentOptionName,
					AmountOff = amountOff
				};

				/**********************************************
			     * Act
			     **********************************************/
				Action executeAction = () => action.Execute(context);

				/**********************************************
			     * Assert
			     **********************************************/
				executeAction.Should().NotThrow<Exception>();
				cart.Lines.SelectMany(l => l.Adjustments).Should().HaveCount(0);
				cart.Adjustments.Should().NotBeEmpty();
				cart.Adjustments.LastOrDefault().Should().NotBeNull();
				cart.Adjustments.LastOrDefault().Should().BeOfType<CartLevelAwardedAdjustment>();
				cart.Adjustments.LastOrDefault().Name.Should().Be(commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount);
				cart.Adjustments.LastOrDefault().DisplayName.Should().Be(commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount);
				cart.Adjustments.LastOrDefault().AdjustmentType.Should().Be(commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount);
				cart.Adjustments.LastOrDefault().AwardingBlock.Should().Contain(nameof(CartShippingOptionAmountOffAction));
				cart.Adjustments.LastOrDefault().IsTaxable.Should().BeFalse();
				cart.Adjustments.LastOrDefault().Adjustment.CurrencyCode.Should().Be(commerceContext.CurrentCurrency());
				cart.Adjustments.LastOrDefault().Adjustment.Amount.Should().Be(-MINIMUM_AMOUNT_OFF);
				cart.HasComponent<MessagesComponent>().Should().BeTrue();
			}

			[Theory, AutoNSubstituteData]
			public void Execute_WithProperties(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				string fulfillmentOptionNameValue,
				FulfillmentComponent fulfillmentComponent)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				const decimal FULFILLMENT_FEE = MINIMUM_AMOUNT_OFF + 10m;
				cart.Adjustments.Clear();
				var awardedAdjustment = new AwardedAdjustment() { Name = "FulfillmentFee" };
				awardedAdjustment.Adjustment = new Money(FULFILLMENT_FEE);
				cart.Adjustments.Add(awardedAdjustment);
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);
				
				var globalPricingPolicy = commerceContext.GetPolicy<GlobalPricingPolicy>();
				globalPricingPolicy.ShouldRoundPriceCalc = false;
				fulfillmentOptionName.Yield(context).ReturnsForAnyArgs(fulfillmentOptionNameValue);
				amountOff.Yield(context).ReturnsForAnyArgs(MINIMUM_AMOUNT_OFF);
				var propertiesModel = new PropertiesModel();
				propertiesModel.Properties.Add("PromotionId", "id");
				propertiesModel.Properties.Add("PromotionCartText", "carttext");
				propertiesModel.Properties.Add("PromotionText", "text");
				commerceContext.AddObject(propertiesModel);

				var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
				var command = Substitute.For<GetFulfillmentMethodsCommand>(
					Substitute.For<IFindEntityPipeline>(),
					Substitute.For<IGetCartFulfillmentMethodsPipeline>(),
					Substitute.For<IGetCartLineFulfillmentMethodsPipeline>(),
					Substitute.For<IGetFulfillmentMethodsPipeline>(),
					Substitute.For<IServiceProvider>());

				var fulfillmentMethod = new FulfillmentMethod() { FulfillmentType = fulfillmentOptionNameValue };
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var action = new CartShippingOptionAmountOffAction(commander)
				{
					FulfillmentOptionName = fulfillmentOptionName,
					AmountOff = amountOff
				};

				/**********************************************
                 * Act
                 **********************************************/
				Action executeAction = () => action.Execute(context);

				/**********************************************
                 * Assert
                 **********************************************/
				executeAction.Should().NotThrow<Exception>();
				cart.Lines.SelectMany(l => l.Adjustments).Should().HaveCount(0);
				cart.Adjustments.Should().NotBeEmpty();
				cart.Adjustments.LastOrDefault().Should().NotBeNull();
				cart.Adjustments.LastOrDefault().Should().BeOfType<CartLevelAwardedAdjustment>();
				cart.Adjustments.LastOrDefault().Name.Should().Be("text");
				cart.Adjustments.LastOrDefault().DisplayName.Should().Be("carttext");
				cart.Adjustments.LastOrDefault().AdjustmentType.Should().Be(commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount);
				cart.Adjustments.LastOrDefault().AwardingBlock.Should().Contain(nameof(CartShippingOptionAmountOffAction));
				cart.Adjustments.LastOrDefault().IsTaxable.Should().BeFalse();
				cart.Adjustments.LastOrDefault().Adjustment.CurrencyCode.Should().Be(commerceContext.CurrentCurrency());
				cart.Adjustments.LastOrDefault().Adjustment.Amount.Should().Be(-MINIMUM_AMOUNT_OFF);
				cart.HasComponent<MessagesComponent>().Should().BeTrue();
			}
		}
	}
}
