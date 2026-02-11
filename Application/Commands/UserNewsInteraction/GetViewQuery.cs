using Application.Abstractions;
using Application.Projections;
using AutoMapper;
using Core.Enums;
using Core.Models;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.UserNewsInteraction
{
    public class GetViewQuery : IParammeterResultDbCommand<(BaseFilter filter, Guid? NewsId, Guid? userId), List<ViewModel>>
    {
        public GetViewQuery(){}
        public async Task<List<ViewModel>> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, (BaseFilter filter, Guid? NewsId, Guid? userId) parameter)
        {
            var views = new List<ViewModel>();

            if (parameter.filter == BaseFilter.All)
            {
               var  viewsList = await dbContext.Views.Include(x => x.News).ToListAsync(cancellationToken);

                views = viewsList.GroupBy(x=> x.NewsId)
                  .Select(x=> new ViewModel()
                    {
                        NewsId = x.FirstOrDefault()!.NewsId,
                        Title = x.FirstOrDefault().News.Title,
                        NumberOfViews = x.Count()
                    })
                    .ToList();

                return views;
            }
            else if (parameter.filter == BaseFilter.Users)
            {
                views = await dbContext.Views
                    .Select(x=> x.MapViewModel())
                    .Where(x => x.UserId == parameter.userId)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                views = await dbContext.Views.Select(x => x.MapViewModel())
                    .Where(x => x.NewsId == parameter.NewsId).ToListAsync(cancellationToken);
            }

            return views;
        }
    }
}
