using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;
using System.Web.Http.Cors;

[assembly: OwinStartup(typeof(MobileWebApi.Startup))]

namespace MobileWebApi
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            //Enables the Cross Origin Resource Sharing
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            //Declaring the Intsof Auth Provider
            var intsofAuthProvider = new IntsofAuthorizationServerProvider();

            OAuthAuthorizationServerOptions authOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString(String.Concat("/", MobileWebApiResource.OAuthAuthorizationTokenUrl)),
                Provider = intsofAuthProvider,
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(Convert.ToInt32(MobileWebApiResource.AccessTokenExpireTimeSpanMinutes)),
                AllowInsecureHttp = Convert.ToBoolean(MobileWebApiResource.AllowInsecureHttp),
                RefreshTokenProvider = new RefreshTokenProvider()
            };

            app.UseOAuthAuthorizationServer(authOptions);

            //app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            ConfigureOAuth(app);
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
            //Token Consumption
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
        }
    }
}
