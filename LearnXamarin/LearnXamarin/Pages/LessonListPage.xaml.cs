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
            foreach (Lesson L in App.Lessons)
                DisplayAlert($"Lesson #{L.id}", L.title, "OK");
        }
    }
}