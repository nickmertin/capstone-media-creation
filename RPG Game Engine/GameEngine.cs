using Mertin.RPG.Engine.DataModel;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace Mertin.RPG.Engine
{
    public sealed class GameEngine
    {
        Config config;
        Dictionary<string, string> storedVars = new Dictionary<string, string>();
        Regex processRegex = new Regex("{{([^{}]+)}}", RegexOptions.Compiled);

        public GameEngine(Config config)
        {
            this.config = config;
        }

        private string Process(string text) => processRegex.Replace(text, match => storedVars.ContainsKey(match.Groups[1].Value) ? storedVars[match.Groups[1].Value] : match.Value);

        public void Run(GameWindow window)
        {
            var current = config.Events[config.StartEventKey];
            while (true)
            {
                window.WriteLine(Process(current.DisplayText), Colors.Yellow);
                switch (current)
                {
                    case Info i:
                        window.Write("Press any key to continue...", Colors.Red);
                        window.WaitForKey(k => true);
                        window.WriteLine("", new Color());
                        current = config.Events[i.NextKey];
                        break;
                    case Fork f:
                        window.WriteLine(Process(f.Question) + "\n", Colors.LightGreen);
                        for (int i = 0; i < f.Options.Count; ++i)
                            window.WriteLine($"  - [{i}]: {Process(f.Options[i].Text)}", Colors.Yellow);
                        window.WriteLine("", new Color());
                        current = config.Events[f.Options[window.Get("Choose an option: ", i => i < f.Options.Count && i >= 0, int.Parse)].NextKey];
                        break;
                    case Prompt p:
                        storedVars[p.StoreKey] = window.Get(Process(p.PromptText) + " ", string.IsNullOrEmpty(p.Regex) ? new Predicate<string>(_ => true) : new Regex(p.Regex).IsMatch, _ => _);
                        current = config.Events[p.NextKey];
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
