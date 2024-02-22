using Microsoft.EntityFrameworkCore;
using NeoVu.Playlist.Domain.PlaylistContent.AggregateRoot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;

namespace NeoVu.Playlist.EntityFrameworkCore.EFCore
{
    public interface INeoVuPlaylistDbContext : IEfCoreDbContext
    {
        DbSet<BlobContent> blobDetails { get; }
    }
}
