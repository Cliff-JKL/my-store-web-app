using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace practice.Services
{
    public static class CookiesService
    {
        public static string GetLoginCookie(Controller controller)
        {
            return controller.Request.Cookies["login"];
        }

        public static string GetPasswordCookie(Controller controller)
        {
            return controller.Request.Cookies["password"];
        }

        public static void UpdatePersonCookies(Controller controller, string login, string password)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(7);
            controller.Response.Cookies.Append("login", login, options);
            controller.Response.Cookies.Append("password", password, options);
        }

        public static void DeletePersonCookies(Controller controller)
        {
            controller.Response.Cookies.Delete("login");
            controller.Response.Cookies.Delete("password");
        }

        public static void DeleteShoppingCartCookies(Controller controller)
        {
            controller.Response.Cookies.Delete("cartData");
            controller.Response.Cookies.Delete("cartCost");
        }

        public static bool IsPersonCookiesExist(Controller controller)
        {
            return (controller.Request.Cookies["login"] != null && controller.Request.Cookies["password"] != null);
        }

        public static bool IsShoppingCartCookiesExist(Controller controller)
        {
            return (controller.Request.Cookies["cartCost"] != null && controller.Request.Cookies["cartData"] != null);
        }

        public static string GetShoppingCartCostCookie(Controller controller)
        {
            return controller.Request.Cookies["cartCost"];
        }

        public static string GetShoppingCartDataCookie(Controller controller)
        {
            return controller.Request.Cookies["cartData"];
        }

        public static void UpdateShoppingCartCookies(Controller controller, string cartCost, string cartData)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(7);
            controller.Response.Cookies.Append("cartCost", cartCost, options);
            controller.Response.Cookies.Append("cartData", cartData, options);
        }
    }
}
