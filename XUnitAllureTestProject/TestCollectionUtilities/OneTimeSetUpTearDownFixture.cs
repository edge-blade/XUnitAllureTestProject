using Allure.Commons;
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using OpenQA.Selenium.Support.UI;

namespace XUnitAllureTestProject.TestCollectionUtilities
{
    /// <summary>
    /// Functions as OneTimeSetup and OneTimeTearDown when tests are utilizing this fixture
    /// </summary>
    public class OneTimeSetupTearDownFixture : IDisposable
    {
        public OneTimeSetupTearDownFixture()
        {
            Environment.SetEnvironmentVariable(
                AllureConstants.ALLURE_CONFIG_ENV_VARIABLE,
                Path.Combine(Environment.CurrentDirectory, AllureConstants.CONFIG_FILENAME));

            // Shared Setup across all tests
            browserOptions = new ChromeOptions();
            browserOptions.AddArguments("no-sandbox", "window-size=1920,1080", "disable-extensions", "disable-popup-blocking");
            //browserOptions.AddAdditionalOption("useAutomationExtension", false);
            browserOptions.AddUserProfilePreference("Chrome PDF Viewer", false);


            driver = new ChromeDriver(@"C:\Selenium\", browserOptions);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            lifeCycle = AllureLifecycle.Instance;

            resultsDirectory = Path.Combine(Environment.CurrentDirectory, AllureLifecycle.Instance.AllureConfiguration.Directory);

            // only check to create once
            if (!Directory.Exists(resultsDirectory))
            {
                Directory.CreateDirectory(resultsDirectory);
            }
            else
            {
                Directory.Delete(resultsDirectory, true);
                var retries = 0;
                while (retries++ <= 10)
                {
                    if (!Directory.Exists(resultsDirectory))
                    {
                        continue;
                    }
                    System.Threading.Thread.Sleep(500);
                }

                Directory.CreateDirectory(resultsDirectory);
            }
        }
        public void Dispose()
        {
            // Final Quit that should kill all browser windows of the web-driver
            driver.Close();
            driver.Quit();
        }

        public WebDriverWait wait;
        public ChromeOptions browserOptions;
        public IWebDriver driver;

        public AllureLifecycle lifeCycle;
        public string resultsDirectory = null;
    }
}
