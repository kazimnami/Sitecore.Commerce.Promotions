using AutoFixture.Xunit2;
using Xunit;
using Xunit.Sdk;

namespace Feature.Carts.Engine.Tests
{
    using System.Collections.Generic;

    internal class InlineAutoNSubstituteDataAttribute : CompositeDataAttribute
    {
        public InlineAutoNSubstituteDataAttribute(params object[] values)
            : base(new DataAttribute[] {
            new InlineDataAttribute(values), new AutoNSubstituteDataAttribute() })
        {
        }
    }
}
