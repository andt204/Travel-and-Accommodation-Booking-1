using BookingHotel.Core.IServices;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BookingHotel.Core.Services {
    public class ThumbnailStorageService : IThumbnailStorageService {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ThumbnailStorageService(IHostingEnvironment hostingEnvironment) {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> UploadThumbnail(byte[] thumbnailBytes) {
            try {
                // Tạo đường dẫn và tên file mới cho ảnh thumbnail
                string fileName = $"thumbnail_{Guid.NewGuid().ToString()}.jpg";
                string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "ThumbnailHotel", fileName);

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
