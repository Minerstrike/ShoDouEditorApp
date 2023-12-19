using MahApps.Metro.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShoDouEditor.Windows.Base;

/// <summary>
/// A window that serves as the base for other windows.
/// Inherits from MetroWindow(mahApps) and INotifyPropertyChanged
/// </summary>
public class BaseWindow : MetroWindow, INotifyPropertyChanged
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
