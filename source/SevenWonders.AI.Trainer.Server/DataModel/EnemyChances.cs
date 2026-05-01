namespace SevenWonders.AI.Model.DataModel
{
    public class EnemyChances
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public List<EnemyChance> Chances { get; set; }

        public EnemyChances()
        {
            Chances = new List<EnemyChance>();
        }
    }
}
