using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using webNamana.Domain.Entities.User;


namespace webNamana.Helpers
{
    public static class SessionHelper
    {
        public const string BasketCount = "BasketCount";

        public static UserMinimal User
        {
            get => (UserMinimal)HttpContext.Current.Session["User"];
            set => HttpContext.Current.Session["User"] = value;
        }

        public static int ProductsInCartCount
        {
            get => ((int?)HttpContext.Current.Session["BasketCount"] ?? 0);
            set => HttpContext.Current.Session["BasketCount"] = value;
        }

        public static List<string> ProductsSelectedForOrder
        {
            get => (List<string>)HttpContext.Current.Session["PreOrder"];
            set => HttpContext.Current.Session["PreOrder"] = value;
        }

        public static decimal OrderPrice
        {
            get => (decimal?)HttpContext.Current.Session["OrderPrice"] ?? 0m;
            set => HttpContext.Current.Session["OrderPrice"] = value;
        }

        public static string OrderPaymentType
        {
            get => (string)HttpContext.Current.Session["OrderPaymentType"];
            set => HttpContext.Current.Session["OrderPaymentType"] = value;
        }

        public static string OrderMessage
        {
            get => (string)HttpContext.Current.Session["OrderMessage"];
            set => HttpContext.Current.Session["OrderMessage"] = value;
        }

    }
}
