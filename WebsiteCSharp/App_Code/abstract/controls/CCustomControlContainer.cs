using Microsoft.VisualBasic;
using System.Web.UI;

public enum ELayout
{

    Flow,

    Horizontal,

    Vertical,

    None,
}

public abstract class CCustomControlContainer : UserControl
{

    public ELayout Layout
    {
        get
        {
            if (Flow.Visible)
            {
                return ELayout.Flow;
            }

            if (Horizontal.Visible)
            {
                return ELayout.Horizontal;
            }

            if (Vertical.Visible)
            {
                return ELayout.Vertical;
            }

            return ELayout.None;
        }
        set
        {
            Flow.Visible = (value == ELayout.Flow);
            Horizontal.Visible = (value == ELayout.Horizontal);
            Vertical.Visible = (value == ELayout.Vertical);
        }
    }


    protected abstract  Control Flow { get; }
    protected abstract  Control Horizontal { get; }
    protected abstract  Control Vertical { get; }

    public static void SetLayout(Control parent, ELayout layout)
    {
        if (((parent is CCustomControl) && (((CCustomControl)(parent)).Layout != ELayout.None)))
        {
            ((CCustomControl)(parent)).Layout = layout;
        }

        if ((( parent is CCustomControlContainer) && (((CCustomControlContainer)(parent)).Layout != ELayout.None)))
        {
            ((CCustomControlContainer)(parent)).Layout = layout;
        }

        foreach (Control i in parent.Controls)
        {
            CCustomControlContainer.SetLayout(i, layout);
        }

    }
}