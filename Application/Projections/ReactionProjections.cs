using Core.Enums;
using Core.Models;
using Domain.Entities;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Application.Projections
{
    public static class ReactionProjections
    {
        public static ReactionEntity MapEntityFromModel(this ReactionModel model)
        {
            return new ReactionEntity
            {
                Id = model.Id,
                NewsId = model.NewsId,
                UserId = model.UserId ?? Guid.Empty,
                ReactionType = model.ReactionType
            };
        }
        [Expandable(nameof(MapModelFromEntityImpl))]
        public static ReactionModel MapModelFromEntity(this ReactionEntity entity)
        {
            throw new NotImplementedException();
        }

        private static Expression<Func<ReactionEntity, ReactionModel>> MapModelFromEntityImpl()
        {
            return (x) => new ReactionModel()
            {
                NewsId = x.NewsId,
                UserId = x.UserId,
                ReactionType = x.ReactionType,
                Id = x.Id,
                Title = x.News.Title,
                UserName = x.User.UserName,
            };
        }
        [Expandable(nameof(MapReactionFromNewsImpl))]
        public static ReactionModel MapReactionFromNews(this NewsEntity entity)
        {
            throw new NotImplementedException();
        }

        private static Expression<Func<NewsEntity, ReactionModel>> MapReactionFromNewsImpl()
        {
            return (x) => new ReactionModel()
            {
                NewsId = x.Id,
                Title = x.Title,
                Sad = x.Reactions.Where(x=> x.ReactionType == ReactionTypes.Sad).Count(),
                Happy = x.Reactions.Where(x=> x.ReactionType == ReactionTypes.Happy).Count(),
                Angry = x.Reactions.Where(x=> x.ReactionType == ReactionTypes.Angry).Count()
            };
        }
    }
}
