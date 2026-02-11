using Autofac;
using Microsoft.AspNetCore.Mvc;

namespace Portal.Controllers.Base
{
    [Route("[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILifetimeScope scope;
        public BaseController(ILifetimeScope scope)
        {
            this.scope = scope;
        }
    }
}
