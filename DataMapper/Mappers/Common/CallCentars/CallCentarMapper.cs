using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Common.CallCentars;
using ServiceInterfaces.ViewModels.Common.CallCentars;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Common.CallCentars
{
    public static class CallCentarMapper
    {
        public static List<CallCentarViewModel> ConvertToCallCentarViewModelList(this IEnumerable<CallCentar> callCentars)
        {
            List<CallCentarViewModel> CallCentarViewModels = new List<CallCentarViewModel>();
            foreach (CallCentar CallCentar in callCentars)
            {
                CallCentarViewModels.Add(CallCentar.ConvertToCallCentarViewModel());
            }
            return CallCentarViewModels;
        }

        public static CallCentarViewModel ConvertToCallCentarViewModel(this CallCentar CallCentar)
        {
            CallCentarViewModel CallCentarViewModel = new CallCentarViewModel()
            {
                Id = CallCentar.Id,
                Identifier = CallCentar.Identifier,

                Code = CallCentar.Code,
                ReceivingDate = CallCentar.ReceivingDate,

                User = CallCentar.User?.ConvertToUserViewModelLite(),

                Comment = CallCentar.Comment,
                EndingDate = CallCentar.EndingDate,
                CheckedDone = CallCentar.CheckedDone,
                IsActive = CallCentar.Active,

                CreatedBy = CallCentar.CreatedBy?.ConvertToUserViewModelLite(),
                Company = CallCentar.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = CallCentar.UpdatedAt,
                CreatedAt = CallCentar.CreatedAt,
            };

            return CallCentarViewModel;
        }

        public static CallCentarViewModel ConvertToCallCentarViewModelLite(this CallCentar CallCentar)
        {
            CallCentarViewModel CallCentarViewModel = new CallCentarViewModel()
            {
                Id = CallCentar.Id,
                Identifier = CallCentar.Identifier,

                Code = CallCentar.Code,
                ReceivingDate = CallCentar.ReceivingDate,
                Comment = CallCentar.Comment,
                EndingDate = CallCentar.EndingDate,
                CheckedDone = CallCentar.CheckedDone,
                IsActive = CallCentar.Active,

                UpdatedAt = CallCentar.UpdatedAt,
                CreatedAt = CallCentar.CreatedAt
            };

            return CallCentarViewModel;
        }

        public static CallCentar ConvertToCallCentar(this CallCentarViewModel CallCentarViewModel)
        {
            CallCentar CallCentar = new CallCentar()
            {
                Id = CallCentarViewModel.Id,
                Identifier = CallCentarViewModel.Identifier,

                Code = CallCentarViewModel.Code,
                ReceivingDate = CallCentarViewModel.ReceivingDate,

                UserId = CallCentarViewModel.User?.Id ?? null,

                Comment = CallCentarViewModel.Comment,
                EndingDate = CallCentarViewModel.EndingDate,
                CheckedDone = CallCentarViewModel.CheckedDone,
                Active = CallCentarViewModel.IsActive,

                CreatedById = CallCentarViewModel.CreatedBy?.Id ?? null,
                CompanyId = CallCentarViewModel.Company?.Id ?? null,

                CreatedAt = CallCentarViewModel.CreatedAt,
                UpdatedAt = CallCentarViewModel.UpdatedAt,
            };

            return CallCentar;
        }
    }
}
