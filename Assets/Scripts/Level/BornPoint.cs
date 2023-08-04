using System;
using UnityEngine;

namespace Level
{
    public class BornPoint : MonoBehaviour, IBornPoint
    {
        public bool NoTankInPoint() => _curStayObject == 0;

        private int _curStayObject;

        public Vector3 PointPosition()
        {
            return transform.position;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Enemy") || col.CompareTag("Player"))
            {
                _curStayObject++;
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.CompareTag("Enemy") || col.CompareTag("Player"))
            {
                _curStayObject--;
            }
        }
    }
}