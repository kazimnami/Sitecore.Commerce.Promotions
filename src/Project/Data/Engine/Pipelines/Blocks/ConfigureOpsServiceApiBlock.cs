namespace SamplePromotions.Project.SamplePromotions.Engine.Pipelines.Blocks
{
    using Microsoft.AspNetCore.OData.Builder;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Core.Commands;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;
    using System.Threading.Tasks;

    [PipelineDisplayName("SamplePromotions.Block.ConfigureOpsServiceApi")]
    public class ConfigureOpsServiceApiBlock : PipelineBlock<ODataConventionModelBuilder, ODataConventionModelBuilder, CommercePipelineExecutionContext>
    {
        public override Task<ODataConventionModelBuilder> Run(ODataConventionModelBuilder arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The argument can not be null");
            
            arg.Function("InitialiseSamplePromotions").ReturnsFromEntitySet<CommerceCommand>("Commands");

            return Task.FromResult(arg);
        }

        public ConfigureOpsServiceApiBlock()
          : base(null)
        {
        }
    }
}