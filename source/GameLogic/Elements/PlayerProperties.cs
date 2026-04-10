using GameLogic.Elements.Disciplines;
using GameLogic.Elements.Goods;

namespace GameLogic.Elements
{
    public class PlayerProperties
    {
        public Player Player { get; }
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
        public int VictoryPoints { get; set; }

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

        public PlayerProperties(Player player)
        {
            Player = player;
            m_goods = new Dictionary<Type, Good>();
            m_disciplines = new Dictionary<Type, int>();
            VictoryPoints = 0;
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

        private readonly IDictionary<Type, Good> m_goods;
        private readonly IDictionary<Type, int> m_disciplines;
    }
}
