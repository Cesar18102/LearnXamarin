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

            int i = 0, ei = 0;
            foreach (Lesson L in App.Lessons)
            {
                Button LessonEntry = new Button();
                LessonEntry.Text = L.is_exam ? $"К/Р #{++ei}: {L.title}" : $"Урок #{++i}: {L.title}";
                LessonEntry.Style = (Style)Resources["Lesson"];

                int li = L.is_exam ? ei : i;

                if (L.is_exam)
                    LessonEntry.Clicked += (context, args) => { App.Current.MainPage = new ExamPage(L, li); };
                else
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