using ShoDouEditor.Windows.User_Controls.Base;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Xps;

namespace ShoDouEditor.Windows.User_Controls;

/// <summary>
/// Interaction logic for SearchPopup.xaml
/// </summary>
public partial class SearchPopup : BaseUserControl
{
    #region Properties

    private bool _isShowingSearchPopup = false;
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
                textBoxMain.Focus();
            }
            else
            {
                searchPopupWrapper.Visibility = Visibility.Collapsed;
                searchString = string.Empty;
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

    //private string _targetText;
    ///// <summary>
    ///// The text that much be searched
    ///// </summary>
    //public string targetText
    //{
    //    get => _targetText;
    //    set 
    //    { 
    //        _targetText = value;
    //        NotifyPropertyChanged();
    //    }
    //}

    private Regex? _searchRegex;
    /// <summary>
    /// Responsible for searching for the searchString
    /// </summary>
    public Regex? searchRegex
    {
        get => _searchRegex;
        set
        {
            _searchRegex = value;
            NotifyPropertyChanged();
        }
    }

    private MatchCollection? _matches;
    /// <summary>
    /// Responsible for the list of matches against the targetText
    /// </summary>
    public MatchCollection? matches
    {
        get => _matches;
        set
        {
            _matches = value;
            NotifyPropertyChanged();
        }
    }

    private int _currentMatch = 0;
    /// <summary>
    /// Keeps track of the current match in the list of matches
    /// </summary>
    public int currentMatch
    {
        get => _currentMatch;
        set 
        { 
            _currentMatch = value;
            NotifyPropertyChanged();
        }
    }


    private int _start = 0;
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
    }

    #endregion

    #region Button Events

    private void SearchPopupCloseButton_Click(object sender, RoutedEventArgs e)
    {
        isShowingSearchPopup = false;
    }

    private void Previous_Button_Clicked(object sender, RoutedEventArgs e)
    {
        OnPreviousButtonDown();
    }

    private void Next_Button_Clicked(object sender, RoutedEventArgs e)
    {
        OnNextButtonDown();
    }

    #endregion

    #region Control Events

    private void SearchPopup_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            OnSearchPopupEnterKeyIsDown();

            

            OnSearchPopupEnterKeyDown();
        }
    }

    #endregion

    #region Window Events

    private void SearchPopupUserControl_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            isShowingSearchPopup = false;
        }
    } 

    #endregion

    #region Custom Events

    public delegate void SearchPopupEnterKeyDownEventHandler();

    public event SearchPopupEnterKeyDownEventHandler? SearchPopupEnterKeyIsDown;
    protected virtual void OnSearchPopupEnterKeyIsDown()
    {
        if (SearchPopupEnterKeyIsDown is not null)
        {
            SearchPopupEnterKeyIsDown();
        }
    }

    public event SearchPopupEnterKeyDownEventHandler? SearchPopupEnterKeyDown;
    protected virtual void OnSearchPopupEnterKeyDown()
    {
        if (SearchPopupEnterKeyDown is not null)
        {
            SearchPopupEnterKeyDown();
        }
    }


    public delegate void ButtonEventHandler();

    public event ButtonEventHandler? NextButtonDown;
    protected virtual void OnNextButtonDown()
    {
        if (NextButtonDown is not null)
        {
            NextButtonDown();
        }
    }

    public event ButtonEventHandler? PreviousButtonDown;
    protected void OnPreviousButtonDown()
    {
        if (PreviousButtonDown is not null)
        {
            PreviousButtonDown();
        }
    }

    #endregion
}
