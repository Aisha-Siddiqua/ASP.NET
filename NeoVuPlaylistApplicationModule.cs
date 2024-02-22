using NeoVu.Playlist.Application.Contracts;
using NeoVu.Playlist.Domain;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace NeoVu.Playlist.Application
{
    [DependsOn(
        typeof(AbpDddApplicationModule),
        typeof(NeoVuPlaylistApplicationContractsModule),
        typeof(NeoVuPlaylistDomainModule)
        )]
    public class NeoVuPlaylistApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
        }

    }
}