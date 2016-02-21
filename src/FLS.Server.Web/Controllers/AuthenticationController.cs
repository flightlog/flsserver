using FLS.Server.WebApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace FLS.Server.WebApi.Controllers
{
    [RoutePrefix("auth")]
    public class AuthenticationController : Controller
    {
        private IAuthenticationManager Authentication { get { return Request.GetOwinContext().Authentication; } }

        [AllowAnonymous]
        [HttpGet, Route("external")]
        public ActionResult ExternalJson(string provider)
        {
            string redirectUri = "";

            if (!User.Identity.IsAuthenticated)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            // create bearer token and return... like /Token
            return Json(new { token = "todo" }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet, Route("external/signin")]
        public ActionResult ExternalSignIn(string provider, string error = null)
        {
            if(string.IsNullOrEmpty(provider))
                return View(new ExternalChallangeResultViewModel { Failed = true, ErrorMessage = "no provider set." });

            if (!string.IsNullOrEmpty(error))
                return View(new ExternalChallangeResultViewModel { Failed = true, ErrorMessage = error });

            // unauthorized - send challange (redirects to login provider)
            if (!User.Identity.IsAuthenticated)
            {
                var owinContext = Request.GetOwinContext();
                owinContext.Authentication.Challenge(provider);
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            return View(new ExternalChallangeResultViewModel { Failed = false });
        }




            //var redirectUriValidationResult = ValidateClientAndRedirectUri(Request, ref redirectUri);

            //if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
            //    return BadRequest(redirectUriValidationResult);

            //var claimsIdentity = User.Identity as ClaimsIdentity;
            ////var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            //if (claimsIdentity == null)
            //    return InternalServerError();

            // todo: reenable check login from correct provider... maybe.
            //if (externalLogin.LoginProvider != provider)
            //{
            //    Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            //    return new ChallengeResult(provider, this);
            //}

            //IdentityUser user = await _repo.FindAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

            //bool hasRegistered = user != null;

            //redirectUri = string.Format("{0}#external_access_token={1}&provider={2}&haslocalaccount={3}&external_user_name={4}",
            //                                redirectUri,
            //                                externalLogin.ExternalAccessToken,
            //                                externalLogin.LoginProvider,
            //                                hasRegistered.ToString(),
            //                                externalLogin.UserName);
            
        //private string ValidateClientAndRedirectUri(HttpRequestMessage request, ref string redirectUriOutput)
        //{

        //    Uri redirectUri;
        //    var redirectUriString = GetQueryString(Request, "redirect_uri");

        //    if (string.IsNullOrWhiteSpace(redirectUriString))
        //        return "redirect_uri is required";

        //    bool validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);

        //    if (!validUri)
        //        return "redirect_uri is invalid";

        //    var clientId = GetQueryString(Request, "client_id");

        //    if (string.IsNullOrWhiteSpace(clientId))
        //        return "client_Id is required";

        //    //var client = _repo.FindClient(clientId);

        //    //if (client == null)
        //    //    return string.Format("Client_id '{0}' is not registered in the system.", clientId);

        //    //if (!string.Equals(client.AllowedOrigin, redirectUri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
        //    //    return string.Format("The given URL is not allowed by Client_id '{0}' configuration.", clientId);

        //    redirectUriOutput = redirectUri.AbsoluteUri;

        //    return string.Empty;
        //}

        //private string GetQueryString(HttpRequestMessage request, string key)
        //{
        //    var queryStrings = request.GetQueryNameValuePairs();

        //    if (queryStrings == null)
        //        return null;

        //    var match = queryStrings.FirstOrDefault(keyValue => string.Compare(keyValue.Key, key, true) == 0);

        //    if (string.IsNullOrEmpty(match.Value))
        //        return null;

        //    return match.Value;
        //}


    }
}
