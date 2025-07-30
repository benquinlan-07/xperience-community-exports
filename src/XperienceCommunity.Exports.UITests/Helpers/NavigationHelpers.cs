using OpenQA.Selenium;

namespace XperienceCommunity.Exports.UITests.Helpers
{
    internal class NavigationHelpers
    {
        public static void GoToHomepage(WebDriver driver)
        {
            driver.Navigate().GoToUrl(Constants.BaseUrl);
        }

        public static void SignInToAdmin(WebDriver driver)
        {
            driver.Navigate().GoToUrl($"{Constants.BaseUrl}/admin");
            var usernameInput = driver.FindElement(By.CssSelector("[data-testid=\"userName\"]"));
            usernameInput.SendKeys(Constants.AdministratorUsername);
            var passwordInput = driver.FindElement(By.CssSelector("[data-testid=\"password\"]"));
            passwordInput.SendKeys(Constants.AdministratorPassword);
            var signinButton = driver.FindElement(By.CssSelector("[data-testid=\"submit\"]"));
            signinButton.Click();
        }
    }
}
