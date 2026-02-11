using Core.Models;
using Domain.Entities;
using System;
using LinqKit;
using System.Threading.Tasks;
using System.Linq.Expressions;


namespace Application.Projections
{
    public static class ViewsProjections
    {
        [Expandable(nameof(MapViewModelImpl))]
        public static ViewModel MapViewModel(this ViewsEntity entity)
        {
            throw new Exception();
        }

        private static Expression<Func<ViewsEntity, ViewModel>> MapViewModelImpl()
        {
            return (x) => new ViewModel
            {
                Id = x.Id,
                FingerPrintId = x.FingerPrintId,
                NewsId = x.NewsId,
                Title= x.News.Title,
                UserId = x.UserId,
                UserName = x.User.UserName,
                ViewedOn = x.ViewedOn
            };
        }


    }
}
