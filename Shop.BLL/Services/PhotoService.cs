using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Shop.BLL.Models;
using Shop.DAL.Data.Entities;

namespace Shop.BLL.Services
{
    public class PhotoService
    {
        private string userImages_FolderPath;
        private string advertImages_FolderPath;
        private readonly UserManager<User> userManager;
        public PhotoService(UserManager<User> _userManager, IWebHostEnvironment webHostEnvironment)
        {
            userManager = _userManager;
            Console.WriteLine(webHostEnvironment.WebRootPath);
            userImages_FolderPath = Path.Combine(webHostEnvironment.ContentRootPath, "wwwroot\\UserPhotos\\");
            advertImages_FolderPath = Path.Combine(webHostEnvironment.ContentRootPath, "wwwroot\\AdvertPhotos\\");
            if (!Directory.Exists(userImages_FolderPath))
            {
                Directory.CreateDirectory(userImages_FolderPath);
            }
            if (!Directory.Exists(advertImages_FolderPath))
            {
                Directory.CreateDirectory(advertImages_FolderPath);
            }

        }
        public async Task<ServicesResultModel> SetUserProfileImage(IFormFile photo, string userId)
        {
            var ext = photo.FileName.Substring(photo.FileName.LastIndexOf('.'));
            var fullPath = Path.Combine(userImages_FolderPath, $"{userId}{ext}");
            using (var fs = new FileStream(fullPath, FileMode.Create))
            {
                await photo.CopyToAsync(fs);
            }
            var user = await userManager.FindByIdAsync(userId);
            user.ImageURL = fullPath;
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new ServicesResultModel { ResultCode = ResultCodes.Success };
            }
            else
            {
                return new ServicesResultModel { ResultCode = ResultCodes.Fail, Errors = result.Errors.Select(x => x.Description).ToList() };
            }
        }

        //public async Task<ServicesResultModel> SetAdvertProfileImage(IFormFile file, Guid advertId)
        //{
        //    var path = Path.Combine();
        //}

        public async Task<ServicesResultModel> DeleteUserImage(string userId)
        {
            var filePath = new DirectoryInfo(userImages_FolderPath + userId).GetFiles().FirstOrDefault(x => x.FullName.StartsWith(userId.ToString()));
            if (filePath != null)
            {
                filePath.Delete();
                var user = await userManager.FindByIdAsync(userId);
                user.ImageURL = null;
                var deleteImage_Result = await userManager.UpdateAsync(user);
                if (deleteImage_Result.Succeeded)
                {
                    return new ServicesResultModel { ResultCode = ResultCodes.Success };
                }
                else
                {
                    return new ServicesResultModel { ResultCode = ResultCodes.Fail, Errors = deleteImage_Result.Errors.Select(x => x.Description).ToList() };
                }
            }
            else
            {
                return new ServicesResultModel { ResultCode = ResultCodes.Fail, Errors = new List<string> { "No file found" } };
            }
        }

        public void DeleteAdvertImage(Guid advertId)
        {

        }
    }
}
