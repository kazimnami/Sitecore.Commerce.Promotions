namespace SamplePromotions.Feature.Coupons.Engine.Tests
{
	using Microsoft.ApplicationInsights;
	using Microsoft.AspNetCore.Http;
	using Microsoft.Extensions.Logging;
	using NSubstitute;
	using NSubstitute.Core;
	using NSubstitute.Routing.Handlers;
	using Sitecore.Commerce.Core;

	public static class TestHelper
	{
		internal static CommercePipelineExecutionContext GetPipelineExecutionContext()
		{
			var list = SubstitutionContext.Current.DequeueAllArgumentSpecifications();
			if (list.Count > 0)
			{
				var handler = new ClearUnusedCallSpecHandler(Substitute.For<IPendingSpecification>());
				handler.Handle(Substitute.For<ICall>());
			}

			var commerceContext = GetCommerceContext();
			var logger = Substitute.For<ILogger>();
			var pipelineContext = Substitute.For<CommercePipelineExecutionContext>(new CommercePipelineExecutionContextOptions(commerceContext), logger);

			return pipelineContext;
		}

		internal static CommerceContext GetCommerceContext()
		{
			var commerceContext = new CommerceContext(Substitute.For<ILogger>(), new TelemetryClient(), Substitute.For<IGetLocalizableMessagePipeline>())
			{
				Headers = new HeaderDictionary
				{
					{
						"ShopName", new[]
						{
							"MyShop"
						}
					},
					{
						"ShopperId", new[]
						{
							"ShopperId"
						}
					}
				}
			};

			var environment = Substitute.For<CommerceEnvironment>();
			environment.Policies.Add(new GeoLocationDefaultsPolicy());
			commerceContext.Environment = environment;
			commerceContext.GlobalEnvironment = environment;
			return commerceContext;
		}
	}
}
