using AddBook.ViewModels;
using AddBook.Views;
using Xamarin.Forms;

namespace AddBook
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var page = new ContactsListPage
            {
                BindingContext = new ContactsListViewModel()
            };
            MainPage = new NavigationPage(page);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
