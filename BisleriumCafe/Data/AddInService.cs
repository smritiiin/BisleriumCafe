using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BisleriumCafe.Data
{
    public class AddInService
    {
        public static void SaveAllAddIn(List<AddIn> addIns)
        {
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string appAddInsFilePath = Utils.GetAppAddInFilePath();
            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            var json = JsonSerializer.Serialize(addIns);
            File.WriteAllText(appAddInsFilePath, json);
        }

        public static List<AddIn> AddAddIn(string AddInName, int AddInPrice)
        {
            List<AddIn> addIns = GetAllAddIn();
            addIns.Add(
                new AddIn
                {
                    ID = addIns.Count() + 1,
                    AddInName = AddInName,
                    AddInPrice = AddInPrice,
                });
            SaveAllAddIn(addIns);
            return addIns;
        }


        public static List<AddIn> GetAllAddIn()
        {
            string addInFilePath = Utils.GetAppAddInFilePath();
            if (!File.Exists(addInFilePath))
            {
                return new List<AddIn>();
            }

            var json = File.ReadAllText(addInFilePath);

            return JsonSerializer.Deserialize<List<AddIn>>(json);

        }

        public static List<AddIn> UpdateAddIn(int ID, string newAddInName, int newAddInPrice)
        {
            List<AddIn> addIns = GetAllAddIn();

            AddIn addInToUpdate = addIns.Find(x => x.ID == ID);
            addIns.Remove(addInToUpdate);

            addIns.Add(
                new AddIn
                {
                    ID = ID,
                    AddInName = newAddInName,
                    AddInPrice = newAddInPrice,
                });
            SaveAllAddIn(addIns);
            return addIns;

        }
        public static List<AddIn> DeleteAddIn(int ID)
        {
            List<AddIn> addIns = GetAllAddIn();
            AddIn addInToDelete = addIns.Find(x => x.ID == ID);

            if (addInToDelete == null)
                throw new Exception("CoffAddIn not found");
            addIns.Remove(addInToDelete);
            SaveAllAddIn(addIns);
            return addIns;
        }
    }

}
