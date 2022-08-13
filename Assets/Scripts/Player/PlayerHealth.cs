using BSTW.Utils;

namespace BSTW.Player
{
    public class PlayerHealth : Health
    {
        protected override void CheckHit(Hit hit)
        {
            var hasBeenKnockDown = hit.Damage >= (MaxHealth * KnockDownPercentage);

            OnDamageStarted?.Invoke();

            if (hasBeenKnockDown && PlayerFoot.IsOnTheGround)
                OnKnockdownStarted?.Invoke();
            else
                OnHitStarted?.Invoke();

            GotHit = true;
        }

        protected override bool CanGotHit(Hit hit)
        {
            return IsAlive || !GotHit || CanUpdateHealth || hit.Damage != 0f;
        }
    }
}

