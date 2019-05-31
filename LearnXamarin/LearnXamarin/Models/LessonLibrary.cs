﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LearnXamarin.DB;

namespace LearnXamarin.Models
{
    public class LessonLibrary : IEnumerable
    {
        private bool Loaded = false;
        private static List<Lesson> Lessons = new List<Lesson>();
        private static List<Theory> Theories = new List<Theory>();
        private static List<Test> Tests = new List<Test>();
        private static List<Variant> Variants = new List<Variant>();

        public async void Load()
        {
            if (Loaded)
                return;

            Loaded = true;

            Variants = await DbContext.SelectAsync<Variant>();
            Tests = await DbContext.SelectAsync<Test>();

            foreach (Test T in Tests)
                T.AddVariants(Variants.Where(V => V.test_id == T.id));

            Theories = await DbContext.SelectAsync<Theory>();
            Lessons = await DbContext.SelectAsync<Lesson>();

            foreach (Lesson L in Lessons)
            {
                L.AddTasks(Theories.Where(T => T.lesson_id == L.id));
                L.AddTasks(Tests.Where(T => T.lesson_id == L.id));
            }
        }

        public IEnumerator GetEnumerator()
        {
            foreach (Lesson L in Lessons)
                yield return L;
        }
    }
}