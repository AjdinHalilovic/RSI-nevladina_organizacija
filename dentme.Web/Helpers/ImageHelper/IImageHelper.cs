using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nevladinaOrg.Web.Helpers.ImageHelper
{
    public interface IImageHelper
    {
        Task<byte[]> SaveAsync(IFormFile image);
        byte[] Save(IFormFile image);
        byte[] GetThumbnail(byte[] image, int width, int height);
        string UploadDropzoneImage(IFormFile file);
        string UploadDropzoneDocument(IFormFile file);
        void DropzoneCleaner(List<string> Files = null);
        void DropzoneCleanerTempThumbs(List<string> Files);
    }
}
