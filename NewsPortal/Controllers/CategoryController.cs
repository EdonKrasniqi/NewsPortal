using Application.Commands.Categories;
using Autofac;
using Core.Constants;
using Core.Models;
using Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    public class CategoryController : BaseController
    {
        public CategoryController(ILifetimeScope scope) : base(scope) { }

        [HttpGet]
        public async Task<ActionResult<List<CategoryModel>>> GetAllCategoriesAsync(CancellationToken cancellationToken)
        {
            var categories = await scope.Resolve<GetCategoriesQuery>()
                 .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, Guid.Empty);

            return Ok(categories);
        }
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult> AddCategoyAsync(CancellationToken cancellationToken, CategoryModel category)
        {
            var res = await scope.Resolve<AddOrUpdateCategories>()
                 .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, category);

            return Ok(res);

        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetCategoryAsync(CancellationToken cancellationToken, Guid Id)
        {
            var category = await scope.Resolve<GetCategoriesQuery>()
                .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, Id);

            return Ok(category);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult> DeleteCategoryAsync(CancellationToken cancellationToken, Guid id)
        {
            await scope.Resolve<DeleteCategoryCommand>()
                 .ExecuteAsync(cancellationToken, scope.Resolve<AppDbContext>(), true, id);

            return Ok();
        }
    }
}
