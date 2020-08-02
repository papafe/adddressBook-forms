using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AddBook.Models;
using AddBook.Services;
using Realms;
using Xamarin.Forms;

namespace AddBook.ViewModels
{
    public class ContactsListViewModel : BaseViewModel
    {
        readonly Realm realm;
        readonly IEnumerable<Contact> allContacts;
        readonly Contact selectedContact;

        IEnumerable<Contact> contacts;
        string searchText;

        public ICommand SearchCommand { get; private set; }
        public ICommand AddContactCommand { get; private set; }
        public ICommand DeleteContactCommand { get; private set; }
        public ICommand EditContactCommand { get; private set; }

        public IEnumerable<Contact> Contacts
        {
            get => contacts;
            private set => SetProperty(ref contacts, value);
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                if (SetProperty(ref searchText, value))
                    SearchCommand.Execute(searchText);
            }
        }

        public Contact SelectedContact
        {
            get => selectedContact;
            set
            {
                EditContactCommand.Execute(value);
                OnPropertyChanged();
            }
        }

        public ContactsListViewModel()
        {
            realm = Realm.GetInstance();

            allContacts = realm.All<Contact>()
                .OrderBy(c => c.FullName);

            InitProperties();
            InitCommands();
        }

        void InitProperties()
        {
            Contacts = allContacts;
        }

        void InitCommands()
        {
            SearchCommand = new Command<string>(Search);
            AddContactCommand = new Command(async () => await AddContact());
            EditContactCommand = new Command<Contact>(async (c) => await EditContact(c));
            DeleteContactCommand = new Command<Contact>(DeleteContact);
        }

        #region Commands

        void Search(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
                Contacts = allContacts;
            else
            {
                var comparisonType = StringComparison.OrdinalIgnoreCase;
                Contacts = realm.All<Contact>().Where(c =>
                    c.FullName.Contains(searchText, comparisonType)
                    || c.Nickname.Contains(searchText, comparisonType)
                    || c.Company.Contains(searchText, comparisonType))
                    .OrderBy(c => c.FullName);
            }
        }

        async Task AddContact()
        {
            var transaction = realm.BeginWrite();
            var newContact = realm.Add(new Contact());

            await NavigationService.NavigateTo(new AddEditContactViewModel(transaction, newContact));
        }

        async Task EditContact(Contact contact)
        {
            var transaction = realm.BeginWrite();
            await NavigationService.NavigateTo(new AddEditContactViewModel(transaction, contact));
        }

        void DeleteContact(Contact contact)
        {
            realm.Write(() =>
            {
                realm.Remove(contact);
            });
        }

        #endregion
    }
}
