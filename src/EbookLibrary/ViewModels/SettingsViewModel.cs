using Caliburn.Micro;
using EbookLibrary.Helpers;
using EbookLibrary.Messages;
using EbookLibrary.Services;

namespace EbookLibrary.ViewModels
{
    public class SettingsViewModel : Screen
    {
        private readonly ISettingsService settingsService;
        private readonly IEventAggregator eventAggregator;
        private string selectedLanguage;
        private string selectedTheme;

        public SettingsViewModel(ISettingsService settingsService, IEventAggregator eventAggregator)
        {
            this.settingsService = settingsService;
            this.eventAggregator = eventAggregator;
            selectedLanguage = settingsService.Language;
            selectedTheme = settingsService.Theme;
        }

        public bool IsPolish
        {
            get => selectedLanguage == "pl";
            set { if (value) SelectedLanguage = "pl"; }
        }

        public bool IsEnglish
        {
            get => selectedLanguage == "en";
            set { if (value) SelectedLanguage = "en"; }
        }

        public bool IsLightTheme
        {
            get => selectedTheme == "Light";
            set { if (value) SelectedTheme = "Light"; }
        }

        public bool IsDarkTheme
        {
            get => selectedTheme == "Dark";
            set { if (value) SelectedTheme = "Dark"; }
        }

        private string SelectedLanguage
        {
            set
            {
                selectedLanguage = value;
                NotifyOfPropertyChange(() => IsPolish);
                NotifyOfPropertyChange(() => IsEnglish);
                settingsService.Language = value;
                settingsService.Save();
                LocalizationHelper.ApplyLanguage(value);
            }
        }

        private string SelectedTheme
        {
            set
            {
                selectedTheme = value;
                NotifyOfPropertyChange(() => IsLightTheme);
                NotifyOfPropertyChange(() => IsDarkTheme);
                settingsService.Theme = value;
                settingsService.Save();
                ThemeHelper.ApplyTheme(value);
            }
        }

        public void Back()
        {
            _ = eventAggregator.PublishOnUIThreadAsync(new DisplayListEbookViewMessage());
        }
    }
}
