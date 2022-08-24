using BSTW.Player;

namespace BSTW.Environment.Items
{
    public class HealthItem : Item
    {
        private PlayerHealth _playerHealth;

        private void Start()
        {
            _playerHealth = collector.GetComponent<PlayerHealth>();
        }

        protected override bool CanBeCollected()
        {
            return !_playerHealth.IsHealthFull;
        }

        protected override void OnCollected()
        {
            _playerHealth.AddHealth(bonusAmount);
        }
    }
}

