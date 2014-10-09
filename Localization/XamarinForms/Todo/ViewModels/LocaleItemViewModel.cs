using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using Xamarin.Forms;

namespace Todo
{
    public class LocaleItemViewModel : INotifyPropertyChanged
    {
        private static readonly string[] supportedCultureTags = { "en", "de", "es", "fr", "ja", "pt-BR", "ru", "zh-Hans", "zh-Hant" };
        private List<LocaleItemWithCultureCode> _all = new List<LocaleItemWithCultureCode>(GetLocaleItems());

        public event PropertyChangedEventHandler PropertyChanged;

        public List<LocaleItemWithCultureCode> All { get { return _all; } }
        public ICommand ChangeLanguageCommand { get; protected set; }
        public LocaleItemWithCultureCode SelectedItem { get; set; }

        public LocaleItemViewModel()
        {
            ChangeLanguageCommand = new Command(() =>
            {
                DependencyService.Get<ILocale>().SetLocale(SelectedItem);
                L10n.SetLocale();
                var netLanguage = DependencyService.Get<ILocale>().GetCurrent();
                Resx.AppResources.Culture = new CultureInfo(netLanguage);
            });
        }

        private static IEnumerable<LocaleItemWithCultureCode> GetLocaleItems()
        {
            var localeList = new List<LocaleItemWithCultureCode>();
            foreach (var supportedCulture in supportedCultureTags)
            {
                var localeItem = new LocaleItemWithCultureCode();
                var cultureInfo = new CultureInfo(supportedCulture);

                localeItem.CultureCode = supportedCulture;
                localeItem.LanguageName = cultureInfo.DisplayName;
                localeItem.LanguageCode = cultureInfo.TwoLetterISOLanguageName;
                try
                {
                    var regionInfo = new RegionInfo(cultureInfo.Name);
                    localeItem.Country = regionInfo.EnglishName;
                    localeItem.CountryCode = regionInfo.TwoLetterISORegionName;
                }
                catch (ArgumentException)
                {
                    localeItem.Country = localeItem.CountryCode = String.Empty;
                }
                localeList.Add(localeItem);
            }
            return localeList;
        }
    }
}