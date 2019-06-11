// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InitialisePromotionsPipeline.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SamplePromotions.Project.SamplePromotions.Engine.Pipelines
{
    using Microsoft.Extensions.Logging;
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Pipelines;

    /// <inheritdoc />
    /// <summary>
    ///  Defines the InitialisePromotionsPipeline pipeline.
    /// </summary>
    /// <seealso>
    ///     <cref>
    ///         Sitecore.Commerce.Core.CommercePipeline{string,
    ///         string}
    ///     </cref>
    /// </seealso>
    /// <seealso cref="T:Project.SamplePromotions.Engine.Pipelines.InitialisePromotionsPipeline" />
    public class InitialisePromotionsPipeline : CommercePipeline<string, string>, IInitialisePromotionsPipeline
	{
		/// <inheritdoc />
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Project.SamplePromotions.Engine.Pipelines.InitialisePromotionsPipeline" /> class.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		/// <param name="loggerFactory">The logger factory.</param>
		public InitialisePromotionsPipeline(IPipelineConfiguration<IInitialisePromotionsPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}

