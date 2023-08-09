using System;
using Level;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Controller
{
    public class GameSessionController
    {
        private static GameSessionController _instance;
        private GameObject _curSessionPrefab;
        private GameObject _curSessionObj;
        private ILevelSession _curSession;
        private GameObject Player;
        public event Action<bool, int> OnSessionComplete;

        public static GameSessionController GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GameSessionController();
            }

            return _instance;
        }

        private GameSessionController()
        {
            Player = Resources.Load<GameObject>("Prefabs/Tank");
            _curSessionPrefab = Resources.Load<GameObject>("Sessions/Level1Session1");
            _curSession = _curSessionPrefab.gameObject.GetComponentInChildren<ILevelSession>();
        }

        public void Init()
        {
            Player.tag = "Player";
            Object.Instantiate(Player, new Vector3(0, 1, 0), Quaternion.identity);
            Player.transform.position = _curSession.PlayerBornPoint();
            _curSession.OnSessionEnd += () => { _curSession = null; };
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
            if (Camera.main != null) Camera.main.gameObject.transform.position = new Vector3(_curSession.PlayerBornPoint().x,_curSession.PlayerBornPoint().x,-10);
        }
    }
}