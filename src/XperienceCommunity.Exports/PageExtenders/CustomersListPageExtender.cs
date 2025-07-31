using System.Threading;
using System.Threading.Tasks;
using CMS.Commerce;
using CMS.DataEngine;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.DigitalCommerce.UIPages;
using XperienceCommunity.Exports.PageExtenders;
using XperienceCommunity.Exports.PageExtenders.Base;

[assembly: PageExtender(typeof(CustomersListPageExtender))]

namespace XperienceCommunity.Exports.PageExtenders;

[UIPermission(Permissions.Name, Permissions.DisplayName)]
public class CustomersListPageExtender : ExportPageExtender<CustomersList>
{
    private readonly IInfoProvider<CustomerInfo> _customerInfoProvider;

    public CustomersListPageExtender(IUIPermissionEvaluator permissionEvaluator,
        IInfoProvider<CustomerInfo> customerInfoProvider)
        : base(permissionEvaluator)
    {
        _customerInfoProvider = customerInfoProvider;
    }

    protected override Task<string> GetFileNamePrefix() => Task.FromResult("Customers");

    protected override bool CanExportColumn(string columnName)
    {
        return !CustomerInfo.TYPEINFO.SensitiveColumns?.Contains(columnName) ?? base.CanExportColumn(columnName);
    }

    [PageCommand(CommandName = EXPORT_COMMAND, Permission = Permissions.Name)]
    public override async Task<ICommandResponse> ExportCommandHandler(CancellationToken cancellationToken = default)
    {
        var dcs = await _customerInfoProvider.Get()
            .OrderBy(nameof(CustomerInfo.CustomerID))
            .GetDataContainerResultAsync(cancellationToken: cancellationToken);

        return await Export(dcs, cancellationToken);
    }
}