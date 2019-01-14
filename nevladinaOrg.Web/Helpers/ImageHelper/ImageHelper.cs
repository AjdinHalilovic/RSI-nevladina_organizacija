using nevladinaOrg.Web.Constants;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Paths = nevladinaOrg.Web.Constants.Paths;

namespace nevladinaOrg.Web.Helpers.ImageHelper
{
    public class ImageHelper : IImageHelper
    {
        #region properties
        private IHostingEnvironment _hostingEnvironment;

        #endregion
        public ImageHelper(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<byte[]> SaveAsync(IFormFile image)
        {
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
        public byte[] Save(IFormFile image)
        {
            using (var memoryStream = new MemoryStream())
            {
                image.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        public byte[] GetThumbnail(byte[] image, int width, int height)
        {
            using (var thumb = new MagickImage(image))
            {
                var size = new MagickGeometry(width, height)
                {
                    // This will resize the image to a fixed size without maintaining the aspect ratio.
                    // By default, an image will be resized to fit inside the specified size.
                    IgnoreAspectRatio = true
                };

                // Resize according to size
                thumb.Resize(size);

                using (var stream = new MemoryStream())
                {
                    thumb.Write(stream);
                    return stream.ToArray();
                }
            }
        }
 
        public string UploadDropzoneImage(IFormFile file)
        {
            var streamId = Guid.Empty.ToString();
            try
            {
                streamId = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);

                string pathToImagesThumb = _hostingEnvironment.WebRootPath + Paths.DropzoneTempThumbnails + streamId;
                var ms = new MemoryStream();
                file.CopyTo(ms);
                var thumb = GetThumbnail(ms.ToArray(), 120, 120);
                using (var stream = new FileStream(pathToImagesThumb, FileMode.Create))
                {
                    var msThumb = new MemoryStream(thumb);
                    msThumb.CopyTo(stream);
                }


                string pathToImages = _hostingEnvironment.WebRootPath + Paths.DropzoneTemp + streamId;
                using (var stream = new FileStream(pathToImages, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            catch (Exception ex)
            {
                streamId = Guid.Empty.ToString();
            }

            return streamId;
        }
        public string UploadDropzoneDocument(IFormFile file)
        {
            var streamDocumentId = Guid.Empty.ToString();
            try
            {
                streamDocumentId = Guid.NewGuid().ToString()+System.IO.Path.GetExtension(file.FileName);
                string pathToImages = _hostingEnvironment.WebRootPath + Paths.DropzoneTemp + streamDocumentId;
                using (var stream = new FileStream(pathToImages, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            catch (Exception ex)
            {
                streamDocumentId = Guid.Empty.ToString();
            }
            return streamDocumentId;
        }
        public void DropzoneCleaner(List<string> Files = null)
        {
            var di = new DirectoryInfo(_hostingEnvironment.WebRootPath + Paths.DropzoneTemp);
            var diThumb = new DirectoryInfo(_hostingEnvironment.WebRootPath + Paths.DropzoneTempThumbnails);

            if (Files != null)
            {
                foreach (var file in di.GetFiles())
                {
                    if (Files.Contains(file.Name))
                    {
                        var thumb = diThumb.GetFiles().SingleOrDefault(x => x.Name == file.Name);
                        if (thumb != null)
                        {
                            thumb.Delete();
                        }
                        file.Delete();
                    }
                }
            }
        }
        public void DropzoneCleanerTempThumbs(List<string>Files) {
            var diThumb = new DirectoryInfo(_hostingEnvironment.WebRootPath + Paths.DropzoneTempThumbnails);
            if (Files != null)
            {
                foreach (var file in diThumb.GetFiles())
                {
                    if (Files.Contains(file.Name))
                    {
                        file.Delete();
                    }
                }
            }
        }
    }
}
