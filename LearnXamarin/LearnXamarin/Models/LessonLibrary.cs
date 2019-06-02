using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LearnXamarin.DB;

namespace LearnXamarin.Models
{
    public class LessonLibrary : IEnumerable<Lesson>
    {
        public Task Loading { get; private set; }

        private static bool Loaded = false;
        private List<Lesson> Lessons = new List<Lesson>();
        private List<Theory> Theories = new List<Theory>();
        private List<Test> Tests = new List<Test>();
        private List<Variant> Variants = new List<Variant>();

        public void Load()
        {
            if (Loaded)
                return;

            Loaded = true;
            Loading = LoadAsync();
        }

        private async Task LoadAsync()
        {
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

        public IEnumerator<Lesson> GetEnumerator() => new Constants.Enumerator<Lesson>(Lessons);
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
