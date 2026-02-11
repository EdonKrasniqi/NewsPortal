using Core.Models;
using Domain.Entities;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Application.Projections
{
    public static class CategoryProjection
    {
        [Expandable(nameof(MapEntityToModelImpl))]
        public static CategoryModel MapEntityToModel(this CategoryEntity model)
        {
            throw new NotImplementedException();
        }

        private static Expression<Func<CategoryEntity, CategoryModel>> MapEntityToModelImpl()
        {
            return (x) => new CategoryModel()
            {
                Id = x.Id,
                Name = x.Name,
                ShowOnline = x.ShowOnline,
                Description = x.Description,
            };
        }

        public static CategoryEntity MapModelToEntity(this CategoryModel model)
        {
            return new CategoryEntity
            {
                Id = model.Id,
                Name = model.Name,
                ShowOnline = model.ShowOnline,
                Description = model.Description,
            };
        }
    }
}
