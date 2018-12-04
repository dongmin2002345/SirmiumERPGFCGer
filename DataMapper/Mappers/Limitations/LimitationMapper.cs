using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Limitations;
using ServiceInterfaces.ViewModels.Limitations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Limitations
{
    public static class LimitationMapper
    {
        public static List<LimitationViewModel> ConvertToLimitationViewModelList(this IEnumerable<Limitation> limitations)
        {
            List<LimitationViewModel> limitationsViewModels = new List<LimitationViewModel>();
            foreach (Limitation limitation in limitations)
            {
                limitationsViewModels.Add(limitation.ConvertToLimitationViewModel());
            }
            return limitationsViewModels;
        }

        public static LimitationViewModel ConvertToLimitationViewModel(this Limitation limitation)
        {
            LimitationViewModel limitationViewModel = new LimitationViewModel()
            {
                Id = limitation.Id,
                Identifier = limitation.Identifier,

                ConstructionSiteLimit = limitation.ConstructionSiteLimit,
                BusinessPartnerConstructionSiteLimit = limitation.BusinessPartnerConstructionSiteLimit,
                EmployeeConstructionSiteLimit = limitation.EmployeeConstructionSiteLimit,
                EmployeeBusinessPartnerLimit = limitation.EmployeeBusinessPartnerLimit,
                EmployeeBirthdayLimit = limitation.EmployeeBirthdayLimit,

                EmployeePassportLimit = limitation.EmployeePassportLimit,
                EmployeeEmbasyLimit = limitation.EmployeeEmbasyLimit,
                EmployeeVisaTakeOffLimit = limitation.EmployeeVisaTakeOffLimit,
                EmployeeVisaLimit = limitation.EmployeeVisaLimit,
                EmployeeWorkLicenceLimit = limitation.EmployeeWorkLicenceLimit,
                EmployeeDriveLicenceLimit = limitation.EmployeeDriveLicenceLimit,
                EmployeeEmbasyFamilyLimit = limitation.EmployeeEmbasyFamilyLimit,

                PersonPassportLimit = limitation.PersonPassportLimit,
                PersonEmbasyLimit = limitation.PersonEmbasyLimit,
                PersonVisaTakeOffLimit = limitation.PersonVisaTakeOffLimit,
                PersonVisaLimit = limitation.PersonVisaLimit,
                PersonWorkLicenceLimit = limitation.PersonWorkLicenceLimit,
                PersonDriveLicenceLimit = limitation.PersonDriveLicenceLimit,
                PersonEmbasyFamilyLimit = limitation.PersonEmbasyFamilyLimit,

                IsActive = limitation.Active,

                CreatedBy = limitation.CreatedBy?.ConvertToUserViewModelLite(),
                Company = limitation.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = limitation.UpdatedAt,
                CreatedAt = limitation.CreatedAt

            };
            return limitationViewModel;
        }

        public static LimitationViewModel ConvertToLimitationViewModelLite(this Limitation limitation)
        {
            LimitationViewModel limitationViewModel = new LimitationViewModel()
            {
                Id = limitation.Id,
                Identifier = limitation.Identifier,

                ConstructionSiteLimit = limitation.ConstructionSiteLimit,
                BusinessPartnerConstructionSiteLimit = limitation.BusinessPartnerConstructionSiteLimit,
                EmployeeConstructionSiteLimit = limitation.EmployeeConstructionSiteLimit,
                EmployeeBusinessPartnerLimit = limitation.EmployeeBusinessPartnerLimit,
                EmployeeBirthdayLimit = limitation.EmployeeBirthdayLimit,

                EmployeePassportLimit = limitation.EmployeePassportLimit,
                EmployeeEmbasyLimit = limitation.EmployeeEmbasyLimit,
                EmployeeVisaTakeOffLimit = limitation.EmployeeVisaTakeOffLimit,
                EmployeeVisaLimit = limitation.EmployeeVisaLimit,
                EmployeeWorkLicenceLimit = limitation.EmployeeWorkLicenceLimit,
                EmployeeDriveLicenceLimit = limitation.EmployeeDriveLicenceLimit,
                EmployeeEmbasyFamilyLimit = limitation.EmployeeEmbasyFamilyLimit,

                PersonPassportLimit = limitation.PersonPassportLimit,
                PersonEmbasyLimit = limitation.PersonEmbasyLimit,
                PersonVisaTakeOffLimit = limitation.PersonVisaTakeOffLimit,
                PersonVisaLimit = limitation.PersonVisaLimit,
                PersonWorkLicenceLimit = limitation.PersonWorkLicenceLimit,
                PersonDriveLicenceLimit = limitation.PersonDriveLicenceLimit,
                PersonEmbasyFamilyLimit = limitation.PersonEmbasyFamilyLimit,

                IsActive = limitation.Active,

                CreatedAt = limitation.CreatedAt,
                UpdatedAt = limitation.UpdatedAt
            };
            return limitationViewModel;
        }

        public static Limitation ConvertToLimitation(this LimitationViewModel limitationViewModel)
        {
            Limitation limitation = new Limitation()
            {
                Id = limitationViewModel.Id,
                Identifier = limitationViewModel.Identifier,

                ConstructionSiteLimit = limitationViewModel.ConstructionSiteLimit,
                BusinessPartnerConstructionSiteLimit = limitationViewModel.BusinessPartnerConstructionSiteLimit,
                EmployeeConstructionSiteLimit = limitationViewModel.EmployeeConstructionSiteLimit,
                EmployeeBusinessPartnerLimit = limitationViewModel.EmployeeBusinessPartnerLimit,
                EmployeeBirthdayLimit = limitationViewModel.EmployeeBirthdayLimit,

                EmployeePassportLimit = limitationViewModel.EmployeePassportLimit,
                EmployeeEmbasyLimit = limitationViewModel.EmployeeEmbasyLimit,
                EmployeeVisaTakeOffLimit = limitationViewModel.EmployeeVisaTakeOffLimit,
                EmployeeVisaLimit = limitationViewModel.EmployeeVisaLimit,
                EmployeeWorkLicenceLimit = limitationViewModel.EmployeeWorkLicenceLimit,
                EmployeeDriveLicenceLimit = limitationViewModel.EmployeeDriveLicenceLimit,
                EmployeeEmbasyFamilyLimit = limitationViewModel.EmployeeEmbasyFamilyLimit,

                PersonPassportLimit = limitationViewModel.PersonPassportLimit,
                PersonEmbasyLimit = limitationViewModel.PersonEmbasyLimit,
                PersonVisaTakeOffLimit = limitationViewModel.PersonVisaTakeOffLimit,
                PersonVisaLimit = limitationViewModel.PersonVisaLimit,
                PersonWorkLicenceLimit = limitationViewModel.PersonWorkLicenceLimit,
                PersonDriveLicenceLimit = limitationViewModel.PersonDriveLicenceLimit,
                PersonEmbasyFamilyLimit = limitationViewModel.PersonEmbasyFamilyLimit,

                Active = limitationViewModel.IsActive,

                CreatedById = limitationViewModel.CreatedBy?.Id ?? null,
                CompanyId = limitationViewModel.Company?.Id ?? null,

                CreatedAt = limitationViewModel.CreatedAt,
                UpdatedAt = limitationViewModel.UpdatedAt

            };
            return limitation;
        }
    }
}
