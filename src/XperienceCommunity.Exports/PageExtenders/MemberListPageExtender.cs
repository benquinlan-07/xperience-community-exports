using System.Threading;
using System.Threading.Tasks;
using CMS.DataEngine;
using CMS.Membership;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.UIPages;
using XperienceCommunity.Exports.PageExtenders;
using XperienceCommunity.Exports.PageExtenders.Base;

[assembly: PageExtender(typeof(MemberListPageExtender))]

namespace XperienceCommunity.Exports.PageExtenders;

[UIPermission(Permissions.Name, Permissions.DisplayName)]
public class MemberListPageExtender : ExportPageExtender<MemberList>
{
    private readonly IInfoProvider<MemberInfo> _memberInfoProvider;

    public MemberListPageExtender(IUIPermissionEvaluator permissionEvaluator,
        IInfoProvider<MemberInfo> memberInfoProvider)
        : base(permissionEvaluator)
    {
        _memberInfoProvider = memberInfoProvider;
    }

    protected override Task<string> GetFileNamePrefix() => Task.FromResult("Members");

    protected override bool CanExportColumn(string columnName)
    {
        return !MemberInfo.TYPEINFO.SensitiveColumns?.Contains(columnName) ?? base.CanExportColumn(columnName);
    }

    [PageCommand(CommandName = EXPORT_COMMAND, Permission = Permissions.Name)]
    public override async Task<ICommandResponse> ExportCommandHandler(CancellationToken cancellationToken = default)
    {
        var dcs = await _memberInfoProvider.Get()
            .OrderBy(nameof(MemberInfo.MemberID))
            .GetDataContainerResultAsync(cancellationToken: cancellationToken);

        return await Export(dcs, cancellationToken);
    }
}