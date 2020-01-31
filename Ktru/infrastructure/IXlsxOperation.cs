using Ktru.model;
using System.Collections.Generic;

namespace Ktru.infrastructure
{
    interface IXlsxOperation
    {
        void SaveKtruFile(string filePath, IEnumerable<KtruItem> ktrus);
    }
}
