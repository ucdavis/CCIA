using System;

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
    }
}
