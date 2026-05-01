using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Disciplines;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Elements.Goods.Products;
using SevenWonders.Game.Logic.Elements.Goods.Resources;

namespace SevenWonders.AI.Model.Services.Encoders
{
    public class EasyPlayerEncoder : IEasyPlayerEncoder
    {
        public void EncodePlayer(List<float> vector, PlayerProperties playerProperties)
        {
            vector.Add(playerProperties.Owner.Money / 100f);
            vector.Add(playerProperties.VictoryPoints / 100f);
            vector.Add(playerProperties.Strength / 100f);
            vector.Add(playerProperties.Owner.Wonders.Count(w => w.HasBeenBuilt) / 5f);

            Type[] goodTypes = [typeof(Clay), typeof(Stone), typeof(Wood), typeof(Glass), typeof(Papirus)];
            foreach (var goodType in goodTypes)
            {
                vector.Add(playerProperties.Goods.TryGetValue(goodType, out var good) ? good.Amount / 10f : 0f);
            }

            Type[] disciplineTypes = [typeof(Building), typeof(Geography), typeof(Healing), typeof(Mechanics), typeof(Physics), typeof(Trading), typeof(Writing)];
            foreach (var disciplineType in disciplineTypes)
            {
                vector.Add(playerProperties.Disciplines.TryGetValue(disciplineType, out var count) ? count / 10f : 0f);
            }

            Type[] cardTypes = [typeof(BrownCard), typeof(BlueCard), typeof(GrayCard), typeof(GreenCard), typeof(PurpleCard), typeof(RedCard), typeof(YellowCard)];
            foreach (var cardType in cardTypes)
            {
                vector.Add(playerProperties.Owner.Cards.Count(c => c.GetType() == cardType) / 10f);
            }
        }
    }
}
