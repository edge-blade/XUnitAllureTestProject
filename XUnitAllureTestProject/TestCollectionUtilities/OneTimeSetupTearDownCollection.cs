using Xunit;

namespace XUnitAllureTestProject.TestCollectionUtilities
{
    [CollectionDefinition("Test Suite Collection")]
    public class OneTimeSetupTearDownCollection : ICollectionFixture<OneTimeSetupTearDownFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
