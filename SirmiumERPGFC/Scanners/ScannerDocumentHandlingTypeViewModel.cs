using ServiceInterfaces.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SirmiumERPGFC.Scanners.WIAScanner;

namespace SirmiumERPGFC.Scanners
{
    public class ScannerDocumentHandlingTypeViewModel : BaseEntityViewModel
    {
        public string Name { get; set; }
        public WiaDocumentHandlingType? Type { get; set; }
    }
}
