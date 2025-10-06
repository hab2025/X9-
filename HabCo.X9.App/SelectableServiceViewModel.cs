using CommunityToolkit.Mvvm.ComponentModel;
using HabCo.X9.Core;

namespace HabCo.X9.App;

public partial class SelectableServiceViewModel : ObservableObject
{
    public Service Service { get; }

    [ObservableProperty]
    private bool _isSelected;

    public SelectableServiceViewModel(Service service)
    {
        Service = service;
    }
}