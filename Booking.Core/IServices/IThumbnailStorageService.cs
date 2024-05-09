using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.IServices
{
    public interface IThumbnailStorageService {
        Task<string> UploadThumbnail(byte[] thumbnailBytes);
    }

}
