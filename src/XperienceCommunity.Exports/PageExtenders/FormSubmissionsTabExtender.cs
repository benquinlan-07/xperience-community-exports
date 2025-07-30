using CMS.DataEngine;
using CMS.OnlineForms;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.DigitalMarketing.UIPages;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XperienceCommunity.Exports.PageExtenders;
using XperienceCommunity.Exports.PageExtenders.Base;

[assembly: PageExtender(typeof(FormSubmissionsTabExtender))]

namespace XperienceCommunity.Exports.PageExtenders;

[UIPermission(Permissions.Name, Permissions.DisplayName)]
public class FormSubmissionsTabExtender : ExportPageExtender<FormSubmissionsTab>
{
    public FormSubmissionsTabExtender(IUIPermissionEvaluator permissionEvaluator)
        : base(permissionEvaluator)
    {
    }

    private async Task<(BizFormInfo form, DataClassInfo dataClass)> GetFormDataFromPage(CancellationToken cancellationToken = default)
    {
        var forms = await BizFormInfoProvider.ProviderObject.Get()
            .WhereEquals(nameof(BizFormInfo.FormID), Page.FormId)
            .GetEnumerableTypedResultAsync(cancellationToken: cancellationToken);

        return forms
            .Select(f => (f, DataClassInfoProvider.GetDataClassInfo(f.FormClassID)))
            .FirstOrDefault();
    }

    protected override async Task<string> GetFileNamePrefix()
    {
        var form = await GetFormDataFromPage();
        return form.form?.FormDisplayName ?? "FormSubmissions";
    }

    [PageCommand(CommandName = EXPORT_COMMAND, Permission = Permissions.Name)]
    public override async Task<ICommandResponse> ExportCommandHandler(CancellationToken cancellationToken = default)
    {
        var data = await GetFormDataFromPage(cancellationToken);

        var dcs = await BizFormItemProvider.GetItems(data.dataClass.ClassName)
            .OrderByDescending(nameof(BizFormItem.FormInserted))
            .GetDataContainerResultAsync(cancellationToken: cancellationToken);

        return await Export(dcs, cancellationToken);
    }
}