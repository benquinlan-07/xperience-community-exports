using CMS.Base;
using CMS.Core;
using CMS.EmailLibrary.Internal;
using CMS.OnlineForms;
using Kentico.Xperience.Admin.Base;
using XperienceCommunity.Exports;

[assembly: CMS.AssemblyDiscoverable]
[assembly: CMS.RegisterModule(typeof(ExtensionAdminModule))]

namespace XperienceCommunity.Exports;

internal class ExtensionAdminModule : AdminModule
{
    public ExtensionAdminModule()
        : base(Constants.ModuleName)
    {
    }

    protected override void OnInit()
    {
        base.OnInit();

        // Makes the module accessible to the admin UI
        RegisterClientModule("xperiencecommunity-exports", "web-admin");
    }
}