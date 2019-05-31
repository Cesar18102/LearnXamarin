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

            int i = 1;
            foreach (Lesson L in App.Lessons)
            {
                Button LessonEntry = new Button();
                LessonEntry.Text = $"Lesson #{i++}: {L.title}";
                LessonEntry.Style = (Style)Resources["Lesson"];
                LessonEntry.Clicked += (context, args) => App.Current.MainPage = new LessonPage(L);

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