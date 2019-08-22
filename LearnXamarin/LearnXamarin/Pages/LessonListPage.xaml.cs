using System;
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
    public partial class LessonListPage : ContentPage
    {
        public LessonListPage()
        {
            InitializeComponent();
            this.Appearing += LessonListPage_Appearing;
        }

        private void LessonListPage_Appearing(object sender, EventArgs e)
        {
            App.Lessons.Loading.Wait();
            App.CurrentUser.LoadPassedTasks();

            ChangePasswordButton.Clicked += (context, args) => App.Current.MainPage = new ChangePasswordPage();

            int i = 0;
            foreach (Lesson L in App.Lessons)
            {
                if (L.is_exam)
                    continue;

                Button LessonEntry = new Button();
                LessonEntry.Text = $"Урок #{++i}: {L.title}";
                LessonEntry.Style = (Style)Resources["Lesson"];

                int li = i;
                LessonEntry.Clicked += (context, args) => { App.Current.MainPage = new LessonPage(L, li); };

                LessonsList.Children.Add(LessonEntry);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new MainPage();
            return true;
        }
    }
}