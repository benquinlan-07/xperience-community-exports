using System.Threading;
using System.Threading.Tasks;
using CMS.DataEngine;
using CMS.DataEngine.Query;
using CMS.OnlineForms;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.DigitalMarketing.UIPages;
using XperienceCommunity.Exports.PageExtenders;
using XperienceCommunity.Exports.PageExtenders.Base;

[assembly: PageExtender(typeof(FormListPageExtender))]

namespace XperienceCommunity.Exports.PageExtenders;

[UIPermission(Permissions.Name, Permissions.DisplayName)]
public class FormListPageExtender : ExportPageExtender<FormList>
{
    private readonly IInfoProvider<BizFormInfo> _bizFormInfoProvider;

    public FormListPageExtender(IUIPermissionEvaluator permissionEvaluator,
        IInfoProvider<BizFormInfo> bizFormInfoProvider)
        : base(permissionEvaluator)
    {
        _bizFormInfoProvider = bizFormInfoProvider;
    }

    protected override async Task<bool> CanSeeExportAction()
    {
        var count = await _bizFormInfoProvider.Get().GetCountAsync();
        return count > 0;
    }

    protected override Task<string> GetFileNamePrefix() => Task.FromResult("Forms");

    protected override bool CanExportColumn(string columnName)
    {
        return !BizFormInfo.TYPEINFO.SensitiveColumns?.Contains(columnName) ?? base.CanExportColumn(columnName);
    }

    [PageCommand(CommandName = EXPORT_COMMAND, Permission = Permissions.Name)]
    public override async Task<ICommandResponse> ExportCommandHandler(CancellationToken cancellationToken = default)
    {
        var dcs = await _bizFormInfoProvider.Get()
            .OrderBy(nameof(BizFormInfo.FormID))
            .GetDataContainerResultAsync(cancellationToken: cancellationToken);

        return await Export(dcs, cancellationToken);
    }
}
