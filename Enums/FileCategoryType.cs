using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoVu.Playlist.Domain.Shared.Enums
{
    public class FileCategoryType : SmartEnum<FileCategoryType>
    {
        public static readonly FileCategoryType Video_360 = new FileCategoryType(nameof(Video_360), 1);
        public static readonly FileCategoryType Video_2D = new FileCategoryType(nameof(Video_2D), 2);
        public static readonly FileCategoryType Image_360 = new FileCategoryType(nameof(Image_360), 3);
        public static readonly FileCategoryType Image_2D = new FileCategoryType(nameof(Image_2D), 4);
        public static readonly FileCategoryType Thumbnail = new FileCategoryType(nameof(Thumbnail), 5);

        public FileCategoryType(string name, int value) : base(name, value)
        {
        }
    }
}
