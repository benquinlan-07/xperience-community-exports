using Kentico.Xperience.Admin.Base;

namespace XperienceCommunity.Exports.Components.DataExport;

public class DataExportProperties : IActionComponentProperties
{
    public string FileNamePrefix { get; set; } = "";
    public string CommandName { get; set; } = "";
}