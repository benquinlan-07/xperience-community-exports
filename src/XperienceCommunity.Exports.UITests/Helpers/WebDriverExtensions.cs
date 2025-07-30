using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace XperienceCommunity.Exports.UITests.Helpers
{
    internal static class WebDriverExtensions
    {
        public static IWebElement FindByAttribute(this WebDriver input, string attributeName, string attributeValue)
        {
            return input.FindElement(By.CssSelector($"[{attributeName}=\"{attributeValue}\"]"));
        }

        public static IWebElement FindByAttribute(this IWebElement input, string attributeName, string attributeValue)
        {
            return input.FindElement(By.CssSelector($"[{attributeName}=\"{attributeValue}\"]"));
        }

        public static ReadOnlyCollection<IWebElement> FindAllByAttribute(this WebDriver input, string attributeName, string attributeValue)
        {
            return input.FindElements(By.CssSelector($"[{attributeName}=\"{attributeValue}\"]"));
        }

        public static ReadOnlyCollection<IWebElement> FindAllByAttribute(this IWebElement input, string attributeName, string attributeValue)
        {
            return input.FindElements(By.CssSelector($"[{attributeName}=\"{attributeValue}\"]"));
        }
    }
}
