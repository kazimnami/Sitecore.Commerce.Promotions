using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using System.Diagnostics.CodeAnalysis;
using Xunit.Sdk;

namespace Feature.Carts.Engine.Tests
{
    public class AutoNSubstituteDataAttribute : AutoDataAttribute
    {
        [ExcludeFromCodeCoverage]
        public AutoNSubstituteDataAttribute()
            : base(() => BaseFixture.Create().Customize(new AutoNSubstituteCustomization()))
        {
        }
    }
}
