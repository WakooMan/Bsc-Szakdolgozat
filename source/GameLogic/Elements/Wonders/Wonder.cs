using GameLogic.Elements.Effects;
using GameLogic.Elements.Goods;
using GameLogic.Events;
using GameLogic.Handlers;

namespace GameLogic.Elements.Wonders
{
    public class Wonder : IBuildable
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

        public void OnBuilt(Player player, IEventManager eventManager)
        {
            Effects.ForEach(effect => effect.Apply(player, eventManager));
        }
    }
}
