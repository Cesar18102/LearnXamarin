using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using LearnXamarin.Pages;

namespace LearnXamarin
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            SignUpButton.Clicked += SignUpButton_Clicked;
            LogInButton.Clicked += LogInButton_Clicked;
        }

        private void LogInButton_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new LogInPage();
        }

        private void SignUpButton_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new SignUpPage();
        }
    }
}
