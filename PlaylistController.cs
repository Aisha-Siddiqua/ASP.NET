using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeoVu.Playlist.Application.Contracts.PlaylistContent.AppService;
using NeoVu.Playlist.Application.Contracts.PlaylistContent.DTO;
using NeoVu.Sessions.Application.Contracts.AppServices;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace NeoVu.Host.Controllers
{
    [Authorize]

    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : AbpController
    {
        private readonly IBlobService _blobService;
        private readonly ISessionAppService _sessionAppService;
        public PlaylistController(IBlobService blobService, ISessionAppService sessionAppService)
        {
            _blobService = blobService;
            _sessionAppService = sessionAppService;
        }

        [HttpPost]
        [Route("UploadFile")]
        public async Task<IActionResult> UploadFile([FromBody] BlobContentDto blobInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Guid dto = await _blobService.Upload(blobInfo);
                    if (dto == Guid.Empty)
                    {
                        return BadRequest("Unable to upload files");
                    }
                    else
                        return Ok(dto);
                }
                else
                    return BadRequest("Invalid input data");
            }
            catch (Exception ex)
            {
                return BadRequest(error: new { exception = "Could not upload the file. Check if all input fields are filled" });
            }
        }

        [HttpGet]
        [Route("UploadFileSasToken")]
        public async Task<IActionResult> UploadFileWithToken(string blobName)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string dto = await _blobService.GetSasTokenForBlob(blobName);
                    if (dto == null)
                    {
                        return BadRequest("Unable to upload files");
                    }
                    else
                        return Ok(dto);
                }
                else
                    return BadRequest("Invalid input data");
            }
            catch (Exception ex)
            {
                return BadRequest(error: new { exception = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetFile")]
        public async Task<IActionResult> GetFileById(Guid id)
        {
            try
            {
                string blobUrl = await _blobService.GetBlob(id);
                if (blobUrl != null)
                {
                    return Ok(blobUrl);
                }
                else
                    return BadRequest("File not found");
            }
            catch (Exception ex)
            {
                return BadRequest(error: new { exception = "Could not get the file with ID " + id });
            }
        }

        [HttpGet]
        [Route("GetFileInfo")]
        public async Task<IActionResult> GetFileInfoById(Guid id)
        {
            try
            {
                BlobContentDto info = await _blobService.GetBlobInfo(id);
                if (info != null)
                {
                    return Ok(info);
                }
                else
                    return BadRequest("File with not found");
            }
            catch (Exception ex)
            {
                return BadRequest(error: new { exception = "Could not get information about file " + id });
            }
        }

        [HttpGet]
        [Route("GetBlobList")]
        public async Task<IActionResult> GetBlobsListWithPaging(int MaxResult, int PageNumber)
        {
            try
            {
                PagedResultRequestDto input = new PagedResultRequestDto();
                input.MaxResultCount = MaxResult;
                input.SkipCount = (PageNumber - 1) * MaxResult;
                PagedResultDto<BlobContentDto> dto = await _blobService.GetBlobsWithPagination(input);
                if (dto != null)
                {
                    return Ok(dto);
                }
                else
                    return BadRequest("No results found");
            }
            catch (Exception ex)
            {
                return BadRequest(error: new { exception = "Unable to get content list" });
            }
        }

        [HttpDelete]
        [Route("DeleteFile")]
        public async Task<IActionResult> DeleteFileById(Guid id)
        {
            try
            {
                bool canDelete = await _sessionAppService.CheckPlaylistInSession(id);
                if (canDelete)
                {
                    bool deleted = await _blobService.DeleteBlobContent(id);
                    if (deleted)
                    {
                        return Ok("File deleted");
                    }
                    else
                        return BadRequest("Unable to delete the file");
                }
                else
                    return BadRequest("Cannot delete content that is part of a session.");
            }
            catch (Exception ex)
            {
                return BadRequest(error: new { exception = "Could not delete the file with ID " + id + "\n" + ex });
            }

        }
    }
}
