using Level;
using UnityEngine;

namespace Controller
{
    public class GameSessionController
    {
        private static GameSessionController _instance;
        private ILevelSession _curSession;
        private GameObject Player;

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
        }

        public void Init()
        {
            Player.tag = "Player";
            Object.Instantiate(Player, new Vector3(0, 1, 0), Quaternion.identity);
        }

        public void Register(ILevelSession session)
        {
            _curSession = session;
            Player.transform.position = session.PlayerBornPoint();

            session.OnSessionEnd += () => { _curSession = null; };
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
            
        }
    }
}