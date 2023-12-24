using Framework;
using Microsoft.VisualBasic;
using System.Web.UI;
using System.Web.UI.WebControls;

public enum EControlMode
{

    Editable = 0,

    Disabled = 1,

    Locked = 2,
}
public abstract class CCustomControl : UserControl
{

    public event ServerValidateEventHandler ServerValidate;

    private string m_errorMessage;

    private string m_errorMessageCustom;

    protected override void OnInit(System.EventArgs e)
    {
        base.OnInit(e);
        RestoreMode();
        CtrlValidatorCustom.ServerValidate += new ServerValidateEventHandler(this.CtrlValidatorCustom_ServerValidate);
    }

    protected override void OnPreRender(System.EventArgs e)
    {
        base.OnPreRender(e);
        CtrlMain.Visible = !Locked;
        CtrlValidator.Visible = !Locked;
        CtrlValidatorCustom.Visible = !Locked;
        if (Locked)
        {
            CtrlLocked.Text = this.GetLockedText();
            CtrlMain.Visible = false;
            CtrlLocked.Visible = true;
        }

        CtrlLocked.Visible = (Locked
                    & (CtrlLocked.Text.Length > 0));
        if ((Locked || Disabled))
        {
            Required = false;
        }

        SetEnabled = Enabled;
        CtrlLabel.Visible = (Layout != ELayout.None);
        if ((!(CtrlDescription == null) && (0 == CtrlDescription.Text.Length)))
        {
            CtrlDescription.Visible = false;
        }

        if (!(CtrlValidator == null))
        {
            CtrlValidator.ErrorMessage = string.Format(ErrorMessage, Label);
        }

        if (!(CtrlValidatorCustom == null))
        {
            CtrlValidatorCustom.ErrorMessage = string.Format(ErrorMessageCustom, Label);
        }

        if (!(CtrlValidatorScript == null))
        {
            CtrlValidatorScript.Visible = Required;
        }

    }

    void CtrlValidatorCustom_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        ServerValidate(this, e);
    }

    public ELayout Layout
    {
        get
        {
            return CtrlContainerBegin.Layout;
        }
        set
        {
            CtrlContainerBegin.Layout = value;
            CtrlContainerEnd.Layout = value;
            CtrlSeparator1.Layout = value;
            CtrlSeparator2.Layout = value;
        }
    }

    public string Label
    {
        get
        {
            return CtrlLabel.Text;
        }
        set
        {
            CtrlLabel.Text = value;
        }
    }

    public bool ShowLabel
    {
        get
        {
            return CtrlLabel.Visible;
        }
        set
        {
            CtrlLabel.Visible = value;
        }
    }

    public string ErrorMessage
    {
        get
        {
            if (string.IsNullOrEmpty(m_errorMessage))
            {
                return DefaultFormatRequiredText;
            }

            return m_errorMessage;
        }
        set
        {
            m_errorMessage = value;
        }
    }

    public string ErrorMessageCustom
    {
        get
        {
            if (string.IsNullOrEmpty(m_errorMessageCustom))
            {
                return DefaultFormatRequiredText;
            }

            return m_errorMessageCustom;
        }
        set
        {
            m_errorMessageCustom = value;
        }
    }

    public string Description
    {
        get
        {
            return (CtrlDescription == null) ? string.Empty : CtrlDescription.Text;
        }
        set
        {
            if (null != CtrlDescription)
                CtrlDescription.Text = value;

        }
    }

    public EControlMode Mode
    {
        get
        {
            return ((EControlMode)(CTextbox.GetInteger(CtrlHidden.Value)));
        }
        set
        {
            CtrlHidden.Value = ((int)value).ToString();
        }
    }

    public bool Required
    {
        get
        {
            return CtrlValidator.Enabled;
        }
        set
        {
            CtrlValidator.Enabled = value;
        }
    }

    public bool RequiredCustom
    {
        get
        {
            return CtrlValidatorCustom.Enabled;
        }
        set
        {
            CtrlValidatorCustom.Enabled = value;
        }
    }

    public bool Disabled
    {
        get
        {
            return this.Mode == EControlMode.Disabled;
        }
        set
        {
            Enabled = !value;
        }
    }

    public bool Enabled
    {
        get
        {
            return !Disabled;
        }
        set
        {
            if (value)
            {
                Mode = EControlMode.Editable;
            }
            else
            {
                Mode = EControlMode.Disabled;
            }

        }
    }

    public bool Locked
    {
        get
        {
            return this.Mode == EControlMode.Locked;
        }
    }

    protected abstract string GetLockedText();
    protected abstract string DefaultFormatRequiredText { get; }
    protected abstract bool SetEnabled { set; }
    public abstract string Tooltip { get; set; }


    protected abstract Literal CtrlLabel { get; }
    protected abstract Label CtrlLocked { get; }
    protected abstract Control CtrlMain { get; }
    protected abstract BaseValidator CtrlValidator { get; }
    protected abstract CustomValidator CtrlValidatorCustom { get; }
    protected abstract HiddenField CtrlHidden { get; }


    protected virtual Label CtrlDescription {  get { return null; } }
    protected virtual PlaceHolder CtrlValidatorScript { get { return null; } }

    protected abstract CCustomControlContainer CtrlContainerBegin { get; }
    protected abstract CCustomControlContainer CtrlContainerEnd { get; }
    protected abstract CCustomControlContainer CtrlSeparator1 { get; }
    protected abstract CCustomControlContainer CtrlSeparator2 { get; }

    private void RestoreMode()
    {
        // in lieu of viewstate
        if (!Page.IsPostBack)
        {
            return;
        }

        string key = CtrlHidden.UniqueID;
        string value = Request.Form[key];
        int mode = 0;
        if (int.TryParse(value, out mode))
        {
            this.Mode = (EControlMode)mode;
        }

    }

    // Editable/Disabled/Locked
    public static void SetControlMode(Control parent, EControlMode mode)
    {
        if ((parent is CCustomControl))
        {
            ((CCustomControl)parent).Mode = mode;
        }

        foreach (Control i in parent.Controls)
        {
            SetControlMode(i, mode);
        }

    }
}