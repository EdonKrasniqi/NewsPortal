using Autofac;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Security
{
    public class Authorization : IAuthorizationInterface
    {
        private readonly IComponentContext _scope;
        private readonly IHttpContextAccessor _contextAccessor;
        private Guid? UserId;

        public Authorization(IComponentContext scope, IHttpContextAccessor contextAccessor)
        {
            _scope = scope;
            _contextAccessor = contextAccessor;
        }

        public Guid? GetCurrentUserId()
        {
            return this.UserId;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            var token = GetCurrentToken();
            if (string.IsNullOrEmpty(token) || token == "null")
            {
                return;
            }
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;

            var userId = tokenS.Claims.Where(x => x.Type == "sub").Select(x => x.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(userId))
            {
                this.UserId = Guid.Parse(userId);
            }

        }

        public bool isAdmin()
        {
            throw new NotImplementedException();
        }

        private string GetCurrentToken()
        {
            var token = _contextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token))
            {
                var query = _contextAccessor.HttpContext?.Request.Query["access_token"];
                if (query.HasValue)
                {
                    return query.Value;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return token.Split(" ")[1];
            }
        }
    }
}
