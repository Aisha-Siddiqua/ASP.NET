using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoVu.Playlist.Domain.Shared.Enums
{
    public class FileAccessLevelType : SmartEnum<FileAccessLevelType>
    {
        public static readonly FileAccessLevelType Public = new FileAccessLevelType(nameof(Public), 1);
        public static readonly FileAccessLevelType Private = new FileAccessLevelType(nameof(Private), 2);
        public FileAccessLevelType(string name, int value) : base(name, value)
        {
        }
    }
}
