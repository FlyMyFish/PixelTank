using System;
using System.Linq;
using UnityEngine;

namespace Item
{
    public class BulletItem : MonoBehaviour
    {
        private readonly string[] _tags = new string[] {"Enemy", "Player", "SceneWall", "GameBuild"};
        private static readonly int Hit = Animator.StringToHash("Hit");
        public float BulletDamage { set; get; }
        public AudioClip hitSound;
        public string From;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (_tags.Contains(col.tag))
            {
                if (From=="Player")
                {
                    //只有玩家的子弹可以有声音
                    var hitSoundAudioSource = GetComponent<AudioSource>();
                    var mainCamera = Camera.main;
                    if (mainCamera != null)
                    {
                        var dist = Vector3.Distance(transform.position, mainCamera.transform.position);
                        hitSoundAudioSource.volume = Math.Min(1 - dist / 35, 1);
                    }

                    hitSoundAudioSource.PlayOneShot(hitSound);
                }
                var mRigidbody2D = GetComponent<Rigidbody2D>();
                mRigidbody2D.velocity = Vector2.zero;
                mRigidbody2D.isKinematic = true;
                GetComponent<Animator>().SetTrigger(Hit);
            }
        }

        public void BulletHitStart()
        {
            Destroy(GetComponent<BoxCollider2D>());
        }

        public void BulletHitEnd()
        {
            GameObject o;
            (o = gameObject).SetActive(false);
            Destroy(o);
        }
    }
}