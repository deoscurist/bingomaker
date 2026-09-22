using System.Threading.Tasks;
using BingoMaker.Services;

namespace BingoMaker.Design.Services;

public class DesignDialogService : IDialogService
{
    public Task<bool> ShowConfirmAsync(string message) => Task.FromResult(true);
}