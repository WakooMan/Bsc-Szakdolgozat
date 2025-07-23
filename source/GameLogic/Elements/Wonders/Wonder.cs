using GameLogic.Elements.Effects;
using GameLogic.Elements.Goods;

namespace GameLogic.Elements.Wonders
{
    public class Wonder
    {
        public string Name { get; set; }
        public List<Good> GoodCost { get; set; }
        public List<Effect> Effects { get; set; }

        public bool HasBeenBuilt { get; set; }

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
    }
}
