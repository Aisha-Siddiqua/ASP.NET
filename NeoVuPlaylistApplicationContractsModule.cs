using NeoVu.Playlist.Domain.Shared;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace NeoVu.Playlist.Application.Contracts
{
    [DependsOn(
        typeof(AbpDddApplicationContractsModule),
        typeof(NeoVuPlaylistDomainSharedModule)
        )]
    public class NeoVuPlaylistApplicationContractsModule : AbpModule
    {

    }
}