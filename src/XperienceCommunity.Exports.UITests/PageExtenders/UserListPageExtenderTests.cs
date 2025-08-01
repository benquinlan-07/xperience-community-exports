using NUnit.Framework.Legacy;
using XperienceCommunity.Exports.UITests.Helpers;

namespace XperienceCommunity.Exports.UITests.PageExtenders
{
    public class UserListPageExtenderTests : UITestBase
    {
        [SetUp]
        public void Setup()
        {
            SetupDriver();
        }

        [Test]
        public void Export_Button_Triggers_Download()
        {
            // Sign in to admin
            NavigationHelpers.SignInToAdmin(WebDriver);
            // Click on the forms application tile
            var usersTile = WebDriver.FindByAttribute("data-testid", "action-tile-users");
            usersTile.Click();
            // Go to the contact form
            var exportButton = WebDriver.FindByAttribute("aria-label", "Export");
            exportButton.Click();
            // Wait for the export to complete
            Thread.Sleep(2000);
            // Check for the download
            var files = Directory.GetFiles(DownloadsDirectory, "*.csv");
            // Validate a file was downloaded
            ClassicAssert.IsTrue(files.Any(x => Path.GetFileName(x).StartsWith("Users")), "No files were downloaded.");
        }

        [TearDown]
        public void TearDown()
        {
            TearDownDriver();
        }
    }
}
