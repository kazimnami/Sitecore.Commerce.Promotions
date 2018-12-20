using AutoFixture.Xunit;
using System.Diagnostics.CodeAnalysis;
using Xunit.Extensions;

namespace Feature.Carts.Engine.Tests
{
    internal class InlineAutoNSubstituteDataAttribute : CompositeDataAttribute
    {
        [ExcludeFromCodeCoverage]
        internal InlineAutoNSubstituteDataAttribute(params object[] values)
            : base(new DataAttribute[] {
            new InlineDataAttribute(values), new AutoNSubstituteDataAttribute() })
        {
        }
    }
}
