using System.Windows.Markup;

namespace Mertin.RPG.Engine.DataModel
{
    [ContentProperty("DisplayText")]
    public sealed class Info : Event
    {
        public string NextKey { get; set; }
    }
}
