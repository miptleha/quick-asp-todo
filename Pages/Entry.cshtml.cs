using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace quick_asp_todo.Pages
{
    public class EntryModel : PageModel
    {
        public void OnGet(int? id)
        {
            Id = id;
            if (id != null)
            {
                Entry = ToDoEntry.ReadOne(id.Value);
            }
            else
            {
                Entry = new ToDoEntry();
                Entry.Items.Add(new ToDoItem(null));
            }
        }

        public ToDoEntry Entry { get; private set; }

        public int? Id { get; private set; }

        public IActionResult OnPostSubmit()
        {
            var id = Request.Form["id"][0];

            if (!string.IsNullOrEmpty(id))
            {
                Entry = ToDoEntry.ReadOne(int.Parse(id));
            }
            else
            {
                Entry = new ToDoEntry();
                ToDoEntry.ReadAll().Add(Entry);
            }

            int max = 0;
            foreach (var k in Request.Form.Keys)
            {
                if (k.ToLower().StartsWith("text-"))
                {
                    int n;
                    if (int.TryParse(k.Substring(5), out n) && n > max)
                        max = n;
                }
            }

            Entry.Title = Request.Form["title"];
            int i1 = 0;
            for (int i = 0; i <= max; i++)
            {
                if (!Request.Form.ContainsKey($"text-{i}") || string.IsNullOrWhiteSpace(Request.Form[$"text-{i}"]))
                    continue;

                var text = Request.Form[$"text-{i}"];
                bool done = false;
                if (Request.Form.ContainsKey($"done-{i}"))
                    done = true;
                
                if (Entry.Items.Count > i1)
                {
                    Entry.Items[i1].Text = text;
                    Entry.Items[i1].Done = done;
                }
                else
                {
                    Entry.Items.Add(new ToDoItem(text, done));
                }

                i1++;
            }

            while (Entry.Items.Count > max + 1)
                Entry.Items.RemoveAt(Entry.Items.Count - 1);

            ToDoEntry.Save();
            
            return Redirect("~/");
        }

        public IActionResult OnPostCheck(int id1, int id2, bool check)
        {
            var ToDo = ToDoEntry.ReadAll();
            if (id1 < ToDo.Count && id2 < ToDo[id1].Items.Count)
            {
                ToDo[id1].Items[id2].Done = check;
                ToDoEntry.Save();
            }

            return new EmptyResult();
        }

        public IActionResult OnPostDelete()
        {
            var id = Request.Form["id"][0];

            int n;
            if (!string.IsNullOrEmpty(id) && int.TryParse(id, out n))
            {
                var list = ToDoEntry.ReadAll();
                if (list.Count > n)
                    list.RemoveAt(n);
                ToDoEntry.Save();
            }

            return Redirect("~/");
        }
    }
}
