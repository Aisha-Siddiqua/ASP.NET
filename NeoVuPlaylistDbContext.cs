using Microsoft.EntityFrameworkCore;
using NeoVu.Playlist.Domain.PlaylistContent.AggregateRoot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NeoVu.Playlist.EntityFrameworkCore.EFCore
{
    [ConnectionStringName("Default")]
    public class NeoVuPlaylistDbContext : AbpDbContext<NeoVuPlaylistDbContext>, INeoVuPlaylistDbContext
    {
        public NeoVuPlaylistDbContext(DbContextOptions<NeoVuPlaylistDbContext> options) : base(options)
        {
        }
        public DbSet<BlobContent> blobDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ConfigureNeoVuBlobContentPlaylist();
        }
    }
}
