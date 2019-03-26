// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InitialisePromotionsCommand.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2019
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Project.SamplePromotions.Engine.Commands
{
    using Project.SamplePromotions.Engine.Pipelines;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Core.Commands;
    using System;
    using System.Threading.Tasks;

    /// <inheritdoc />
    /// <summary>
    /// Defines the InitialisePromotionsCommand command.
    /// </summary>
    public class InitialisePromotionsCommand : CommerceCommand
    {
        /// <summary>
        /// Gets or sets the commander.
        /// </summary>
        /// <value>
        /// The commander.
        /// </value>
        protected CommerceCommander Commander { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Project.SamplePromotions.Engine.Commands.InitialisePromotionsCommand" /> class.
        /// </summary>
        /// <param name="pipeline">
        /// The pipeline.
        /// </param>
        /// <param name="serviceProvider">The service provider</param>
        public InitialisePromotionsCommand(CommerceCommander commander, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.Commander = commander;
        }

        /// <summary>
        /// The process of the command
        /// </summary>
        /// <param name="commerceContext">
        /// The commerce context
        /// </param>
        /// <param name="parameter">
        /// The parameter for the command
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<object> Process(CommerceContext commerceContext)
        {
            object result = null;

            using (var activity = CommandActivity.Start(commerceContext, this))
            {
                await PerformTransaction(commerceContext, async () =>
                {
                    /* Replace logic here */
                    result = await Commander.Pipeline<IInitialisePromotionsPipeline>().Run("override", commerceContext.GetPipelineContextOptions());
                });
            }

            return result;
        }
    }
}