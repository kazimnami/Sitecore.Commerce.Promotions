using Microsoft.Extensions.DependencyInjection;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Promotions;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;
using Sitecore.Framework.Rules;
using System.Reflection;

namespace SamplePromotions.Feature.Promotions.Engine
{
    public class ConfigureSitecore : IConfigureSitecore
    {
        public void ConfigureServices(IServiceCollection services)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);
            services.RegisterAllCommands(assembly);
            services.Sitecore().Rules(rules => rules
                .Registry(registry => registry
                    .RegisterAssembly(assembly)
                    .ExcludeType<Sitecore.Commerce.Plugin.Promotions.ItemsCollectionCondition>()
                    ));

            services.Sitecore().Pipelines(config => config

                .ConfigurePipeline<IDoActionPipeline>(pipeline => pipeline
                    .Replace<DoActionAddQualificationBlock, Pipelines.Blocks.DoActionAddQualificationBlock>()
                    .Replace<DoActionAddBenefitBlock, Pipelines.Blocks.DoActionAddBenefitBlock>()
                )
				
				.ConfigurePipeline<IPopulateEntityViewActionsPipeline>(builder => builder
					.Add<Pipelines.Blocks.PopulatePromotionViewActionsBlock>().After<PopulatePromotionViewActionsBlock>()
				)

				.ConfigurePipeline<IGetEntityViewPipeline>(builder => builder
					.Add<Pipelines.Blocks.GetPromotionDetailsViewBlock>().After<GetPromotionDetailsViewBlock>()
				)

				.ConfigurePipeline<IEditPromotionPipeline>(builder => builder
					.Replace<EditPromotionBlock, Pipelines.Blocks.EditPromotionBlock>()
				)
			);
        }
    }
}