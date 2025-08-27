using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMS.Base;
using CMS.FormEngine;
using CMS.OnlineForms;
using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.DigitalMarketing.UIPages;
using Microsoft.AspNetCore.Http;

namespace XperienceCommunity.Exports.Transformers
{
    public class FormSubmissionFileUrlDataTransformer : IExportDataTransformer
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FormSubmissionFileUrlDataTransformer(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<IDataContainer>> Transform<TPage>(TPage page, IEnumerable<IDataContainer> dataContainers, CancellationToken? cancellationToken = null) where TPage : ListingPage
        {
            if (page is not FormSubmissionsTab formSubmissionsTab)
                return dataContainers;

            var request = _httpContextAccessor.HttpContext?.Request;
            var authority = request != null ? $"{request.Scheme}://{request.Host}" : "";

            var dcCollection = dataContainers.ToArray();

            var formsResult = await BizFormInfoProvider.ProviderObject.Get()
                .WhereEquals(nameof(BizFormInfo.FormID), formSubmissionsTab.FormId)
                .GetEnumerableTypedResultAsync(cancellationToken: cancellationToken);
            var formResult = formsResult.First();

            foreach (var formItem in formResult.Form.ItemsList.OfType<FormFieldInfo>())
            {
                if (formItem.DataType == "bizformfile")
                {
                    foreach (var dc in dcCollection)
                    {
                        var currentValue = dc[formItem.Name] as string;
                        if (string.IsNullOrWhiteSpace(currentValue))
                            continue;

                        var parts = currentValue.Split('/');
                        if (parts.Length != 2)
                            continue;

                        dc[formItem.Name] = $"{authority}/kentico.formbuilder/file/get?filename={parts[0]}&originalfilename={parts[1]}";
                    }
                }
            }

            return dcCollection;
        }
    }
}
