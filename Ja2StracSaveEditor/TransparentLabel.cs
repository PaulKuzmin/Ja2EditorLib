namespace Ja2StracSaveEditor;

using System.Drawing;
using System.Windows.Forms;

public class TransparentLabel : Label
{
    public TransparentLabel()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.UserPaint, true);
    }

    protected override void CreateHandle()
    {
        base.CreateHandle();

        // ReSharper disable VirtualMemberCallInConstructor
        BackColor = Color.Transparent;
        ForeColor = Color.Yellow;
        Font = new Font("Segoe UI", 20, FontStyle.Bold);
        // ReSharper restore VirtualMemberCallInConstructor
    }

    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.ExStyle |= 0x20; // WS_EX_TRANSPARENT
            return cp;
        }
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        // Не закрашиваем фон — он "прозрачный"
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        // Отрисуем фон родителя
        if (Parent != null)
        {
            using Bitmap bmp = new Bitmap(Parent.ClientSize.Width, Parent.ClientSize.Height);
            Parent.DrawToBitmap(bmp, Parent.ClientRectangle);
            e.Graphics.DrawImage(bmp, -Left, -Top);
        }

        // Отрисуем текст
        TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
    }
}
