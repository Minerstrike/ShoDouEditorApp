using ShoDouEditor.Windows.User_Controls.Base;
using System.DirectoryServices.ActiveDirectory;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace ShoDouEditor.Windows.User_Controls;

/// <summary>
/// Interaction logic for SearchPopup.xaml
/// </summary>
public partial class SearchPopup : BaseUserControl
{
    #region Properties

    private bool _isShowingSearchPopup;
    /// <summary>
    /// Determines whether the searhc popup is showing
    /// </summary>
    public bool isShowingSearchPopup
    {
        get => _isShowingSearchPopup;
        set
        {
            _isShowingSearchPopup = value;
            NotifyPropertyChanged();

            if (_isShowingSearchPopup)
            {
                searchPopupWrapper.Visibility = Visibility.Visible;
            }
            else
            {
                searchPopupWrapper.Visibility = Visibility.Collapsed;
            }
        }
    }

    private string _searchString = string.Empty;
    /// <summary>
    /// The property responsible for dealing with the search popups text
    /// </summary>
    public string searchString
    {
        get => _searchString;
        set
        {
            _searchString = value;
            NotifyPropertyChanged();
        }
    }

    private string _targetText;
    /// <summary>
    /// The text that much be searched
    /// </summary>
    public string targetText
    {
        get => _targetText;
        set 
        { 
            _targetText = value;
            NotifyPropertyChanged();
        }
    }


    private Regex _searchRegex;
    /// <summary>
    /// Responsible for searching for the searchString
    /// </summary>
    public Regex searchRegex
    {
        get => _searchRegex;
        set 
        { 
            _searchRegex = value;
            NotifyPropertyChanged();
        }
    }

    private int _start;
    /// <summary>
    /// Where in the target text the search string was found
    /// </summary>
    public int start
    {
        get => _start;
        set 
        {
            _start = value;
            NotifyPropertyChanged();
        }
    }

    #endregion

    #region Constructor

    public SearchPopup()
    {
        InitializeComponent();

        searchRegex = new Regex(string.Empty);
        targetText = string.Empty;
    }

    #endregion

    #region Button Events

    private void SearchPopupCloseButton_Click(object sender, RoutedEventArgs e)
    {
        isShowingSearchPopup = false;
    }

    #endregion

    #region Control Events

    private void SearchPopup_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            searchRegex = new Regex("(?i)" + searchString);
            Match match = searchRegex.Match(targetText);

            if (match.Success)
            {
                start = match.Index;
            }
            else
            {
                MessageBox.Show("Item not found", "Not found", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

    #endregion
}
