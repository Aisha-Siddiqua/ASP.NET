using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoVu.Playlist.Domain.Shared.Enums
{
    public class FileLocationType : SmartEnum<FileLocationType>
    {
        public static readonly FileLocationType Remote = new FileLocationType(nameof(Remote), 1);
        public static readonly FileLocationType Local = new FileLocationType(nameof(Local), 2);
        public FileLocationType(string name, int value) : base(name, value)
        {
        }

    }
}
