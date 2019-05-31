using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LearnXamarin.Models;

namespace LearnXamarin
{
    public partial class App : Application
    {
        public static User CurrentUser { get; set; }
        public static LessonLibrary Lessons { get; private set; }

        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();

            Lessons = new LessonLibrary();
            Lessons.Load();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
