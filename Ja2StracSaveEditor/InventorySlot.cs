using System.ComponentModel;
using System.Runtime.CompilerServices;

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
    private int _count = 99;
    private string _buttonText = string.Empty;
    public VerticalProgressBar ProgressBar { get; }

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
            CountLabel.Text = $@"{value}";
            _count = value;
            OnPropertyChanged();
        }
    }

    public Label CountLabel { get; }

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

    public InventorySlot(Point location, InventorySlotSize size = InventorySlotSize.Small, bool hasCount = true)
    {
        ProgressBar = new VerticalProgressBar
            { Value = 100, Maximum = 100, Minimum = 0, Location = location, Size = new Size(12, 85) };

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
            Text = hasCount ? $"{Count}" : "",
            Visible = hasCount
        };

        Button = new Button()
        {
            BackColor = Color.Transparent,
            ForeColor = Color.White,
            AutoSize = false,
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
        Button.SendToBack();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}