using BSTW.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace BSTW.Equipments.Weapons
{
    public class BulletTarget : MonoBehaviour
    {
        [SerializeField] private ObjectPooling _hitVFXPooling;
        [SerializeField] private UnityEvent<float> _onHit;

        public void Hit(float damage, Vector3 point)
        {
            _onHit?.Invoke(damage);
            PlayHitVFX(point);
        }

        private void PlayHitVFX(Vector3 point)
        {
            var hitGO = _hitVFXPooling.GetObject();
            hitGO.transform.position = point;

            var hitVFX = hitGO.GetComponent<ParticleSystem>();
            hitVFX.Play();

            StartCoroutine(DisableHitVFX(hitVFX));
        }

        private IEnumerator DisableHitVFX(ParticleSystem vfx)
        {
            yield return new WaitUntil(() => vfx.isStopped);

            vfx.gameObject.SetActive(false);
        }
    }
}

