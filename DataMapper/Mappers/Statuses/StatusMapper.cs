using DataMapper.Mappers.Common.Companies;
using DataMapper.Mappers.Common.Identity;
using DomainCore.Statuses;
using ServiceInterfaces.ViewModels.Statuses;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMapper.Mappers.Statuses
{
    public static class StatusMapper
    {
        public static List<StatusViewModel> ConvertToStatusViewModelList(this IEnumerable<Status> statuses)
        {
            List<StatusViewModel> statusViewModels = new List<StatusViewModel>();
            foreach (Status status in statuses)
            {
                statusViewModels.Add(status.ConvertToStatusViewModel());
            }
            return statusViewModels;
        }

        public static StatusViewModel ConvertToStatusViewModel(this Status status)
        {
            StatusViewModel statusViewModel = new StatusViewModel()
            {
                Id = status.Id,
                Identifier = status.Identifier,

                Code = status.Code,
                Name = status.Name,
                ShortName = status.ShortName,

                IsActive = status.Active,

                CreatedBy = status.CreatedBy?.ConvertToUserViewModelLite(),
                Company = status.Company?.ConvertToCompanyViewModelLite(),

                UpdatedAt = status.UpdatedAt,
                CreatedAt = status.CreatedAt,
            };

            return statusViewModel;
        }

        public static StatusViewModel ConvertToStatusViewModelLite(this Status status)
        {
            StatusViewModel statusViewModel = new StatusViewModel()
            {
                Id = status.Id,
                Identifier = status.Identifier,

                Code = status.Code,
                Name = status.Name,
                ShortName = status.ShortName,

                IsActive = status.Active,

                UpdatedAt = status.UpdatedAt,
                CreatedAt = status.CreatedAt
            };

            return statusViewModel;
        }

        public static Status ConvertToStatus(this StatusViewModel statusViewModel)
        {
            Status status = new Status()
            {
                Id = statusViewModel.Id,
                Identifier = statusViewModel.Identifier,

                Code = statusViewModel.Code,
                Name = statusViewModel.Name,
                ShortName = statusViewModel.ShortName,

                Active = statusViewModel.IsActive,

                CreatedById = statusViewModel.CreatedBy?.Id ?? null,
                CompanyId = statusViewModel.Company?.Id ?? null,

                CreatedAt = statusViewModel.CreatedAt,
                UpdatedAt = statusViewModel.UpdatedAt,
            };

            return status;
        }
    }
}
