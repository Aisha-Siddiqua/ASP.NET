using Microsoft.EntityFrameworkCore;
using NeoVu.Playlist.Domain.PlaylistContent.AggregateRoot;
using NeoVu.Playlist.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NeoVu.Playlist.EntityFrameworkCore.EFCore
{
    public static class NeoVuPlaylistDbContextModelCreatingExtensions
    {

        public static void ConfigureNeoVuBlobContentPlaylist(this ModelBuilder builder)
        {
            builder.Entity<BlobContent>(b =>
            {
                b.ToTable("NeoVuBlobContent");
                b.Property(p => p.FileCategory).HasConversion(p => p.Value, p => FileCategoryType.FromValue(p));
                b.Property(p => p.FileAccessLevel).HasConversion(p => p.Value, p => FileAccessLevelType.FromValue(p));
                b.Property(p => p.FileLocation).HasConversion(p => p.Value, p => FileLocationType.FromValue(p));
                b.ConfigureByConvention();
            });
        }
    }
}
