using GenenticAlgorithmBlazor.Server.Models;
namespace GenenticAlgorithmBlazor.Server.Controllers
{
    public class DBConnection
    {
        private DataContext _dataContext;
        public DBConnection()
        {
            //context = new Context();
        }
        public  bool AddText(string textToSet)
        {
            _dataContext.StoredTexts.Add(new StoredText(){text= textToSet});
            return true;
        }

        public  string GetText(int id = -1)
        {
            return "";
        }

    }
}