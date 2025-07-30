using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using XperienceCommunity.Exports.UITests.Helpers;

namespace XperienceCommunity.Exports.UITests
{
    public class BaseWebsiteTests
    {
        private EdgeDriver _driver;

        [SetUp]
        public void Setup()
        {
            _driver = new EdgeDriver();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [Test]
        public void Website_Did_Start()
        {
            NavigationHelpers.GoToHomepage(_driver);
            var logo = _driver.FindElement(By.ClassName("logo-image"));
            Assert.IsNotNull(logo);
        }

        [Test]
        public void Can_Access_Admin()
        {
            NavigationHelpers.SignInToAdmin(_driver);
            var menuElement = _driver.FindElement(By.CssSelector("[data-testid=application-menu]"));
            Assert.IsNotNull(menuElement);
        }

        [TearDown]
        public void TearDown()
        {
            Thread.Sleep(10000);

            _driver.Quit();
            _driver.Dispose();
        }

    }
}