using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Controller
{
    public class UIManager : MonoBehaviour
    {
        public Text displayMaxCount;
        public Text displayKilledCount;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            GameSessionController.GetInstance().Init();
        }

        // Update is called once per frame
        void Update()
        {
            displayKilledCount.text = GameSessionController.GetInstance().GetKilledEnemyCount().ToString();
            displayMaxCount.text = GameSessionController.GetInstance().GetMaxEnemyCount().ToString();
        }
    }
}