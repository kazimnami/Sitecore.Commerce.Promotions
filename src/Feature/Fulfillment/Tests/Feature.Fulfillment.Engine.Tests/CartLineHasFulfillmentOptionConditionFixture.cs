using FluentAssertions;
using NSubstitute;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Fulfillment;
using Sitecore.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using CartLineHasFulfillmentOptionCondition = Feature.Fulfillment.Engine.Rules.Conditions.CartLineHasFulfillmentOptionCondition;

namespace Feature.Fulfillment.Engine.Tests
{
	public class CartLineHasFulfillmentOptionConditionFixture
	{
		public class Boundary
		{
			[Theory, AutoNSubstituteData]
			public void Evaluate_01_NoCommerceContext(
				IRuleValue<string> fulfillmentOptionName,
				IRuleExecutionContext context)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				context.Fact<CommerceContext>().ReturnsForAnyArgs((CommerceContext)null);

				var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
				var condition = new CartLineHasFulfillmentOptionCondition(commander);
				condition.FulfillmentOptionName = fulfillmentOptionName;

				/**********************************************
                 * Act
                 **********************************************/
				var result = condition.Evaluate(context);

				/**********************************************
                 * Assert
                 **********************************************/
				result.Should().BeFalse();
			}

			[Theory, AutoNSubstituteData]
			public void Evaluate_02_NoCart(
				IRuleValue<string> fulfillmentOptionName,
				CommerceContext commerceContext,
				IRuleExecutionContext context)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);

				var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
				var condition = new CartLineHasFulfillmentOptionCondition(commander);
				condition.FulfillmentOptionName = fulfillmentOptionName;

				/**********************************************
                 * Act
                 **********************************************/
				var result = condition.Evaluate(context);

				/**********************************************
                 * Assert
                 **********************************************/
				result.Should().BeFalse();
			}

			[Theory, AutoNSubstituteData]
			public void Evaluate_03_NoCartLines(
				IRuleValue<string> fulfillmentOptionName,
				CommerceContext commerceContext,
				Cart cart,
				IRuleExecutionContext context)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.Lines.Clear();
				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
				var condition = new CartLineHasFulfillmentOptionCondition(commander);
				condition.FulfillmentOptionName = fulfillmentOptionName;

				/**********************************************
                 * Act
                 **********************************************/
				var result = condition.Evaluate(context);

				/**********************************************
                 * Assert
                 **********************************************/
				result.Should().BeFalse();
			}

			[Theory, AutoNSubstituteData]
			public void Evaluate_04_NoCartSplitFulfillmentComponent(
				IRuleValue<string> fulfillmentOptionName,
				CommerceContext commerceContext,
				Cart cart,
				IRuleExecutionContext context)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
				var condition = new CartLineHasFulfillmentOptionCondition(commander);
				condition.FulfillmentOptionName = fulfillmentOptionName;

				/**********************************************
                 * Act
                 **********************************************/
				var result = condition.Evaluate(context);

				/**********************************************
                 * Assert
                 **********************************************/
				result.Should().BeFalse();
			}

			[Theory, AutoNSubstituteData]
			public void Evaluate_05_NoFulfillmentOptionName(
				CommerceContext commerceContext,
				Cart cart,
				IRuleExecutionContext context,
				SplitFulfillmentComponent fulfillmentComponent)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
				var condition = new CartLineHasFulfillmentOptionCondition(commander);
				condition.FulfillmentOptionName = null;

				/**********************************************
                 * Act
                 **********************************************/
				Action evaluateCondition = () => condition.Evaluate(context);

				/**********************************************
                 * Assert
                 **********************************************/
				evaluateCondition.Should().Throw<Exception>();
			}

			[Theory, AutoNSubstituteData]
			public void Evaluate_06_NoFulfillmentMethods(
				IRuleValue<string> fulfillmentOptionName,
				CommerceContext commerceContext,
				Cart cart,
				IRuleExecutionContext context,
				SplitFulfillmentComponent fulfillmentComponent,
				string fulfillmentOptionNameValue)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				fulfillmentOptionName.Yield(context).ReturnsForAnyArgs(fulfillmentOptionNameValue);

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
				var condition = new CartLineHasFulfillmentOptionCondition(commander)
				{
					FulfillmentOptionName = fulfillmentOptionName
				};

				/**********************************************
                 * Act
                 **********************************************/
				var result = condition.Evaluate(context);

				/**********************************************
                 * Assert
                 **********************************************/
				result.Should().BeFalse();
			}

			[Theory, AutoNSubstituteData]
			public void Evaluate_07_NoFulfillmentComponentsOnCartLines(
				IRuleValue<string> fulfillmentOptionName,
				CommerceContext commerceContext,
				Cart cart,
				IRuleExecutionContext context,
				SplitFulfillmentComponent fulfillmentComponent,
				string fulfillmentOptionNameValue,
				IEnumerable<FulfillmentMethod> fulfillmentMethods)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				fulfillmentOptionName.Yield(context).ReturnsForAnyArgs(fulfillmentOptionNameValue);

				var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
				var command = Substitute.For<GetFulfillmentMethodsCommand>(
					Substitute.For<IFindEntityPipeline>(),
					Substitute.For<IGetCartFulfillmentMethodsPipeline>(),
					Substitute.For<IGetCartLineFulfillmentMethodsPipeline>(),
					Substitute.For<IGetFulfillmentMethodsPipeline>(),
					Substitute.For<IServiceProvider>());

				command.Process(commerceContext).ReturnsForAnyArgs(fulfillmentMethods);
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var condition = new CartLineHasFulfillmentOptionCondition(commander)
				{
					FulfillmentOptionName = fulfillmentOptionName
				};

				/**********************************************
                 * Act
                 **********************************************/
				var result = condition.Evaluate(context);

				/**********************************************
                 * Assert
                 **********************************************/
				result.Should().BeFalse();
			}

			[Theory, AutoNSubstituteData]
			public void Evaluate_08_EmptyFulfillmentMethodEntityTarget(
				IRuleValue<string> fulfillmentOptionName,
				CommerceContext commerceContext,
				Cart cart,
				IRuleExecutionContext context,
				SplitFulfillmentComponent splitFulfillmentComponent,
				FulfillmentComponent fulfillmentComponent,
				string fulfillmentOptionNameValue)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				fulfillmentComponent.FulfillmentMethod.EntityTarget = null;
				cart.SetComponent(splitFulfillmentComponent);
				while (cart.Lines.Count > 1)
				{
					cart.Lines.RemoveAt(0);
				}
				cart.Lines[0].SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				fulfillmentOptionName.Yield(context).ReturnsForAnyArgs(fulfillmentOptionNameValue);
				var command = Substitute.For<GetFulfillmentMethodsCommand>(
					Substitute.For<IFindEntityPipeline>(),
					Substitute.For<IGetCartFulfillmentMethodsPipeline>(),
					Substitute.For<IGetCartLineFulfillmentMethodsPipeline>(),
					Substitute.For<IGetFulfillmentMethodsPipeline>(),
					Substitute.For<IServiceProvider>());

				var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
				var fulfillmentMethod = new FulfillmentMethod() { FulfillmentType = fulfillmentOptionNameValue };
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var condition = new CartLineHasFulfillmentOptionCondition(commander)
				{
					FulfillmentOptionName = fulfillmentOptionName
				};

				/**********************************************
                 * Act
                 **********************************************/
				var result = condition.Evaluate(context);

				/**********************************************
                 * Assert
                 **********************************************/
				result.Should().BeFalse();
			}

			[Theory, AutoNSubstituteData]
			public void Evaluate_09_EmptyFulfillmentMethodName(
				IRuleValue<string> fulfillmentOptionName,
				CommerceContext commerceContext,
				Cart cart,
				IRuleExecutionContext context,
				SplitFulfillmentComponent splitFulfillmentComponent,
				FulfillmentComponent fulfillmentComponent,
				string fulfillmentOptionNameValue)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				fulfillmentComponent.FulfillmentMethod.Name = null;
				cart.SetComponent(splitFulfillmentComponent);
				while (cart.Lines.Count > 1)
				{
					cart.Lines.RemoveAt(0);
				}
				cart.Lines[0].SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				fulfillmentOptionName.Yield(context).ReturnsForAnyArgs(fulfillmentOptionNameValue);
				var command = Substitute.For<GetFulfillmentMethodsCommand>(
					Substitute.For<IFindEntityPipeline>(),
					Substitute.For<IGetCartFulfillmentMethodsPipeline>(),
					Substitute.For<IGetCartLineFulfillmentMethodsPipeline>(),
					Substitute.For<IGetFulfillmentMethodsPipeline>(),
					Substitute.For<IServiceProvider>());

				var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
				var fulfillmentMethod = new FulfillmentMethod() { FulfillmentType = fulfillmentOptionNameValue };
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var condition = new CartLineHasFulfillmentOptionCondition(commander)
				{
					FulfillmentOptionName = fulfillmentOptionName
				};

				/**********************************************
                 * Act
                 **********************************************/
				var result = condition.Evaluate(context);

				/**********************************************
                 * Assert
                 **********************************************/
				result.Should().BeFalse();
			}

			[Theory, AutoNSubstituteData]
			public void Evaluate_09_NoMatchingEntityTargets(
				IRuleValue<string> fulfillmentOptionName,
				CommerceContext commerceContext,
				Cart cart,
				IRuleExecutionContext context,
				SplitFulfillmentComponent splitFulfillmentComponent,
				FulfillmentComponent fulfillmentComponent,
				string fulfillmentOptionNameValue)
			{
				/**********************************************
                 * Arrange
                 **********************************************/
				cart.SetComponent(splitFulfillmentComponent);
				cart.Lines[0].SetComponent(fulfillmentComponent);

				context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
				commerceContext.AddObject(cart);

				fulfillmentOptionName.Yield(context).ReturnsForAnyArgs(fulfillmentOptionNameValue);
				var command = Substitute.For<GetFulfillmentMethodsCommand>(
					Substitute.For<IFindEntityPipeline>(),
					Substitute.For<IGetCartFulfillmentMethodsPipeline>(),
					Substitute.For<IGetCartLineFulfillmentMethodsPipeline>(),
					Substitute.For<IGetFulfillmentMethodsPipeline>(),
					Substitute.For<IServiceProvider>());

				var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
				var fulfillmentMethod = new FulfillmentMethod()
				{
					FulfillmentType = fulfillmentOptionNameValue
				};
				command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
				commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
				commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
				var condition = new CartLineHasFulfillmentOptionCondition(commander)
				{
					FulfillmentOptionName = fulfillmentOptionName
				};

				/**********************************************
                 * Act
                 **********************************************/
				var result = condition.Evaluate(context);

				/**********************************************
                 * Assert
                 **********************************************/
				result.Should().BeFalse();
			}

			public class Functional
			{
				[Theory, AutoNSubstituteData]
				public void Evaluate_HasMatchingFulfillmentMethod_False(
					IRuleValue<string> fulfillmentOptionName,
					CommerceContext commerceContext,
					Cart cart,
					IRuleExecutionContext context,
					SplitFulfillmentComponent splitFulfillmentComponent,
					FulfillmentComponent fulfillmentComponent,
					string fulfillmentOptionNameValue)
				{
					/**********************************************
					 * Arrange
					 **********************************************/
					cart.SetComponent(splitFulfillmentComponent);
					cart.Lines[0].SetComponent(fulfillmentComponent);

					context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
					commerceContext.AddObject(cart);

					fulfillmentOptionName.Yield(context).ReturnsForAnyArgs(fulfillmentOptionNameValue);
					var command = Substitute.For<GetFulfillmentMethodsCommand>(
						Substitute.For<IFindEntityPipeline>(),
						Substitute.For<IGetCartFulfillmentMethodsPipeline>(),
						Substitute.For<IGetCartLineFulfillmentMethodsPipeline>(),
						Substitute.For<IGetFulfillmentMethodsPipeline>(),
						Substitute.For<IServiceProvider>());

					var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
					var fulfillmentMethod = new FulfillmentMethod()
					{
						FulfillmentType = fulfillmentOptionNameValue,
						Id = fulfillmentComponent.FulfillmentMethod.EntityTarget
					};
					command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
					commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
					commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
					var condition = new CartLineHasFulfillmentOptionCondition(commander)
					{
						FulfillmentOptionName = fulfillmentOptionName
					};

					/**********************************************
					 * Act
					 **********************************************/
					var result = condition.Evaluate(context);

					/**********************************************
					 * Assert
					 **********************************************/
					result.Should().BeFalse();
				}

				[Theory, AutoNSubstituteData]
				public void Evaluate_HasMatchingFulfillmentMethod_True(
					IRuleValue<string> fulfillmentOptionName,
					CommerceContext commerceContext,
					Cart cart,
					IRuleExecutionContext context,
					SplitFulfillmentComponent splitFulfillmentComponent,
					FulfillmentComponent fulfillmentComponent,
					string fulfillmentOptionNameValue)
				{
					/**********************************************
					 * Arrange
					 **********************************************/
					cart.SetComponent(splitFulfillmentComponent);
					cart.Lines[0].SetComponent(fulfillmentComponent);

					context.Fact<CommerceContext>().ReturnsForAnyArgs(commerceContext);
					commerceContext.AddObject(cart);

					fulfillmentOptionName.Yield(context).ReturnsForAnyArgs(fulfillmentOptionNameValue);
					var command = Substitute.For<GetFulfillmentMethodsCommand>(
						Substitute.For<IFindEntityPipeline>(),
						Substitute.For<IGetCartFulfillmentMethodsPipeline>(),
						Substitute.For<IGetCartLineFulfillmentMethodsPipeline>(),
						Substitute.For<IGetFulfillmentMethodsPipeline>(),
						Substitute.For<IServiceProvider>());

					var commander = Substitute.For<CommerceCommander>(Substitute.For<IServiceProvider>());
					var fulfillmentMethod = new FulfillmentMethod
					{
						FulfillmentType = fulfillmentOptionNameValue,
						Id = fulfillmentComponent.FulfillmentMethod.EntityTarget,
						Name = fulfillmentComponent.FulfillmentMethod.Name
					};
					command.Process(commerceContext).ReturnsForAnyArgs(new List<FulfillmentMethod>() { fulfillmentMethod }.AsEnumerable());
					commander.When(x => x.Command<GetFulfillmentMethodsCommand>()).DoNotCallBase();
					commander.Command<GetFulfillmentMethodsCommand>().Returns(command);
					var condition = new CartLineHasFulfillmentOptionCondition(commander)
					{
						FulfillmentOptionName = fulfillmentOptionName
					};

					/**********************************************
					 * Act
					 **********************************************/
					var result = condition.Evaluate(context);

					/**********************************************
					 * Assert
					 **********************************************/
					result.Should().BeTrue();
				}
			}
		}
	}
}