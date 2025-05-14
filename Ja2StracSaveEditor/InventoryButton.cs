namespace Ja2StracSaveEditor;

public class TransparentButton : Button
{
    public TransparentButton()
    {
        this.FlatStyle = FlatStyle.Flat;
        this.FlatAppearance.BorderSize = 0;
        this.FlatAppearance.MouseDownBackColor = Color.Transparent;
        this.FlatAppearance.MouseOverBackColor = Color.Transparent;
        this.BackColor = Color.Transparent;
        this.ForeColor = Color.White; // или любой другой цвет текста
        this.SetStyle(ControlStyles.SupportsTransparentBackColor |
                      ControlStyles.OptimizedDoubleBuffer |
                      ControlStyles.AllPaintingInWmPaint |
                      ControlStyles.UserPaint, true);
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        // Пропускаем отрисовку фона
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        TextRenderer.DrawText(pevent.Graphics, Text, Font, ClientRectangle, ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }
}