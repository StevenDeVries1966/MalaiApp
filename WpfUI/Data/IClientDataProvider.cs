using DataLayer.Classes;
using System.Threading.Tasks;

namespace WpfUI.Data
{
    public interface IMalaiDataProvider
    {
        Task<MalaiContext?> GetDataContextAsync();
    }

    public class MalaiDataProvider : IMalaiDataProvider
    {
        public async Task<MalaiContext?> GetDataContextAsync()
        {
            await Task.Delay(1);
            MalaiContext ctx = new MalaiContext("127.0.0.1", @"Malai_test", "root", "wqEQW5Ag/&6%JT+");
            return ctx;
        }
    }
}
