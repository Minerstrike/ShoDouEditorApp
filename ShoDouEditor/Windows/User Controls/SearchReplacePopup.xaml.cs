using ShoDouEditor.Windows.User_Controls.Base;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ShoDouEditor.Windows.User_Controls;


/// <summary>
/// Interaction logic for SearchReplacePopup.xaml
/// </summary>
public partial class SearchReplacePopup : BaseUserControl
{
    #region Properties

    private bool _isShowingSearchReplacePopup = false;
    /// <summary>
    /// Determines whether the searhc popup is showing
    /// </summary>
    public bool isShowingSearchReplacePopup
    {
        get => _isShowingSearchReplacePopup;
        set
        {
            _isShowingSearchReplacePopup = value;
            NotifyPropertyChanged();

            if (_isShowingSearchReplacePopup)
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

    private string _replaceString = string.Empty;
    /// <summary>
    /// The property responsible for dealing with the text used to replace
    /// </summary>
    public string replaceString
    {
        get => _replaceString;
        set
        {
            _replaceString = value;
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

    public SearchReplacePopup()
    {
        InitializeComponent();

        textBoxMain.Foreground = Brushes.Gray;
        textBoxMain.Text = "Find";
        textBoxMain.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(textBox_GotKeyboardFocus);
        textBoxMain.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(textBoxMain_LostKeyboardFocus);

        textBoxReplace.Foreground = Brushes.Gray;
        textBoxReplace.Text = "Replace";
        textBoxReplace.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(textBox_GotKeyboardFocus);
        textBoxReplace.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(textBoxReplace_LostKeyboardFocus);
    }

    #endregion

    #region Button Events

    private void SearchPopupCloseButton_Click(object sender, RoutedEventArgs e)
    {
        isShowingSearchReplacePopup = false;
    }

    private void Previous_Button_Clicked(object sender, RoutedEventArgs e)
    {
        OnPreviousButtonDown();
    }

    private void Next_Button_Clicked(object sender, RoutedEventArgs e)
    {
        OnNextButtonDown();
    }

    private void Replace_Button_Clicked(object sender, RoutedEventArgs e)
    {
        
    }

    private void ReplaceAll_Button_Clicked(object sender, RoutedEventArgs e)
    {
        
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
            isShowingSearchReplacePopup = false;
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
    protected virtual void OnPreviousButtonDown()
    {
        if (PreviousButtonDown is not null)
        {
            PreviousButtonDown();
        }
    }

    private void textBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (sender is TextBox)
        {
            //If nothing has been entered yet.
            if (((TextBox)sender).Foreground == Brushes.Gray)
            {
                ((TextBox)sender).Text = "";
                ((TextBox)sender).SetResourceReference(Control.ForegroundProperty, "MahApps.Brushes.ThemeForeground");
            }
        }
    }

    private void textBoxMain_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        //Make sure sender is the correct Control.
        if (sender is TextBox)
        {
            //If nothing was entered, reset default text.
            if (((TextBox)sender).Text.Trim().Equals(""))
            {
                ((TextBox)sender).Foreground = Brushes.Gray;
                ((TextBox)sender).Text = "Find";
            }
        }
    }

    private void textBoxReplace_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        //Make sure sender is the correct Control.
        if (sender is TextBox)
        {
            //If nothing was entered, reset default text.
            if (((TextBox)sender).Text.Trim().Equals(""))
            {
                ((TextBox)sender).Foreground = Brushes.Gray;
                ((TextBox)sender).Text = "Replace";
            }
        }
    }

    #endregion
}
