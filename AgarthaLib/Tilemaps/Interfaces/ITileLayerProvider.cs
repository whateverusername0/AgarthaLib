using System;

namespace AgarthaLib.Tilemaps.Interfaces
{
    public interface ITileLayerProvider<T> where T : Enum
    {
        public T GetLayer();
    }
}
