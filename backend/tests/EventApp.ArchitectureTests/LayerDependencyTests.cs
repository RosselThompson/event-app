using NetArchTest.Rules;

namespace EventApp.ArchitectureTests
{

    public class LayerDependencyTests
    {
        private const string ApiNamespace = "EventApp.Api";
        private const string ApplicationNamespace = "EventApp.Application";
        private const string DomainNamespace = "EventApp.Domain";
        private const string InfrastructureNamespace = "EventApp.Infrastructure";

        [Fact]
        public void Domain_Should_Not_Depend_On_Other_Layers()
        {
            TestResult result = Types
            .InAssembly(typeof(EventApp.Domain.AssemblyReference).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                ApiNamespace,
                ApplicationNamespace,
                InfrastructureNamespace)
            .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Application_Should_Not_Depend_On_Api_Or_Infrastructure()
        {
            TestResult result = Types
                .InAssembly(typeof(EventApp.Application.AssemblyReference).Assembly)
                .ShouldNot()
                .HaveDependencyOnAny(
                    ApiNamespace,
                    InfrastructureNamespace)
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void Infrastructure_Should_Not_Depend_On_Api()
        {
            TestResult result = Types
                .InAssembly(typeof(EventApp.Infrastructure.AssemblyReference).Assembly)
                .ShouldNot()
                .HaveDependencyOn(ApiNamespace)
                .GetResult();

            Assert.True(result.IsSuccessful);
        }

    }

}
