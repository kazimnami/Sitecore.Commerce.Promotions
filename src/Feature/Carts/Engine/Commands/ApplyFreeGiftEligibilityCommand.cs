// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplyFreeGiftEligibilityCommand.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2019
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Feature.Carts.Engine.Components;
using JetBrains.Annotations;
using Sitecore.Commerce.Plugin.Carts;

namespace Feature.Carts.Engine.Commands
{
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Core.Commands;
    using System;
    using System.Threading.Tasks;

    /// <inheritdoc />
    /// <summary>
    /// Defines the ApplyFreeGiftEligibilityCommandCommand command.
    /// </summary>
    public class ApplyFreeGiftEligibilityCommand : CommerceCommand, IApplyFreeGiftEligibilityCommand
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Feature.Carts.Engine.Commands.ApplyFreeGiftEligibilityCommandCommand" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        public ApplyFreeGiftEligibilityCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
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
        public void Process(CommerceContext commerceContext, [NotNull] Cart cart, [NotNull] string awardingAction)
        {
            using (CommandActivity.Start(commerceContext, this))
            {
                var propertiesModel = commerceContext.GetObject<PropertiesModel>();
                var freeGiftEligibilityComponent = cart.GetComponent<FreeGiftEligibilityComponent>();

                freeGiftEligibilityComponent.AwardingAction = awardingAction;
                freeGiftEligibilityComponent.PromotionId = propertiesModel?.GetPropertyValue("PromotionId") as string;
            }
        }

        public ApplyFreeGiftEligibilityCommand Command()
        {
            throw new NotImplementedException();
        }
    }
}