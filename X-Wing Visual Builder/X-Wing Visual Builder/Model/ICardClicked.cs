namespace X_Wing_Visual_Builder.Model
{
    public interface ICardClicked
    {
        void UpgradeClicked(int upgradeId);
        void PilotClicked(int pilotId);
    }
}
