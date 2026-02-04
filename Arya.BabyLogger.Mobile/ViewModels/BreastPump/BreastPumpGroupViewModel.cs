using System.Collections.ObjectModel;

namespace Arya.BabyLogger.Mobile.ViewModels.BreastPump;

public class BreastPumpGroupViewModel : ObservableCollection<BreastPumpItemViewModel>
{
    private string _groupTitle = string.Empty;
    public string GroupTitle
    {
        get => _groupTitle;
        set
        {
            _groupTitle = value;
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(GroupTitle)));
        }
    }

    public double TotalAmountInMl => this.Sum(i => i.AmountInMl);

    public BreastPumpGroupViewModel(string groupTitle, BreastPumpItemViewModel[] items) : base(items)
    {
        GroupTitle = groupTitle;
    }

    protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        base.OnCollectionChanged(e);
        OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(TotalAmountInMl)));
    }
}