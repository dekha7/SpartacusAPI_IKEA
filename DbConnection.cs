using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpartacusAPI
{
    public static class DbConnection
    {

        public static string ConnectionString
        {
            get
            {
                return "Data Source=PTPSEELM-NT2069.ikeadt.com;Initial Catalog=PTSQL0172;User ID=SPARTACUS_DEV;Password=sparepli@*!VDS98;Connection Timeout=120;";
                //return "Data Source=CFPSEELM-NT2027.ikeadt.com;Initial Catalog=CFSQL0051;User ID=SPARTACUS_DEV;Password=Spartacusd3v;Connection Timeout=120;";
            }
        }

    }
}
