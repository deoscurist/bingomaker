using System.Threading.Tasks;

namespace BingoMaker.Services;

public interface IDialogService
{
    Task<bool> ShowConfirmAsync(string message);
}