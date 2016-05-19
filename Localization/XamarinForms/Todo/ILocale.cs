using System;

namespace Todo
{
	public interface ILocale
	{
		string GetCurrent();

		void SetLocale();

        void SetLocale(LocaleItemWithCultureCode localeItem);
	}
}

