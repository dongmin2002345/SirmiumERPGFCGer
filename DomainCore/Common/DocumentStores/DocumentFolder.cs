using DomainCore.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainCore.Common.DocumentStores
{
    public class DocumentFolder : BaseEntity
    {

        public string Name { get; set; }
        public string Path { get; set; }


        public int? ParentFolderId { get; set; }
        public DocumentFolder ParentFolder { get; set; }

    }
}
