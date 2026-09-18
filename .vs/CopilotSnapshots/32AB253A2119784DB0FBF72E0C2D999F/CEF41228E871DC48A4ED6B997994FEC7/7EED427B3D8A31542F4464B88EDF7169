using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Skua.Core.Interfaces;
using Skua.Core.Interfaces.Services;

namespace QuestBypasser
{
    public partial class MainPluginView : UserControl
    {
        private IScriptInterface _bot;
        private System.Windows.Threading.DispatcherTimer _statusCheckTimer;

        public MainPluginView()
        {
            InitializeComponent();
        }

        public void SetBot(IScriptInterface bot)
        {
            _bot = bot;
            InitializeStatusCheck();
            InitializePresets();
        }

        private void InitializeStatusCheck()
        {
            // Check status immediately
            RefreshStatus();

            // Setup periodic status check (every 1 second)
            _statusCheckTimer = new System.Windows.Threading.DispatcherTimer();
            _statusCheckTimer.Interval = TimeSpan.FromSeconds(1);
            _statusCheckTimer.Tick += (s, e) => RefreshStatus();
            _statusCheckTimer.Start();
        }

        private void InitializePresets()
        {
            // Get unique map names from presets (first occurrence of each map)
            var uniqueMaps = new Dictionary<string, int>();
            foreach (var (map, questId) in QuestPresets.MapSpecific)
            {
                if (!uniqueMaps.ContainsKey(map))
                {
                    uniqueMaps[map] = questId;
                }
            }

            // Populate combo box with presets
            CmbMapPreset.Items.Clear();
            CmbMapPreset.Items.Add(new KeyValuePair<string, int>("-- Select a preset --", 0));

            foreach (var (map, questId) in uniqueMaps.OrderBy(x => x.Key))
            {
                CmbMapPreset.Items.Add(new KeyValuePair<string, int>(map, questId));
            }

            CmbMapPreset.SelectedIndex = 0;
        }

        private void CmbMapPreset_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbMapPreset.SelectedItem is KeyValuePair<string, int> selected)
            {
                // Update quest ID textbox with the selected preset's quest ID
                TxtQuestId.Text = selected.Value.ToString();
            }
        }

        private void RefreshStatus()
        {
            if (_bot == null)
            {
                UpdateStatus("Status: Bot not initialized.");
                return;
            }

            try
            {
                bool playerLoggedIn = _bot.Player.LoggedIn;
                bool playerLoaded = _bot.Player.Loaded;
                bool mapLoaded = _bot.Map.Loaded;

                if (!playerLoggedIn)
                {
                    UpdateStatus("Status: Waiting for player login...");
                }
                else if (!playerLoaded)
                {
                    UpdateStatus("Status: Waiting for player to load...");
                }
                else if (!mapLoaded)
                {
                    UpdateStatus("Status: Waiting for map to load...");
                }
                else
                {
                    UpdateStatus("Status: Ready");
                }
            }
            catch
            {
                // Silently catch exceptions during status checks
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _statusCheckTimer?.Stop();
            _statusCheckTimer = null;
        }

        private async void BtnReload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BtnReload.IsEnabled = false;
                UpdateStatus("Status: Executing...");
                if (_bot != null)
                {
                    _bot.Log("Quest Bypasser: Button clicked - starting operation");
                }

                // Parse the quest ID from the textbox
                if (!int.TryParse(TxtQuestId.Text, out int questId))
                {
                    UpdateStatus("Status: Invalid quest ID.");
                    if (_bot != null) _bot.Log("Quest Bypasser: Invalid quest ID entered.");
                    return;
                }

                // Get the selected map name from the preset dropdown
                string targetMapName = null;
                if (CmbMapPreset.SelectedItem is KeyValuePair<string, int> selected && selected.Key != "-- Select a preset --")
                {
                    targetMapName = selected.Key;
                    if (_bot != null) _bot.Log($"Quest Bypasser: Target map: {targetMapName}");

                    // If quest ID is 0 or not set, use the preset's quest ID
                    if (questId == 0)
                    {
                        questId = selected.Value;
                        if (_bot != null) _bot.Log($"Quest Bypasser: Using preset quest ID: {questId}");
                    }
                }

                UpdateStatus("Status: Running quest bypass...");

                // Run the quest bypass operation asynchronously without blocking the UI
                await Task.Run(async () =>
                {
                    try
                    {
                        if (_bot == null)
                        {
                            UpdateStatus("Status: Bot is null!");
                            return;
                        }

                        if (!_bot.Player.LoggedIn)
                        {
                            UpdateStatus("Status: Player not logged in.");
                            return;
                        }

                        if (!_bot.Player.Loaded)
                        {
                            UpdateStatus("Status: Player not loaded.");
                            return;
                        }

                        if (!_bot.Map.Loaded)
                        {
                            UpdateStatus("Status: Map not loaded.");
                            return;
                        }

                        // If no quest ID is set, auto-detect the last quest from the map
                        if (questId == 0)
                        {
                            var mapQuests = _bot.Quests.Tree;
                            if (mapQuests != null && mapQuests.Count > 0)
                            {
                                questId = mapQuests.Max(q => q.ID);
                                _bot.Log($"Quest Bypasser: Auto-detected quest ID from map: {questId}");
                            }
                            else
                            {
                                UpdateStatus("Status: No quests found in map.");
                                _bot?.Log("Quest Bypasser: No quests available in current map.");
                                return;
                            }
                        }

                        // Perform the quest bypass
                        _bot.Log($"Updating Quest ID: {questId}");
                        _bot.Quests.UpdateQuest(questId);
                        _bot.Log($"Quest bypass completed for ID: {questId}");

                        await Task.Delay(1500);

                        // If a target map was selected, either reload or join it
                        if (!string.IsNullOrEmpty(targetMapName))
                        {
                            string currentMap = _bot.Map.Name;
                            _bot.Log($"Current map: {currentMap}, Target map: {targetMapName}");

                            if (currentMap.Equals(targetMapName, StringComparison.OrdinalIgnoreCase))
                            {
                                _bot.Log($"Already on {targetMapName}, reloading...");
                                _bot.Map.Reload();
                                UpdateStatus($"Status: Reloading {targetMapName}...");
                            }
                            else
                            {
                                _bot.Log($"Joining map: {targetMapName}");
                                _bot.Player.Goto(targetMapName);
                                UpdateStatus($"Status: Joining {targetMapName}...");
                            }

                            _bot.Wait.ForMapLoad(targetMapName);
                            _bot.Log($"Map {targetMapName} loaded");
                        }
                        else
                        {
                            _bot.Log("No target map, reloading current...");
                            _bot.Map.Reload();
                            UpdateStatus("Status: Reloading current map...");
                            _bot.Wait.ForMapLoad(_bot.Map.Name);
                        }

                        _bot.Wait.ForTrue(() => _bot.Player.Loaded, 30);
                        _bot.Log("Player loaded, operation complete");

                        UpdateStatus($"Status: Updated Quest #{questId} & Reloaded");
                    }
                    catch (Exception innerEx)
                    {
                        if (_bot != null) _bot.Log($"Quest Bypasser Error: {innerEx}");
                        UpdateStatus($"Error: {innerEx.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                UpdateStatus($"Critical Error: {ex.Message}");
                if (_bot != null) _bot.Log($"Quest Bypasser Critical Error: {ex}");
            }
            finally
            {
                BtnReload.IsEnabled = true;
            }
        }

        private void UpdateStatus(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                TxtStatus.Text = message;
            });
        }
    }
}