using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.DocumentStores
{
    public class DocumentFile : BaseEntity
    {

        public string Name { get; set; }
        public string Path { get; set; }

        public int? DocumentFolderId { get; set; }
        public DocumentFolder DocumentFolder { get; set; }

        public double Size { get; set; }
    }
}
