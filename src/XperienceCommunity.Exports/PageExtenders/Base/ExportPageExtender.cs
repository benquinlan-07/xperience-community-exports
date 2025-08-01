using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CMS.Base;
using CsvHelper;
using Kentico.Xperience.Admin.Base;
using XperienceCommunity.Exports.Components.DataExport;

namespace XperienceCommunity.Exports.PageExtenders.Base;

public abstract class ExportPageExtender<TPage> : PageExtender<TPage> where TPage : ListingPage
{
    private readonly IUIPermissionEvaluator _permissionEvaluator;

    protected ExportPageExtender(IUIPermissionEvaluator permissionEvaluator)
    {
        _permissionEvaluator = permissionEvaluator;
    }

    public const string EXPORT_COMMAND = "EXPORT_LIST";

    public override async Task ConfigurePage()
    {
        await base.ConfigurePage();

        var canSeeExportAction = await CanSeeExportAction();
        if (canSeeExportAction)
        {
            var component = new DataExportComponent()
            {
                Properties = new DataExportProperties
                {
                    CommandName = EXPORT_COMMAND,
                    FileNamePrefix = await GetFileNamePrefix()
                }
            };

            var result = await _permissionEvaluator.Evaluate(Permissions.Name);

            _ = Page.PageConfiguration
                .HeaderActions
                .AddActionWithCustomComponent(component, $"Export", disabled: !result.Succeeded, icon: "xp-arrow-down-line", title: "export");
        }
    }

    protected abstract Task<string> GetFileNamePrefix();

    protected virtual Task<bool> CanSeeExportAction() => Task.FromResult(true);

    protected virtual bool CanExportColumn(string columnName) => true;

    public abstract Task<ICommandResponse> ExportCommandHandler(CancellationToken cancellationToken = default);

    protected async Task<ICommandResponse> Export(IEnumerable<IDataContainer> data, CancellationToken cancellationToken = default)
    {
        var dataItems = new List<dynamic>();

        foreach (var dc in data)
        {
            dynamic ds = new ExpandoObject();
            var dict = (IDictionary<string, object>)ds;
            foreach (string column in dc.ColumnNames)
            {
                if (CanExportColumn(column))
                {
                    dict[column] = dc.TryGetValue(column, out object value)
                        ? value?.ToString() ?? ""
                        : "";

                }
            }

            dataItems.Add(ds);
        }

        using var ms = new MemoryStream();
        await using var writer = new StreamWriter(ms);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        await csv.WriteRecordsAsync(dataItems, cancellationToken);
        await writer.FlushAsync(cancellationToken);
        ms.Position = 0;

        return ResponseFrom(DataExportResponse.Data(Convert.ToBase64String(ms.ToArray())))
            .AddSuccessMessage("Export complete");
    }
}
