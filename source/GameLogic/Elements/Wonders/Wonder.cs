using GameLogic.Elements.Effects;
using GameLogic.Elements.Goods;
using GameLogic.Handlers;
using System.Xml.Serialization;

namespace GameLogic.Elements.Wonders
{
    public class Wonder : IBuildable
    {
        public string Name { get; set; }
        public List<Good> GoodCost { get; set; }
        public List<Effect> Effects { get; set; }
        public bool HasBeenBuilt { get; set; }
        [XmlIgnore]
        public string BuildingType => GetType().Name;

        [XmlIgnore]
        public int MoneyCost => 0;

        public Wonder()
        {
            Effects = new List<Effect>();
            GoodCost = new List<Good>();
            HasBeenBuilt = false;
        }

        private Wonder(Wonder wonder)
        {
            Name = wonder.Name;
            GoodCost = wonder.GoodCost.Select(good => good.Clone()).ToList();
            Effects = wonder.Effects.Select(effect => effect.Clone()).ToList();
            HasBeenBuilt = wonder.HasBeenBuilt;
        }

        public Wonder Clone()
        {
            return new Wonder(this);
        }

        public void OnBuilt(IGameContext gameContext)
        {
            Effects.ForEach(effect => effect.Apply(gameContext));
        }
    }
}
