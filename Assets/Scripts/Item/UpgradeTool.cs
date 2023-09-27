using Controller;
using Factory;
using UnityEngine;

namespace Item
{
    public class UpgradeTool : MonoBehaviour
    {
        //armor 3种 engine 2种 weapon 2种
        private (int typeIndex,int weightIndex) _upgradeType;

        private void Awake()
        {
            _upgradeType = BoxFactory.CreateRandomBox();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                var playerTankController = col.gameObject.GetComponent<PlayerTankController>();
                playerTankController.OnTouchBox(_upgradeType);
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}