using Microsoft.Extensions.DependencyInjection;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Rules;
using System.Reflection;

namespace SamplePromotions.Foundation.Carts.Engine
{
    public class ConfigureSitecore : IConfigureSitecore
    {
        public void ConfigureServices(IServiceCollection services)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);
            services.RegisterAllCommands(assembly);
            services.Sitecore().Rules(rules => rules.Registry(registry => registry.RegisterAssembly(assembly)));
        }
    }
}