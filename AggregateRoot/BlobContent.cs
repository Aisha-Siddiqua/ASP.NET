using NeoVu.Playlist.Domain.Shared;
using NeoVu.Playlist.Domain.Shared.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NeoVu.Playlist.Domain.PlaylistContent.AggregateRoot
{
    public class BlobContent : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        [Required]
        [StringLength(StringLengthValue.length)]
        public string FileName { get; private set; }
        [Required]
        public FileCategoryType FileCategory { get; private set; }
        [Required]
        public FileLocationType? FileLocation { get; private set; } 
        public FileAccessLevelType? FileAccessLevel { get; private set; }
        [StringLength(StringLengthValue.length)]
        public string FileUrl { get; private set; }
        [StringLength(StringLengthValue.length)]
        public string? UserName { get; private set; }

        [StringLength(StringLengthValue.length)]
        [DataType(DataType.Password)]
        public string? Password { get; private set; }    
        public long? FileSize { get; private set; }

        public Guid? TenantId { get; private set; }
        protected BlobContent() { }

        public BlobContent(Guid id, string name, string url, FileCategoryType category, long? fileSize = null,
                           Guid? tenant = null, FileLocationType? location = null, FileAccessLevelType? accessType = null,
                           string? username=null, string? password=null)
        {
            Id = id;
            FileName = name;
            FileCategory = category;
            FileLocation = location;
            TenantId = tenant;
            FileSize = fileSize;
            FileUrl = url;

            if (location == FileLocationType.Remote && (username != null && password != null))
            {
                SetRemoteContent(username, password, accessType);
            }
        }

        public void SetRemoteContent(string username, string password, FileAccessLevelType? accessType = null)
        {
            UserName = username;
            Password = password;
            FileAccessLevel = accessType;
        }
    }
}
