using ServiceInterfaces.Messages.Common.Individuals;
using ServiceInterfaces.ViewModels.Common.Individuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInterfaces.Abstractions.Common.Individuals
{
    public interface IIndividualService
    {
        IndividualListResponse GetIndividuals(string filterString);

        IndividualListResponse GetIndividualsByPage(int currentPage = 1, int itemsPerPage = 20, string searchParameter = "");

        IndividualListResponse GetIndividualsForPopup(string filterString);

        IndividualListResponse GetIndividualsCount(string searchParameter = "");

        IndividualResponse GetIndividual(int id);

        IndividualCodeResponse GetNewCodeValue();

        IndividualResponse Create(IndividualViewModel individual);

        IndividualResponse Update(IndividualViewModel individual);

        IndividualResponse Delete(int id);
    }
}
