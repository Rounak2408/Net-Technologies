using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DefaultPage : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SuccessPanel.Visible = false;
        }
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        // Custom check to ensure at least one checkbox is checked
        bool anyEventSelected = false;
        foreach (ListItem item in cblEvents.Items)
        {
            if (item.Selected)
            {
                anyEventSelected = true;
                break;
            }
        }

        if (!anyEventSelected)
        {
            CustomValidator cv = new CustomValidator();
            cv.IsValid = false;
            cv.ErrorMessage = "At least one specific event must be selected.";
            Page.Validators.Add(cv);
        }

        if (Page.IsValid)
        {
            // Gather selected events
            var selectedEvents = new StringBuilder();
            foreach (ListItem item in cblEvents.Items)
            {
                if (item.Selected)
                {
                    if (selectedEvents.Length > 0)
                        selectedEvents.Append(", ");
                    selectedEvents.Append(item.Text);
                }
            }

            // Fill labels
            lblResultName.Text = Server.HtmlEncode(txtFullName.Text);
            lblResultEmail.Text = Server.HtmlEncode(txtEmail.Text);
            lblResultPhone.Text = Server.HtmlEncode(txtPhone.Text);
            lblResultAge.Text = Server.HtmlEncode(txtAge.Text);
            lblResultCategory.Text = Server.HtmlEncode(ddlCategory.SelectedValue);
            lblResultTshirt.Text = Server.HtmlEncode(rblTshirt.SelectedValue);
            lblResultEvents.Text = Server.HtmlEncode(selectedEvents.ToString());

            // Show panel
            SuccessPanel.Visible = true;

            // Clear input fields for a fresh registration
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtAge.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            ddlCategory.SelectedIndex = 0;
            
            // Uncheck t-shirt and reset to default M
            foreach (ListItem item in rblTshirt.Items)
            {
                item.Selected = (item.Value == "M");
            }
            
            // Clear check lists
            foreach (ListItem item in cblEvents.Items)
            {
                item.Selected = false;
            }
        }
        else
        {
            SuccessPanel.Visible = false;
        }
    }
}
