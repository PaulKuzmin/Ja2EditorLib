using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Ja2StracSaveEditor;

public partial class SettingsForm : Form
{
    public SettingsModel Settings;

    public SettingsForm()
    {
        InitializeComponent();
    }

    public SettingsForm(SettingsModel settings)
    {
        Settings = settings;
        InitializeComponent();
    }

    private void button2_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (!Directory.Exists(Settings.Ja2DataPath) || !Directory.Exists(Settings.Ja2StracPath))
        {
            MessageBox.Show(@"Selected directories do not exist");
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        if (DialogResult != DialogResult.OK)
            DialogResult = DialogResult.Cancel;
    }

    private void ChooseJa2StracBtn_Click(object sender, EventArgs e)
    {
        Settings.Ja2StracPath = ChooseDirectory();
    }

    private void ChooseJa2DataBtn_Click(object sender, EventArgs e)
    {
        Settings.Ja2DataPath = ChooseDirectory();
    }

    private string ChooseDirectory()
    {
        using var dialog = new FolderBrowserDialog();
        dialog.ShowNewFolderButton = false;
        return dialog.ShowDialog() != DialogResult.OK ? null : dialog.SelectedPath;
    }

    private void SettingsForm_Load(object sender, EventArgs e)
    {
        label1.Text = typeof(SettingsModel).GetProperty(nameof(SettingsModel.Ja2StracPath))?
            .GetCustomAttribute<DisplayAttribute>()?.Name ?? nameof(SettingsModel.Ja2StracPath);
        label2.Text = typeof(SettingsModel).GetProperty(nameof(SettingsModel.Ja2DataPath))?
            .GetCustomAttribute<DisplayAttribute>()?.Name ?? nameof(SettingsModel.Ja2DataPath);

        Ja2DataEdit.DataBindings.Add(nameof(Ja2DataEdit.Text), Settings, nameof(Settings.Ja2DataPath), true,
            DataSourceUpdateMode.OnPropertyChanged);
        Ja2StracEdit.DataBindings.Add(nameof(Ja2StracEdit.Text), Settings, nameof(Settings.Ja2StracPath), true,
            DataSourceUpdateMode.OnPropertyChanged);
    }
}