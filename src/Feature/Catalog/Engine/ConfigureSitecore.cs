// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigureSitecore.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SamplePromotions.Feature.Catalog.Engine
{
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;
    using Sitecore.Framework.Configuration;
    using Sitecore.Framework.Pipelines.Definitions.Extensions;

    /// <summary>
    /// The configure sitecore class.
    /// </summary>
    public class ConfigureSitecore : IConfigureSitecore
    {
        /// <summary>
        /// The configure services.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            services.Sitecore().Pipelines(config => config
                .ConfigurePipeline<IDoActionPipeline>(pipeline => pipeline
                    .Add<Pipelines.Blocks.DoActionSelectQualificationBlock>().After<Sitecore.Commerce.Plugin.Promotions.DoActionSelectQualificationBlock>()
                    .Add<Pipelines.Blocks.DoActionSelectBenefitBlock>().After<Sitecore.Commerce.Plugin.Promotions.DoActionSelectBenefitBlock>()
                    .Add<Pipelines.Blocks.DoActionAddQualificationBlock>().Before<Sitecore.Commerce.Plugin.Promotions.DoActionAddQualificationBlock>()
                    .Add<Pipelines.Blocks.DoActionAddBenefitBlock>().Before<Sitecore.Commerce.Plugin.Promotions.DoActionAddBenefitBlock>()
                    .Add<Pipelines.Blocks.DoActionEditQualificationBlock>().Before<Sitecore.Commerce.Plugin.Promotions.DoActionEditQualificationBlock>()
                    .Add<Pipelines.Blocks.DoActionEditBenefitBlock>().Before<Sitecore.Commerce.Plugin.Promotions.DoActionEditBenefitBlock>()
                )

                .ConfigurePipeline<IGetEntityViewPipeline>(pipeline => pipeline
                    .Add<Pipelines.Blocks.GetPromotionQualificationDetailsViewBlock>().After<Sitecore.Commerce.Plugin.Promotions.GetPromotionQualificationDetailsViewBlock>()
                    .Add<Pipelines.Blocks.GetPromotionBenefitDetailsViewBlock>().After<Sitecore.Commerce.Plugin.Promotions.GetPromotionBenefitDetailsViewBlock>()
                )
            );

            services.RegisterAllCommands(assembly);
        }
    }
}