using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using beitostolen_live_api.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Facebook;
using beitostolen_live_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.HttpOverrides;

namespace beitostolen_live_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetSection("AppSettings")["DefaultConnection"])
            );

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "github";
            })
            .AddFacebook("Facebook", options =>
            {
                options.AppId = Configuration["Facebook:AppId"];
                options.AppSecret = Configuration["Facebook:AppSecret"];
                options.CallbackPath = new PathString("/signin-facebook");

                options.SaveTokens = true;
            })
            .AddCookie()
            .AddOAuth("github", "Github", options =>
            {
                options.ClientId = Configuration["GitHub:ClientId"];
                options.ClientSecret = Configuration["GitHub:ClientSecret"];
                options.CallbackPath = new PathString("/signin-github");

                options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                options.TokenEndpoint = "https://github.com/login/oauth/access_token";
                options.UserInformationEndpoint = "https://api.github.com/user";

                //options.ClaimsIssuer = "OAuth2-Github";
                options.SaveTokens = true;

                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                options.ClaimActions.MapJsonKey("urn:github:login", "login");
                options.ClaimActions.MapJsonKey("urn:github:url", "html_url");
                options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");

                options.Events = new OAuthEvents
                {
                    //OnRemoteFailure = HandleOnRemoteFailure,
                    OnCreatingTicket = async context =>
                    {
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                        var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();

                        var user = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());

                        context.HttpContext.Response.Cookies.Append("token", context.AccessToken);

                        context.RunClaimActions(user.RootElement);
                    }
                };
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddControllersWithViews().AddCookieTempDataProvider();
            services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Admin", "Admin");
                    options.Conventions.AuthorizePage("/Day", "FacebookUser");
                    options.Conventions.AuthorizePage("/ThankYou", "FacebookUser");
                    options.Conventions.AllowAnonymousToPage("/Index");
                });

            services.AddAuthorization(options =>
            {
                /*options.AddPolicy("FacebookUser", policyBuilder =>
                    policyBuilder.RequireAssertion(FacebookAuthPolicy));
                options.AddPolicy("Admin", policyBuilder =>
                    policyBuilder.RequireClaim(
                        "urn:github:login",
                        Configuration["Authorization:AdminUsers"].Split(',')));*/
                options.AddPolicy("FacebookUser", policyBuilder =>
                {
                    policyBuilder.AddAuthenticationSchemes("Facebook");
                    policyBuilder.RequireAuthenticatedUser();
                });
                options.AddPolicy("Admin", policyBuilder =>
                {
                    policyBuilder.AddAuthenticationSchemes("github");
                    policyBuilder.RequireClaim(
                        "urn:github:login",
                        Configuration["Authorization:AdminUsers"].Split(','));
                });
            });

            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IResponseService, ResponseService>();
            services.AddScoped<IAlternativService, AlternativService>();
            services.AddScoped<IWinnerService, WinnerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private bool FacebookAuthPolicy(AuthorizationHandlerContext context)
        {
            var claim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            return claim != null;
        }
    }
}
