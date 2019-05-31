﻿using System;
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
    public partial class TheoryPage : ContentPage
    {
        private Theory T { get; set; }
        private Lesson L { get; set; }

        public TheoryPage(Theory T, Lesson L)
        {
            InitializeComponent();
            this.T = T;
            this.L = L;
            this.Appearing += TheoryPage_Appearing;
        }

        private void TheoryPage_Appearing(object sender, EventArgs e)
        {
            App.CurrentUser.Pass(L, T);
        }

        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new LessonPage(L);
            return true;
        }
    }
}