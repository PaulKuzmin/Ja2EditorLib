using Ja2StracSaveEditorLib;

namespace Ja2StracSaveEditor;

public partial class InventoryControl : UserControl
{
    public Dictionary<InvSlotPos, InventorySlot> InventorySlots = new Dictionary<InvSlotPos, InventorySlot>
    {
        { InvSlotPos.HELMETPOS, new InventorySlot(new Point(495, 28), InventorySlotSize.Medium, hasCount: false) },
        { InvSlotPos.VESTPOS, new InventorySlot(new Point(495, 144), InventorySlotSize.Medium, hasCount: false) },
        { InvSlotPos.LEGPOS, new InventorySlot(new Point(495, 384), InventorySlotSize.Medium, hasCount: false) },
        { InvSlotPos.HEAD1POS, new InventorySlot(new Point(23, 28), hasCount: false) },
        { InvSlotPos.HEAD2POS, new InventorySlot(new Point(23, 124), hasCount: false) },
        { InvSlotPos.HANDPOS, new InventorySlot(new Point(23, 340), InventorySlotSize.Large) },
        { InvSlotPos.SECONDHANDPOS, new InventorySlot(new Point(23, 436), InventorySlotSize.Large) },
        { InvSlotPos.BIGPOCK1POS, new InventorySlot(new Point(992, 24), InventorySlotSize.Large) },
        { InvSlotPos.BIGPOCK2POS, new InventorySlot(new Point(992, 120), InventorySlotSize.Large) },
        { InvSlotPos.BIGPOCK3POS, new InventorySlot(new Point(992, 216), InventorySlotSize.Large) },
        { InvSlotPos.BIGPOCK4POS, new InventorySlot(new Point(992, 312), InventorySlotSize.Large) },
        { InvSlotPos.SMALLPOCK1POS, new InventorySlot(new Point(704, 24)) },
        { InvSlotPos.SMALLPOCK2POS, new InventorySlot(new Point(704, 120)) },
        { InvSlotPos.SMALLPOCK3POS, new InventorySlot(new Point(704, 216)) },
        { InvSlotPos.SMALLPOCK4POS, new InventorySlot(new Point(704, 312)) },
        { InvSlotPos.SMALLPOCK5POS, new InventorySlot(new Point(848, 24)) },
        { InvSlotPos.SMALLPOCK6POS, new InventorySlot(new Point(848, 120)) },
        { InvSlotPos.SMALLPOCK7POS, new InventorySlot(new Point(848, 216)) },
        { InvSlotPos.SMALLPOCK8POS, new InventorySlot(new Point(848, 312)) },

        { InvSlotPos.NUM_INV_SLOTS, new InventorySlot(new Point(10, 10)) },
    };

    public InventoryControl()
    {
        InitializeComponent();

        foreach (var slot in InventorySlots)
        {
            if (slot.Key == InvSlotPos.NUM_INV_SLOTS) continue;

            this.Controls.Add(slot.Value.ProgressBar);
            this.Controls.Add(slot.Value.AsteriskLabel);
            this.Controls.Add(slot.Value.CountLabel);
            this.Controls.Add(slot.Value.Button);

            slot.Value.Button.Click += (sender, args) =>
            {
                InventorySlots[slot.Key].Count = int.MaxValue;
                MessageBox.Show($@"Clicked {slot.Key}");
            };
        }
    }
}