﻿using Microsoft.Xna.Framework;

namespace KnightsTrial
{
    public static class PointExt
    {
        public static Vector2 ToVector2(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }


    }
}
