using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeoVu.Playlist.Application.Contracts.PlaylistContent.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NeoVu.Playlist.Application.Contracts.PlaylistContent.AppService
{
    public interface IBlobService : IApplicationService
    {
        Task<Guid> Upload(BlobContentDto blob);
        Task<string> GetSasTokenForBlob(string blobName);
        Task<string> GetBlob(Guid id);
        Task<BlobContentDto> GetBlobInfo(Guid id);

        Task<bool> DeleteBlobContent(Guid id);

        Task<PagedResultDto<BlobContentDto>> GetBlobsWithPagination(PagedResultRequestDto dto);

        Task<long> GetUsedSpace();

    }
}
