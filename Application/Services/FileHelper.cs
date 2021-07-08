using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace API.Extensions
{
    public static class FileHelper
    {
        public static async Task<string> UploadFile(IFormFile file, string path)
        {
            string fileName = DateTime.Now.ToString("yyyymmssfff") + "_" + Guid.NewGuid().ToString();
            string fileToUpload = fileName + Path.GetExtension(file.FileName);
            string pathToUpload = Path.Combine(path, fileToUpload);

            using (var fileStream = new FileStream(pathToUpload, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return fileName;

        }
    }
}