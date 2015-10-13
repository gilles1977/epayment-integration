using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace IntegrationWeb
{
    public static class ConfigurationUtil
    {
        public static readonly string ApiUrl = ConfigurationManager.AppSettings["apiUrl"] ?? "http://localhost:53851/";

        public static readonly string PassPhrase = ConfigurationManager.AppSettings["passPhrase"] ?? string.Empty;

        public static readonly string AcceptUrl = ConfigurationManager.AppSettings["AcceptUrl"] ??
                                         new Uri(HttpContext.Current.Request.Url, "/Products/Success").ToString();

        public static readonly string DirectPassword = ConfigurationManager.AppSettings["DirectPassword"] ?? "Asm441190bxl";

        public static readonly string DirectUserName = ConfigurationManager.AppSettings["DirectUserName"] ?? "CGIAPI";

        public static readonly string StandardOrder = ConfigurationManager.AppSettings["StandardOrder"] ?? "https://webdev.oglan.local/ncol/test/orderstandard_utf8.asp";

        public static readonly string DirectQuery = ConfigurationManager.AppSettings["DirectQuery"] ?? "https://webdev.oglan.local/ncol/test/querydirect.asp";

        public static readonly string DirectOrder = ConfigurationManager.AppSettings["DirectOrder"] ?? "https://webdev.oglan.local/ncol/test/orderdirect.asp";
        
        public static readonly string DirectMaintenance = ConfigurationManager.AppSettings["DirectMaintenance"] ?? "https://webdev.oglan.local/ncol/test/maintenancedirect.asp";

        public static readonly string PspId = ConfigurationManager.AppSettings["PSPID"] ?? "CGIDEV";

        public static readonly string DefaultOperation = ConfigurationManager.AppSettings["DefaultOperation"] ?? "RES";

        public static readonly string DefaultCurrency = ConfigurationManager.AppSettings["DefaultCurrency"] ?? "EUR";
    }
}