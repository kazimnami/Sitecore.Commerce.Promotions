﻿using Microsoft.Extensions.DependencyInjection;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;
using Sitecore.Framework.Rules;
using System.Reflection;

namespace SamplePromotions.Feature.Customers.Engine
{
    public class ConfigureSitecore : IConfigureSitecore
    {
        public void ConfigureServices(IServiceCollection services)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);
            services.RegisterAllCommands(assembly);
            services.Sitecore().Rules(rules => rules.Registry(registry => registry.RegisterAssembly(assembly)));
            services.Sitecore().Pipelines(config => config
                .ConfigurePipeline<ICalculateCartLinesPipeline>(pipeline =>
                {
                    pipeline.Add<PopulateCartContactBehaviourComponentBlock>().After<ClearCartLinesBlock>();
                }, order: 2000)
            );
        }
    }
}