using System.Collections.Generic;
using System.Windows.Markup;

namespace Mertin.RPG.Engine.DataModel
{
    [ContentProperty("Options")]
    public sealed class Fork : Event
    {
        public string Question { get; set; }

        public List<ForkOption> Options { get; } = new List<ForkOption>();
    }
}
