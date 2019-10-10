using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceInterfaces.Gloabals
{
    public static class ItemStatus
    {
        public static int Submited = 1;
        public static int Added = 2;
        public static int Edited = 3;
        public static int Deleted = 4;
        public static int StockForUpdate = 5;
        public static int StockUpdated = 6;

        public static string SubmitedText = "Proknjižen";
        public static string AddedText = "Dodat";
        public static string EditedText = "Promenjen";
        public static string DeletedText = "Obrisan";
        public static string StockForUpdateText = "Promena lagera";
        public static string StockUpdatedText = "Lager promenjen";
    }
}
