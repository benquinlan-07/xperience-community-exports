using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CMS.Base;
using Kentico.Xperience.Admin.Base;

namespace XperienceCommunity.Exports.Transformers
{
    public interface IExportDataTransformer
    {
        Task<IEnumerable<IDataContainer>> Transform<TPage>(TPage page, IEnumerable<IDataContainer> dataContainers, CancellationToken? cancellationToken = null) where TPage : ListingPage;
    }
}
