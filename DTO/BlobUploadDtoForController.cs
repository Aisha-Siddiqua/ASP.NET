using NeoVu.Playlist.Application.Contracts.PlaylistContent.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoVu.Playlist.Application.Contracts.DTO
{
    public class BlobUploadDtoForController
    { 
        [Required]
        public byte[] ContentFile { get; set; }

        [Required]
        public BlobContentDto blobContentDto { get; set; }

    }
}
