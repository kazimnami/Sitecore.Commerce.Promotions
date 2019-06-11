using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplePromotions.Foundation.Carts.Engine.Tests
{
    public static class BaseFixture
    {
        public static Fixture Create()
        { //from https://github.com/AutoFixture/AutoFixture/issues/337
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());//recursionDepth

            return fixture;
        }
    }
}
