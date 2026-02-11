using Core.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Projections
{
    public  static class SiteConfigurationProjections
    {

        public static SiteConfigurationModel MapModelFromEntity( this SiteConfigsEntity entity)
        {
            return new SiteConfigurationModel()
            {
                Id = entity.Id,
                HeaderColor = entity.HeaderColor,
                FooterColor = entity.FooterColor,
                FooterLogoId = entity.FooterLogoId,
                HeaderLogoId = entity.HeaderLogoId,
                LatUpdatedOn = entity.LatUpdatedOn,
                UpdatedBy = entity.UpdatedBy?.UserName
            };
        }

        public static SiteConfigsEntity MapEntityFromModel(this SiteConfigurationModel model)
        {
            return new SiteConfigsEntity()
            {
                Id = model.Id,
                FooterColor = model.FooterColor,
                FooterLogoId = model.FooterLogoId,
                HeaderColor = model.HeaderColor,
                HeaderLogoId = model.HeaderLogoId,
            };
        }
    }
}
