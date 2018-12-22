using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Framework.Rules;
using System;
using System.Linq;
using Xunit;

namespace Feature.Carts.Engine.Tests
{
    public class CartItemTargetTagSubtotalAmountOffActionFixture
    {
        public static Action<IFixture> AutoSetup() => f =>
        {
            f.Freeze<IRuleExecutionContext>();
            f.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => f.Behaviors.Remove(b));
            f.Behaviors.Add(new OmitOnRecursionBehavior());
        };

        [Theory, AutoNSubstituteData]
        public void Evaluate_False_NullContext(
            CommerceContext commerceContext,
            Cart cart,
            IRuleExecutionContext context,
            CartItemTargetTagSubtotalAmountOffAction action,
            FactIdentifier<CommerceContext> factIdentifier)
        {
            commerceContext.AddObject(cart);
            context.Fact(factIdentifier).ReturnsForAnyArgs((CommerceContext)null);

            action.Execute(context);

            true.Should().BeFalse();
        }

        //[Theory, AutoNSubstituteData]
        //public void Evaluate_False_NullCart(
        //    CommerceContext commerceContext,
        //    IRuleExecutionContext context,
        //    ItemsCollectionCondition condition,
        //    FactIdentifier<CommerceContext> factIdentifier)
        //{
        //    context.Fact(factIdentifier).ReturnsForAnyArgs(commerceContext);

        //    var result = condition.Evaluate(context);

        //    result.Should().BeFalse();
        //}

        //[Theory, AutoNSubstituteData]
        //public void Evaluate_False_NoProperties(
        //    CommerceContext commerceContext,
        //    IRuleExecutionContext context,
        //    Cart cart,
        //    ItemsCollectionCondition condition,
        //    FactIdentifier<CommerceContext> factIdentifier)
        //{
        //    commerceContext.AddObject(cart);
        //    context.Fact(factIdentifier).ReturnsForAnyArgs(commerceContext);

        //    var result = condition.Evaluate(context);

        //    result.Should().BeFalse();
        //}

        //[Theory, AutoNSubstituteData]
        //public void Evaluate_False_NoMathingItems(
        //    CommerceContext commerceContext,
        //    IRuleExecutionContext context,
        //    Cart cart,
        //    ItemsCollectionCondition condition,
        //    FactIdentifier<CommerceContext> factIdentifier)
        //{
        //    var propertiesModel = new PropertiesModel();
        //    var promotions = new PromotionItemsComponent();
        //    promotions.Items.AddRange(new List<PromotionItemModel> { new PromotionItemModel("notamatchingitemid") });
        //    propertiesModel.Properties.Add("PromotionItems", promotions);
        //    commerceContext.AddObject(propertiesModel);
        //    commerceContext.AddObject(cart);
        //    context.Fact(factIdentifier).ReturnsForAnyArgs(commerceContext);

        //    var result = condition.Evaluate(context);

        //    result.Should().BeFalse();
        //}

        //[Theory, AutoNSubstituteData]
        //public void Evaluate_False_ExcludedItems(
        //    CommerceContext commerceContext,
        //    IRuleExecutionContext context,
        //    Cart cart,
        //    ItemsCollectionCondition condition,
        //    FactIdentifier<CommerceContext> factIdentifier)
        //{
        //    var propertiesModel = new PropertiesModel();
        //    var promotion = new PromotionItemsComponent();
        //    promotion.Items.AddRange(new List<PromotionItemModel> { new PromotionItemModel("itemid", true) });
        //    propertiesModel.Properties.Add("PromotionItems", promotion);
        //    commerceContext.AddObject(propertiesModel);
        //    commerceContext.AddObject(cart);
        //    context.Fact(factIdentifier).ReturnsForAnyArgs(commerceContext);

        //    var result = condition.Evaluate(context);

        //    result.Should().BeFalse();
        //}

        //[Theory, AutoNSubstituteData]
        //public void Evaluate_True(CommerceContext commerceContext,
        //    IRuleExecutionContext context,
        //    Cart cart,
        //    ItemsCollectionCondition condition,
        //    FactIdentifier<CommerceContext> factIdentifier)
        //{
        //    var propertiesModel = new PropertiesModel();
        //    var promotion = new PromotionItemsComponent();
        //    promotion.Items.AddRange(new List<PromotionItemModel> { new PromotionItemModel(cart.Lines.FirstOrDefault().ItemId) });
        //    propertiesModel.Properties.Add("PromotionItems", promotion);
        //    commerceContext.AddObject(propertiesModel);
        //    commerceContext.AddObject(cart);
        //    context.Fact(factIdentifier).ReturnsForAnyArgs(commerceContext);

        //    var result = condition.Evaluate(context);

        //    result.Should().BeTrue();
        //}
    }
}