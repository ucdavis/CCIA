using System;
using System.Collections.Generic;
using System.Linq;

namespace CCIA.Helpers
{
    public class CertYearFinder
    {
        private static int _certYear;

        public static int CertYear
        {
            get
            {
                if (_certYear== 0)
                {
                    if (DateTime.Now.Month >= 10 && DateTime.Now.Month <= 12)
                    {
                        _certYear = DateTime.Now.Year + 1;
                    }
                    else
                    {
                        _certYear = DateTime.Now.Year;
                    }
                }
                return _certYear;
            }
        }

        public static List<int> certYearList
        {
            get{
                return Enumerable.Range(2007, CertYear - 2006).ToList();
            }
        }

        public static List<int> certYearListReverse
        {
            get{
                return Enumerable.Range(2007, CertYear - 2006).Reverse().ToList();
            }
        }
    }
}
