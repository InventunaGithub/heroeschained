using UnityEngine;
using System;

namespace DuloGames.UI
{
    [Serializable]
    public enum UIItemQuality : int
    {
        Common = 0,
        Uncommon = 1,
        Rare = 2,
        Unique = 3,
    }
    
    public class UIItemQualityColor
    {
        public const string Common = "ffffffff";
        public const string Uncommon = "1eff00ff";
        public const string Rare = "0070ffff";
        public const string Unique = "a335eeff";

        public static string GetHexColor(UIItemQuality quality)
        {
            switch (quality)
            {
                case UIItemQuality.Common: return Common;
                case UIItemQuality.Uncommon: return Uncommon;
                case UIItemQuality.Rare: return Rare;
                case UIItemQuality.Unique: return Unique;
            }

            return Common;
        }

        public static Color GetColor(UIItemQuality quality)
        {
            return CommonColorBuffer.StringToColor(GetHexColor(quality));
        }
    }
}
