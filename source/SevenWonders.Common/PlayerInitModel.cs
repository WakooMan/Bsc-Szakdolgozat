namespace SevenWonders.Common
{
    public class PlayerInitModel
    {
        public string Name { get; set; }
        public PlayerType PlayerType { get; set; }
        
        public PlayerInitModel(string name, PlayerType playerType)
        {
            Name = name;
            PlayerType = playerType;
        }

        public PlayerInitModel() { Name = string.Empty; PlayerType = PlayerType.LocalPlayer; }
    }
}
