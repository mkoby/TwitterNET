using System;

namespace TwitterNET
{
    public interface IExtendedUserProperties
    {
        DateTime CreateAt { get; }
        int FavoritesCount { get; } 
        int UTCOffset { get; }
        string TimeZone { get; }
        string ProfileBackgroundImageURL { get; }
        bool ProfileBackgroundTile { get; }
        long StatusCount { get; }
        bool Notifications { get; }
        bool Following { get; }
        string ProfileBackgroundColor { get; }
        string ProfileTextColor { get; }
        string ProfileLinkColor { get; } 
        string ProfileSidebarFillColor { get; }
        string ProfileSidebarBorderColor { get; }
    }
}