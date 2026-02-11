using Core.Entities;
using Core.Models;
using Domain.Entities;
using LinqKit;
using Portal.ViewModels;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Application.Projections
{
    public static class UserProjections
    {
        [Expandable(nameof(MapUserModelFromEntityImpl))]
        public static UserModel MapUserModelFromEntity(this AppUserEntity entity)
        {
            throw new NotImplementedException();
        }
        public static Expression<Func<AppUserEntity, UserModel>> MapUserModelFromEntityImpl()
        {
            return entity=> new UserModel()
            {
                UserId = entity.Id,
                AvatarId = entity.AvatarId,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                UserName = entity.UserName,
                Role = entity.UserRoles.Select(x=> x.Role.Name).FirstOrDefault()
            };
        }
    }
}
