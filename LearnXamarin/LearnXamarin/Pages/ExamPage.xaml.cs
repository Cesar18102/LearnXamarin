using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using LearnXamarin.Models;
using XLabs.Forms.Controls;

namespace LearnXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExamPage : ContentPage
    {
        private Lesson L { get; set; }
        private int ENum { get; set; }

        private Dictionary<Test, Dictionary<Variant, CustomRadioButton>> SingleAnswerTests = new Dictionary<Test, Dictionary<Variant, CustomRadioButton>>();
        private Dictionary<Test, Dictionary<Variant, CheckBox>> MultiAnswerTests = new Dictionary<Test, Dictionary<Variant, CheckBox>>();

        public ExamPage(Lesson L, int ENum)
        {
            InitializeComponent();

            this.L = L;
            this.ENum = ENum;
            ExamTitle.Text = L.title;
            this.Appearing += ExamPage_Appearing;
            this.FinishButton.Clicked += FinishButton_Clicked;
        }

        private void ExamPage_Appearing(object sender, EventArgs e)
        {
            int row = 0;
            foreach (LessonTask LTask in L)
            {
                if (!LTask.GetType().Equals(typeof(Test)))
                    continue;

                Test T = LTask as Test;
                if (T.RightVariantsCount == 1)
                {
                    Dictionary<Variant, CustomRadioButton> Selects = new Dictionary<Variant, CustomRadioButton>();

                    Label testTitle = new Label();
                    testTitle.Text = T.text;
                    TestGrid.Children.Add(testTitle, 0, row++);

                    foreach (Variant V in T)
                    {
                        CustomRadioButton RB = new CustomRadioButton();
                        Selects.Add(V, RB);
                        RB.Text = V.text;

                        RB.CheckedChanged = (context, args) =>
                        {
                            if(args.Value)
                                foreach (CustomRadioButton CRB in Selects.Values)
                                    if(!CRB.Equals(RB))
                                        CRB.Checked = false;
                        };

                        TestGrid.Children.Add(RB, 0, row++);
                    }

                    SingleAnswerTests.Add(T, Selects);
                }
                else
                {
                    Dictionary<Variant, CheckBox> Selects = new Dictionary<Variant, CheckBox>();

                    Label testTitle = new Label();
                    testTitle.Text = T.text;
                    TestGrid.Children.Add(testTitle, 0, row++);

                    foreach (Variant V in T)
                    {
                        CheckBox CB = new CheckBox();
                        Selects.Add(V, CB);
                        CB.DefaultText = V.text;
                        TestGrid.Children.Add(CB, 0, row++);
                    }

                    MultiAnswerTests.Add(T, Selects);
                }

                BoxView Splitter = new BoxView();
                Splitter.Style = (Style)(Resources["TestSplitter"]);
                TestGrid.Children.Add(Splitter, 0, row++);
            }
        }

        private void FinishButton_Clicked(object sender, EventArgs e)
        {
            foreach (LessonTask LT in L)
                App.CurrentUser.Pass(L, LT);

            int rightCount = 0;
            foreach (KeyValuePair<Test, Dictionary<Variant, CustomRadioButton>> singleAnswerTestAnswers in SingleAnswerTests)
                foreach (KeyValuePair<Variant, CustomRadioButton> answer in singleAnswerTestAnswers.Value)
                    if (answer.Key.is_right && answer.Value.Checked)
                    {
                        rightCount++;
                        break;
                    }
            foreach (KeyValuePair<Test, Dictionary<Variant, CheckBox>> multiAnswerTestAnswers in MultiAnswerTests)
            {
                bool isRightAnswer = true;
                foreach (KeyValuePair<Variant, CheckBox> answer in multiAnswerTestAnswers.Value)
                    isRightAnswer &= !(answer.Key.is_right ^ answer.Value.Checked);
                rightCount += isRightAnswer ? 1 : 0;
            }

            DisplayAlert("Результаты", "Правильных ответов: " + rightCount + " из " + (SingleAnswerTests.Count + MultiAnswerTests.Count), "ОК");
            App.Current.MainPage = new LessonListPage();
        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new LessonListPage();
            return true;
        }
    }
}