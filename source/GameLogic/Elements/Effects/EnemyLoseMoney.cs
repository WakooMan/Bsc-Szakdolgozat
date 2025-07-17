using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.Elements.Effects
{
    public class EnemyLoseMoney : Effect
    {
        public int Money { get; set; }

        public EnemyLoseMoney() { }

        private EnemyLoseMoney(EnemyLoseMoney enemyLoseMoney)
        {
            Money = enemyLoseMoney.Money;
        }

        public override void Apply()
        {
            throw new NotImplementedException();
        }

        public override EnemyLoseMoney Clone()
        {
            return new EnemyLoseMoney(this);
        }
    }
}
