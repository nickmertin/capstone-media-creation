using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Mertin.RPG.Engine
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public sealed partial class GameWindow : Window
    {
        GameEngine engine;

        public GameWindow(GameEngine engine)
        {
            this.engine = engine;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            new Task(() => engine.Run(this)).Start();
        }

        public void Write(string s, Color c) => Dispatcher.Invoke(() =>
        {
            var b = new SolidColorBrush(c);
            IEnumerable<string> strs = s.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
            if (panel.Children.Count > 0)
            {
                panel.Children.OfType<TextBlock>().Last().Inlines.Add(new Run(strs.First()) { Foreground = b });
                strs = strs.Skip(1);
            }
            foreach (var l in strs)
                panel.Children.Add(new TextBlock { Text = l, Foreground = b });
        });

        public void Write(object o, Color c) => Write(o.ToString(), c);

        public void WriteLine(object o, Color c) => Write(o.ToString() + "\n", c);

        public string ReadLine(Color c)
        {
            Run t = null;
            int offset = 0;
            KeyEventHandler h = null;
            using (var ewh = new EventWaitHandle(false, EventResetMode.ManualReset))
            {
                h = (sender, e) =>
                {
                    switch (e.Key)
                    {
                        case Key.Back:
                            if (offset > 0)
                                t.Text = t.Text.Remove(--offset, 1);
                            break;
                        case Key.Delete:
                            if (offset < t.Text.Length)
                                t.Text = t.Text.Remove(offset, 1);
                            break;
                        case Key.Left:
                            if (offset > 0)
                                --offset;
                            break;
                        case Key.Right:
                            if (offset < t.Text.Length)
                                ++offset;
                            break;
                        case Key.Enter:
                            KeyDown -= h;
                            panel.Children.Add(new TextBlock());
                            ewh.Set();
                            break;
                        default:
                            t.Text = t.Text.Insert(offset++, KeyUtility.GetCharFromKey(e.Key).ToString());
                            break;
                    }
                };
                Dispatcher.Invoke(() =>
                {
                    t = new Run() { Foreground = new SolidColorBrush(c) };
                    if (panel.Children.Count > 0)
                        panel.Children.Cast<TextBlock>().Last().Inlines.Add(t);
                    else
                        panel.Children.Add(new TextBlock(t));
                    KeyDown += h;
                });
                ewh.WaitOne();
                return t.Dispatcher.Invoke(() => t.Text);
            }
        }

        public void WaitForKey(Predicate<Key> predicate)
        {
            KeyEventHandler h = null;
            using (var ewh = new EventWaitHandle(false, EventResetMode.ManualReset))
            {
                h = (sender, e) =>
                {
                    if (predicate(e.Key))
                    {
                        KeyDown -= h;
                        ewh.Set();
                    }
                };
                Dispatcher.Invoke(() => KeyDown += h);
                ewh.WaitOne();
            }
        }

        public void Clear() => Dispatcher.Invoke(() => panel.Children.Clear());

        public T Get<T>(string prompt, Predicate<T> condition, Func<string, T> parser)
        {
            bool started = false;
            T result;
            do
            {
                if (started)
                    WriteLine("Invalid input!", Colors.Red);
                else
                    started = true;
                _try:
                try
                {
                    Write(prompt, Colors.Red);
                    result = parser(ReadLine(Colors.Magenta));
                }
                catch
                {
                    WriteLine("Invalid input!", Colors.Red);
                    goto _try;
                }
            } while (!condition(result));
            return result;
        }
    }
}
