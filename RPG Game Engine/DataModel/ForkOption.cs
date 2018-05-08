using System.Windows.Markup;

namespace Mertin.RPG.Engine.DataModel
{
    [ContentProperty("Text")]
    public sealed class ForkOption
    {
        public string Text { get; set; }

        public string NextKey { get; set; }
    }
}
