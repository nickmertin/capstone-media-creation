using Mertin.RPG.Engine.DataModel;
using System.Windows;
using System.Windows.Media;

namespace Mertin.RPG.Engine
{
    public sealed class GameEngine
    {
        Config config;

        public GameEngine(Config config)
        {
            this.config = config;
        }

        public void Run(GameWindow window)
        {
            var current = config.Events[config.StartEventKey];
            while (true)
            {
                window.WriteLine(current.DisplayText, Colors.Yellow);
                switch (current)
                {
                    case Info i:
                        window.Write("Press any key to continue...", Colors.Red);
                        window.WaitForKey(k => true);
                        window.WriteLine("", new Color());
                        current = config.Events[i.NextKey];
                        break;
                    case Fork f:
                        window.WriteLine(f.Question + "\n", Colors.Red);
                        for (int i = 0; i < f.Options.Count; ++i)
                            window.WriteLine($"  - [{i}]: {f.Options[i].Text}", Colors.Yellow);
                        window.WriteLine("", new Color());
                        current = config.Events[f.Options[window.Get("What would you like to do? ", i => i < f.Options.Count && i >= 0, int.Parse)].NextKey];
                        break;
                    case GameOver g:
                        window.WriteLine(g.Win ? "You won!" : "You lost!", Colors.Aqua);
                        window.Write("Press any key to exit...", Colors.Red);
                        window.WaitForKey(k => true);
                        Application.Current.Dispatcher.Invoke(Application.Current.Shutdown);
                        return;
                }
            }
        }
    }
}
