// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInitialisePromotionsPipeline.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SamplePromotions.Project.SamplePromotions.Engine.Pipelines
{
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Pipelines;

    /// <summary>
    /// Defines the IInitialisePromotionsPipeline interface
    /// </summary>
    /// <seealso>
    ///     <cref>
    ///         Sitecore.Framework.Pipelines.IPipeline{string,
    ///         string, Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///     </cref>
    /// </seealso>
    [PipelineDisplayName("SamplePromotions.Pipeline.IInitialisePromotionsPipeline")]
    public interface IInitialisePromotionsPipeline : IPipeline<string, string, CommercePipelineExecutionContext>
    {
    }
}
