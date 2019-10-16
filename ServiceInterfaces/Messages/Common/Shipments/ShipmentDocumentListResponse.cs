using ServiceInterfaces.Messages.Base;
using ServiceInterfaces.ViewModels.Common.Shipments;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Messages.Common.Shipments
{
    public class ShipmentDocumentListResponse : BaseResponse
    {
        public List<ShipmentDocumentViewModel> ShipmentDocuments { get; set; }
        public int TotalItems { get; set; }
    }
}
