using Microsoft.Extensions.DependencyInjection;
using NeoVu.Playlist.Domain;
using NeoVu.Playlist.EntityFrameworkCore.EFCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NeoVu.Playlist.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule), typeof(NeoVuPlaylistDomainModule))]
    public class NeoVuPlaylistEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);

            context.Services.AddAbpDbContext<NeoVuPlaylistDbContext>(options =>
            {
                options.AddDefaultRepositories();
                
            });
        }

    }
}