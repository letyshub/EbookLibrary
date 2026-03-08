using System;
using System.IO;
using System.Text.Json;
using EbookLibrary.Services;

namespace EbookLibrary
{
    public class SettingsService : ISettingsService
    {
        private static readonly string SettingsPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

        private SettingsData data;

        public SettingsService()
        {
            Load();
        }

        public string Language
        {
            get => data.Language;
            set => data.Language = value;
        }

        public string Theme
        {
            get => data.Theme;
            set => data.Theme = value;
        }

        public void Save()
        {
            File.WriteAllText(SettingsPath, JsonSerializer.Serialize(data));
        }

        private void Load()
        {
            if (File.Exists(SettingsPath))
            {
                try
                {
                    data = JsonSerializer.Deserialize<SettingsData>(File.ReadAllText(SettingsPath))
                           ?? new SettingsData();
                    return;
                }
                catch { }
            }
            data = new SettingsData();
        }
    }

    internal class SettingsData
    {
        public string Language { get; set; } = "pl";
        public string Theme { get; set; } = "Light";
    }
}
