using NeoVu.Playlist.Application.Contracts.PlaylistContent.DTO;
using NeoVu.Playlist.Domain.PlaylistContent.AggregateRoot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoVu.Playlist.Application.MappingDTOs
{
    public static class BlobInfoMappingExtensions
    {
        public static BlobContentDto GetBlobByIdMapping(this BlobContent blob)
        {
            BlobContentDto blobDto = new BlobContentDto
            {
                Id= blob.Id,
                FileName = blob.FileName,
                FileCategory = blob.FileCategory.Name,
                FileLocation = blob.FileLocation,
                FileAccessLevel = blob.FileAccessLevel,
                FileUrl = blob.FileUrl,
                UserName = blob.UserName,
                Password = blob.Password,
                FileSize = blob.FileSize,
                UploadDate = blob.CreationTime,
            };
            return blobDto;
        }
    }
}
