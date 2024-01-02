using ControlzEx.Theming;
using Microsoft.Win32;
using ShoDouEditor.Windows.Base;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace ShoDouEditor.Windows;


/// <summary>
/// Interaction logic for TextEditorWindow.xaml
/// </summary>
public partial class TextEditorWindow : BaseWindow
{
    #region Properties

    private string _fileName = string.Empty;
    /// <summary>
    /// The property holding the fileName
    /// </summary>
    public string fileName
    {
        get => _fileName;
        set
        {
            _fileName = value;
            NotifyPropertyChanged();
        }
    }

    /// <summary>
    /// Determines whether the searhc popup is showing
    /// </summary>
    public bool isShowingSearchPopup
    {
        get => searchPopup is not null ? searchPopup.isShowingSearchPopup : false;
    }

    /// <summary>
    /// Determines whether the searhc popup is showing
    /// </summary>
    public bool isShowingSearchReplacePopup
    {
        get => searchReplacePopup is not null ? searchReplacePopup.isShowingSearchReplacePopup : false;
    }

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor for TextEditorWindow
    /// </summary>
    public TextEditorWindow()
    {
        InitializeComponent();

        ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
        ThemeManager.Current.SyncTheme();

        searchPopup.SearchPopupEnterKeyDown += SearchPopup_EnterKeyDown;
        searchPopup.NextButtonDown          += NextMatch;
        searchPopup.PreviousButtonDown      += PreviousMatch;
    }

    #endregion

    #region Bindings

    private string _textEditorText = string.Empty;
    /// <summary>
    /// The property responsible for dealing with the text editors text
    /// </summary>
    public string textEditorText
    {
        get => _textEditorText;
        set
        {
            _textEditorText = value;
            NotifyPropertyChanged();
        }
    } 

    #endregion

    #region Menu Events

    #region File Section

    private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
    {
        OpenFile();
    }

    private void MenuItem_Save_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        SaveToFile();
    }

    private void MenuItem_SaveAs_Click(object sender, RoutedEventArgs e)
    {
        SaveAsFile();
    }

    private void MenuItem_Exit_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    #endregion

    #region Edit Section

    private void MenuItem_Undo_Click(object sender, RoutedEventArgs e)
    {
        TbMain.Undo();
    }

    private void MenuItem_Cut_Click(object sender, RoutedEventArgs e)
    {
        TbMain.Cut();
    }

    private void MenuItem_Copy_Click(object sender, RoutedEventArgs e)
    {
        TbMain.Copy();
    }

    private void MenuItem_Paste_Click(object sender, RoutedEventArgs e)
    {
        TbMain.Paste();
    }

    private void MenuItem_Delete_Click(object sender, RoutedEventArgs e)
    {
        TbMain.SelectedText = string.Empty;
    }

    private void MenuItem_Find_Click(object sender, RoutedEventArgs e)
    {
        ShowSearchPopup();
    }

    private void MenuItem_FindNext_Click(object sender, RoutedEventArgs e)
    {
        NextMatch();
    }

    private void MenuItem_FindPrevious_Click(object sender, RoutedEventArgs e)
    {
        PreviousMatch();
    }

    private void MenuItem_Replace_Click(object sender, RoutedEventArgs e)
    {
        ShowReplaceSearchPopup();
    }

    private void MenuItem_GoTo_Click(object sender, RoutedEventArgs e)
    {
        ShowSearchPopup();
    }

    private void MenuItem_SelectAll_Click(object sender, RoutedEventArgs e)
    {
        TbMain.SelectAll();
    }

    private void MenuItem_Font_Click(object sender, RoutedEventArgs e)
    {
        NotImplementedMessageBox();
    }

    #endregion

    #region View Section

    private void MenuItem_LightTheme_Click(object sender, RoutedEventArgs e)
    {
        ThemeManager.Current.ChangeTheme(Application.Current, "Light.Steel");
        ThemeManager.Current.ChangeTheme(searchPopup, "Light.Steel");
    }

    private void MenuItem_DarkTheme_Click(object sender, RoutedEventArgs e)
    {
        ThemeManager.Current.ChangeTheme(Application.Current, "Dark.Mauve");
        ThemeManager.Current.ChangeTheme(searchPopup, "Dark.Mauve");
    }

    #endregion

    #endregion

    #region Methods

    /// <summary>
    /// A method that displays a message saying that the requested feature is not implemented
    /// </summary>
    public void NotImplementedMessageBox()
    {
        MessageBox.Show(new NotImplementedException().Message, "Not implemented", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    /// <summary>
    /// A method that displays a message saying that the requested feature is in development
    /// </summary>
    public void InDevelopmentMessageBox()
    {
        MessageBox.Show("This feature is under development. This is unstable and may cause unforseen problems.", "Under development", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    public void searchTextEditor()
    {
        if (searchPopup.matches is null)
        {
            searchPopup.searchRegex = new Regex("(?i)" + searchPopup.searchString);
            searchPopup.matches = searchPopup.searchRegex.Matches(textEditorText); 
        }

        if (searchPopup.matches.Count > 0)
        {
            searchPopup.start = searchPopup.matches[searchPopup.currentMatch].Index;

            TbMain.Select(searchPopup.start, searchPopup.searchString.Length);
            TbMain.Focus();
        }
        else
        {
            MessageBox.Show("Item not found", "Not found", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    #region File

    public void OpenFile()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Text Files Only | *.txt";

        if (openFileDialog.ShowDialog().HasValue)
        {
            fileName = openFileDialog.FileName;
        }

        if (string.IsNullOrWhiteSpace(fileName) == false)
        {
            textEditorText = File.ReadAllText(fileName);
        }
    }

    public void SaveToFile()
    {
        if (string.IsNullOrWhiteSpace(fileName) == false)
        {
            File.WriteAllText(fileName, textEditorText);
        }
    }

    public void SaveAsFile()
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Text Files Only | *.txt";

        if (saveFileDialog.ShowDialog().HasValue)
        {
            fileName = saveFileDialog.FileName;
        }

        if (string.IsNullOrWhiteSpace(fileName) == false)
        {
            File.WriteAllText(fileName, textEditorText);
        }
    }

    #endregion

    #region Edit

    public void ShowSearchPopup()
    {
        searchPopup.isShowingSearchPopup = true;

        if (searchReplacePopup.isShowingSearchReplacePopup) {
            searchReplacePopup.isShowingSearchReplacePopup = false;
        }
    }

    public void ShowReplaceSearchPopup()
    {
        searchReplacePopup.isShowingSearchReplacePopup = true;

        if (searchPopup.isShowingSearchPopup)
        {
            searchPopup.isShowingSearchPopup = false;
        }
    }

    #endregion

    private void NextMatch()
    {
        searchPopup.searchRegex = new Regex("(?i)" + searchPopup.searchString);
        searchPopup.matches = searchPopup.searchRegex.Matches(textEditorText);

        if (searchPopup.matches is not null)
        {
            if (searchPopup.currentMatch + 1 >= searchPopup.matches.Count)
            {
                searchPopup.currentMatch = 0;
                searchTextEditor();
            }
            else if ((searchPopup.matches.Count > 0) && (searchPopup.currentMatch + 1 < searchPopup.matches.Count))
            {
                searchPopup.currentMatch++;
                searchTextEditor();
            }
            else
            {
                MessageBox.Show("Item not found", "Not found", MessageBoxButton.OK, MessageBoxImage.Information);
            } 
        }
        else 
        {
            searchTextEditor();
        }
    }

    private void PreviousMatch()
    {
        searchPopup.searchRegex = new Regex("(?i)" + searchPopup.searchString);
        searchPopup.matches = searchPopup.searchRegex.Matches(textEditorText);

        if (searchPopup.matches is not null)
        {
            if (searchPopup.currentMatch <= 0)
            {
                searchPopup.currentMatch = searchPopup.matches.Count - 1;
                searchTextEditor();
            }
            else if ((searchPopup.matches.Count > 0) && (searchPopup.currentMatch > 0))
            {
                searchPopup.currentMatch--;
                searchTextEditor();
            }
            else
            {
                MessageBox.Show("Item not found", "Not found", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        else
        {
            searchTextEditor();
        }
    }

    #endregion

    #region Window Events

    protected override void OnActivated(EventArgs e)
    {
        base.OnActivated(e);

        TbMain.Focus();
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);

        Application.Current.Shutdown();
    }

    private void thisWindow_KeyDown(object sender, KeyEventArgs e)
    {
        TbMain.IsEnabled = false;

        #region File

        if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.O)
        {
            OpenFile();
        }

        if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
        {
            SaveToFile();
        }

        if (Keyboard.Modifiers == ModifierKeys.Control && Keyboard.Modifiers == ModifierKeys.Shift && e.Key == Key.S)
        {
            SaveAsFile();
        }

        #endregion

        #region Edit

        if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.F)
        {
            ShowSearchPopup();
        }

        TbMain.IsEnabled = true; 

        #endregion
    }

    #endregion

    #region Control Events

    private void SearchPopup_EnterKeyDown()
    {
        searchTextEditor();
    }

    #endregion
}
