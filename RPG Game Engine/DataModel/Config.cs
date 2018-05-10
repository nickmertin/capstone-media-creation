using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;

namespace Mertin.RPG.Engine.DataModel
{
    [ContentProperty("EventsList")]
    public sealed class Config
    {
        public string StartEventKey { get; set; }

        public List<Event> EventsList { get; } = new List<Event>();

        public IReadOnlyDictionary<string, Event> Events => new ReadOnlyDictionary<string, Event>(EventsList.ToDictionary(e => e.Key));
    }
}
