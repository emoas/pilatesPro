using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SessionInterface.Sessions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace WebApi.Filters
{
    [ExcludeFromCodeCoverage]
    public class AuthorizationAttributeFilter : Attribute, IAuthorizationFilter
    {
        private readonly ISessionLogic sessions;

        public AuthorizationAttributeFilter(ISessionLogic sessionsLogic)
        {
            this.sessions = sessionsLogic;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string headerToken = context.HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(headerToken))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Usuario no logeado."
                };
            }
            else
            {
                try
                {
                    Guid token = Guid.Parse(headerToken);
                    if (!sessions.IsCorrectToken(token))
                    {
                        context.Result = new ContentResult()
                        {
                            StatusCode = 403,
                            Content = "Token invalido."
                        };
                    }
                }
                catch (FormatException)
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = 401,
                        Content = "Formato invalido de token"
                    };
                }
            }
        }
    }
}
