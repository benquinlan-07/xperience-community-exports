using System.Threading.Tasks;
using Kentico.Xperience.Admin.Base;

namespace XperienceCommunity.Exports.Components;

public class DataExportComponent : ActionComponent<DataExportProperties, DataExportClientProperties>
{
    public override string ClientComponentName => "@xperiencecommunity-exports/web-admin/DataExport";

    protected override Task ConfigureClientProperties(DataExportClientProperties clientProperties)
    {
        clientProperties.FileNamePrefix = Properties.FileNamePrefix;
        clientProperties.CommandName = Properties.CommandName;

        return base.ConfigureClientProperties(clientProperties);
    }
}

public class DataExportProperties : IActionComponentProperties
{
    public string FileNamePrefix { get; set; } = "";
    public string CommandName { get; set; } = "";
}

public class DataExportClientProperties : IActionComponentClientProperties
{
    public string ComponentName { get; init; } = "";
    public string CommandName { get; set; } = "";

    public string FileNamePrefix { get; set; } = "";
}

public record DataExportResponse(string? FileData, string? ErrorMessage)
{
    public static DataExportResponse Data(string fileData) => new(fileData, null);
    public static DataExportResponse Error(string error) => new(null, error);
}