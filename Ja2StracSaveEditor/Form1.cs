using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Ja2StracSaveEditorLib;
using Newtonsoft.Json;

namespace Ja2StracSaveEditor;

public partial class Form1 : Form
{
    private const string SettingsFilename = @"settings.json";

    private SettingsModel _settings;
    private SaveGame _saveGame;
    private Soldier _soldier;

    public Form1()
    {
        InitializeComponent();
        LoadSettings();
    }

    private void LoadSettings()
    {
        if (!File.Exists(SettingsFilename))
        {
            _settings = new SettingsModel();
            return;
        }

        var content = File.ReadAllText(SettingsFilename, new UTF8Encoding(false));
        _settings = JsonConvert.DeserializeObject<SettingsModel>(content);
    }

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start(new ProcessStartInfo(linkLabel1.Text) { UseShellExecute = true });
    }

    private void SettingsBtn_Click(object sender, EventArgs e)
    {
        var settingsForm = new SettingsForm(_settings);
        if (DialogResult.OK != settingsForm.ShowDialog()) return;
        var content = JsonConvert.SerializeObject(_settings, Formatting.Indented);
        File.WriteAllText(SettingsFilename, content, new UTF8Encoding(false));
    }

    private void OpenBtn_Click(object sender, EventArgs e)
    {
        _soldier = null;

        using var dialog = new OpenFileDialog();
        dialog.Filter = @"Save Files (*.sav)|*.sav";
        dialog.Title = @"Choose .sav";
        dialog.Multiselect = false;
        dialog.RestoreDirectory = true;

        if (dialog.ShowDialog() != DialogResult.OK) return;

        try
        {
            _saveGame = new SaveGame(dialog.FileName, _settings.Ja2StracPath);
            _saveGame.Load();

            SoldiersLst.DisplayMember = nameof(Soldier.name);
            SoldiersLst.ValueMember = nameof(Soldier.ubID);
            SoldiersLst.DataSource = _saveGame.Soldiers;

            //
        }
        catch (Exception exc)
        {
            MessageBox.Show($@"{exc.Message}\r\n{exc.StackTrace}", @"Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void LoadAttributes()
    {
        tabPage1.SuspendLayout();
        //tabPage1.Visible = false;
        tabPage1.Controls.Clear();

        try
        {
            if (_soldier == null) return;

            var skipProperties = new HashSet<string>
            {
                nameof(Soldier.CheckSum)
            };

            var t = typeof(Soldier);
            var ctrlX = 10;
            var startCtrlY = 10;
            var ctrlY = startCtrlY;
            var ctrlWidth = 120;
            var ctrlPerCol = 10;
            var ctrlCol = 0;
            var ctrlColSize = 2 * ctrlX + ctrlWidth + 10 + 100;
            var ctrlCnt = 1;

            var props = _soldier.Offsets
                .Select(s => s.Key)
                .Select(name => t.GetProperty(name))
                .Where(p => p != null)
                .OrderBy(p => p.GetCustomAttribute<DisplayAttribute>()?.Order ?? int.MinValue)
                .ThenBy(p => p.Name)
                .ToList();

            foreach (var prop in props)
            {
                if (skipProperties.Contains(prop.Name)) continue;

                var labelText = prop.GetCustomAttribute<DisplayAttribute>()?.Name ?? prop.Name;
                var value = prop.GetValue(_soldier, null);
                if (value == null) continue;

                var valueType = value.GetType();

                Control ctrl;

                var enumAttr = prop.GetCustomAttribute<EnumDataTypeAttribute>();
                if (enumAttr != null || valueType.IsEnum)
                {
                    var enumType = enumAttr?.EnumType ?? valueType;
                    var comboBox = new ComboBox
                    {
                        DropDownStyle = ComboBoxStyle.DropDownList,
                        Width = ctrlWidth,
                        Location = new Point(ctrlCol * ctrlColSize + ctrlX, ctrlY),
                        DataSource = Enum.GetValues(enumType)
                    };

                    // Привязка через SelectedItem — работает с Enum напрямую
                    var enumBinding = new Binding(nameof(comboBox.SelectedItem), _soldier, prop.Name, true,
                        DataSourceUpdateMode.OnPropertyChanged);

                    enumBinding.Format += (_, e) =>
                    {
                        if (e.Value == null || enumType.IsInstanceOfType(e.Value)) return;
                        var enumValue = Enum.ToObject(enumType, e.Value);
                        if (!Equals(e.Value, enumValue))
                            e.Value = enumValue;
                    };

                    enumBinding.Parse += (_, e) =>
                    {
                        if (e.Value == null) return;
                        var converted = Enum.ToObject(enumType, e.Value);

                        if (!Equals(e.Value, converted))
                            e.Value = converted;
                    };

                    comboBox.DataBindings.Add(enumBinding);

                    ctrl = comboBox;
                }
                else if (valueType == typeof(sbyte) || valueType == typeof(byte) || valueType == typeof(short) ||
                         valueType == typeof(ushort))
                {
                    var numCtrl = new NumericUpDown();
                    numCtrl.Width = ctrlWidth;
                    numCtrl.Location = new Point(ctrlCol * ctrlColSize + ctrlX, ctrlY);

                    var binding = new Binding(nameof(numCtrl.Value), _soldier, prop.Name, true,
                        DataSourceUpdateMode.OnPropertyChanged);

                    binding.Format += (_, e) =>
                    {
                        if (e.DesiredType == typeof(decimal) && e.Value != null)
                            e.Value = Convert.ToDecimal(e.Value);
                    };

                    binding.Parse += (_, e) =>
                    {
                        if (e.DesiredType == typeof(sbyte))
                            e.Value = Convert.ToSByte(e.Value);
                        else if (e.DesiredType == typeof(byte))
                            e.Value = Convert.ToByte(e.Value);
                        else if (e.DesiredType == typeof(short))
                            e.Value = Convert.ToInt16(e.Value);
                        else if (e.DesiredType == typeof(ushort))
                            e.Value = Convert.ToUInt16(e.Value);
                    };


                    if (valueType == typeof(sbyte))
                    {
                        numCtrl.Minimum = sbyte.MinValue;
                        numCtrl.Maximum = sbyte.MaxValue;
                    }
                    else if (valueType == typeof(byte))
                    {
                        numCtrl.Minimum = byte.MinValue;
                        numCtrl.Maximum = byte.MaxValue;
                    }
                    else if (valueType == typeof(short))
                    {
                        numCtrl.Minimum = short.MinValue;
                        numCtrl.Maximum = short.MaxValue;
                    }
                    else if (valueType == typeof(ushort))
                    {
                        numCtrl.Minimum = ushort.MinValue;
                        numCtrl.Maximum = ushort.MaxValue;
                    }

                    numCtrl.DataBindings.Add(binding);
                    ctrl = numCtrl;
                }
                else
                {
                    throw new NotImplementedException($"Format type {valueType.Name}");
                }

                tabPage1.Controls.Add(ctrl);

                var label = new Label();
                label.Text = labelText;
                label.AutoSize = true;
                label.Location = new Point(ctrlCol * ctrlColSize + ctrlX + ctrlWidth + 10, ctrlY);
                tabPage1.Controls.Add(label);

                ctrlY += 30;
                var nextCtrlCol =
                    Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ctrlCnt) / Convert.ToDouble(ctrlPerCol))) -
                    1;
                if (ctrlCol != nextCtrlCol) ctrlY = startCtrlY;
                ctrlCol = nextCtrlCol;
                ctrlCnt++;
            }
        }
        catch (Exception exc)
        {
            MessageBox.Show($@"{exc.Message}\r\n{exc.StackTrace}", @"Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            tabPage1.ResumeLayout();
            //tabPage1.Visible = true;
        }
    }

    private void SaveBtn_Click(object sender, EventArgs e)
    {
        try
        {
            _saveGame?.Save();
            MessageBox.Show(@"Saved");
        }
        catch (Exception exc)
        {
            MessageBox.Show($@"{exc.Message}\r\n{exc.StackTrace}", @"Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void SoldiersLst_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (SoldiersLst.SelectedItem is not Soldier soldier) return;
        _soldier = soldier;

        LoadAttributes();
        LoadInventory();
    }

    private void LoadInventory()
    {
        try
        {
            if (_soldier?.inv == null) return;
            ;
        }
        catch (Exception exc)
        {
            MessageBox.Show($@"{exc.Message}\r\n{exc.StackTrace}", @"Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}