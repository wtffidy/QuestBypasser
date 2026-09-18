using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Skua.Core.Interfaces;
using Skua.Core.Models.Items;
using Skua.Core.Interfaces.Services;

namespace QuestBypasser
{
    public class Bypasser : ISkuaPlugin
    {
        public string Name => "Quest Bypasser";
        public string Author => "User";
        public string Description => "Updates map quest IDs and reloads current map.";
        public string Version => "1.0.0";

        public List<IOption> Options => [];

        private IScriptInterface _bot;
        private IPluginHelper _helper;
        private PluginWindow _pluginWindow;

        public void Load(IServiceProvider provider, IPluginHelper helper)
        {
            _bot = provider.GetRequiredService<IScriptInterface>();
            _helper = helper;

            Application.Current.Dispatcher.Invoke(() =>
            {
                _pluginWindow = new PluginWindow(_bot);
            });

            // Add menu button to open the plugin window
            _helper.AddMenuButton(Name, () =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (_pluginWindow == null)
                    {
                        _pluginWindow = new PluginWindow(_bot);
                    }

                    _pluginWindow.Show();
                    _pluginWindow.BringIntoView();
                    _pluginWindow.Activate();
                });
            });
        }

        public void Unload()
        {
            _helper?.RemoveMenuButton(Name);
            Application.Current.Dispatcher.Invoke(() =>
            {
                _pluginWindow?.Close();
                _pluginWindow = null;
            });
        }
    }
}