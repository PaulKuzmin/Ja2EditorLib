using Ja2StracSaveEditorLib;
using Ja2StracSaveEditorLib.Managers;

namespace Ja2StracSaveEditor;

public partial class AttachForm : Form
{
    private readonly List<Item> _items = new List<Item>();

    private readonly Dictionary<AttachmentSlot, InventorySlot> _slots = new Dictionary<AttachmentSlot, InventorySlot>
    {
        { AttachmentSlot.Slot1, new InventorySlot(new Point(10, 10)) },
        { AttachmentSlot.Slot2, new InventorySlot(new Point(155, 10)) },
        { AttachmentSlot.Slot3, new InventorySlot(new Point(10, 106)) },
        { AttachmentSlot.Slot4, new InventorySlot(new Point(155, 106)) },
    };

    public ushort[] Attachments { get; } = new ushort[ObjectType.MAX_ATTACHMENTS];
    public sbyte[] Status { get; } = new sbyte[ObjectType.MAX_ATTACHMENTS];

    public AttachForm()
    {
        InitializeComponent();
        InitSlots();
    }

    public AttachForm(List<Item> items, ushort[] attachments, sbyte[] status)
    {
        InitializeComponent();
        InitSlots();

        _items = items;

        for (var i = 0; i < ObjectType.MAX_ATTACHMENTS; i++)
        {
            Attachments[i] = attachments[i];
        }

        for (var i = 0; i < ObjectType.MAX_ATTACHMENTS; i++)
        {
            Status[i] = status[i];
        }
    }

    private void InitSlots()
    {
        foreach (var slot in _slots)
        {
            panel3.Controls.Add(slot.Value.ProgressBar);
            panel3.Controls.Add(slot.Value.Button);
        }
    }

    private void button2_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }

    private void AttachForm_Load(object sender, EventArgs e)
    {
        foreach (var slot in _slots)
        {
            var index = (int)slot.Key;
            if (index > Attachments.Length) continue;
            var itemIndex = Attachments[index];

            var itemStatus = 100;
            if (index < Attachments.Length) itemStatus = Status[index];

            slot.Value.Status = itemStatus;
            slot.Value.Item = _items?.FirstOrDefault(f => f.itemIndex == itemIndex);
            if (slot.Value.Item != null)
            {
                slot.Value.ButtonImage = Exts.GetItemImageFilename(slot.Value.Item);
                slot.Value.ButtonText = slot.Value.Item.internalName;
            }

            slot.Value.Button.Click += (o, args) =>
            {
                try
                {
                    panel3.Enabled = false;

                    var itemsForm = new ItemsForm(_items)
                    {
                        Item = slot.Value.Item
                    };

                    if (DialogResult.OK != itemsForm.ShowDialog() || itemsForm.Item == null) return;

                    Attachments[index] = Convert.ToUInt16(itemsForm.Item.itemIndex);
                    Status[index] = 100;

                    slot.Value.Item = itemsForm.Item;
                    slot.Value.Status = Status[index];
                    if (slot.Value.Item != null)
                    {
                        slot.Value.ButtonImage = Exts.GetItemImageFilename(slot.Value.Item);
                        slot.Value.ButtonText = slot.Value.Item.internalName;
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show($@"{exc.Message}\r\n{exc.StackTrace}", @"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                finally
                {
                    panel3.Enabled = true;
                }
            };
        }
    }
}