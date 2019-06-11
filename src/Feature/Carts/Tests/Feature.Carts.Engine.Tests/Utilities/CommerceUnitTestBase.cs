namespace SamplePromotions.Feature.Carts.Engine.Tests.Utilities
{
    using System;
    using Microsoft.Extensions.Logging;
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Pipelines;

    public class CommerceUnitTestBase
    {
        public static CommercePipelineExecutionContext CreateCommercePipelineExecutionContext()
        {
            return new CommercePipelineExecutionContext(CreateOptions(), CreateLogger());
        }

        private static ILogger CreateLogger()
        {
            return new NullLogger();
        }

        private static IPipelineExecutionContextOptions CreateOptions()
        {
            return new CommercePipelineExecutionContextOptions(CreateCommerceContext());
        }

        private static CommerceContext CreateCommerceContext()
        {
            var context = new CommerceContext(new NullLogger(), null);

            context.Environment = new CommerceEnvironment();

            return context;
        }
    }

    class NullLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            return new NullDisposable();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
        }
    }

    class NullDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}
