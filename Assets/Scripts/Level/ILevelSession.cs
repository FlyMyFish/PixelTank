using System;
using UnityEngine;

namespace Level
{
    public interface ILevelSession
    {
        void OnEnemyDestroy(GameObject o);
        
        public event Action OnSessionStart;
        public event Action OnSessionEnd;
        int LeftEnemyCount();
        int CurEnemyCount();

        int MaxEnemyCount();
        int KilledEnemyCount();

        MonoBehaviour GetContext();

        Vector3 PlayerBornPoint();

        void DestroySession();
    }
}