using AutoFixture.Xunit2;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Xunit.Sdk;

namespace Foundation.Carts.Engine.Tests
{
    internal class InlineAutoNSubstituteDataAttribute : CompositeDataAttribute
    {
        internal InlineAutoNSubstituteDataAttribute(params object[] values)
            : base(new DataAttribute[] {
            new InlineDataAttribute(values), new AutoNSubstituteDataAttribute() })
        {
        }
    }
}
