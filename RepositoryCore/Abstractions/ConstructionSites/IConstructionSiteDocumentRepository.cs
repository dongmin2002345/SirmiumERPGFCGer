using DomainCore.ConstructionSites;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.ConstructionSites
{
    public interface IConstructionSiteDocumentRepository
    {
        List<ConstructionSiteDocument> GetConstructionSiteDocuments(int companyId);
        List<ConstructionSiteDocument> GetConstructionSiteDocumentsByConstructionSite(int constructionSiteId);
        List<ConstructionSiteDocument> GetConstructionSiteDocumentsNewerThen(int companyId, DateTime lastUpdateTime);

        ConstructionSiteDocument Create(ConstructionSiteDocument constructionSiteDocument);
        ConstructionSiteDocument Delete(Guid identifier);
    }
}
