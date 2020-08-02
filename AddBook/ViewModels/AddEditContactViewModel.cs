using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AddBook.Models;
using AddBook.Services;
using Realms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AddBook.ViewModels
{
    public class AddEditContactViewModel : BaseViewModel
    {
        readonly Transaction transaction;

        public ICommand AddEmailCommand { get; private set; }
        public ICommand DeleteEmailCommand { get; private set; }
        public ICommand AddPhoneNumberCommand { get; private set; }
        public ICommand DeletePhoneNumberCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public Contact Contact { get; private set; }
        public ObservableCollection<EmailAddress> EmailAddresses { get; private set; }
        public ObservableCollection<PhoneNumber> PhoneNumbers { get; private set; }

        public AddEditContactViewModel(Transaction transaction, Contact contact)
        {
            this.transaction = transaction;
            Contact = contact;

            InitProperties();
            InitCommands();
        }

        void InitProperties()
        {
            EmailAddresses = new ObservableCollection<EmailAddress>(Contact.EmailAddresses);
            PhoneNumbers = new ObservableCollection<PhoneNumber>(Contact.PhoneNumbers);
        }

        void InitCommands()
        {
            AddEmailCommand = new Command(AddEmail);
            DeleteEmailCommand = new Command<EmailAddress>(DeleteEmail);
            AddPhoneNumberCommand = new Command(AddPhoneNumber);
            DeletePhoneNumberCommand = new Command<PhoneNumber>(DeletePhoneNumber);
            SaveCommand = new Command(async () => await Save());
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            transaction?.Dispose();
        }

        #region Commands

        async Task Save()
        {
            Contact.EmailAddresses.Clear();
            EmailAddresses.ForEach(Contact.EmailAddresses.Add);

            Contact.PhoneNumbers.Clear();
            PhoneNumbers.ForEach(Contact.PhoneNumbers.Add);

            if (!await PerformValidation())
                return;

            transaction.Commit();

            await NavigationService.GoBack();
        }

        void AddEmail()
        {
            EmailAddresses.Add(new EmailAddress());
        }

        void DeleteEmail(EmailAddress emailAddress)
        {
            EmailAddresses.Remove(emailAddress);
        }

        void AddPhoneNumber()
        {
            PhoneNumbers.Add(new PhoneNumber());
        }

        void DeletePhoneNumber(PhoneNumber phoneNumber)
        {
            PhoneNumbers.Remove(phoneNumber);
        }

        #endregion

        #region Utils

        async Task<bool> PerformValidation()
        {
            if (!Contact.Validate(out var error))
            {
                await DialogService.ShowAlert("Error", error);
                return false;
            }

            return true;
        }

        #endregion
    }
}
