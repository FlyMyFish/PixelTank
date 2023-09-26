using System;
using System.Linq;
using UnityEngine;

namespace Controller
{
    public class ForeBox : MonoBehaviour
    {
        public event Action<bool> OnTriggerChanged;
        private readonly string[] _usableTags = {"Player", "Enemy", "SceneWall", "GameBuild"};

        private void OnTriggerEnter2D(Collider2D col)
        {
            var colTag = col.tag;
            if (_usableTags.Contains(colTag))
            {
                OnTriggerChanged?.Invoke(true);
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            var colTag = col.tag;
            if (_usableTags.Contains(colTag))
            {
                OnTriggerChanged?.Invoke(false);
            }
        }
    }
}