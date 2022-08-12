using BSTW.Utils;

namespace BSTW.Player
{
    public class PlayerHealth : Health
    {
        protected override void CheckHit(float damage)
        {
            var hasBeenKnockDown = damage >= (MaxHealth * KnockDownPercentage);

            OnDamageStarted?.Invoke();

            if (hasBeenKnockDown && PlayerFoot.IsOnTheGround)
                OnKnockdownStarted?.Invoke();
            else
                OnHitStarted?.Invoke();

            GotHit = true;
        }

        protected override bool CanGotHit(float damage)
        {
            return IsAlive || !GotHit || CanUpdateHealth || damage != 0f;
        }
    }
}

