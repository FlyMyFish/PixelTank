using System;

namespace Basic
{
    /// <summary>
    /// 可消耗品
    /// </summary>
    public interface IConsumable
    {
        void Full(IConsumable type);
        event Action<IConsumable> OnEmpty;
    }
}