using ControlzEx.Theming;
using Microsoft.Win32;
using ShoDouEditor.Windows.Base;
using System.IO;
using System.Windows;

namespace ShoDouEditor.Windows;


/// <summary>
/// Interaction logic for TextEditorWindow.xaml
/// </summary>
public partial class TextEditorWindow : BaseWindow
{
    #region Properties

    private string _fileName = string.Empty;
    public string fileName
    {
        get => _fileName;
        set 
        { 
            _fileName = value;
            NotifyPropertyChanged();
        }
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

    #region Constructor

    /// <summary>
    /// Constructor for TextEditorWindow
    /// </summary>
    public TextEditorWindow()
    {
        InitializeComponent();
    }

    #endregion

    #region Menu Events

        #region File Section

    private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
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

    private void MenuItem_Save_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(fileName) == false)
        {
            File.WriteAllText(fileName, textEditorText);
        }
    }

    private void MenuItem_SaveAs_Click(object sender, RoutedEventArgs e)
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
        NotImplementedMessageBox();
    }

    private void MenuItem_FindNext_Click(object sender, RoutedEventArgs e)
    {
        NotImplementedMessageBox();
    }

    private void MenuItem_FindPrevious_Click(object sender, RoutedEventArgs e)
    {
        NotImplementedMessageBox();
    }

    private void MenuItem_Replace_Click(object sender, RoutedEventArgs e)
    {
        NotImplementedMessageBox();
    }

    private void MenuItem_GoTo_Click(object sender, RoutedEventArgs e)
    {
        NotImplementedMessageBox();
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
        ThemeManager.Current.ChangeTheme(this, "Light.Steel");
    }

    private void MenuItem_DarkTheme_Click(object sender, RoutedEventArgs e)
    {
        ThemeManager.Current.ChangeTheme(this, "Dark.Mauve");
    }

    #endregion

    #endregion

    #region Methods

    /// <summary>
    /// A method that display a message saying that the requested feature is not implemented
    /// </summary>
    public void NotImplementedMessageBox()
    {
        MessageBox.Show(new NotImplementedException().Message, "Not implemented", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public void InDevelopmentMessageBox()
    {
        MessageBox.Show("This feature is under development. This is unstable and may cause unforseen problems.", "Under development", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    #endregion

    #region Window Events

    protected override void OnActivated(EventArgs e)
    {
        base.OnActivated(e);

        TbMain.Focus();
    }

    #endregion
}
