
using UnityEngine;

namespace ManaSword.Utility
{
    public static class GridSnapping
    {
        public const float PixelPerUnit = 32;
        public static readonly float GridSize = 1 / PixelPerUnit;

        public static float RoundToNearestGrid(float pos)
        {
            float xDiff = pos % GridSize;
            bool isPositive = pos > 0 ? true : false;
            pos -= xDiff;
            if (Mathf.Abs(xDiff) > GridSize / 2)
            {
                if (isPositive)
                {
                    pos += GridSize;
                }
                else
                {
                    pos -= GridSize;
                }
            }
            return pos;
        }

        public static Vector3 RoundToNearestGrid(Vector3 pos)
        {
            return new Vector3
                (
                    RoundToNearestGrid(pos.x),
                    RoundToNearestGrid(pos.y),
                    RoundToNearestGrid(pos.z)
                );
        }

        public static bool IsSnappedValue(Vector3 pos)
        {
            return pos.x % GridSize == 0 && pos.y % GridSize == 0 && pos.y % GridSize == 0;
        }
    }
}