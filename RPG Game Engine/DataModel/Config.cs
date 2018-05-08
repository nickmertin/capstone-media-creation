using System.Collections.Generic;
using System.Windows.Markup;

namespace Mertin.RPG.Engine.DataModel
{
    [ContentProperty("Events")]
    public sealed class Config
    {
        public string StartEventKey { get; set; }

        public IDictionary<string, Event> Events { get; } = new Dictionary<string, Event>();
    }
}
