using Application.Abstractions;
using Core.Constants;
using Core.Entities;
using Core.Enums;
using Domain.Entities;
using Infrastructure.Database;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Portal.Models;
using System.Linq.Expressions;

namespace Application.Commands.Reports
{
    public class GetDashbordViewModelQuery : IParammeterResultDbCommand<DashbordMainFilter, DashboardViewModel>
    {
        private readonly UserManager<AppUserEntity> userManager;

        public GetDashbordViewModelQuery(UserManager<AppUserEntity> userMenager)
        {
            this.userManager = userMenager;
        }

        public async Task<DashboardViewModel> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, DashbordMainFilter parameter)
        {
            var model = new DashboardViewModel();
            model.News = await GetDashboardStisticsAsync<NewsEntity>(cancellationToken,x=> x.CreatedOnDate, dbContext, parameter);
            model.Views = await GetDashboardStisticsAsync<ViewsEntity>(cancellationToken,x=> x.ViewedOn ,dbContext, parameter);
            model.Angry = await dbContext.Reactions.Where(x => x.ReactionType == ReactionTypes.Angry).CountAsync(cancellationToken);
            model.Sad = await dbContext.Reactions.Where(x => x.ReactionType == ReactionTypes.Sad).CountAsync(cancellationToken);
            model.Happy = await dbContext.Reactions.Where(x => x.ReactionType == ReactionTypes.Happy).CountAsync(cancellationToken);
            model.Users = await dbContext.Users.CountAsync(cancellationToken);
            model.SavedNews = await dbContext.Saved.CountAsync(cancellationToken);
            var admins = await userManager.GetUsersInRoleAsync(Roles.Admin);
            var simpleUsers = await userManager.GetUsersInRoleAsync(Roles.SimpleUser);
            model.Admins = admins.Count();
            model.Users = simpleUsers.Count();

            return model;
        }
     
        private async Task<int[]> GetDashboardStisticsAsync<T>(CancellationToken cancellationToken, Expression<Func<T,DateTime>> expression, AppDbContext appContext, DashbordMainFilter filter) where T : class, new()
        {
            int[] model;

            if (filter == DashbordMainFilter.Week)
            {
                model = new int[7];

                var day = DateTime.Now.Date.Day % 7;

                List<int> dates = Enumerable.Range(1, day).Select((x, index) => -(x - index)).ToList();

                var list = await appContext.Set<T>().Where(x=> dates.Contains(expression.Invoke(x).Day) && expression.Invoke(x).Month == DateTime.Now.Month
                    && expression.Invoke(x).Year == DateTime.Now.Year)
                    .Select(x => new { expression.Invoke(x).Day })
                   .ToListAsync(cancellationToken);

                for (int i = 1; i <= day; i++)
                {
                    var dateOfIteration = DateTime.Now.AddDays(-(day - i)).Day;
                    model[i - 1] = list.Where(x => x.Day == dateOfIteration).Count();
                }
            }
            else if (filter == DashbordMainFilter.Mounth)
            {
                model = new int[4];
                var mounth = DateTime.Now.Month;
                var list = await appContext.Set<T>().Where(x => expression.Invoke(x).Month == mounth).Select(x => new { expression.Invoke(x).Day }).ToListAsync(cancellationToken);
                model[0] = list.Where(x => x.Day >= 1 && x.Day <= 7).Count();
                model[1] = list.Where(x => x.Day >= 8 && x.Day <= 14).Count();
                model[2] = list.Where(x => x.Day >= 15 && x.Day <= 21).Count();
                model[3] = list.Where(x => x.Day >= 22 && x.Day <= 31).Count();
            }
            else
            {
                model = new int[12];
                var mounth = DateTime.Now.Month;
                var newsList = await appContext.Set<T>()
                    .Where(x => expression.Invoke(x).Month == DateTime.Now.Year)
                    .Select(x => new { expression.Invoke(x).Month })
                    .ToListAsync(cancellationToken);

                for (int i = 1; i <= 12; i++)
                {
                    model[i - 1] = newsList.Where(x => x.Month == i).Count();
                }
            }

            return model;
        }
    }
}
