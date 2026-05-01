using SevenWonders.Game.Logic.Elements.Disciplines;
using SevenWonders.Game.Logic.Elements.Effects;
using SevenWonders.Game.Logic.Elements.Goods;

namespace SevenWonders.Game.Logic.Elements
{
    public class PlayerProperties
    {
        public Player Owner { get; }
        public Player Opponent { get; }
        public IReadOnlyDictionary<Type, Good> Goods
        {
            get
            {
                lock (m_goods)
                {
                    return m_goods.AsReadOnly();
                }
            }
        }

        public IReadOnlyList<Effect> Effects
        {
            get
            {
                lock (m_effects)
                {
                    return m_effects.AsReadOnly();
                }
            }
        }

        public IReadOnlyList<TEffect> GetEffects<TEffect>() where TEffect : Effect
        {
            lock (m_effects)
            {
                return m_effects.OfType<TEffect>().ToList();
            }
        }

        public int VictoryPoints { get; set; }
        public int Strength { get; set; }

        public IReadOnlyDictionary<Type, int> Disciplines
        {
            get
            {
                lock (m_disciplines)
                {
                    return m_disciplines.AsReadOnly();
                }
            }
        }

        public PlayerProperties(Player owner, Player opponent)
        {
            Owner = owner;
            Opponent = opponent;
            m_goods = new Dictionary<Type, Good>();
            m_disciplines = new Dictionary<Type, int>();
            m_effects = new List<Effect>();
            VictoryPoints = 0;
            Strength = 0;
        }

        public void AddGood(Good good)
        {
            lock (m_goods)
            {
                if (m_goods.ContainsKey(good.GetType()))
                {
                    m_goods[good.GetType()].Amount += good.Amount;
                }
                else
                {
                    m_goods.Add(good.GetType(), good.Clone());
                }
            }
        }

        public void AddDiscipline(Discipline discipline)
        {
            lock (m_disciplines)
            {
                Type type = discipline.GetType();
                if (m_disciplines.ContainsKey(type))
                {
                    m_disciplines[type] += 1;
                }
                else
                {
                    m_disciplines[type] = 1;
                }
            }
        }

        public void AddEffect(Effect effect)
        {
            lock (m_effects)
            {
                m_effects.Add(effect);
            }
        }

        private readonly IDictionary<Type, Good> m_goods;
        private readonly IDictionary<Type, int> m_disciplines;
        private readonly List<Effect> m_effects;
    }
}
