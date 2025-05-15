using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Ja2StracSaveEditorLib;
using Ja2StracSaveEditorLib.Managers;
using Newtonsoft.Json;

namespace Ja2StracSaveEditor;

public partial class Form1 : Form
{
    private SaveGame _saveGame;
    private Soldier _soldier;

    public Form1()
    {
        InitializeComponent();
        LoadSettings();

        if (!Directory.Exists(Program.ImageCacheDir))
            Directory.CreateDirectory(Program.ImageCacheDir);
    }

    private void LoadSettings()
    {
        if (!File.Exists(Program.SettingsFilename))
        {
            Program.Settings = new SettingsModel();
            return;
        }

        var content = File.ReadAllText(Program.SettingsFilename, new UTF8Encoding(false));
        Program.Settings = JsonConvert.DeserializeObject<SettingsModel>(content);
    }

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start(new ProcessStartInfo(linkLabel1.Text) { UseShellExecute = true });
    }

    private void SettingsBtn_Click(object sender, EventArgs e)
    {
        var settingsForm = new SettingsForm(Program.Settings);
        if (DialogResult.OK != settingsForm.ShowDialog()) return;
        var content = JsonConvert.SerializeObject(Program.Settings, Formatting.Indented);
        File.WriteAllText(Program.SettingsFilename, content, new UTF8Encoding(false));
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
            _saveGame = new SaveGame(dialog.FileName, Program.Settings.Ja2StracPath);
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

            foreach (InvSlotPos slot in Enum.GetValues(typeof(InvSlotPos)))
            {
                if (slot == InvSlotPos.NUM_INV_SLOTS) continue;
                var obj = _soldier.inv[(int)slot];

                var invSlot = inventoryControl1.InventorySlots[slot];

                invSlot.Item = obj.Item;
                invSlot.ButtonText = GetObjectName(obj);
                invSlot.ButtonImage = Exts.GetItemImageFilename(obj.Item);
                invSlot.AsteriskVisible = HasAttachments(obj);
                invSlot.Status = GetStatus(obj);
                invSlot.BulletsCount = GetShotsLeft(obj);
                invSlot.Count = GetCnt(obj);

                invSlot.Button.Click += (_, _) =>
                {
                    var allItems = _saveGame?.ItemDataManager?.Items?
                        .OrderBy(o => o.itemIndex)
                        .ToList();

                    if (slot is InvSlotPos.HELMETPOS or InvSlotPos.VESTPOS or InvSlotPos.LEGPOS)
                    {
                        // ReSharper disable once AssignNullToNotNullAttribute
                        allItems = allItems
                            .Where(w => (w.usItemClass == Item.IC_ARMOUR &&
                            (w.bAttachment == null || !w.bAttachment.Value)
                                ) || w.itemIndex == obj.usItem)
                            .ToList();
                    }

                    var form = new ItemsForm(allItems)
                    {
                        Item = obj.Item
                    };

                    if (DialogResult.OK == form.ShowDialog() && form.Item != null)
                    {
                        var newObj = new ObjectType
                        {
                            usItem = (ushort)form.Item.itemIndex,
                            usItemCheckSum = (ushort)form.Item.itemIndex,
                            Item = form.Item,
                            ubNumberOfObjects = Convert.ToByte(form.Item.ubPerPocket <= 0 ? 1 : form.Item.ubPerPocket),
                            bGunStatus = 100,
                            ubGunShotsLeft = Convert.ToByte(form.Item.ubMagSize ?? 0), //obj.ubGunShotsLeft,
                            bStatus = FillStatus(form.Item.ubPerPocket <= 0 ? 1 : form.Item.ubPerPocket),

                            ubGunAmmoType = obj.ubGunAmmoType,
                            usGunAmmoItem = obj.usGunAmmoItem,
                            bGunAmmoStatus = obj.bGunAmmoStatus,

                            ubShotsLeft = obj.ubShotsLeft,
                            
                            bMoneyStatus = obj.bMoneyStatus,
                            uiMoneyAmount = obj.uiMoneyAmount,
                            padding = obj.padding,
                            bBombStatus = obj.bBombStatus,
                            bDetonatorType = obj.bDetonatorType,
                            usBombItem = obj.usBombItem,
                            bDelay = obj.bDelay,
                            ubBombOwner = obj.ubBombOwner,
                            bActionValue = obj.bActionValue,
                            ubTolerance = obj.ubTolerance,
                            bKeyStatus = obj.bKeyStatus,
                            ubKeyID = obj.ubKeyID,
                            ubOwnerProfile = obj.ubOwnerProfile,
                            ubOwnerCivGroup = obj.ubOwnerCivGroup,

                            usAttachItem = new ushort[ObjectType.MAX_ATTACHMENTS],
                            bAttachStatus = new sbyte[ObjectType.MAX_ATTACHMENTS],

                            fFlags = 0,
                            ubMission = 0,
                            bTrap = 0,
                            ubImprintID = 200,
                            ubWeight = 1, //1g
                            fUsed = 0,

                            Offset = obj.Offset,
                        };

                        _soldier.inv[(int)slot] = newObj;

                        invSlot.Item = newObj.Item;
                        invSlot.ButtonText = GetObjectName(newObj);
                        invSlot.ButtonImage = Exts.GetItemImageFilename(newObj.Item);
                        invSlot.AsteriskVisible = HasAttachments(newObj);
                        invSlot.Status = GetStatus(newObj);
                        invSlot.BulletsCount = GetShotsLeft(newObj);
                        invSlot.Count = GetCnt(newObj);

                        obj = newObj;
                    }
                };
            }
        }
        catch (Exception exc)
        {
            MessageBox.Show($@"{exc.Message}\r\n{exc.StackTrace}", @"Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private sbyte[] FillStatus(int numItems)
    {
        var result = new sbyte[ObjectType.MAX_OBJECTS_PER_SLOT];
        for (var i = 0; i < numItems && i < ObjectType.MAX_OBJECTS_PER_SLOT; i++)
        {
            result[i] = 100;
        }

        return result;
    }

    private int GetCnt(ObjectType obj)
    {
        return obj?.ubNumberOfObjects ?? 0;
    }

    private int GetShotsLeft(ObjectType obj)
    {
        if (obj?.Item == null) return -1;
        if (obj.Item.usItemClass != Item.IC_GUN) return -1;

        return obj.ubGunShotsLeft;
    }

    private int GetStatus(ObjectType obj)
    {
        if (obj?.Item == null) return 0;

        switch (obj.Item.usItemClass)
        {
            case Item.IC_GUN:
                return obj.bGunStatus;
            case Item.IC_MONEY:
                return obj.bMoneyStatus;
            default:
                return FindStatus(obj.bStatus);
        }
    }

    private int FindStatus(sbyte[] array)
    {
        return array?.FirstOrDefault(f => f != 0) ?? 0;
    }

    private bool HasAttachments(ObjectType obj)
    {
        return obj?.usAttachItem != null && obj.usAttachItem.Any(a => a ! > 0);
    }

    private string GetObjectName(ObjectType obj)
    {
        if (obj == null) return string.Empty;
        if (obj.Item != null && !string.IsNullOrWhiteSpace(obj.Item.internalName))
        {
            if (obj.Item.internalName.Equals("nothing", StringComparison.InvariantCultureIgnoreCase) ||
                obj.Item.internalName.Equals("none", StringComparison.InvariantCultureIgnoreCase))
                return string.Empty;

            return obj.Item.internalName;
        }

        return $"#{obj.usItem}";
    }
}