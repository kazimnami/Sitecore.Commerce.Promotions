

namespace SamplePromotions.Foundation.Rules.Engine.Pipelines.Blocks
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Commerce.Plugin.Rules;
    using Sitecore.Framework.Rules;
    using Sitecore.Framework.Rules.Registry;
    using System;
    using System.Linq;

    public class BuildRuleSetBlock : Sitecore.Commerce.Plugin.Rules.BuildRuleSetBlock
    {
        private readonly IEntityRegistry _entityRegistry;
        private IRuleBuilderInit _ruleBuilder;
        private readonly IServiceProvider _services;

        public BuildRuleSetBlock(IEntityRegistry entityRegistry, IServiceProvider services)
            : base(entityRegistry, services)
        {
            this._entityRegistry = entityRegistry;
            this._services = services;
        }

        protected override IRule BuildRule(RuleModel model)
        {
            _ruleBuilder = _services.GetService<IRuleBuilderInit>();
            var model1 = model.Conditions.First();
            var metadata1 = this._entityRegistry.GetMetadata(model1.LibraryId);
            var ruleBuilder = this._ruleBuilder.When(Engine.ModelExtensions.ConvertToCondition(model1, metadata1, this._entityRegistry, this._services));
            for (var index = 1; index < model.Conditions.Count; ++index)
            {
                var condition1 = model.Conditions[index];
                var metadata2 = this._entityRegistry.GetMetadata(condition1.LibraryId);
                var condition2 = Engine.ModelExtensions.ConvertToCondition(model1, metadata1, this._entityRegistry, this._services);
                if (!string.IsNullOrEmpty(condition1.ConditionOperator))
                {
                    if (condition1.ConditionOperator.ToUpper() == "OR")
                    {
                        ruleBuilder.Or(condition2);
                    }
                    else
                    {
                        ruleBuilder.And(condition2);
                    }
                }
            }

            foreach (var thenAction in model.ThenActions)
            {
                var metadata2 = this._entityRegistry.GetMetadata(thenAction.LibraryId);
                var action = Engine.ModelExtensions.ConvertToAction(thenAction, metadata2, this._entityRegistry, this._services);
                ruleBuilder.Then(action);
            }

            foreach (var elseAction in model.ElseActions)
            {
                var metadata2 = this._entityRegistry.GetMetadata(elseAction.LibraryId);
                var action = Engine.ModelExtensions.ConvertToAction(elseAction, metadata2, this._entityRegistry, this._services);
                ruleBuilder.Else(action);
            }

            return ruleBuilder.ToRule();
        }
    }
}
