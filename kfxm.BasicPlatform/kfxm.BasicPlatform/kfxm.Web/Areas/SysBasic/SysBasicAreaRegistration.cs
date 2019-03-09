using System.Web.Mvc;

namespace kfxms.Web.Areas.SysBasic
{
    public class SysBasicAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SysBasic";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SysBasic_default",
                "SysBasic/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional } 
            ); 
        }
    }
}
