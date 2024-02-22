using NeoVu.Playlist.Application.Contracts.PlaylistContent.AppService;
using NeoVu.Playlist.Application.Contracts.PlaylistContent.DTO;
using NeoVu.Playlist.Application.MappingDTOs;
using NeoVu.Playlist.Domain;
using NeoVu.Playlist.Domain.PlaylistContent.AggregateRoot;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using NeoVu.Playlist.Domain.Shared.Enums;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Azure.Storage;
namespace NeoVu.Playlist.Application.BlobServices
{
    public class BlobService : ApplicationService, IBlobService
    {
        private readonly IBlobContainer<NeoVuBlobContainer> _blobContainer;
        private readonly IRepository<BlobContent, Guid> _repository;
        private readonly IConfiguration _configuration;
        private readonly string _storageConnectionString;
        private readonly string _storageContainerName;
        private const long MAXFILESIZE = 10737418240;

        public BlobService(IBlobContainer<NeoVuBlobContainer> blobContainer, IRepository<BlobContent, Guid> repository,
                           IConfiguration configuration)
        {
            _configuration = configuration;
            _blobContainer = blobContainer;
            _repository = repository;
            _storageConnectionString = _configuration.GetValue<string>("AzureConnectionString:ConnectionString");
            _storageContainerName = _configuration.GetValue<string>("BlobContainerName:StorageContainer");
        }

        public async Task<Guid> Upload(BlobContentDto blob)
        {
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            string[] validFileTypes = { "mp4", "jpg", "jpeg", "png" };
            bool isValidType = validFileTypes.Any(t => Path.GetExtension(blob.FileName) == "." + t);

            if (isValidType)
            {
                //  Get the currently used user space size 
                var userUsedSize = await _repository.SumAsync(t => t.FileSize);
                var userTotalSize = blob.FileSize + userUsedSize;
                if (userTotalSize > MAXFILESIZE)
                {
                    throw new UserFriendlyException(" Insufficient space left");
                }

                int accessLevel = 0;
                if (blob.FileAccessLevel == null || blob.FileAccessLevel == 0)
                {
                    accessLevel = 1;
                }
                else accessLevel = blob.FileAccessLevel.Value;
                FileAccessLevelType access = FileAccessLevelType.FromValue(accessLevel);

                int location = 0;
                if (blob.FileLocation == null || blob.FileLocation == 0)
                {
                    location = 1;
                }
                else location = blob.FileLocation.Value;
                FileLocationType loc = FileLocationType.FromValue(location);

                FileCategoryType cat = FileCategoryType.FromName(blob.FileCategory);

                if (loc.Name == "Local")
                {
                    string uri = container.Uri.ToString();
                    blob.FileUrl = $"{uri}/{blob.FileName}";
                    //Uri blobUri = new Uri(blob.FileUrl); 
                    BlobContent newFile = new BlobContent(GuidGenerator.Create(), blob.FileName,
                    blob.FileUrl, cat, blob.FileSize, CurrentTenant.Id, loc, access, blob.UserName, blob.Password);

             //       await _blobContainer.SaveAsync(newFile.Id.ToString(), file).ConfigureAwait(false);

                    await _repository.InsertAsync(newFile);
                    //     blobDto = newFile.GetBlobByIdMapping();

                    return newFile.Id;
                }
                else
                {
                    BlobContent remoteFile = await _repository.FirstOrDefaultAsync(b => b.FileUrl == blob.FileUrl);
                    BlobContent newFile = new BlobContent(GuidGenerator.Create(), blob.FileName, blob.FileUrl, cat,
                        blob.FileSize, CurrentTenant.Id, loc, access, blob.UserName, blob.Password);

                    await _repository.InsertAsync(newFile);
                    //     blobDto = newFile.GetBlobByIdMapping();
                    return newFile.Id;
                }
            }
            else
                throw new UserFriendlyException("Invalid file type");
        }

        public async Task<string> GetSasTokenForBlob(string blobName)
        {
            BlobClient blobClient = new BlobClient(_storageConnectionString, _storageContainerName, blobName);
            //BlobServiceClient blobServiceClient =
            //        blobClient.GetParentBlobContainerClient().GetParentBlobServiceClient();

            //UserDelegationKey userDelegationKey = await blobServiceClient.GetUserDelegationKeyAsync(DateTimeOffset.UtcNow,
                                                                  //DateTimeOffset.UtcNow.AddDays(2));
            BlobSasBuilder blobSasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = blobClient.BlobContainerName,
                BlobName = blobClient.Name,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(2)//Let SAS token expire after 5 minutes.
            };
            blobSasBuilder.SetPermissions(BlobSasPermissions.Read |
                              BlobSasPermissions.Write);
            //blobSasBuilder.SetPermissions(
            //                                BlobContainerSasPermissions.Read |
            //                                BlobContainerSasPermissions.Add |
            //                                BlobContainerSasPermissions.Create |
            //                                BlobContainerSasPermissions.Write |
            //                                BlobContainerSasPermissions.List
            //                             );
            var sasToken = blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential("neovustorage", "I2Dzjr8WS9YesscQDLT69qYy+RON44+wynLvV1r/XC/GrgOY5OSRvq+bKMAWvffkCzoXpj+7hAaB+AStgEVhgg=="));
            //BlobUriBuilder blobUriBuilder = new BlobUriBuilder(blobClient.Uri)
            //{
            //    // Specify the user delegation key.
            //    Sas = blobSasBuilder.ToSasQueryParameters(userDelegationKey,
            //                                 blobServiceClient.AccountName)
            //};
            string sasUrl = blobClient.Uri.AbsoluteUri + "?" + sasToken;
            return sasToken.ToString();
        }

        public async Task<bool> DeleteBlobContent(Guid id)
        {
            BlobContent file = await _repository.FindAsync(id);

            if (file != null)
            {
                await _repository.DeleteAsync(id);
                await _blobContainer.DeleteAsync(id.ToString());
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> GetBlob(Guid id)
        {
            BlobContent currentFile = await _repository.FindAsync(id);
            if (currentFile != null)
            {
                byte[] myfile = await _blobContainer.GetAllBytesOrNullAsync(id.ToString());
                return currentFile.FileUrl;
            }

            throw new UserFriendlyException(" File does not exist !");
        }

        public async Task<BlobContentDto> GetBlobInfo(Guid id)
        {
            BlobContent currentFile = await _repository.FindAsync(x => x.Id == id);
            if (currentFile != null)
            {
                BlobContent blobInfo = await _repository.GetAsync(id);
                BlobContentDto dto = blobInfo.GetBlobByIdMapping();
                return dto;
            }
            else
                throw new UserFriendlyException("File info does not exist");
        }

        public async Task<PagedResultDto<BlobContentDto>> GetBlobsWithPagination(PagedResultRequestDto input)
        {
            IQueryable<BlobContent> queryable = await _repository.GetQueryableAsync();
            queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount);
            List<BlobContent> result = await AsyncExecuter.ToListAsync(queryable);
            //            int count = await AsyncExecuter.CountAsync<BlobContent>(queryable);
            int count = await _repository.CountAsync();

            List<BlobContentDto> blobDtos = new List<BlobContentDto>();
            foreach(var blob in result)
            {
                BlobContentDto dto = blob.GetBlobByIdMapping();
                blobDtos.Add(dto);
            }
            return new PagedResultDto<BlobContentDto>(count, blobDtos);
        }
    
        public async Task<long> GetUsedSpace()
        {
            var usedSpace = await _repository.SumAsync(t => t.FileSize);
            return (long)usedSpace;
        }

    }
}
