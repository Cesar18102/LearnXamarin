﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LearnXamarin.DB;
using LearnXamarin.Models;

namespace LearnXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogInPage : ContentPage
    {
        private List<Entry> Inputs = new List<Entry>();
        private Dictionary<Entry, Predicate<Entry>> Validators = new Dictionary<Entry, Predicate<Entry>>();
        private Dictionary<Entry, Constants.Alert> Alerts = new Dictionary<Entry, Constants.Alert>();

        private static readonly Color ValidEntryColor = Color.Green;
        private static readonly Color InvalidEntryColor = Color.Red;

        public LogInPage()
        {
            InitializeComponent();

            Inputs.Add(LoginEntry);
            Inputs.Add(PasswordEntry);

            Validators.Add(LoginEntry, E => User.LoginValidator(E.Text));
            Validators.Add(PasswordEntry, E => User.PasswordValidator(E.Text));

            Alerts.Add(LoginEntry, new Constants.Alert("Input error", "Login must consist of latin letters, digits and its length must be betweeen 3 and 20", "OK", DisplayAlert));
            Alerts.Add(PasswordEntry, new Constants.Alert("Input error", "Password must consist of latin letters, digits and its length must be betweeen 3 and 20", "OK", DisplayAlert));

            LoginEntry.TextChanged += LogInEntry_TextChanged;
            PasswordEntry.TextChanged += LogInEntry_TextChanged;

            Send.Clicked += Send_Clicked;
        }

        private void LogInEntry_TextChanged(object sender, TextChangedEventArgs e)
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

            if (!Validate(true))
                return;

            LoginActivityIndicator.IsRunning = true;
            User User = await DbContext.SingleAsync<User>(U => U.login == Login);
            LoginActivityIndicator.IsRunning = false;

            if (User.login != Login)
            {
                await DisplayAlert("Error", "No such user", "OK");
                return;
            }

            if(User.password_md5 != Constants.Hash(Password, Constants.UTF8, Constants.mD5))
            {
                await DisplayAlert("Error", "Wrong password", "OK");
                return;
            }

            App.CurrentUser = User;
            await DisplayAlert("Success", "Authorization successful", "OK");
            App.Current.MainPage = new LessonListPage();
        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new MainPage();
            return true;
        }
    }
}