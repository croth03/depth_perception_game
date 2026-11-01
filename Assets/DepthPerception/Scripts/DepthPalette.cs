using UnityEngine;

namespace DepthPerception
{
    public static class DepthPalette
    {
        public static readonly Color Ink = Hex("#070913");
        public static readonly Color Night = Hex("#1A2440");
        public static readonly Color Stone = Hex("#46516B");
        public static readonly Color Mist = Hex("#7A8DA8");
        public static readonly Color Signal = Hex("#82E9EE");
        public static readonly Color Drift = Hex("#AD8CFF");
        public static readonly Color Doubt = Hex("#E790BD");
        public static readonly Color PlayerPrimary = Hex("#D8F4F2");

        public static Color WithAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        private static Color Hex(string value)
        {
            ColorUtility.TryParseHtmlString(value, out Color color);
            return color;
        }
    }
}
