using Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class FileHelper
    {

        public static async Task<SaveFileModel> ConvertToSaveModelAsync(this IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var saveFileModel = new SaveFileModel
            {
                FileName = Path.GetFileNameWithoutExtension(file.FileName),
                FileExtension = Path.GetExtension(file.FileName)
            };

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                saveFileModel.Data = memoryStream.ToArray();
            }

            return saveFileModel;
        }

        public static string GetBase64String(FileModel model)
        {
            var img = $"data:{model.Type};base64," + Convert.ToBase64String(model.Data);
            return img;
        }
    }
}
