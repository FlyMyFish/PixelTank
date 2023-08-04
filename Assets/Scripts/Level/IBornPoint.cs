using UnityEngine;

namespace Level
{
    public interface IBornPoint
    {
        bool NoTankInPoint();
        Vector3 PointPosition();
    }
}