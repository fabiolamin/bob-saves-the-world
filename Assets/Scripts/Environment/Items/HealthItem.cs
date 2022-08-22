using BSTW.Player;

namespace BSTW.Environment.Items
{
    public class HealthItem : Item<PlayerHealth>
    {
        protected override bool CanBeCollected()
        {
            return !collectorPerk.IsHealthFull;
        }

        protected override void OnCollected()
        {
            collectorPerk.AddHealth(bonusAmount);
        }
    }
}

