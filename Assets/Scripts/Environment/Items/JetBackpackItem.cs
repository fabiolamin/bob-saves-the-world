using BSTW.Equipments;

namespace BSTW.Environment.Items
{
    public class JetBackpackItem : Item
    {
        private JetBackpack _playerJetBackpack;

        private void Start()
        {
            _playerJetBackpack = collector.GetComponentInChildren<JetBackpack>();
        }

        protected override bool CanBeCollected()
        {
            return !_playerJetBackpack.JetBackpackData.FullFuel;
        }

        protected override void OnCollected()
        {
            _playerJetBackpack.GetFuel(bonusAmount);
        }
    }
}

