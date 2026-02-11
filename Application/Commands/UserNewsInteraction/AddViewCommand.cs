using Application.Abstractions;
using AutoMapper;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using DocumentFormat.OpenXml.EMMA;
using Domain.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.UserNewsInteraction
{
    public class AddViewCommand : IParameterDbCommand<ViewModel>
    {
        private readonly IAuthorizationInterface authorizationInterface;

        public AddViewCommand(IAuthorizationInterface authorizationInterface)
        {
            this.authorizationInterface = authorizationInterface;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken, AppDbContext dbContext, bool saveChanges, ViewModel parameter)
        {
            var entity = new ViewsEntity();
            var userId = authorizationInterface.GetCurrentUserId();
            entity.ViewedOn = DateTime.Now;
            entity.NewsId = parameter.NewsId;

            if(userId.HasValue)
            {
                entity.UserId = userId;
            }
            else
            {
                entity.FingerPrintId = parameter.FingerPrintId;
            }

            if(entity == null)
            {
                throw new AppBadDataException();
            }

            await dbContext.Views.AddAsync(entity, cancellationToken);

            if (saveChanges)
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }


        }
    }
}
