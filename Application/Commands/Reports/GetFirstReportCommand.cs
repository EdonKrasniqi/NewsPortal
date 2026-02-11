using Application.Abstractions;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Models;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Reports
{
    public class GetReportCommand : IParammeterResultDbCommand<ReportModel, ReportModel>
    {
        public UserManager<AppUserEntity> userManager;

        public GetReportCommand(UserManager<AppUserEntity> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<ReportModel> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, ReportModel model)
        {
            var result = new List<ReportItemModel>();
            if (model.MainElement == MainElement.Users)
            {
                if (model.Display == Display.Views)
                {
                    result = await GetReportItemsAsync(cancellationToken, dbContext, model);
                }
                else if (model.Display == Display.SavedNews)
                {
                    result = await GetReportItemsAsync<SavedNewsEntity>(cancellationToken, dbContext, x => true, model.MainElement);
                }
                else if (model.Display == Display.Reactions)
                {
                    result = await GetReportItemsAsync<ReactionEntity>(cancellationToken, dbContext, x => true, model.MainElement);

                }
            }

            else if (model.MainElement == MainElement.News)
            {
                if (model.Display == Display.Views)
                {
                    result = await GetReportItemsAsync(cancellationToken, dbContext, model);
                }
                else if (model.Display == Display.SavedNews)
                {
                    result = await GetReportItemsAsync<SavedNewsEntity>(cancellationToken, dbContext, x => true, model.MainElement);
                }
                else if (model.Display == Display.Reactions)
                {
                    result = await GetReportItemsAsync<ReactionEntity>(cancellationToken, dbContext, x => true, model.MainElement);

                }
            }

            model.Result = result;
            return model;
        }

        private async Task<List<ReportItemModel>> GetReportItemsAsync<T>(CancellationToken cancellationToken,
            AppDbContext dbContext, Expression<Func<T, bool>> predicate, MainElement mainElement) where T : class, IUserNews, new()
        {
            var model = new List<ReportItemModel>();

            var result = await dbContext.Set<T>()
                        .Where(predicate)
                        .Select(x => mainElement == MainElement.Users ? new ListItemModel(x.UserId, x.User.UserName) : new ListItemModel(x.NewsId, x.News.Title))
                        .ToListAsync(cancellationToken);

            var groups = result.GroupBy(x => x.Id).ToList();

            foreach (var item in groups)
            {
                model.Add(new ReportItemModel
                {
                    Id = item.First().Id,
                    Name = item.FirstOrDefault().Name ?? "",
                    Number = item.Count()
                });
            }

            return model;
        }

        private async Task<List<ReportItemModel>> GetReportItemsAsync(CancellationToken cancellationToken, AppDbContext dbContext, ReportModel model)
        {
            var result = new List<ReportItemModel>();
            var viewsList = await dbContext.Views
                       .Where(x => x.ViewedOn >= model.From && x.ViewedOn <= model.To && x.UserId != null)
                        .Select(x => model.MainElement == MainElement.Users ? new ListItemModel(x.UserId ?? Guid.NewGuid(), x.User.UserName) : new ListItemModel(x.NewsId, x.News.Title))
                        .ToListAsync(cancellationToken);
                        
            var views = viewsList.GroupBy(x => x.Id)
                       .ToList();

            foreach (var item in views)
            {
                result.Add(new ReportItemModel
                {
                    Id = item.FirstOrDefault().Id,
                    Name = item.FirstOrDefault().Name ?? "",
                    Number = item.Count()
                });
            }

            return result;
        }
    }
}
