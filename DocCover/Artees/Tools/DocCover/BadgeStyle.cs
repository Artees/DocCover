using System.Collections.Generic;

namespace Artees.Tools.DocCover
{
    public class BadgeStyle : TypesafeEnum
    {
        public static readonly Dictionary<string, BadgeStyle> All =
            new Dictionary<string, BadgeStyle>();

        // ReSharper disable UnusedMember.Global
        public static readonly BadgeStyle Plastic = new BadgeStyle("plastic"),
            Flat = new BadgeStyle("flat"),
            FlatSquare = new BadgeStyle("flat-square"),
            ForTheBadge = new BadgeStyle("for-the-badge"),
            Popout = new BadgeStyle("popout"),
            PopoutSquare = new BadgeStyle("popout-square"),
            Social = new BadgeStyle("social");
        // ReSharper restore UnusedMember.Global

        private BadgeStyle(string name) : base(name)
        {
            All[name] = this;
        }
    }
}