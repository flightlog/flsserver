using System.Web.Http;
using System.Web.Http.Filters;
using FLS.Server.Data;
using FLS.Server.Service;
using Microsoft.Practices.Unity;

namespace FLS.Server.WebApi.ActionFilters
{
    public class UserInitActionFilter : ActionFilterAttribute
    {
        //[Dependency]
        //public IIdentityService IdentityService { get; set; }

        //[Dependency]
        //public UserService UserService { get; set; }
        
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var identityService = actionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(IIdentityService)) as IIdentityService;
            var userService = actionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(UserService)) as UserService;
            var controller = (actionContext.ControllerContext.Controller as ApiController);
            if (controller != null)
            {
                var principal = controller.User;

                if (userService != null && identityService != null)
                {
                    var user = userService.GetUser(principal.Identity.Name);
                    identityService.SetUser(user);
                }
            }

            base.OnActionExecuting(actionContext);
        }
    }
}