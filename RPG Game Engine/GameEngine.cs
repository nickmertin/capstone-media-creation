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
                if (current is Info)
                {
                    window.Write("Press any key to continue...", Colors.Red);
                    window.WaitForKey(k => true);
                    window.WriteLine("", new Color());
                    current = config.Events[(current as Info).NextKey];
                }
                else if (current is Fork)
                {
                    var c = current as Fork;
                    window.WriteLine(c.Question + "\n", Colors.Red);
                    for (int i = 0; i < c.Options.Count; ++i)
                        window.WriteLine($"  - [{i}]: {c.Options[i].Text}", Colors.Yellow);
                    window.WriteLine("", new Color());
                    current = config.Events[c.Options[window.Get("What would you like to do? ", i => i < c.Options.Count && i >= 0, int.Parse)].NextKey];
                }
                else if (current is GameOver)
                {
                    window.Write("Press any key to continue...", Colors.Red);
                    window.WaitForKey(k => true);
                    Application.Current.Dispatcher.Invoke(Application.Current.Shutdown);
                    return;
                }
            }
        }
    }
}
