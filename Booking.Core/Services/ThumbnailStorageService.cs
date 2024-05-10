using BookingHotel.Core.IServices;
using BookingHotel.Core.Models.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BookingHotel.Core.Services {
    public class ThumbnailStorageService : IThumbnailStorageService {
        //private readonly IHostingEnvironment _hostingEnvironment;

        //public ThumbnailStorageService(IHostingEnvironment hostingEnvironment) {
        //    _hostingEnvironment = hostingEnvironment;
        //}

        //public async Task<string> UploadThumbnail(byte[] thumbnailBytes) {
        //    try {
        //        // Tạo đường dẫn và tên file mới cho ảnh thumbnail
        //        string fileName = $"thumbnail_{Guid.NewGuid().ToString()}.jpg";
        //        string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Images", "ThumbnailHotel", fileName);

        //        // Ghi dữ liệu của ảnh thumbnail vào tệp mới
        //        await File.WriteAllBytesAsync(filePath, thumbnailBytes);

        //        // Trả về đường dẫn tới ảnh thumbnail đã lưu
        //        return filePath;
        //    } catch (Exception ex) {
        //        // Xử lý lỗi nếu có
        //        Console.WriteLine($"Error uploading thumbnail: {ex.Message}");
        //        throw;
        //    }
        //}
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };
        private const int MaxImageSizeInBytes = 5 * 1024 * 1024; // 5 MB

        public ThumbnailStorageService(IHostingEnvironment hostingEnvironment) {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> UploadThumbnail(byte[] thumbnailBytes) {
            try {
                // Check if image size exceeds the maximum allowed size
                if (thumbnailBytes.Length > MaxImageSizeInBytes) {
                    throw new Exception("Image size exceeds the maximum allowed size.");
                }

                // Get the file extension
                string fileExtension = GetFileExtension(thumbnailBytes);

                // Check if the file extension is allowed
                if (!_allowedExtensions.Contains(fileExtension)) {
                    throw new Exception("Invalid file type. Only JPG, JPEG, PNG files are allowed.");
                }

                // Generate a unique file name with the correct extension
                string fileName = $"thumbnail_{Guid.NewGuid().ToString()}{fileExtension}";

                // Tạo đường dẫn và tên file mới cho ảnh thumbnail
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

        private string GetFileExtension(byte[] fileBytes) {
            // Define known file signatures
            Dictionary<string, List<byte[]>> fileSignatures = new Dictionary<string, List<byte[]>> {
                { ".jpg", new List<byte[]> { new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 }, new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 } } }, // JPEG
                { ".jpeg", new List<byte[]> { new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 }, new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 } } }, // JPEG
                { ".png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47 } } } // PNG
            };

            // Get the first 4 bytes of the file
            byte[] signature = fileBytes.Take(4).ToArray();

            // Check which file signature matches
            foreach (var kvp in fileSignatures) {
                foreach (var fileSignature in kvp.Value) {
                    if (signature.SequenceEqual(fileSignature)) {
                        return kvp.Key;
                    }
                }
            }

            // If no matching signature found, return empty string
            return string.Empty;
        }
    }
}
