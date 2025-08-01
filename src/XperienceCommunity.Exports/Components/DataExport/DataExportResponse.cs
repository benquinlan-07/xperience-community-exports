namespace XperienceCommunity.Exports.Components.DataExport;

public record DataExportResponse(string FileData, string ErrorMessage)
{
    public static DataExportResponse Data(string fileData) => new(fileData, null);
    public static DataExportResponse Error(string error) => new(null, error);
}
