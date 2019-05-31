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
    public partial class LessonPage : ContentPage
    {
        private Lesson L { get; set; }

        public LessonPage(Lesson L)
        {
            InitializeComponent();

            this.L = L;
            LessonTitle.Text = L.title;
            this.Appearing += LessonPage_Appearing;
        }

        private void LessonPage_Appearing(object sender, EventArgs e)
        {
            int Row = 0;
            int Column = 0;
            int CounterTests = 0;
            int CounterTheories = 0;

            foreach (LessonTask LT in L)
            {
                BoxView TaskProgressIndicator = new BoxView();
                bool Passed = App.CurrentUser.IsTaskPassed(L, LT);
                TaskProgressIndicator.Style = (Style)(Resources[Passed ? "PassedTaskIndicator" : "UnpassedTaskIndicator"]);

                LessonProgressIndicator.Children.Add(TaskProgressIndicator, Column, Row);

                if (++Column == 10)
                {
                    Row++;
                    Column = 0;
                }

                Button TaskButton = new Button();
                TaskButton.Style = (Style)(Resources[Passed ? "PassedTask" : "Task"]);

                if (LT is Test)
                {
                    TaskButton.Text = $"Practice #{++CounterTests}";
                    TaskButton.Clicked += (context, args) => App.Current.MainPage = new TestPage((Test)LT, L);
                }
                else
                {
                    TaskButton.Text = $"Theory #{++CounterTheories}";
                    TaskButton.Clicked += (context, args) => App.Current.MainPage = new TheoryPage((Theory)LT, L);
                }

                TasksList.Children.Add(TaskButton);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new LessonListPage();
            return true;
        }
    }
}