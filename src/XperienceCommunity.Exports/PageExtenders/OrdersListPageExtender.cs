using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CMS.Commerce;
using CMS.DataEngine;
using CMS.DataEngine.Query;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.DigitalCommerce.UIPages;
using XperienceCommunity.Exports.PageExtenders;
using XperienceCommunity.Exports.PageExtenders.Base;
using XperienceCommunity.Exports.Transformers;

[assembly: PageExtender(typeof(OrdersListPageExtender))]

namespace XperienceCommunity.Exports.PageExtenders;

[UIPermission(Permissions.Name, Permissions.DisplayName)]
public class OrdersListPageExtender : ExportPageExtender<OrdersList>
{
    private readonly IInfoProvider<OrderInfo> _orderInfoProvider;

    public OrdersListPageExtender(IUIPermissionEvaluator permissionEvaluator,
        IInfoProvider<OrderInfo> orderInfoProvider,
        IEnumerable<IExportDataTransformer> exportTransformations)
        : base(permissionEvaluator, exportTransformations)
    {
        _orderInfoProvider = orderInfoProvider;
    }

    protected override async Task<bool> CanSeeExportAction()
    {
        var count = await _orderInfoProvider.Get().GetCountAsync();
        return count > 0;
    }

    protected override Task<string> GetFileNamePrefix() => Task.FromResult("Orders");

    protected override bool CanExportColumn(string columnName)
    {
        return !OrderInfo.TYPEINFO.SensitiveColumns?.Contains(columnName) ?? base.CanExportColumn(columnName);
    }

    [PageCommand(CommandName = EXPORT_COMMAND, Permission = Permissions.Name)]
    public override async Task<ICommandResponse> ExportCommandHandler(CancellationToken cancellationToken = default)
    {
        var dcs = await _orderInfoProvider.Get()
            .OrderBy(nameof(OrderInfo.OrderID))
            .GetDataContainerResultAsync(cancellationToken: cancellationToken);

        return await Export(dcs, cancellationToken);
    }
}
