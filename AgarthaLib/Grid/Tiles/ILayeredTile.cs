using System;

namespace AgarthaLib.Grid.Tiles
{
    public interface ILayeredTile<T> where T : Enum
    {
        public T GetLayer();
    }
}
