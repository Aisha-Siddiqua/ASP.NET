using NeoVu.Playlist.Domain.Shared;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NeoVu.Playlist.Domain
{
    [DependsOn(
    typeof(AbpDddDomainModule), typeof(AbpBlobStoringModule), typeof(NeoVuPlaylistDomainSharedModule))]

    public class NeoVuPlaylistDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
        }

    }
}