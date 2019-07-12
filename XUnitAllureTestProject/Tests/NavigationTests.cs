using Xunit;
using Xunit.Abstractions;
using XUnitAllureTestProject.TestCollectionUtilities;

namespace XUnitAllureTestProject.Tests
{
    [Collection("Test Suite Collection")]
    [Trait("Category", "Navigation")]
    public class NavigationTests : BaseTest
    {
        public NavigationTests(OneTimeSetupTearDownFixture fixture, ITestOutputHelper outputHelper): base(fixture, outputHelper) { }
        

        [Fact]
        public void NavigateToGoogle()
        {
            _testSuite.driver.Navigate().GoToUrl("www.google.com");
        }
    }
}
