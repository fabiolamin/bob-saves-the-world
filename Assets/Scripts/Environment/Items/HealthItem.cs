using BSTW.Utils;

namespace BSTW.Environment.Items
{
    public class HealthItem : Item<Health>
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

