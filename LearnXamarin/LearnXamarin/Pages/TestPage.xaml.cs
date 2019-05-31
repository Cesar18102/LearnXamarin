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
    public partial class TestPage : ContentPage
    {
        private Test T { get; set; }
        private Lesson L { get; set; }

        public TestPage(Test T, Lesson L)
        {
            InitializeComponent();
            this.T = T;
            this.L = L;
        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new LessonPage(L);
            return true;
        }
    }
}