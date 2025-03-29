using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MemoryWPF
{
    public class StatisticsViewModel
    {
        public ObservableCollection<UserModel> Users { get; set; }
        public StatisticsViewModel() 
        {
            string usersPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Users = new ObservableCollection<UserModel>();
            if (Directory.Exists(usersPath))
            {
                List<string> userFiles = Directory.GetFiles(usersPath, "*User.json").ToList();
                foreach (string file in userFiles)
                {
                    string jsonContent = File.ReadAllText(file);
                    try
                    {
                        UserModel temp = JsonSerializer.Deserialize<UserModel>(jsonContent);
                        Users.Add(temp);
                    }
                    catch { Console.WriteLine("Error deserializing: " + file); }

                }
            }
        }
    }
}
