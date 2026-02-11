using Application.Abstractions;
using Application.Projections;
using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Categories
{
    public class AddOrUpdateCategories : IParammeterResultDbCommand<CategoryModel,Guid>
    {
        public AddOrUpdateCategories(){}
        public async Task<Guid> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, CategoryModel parameter)
        {
            if (parameter == null)
            {
                throw new AppBadDataException();
            }
            var category = parameter.MapModelToEntity();

            if (category.Id == Guid.Empty)
            {
                await dbContext.Categories.AddAsync(category, cancellationToken);
            }
            else
            {
                dbContext.Categories.Update(category);
            }

            if (saveChanges)
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            var id = category.Id;
           
            return id;
        }
    }
}
