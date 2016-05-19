﻿using System;
using Xamarin.Forms;
using System.Threading;

[assembly:Dependency(typeof(Todo.Android.Locale_Android))]

namespace Todo.Android
{
	public class Locale_Android : Todo.ILocale
	{
		public void SetLocale (){

			var androidLocale = Java.Util.Locale.Default; // user's preferred locale
			var netLocale = androidLocale.ToString().Replace ("_", "-"); 
			var ci = new System.Globalization.CultureInfo (netLocale);
			Thread.CurrentThread.CurrentCulture = ci;
			Thread.CurrentThread.CurrentUICulture = ci;
		}

		/// <remarks>
		/// Not sure if we can cache this info rather than querying every time
		/// </remarks>
		public string GetCurrent() 
		{
			var androidLocale = Java.Util.Locale.Default; // user's preferred locale

			// en, es, ja
			var netLanguage = androidLocale.Language.Replace ("_", "-"); 
			// en-US, es-ES, ja-JP
			var netLocale = androidLocale.ToString().Replace ("_", "-"); 

			#region Debugging output
			Console.WriteLine ("android:  " + androidLocale.ToString());
			Console.WriteLine ("netlang:  " + netLanguage);
			Console.WriteLine ("netlocale:" + netLocale);

			var ci = new System.Globalization.CultureInfo (netLocale);
			Thread.CurrentThread.CurrentCulture = ci;
			Thread.CurrentThread.CurrentUICulture = ci;

			Console.WriteLine ("thread:  "+Thread.CurrentThread.CurrentCulture);
			Console.WriteLine ("threadui:"+Thread.CurrentThread.CurrentUICulture);
			#endregion

			return netLocale;
		}

        public void SetLocale(LocaleItemWithCultureCode cultureCode)
        {
            Locale locale = String.IsNullOrEmpty(cultureCode.CountryCode) ? new Locale(cultureCode.LanguageCode) : new Locale(cultureCode.LanguageCode, cultureCode.CountryCode);
            Locale.Default = locale;
            var config = new global::Android.Content.Res.Configuration();
            config.Locale = locale;
            var context = global::Android.App.Application.Context;
            context.Resources.UpdateConfiguration(config, context.Resources.DisplayMetrics);
        }
	}
}

