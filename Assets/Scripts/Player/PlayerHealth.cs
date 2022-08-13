using BSTW.Utils;
using UnityEngine;

namespace BSTW.Player
{
    public class PlayerHealth : Health
    {
        [SerializeField] private PlayerCameraShake _playerCameraShake;

        protected override void CheckHit(Hit hit)
        {
            if (hit.HitType != HitType.Explosion)
            {
                _playerCameraShake.StartShakeCamera(
                hit.Damage / _playerCameraShake.MaxIntensity,
                (_playerCameraShake.MaxTimer * hit.Damage) / MaxHealth);
            }

            OnDamageStarted?.Invoke();

            if (HasBeenKnockDown(hit))
                OnKnockdownStarted?.Invoke();
            else
                OnHitStarted?.Invoke();

            GotHit = true;
        }

        private bool HasBeenKnockDown(Hit hit)
        {
            return hit.Damage >= (MaxHealth * KnockDownPercentage) && PlayerFoot.IsOnTheGround;
        }

        protected override bool CanGotHit(Hit hit)
        {
            return IsAlive || !GotHit || CanUpdateHealth || hit.Damage != 0f;
        }
    }
}

