using BookingHotel.Core.IServices;
using BookingHotel.Core.Models.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace BookingHotel.Core.Services {
    public class ThumbnailStorageService : IThumbnailStorageService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ThumbnailStorageService(IHostingEnvironment hostingEnvironment) {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> GenerateUrlOfImage<T>(T entityModel)
        {
            string thumbnailUrl;
            using (var memoryStream = new MemoryStream())
            {
                // Assuming entityModel has a property representing the thumbnail file
                // You might need to adjust this part according to your actual implementation
                PropertyInfo thumbnailProperty = typeof(T).GetProperty("ThumbnailFile");

                if (thumbnailProperty != null)
                {
                    // Get the thumbnail file from the entity model
                    var thumbnailFile = thumbnailProperty.GetValue(entityModel) as IFormFile;

                    if (thumbnailFile != null)
                    {
                        // Copy data from the thumbnail file to the memory stream asynchronously
                        await thumbnailFile.CopyToAsync(memoryStream);
                    }
                    else
                    {
                        throw new ArgumentException("ThumbnailFile is missing or invalid in the entity model.");
                    }
                }
                else
                {
                    throw new ArgumentException("ThumbnailFile property not found in the entity model.");
                }

                // Convert byte array to Base64 string
                string base64String = Convert.ToBase64String(memoryStream.ToArray());

                // Generate the thumbnail URL (or path)
                thumbnailUrl = $"data:image/jpeg;base64,{base64String}";
            }
            return thumbnailUrl;
        }

        public async Task<string> UploadThumbnail(byte[] thumbnailBytes) {
            try {
                // Tạo đường dẫn và tên file mới cho ảnh thumbnail
                string fileName = $"thumbnail_{Guid.NewGuid().ToString()}.jpg";
                string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Images", "ThumbnailHotel", fileName);

                // Ghi dữ liệu của ảnh thumbnail vào tệp mới
                await File.WriteAllBytesAsync(filePath, thumbnailBytes);

                // Trả về đường dẫn tới ảnh thumbnail đã lưu
                return filePath;
            } catch (Exception ex) {
                // Xử lý lỗi nếu có
                Console.WriteLine($"Error uploading thumbnail: {ex.Message}");
                throw;
            }
        }
    }
}
