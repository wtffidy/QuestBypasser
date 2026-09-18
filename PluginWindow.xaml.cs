using System.Windows;
using Skua.Core.Interfaces;

namespace QuestBypasser
{
    public partial class PluginWindow : Window
    {
        public PluginWindow(IScriptInterface bot)
        {
            InitializeComponent();
            PluginView.SetBot(bot);
        }

        protected override void OnClosed(System.EventArgs e)
        {
            base.OnClosed(e);
        }
    }
}
