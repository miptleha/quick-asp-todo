using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quick_asp_todo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<ToDoEntry> ToDo { get; private set; }

        public IEnumerable<string> Categories { get; private set; }
        public string Category { get; private set; }

        public void OnGet(string category)
        {
            ToDo = ToDoEntry.ReadAll();
            Categories = ToDo.Select(i => i.Category).Distinct().Where(i => !string.IsNullOrWhiteSpace(i));
            
            if (!string.IsNullOrWhiteSpace(category))
            {
                Category = category;
                var todo = new List<ToDoEntry>();
                foreach (var i in ToDo)
                {
                    if (i.Category == category)
                        todo.Add(i);
                }
                ToDo = todo;
            }
        }
    }
}
