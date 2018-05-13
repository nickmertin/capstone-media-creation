using System.Windows.Markup;

namespace Mertin.RPG.Engine.DataModel
{
    [ContentProperty("DisplayText")]
    public sealed class Prompt : Event
    {
        public string PromptText { get; set; }

        public string StoreKey { get; set; }

        public string Regex { get; set; }

        public string NextKey { get; set; }
    }
}
