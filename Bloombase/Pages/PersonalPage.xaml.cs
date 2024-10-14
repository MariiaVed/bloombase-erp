namespace Bloombase.Pages
{
    public partial class PersonalPage : ContentPage
    {
        public PersonalPage()
        {
            InitializeComponent();
            BindingContext = new PersonalPageViewModel(context: new BloombaseContext());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Refresh the list every time the page appears
            var viewModel = BindingContext as PersonalPageViewModel;
            viewModel?.LoadPageCommand.Execute(null);
        }
    }
}