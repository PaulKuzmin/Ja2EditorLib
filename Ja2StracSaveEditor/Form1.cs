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
                    try
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

                        inventoryControl1.Enabled = false;

                        if (DialogResult.OK != form.ShowDialog() || form.Item == null) return;

                        ushort gunAmmoItem = 0;
                        if (!string.IsNullOrWhiteSpace(form.Item.calibre) && allItems != null)
                        {
                            var ammo = allItems
                                .Where(w => w.calibre == form.Item.calibre && w.usItemClass == Item.IC_AMMO)
                                .MaxBy(o => o.ubCoolness);
                            if (ammo != null)
                            {
                                gunAmmoItem = Convert.ToUInt16(ammo.itemIndex);
                            }
                        }

                        var ubPerPocket = Convert.ToByte(form.Item.ubPerPocket <= 0 ? 1 : form.Item.ubPerPocket);
                        var newObj = new ObjectType
                        {
                            usItem = (ushort)form.Item.itemIndex,
                            usItemCheckSum = (ushort)form.Item.itemIndex,
                            Item = form.Item,
                            ubNumberOfObjects = ubPerPocket,
                            bGunStatus = 100,
                            ubGunShotsLeft = Convert.ToByte(form.Item.ubMagSize ?? 0), //obj.ubGunShotsLeft,
                            bStatus = FillStatus(ubPerPocket),
                            ubShotsLeft =
                                FillShotsLeft(Convert.ToByte(form.Item.ubMagSize), ubPerPocket), //obj.ubShotsLeft,
                            usAttachItem = new ushort[ObjectType.MAX_ATTACHMENTS],
                            bAttachStatus = new sbyte[ObjectType.MAX_ATTACHMENTS],
                            fFlags = 0,
                            ubMission = 0,
                            bTrap = 0,
                            ubImprintID = 200,
                            ubWeight = 1, //1g
                            fUsed = 0,
                            Offset = obj.Offset,
                            uiMoneyAmount = uint.MaxValue,
                            bMoneyStatus = 100,
                            padding = new sbyte[4],
                            bBombStatus = 100,
                            bGunAmmoStatus = 100,
                            bDetonatorType = 0,
                            bDelay = 0,
                            ubOwnerCivGroup = 0,
                            ubBombOwner = 0, //obj.ubBombOwner,
                            bActionValue = 0, //obj.ubBombOwner,
                            ubTolerance = 0, //obj.ubBombOwner,
                            bKeyStatus = new sbyte[6], //obj.ubBombOwner,
                            ubKeyID = 0, //obj.ubBombOwner,
                            ubOwnerProfile = 0, //obj.ubBombOwner,

                            ubGunAmmoType = form.Item.AmmoTypeByte(),
                            usGunAmmoItem = gunAmmoItem,
                            usBombItem = 0, //obj.usBombItem,
                        };

                        //
                        if (newObj.Item.usItemClass == Item.IC_GUN)
                        {
                            newObj.ubGunShotsLeft = Convert.ToByte(newObj.Item.ubMagSize ?? 0);
                            newObj.ubShotsLeft = FillShotsLeft(Convert.ToByte(newObj.Item.ubMagSize), ubPerPocket);
                        }
                        else
                        {
                            var item = _saveGame?.ItemDataManager?.Items?.FirstOrDefault(f => f.itemIndex == newObj.usItem);
                            if (item is { capacity: > 0 })
                            {
                                newObj.ubGunShotsLeft = Convert.ToByte(item.capacity);
                                newObj.ubShotsLeft = FillShotsLeft(Convert.ToByte(item.capacity), ubPerPocket);
                            }
                            else
                            {
                                newObj.ubGunShotsLeft = 30;
                                newObj.ubShotsLeft = FillShotsLeft(30, ubPerPocket);
                            }
                        }
                        //

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
                    catch (Exception exc)
                    {
                        MessageBox.Show($@"{exc.Message}\r\n{exc.StackTrace}", @"Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    finally
                    {
                        inventoryControl1.Enabled = true;
                    }
                };

                invSlot.Button.MouseUp += (_, args) =>
                {
                    if (args.Button != MouseButtons.Right) return;

                    try
                    {
                        inventoryControl1.Enabled = false;

                        var attachObj = _soldier.inv[(int)slot];

                        var attachableItems = _saveGame.ItemDataManager.Items
                            .Where(w => w.bAttachment != null && w.bAttachment.Value)
                            .ToList();

                        var attForm = new AttachForm(attachableItems, attachObj.usAttachItem, attachObj.bAttachStatus);
                        if (DialogResult.OK != attForm.ShowDialog()) return;

                        attachObj.usAttachItem = attForm.Attachments;
                        attachObj.bAttachStatus = attForm.Status;

                        invSlot.AsteriskVisible = HasAttachments(obj);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($@"{e.Message}\r\n{e.StackTrace}", @"Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    finally
                    {
                        inventoryControl1.Enabled = true;
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

    private byte[] FillShotsLeft(byte magSize, int n)
    {
        var result = new byte[ObjectType.MAX_OBJECTS_PER_SLOT];
        for (var i = 0; i < n && i < ObjectType.MAX_OBJECTS_PER_SLOT; i++)
        {
            result[i] = magSize;
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
        return obj?.usAttachItem != null && obj.usAttachItem.Any(a => a! > 0);
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

    private sbyte[] RepearStatus(sbyte[] s, int l)
    {
        s ??= new sbyte[l];
        for (var i = 0; i < l && i < s.Length; i++)
        {
            s[i] = 100;
        }

        return s;
    }

    private void RepearAllBtn_Click(object sender, EventArgs e)
    {
        try
        {
            if (_soldier?.inv == null) return;
            for (var index = 0; index < _soldier.inv.Length; index++)
            {
                var i = _soldier.inv[index];
                i.bGunStatus = 100;
                i.bStatus = RepearStatus(i.bStatus, ObjectType.MAX_OBJECTS_PER_SLOT);
                i.bAttachStatus = RepearStatus(i.bStatus, ObjectType.MAX_ATTACHMENTS);
                i.bMoneyStatus = 100;
                i.bBombStatus = 100;
                i.bGunAmmoStatus = 100;

                try
                {
                    var slot = (InvSlotPos)index;
                    var invSlot = inventoryControl1.InventorySlots[slot];
                    invSlot.Status = GetStatus(i);
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                }
            }
        }

        catch (Exception exc)
        {
            MessageBox.Show($@"{exc.Message}\r\n{exc.StackTrace}", @"Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void ReloadAllBtn_Click(object sender, EventArgs e)
    {
        try
        {
            if (_soldier?.inv == null) return;
            for (var index = 0; index < _soldier.inv.Length; index++)
            {
                var i = _soldier.inv[index];
                if (i.Item == null) continue;

                if (i.Item.usItemClass != Item.IC_GUN && i.Item.usItemClass != Item.IC_AMMO) continue;

                var ubPerPocket = Convert.ToByte(i.Item.ubPerPocket <= 0 ? 1 : i.Item.ubPerPocket);
                i.bGunAmmoStatus = 100;

                if (i.Item.usItemClass == Item.IC_GUN)
                {
                    i.ubGunShotsLeft = Convert.ToByte(i.Item.ubMagSize ?? 0);
                    i.ubShotsLeft = FillShotsLeft(Convert.ToByte(i.Item.ubMagSize), ubPerPocket);
                }
                else
                {
                    var item = _saveGame.ItemDataManager.Items.FirstOrDefault(f => f.itemIndex == i.usItem);
                    if (item is { capacity: > 0 })
                    {
                        i.ubGunShotsLeft = Convert.ToByte(item.capacity);
                        i.ubShotsLeft = FillShotsLeft(Convert.ToByte(item.capacity), ubPerPocket);
                    }
                    else
                    {
                        i.ubGunShotsLeft = 30;
                        i.ubShotsLeft = FillShotsLeft(30, ubPerPocket);
                    }
                }

                try
                {
                    var slot = (InvSlotPos)index;
                    var invSlot = inventoryControl1.InventorySlots[slot];
                    invSlot.BulletsCount = GetShotsLeft(i);
                    invSlot.Count = GetCnt(i);
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                }
            }
        }

        catch (Exception exc)
        {
            MessageBox.Show($@"{exc.Message}\r\n{exc.StackTrace}", @"Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void GetAmmoBtn_Click(object sender, EventArgs e)
    {
        if (_saveGame == null) return;
        var weaponSlot = inventoryControl1.InventorySlots[InvSlotPos.HANDPOS];
        if (weaponSlot?.Item == null || string.IsNullOrWhiteSpace(weaponSlot.Item.calibre)) return;

        var ammo = _saveGame.ItemDataManager.Items
            .Where(w => w.calibre == weaponSlot.Item.calibre && w.usItemClass == Item.IC_AMMO)
            .MaxBy(w => w.ubCoolness);
        if (ammo == null) return;

        var checkSlots = new HashSet<InvSlotPos>
        {
            InvSlotPos.BIGPOCK1POS,
            InvSlotPos.BIGPOCK2POS,
            InvSlotPos.BIGPOCK3POS,
            InvSlotPos.BIGPOCK4POS,

            InvSlotPos.SMALLPOCK1POS,
            InvSlotPos.SMALLPOCK2POS,
            InvSlotPos.SMALLPOCK3POS,
            InvSlotPos.SMALLPOCK4POS,
            InvSlotPos.SMALLPOCK5POS,
            InvSlotPos.SMALLPOCK6POS,
            InvSlotPos.SMALLPOCK7POS,
            InvSlotPos.SMALLPOCK8POS
        };

        foreach (var slot in checkSlots)
        {
            var iSlot = _soldier.inv[(int)slot];
            if (iSlot == null || iSlot.usItem > 0) continue;

            var ubPerPocket = Convert.ToByte(ammo.ubPerPocket <= 0 ? 1 : ammo.ubPerPocket);
            var newObj = new ObjectType
            {
                usItem = (ushort)ammo.itemIndex,
                usItemCheckSum = (ushort)ammo.itemIndex,
                Item = ammo,
                ubNumberOfObjects = ubPerPocket,
                bGunStatus = 100,
                ubGunShotsLeft = Convert.ToByte(ammo.ubMagSize ?? 0), //obj.ubGunShotsLeft,
                bStatus = FillStatus(ubPerPocket),
                ubShotsLeft =
                    FillShotsLeft(Convert.ToByte(ammo.ubMagSize), ubPerPocket), //obj.ubShotsLeft,
                usAttachItem = new ushort[ObjectType.MAX_ATTACHMENTS],
                bAttachStatus = new sbyte[ObjectType.MAX_ATTACHMENTS],
                fFlags = 0,
                ubMission = 0,
                bTrap = 0,
                ubImprintID = 200,
                ubWeight = 1, //1g
                fUsed = 0,
                Offset = iSlot.Offset,
                uiMoneyAmount = uint.MaxValue,
                bMoneyStatus = 100,
                padding = new sbyte[4],
                bBombStatus = 100,
                bGunAmmoStatus = 100,
                bDetonatorType = 0,
                bDelay = 0,
                ubOwnerCivGroup = 0,
                ubBombOwner = 0, //obj.ubBombOwner,
                bActionValue = 0, //obj.ubBombOwner,
                ubTolerance = 0, //obj.ubBombOwner,
                bKeyStatus = new sbyte[6], //obj.ubBombOwner,
                ubKeyID = 0, //obj.ubBombOwner,
                ubOwnerProfile = 0, //obj.ubBombOwner,

                ubGunAmmoType = ammo.AmmoTypeByte(),
                usGunAmmoItem = Convert.ToUInt16(ammo.itemIndex),
                usBombItem = 0, //obj.usBombItem,
            };

            if (ammo is { capacity: > 0 })
            {
                newObj.ubGunShotsLeft = Convert.ToByte(ammo.capacity);
                newObj.ubShotsLeft = FillShotsLeft(Convert.ToByte(ammo.capacity), ubPerPocket);
            }
            else
            {
                newObj.ubGunShotsLeft = 30;
                newObj.ubShotsLeft = FillShotsLeft(30, ubPerPocket);
            }

            _soldier.inv[(int)slot] = newObj;

            var invSlot = inventoryControl1.InventorySlots[slot];

            invSlot.Item = newObj.Item;
            invSlot.ButtonText = GetObjectName(newObj);
            invSlot.ButtonImage = Exts.GetItemImageFilename(newObj.Item);
            invSlot.AsteriskVisible = HasAttachments(newObj);
            invSlot.Status = GetStatus(newObj);
            invSlot.BulletsCount = GetShotsLeft(newObj);
            invSlot.Count = GetCnt(newObj);
        }
    }
}