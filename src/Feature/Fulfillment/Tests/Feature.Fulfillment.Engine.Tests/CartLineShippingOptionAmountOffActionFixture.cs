using SamplePromotions.Feature.Fulfillment.Engine.Rules.Actions;
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

namespace SamplePromotions.Feature.Fulfillment.Engine.Tests
{
	public class CartLineShippingOptionAmountOffActionFixture
	{
		const decimal MINIMUM_AMOUNT_OFF = 0.01m;

		public class Boundary
		{
			[Theory, AutoNSubstituteData]
			public void Execute_01_NoCommerceContext(
				CartLineShippingOptionAmountOffAction action,
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
				CartLineShippingOptionAmountOffAction action,
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
				CartLineShippingOptionAmountOffAction action,
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
			public void Execute_04_NoCartSplitFulfillmentComponent(
				CartLineShippingOptionAmountOffAction action,
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
			public void Execute_05_NoCartTotals(
				CartLineShippingOptionAmountOffAction action,
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				SplitFulfillmentComponent fulfillmentComponent)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(fulfillmentComponent);

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
			public void Execute_06_NoCartTotalLines(
				CartLineShippingOptionAmountOffAction action,
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CartTotals cartTotals,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				SplitFulfillmentComponent fulfillmentComponent)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cartTotals.Lines.Clear();
				cart.SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cartTotals);
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
			public void Execute_07_NoFulfillmentOptionName(
				CartLineShippingOptionAmountOffAction action,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CartTotals cartTotals,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				SplitFulfillmentComponent fulfillmentComponent)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cartTotals);
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
			public void Execute_08_NoAmountOff(
				CartLineShippingOptionAmountOffAction action,
				IRuleValue<string> fulfillmentOptionName,
				Cart cart,
				CartTotals cartTotals,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				SplitFulfillmentComponent fulfillmentComponent)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cartTotals);
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
			public void Execute_09_EmptyFulfillmentOptionName(
				CartLineShippingOptionAmountOffAction action,
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CartTotals cartTotals,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				SplitFulfillmentComponent fulfillmentComponent)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cartTotals);
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
			public void Execute_10_ZeroAmountOff(
				CartLineShippingOptionAmountOffAction action,
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CartTotals cartTotals,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				SplitFulfillmentComponent fulfillmentComponent)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cartTotals);
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
			public void Execute_11_NoFulfillmentMethods(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CartTotals cartTotals,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				SplitFulfillmentComponent fulfillmentComponent,
				string fulfillmentOptionNameValue)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cartTotals);
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
				var action = new CartLineShippingOptionAmountOffAction(commander)
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
			public void Execute_12_NoFulfillmentComponentsOnCartLines(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CartTotals cartTotals,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				SplitFulfillmentComponent fulfillmentComponent,
				string fulfillmentOptionNameValue)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cartTotals);
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
				var action = new CartLineShippingOptionAmountOffAction(commander)
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
			public void Execute_13_EmptyFulfillmentMethodEntityTarget(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CartTotals cartTotals,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				SplitFulfillmentComponent splitFulfillmentComponent,
				string fulfillmentOptionNameValue,
				FulfillmentComponent fulfillmentComponent)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(splitFulfillmentComponent);
				while (cart.Lines.Count > 1)
				{
					cart.Lines.RemoveAt(0);
				}
				fulfillmentComponent.FulfillmentMethod.EntityTarget = null;
				cart.Lines[0].SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cartTotals);
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
				var action = new CartLineShippingOptionAmountOffAction(commander)
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
			public void Execute_14_EmptyFulfillmentMethodName(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CartTotals cartTotals,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				SplitFulfillmentComponent splitFulfillmentComponent,
				string fulfillmentOptionNameValue,
				FulfillmentComponent fulfillmentComponent)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(splitFulfillmentComponent);
				while (cart.Lines.Count > 1)
				{
					cart.Lines.RemoveAt(0);
				}
				fulfillmentComponent.FulfillmentMethod.Name = null;
				cart.Lines[0].SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cartTotals);
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
				var action = new CartLineShippingOptionAmountOffAction(commander)
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
			public void Execute_15_NoMatchingEntityTargets(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CartTotals cartTotals,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				SplitFulfillmentComponent splitFulfillmentComponent,
				string fulfillmentOptionNameValue,
				FulfillmentComponent fulfillmentComponent)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(splitFulfillmentComponent);
				while (cart.Lines.Count > 1)
				{
					cart.Lines.RemoveAt(0);
				}
				cart.Lines[0].SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cartTotals);
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
				var action = new CartLineShippingOptionAmountOffAction(commander)
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
			public void Execute_16_NoMatchingMethodNames(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CartTotals cartTotals,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				SplitFulfillmentComponent splitFulfillmentComponent,
				string fulfillmentOptionNameValue,
				FulfillmentComponent fulfillmentComponent)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(splitFulfillmentComponent);
				while (cart.Lines.Count > 1)
				{
					cart.Lines.RemoveAt(0);
				}
				cart.Lines[0].SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cartTotals);
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

				var fulfillmentMethod = new FulfillmentMethod()
				{
					FulfillmentType = fulfillmentOptionNameValue,
					Id = fulfillmentComponent.FulfillmentMethod.EntityTarget
				};
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var action = new CartLineShippingOptionAmountOffAction(commander)
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
			public void Execute_17_NoLineIdInTotalsLines(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CartTotals cartTotals,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				SplitFulfillmentComponent splitFulfillmentComponent,
				string fulfillmentOptionNameValue,
				FulfillmentComponent fulfillmentComponent)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(splitFulfillmentComponent);
				while (cart.Lines.Count > 1)
				{
					cart.Lines.RemoveAt(0);
				}
				cart.Lines[0].SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cartTotals);
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

				var fulfillmentMethod = new FulfillmentMethod()
				{
					FulfillmentType = fulfillmentOptionNameValue,
					Id = fulfillmentComponent.FulfillmentMethod.EntityTarget,
					Name = fulfillmentComponent.FulfillmentMethod.Name
				};
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var action = new CartLineShippingOptionAmountOffAction(commander)
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
			public void Execute_18_LineHasNoPurchaseOptionMoneyPolicy(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CartTotals cartTotals,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				SplitFulfillmentComponent splitFulfillmentComponent,
				string fulfillmentOptionNameValue,
				FulfillmentComponent fulfillmentComponent,
				Totals totals)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(splitFulfillmentComponent);
				while (cart.Lines.Count > 1)
				{
					cart.Lines.RemoveAt(0);
				}
				var cartLine = cart.Lines[0];
				cartLine.SetComponent(fulfillmentComponent);
				cartTotals.Lines.Add(cartLine.Id, totals);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cartTotals);
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

				var fulfillmentMethod = new FulfillmentMethod()
				{
					FulfillmentType = fulfillmentOptionNameValue,
					Id = fulfillmentComponent.FulfillmentMethod.EntityTarget,
					Name = fulfillmentComponent.FulfillmentMethod.Name
				};
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var action = new CartLineShippingOptionAmountOffAction(commander)
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
			public void Execute_19_NoFulfillmentFee(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CartTotals cartTotals,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				SplitFulfillmentComponent splitFulfillmentComponent,
				string fulfillmentOptionNameValue,
				FulfillmentComponent fulfillmentComponent,
				Totals totals)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(splitFulfillmentComponent);
				while (cart.Lines.Count > 1)
				{
					cart.Lines.RemoveAt(0);
				}
				var cartLine = cart.Lines[0];
				cartLine.SetComponent(fulfillmentComponent);
				cartTotals.Lines.Add(cartLine.Id, totals);
				cartLine.SetPolicy(new PurchaseOptionMoneyPolicy());

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cartTotals);
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

				var fulfillmentMethod = new FulfillmentMethod()
				{
					FulfillmentType = fulfillmentOptionNameValue,
					Id = fulfillmentComponent.FulfillmentMethod.EntityTarget,
					Name = fulfillmentComponent.FulfillmentMethod.Name
				};
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var action = new CartLineShippingOptionAmountOffAction(commander)
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
			//
			[Theory, AutoNSubstituteData]
			public void Execute_13_ZeroDiscount(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CartTotals cartTotals,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				SplitFulfillmentComponent splitFulfillmentComponent,
				string fulfillmentOptionNameValue,
				FulfillmentComponent fulfillmentComponent,
				Totals totals)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(splitFulfillmentComponent);
				while (cart.Lines.Count > 1)
				{
					cart.Lines.RemoveAt(0);
				}
				var cartLine = cart.Lines[0];
				cartLine.SetComponent(fulfillmentComponent);
				cartTotals.Lines.Add(cartLine.Id, totals);
				cartLine.SetPolicy(new PurchaseOptionMoneyPolicy());
				cartLine.Adjustments.Add(new AwardedAdjustment() { Name = "FulfillmentFee" });

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cartTotals);
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

				var fulfillmentMethod = new FulfillmentMethod()
				{
					FulfillmentType = fulfillmentOptionNameValue,
					Id = fulfillmentComponent.FulfillmentMethod.EntityTarget,
					Name = fulfillmentComponent.FulfillmentMethod.Name
				};
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var action = new CartLineShippingOptionAmountOffAction(commander)
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
				cart.Lines.SelectMany(l => l.Adjustments).Should().HaveCount(1);
				cart.Adjustments.Should().BeEmpty();
			}
		}

		public class Functional
		{
			[Theory, AutoNSubstituteData]
			public void Execute_AmountOffLessThanFulfillmentFee_True(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CartTotals cartTotals,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				SplitFulfillmentComponent splitFulfillmentComponent,
				string fulfillmentOptionNameValue,
				FulfillmentComponent fulfillmentComponent,
				Totals totals)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				const decimal FULFILLMENT_FEE = MINIMUM_AMOUNT_OFF + 10m;
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(splitFulfillmentComponent);
				while (cart.Lines.Count > 1)
				{
					cart.Lines.RemoveAt(0);
				}
				var cartLine = cart.Lines[0];
				cartLine.SetComponent(fulfillmentComponent);
				cartTotals.Lines.Add(cartLine.Id, totals);
				cartLine.SetPolicy(new PurchaseOptionMoneyPolicy());
				var awardedAdjustment = new AwardedAdjustment
				{
					Name = "FulfillmentFee",
					Adjustment = new Money(FULFILLMENT_FEE)
				};
				cartLine.Adjustments.Add(awardedAdjustment);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cartTotals);
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

				var fulfillmentMethod = new FulfillmentMethod()
				{
					FulfillmentType = fulfillmentOptionNameValue,
					Id = fulfillmentComponent.FulfillmentMethod.EntityTarget,
					Name = fulfillmentComponent.FulfillmentMethod.Name
				};
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var action = new CartLineShippingOptionAmountOffAction(commander)
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
				cart.Lines.SelectMany(l => l.Adjustments).Should().HaveCount(2, "The cart line adjustments should only contain the fulfillment fee and shipping discount adjustments.");
				cart.Adjustments.Should().BeEmpty();
				var adjustment = cartLine.Adjustments.LastOrDefault();
				adjustment.AwardingBlock.Should().Be(nameof(CartLineShippingOptionAmountOffAction), "Adjustment should be awarded by CartLineShippingOptionAmountOffAction.");
				adjustment.Adjustment.Amount.Should().Be(-MINIMUM_AMOUNT_OFF, "Adjustment amount should match the AmountOff rule.");
			}

			[Theory, AutoNSubstituteData]
			public void Execute_AmountOffLessThanFulfillmentFee_False(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CartTotals cartTotals,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				SplitFulfillmentComponent splitFulfillmentComponent,
				string fulfillmentOptionNameValue,
				FulfillmentComponent fulfillmentComponent,
				Totals totals)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				const decimal FULFILLMENT_FEE = 10m;
				const decimal AMOUNT_OFF = FULFILLMENT_FEE + 1m;
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(splitFulfillmentComponent);
				while (cart.Lines.Count > 1)
				{
					cart.Lines.RemoveAt(0);
				}
				var cartLine = cart.Lines[0];
				cartLine.SetComponent(fulfillmentComponent);
				cartTotals.Lines.Add(cartLine.Id, totals);
				cartLine.SetPolicy(new PurchaseOptionMoneyPolicy());
				var awardedAdjustment = new AwardedAdjustment
				{
					Name = "FulfillmentFee",
					Adjustment = new Money(FULFILLMENT_FEE)
				};
				cartLine.Adjustments.Add(awardedAdjustment);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cartTotals);
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

				var fulfillmentMethod = new FulfillmentMethod()
				{
					FulfillmentType = fulfillmentOptionNameValue,
					Id = fulfillmentComponent.FulfillmentMethod.EntityTarget,
					Name = fulfillmentComponent.FulfillmentMethod.Name
				};
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var action = new CartLineShippingOptionAmountOffAction(commander)
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
				cart.Lines.SelectMany(l => l.Adjustments).Should().HaveCount(2, "The cart line adjustments should only contain the fulfillment fee and shipping discount adjustments.");
				cart.Adjustments.Should().BeEmpty();
				var adjustment = cartLine.Adjustments.LastOrDefault();
				adjustment.Adjustment.Amount.Should().Be(-FULFILLMENT_FEE, "Adjustment amount should match the AmountOff rule.");
			}

			[Theory, AutoNSubstituteData]
			public void Execute_ShouldRoundPriceCalc_True(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CartTotals cartTotals,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				SplitFulfillmentComponent splitFulfillmentComponent,
				string fulfillmentOptionNameValue,
				FulfillmentComponent fulfillmentComponent,
				Totals totals)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				const decimal AMOUNT_OFF = 0.006m;
				const decimal FULFILLMENT_FEE = AMOUNT_OFF + 10m;
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(splitFulfillmentComponent);
				while (cart.Lines.Count > 1)
				{
					cart.Lines.RemoveAt(0);
				}
				var cartLine = cart.Lines[0];
				cartLine.SetComponent(fulfillmentComponent);
				cartTotals.Lines.Add(cartLine.Id, totals);
				cartLine.SetPolicy(new PurchaseOptionMoneyPolicy());
				var awardedAdjustment = new AwardedAdjustment
				{
					Name = "FulfillmentFee",
					Adjustment = new Money(FULFILLMENT_FEE)
				};
				cartLine.Adjustments.Add(awardedAdjustment);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cartTotals);
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

				var fulfillmentMethod = new FulfillmentMethod()
				{
					FulfillmentType = fulfillmentOptionNameValue,
					Id = fulfillmentComponent.FulfillmentMethod.EntityTarget,
					Name = fulfillmentComponent.FulfillmentMethod.Name
				};
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var action = new CartLineShippingOptionAmountOffAction(commander)
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
				cart.Adjustments.Should().BeEmpty();
				cart.Lines.SelectMany(l => l.Adjustments).Should().HaveCount(2, "The cart line adjustments should only contain the fulfillment fee and shipping discount adjustments.");
				cartLine.Adjustments.LastOrDefault().Should().NotBeNull();
				cartLine.Adjustments.LastOrDefault().Should().BeOfType<CartLineLevelAwardedAdjustment>();
				cartLine.Adjustments.LastOrDefault().Name.Should().Be(commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount);
				cartLine.Adjustments.LastOrDefault().DisplayName.Should().Be(commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount);
				cartLine.Adjustments.LastOrDefault().AdjustmentType.Should().Be(commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount);
				cartLine.Adjustments.LastOrDefault().AwardingBlock.Should().Contain(nameof(CartLineShippingOptionAmountOffAction));
				cartLine.Adjustments.LastOrDefault().IsTaxable.Should().BeFalse();
				cartLine.Adjustments.LastOrDefault().Adjustment.CurrencyCode.Should().Be(commerceContext.CurrentCurrency());
				cartLine.Adjustments.LastOrDefault().Adjustment.Amount.Should().Be(-MINIMUM_AMOUNT_OFF);
				cartLine.HasComponent<MessagesComponent>().Should().BeTrue();
			}

			[Theory, AutoNSubstituteData]
			public void Execute_WithProperties(
				IRuleValue<string> fulfillmentOptionName,
				IRuleValue<decimal> amountOff,
				Cart cart,
				CartTotals cartTotals,
				CommerceContext commerceContext,
				IRuleExecutionContext context,
				SplitFulfillmentComponent splitFulfillmentComponent,
				string fulfillmentOptionNameValue,
				FulfillmentComponent fulfillmentComponent,
				Totals totals)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				const decimal FULFILLMENT_FEE = MINIMUM_AMOUNT_OFF + 10m;
				cart.Adjustments.Clear();
				cart.Lines.ForEach(l => l.Adjustments.Clear());
				cart.SetComponent(splitFulfillmentComponent);
				while (cart.Lines.Count > 1)
				{
					cart.Lines.RemoveAt(0);
				}
				var cartLine = cart.Lines[0];
				cartLine.SetComponent(fulfillmentComponent);
				cartTotals.Lines.Add(cartLine.Id, totals);
				cartLine.SetPolicy(new PurchaseOptionMoneyPolicy());
				var awardedAdjustment = new AwardedAdjustment
				{
					Name = "FulfillmentFee",
					Adjustment = new Money(FULFILLMENT_FEE)
				};
				cartLine.Adjustments.Add(awardedAdjustment);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cartTotals);
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

				var fulfillmentMethod = new FulfillmentMethod()
				{
					FulfillmentType = fulfillmentOptionNameValue,
					Id = fulfillmentComponent.FulfillmentMethod.EntityTarget,
					Name = fulfillmentComponent.FulfillmentMethod.Name
				};
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var action = new CartLineShippingOptionAmountOffAction(commander)
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
				cart.Adjustments.Should().BeEmpty();
				cart.Lines.SelectMany(l => l.Adjustments).Should().HaveCount(2, "The cart line adjustments should only contain the fulfillment fee and shipping discount adjustments.");
				cartLine.Adjustments.LastOrDefault().Should().NotBeNull();
				cartLine.Adjustments.LastOrDefault().Should().BeOfType<CartLineLevelAwardedAdjustment>();
				cartLine.Adjustments.LastOrDefault().Name.Should().Be("text");
				cartLine.Adjustments.LastOrDefault().DisplayName.Should().Be("carttext");
				cartLine.Adjustments.LastOrDefault().AdjustmentType.Should().Be(commerceContext.GetPolicy<KnownCartAdjustmentTypesPolicy>().Discount);
				cartLine.Adjustments.LastOrDefault().AwardingBlock.Should().Contain(nameof(CartLineShippingOptionAmountOffAction));
				cartLine.Adjustments.LastOrDefault().IsTaxable.Should().BeFalse();
				cartLine.Adjustments.LastOrDefault().Adjustment.CurrencyCode.Should().Be(commerceContext.CurrentCurrency());
				cartLine.Adjustments.LastOrDefault().Adjustment.Amount.Should().Be(-MINIMUM_AMOUNT_OFF);
				cartLine.HasComponent<MessagesComponent>().Should().BeTrue();
			}
		}
	}
}
