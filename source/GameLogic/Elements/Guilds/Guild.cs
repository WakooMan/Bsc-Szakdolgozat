using System.Xml.Serialization;

namespace GameLogic.Elements.Guilds
{
    [XmlInclude(typeof(BuilderGuild)),
     XmlInclude(typeof(TraderGuild)),
     XmlInclude(typeof(ScienceGuild)),
     XmlInclude(typeof(StrategistGuild)),
     XmlInclude(typeof(SailorGuild)),
     XmlInclude(typeof(MagistrateGuild)),
     XmlInclude(typeof(ExtortionistGuild))]
    public abstract class Guild
    {
        public abstract Guild Clone();

        public virtual void Apply(IGameContext gameContext, Player owner, Player opponent)
        {
        }

        public virtual void OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
        }
    }
}
