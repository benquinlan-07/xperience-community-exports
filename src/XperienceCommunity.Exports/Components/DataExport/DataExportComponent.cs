using System.Threading.Tasks;
using Kentico.Xperience.Admin.Base;

namespace XperienceCommunity.Exports.Components.DataExport;

public class DataExportComponent : ActionComponent<DataExportProperties, DataExportClientProperties>
{
    public override string ClientComponentName => "@xperiencecommunityexports/web-admin/DataExport";

    protected override Task ConfigureClientProperties(DataExportClientProperties clientProperties)
    {
        clientProperties.FileNamePrefix = Properties.FileNamePrefix;
        clientProperties.CommandName = Properties.CommandName;

        return base.ConfigureClientProperties(clientProperties);
    }
}
