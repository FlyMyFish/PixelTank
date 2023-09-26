using System;
using Level;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Controller
{
    public class GameSessionController
    {
        private static GameSessionController _instance;
        private readonly GameObject _curSessionPrefab;
        private GameObject _curSessionObj;
        private ILevelSession _curSession;
        private readonly GameObject _player;
        public event Action<bool, int> OnSessionComplete;

        public static GameSessionController GetInstance()
        {
            return _instance ??= new GameSessionController();
        }

        private GameSessionController()
        {
            _player = Resources.Load<GameObject>("Prefabs/PlayerTank");
            _curSessionPrefab = Resources.Load<GameObject>("Sessions/Level1Session1");
            _curSession = _curSessionPrefab.gameObject.GetComponentInChildren<ILevelSession>();
            _curSession.Reset();
        }

        public void Init()
        {
            Object.Instantiate(_player, new Vector3(0, 1, 0), Quaternion.identity);
            _player.transform.position = _curSession.PlayerBornPoint();
            _curSession.OnSessionEnd += () => { _curSession = null; };
            _curSession.Reset();
            InitLevel();
        }

        private void InitLevel()
        {
            _curSessionObj = Object.Instantiate(_curSessionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }

        public void OnEnemyDestroy(GameObject o)
        {
            _curSession?.OnEnemyDestroy(o);
        }

        public int GetMaxEnemyCount()
        {
            return _curSession?.MaxEnemyCount() ?? 0;
        }

        public int GetKilledEnemyCount()
        {
            return _curSession?.KilledEnemyCount() ?? 0;
        }

        public void GameOver()
        {
            OnSessionComplete?.Invoke(false, 0);
        }

        public void ReStar()
        {
            _curSessionObj.SetActive(false);
            Object.Destroy(_curSessionObj);
            Init();
            if (Camera.main != null)
                Camera.main.gameObject.transform.position = new Vector3(_curSession.PlayerBornPoint().x,
                    _curSession.PlayerBornPoint().x, -10);
        }
    }
}