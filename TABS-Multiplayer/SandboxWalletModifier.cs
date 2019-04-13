using System;
using TABS_Multiplayer;

#pragma warning disable CS0626
namespace Landfall.TABS.Budget.Wallets
{
    class patch_SandboxWallet : SandboxWallet // Patch the wallet handler
    {
        private bool infinite = false;

        public extern bool orig_CanAfford(int value);
        public override bool CanAfford(int value)
        {
            this.UpdateBudget();
            return infinite || value <= base.Budget;
        }

        public extern bool orig_ReturnAmount(int value);
        public override void ReturnAmount(int value)
        {
            this.UpdateBudget();
            if (infinite)
                base.Budget -= value;
            else
                base.Budget += value;
        }

        public extern bool orig_SpendAmount(int value);
        public override void SpendAmount(int value)
        {
            this.UpdateBudget();
            if (infinite)
                base.Budget += value;
            else
                base.Budget -= value;
        }

        protected extern bool orig_OnBudgetSet();
        protected override void OnBudgetSet()
        {
            this.UpdateBudget();
        }

        private void UpdateBudget()
        {
            infinite = base.Budget == -1 || (SocketConnection.maxBudget <= 0 && SocketConnection.getTcpClient().Connected);
            // Make it infinite if it's connected and 0
        }
    }
}
