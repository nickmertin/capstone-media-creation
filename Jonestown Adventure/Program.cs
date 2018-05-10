using Mertin.RPG.Engine;
using Mertin.RPG.Engine.DataModel;
using System;
using System.Windows;

namespace Jonestown
{
    class Program
    {
        [STAThread]
        public static void Main(String[] args)
        {
            var app = new Application { ShutdownMode = ShutdownMode.OnMainWindowClose };
            var config = Application.LoadComponent(new Uri("/Config.xaml", UriKind.Relative)) as Config;
            app.Run(new GameWindow(new GameEngine(config)));
        }
    }
}
