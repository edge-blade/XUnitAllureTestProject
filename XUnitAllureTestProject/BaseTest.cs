using Allure.Commons;
using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using XUnitAllureTestProject.TestCollectionUtilities;

namespace XUnitAllureTestProject
{
    [Collection("Test Suite Collection")]
    public class BaseTest : IDisposable
    {

        public TestOutputHelper _output;

        public BaseTest(OneTimeSetupTearDownFixture testSuite, ITestOutputHelper output)
        {
            _output = (TestOutputHelper)output;
            _testSuite = testSuite;

            testContainer = new TestResultContainer();

            var type = _output.GetType();
            var testMember = type.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
            var test = (ITest)testMember.GetValue(_output);

            testContainer.uuid = test.TestCase.UniqueID;
            testContainer.description = test.TestCase.UniqueID;

            _testSuite.lifeCycle.StartTestContainer(testContainer);
            
            _testSuite.driver.Manage().Window.Maximize();
        }

        public void Dispose()
        {

            var afterFixtureUUID = Guid.NewGuid().ToString("N");
            afterFixture = new FixtureResult()
            {
                name = afterFixtureUUID,
                description = afterFixtureUUID
            };


            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(0);
            MethodBase testName = sf.GetMethod();

            string currentDate = DateTime.Now.ToString("ddMMyyyy");
            ITakesScreenshot screenshotHandler = _testSuite.driver as ITakesScreenshot;
            Screenshot screenshot = screenshotHandler.GetScreenshot();


            string filePath = $@"{_testSuite.resultsDirectory }\{testName + currentDate}.png";

            screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);



            var stepResultID = Guid.NewGuid().ToString("N");
            var stepResult = new StepResult();
            stepResult.name = stepResultID;

            var testResultId = Guid.NewGuid().ToString("N");
            var result = new TestResult();
            result.uuid = testResultId;


            _testSuite.lifeCycle.StartAfterFixture(testContainer.uuid, afterFixtureUUID, afterFixture)
                .StartStep(stepResultID, stepResult)
                .AddAttachment(testName.Name + currentDate, "image/png", filePath)
                .AddScreenDiff(testResultId, filePath, filePath, filePath)
                .StopStep(x => x.status = Status.none);


            _testSuite.lifeCycle.AddAttachment(testName.Name + currentDate, "image/png", filePath);


            _testSuite.lifeCycle.StopFixture(afterFixtureUUID);


            _testSuite.lifeCycle.StopTestContainer(testContainer.uuid)
                .WriteTestContainer(testContainer.uuid);

        }

        public OneTimeSetupTearDownFixture _testSuite;
        public TestResultContainer testContainer;
        public FixtureResult afterFixture;
    }
}
