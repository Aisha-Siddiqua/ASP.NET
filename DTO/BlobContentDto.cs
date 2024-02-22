using NeoVu.Playlist.Domain.Shared;
using NeoVu.Playlist.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace NeoVu.Playlist.Application.Contracts.PlaylistContent.DTO
{
    public class BlobContentDto
    {
        public Guid? Id { get; set; }
        [StringLength(StringLengthValue.length)]
        public string FileName { get; set; }
        public string FileCategory { get; set; }
        public int? FileLocation { get; set; }
        public int? FileAccessLevel { get; set; }
        [StringLength(StringLengthValue.length)]
        public string FileUrl { get; set; }
        [StringLength(StringLengthValue.length)]
        public string? UserName { get; set; }
        [StringLength(StringLengthValue.length)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public long? FileSize { get; set; }
        public DateTime? UploadDate { get; set; }
    }
}
