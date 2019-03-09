using System.Web.Mvc;

namespace kfxms.Web.Areas.Key
{
    public class KeyAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Key";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Key_default",
                "Key/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
