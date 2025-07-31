using Kentico.Xperience.Admin.Base;

namespace XperienceCommunity.Exports.Components.DataExport;

public class DataExportClientProperties : IActionComponentClientProperties
{
    public string ComponentName { get; init; } = "";
    public string CommandName { get; set; } = "";
    public string FileNamePrefix { get; set; } = "";
}