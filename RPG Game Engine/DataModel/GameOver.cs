using System.Windows.Markup;

namespace Mertin.RPG.Engine.DataModel
{
    [ContentProperty("DisplayText")]
    public sealed class GameOver : Event
    {
        public bool Win { get; set; }
    }
}
