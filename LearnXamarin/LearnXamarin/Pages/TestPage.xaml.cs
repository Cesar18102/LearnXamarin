using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XLabs.Forms;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LearnXamarin.Models;
using XLabs.Forms.Controls;
using System.Collections.ObjectModel;
using LearnXamarin.DB;

namespace LearnXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : ContentPage
    {
        private Test T { get; set; }
        private Lesson L { get; set; }
        private int TNum { get; set; }
        private int LNum { get; set; }

        public TestPage(Test T, Lesson L, int TNum, int LNum)
        {
            InitializeComponent();
            this.T = T;
            this.L = L;
            this.TNum = TNum;
            this.LNum = LNum;
            Title.Text = $"Урок #{LNum} Тест #{TNum}";
            TestText.Text = T.text;
            this.Appearing += TestPage_Appearing;
        }

        private void TestPage_Appearing(object sender, EventArgs e)
        {
            if (T.RightVariantsCount == 1)
            {
                Dictionary<Variant, CustomRadioButton> Selects = LoadRB();
                Check.Clicked += (context, args) => {

                    if (!Selects.Aggregate(true, (A, S) => A && (S.Key.is_right == S.Value.Checked)))
                        DisplayAlert("Неверно...", "Вы допустили ошибку", "Еще раз");
                    else
                    {
                        App.CurrentUser.Pass(L, T);
                        App.Current.MainPage = new LessonPage(L, LNum);
                    }
                };
            }
            else
            {
                Dictionary<Variant, CheckBox> Selects = LoadCB();
                Check.Clicked += (context, args) => {

                    if (!Selects.Aggregate(true, (A, S) => A && (S.Key.is_right == S.Value.Checked)))
                        DisplayAlert("Неверно...", "Вы допустили ошибку", "Еще раз");
                    else
                    {
                        App.CurrentUser.Pass(L, T);
                        App.Current.MainPage = new LessonPage(L, LNum);
                    }
                };
            }
        }

        private Dictionary<Variant, CustomRadioButton> LoadRB()
        {
            Dictionary<Variant, CustomRadioButton> VTable = new Dictionary<Variant, CustomRadioButton>();

            foreach (Variant V in T)
            {
                CustomRadioButton Var = new CustomRadioButton();
                Var.Text = V.text;
                Var.Checked = false;
                Var.Style = (Style)Resources["TestRB"];
                VTable.Add(V, Var);
                Var.CheckedChanged = new EventHandler<XLabs.EventArgs<bool>>((sender, e) =>
                {
                    if (!e.Value) return;

                    foreach (CustomRadioButton CRB in VTable.Values)
                        if(!CRB.Equals(sender))
                            CRB.Checked = false;
                });
                Tests.Children.Add(Var);
            }

            return VTable;
        }

        private Dictionary<Variant, CheckBox> LoadCB()
        {
            Dictionary<Variant, CheckBox> VTable = new Dictionary<Variant, CheckBox>();

            foreach (Variant V in T)
            {
                CheckBox Var = new CheckBox();
                Var.DefaultText = V.text;
                Var.Checked = false;
                Var.Style = (Style)Resources["TestCB"];
                VTable.Add(V, Var);
                Tests.Children.Add(Var);
            }

            return VTable;
        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new LessonPage(L, LNum);
            return true;
        }
    }
}