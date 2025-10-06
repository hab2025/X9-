using System.Threading.Tasks;

namespace HabCo.X9.App;

public interface IDialogService
{
    Task<TResult?> ShowDialogAsync<TResult>(object viewModel) where TResult : class;
}