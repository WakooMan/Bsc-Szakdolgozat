using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameLogic.Guilds
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
    }
}
