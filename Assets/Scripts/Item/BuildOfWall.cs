using UnityEngine;

namespace Item
{
    public class BuildOfWall : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Bullet"))
            {
                GameObject o;
                (o = gameObject).SetActive(false);
                Destroy(o);
            }
        }
    }
}