using Application.Abstractions;
using Application.Projections;
using AutoMapper;
using Core.Models;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Categories
{
    public class GetCategoriesQuery : IParammeterResultDbCommand<Guid, List<CategoryModel>>
    {
        public GetCategoriesQuery(){}
        public async Task<List<CategoryModel>> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, Guid parameter)
        {
            var categories = new List<CategoryModel>();
            if (parameter == Guid.Empty)
            {
                categories = await dbContext.Categories
                    .Where(x => x.ShowOnline)
                    .Select(x=> x.MapEntityToModel())
                    .ToListAsync(cancellationToken);
            }
            else
            {
                categories = await dbContext.Categories
                    .Where(x => x.ShowOnline && x.Id == parameter)
                    .Select(x => x.MapEntityToModel())
                    .ToListAsync(cancellationToken);
            }

            return categories;
        }
    }
}
