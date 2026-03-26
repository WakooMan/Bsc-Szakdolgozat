using GameLogic.Elements.Disciplines;
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Goods.Products;
using GameLogic.Elements.Goods.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.Presenter
{
    public static class TextureIdDictionary
    {
        public static int GetTextureId(string textureName)
        {
            if (s_textureIds.TryGetValue(textureName, out int textureId))
            {
                return textureId;
            }
            else
            {
                throw new ArgumentException($"Texture name '{textureName}' not found in the dictionary.");
            }
        }

        private static readonly Dictionary<string, int> s_textureIds = new()
        {
            { "BlueCardHeader", 1096 },
            { "BrownCardHeader", 1097 },
            { "GrayCardHeader", 1098 },
            { "GreenCardHeader", 1099 },
            { "PurpleCardHeader", 1100 },
            { "RedCardHeader", 1101 },
            { "YellowCardHeader", 1102 },
            { "Strength", 1008 },
            { nameof(Wood), 1103 },
            { nameof(Clay), 1104 },
            { nameof(Glass), 1105 },
            { nameof(Papirus), 1106 },
            { nameof(Stone), 1107 },
            { "CardNameBackground", 1108 },
            { "Coin", 1109 },
            { nameof(Building), 284 },
            { nameof(Geography), 285 },
            { nameof(Healing), 286 },
            { nameof(Mechanics), 287 },
            { nameof(Physics), 288 },
            { nameof(Trading), 289 },
            { nameof(Writing), 290 },
            { nameof(VictoryPoints), 291 },
            { "Slash", 292 },
            { nameof(GrayCard), 1098},
            { nameof(BrownCard), 1097},
            { nameof(BlueCard), 1096},
            { nameof(GreenCard), 1099},
            { nameof(RedCard), 1101},
            { nameof(PurpleCard), 1100},
            { nameof(YellowCard), 1102}

        };
    }
}
