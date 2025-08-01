using NUnit.Framework.Legacy;
using XperienceCommunity.Exports.UITests.Helpers;

namespace XperienceCommunity.Exports.UITests.PageExtenders
{
    public class MemberListPageExtenderTests : UITestBase
    {
        [SetUp]
        public void Setup()
        {
            SetupDriver();
        }

        [Test]
        public void Export_Button_Not_Visible_With_No_Submissions()
        {
            // Sign in to admin
            NavigationHelpers.SignInToAdmin(WebDriver);
            // Ensure no submissions exist
            MembersHelper.EnsureNoMembers(WebDriver);
            // Ensure we are back at the contact form submissions
            MembersHelper.GoToMembers(WebDriver);
            // Find the export button
            var exportButtons = WebDriver.WithReducedWaitTime(driver => driver.FindAllByAttribute("aria-label", "Export"));
            // Validate export button doesn't exist
            ClassicAssert.IsEmpty(exportButtons, "Export button was visible");
        }

        [Test]
        public void Export_Button_Triggers_Download_With_Submissions()
        {
            // Sign in to admin
            NavigationHelpers.SignInToAdmin(WebDriver);
            // Ensure members exist
            MembersHelper.EnsureMembersExist(WebDriver);
            // Ensure we are back at the contact form submissions
            MembersHelper.GoToMembers(WebDriver);
            // Find the export button
            var exportButton = WebDriver.WithReducedWaitTime(driver => driver.FindByAttribute("aria-label", "Export"));
            exportButton.Click();
            // Wait for the export to complete
            Thread.Sleep(2000);
            // Check for the download
            var files = Directory.GetFiles(DownloadsDirectory, "*.csv");
            // Validate a file was downloaded
            ClassicAssert.IsTrue(files.Any(x => Path.GetFileName(x).StartsWith("Members")), "No files were downloaded.");
        }

        [TearDown]
        public void TearDown()
        {
            TearDownDriver();
        }
    }
}
