namespace X_Wing_Visual_Builder.Model
{
    public class Expansion
    {
        public string name;
        public ExpansionType expansionType;

        public Expansion(ExpansionType expansionType, string name)
        {
            this.expansionType = expansionType;
            this.name = name;
        }
    }
}
