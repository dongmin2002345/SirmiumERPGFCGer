using DomainCore.Common.Individuals;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryCore.Abstractions.Common.Individuals
{
    public interface IIndividualsRepository
    {
        List<Individual> GetIndividuals(string filterString);

        List<Individual> GetIndividualsForPopup(string filterString);

        List<Individual> GetIndividualsByPage(int currentPage = 1, int itemsPerPage = 20, string individualName = "");

        int GetIndividualsCount(string searchParameter = "");

        Individual GetIndividual(int id);

        int GetNewCodeValue();

        Individual Create(Individual individual);
        Individual Update(Individual individual);
        Individual Delete(int id);
    }
}
