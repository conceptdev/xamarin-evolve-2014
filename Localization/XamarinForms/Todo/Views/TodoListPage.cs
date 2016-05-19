﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Todo.Resx;

namespace Todo
{
	public class TodoListPage : ContentPage
	{
		ListView listView;
		public TodoListPage ()
		{
			listView = new ListView { RowHeight = 40 };
			listView.ItemTemplate = new DataTemplate (typeof (TodoItemCell));

			listView.ItemSelected += (sender, e) => {
				var todoItem = (TodoItem)e.SelectedItem;

				// use C# localization
				var todoPage = new TodoItemPage();

				// use XAML localization
//				var todoPage = new TodoItemXaml();


				todoPage.BindingContext = todoItem;
				Navigation.PushAsync(todoPage);
			};

			var layout = new StackLayout();
			if (Device.OS == TargetPlatform.WinPhone) { // WinPhone doesn't have the title showing
				layout.Children.Add(new Label{Text="Todo", Font=Font.BoldSystemFontOfSize(NamedSize.Large)});
			}
			layout.Children.Add(listView);
			layout.VerticalOptions = LayoutOptions.FillAndExpand;
			Content = layout;


			ToolbarItem tbi = null;
			if (Device.OS == TargetPlatform.iOS)
			{
				tbi = new ToolbarItem("+", null, () =>
					{
						var todoItem = new TodoItem();
						var todoPage = new TodoItemPage();
						todoPage.BindingContext = todoItem;
						Navigation.PushAsync(todoPage);
					}, 0, 0);
			}
			if (Device.OS == TargetPlatform.Android) { // BUG: Android doesn't support the icon being null
				tbi = new ToolbarItem ("+", "plus", () => {
					var todoItem = new TodoItem();
					var todoPage = new TodoItemPage();
					todoPage.BindingContext = todoItem;
					Navigation.PushAsync(todoPage);
				}, 0, 0);
			}
			if (Device.OS == TargetPlatform.WinPhone)
			{
				tbi = new ToolbarItem("Add", "add.png", () =>
					{
						var todoItem = new TodoItem();
						var todoPage = new TodoItemPage();
						todoPage.BindingContext = todoItem;
						Navigation.PushAsync(todoPage);
					}, 0, 0);
			}

			ToolbarItems.Add (tbi);

			if (Device.OS == TargetPlatform.iOS) {
				var tbi2 = new ToolbarItem ("?", null, () => {
					var todos = App.Database.GetItemsNotDone();
					var tospeak = "";
					foreach (var t in todos)
						tospeak += t.Name + " ";
					if (tospeak == "") tospeak = "there are no tasks to do";

					if (todos.Count () > 0) {
						var s = L10n.Localize("SpeakTaskCount", "Number of tasks to do");
						tospeak = String.Format(s, todos.Count()) + tospeak;
					}

					DependencyService.Get<ITextToSpeech>().Speak(tospeak);
				}, 0, 0);
				ToolbarItems.Add (tbi2);
			}

            ToolbarItems.Add(new ToolbarItem("Change Language", null, async () => {
                var page = new LanguageSelectionXaml();
                await Navigation.PushAsync(page);
            }, 0, 0));

		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
            Title = AppResources.ApplicationTitle; // "Todo";
			listView.ItemsSource = App.Database.GetItems ();
		}
	}
}

