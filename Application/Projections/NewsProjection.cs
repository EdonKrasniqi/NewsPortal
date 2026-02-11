using Core.Models;
using Domain.Entities;
using LinqKit;
using Microsoft.EntityFrameworkCore.Internal;
using MimeKit.Encodings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace Application.Projections
{
    public static class NewsProjection
    {
        [Expandable(nameof(MapNewsToModelImpl))]
        public static NewsModel MapNewsToModel(this NewsEntity entity,Guid CurrentUserId)
        {
            throw new Exception();
        }

        private static Expression<Func<NewsEntity,Guid,NewsModel>> MapNewsToModelImpl()
        {
            return (x, currentUser) => new NewsModel
            {
                CategoryId = x.CategoryId,
                Id = x.Id,
                SubTitle = x.SubTitle,
                Title = x.Title,
                isSaved = x.SavedNews.Where(x => x.UserId == currentUser).Any(),
                IsFeatured = x.IsFeatured,
                Content = x.Content,
                CreatedById = x.CreatedById,
                CreatedOnDate = x.CreatedOnDate,
                IsDeleted = x.IsDeleted,
                Tags = x.Tags,
                UpdatedById = x.UpdatedById,
                UpdatedOnDate = x.UpdatedOnDate,
                ImageId = x.ImageId,
                Video = x.Video
            };
        }

        public static NewsEntity MapNewsEntityFromModel(this NewsModel model)
        {
            return new NewsEntity()
            {
                Id = model.Id,
                CategoryId = model.CategoryId,
                Content = model.Content,
                SubTitle = model.SubTitle,
                ImageId = model.ImageId,
                IsDeleted = model.IsDeleted,
                IsFeatured = model.IsFeatured,
                Tags = model.Tags,
                Title = model.Title,
                Video = model.Video,
            };
        }
    }
}
