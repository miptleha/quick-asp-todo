using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace quick_asp_todo
{
    public class ToDoEntry
    {
        public string Title { get; set; }
        public List<ToDoItem> Items { get; set; }

        public static void Save()
        {
            if (_res != null)
            {
                var opt = new JsonSerializerOptions() { WriteIndented = true };
                var str = JsonSerializer.Serialize(_res, opt);
                File.WriteAllText(ToDoPath, str);

            }
        }

        public static void Load()
        {
            try
            {
                var str = File.ReadAllText(ToDoPath);
                //var obj = JsonSerializer.Deserialize(str);
                _res = JsonSerializer.Deserialize<List<ToDoEntry>>(str);
            }
            catch (FileNotFoundException)
            {
                _res = new List<ToDoEntry>();
            }
        }

        static string _path;
        static string ToDoPath
        {
            get
            {
                if (_path == null)
                {
                    var path = Assembly.GetExecutingAssembly().Location;
                    var dir = Path.GetDirectoryName(path);
                    _path = Path.Combine(dir, "todo.json");
                }

                return _path;
            }
        }

        internal static ToDoEntry ReadOne(int id)
        {
            if (id >= _res.Count)
                return null;
            return _res[id];
        }

        static List<ToDoEntry> _res;

        public ToDoEntry(string title = null)
        {
            Title = title;
            Items = new List<ToDoItem>();
        }

        internal static List<ToDoEntry> ReadAll()
        {
            if (_res == null)
                Load();

            return _res;
        }
    }

    public class ToDoItem
    {
        public ToDoItem(string text, bool done = false)
        {
            Text = text;
            Done = done;
        }

        public string Text { get; set; }
        public bool Done { get; set; }
    }
}
