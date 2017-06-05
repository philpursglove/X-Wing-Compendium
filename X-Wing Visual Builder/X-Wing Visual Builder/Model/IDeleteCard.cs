namespace X_Wing_Visual_Builder.Model
{
    public interface IDeleteCard
    {
        void DeletePilotClicked(int uniqueBuildId, int uniquePilotId);
        void DeleteUpgradeClicked(int uniqueBuildId, int uniquePilotId, int upgradeId);
    }
}
