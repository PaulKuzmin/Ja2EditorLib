namespace Ja2StracSaveEditor;

public class VerticalProgressBar : ProgressBar
{
    public VerticalProgressBar()
    {
        this.SetStyle(ControlStyles.UserPaint, true);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var rect = this.ClientRectangle;
        var g = e.Graphics;

        ProgressBarRenderer.DrawVerticalBar(g, rect);

        rect.Inflate(-3, -3);
        if (Value <= 0) return;
        var percent = (float)(Value - Minimum) / (Maximum - Minimum);
        var height = (int)(rect.Height * percent);
        var fill = new Rectangle(rect.X, rect.Bottom - height, rect.Width, height);
        ProgressBarRenderer.DrawVerticalChunks(g, fill);
    }
}