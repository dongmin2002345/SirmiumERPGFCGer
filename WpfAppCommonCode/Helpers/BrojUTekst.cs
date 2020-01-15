using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppCommonCode.Helpers
{

    public class BrojUTekst
    {
        public bool Hrvatskezik { get; set; } = false;

        public BrojUTekst()
        {

        }
        public string PretvoriBrojUTekst(string str, char decSymb, string valuta, string manjaValuta)
        {
            string[] delovi = str.Split(decSymb);

            int val = delovi[0].Length;
            string retVal;
            switch (val)
            {
                case 1:
                    retVal = Jednocifreni(delovi[0], false);
                    break;
                case 2:
                    retVal = Dvocifreni(delovi[0], false);
                    break;

                case 3:
                    retVal = Trocifreni(delovi[0], false);
                    break;
                case 4:
                    retVal = Cetvorocifreni(delovi[0], false);
                    break;
                case 5:
                    retVal = Petocifreni(delovi[0], false);
                    break;
                case 6:
                    retVal = Sestocifreni(delovi[0], false);
                    break;
                case 7:
                    retVal = Sedmocifreni(delovi[0], false);
                    break;
                case 8:
                    retVal = Osmocifreni(delovi[0], false);
                    break;
                case 9:
                    retVal = Devetocifreni(delovi[0], false);
                    break;
                case 10:
                    retVal = Desetocifreni(delovi[0], false);
                    break;
                default:
                    return " ";
            }


            retVal = retVal.ToLower().Replace(" ", "");

            decimal part2parsed;
            decimal.TryParse(delovi[1], out part2parsed);

            int value = (int)Math.Ceiling(part2parsed / 10.0m);

            retVal += " " + valuta.ToLower().Replace(" ", "") + " i " + value.ToString("{0:00}") + "/100";

            return retVal;

        }

        private string Desetocifreni(string jed, bool zenskirod)
        {
            if (jed[0] == '0')
                return Devetocifreni(jed.Substring(1, 9), zenskirod);
            else
            {
                return Milijarde(jed.Substring(0, 1), true) + Devetocifreni(jed.Substring(1, 9), true);

            }
        }

        private string Devetocifreni(string jed, bool zenskirod)
        {
            if (jed[0] == '0')
                return Osmocifreni(jed.Substring(1, 8), zenskirod);
            else
            {
                if (jed[2] == '1' && jed[1] != '1')
                    return Trocifreni(jed.Substring(0, 3), false) + (Hrvatskezik ? "MILIJUN " : "MILION ") + Sestocifreni(jed.Substring(3, 6), false);
                else
                    return Trocifreni(jed.Substring(0, 3), false) + (Hrvatskezik ? "MILIJUNA " : "MILIONA ") + Sestocifreni(jed.Substring(3, 6), false);
            }
        }

        private string Osmocifreni(string jed, bool zenskirod)
        {
            if (jed[0] == '0')
                return Sedmocifreni(jed.Substring(1, 7), zenskirod);
            else
            {
                if (jed[1] == '1' && jed[0] != '1')
                    return Dvocifreni(jed.Substring(0, 2), false) + (Hrvatskezik ? "MILIJUN " : "MILION ") + Sestocifreni(jed.Substring(2, 6), true);
                else
                    return Dvocifreni(jed.Substring(0, 2), false) + (Hrvatskezik ? "MILIJUNA " : "MILIONA ") + Sestocifreni(jed.Substring(2, 6), true);

            }
        }

        private string Sedmocifreni(string jed, bool zenskirod)
        {
            if (jed[0] == '0')
                return Sestocifreni(jed.Substring(1, 6), zenskirod);
            else
            {
                return Milioni(jed.Substring(0, 1), zenskirod) + Sestocifreni(jed.Substring(1, 6), false);
            }
        }

        private string Sestocifreni(string jed, bool zenskirod)
        {
            if (jed[0] == '0')
                return Petocifreni(jed.Substring(1, 5), zenskirod);
            else
            {
                if ((jed[2] == '2' ||
                   jed[2] == '3' ||
                   jed[2] == '2') && (jed[1] != '1'))
                    return Trocifreni(jed.Substring(0, 3), true) + (Hrvatskezik ? "TISUĆE " : "HILJADE ") + Trocifreni(jed.Substring(3, 3), false);
                else
                    return Trocifreni(jed.Substring(0, 3), true) + (Hrvatskezik ? "TISUĆA " : "HILJADA ") + Trocifreni(jed.Substring(3, 3), false);
            }
        }

        private string Petocifreni(string jed, bool zenskirod)
        {
            if (jed[0] == '0')
                return Cetvorocifreni(jed.Substring(1, 4), zenskirod);
            else
            {
                if (jed[0] == '1')
                {
                    return Teen(jed.Substring(0, 2), true) + (Hrvatskezik ? "TISUĆA " : "HILJADA ") + Trocifreni(jed.Substring(2, 3), zenskirod);
                }

                else
                {
                    if ((jed[1] == '2' ||
                    jed[1] == '3' ||
                    jed[1] == '2'))
                        return Dvocifreni(jed.Substring(0, 2), true) + (Hrvatskezik ? "TISUĆE " : "HILJADE ") + Trocifreni(jed.Substring(2, 3), zenskirod);
                    else
                        return Dvocifreni(jed.Substring(0, 2), true) + (Hrvatskezik ? "TISUĆA " : "HILJADA ") + Trocifreni(jed.Substring(2, 3), zenskirod);

                }

            }
        }

        private string Cetvorocifreni(string jed, bool zenskirod)
        {
            if (jed[0] == '0')
                return Trocifreni(jed.Substring(1, 3), zenskirod);
            else
                return Hiljade(jed.Substring(0, 1), zenskirod) + Trocifreni(jed.Substring(1, 3), zenskirod);
        }

        private string Trocifreni(string jed, bool zenskirod)
        {
            if (jed[0] == '0')
                return Dvocifreni(jed.Substring(1, 2), zenskirod);
            else
                return Stotine(jed.Substring(0, 1), zenskirod) + Dvocifreni(jed.Substring(1, 2), zenskirod);
        }

        private string Dvocifreni(string jed, bool zenskirod)
        {
            if (jed[0] == '0')
            {
                if (jed.Length == 1)
                    jed += "0";
                return Jedinice(jed.Substring(1, 1), zenskirod);
            }

            if (jed[0] != '1')
            {
                if (jed.Length == 1)
                    jed += "0";
                return Desetice(jed.Substring(0, 1), zenskirod) + Jedinice(jed.Substring(1, 1), zenskirod);
            }
            else
                return Teen(jed, zenskirod);
        }

        private string Jednocifreni(string jed, bool zenskirod)
        {
            return Jedinice(jed, zenskirod);
        }
        string Jedinice(string jed, bool zenskirod)
        {
            if (jed == "1")
            {
                if (zenskirod)
                    return "JEDNA ";
                else
                    return "JEDAN ";

            }

            else if (jed == "2")
            {
                if (zenskirod)
                    return "DVE ";
                else
                    return "DVA ";

            }
            else if (jed == "3")
                return "TRI ";
            else if (jed == "4")
                return "ČETIRI ";
            else if (jed == "5")
                return "PET ";
            else if (jed == "6")
                return "ŠEST ";
            else if (jed == "7")
                return "SEDAM ";
            else if (jed == "8")
                return "OSAM ";
            else if (jed == "9")
                return "DEVET ";
            else
                return "";
        }
        string Teen(string jed, bool zenskirod)
        {
            if (jed == "10")
                return "DESET ";
            else if (jed == "11")
                return "JEDANAEST ";
            else if (jed == "12")
                return "DVANAEST ";
            else if (jed == "13")
                return "TRINAEST ";
            else if (jed == "14")
                return "ČETRNAEST ";
            else if (jed == "15")
                return "PETNAEST ";
            else if (jed == "16")
                return "ŠESNAEST ";
            else if (jed == "17")
                return "SEDAMNAEST ";
            else if (jed == "18")
                return "OSAMNAEST ";
            else if (jed == "19")
                return "DEVETNAEST ";
            else
                return "";
        }
        string Desetice(string jed, bool zenskirod)
        {
            if (jed == "1")
                return "DESET ";
            else if (jed == "2")
                return "DVADESET ";
            else if (jed == "3")
                return "TRIDESET ";
            else if (jed == "4")
                return "ČETRDESET ";
            else if (jed == "5")
                return "PEDESET ";
            else if (jed == "6")
                return "ŠESDESET ";
            else if (jed == "7")
                return "SEDAMDESET ";
            else if (jed == "8")
                return "OSAMDESET ";
            else if (jed == "9")
                return "DEVEDESET ";
            else
                return "";
        }
        string Stotine(string jed, bool zenskirod)
        {
            if (jed == "1")
                return "STO ";
            else if (jed == "2")
                return "DVESTO ";
            else if (jed == "3")
                return "TRISTO ";
            else if (jed == "4")
                return "ČETIRISTO ";
            else if (jed == "5")
                return "PETSTO ";
            else if (jed == "6")
                return "ŠESTO ";
            else if (jed == "7")
                return "SEDAMSTO ";
            else if (jed == "8")
                return "OSAMSTO ";
            else if (jed == "9")
                return "DEVETSTO ";
            else
                return "";
        }
        string Hiljade(string jed, bool zenskirod)
        {
            if (jed == "1")
                return Hrvatskezik ? "TISUĆA " : "HILJADA ";
            else if (jed == "2")
                return Hrvatskezik ? "DVETISUĆE " : "DVE HILJADE ";
            else if (jed == "3")
                return Hrvatskezik ? "TRITISUĆE " : "TRI HILJADE ";
            else if (jed == "4")
                return Hrvatskezik ? "ČETIRITISUĆE " : "ČETIRI HILJADE ";
            else if (jed == "5")
                return Hrvatskezik ? "PETTISUĆA " : "PET HILJADA ";
            else if (jed == "6")
                return Hrvatskezik ? "ŠESTTISIĆA " : "ŠEST HILJADA ";
            else if (jed == "7")
                return Hrvatskezik ? "SEDAMTISUĆA " : "SEDAM HILJADA ";
            else if (jed == "8")
                return Hrvatskezik ? "OSAMTISUĆA " : "OSAM HILJADA ";
            else if (jed == "9")
                return Hrvatskezik ? "DEVETTISUĆA " : "DEVET HILJADA ";
            else
                return "";
        }
        string Milioni(string jed, bool zenskirod)
        {
            if (jed == "1")
                return Hrvatskezik ? "MILIJUN " : "MILION ";
            else if (jed == "2")
                return Hrvatskezik ? "DVA MILIJUNA " : "DVA MILIONA ";
            else if (jed == "3")
                return Hrvatskezik ? "TRI MILIJUNA " : "TRI MILIONA ";
            else if (jed == "4")
                return Hrvatskezik ? "ČETIRI MILIJUNA " : "ČETIRI MILIONA ";
            else if (jed == "5")
                return Hrvatskezik ? "PET MILIJUNA" : "PET MILIONA ";
            else if (jed == "6")
                return Hrvatskezik ? "ŠEST MILIJUNA " : "ŠEST MILIONA ";
            else if (jed == "7")
                return Hrvatskezik ? "SEDA MMILIJUNA " : "SEDAM MILIONA";
            else if (jed == "8")
                return Hrvatskezik ? "OSAM MILIJUNA " : "OSAM MILIONA ";
            else if (jed == "9")
                return Hrvatskezik ? "DEVET MILIJUNA " : "DEVET MILIONA ";
            else
                return "";
        }
        string Milijarde(string jed, bool zenskirod)
        {
            if (jed == "1")
                return "MILIJARDA";
            else if (jed == "2")
                return "DVE MILIJARDE";
            else if (jed == "3")
                return "TRI MILIJARDE";
            else if (jed == "4")
                return "ČETIRI MILIJARDE";
            else if (jed == "5")
                return "PET MILIJARDI";
            else if (jed == "6")
                return "ŠEST MILIJARDI";
            else if (jed == "7")
                return "SEDAM MILIJARDI";
            else if (jed == "8")
                return "OSAM MILIJARDI";
            else if (jed == "9")
                return "DEVET MILIJARDI";
            else
                return "";
        }
    }
}
