using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit;
using System.Diagnostics.CodeAnalysis;
using Xunit.Sdk;

namespace Feature.Promotions.Engine.Tests
{
    internal class AutoNSubstituteDataAttribute : AutoDataAttribute
    {
        [ExcludeFromCodeCoverage]
        internal AutoNSubstituteDataAttribute()
            : base(() => new Fixture().Customize(new AutoNSubstituteCustomization()))
        {
        }
    }
}
