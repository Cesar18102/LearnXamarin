using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LearnXamarin.Models;

namespace LearnXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordPage : ContentPage
    {
        private List<Entry> Inputs = new List<Entry>();
        private Dictionary<Entry, Predicate<Entry>> Validators = new Dictionary<Entry, Predicate<Entry>>();
        private Dictionary<Entry, Constants.Alert> Alerts = new Dictionary<Entry, Constants.Alert>();

        private static readonly Color ValidEntryColor = Color.Green;
        private static readonly Color InvalidEntryColor = Color.Red;

        public ChangePasswordPage()
        {
            InitializeComponent();

            Inputs.Add(OldPasswordEntry);
            Inputs.Add(NewPasswordEntry);
            Inputs.Add(ConfirmNewPasswordEntry);

            Validators.Add(OldPasswordEntry, E => User.PasswordValidator(E.Text) && Constants.Hash(E.Text, Constants.UTF8, Constants.mD5) == App.CurrentUser.password_md5);
            Validators.Add(NewPasswordEntry, E => User.PasswordValidator(E.Text));
            Validators.Add(ConfirmNewPasswordEntry, E => E.Text != null && NewPasswordEntry.Text != null && E.Text == NewPasswordEntry.Text);

            Alerts.Add(OldPasswordEntry, new Constants.Alert("Ошибка ввода", "Пароль должен состоять из латинских букв, цифр и его длина должна находится в диапазоне от 8 до 12 символов", "OK", DisplayAlert));
            Alerts.Add(NewPasswordEntry, new Constants.Alert("Ошибка ввода", "Пароль должен состоять из латинских букв, цифр и его длина должна находится в диапазоне от 8 до 12 символов", "OK", DisplayAlert));
            Alerts.Add(ConfirmNewPasswordEntry, new Constants.Alert("Ошибка ввода", "Пароль не подтвержден", "OK", DisplayAlert));

            OldPasswordEntry.TextChanged += ChangePasswordEntry_TextChanged;
            NewPasswordEntry.TextChanged += ChangePasswordEntry_TextChanged;
            ConfirmNewPasswordEntry.TextChanged += ChangePasswordEntry_TextChanged;

            ConfirmButtom.Clicked += ConfirmButtom_Clicked;
        }

        private bool Validate(bool Messages)
        {
            return Constants.Validate<Entry>(Inputs, Validators, Alerts,
                                            E => E.BackgroundColor = InvalidEntryColor,
                                            E => E.BackgroundColor = ValidEntryColor,
                                            Messages);
        }

        private void ChangePasswordEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            Validate(false);
        }

        private async void ConfirmButtom_Clicked(object sender, EventArgs e)
        {
            string NewPassword = NewPasswordEntry.Text;

            if (!Validate(true))
                return;

            ChangePasswordActivityIndicator.IsRunning = true;

            await App.CurrentUser.ChangePassword(Constants.Hash(NewPassword, Constants.UTF8, Constants.mD5)).
                ContinueWith(T => { ChangePasswordActivityIndicator.IsRunning = false; });

            await DisplayAlert("Отлично", "Пароль изменен", "OK");
            App.Current.MainPage = new LessonListPage();
        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new LessonListPage();
            return true;
        }
    }
}