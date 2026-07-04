using System;

namespace AgarthaLib._2D.Tilemaps.Interfaces
{
    public interface ITileLayerProvider<T> where T : Enum
    {
        public T GetLayer();
    }
}
