using Application.Abstractions;
using Application.Commands.Files;
using Core.Entities;
using Core.Exceptions;
using Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Portal.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Users
{
    public class UpdateUserCommand : IParammeterResultDbCommand<(EditUserModel model, IFormFile file), EditUserModel>
    {
        private readonly UserManager<AppUserEntity> _userManager;
        private readonly UploadFileCommand _file;
        public UpdateUserCommand(UserManager<AppUserEntity> userManager, UploadFileCommand uploadFile)
        {
            _userManager = userManager;
            _file = uploadFile;
        }

        public async Task<EditUserModel> ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, (EditUserModel model, IFormFile file) parameter)
        {
            var user = await dbContext.Users
                .Where(x => x.Id == parameter.model.Id)
                .FirstAsync(cancellationToken);

            if (user == null)
            {
                throw new AppBadDataException();
            }

            user.PhoneNumber = parameter.model.PhoneNumber;
            user.UserName = parameter.model.UserName;
            user.Email = parameter.model.UserEmail;

            var role = await _userManager.GetRolesAsync(user);

            if (role.FirstOrDefault() != parameter.model.Role)
            {
                await _userManager.RemoveFromRoleAsync(user, role.FirstOrDefault());
                await _userManager.AddToRoleAsync(user, parameter.model.Role);
            }

            if (!string.IsNullOrEmpty(parameter.model.Password))
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, parameter.model.Password);
            }
            if (parameter.file != null)
            {
                user.Avatar = await _file.ExecuteAsync(cancellationToken,dbContext, false, (user.AvatarId, parameter.file));
            }

            if (saveChanges)
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return parameter.model;
        }
    }
}
