using System.ComponentModel;
using System.Runtime.CompilerServices;
using Ja2StracSaveEditorLib.Managers;

namespace Ja2StracSaveEditor;

public enum InventorySlotSize
{
    Small,
    Medium,
    Large
}

public class InventorySlot : INotifyPropertyChanged
{
    private bool _asteriskVisible = true;
    private int _count = -99;
    private int _bulletsCount = -99;
    private int _status = 1;
    private string _buttonText = string.Empty;
    private string _buttonImage = string.Empty;
    private Item _item;
    public VerticalProgressBar ProgressBar { get; }

    public int Status
    {
        get => _status;
        set
        {
            if (value == _status) return;
            
            if (value > ProgressBar.Maximum) ProgressBar.Value = ProgressBar.Maximum;
            if (value < ProgressBar.Minimum) ProgressBar.Value = ProgressBar.Minimum;
            ProgressBar.Value = value;

            _status = value;
            OnPropertyChanged();
        }
    }

    public bool AsteriskVisible
    {
        get => _asteriskVisible;
        set
        {
            if (value == _asteriskVisible) return;
            AsteriskLabel.Visible = value;
            _asteriskVisible = value;
            OnPropertyChanged();
        }
    }

    public Label AsteriskLabel { get; }

    public int Count
    {
        get => _count;
        set
        {
            if (value == _count) return;
            CountLabel.Text = value > 1 ? $@"{value}" : "";
            CountLabel.Visible = value > 1;
            _count = value;
            OnPropertyChanged();
        }
    }

    public Label CountLabel { get; }

    public int BulletsCount
    {
        get => _bulletsCount;
        set
        {
            if (value == _bulletsCount) return;
            BulletsCountLabel.Text = value < 0 ? "" : $@"{value}";
            BulletsCountLabel.Visible = value >= 0;
            _bulletsCount = value;
            OnPropertyChanged();
        }
    }

    public Label BulletsCountLabel { get; }

    public Button Button { get; }

    public string ButtonText
    {
        get => _buttonText;
        set
        {
            if (value == _buttonText) return;
            _buttonText = value;
            Button.Text = value;
            OnPropertyChanged();
        }
    }

    public string ButtonImage
    {
        get => _buttonImage;
        set
        {
            if (value == _buttonImage) return;
            _buttonImage = value;
            try
            {
                Button.Image = string.IsNullOrWhiteSpace(_buttonImage) ? null : Image.FromFile(_buttonImage);
            }
            catch 
            {
                Button.BackgroundImage = null;
            }
            OnPropertyChanged();
        }
    }

    public Item Item
    {
        get => _item;
        set
        {
            if (Equals(value, _item)) return;
            _item = value;
            OnPropertyChanged();
        }
    }

    public InventorySlot(Point location, InventorySlotSize size = InventorySlotSize.Small)
    {
        ProgressBar = new VerticalProgressBar
            { Value = Status, Maximum = 100, Minimum = 0, Location = location, Size = new Size(12, 85) };

        AsteriskLabel = new Label
        {
            ForeColor = Color.Red,
            BackColor = Color.Transparent,
            AutoSize = true,
            Font = new Font("Segoe UI", 20, FontStyle.Bold),
            Location = new Point(
                size switch
                {
                    InventorySlotSize.Small => location.X + 110,
                    InventorySlotSize.Medium => location.X + 165,
                    _ => location.X + 235
                }, location.Y - 5),
            Text = AsteriskVisible ? "*" : ""
        };

        CountLabel = new Label
        {
            ForeColor = Color.Yellow,
            BackColor = Color.Transparent,
            AutoSize = true,
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            Location = new Point(
                size switch
                {
                    InventorySlotSize.Small => location.X + 101,
                    InventorySlotSize.Medium => location.X + 158,
                    _ => location.X + 223
                }, location.Y + 60),
            Text = Count > 0 ? $"{Count}" : "",
            Visible = Count > 0
        };

        BulletsCountLabel = new Label
        {
            ForeColor = Color.Red,
            BackColor = Color.Transparent,
            AutoSize = true,
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            Location = new Point(
                size switch
                {
                    InventorySlotSize.Small => location.X + 22,
                    InventorySlotSize.Medium => location.X + 22,
                    _ => location.X + 22
                }, location.Y + 60),
            Text = BulletsCount >= 0 ? $"{BulletsCount}" : "",
            Visible = BulletsCount >= 0
        };

        Button = new Button()
        {
            BackColor = Color.Transparent,
            ForeColor = Color.White,
            AutoSize = false,
            Font = new Font("Segoe UI", 7, FontStyle.Regular),
            Location = new Point(
                size switch
                {
                    InventorySlotSize.Small => location.X + 25,
                    InventorySlotSize.Medium => location.X + 25,
                    _ => location.X + 25
                }, location.Y),
            Size = new Size(size switch
            {
                InventorySlotSize.Small => 108,
                InventorySlotSize.Medium => 165,
                _ => 233
            }, 85),
            BackgroundImage =
                size switch
                {
                    InventorySlotSize.Small => Properties.Resources.small,
                    InventorySlotSize.Medium => Properties.Resources.medium,
                    _ => Properties.Resources.large
                },
            FlatStyle = FlatStyle.Flat,
            Text = ButtonText
        };
        Button.FlatAppearance.BorderSize = 0;
        Button.ImageAlign = ContentAlignment.TopCenter;
        Button.TextAlign = ContentAlignment.BottomCenter;
        Button.SendToBack();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}