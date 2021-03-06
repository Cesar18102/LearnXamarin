﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LearnXamarin.Models;
using LearnXamarin.DB;

namespace LearnXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        private List<Entry> Inputs = new List<Entry>();
        private Dictionary<Entry, Predicate<Entry>> Validators = new Dictionary<Entry, Predicate<Entry>>();
        private Dictionary<Entry, Constants.Alert> Alerts = new Dictionary<Entry, Constants.Alert>();

        private static readonly Color ValidEntryColor = Color.Green;
        private static readonly Color InvalidEntryColor = Color.Red;

        public SignUpPage()
        {
            InitializeComponent();

            Inputs.Add(LoginEntry);
            Inputs.Add(PasswordEntry);
            Inputs.Add(ConfirmPasswordEntry);
            Inputs.Add(EmailEntry);

            Validators.Add(LoginEntry, E => User.LoginValidator(E.Text));
            Validators.Add(PasswordEntry, E => User.PasswordValidator(E.Text));
            Validators.Add(ConfirmPasswordEntry, E => E.Text != null && PasswordEntry.Text != null && E.Text == PasswordEntry.Text);
            Validators.Add(EmailEntry, E => User.EmailValidator(E.Text));

            Alerts.Add(LoginEntry, new Constants.Alert("Ошибка ввода", "Логин должен состоять из латинских букв, цифр и его длина должна находится в диапазоне от 3 до 20 символов", "OK", DisplayAlert));
            Alerts.Add(PasswordEntry, new Constants.Alert("Ошибка ввода", "Пароль должен состоять из латинских букв, цифр и его длина должна находится в диапазоне от 8 до 12 символов", "OK", DisplayAlert));
            Alerts.Add(ConfirmPasswordEntry, new Constants.Alert("Ошибка ввода", "Пароль не подтвержден", "OK", DisplayAlert));
            Alerts.Add(EmailEntry, new Constants.Alert("Ошибка ввода", "Email содержит запрещенные символы", "OK", DisplayAlert));

            LoginEntry.TextChanged += SignUpEntry_TextChanged;
            PasswordEntry.TextChanged += SignUpEntry_TextChanged;
            ConfirmPasswordEntry.TextChanged += SignUpEntry_TextChanged;
            EmailEntry.TextChanged += SignUpEntry_TextChanged;

            Send.Clicked += Send_Clicked;
        }

        private void SignUpEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            Validate(false);
        }

        private bool Validate(bool Messages)
        {
            return Constants.Validate<Entry>(Inputs, Validators, Alerts,
                                      E => E.BackgroundColor = InvalidEntryColor,
                                      E => E.BackgroundColor = ValidEntryColor,
                                      Messages);
        }

        private async void Send_Clicked(object sender, EventArgs e)
        {
            string Login = LoginEntry.Text;
            string Password = PasswordEntry.Text;
            string PasswordConfirmation = ConfirmPasswordEntry.Text;
            string Email = PasswordEntry.Text;

            if (!Validate(true))
                return;

            SignupActivityIndicator.IsRunning = true;

            List<User> SameLoginUsers = await DbContext.SelectAsync<User>(User => User.login == Login);

            if (SameLoginUsers.Count != 0)
            {
                await DisplayAlert("Ошибка регистрации", "Пользователь с таким логином уже существует", "OK");
                SignupActivityIndicator.IsRunning = false;
                return;
            }

            User U = new User(LoginEntry.Text, PasswordEntry.Text, EmailEntry.Text);
            await DbContext.InsertAsync<User>(U).
                            ContinueWith(T => { SignupActivityIndicator.IsRunning = false; });

            await DisplayAlert("Отлично", "Вы зарегистрировались!", "OK");
            App.Current.MainPage = new LogInPage();
        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new MainPage();
            return true;
        }
    }
}