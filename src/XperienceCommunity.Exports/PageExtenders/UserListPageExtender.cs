using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CMS.DataEngine;
using CMS.DataEngine.Query;
using CMS.Membership;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.UIPages;
using XperienceCommunity.Exports.PageExtenders;
using XperienceCommunity.Exports.PageExtenders.Base;
using XperienceCommunity.Exports.Transformers;

[assembly: PageExtender(typeof(UserListPageExtender))]

namespace XperienceCommunity.Exports.PageExtenders;

[UIPermission(Permissions.Name, Permissions.DisplayName)]
public class UserListPageExtender : ExportPageExtender<UserList>
{
    private readonly IInfoProvider<UserInfo> _userInfoProvider;

    public UserListPageExtender(IUIPermissionEvaluator permissionEvaluator,
        IInfoProvider<UserInfo> userInfoProvider,
        IEnumerable<IExportDataTransformer> exportTransformations)
        : base(permissionEvaluator, exportTransformations)
    {
        _userInfoProvider = userInfoProvider;
    }

    protected override async Task<bool> CanSeeExportAction()
    {
        var count = await _userInfoProvider.Get().GetCountAsync();
        return count > 0;
    }

    protected override Task<string> GetFileNamePrefix() => Task.FromResult("Users");

    protected override bool CanExportColumn(string columnName)
    {
        return !UserInfo.TYPEINFO.SensitiveColumns?.Contains(columnName) ?? base.CanExportColumn(columnName);
    }

    [PageCommand(CommandName = EXPORT_COMMAND, Permission = Permissions.Name)]
    public override async Task<ICommandResponse> ExportCommandHandler(CancellationToken cancellationToken = default)
    {
        var dcs = await _userInfoProvider.Get()
            .OrderBy(nameof(UserInfo.UserID))
            .GetDataContainerResultAsync(cancellationToken: cancellationToken);

        return await Export(dcs, cancellationToken);
    }
}
