using UnityEngine;

namespace Controller
{
    public class EngineController : MonoBehaviour
    {
        public GameObject Left;
        public GameObject Right;

        private Animator _leftAnim, _rightAnim;

        private void Awake()
        {
            _leftAnim = Left.GetComponentInChildren<Animator>();
            _rightAnim = Right.GetComponentInChildren<Animator>();
        }

        /*public void PlayMoveSound()
        {
            _audioSource.PlayOneShot(moveSound);
        }*/

        public void SetTrigger(string trigger)
        {
            Debug.Log($"SetTrigger: {trigger}");
            _leftAnim.SetTrigger(trigger);
            _rightAnim.SetTrigger(trigger);
        }

    }
}
