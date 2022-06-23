using BSTW.Player;

namespace BSTW.Health
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
    }
}

