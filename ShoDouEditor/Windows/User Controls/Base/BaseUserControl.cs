using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace ShoDouEditor.Windows.User_Controls.Base;

public class BaseUserControl : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Notifies of PropertyChanged, gets passed an optional string, otherwise the name of the caller member is used
    /// </summary>
    /// <param name="propertyName"></param>
    public void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
