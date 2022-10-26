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
        private readonly UserManager<ApplicationUser> userManager;
        public PhotoService(UserManager<ApplicationUser> _userManager, IWebHostEnvironment webHostEnvironment)
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
        

        public void DeleteAdvertImage(Guid advertId)
        {

        }
    }
}
