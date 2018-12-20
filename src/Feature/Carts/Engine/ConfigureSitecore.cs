﻿using Microsoft.Extensions.DependencyInjection;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;
using Sitecore.Framework.Rules;
using System.Reflection;

namespace Feature.Carts.Engine
{
    public class ConfigureSitecore : IConfigureSitecore
    {
        public void ConfigureServices(IServiceCollection services)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);
            services.RegisterAllCommands(assembly);
            services.Sitecore().Rules(config => config.Registry(registry => registry.RegisterAssembly(assembly)));
            services.Sitecore().Pipelines(config => config
                .ConfigurePipeline<IPopulateLineItemPipeline>(pipeline =>
                {
                    pipeline.Add<PopulateLineItemProductExtendedBlock>().After<PopulateLineItemProductBlock>();
                }, order: 2000)
            );
        }
    }
}