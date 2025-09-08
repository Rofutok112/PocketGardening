using UnityEngine;

namespace ProtGardening
{
    public enum TileState
    {
        Poor,
        Normal,
        Rich
    }

    public enum ColorType
    {
        Red,
        Blue,
        Yellow,
        Green,
        Purple,
        Orange,
        Black,
        White,
        Ocher
    }

    public enum FlowerState
    {
        Seed,
        Sprout,
        Bloom
    }

    public enum FlowerGrade
    {
        Low,
        Mid,
        High
    }

    public enum ShopGrade
    {
        Low,
        Mid,
        High
    }

    public enum HexDirection
    {
        UpRight,
        MiddleRight,
        DownRight,
        DownLeft,
        MiddleLeft,
        UpLeft
    }

    public static class HexDirectionExtensions
    {
        public static Vector2Int ToVector2Int(this HexDirection direction)
        {
            return direction switch
            {
                HexDirection.UpRight => new Vector2Int(1, 1),
                HexDirection.MiddleRight => new Vector2Int(2, 0),
                HexDirection.DownRight => new Vector2Int(1, -1),
                HexDirection.DownLeft => new Vector2Int(-1, -1),
                HexDirection.MiddleLeft => new Vector2Int(-2, 0),
                HexDirection.UpLeft => new Vector2Int(-1, 1),
                _ => Vector2Int.zero
            };
        }
    }

    public class Constants
    {
        public const float TILE_INTERVAL_X = 0.65f;
        public const float TILE_INTERVAL_Y = 0.91f;
    }
}