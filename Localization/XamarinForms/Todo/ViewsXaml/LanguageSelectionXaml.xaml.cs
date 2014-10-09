using System;

namespace Todo
{
    public partial class LanguageSelectionXaml
    {
        public LanguageSelectionXaml()
        {
            InitializeComponent();
        }

        private async void Next_OnClick(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}