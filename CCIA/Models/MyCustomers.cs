using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

namespace CCIA.Models
{
    public partial class MyCustomers
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public Organizations Organization { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
            
        [ForeignKey("StateId")]
        public StateProvince State { get; set; }
        public int CountyId { get; set; }
        [ForeignKey("CountyId")]
        public County County { get; set; }
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Countries Country { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public List<SelectListItem> GetListItems<T> (String defaultText, IDictionary<string, string> objectMaps = null, 
            bool isDefaultSelected = false, bool isDefaultDisabled = false, String selectedObject = null, 
            bool CapitalizeList = false)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();

            listItems.Add(new SelectListItem { Value = null, Text = defaultText, Selected = isDefaultSelected, Disabled = isDefaultDisabled });

            foreach (string item in Enum.GetNames(typeof(T)))
            {
                var objectName = "";

                if (item.Equals("OutsideUS")) {
                    objectName = "Outside US";
                } else {
                    // split the camelcase words (enum didn't allow 
                    // space so New York became NewYork and etc.)
                    var words = Regex.Split(item, @"(?<!^)(?=[A-Z])");

                    objectName = string.Join(" ", words);

                    if (CapitalizeList)
                        objectName = objectName.ToUpper();
                }

                bool isObjectSelected = false;

                if (!isDefaultSelected)
                    isObjectSelected = selectedObject.Equals(objectName);

                String value = objectName;

                if (objectMaps != null)
                    value = objectMaps[objectName];

                listItems.Add(new SelectListItem(objectName, objectName, isObjectSelected));
            }

            return listItems;
        }

        public IDictionary<string, string> StateCodes
            = new Dictionary<string, string>() {
                {"Outside US", "OUS"},
                {"Alabama", "AL"}, 
                {"Alaska", "AK"},
                {"American Samoa", "AS"},
                {"Arizona", "AZ"},
                {"Arkansas", "AR"},
                {"California", "CA"},
                {"Colorado", "CO"},
                {"Connecticut", "CT"},
                {"Delaware", "DE"},
                {"District of Columbia", "DC"},
                {"Florida", "FL"},
                {"Georgia", "GA"},
                {"Guam", "GU"},
                {"Hawaii", "HI"},
                {"Idaho", "ID"},
                {"Illinois", "IL"},
                {"Indiana", "IN"},
                {"Iowa", "IA"},
                {"Kansas", "KS"},
                {"Kentucky", "KY"},
                {"Louisiana", "LA"},
                {"Maine", "ME"},
                {"Maryland", "MD"},
                {"Marshall Islands", "MH"},
                {"Massachusetts", "MA"},
                {"Michigan", "MI"},
                {"Minnesota", "MN"},
                {"Mississippi", "MS"},
                {"Missouri", "MO"},
                {"Montana", "MT"},
                {"Nebraska", "NE"},
                {"Nevada", "NV"},
                {"New Hampshire", "NH"},
                {"New Jersey", "NJ"},
                {"New Mexico", "NM"},
                {"New York", "NY"},
                {"North Carolina", "NC"},
                {"North Dakota", "ND"},
                {"Ohio", "OH"},
                {"Oklahoma", "OK"},
                {"Oregon", "OR"},
                {"Pennsylvania", "PA"},
                {"Puerto Rico", "PR"},
                {"Rhode Island", "RI"},
                {"South Carolina", "SC"},
                {"South Dakota", "SD"},
                {"Tennessee", "TN"},
                {"Texas", "TX"},
                {"Utah", "UT"},
                {"Vermont", "VT"},
                {"Virgin Islands", "VI"},
                {"Virginia", "VA"},
                {"Washington", "WA"},
                {"West Virginia", "WV"},
                {"Wisconsin", "WI"},
                {"Wyoming", "WY"}
            };
    }

    public enum States
    {
        Alabama,
        Alaska,
        Arizona,
        Arkansas,
        California,
        Colorado,
        Connecticut,
        Delaware,
        Florida,
        Georgia,
        Hawaii,
        Idaho,
        Illinois,
        Indiana,
        Iowa,
        Kansas,
        Kentucky,
        Louisiana,
        Maine,
        Maryland,
        Massachusetts,
        Michigan,
        Minnesota,
        Mississippi,
        Missouri,
        Montana,
        NorthCarolina,
        NorthDakota,
        Nebraska,
        Nevada,
        NewHampshire,
        NewJersey,
        NewMexico,
        NewYork,
        Ohio,
        Oklahoma,
        Oregon,
        OutsideUS,
        Pennsylvania,
        RhodeIsland,
        SouthCarolina,
        SouthDakota,
        Tennessee,
        Texas,
        Utah,
        Vermont,
        Virginia,
        Washington,
        WestVirginia,
        Wisconsin,
        Wyoming
    }

    public enum Counties 
    {
        Alameda,
        Alpine,
        Amador,
        Butte,
        Calaveras,
        Colusa,
        ContraCosta,
        DelNorte,
        ElDorado,
        Fresno,
        Glenn,
        Humboldt,
        Imperial,
        Inyo,
        Kern,
        Kings,
        Lake,
        Lassen,
        LosAngeles,
        Madera,
        Marin,
        Mariposa,
        Mendocino,
        Merced,
        Modoc,
        Mono,
        Monterey,
        Napa,
        Nevada,
        Orange,
        Placer,
        Plumas,
        Riverside,
        Sacramento,
        SanBenito,
        SanBernardino,
        SanDiego,
        SanFrancisco,
        SanJoaquin,
        SanLuisObispo,
        SanMateo,
        SantaBarbara,
        SantaClara,
        SantaCruz,
        Shasta,
        Sierra,
        Siskiyou,
        Solano,
        Sonoma,
        Stanislaus,
        Sutter,
        Tehama,
        Trinity,
        Tulare,
        Tuolumne,
        Ventura,
        Yolo,
        Yuba
    }

    public enum CountryNames {
        Afghanistan,
        Albania,
        Algeria,
        Andorra,
        Angola,
        Argentina,
        Armenia,
        Australia,
        Austria,
        Azerbaijan,
        Bahamas,
        Bahrain,
        Bangladesh,
        Barbados,
        Belarus,
        Belgium,
        Belize,
        Benin,
        Bhutan,
        Bolivia,
        BosniaHerzegovina,
        Botswana,
        Brazil,
        Brunei,
        Bulgaria,
        Burkina,
        Burundi,
        Cambodia,
        Cameroon,
        Canada,
        CapeVerde,
        CentralAfricanRep,
        Chad,
        Chile,
        China,
        Colombia,
        Comoros,
        Congo,
        CostaRica,
        Croatia,
        Cuba,
        Cyprus,
        CzechRepublic,
        Denmark,
        Djibouti,
        Dominica,
        DominicanRepublic,
        EastTimor,
        Ecuador,
        Egypt,
        ElSalvador,
        EquatorialGuinea,
        Eritrea,
        Estonia,
        Ethiopia,
        Fiji,
        Finland,
        France,
        Gabon,
        Gambia,
        Georgia,
        Germany,
        Ghana,
        Greece,
        Grenada,
        Guatemala,
        Guinea,
        GuineaBissau,
        Guyana,
        Haiti,
        Honduras,
        Hungary,
        Iceland,
        India,
        Indonesia,
        Iran,
        Iraq,
        Ireland,
        Israel,
        Italy,
        IvoryCoast,
        Jamaica,
        Japan,
        Jordan,
        Kazakhstan,
        Kenya,
        Kiribati,
        NorthKorea,
        SouthKorea,
        Kosovo,
        Kuwait,
        Kyrgyzstan,
        Laos,
        Latvia,
        Lebanon,
        Lesotho,
        Liberia,
        Libya,
        Liechtenstein,
        Lithuania,
        Luxembourg,
        Macedonia,
        Madagascar,
        Malawi,
        Malaysia,
        Maldives,
        Mali,
        Malta,
        MarshallIslands,
        Mauritania,
        Mauritius,
        Mexico,
        Micronesia,
        Moldova,
        Monaco,
        Mongolia,
        Montenegro,
        Morocco,
        Mozambique,
        Myanmar,
        Namibia,
        Nauru,
        Nepal,
        Netherlands,
        NewZealand,
        Nicaragua,
        Niger,
        Nigeria,
        Norway,
        Oman,
        Pakistan,
        Palau,
        Panama,
        PapuaNewGuinea,
        Paraguay,
        Peru,
        Philippines,
        Poland,
        Portugal,
        Qatar,
        Romania,
        RussianFederation,
        Rwanda,
        StKittsNevis,
        StLucia,
        SaintVincenttheGrenadines,
        Samoa,
        SanMarino,
        SaoTomePrincipe,
        SaudiArabia,
        Senegal,
        Serbia,
        Seychelles,
        SierraLeone,
        Singapore,
        Slovakia,
        Slovenia,
        SolomonIslands,
        Somalia,
        SouthAfrica,
        SouthSudan,
        Spain,
        SriLanka,
        Sudan,
        Suriname,
        Swaziland,
        Sweden,
        Switzerland,
        Syria,
        Taiwan,
        Tajikistan,
        Tanzania,
        Thailand,
        Togo,
        Tonga,
        TrinidadTobago,
        Tunisia,
        Turkey,
        Turkmenistan,
        Tuvalu,
        Uganda,
        Ukraine,
        UnitedArabEmirates,
        UnitedKingdom,
        UnitedStates,
        Uruguay,
        Uzbekistan,
        Vanuatu,
        VaticanCity,
        Venezuela,
        Vietnam,
        Yemen,
        Zambia,
        Zimbabwe
    }
}
