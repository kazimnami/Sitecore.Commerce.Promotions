using System;
using System.Threading.Tasks;
using System.Transactions;
using JetBrains.Annotations;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;

namespace Feature.Carts.Engine.Commands
{
    public interface IApplyFreeGiftDiscountCommand
    {
        /// <summary>
        /// The process of the command
        /// </summary>
        /// <param name="commerceContext">
        /// The commerce context
        /// </param>
        /// <param name="cartLineComponent"></param>
        /// <param name="awardingAction"></param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        void Process(CommerceContext commerceContext, [NotNull] CartLineComponent cartLineComponent, [NotNull] string awardingAction);
        Task ProcessWithTransaction(CommerceContext commerceContext, Func<Task> action);
        Task Process(CommerceContext commerceContext, Func<Task> action);
        Task PerformTransaction(CommerceContext commerceContext, Func<Task> action);
        void ValidateTransaction(CommerceContext context, TransactionScope transaction);
    }
}