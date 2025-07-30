using CMS.DataEngine;
using Kentico.Xperience.Admin.Base;
using System.Threading;
using System.Threading.Tasks;
using CMS.Membership;
using Kentico.Xperience.Admin.Base.UIPages;
using XperienceCommunity.Exports.PageExtenders;
using XperienceCommunity.Exports.PageExtenders.Base;

[assembly: PageExtender(typeof(UsersPageExtender))]

namespace XperienceCommunity.Exports.PageExtenders;

[UIPermission(Permissions.Name, Permissions.DisplayName)]
public class UsersPageExtender : ExportPageExtender<UserList>
{
    private readonly IInfoProvider<UserInfo> userInfoProvider;

    public UsersPageExtender(IUIPermissionEvaluator permissionEvaluator,
        IInfoProvider<UserInfo> userInfoProvider)
        : base(permissionEvaluator)
    {
        this.userInfoProvider = userInfoProvider;
    }

    protected override Task<string> GetFileNamePrefix() => Task.FromResult("Users");

    protected override bool CanExportColumn(string columnName)
    {
        return columnName != "UserPassword" &&
               columnName != "UserSecurityStamp";
    }

    [PageCommand(CommandName = EXPORT_COMMAND, Permission = Permissions.Name)]
    public override async Task<ICommandResponse> ExportCommandHandler(CancellationToken cancellationToken = default)
    {
        var dcs = await userInfoProvider.Get()
            .OrderByDescending(nameof(UserInfo.UserID))
            .GetDataContainerResultAsync(cancellationToken: cancellationToken);

        return await Export(dcs, cancellationToken);
    }
}