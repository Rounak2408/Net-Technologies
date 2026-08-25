<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="DefaultPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>NextGen Event Registration Portal</title>
    <link href="https://fonts.googleapis.com/css2?family=Outfit:wght@300;400;500;600;700&display=swap" rel="stylesheet" />
    <style>
        :root {
            --bg-color: #f1f5f9;
            --card-bg: #ffffff;
            --border-color: #cbd5e1;
            --primary-color: #2563eb;
            --primary-hover: #1d4ed8;
            --text-main: #1e293b;
            --text-muted: #64748b;
            --error-color: #dc2626;
            --success-color: #10b981;
        }

        * {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
        }

        body {
            font-family: 'Outfit', sans-serif;
            background-color: var(--bg-color);
            color: var(--text-main);
            min-height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 30px 15px;
        }

        .container {
            width: 100%;
            max-width: 700px;
            background: var(--card-bg);
            border: 1px solid var(--border-color);
            border-radius: 12px;
            padding: 35px;
            box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05), 0 2px 4px -1px rgba(0, 0, 0, 0.03);
        }

        header {
            text-align: center;
            margin-bottom: 30px;
            border-bottom: 2px solid var(--bg-color);
            padding-bottom: 15px;
        }

        header h1 {
            font-size: 2.2rem;
            font-weight: 700;
            color: var(--primary-color);
            margin-bottom: 5px;
        }

        header p {
            color: var(--text-muted);
            font-size: 1rem;
        }

        .form-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 18px;
        }

        .form-group {
            display: flex;
            flex-direction: column;
        }

        .form-group.full-width {
            grid-column: span 2;
        }

        label {
            font-size: 0.9rem;
            font-weight: 600;
            color: var(--text-main);
            margin-bottom: 6px;
        }

        .required-star {
            color: var(--error-color);
        }

        .input-control {
            background: #ffffff;
            border: 1px solid var(--border-color);
            border-radius: 6px;
            padding: 10px 12px;
            font-family: inherit;
            color: var(--text-main);
            font-size: 0.95rem;
            transition: all 0.2s ease;
            outline: none;
            width: 100%;
        }

        .input-control:focus {
            border-color: var(--primary-color);
            box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.15);
        }

        .val-error {
            color: var(--error-color);
            font-size: 0.8rem;
            margin-top: 4px;
            display: block;
        }

        .val-summary {
            background: #fef2f2;
            border: 1px solid #fca5a5;
            border-radius: 6px;
            padding: 12px 18px;
            margin-bottom: 20px;
            color: #b91c1c;
            font-size: 0.85rem;
        }

        .val-summary ul {
            list-style-type: disc;
            padding-left: 18px;
            margin-top: 5px;
        }

        .list-container {
            background: #ffffff;
            border: 1px solid var(--border-color);
            border-radius: 6px;
            padding: 10px;
        }

        .list-container table {
            width: 100%;
            border-collapse: collapse;
        }

        .list-container td {
            padding: 4px 8px;
            color: var(--text-main);
            font-size: 0.9rem;
        }

        .list-container input[type="radio"], 
        .list-container input[type="checkbox"] {
            margin-right: 6px;
            accent-color: var(--primary-color);
            cursor: pointer;
        }

        .list-container label {
            display: inline;
            cursor: pointer;
            font-weight: 400;
            color: var(--text-main);
        }

        .btn-submit {
            background: var(--primary-color);
            color: white;
            border: none;
            border-radius: 6px;
            padding: 12px 20px;
            font-size: 1rem;
            font-weight: 600;
            cursor: pointer;
            transition: background-color 0.2s ease;
            width: 100%;
            margin-top: 10px;
            font-family: inherit;
        }

        .btn-submit:hover {
            background: var(--primary-hover);
        }

        .success-panel {
            background: #ecfdf5;
            border: 1px solid #a7f3d0;
            border-radius: 8px;
            padding: 20px;
            margin-bottom: 20px;
            color: #065f46;
            animation: fadeIn 0.5s ease-out;
        }

        .success-panel h2 {
            color: #047857;
            font-size: 1.5rem;
            margin-bottom: 10px;
            display: flex;
            align-items: center;
            gap: 8px;
        }

        .success-details {
            display: grid;
            grid-template-columns: 140px 1fr;
            gap: 8px;
            margin-top: 12px;
            border-top: 1px solid #cbd5e1;
            padding-top: 12px;
            font-size: 0.9rem;
        }

        .success-details dt {
            color: #374151;
            font-weight: 600;
        }

        .success-details dd {
            color: #0f172a;
            font-weight: 500;
        }

        @media (max-width: 768px) {
            .form-grid {
                grid-template-columns: 1fr;
            }
            .form-group.full-width {
                grid-column: span 1;
            }
        }
    </style>
</head>
<body>
    <div class="container">
        <header>
            <h1>NextGen Registration</h1>
            <p>Online Event Portal &bull; Experiment 04</p>
        </header>

        <form id="form1" runat="server">
            
            <%-- Validation Summary of all Validation Controls --%>
            <asp:ValidationSummary 
                ID="ValidationSummary1" 
                runat="server" 
                CssClass="val-summary" 
                HeaderText="Please fix the following validation errors to register:" 
                DisplayMode="BulletList" />

            <%-- Success message placeholder --%>
            <asp:Panel ID="SuccessPanel" runat="server" Visible="false" CssClass="success-panel">
                <h2>
                    <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round" class="feather feather-check-circle" style="color: var(--success-color); vertical-align: middle;"><path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"></path><polyline points="22 4 12 14.01 9 11.01"></polyline></svg>
                    Registration Successful!
                </h2>
                <p>Welcome! You have successfully registered for the event. Here are your details:</p>
                <div class="success-details">
                    <dt>Full Name:</dt>
                    <dd><asp:Label ID="lblResultName" runat="server"></asp:Label></dd>
                    
                    <dt>Email Address:</dt>
                    <dd><asp:Label ID="lblResultEmail" runat="server"></asp:Label></dd>
                    
                    <dt>Phone Number:</dt>
                    <dd><asp:Label ID="lblResultPhone" runat="server"></asp:Label></dd>
                    
                    <dt>Age:</dt>
                    <dd><asp:Label ID="lblResultAge" runat="server"></asp:Label></dd>
                    
                    <dt>Category:</dt>
                    <dd><asp:Label ID="lblResultCategory" runat="server"></asp:Label></dd>
                    
                    <dt>T-Shirt Size:</dt>
                    <dd><asp:Label ID="lblResultTshirt" runat="server"></asp:Label></dd>
                    
                    <dt>Events Selected:</dt>
                    <dd><asp:Label ID="lblResultEvents" runat="server"></asp:Label></dd>
                </div>
            </asp:Panel>

            <div class="form-grid">
                
                <%-- Name --%>
                <div class="form-group">
                    <label for="txtFullName">Full Name <span class="required-star">*</span></label>
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="input-control" placeholder="Enter your full name"></asp:TextBox>
                    <asp:RequiredFieldValidator 
                        ID="rfvName" 
                        runat="server" 
                        ControlToValidate="txtFullName" 
                        ErrorMessage="Full Name is required" 
                        CssClass="val-error" 
                        Display="Dynamic">&bull; Full Name is required</asp:RequiredFieldValidator>
                </div>

                <%-- Age --%>
                <div class="form-group">
                    <label for="txtAge">Age <span class="required-star">*</span></label>
                    <asp:TextBox ID="txtAge" runat="server" CssClass="input-control" placeholder="Age (18 to 30)"></asp:TextBox>
                    <asp:RequiredFieldValidator 
                        ID="rfvAge" 
                        runat="server" 
                        ControlToValidate="txtAge" 
                        ErrorMessage="Age is required" 
                        CssClass="val-error" 
                        Display="Dynamic">&bull; Age is required</asp:RequiredFieldValidator>
                    <asp:RangeValidator 
                        ID="rvAge" 
                        runat="server" 
                        ControlToValidate="txtAge" 
                        MinimumValue="18" 
                        MaximumValue="30" 
                        Type="Integer" 
                        ErrorMessage="Age must be between 18 and 30" 
                        CssClass="val-error" 
                        Display="Dynamic">&bull; Age must be between 18 and 30</asp:RangeValidator>
                </div>

                <%-- Email --%>
                <div class="form-group">
                    <label for="txtEmail">Email Address <span class="required-star">*</span></label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="input-control" placeholder="username@example.com"></asp:TextBox>
                    <asp:RequiredFieldValidator 
                        ID="rfvEmail" 
                        runat="server" 
                        ControlToValidate="txtEmail" 
                        ErrorMessage="Email is required" 
                        CssClass="val-error" 
                        Display="Dynamic">&bull; Email is required</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator 
                        ID="revEmail" 
                        runat="server" 
                        ControlToValidate="txtEmail" 
                        ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$" 
                        ErrorMessage="Invalid email format" 
                        CssClass="val-error" 
                        Display="Dynamic">&bull; Invalid email format</asp:RegularExpressionValidator>
                </div>

                <%-- Contact Number --%>
                <div class="form-group">
                    <label for="txtPhone">Contact Number <span class="required-star">*</span></label>
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="input-control" placeholder="10-digit mobile number"></asp:TextBox>
                    <asp:RequiredFieldValidator 
                        ID="rfvPhone" 
                        runat="server" 
                        ControlToValidate="txtPhone" 
                        ErrorMessage="Contact Number is required" 
                        CssClass="val-error" 
                        Display="Dynamic">&bull; Contact Number is required</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator 
                        ID="revPhone" 
                        runat="server" 
                        ControlToValidate="txtPhone" 
                        ValidationExpression="^[6-9]\d{9}$" 
                        ErrorMessage="Invalid phone number (must be 10 digits starting with 6-9)" 
                        CssClass="val-error" 
                        Display="Dynamic">&bull; Must be a valid 10-digit number</asp:RegularExpressionValidator>
                </div>

                <%-- Password --%>
                <div class="form-group">
                    <label for="txtPassword">Password <span class="required-star">*</span></label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="input-control" placeholder="Choose a password"></asp:TextBox>
                    <asp:RequiredFieldValidator 
                        ID="rfvPassword" 
                        runat="server" 
                        ControlToValidate="txtPassword" 
                        ErrorMessage="Password is required" 
                        CssClass="val-error" 
                        Display="Dynamic">&bull; Password is required</asp:RequiredFieldValidator>
                </div>

                <%-- Confirm Password --%>
                <div class="form-group">
                    <label for="txtConfirmPassword">Confirm Password <span class="required-star">*</span></label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="input-control" placeholder="Re-enter password"></asp:TextBox>
                    <asp:RequiredFieldValidator 
                        ID="rfvConfirmPassword" 
                        runat="server" 
                        ControlToValidate="txtConfirmPassword" 
                        ErrorMessage="Password confirmation is required" 
                        CssClass="val-error" 
                        Display="Dynamic">&bull; Password confirmation is required</asp:RequiredFieldValidator>
                    <asp:CompareValidator 
                        ID="cvPassword" 
                        runat="server" 
                        ControlToCompare="txtPassword" 
                        ControlToValidate="txtConfirmPassword" 
                        ErrorMessage="Passwords do not match" 
                        CssClass="val-error" 
                        Display="Dynamic">&bull; Passwords do not match</asp:CompareValidator>
                </div>

                <%-- Event Category --%>
                <div class="form-group">
                    <label for="ddlCategory">Event Category <span class="required-star">*</span></label>
                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="input-control">
                        <asp:ListItem Value="" Text="-- Choose Category --"></asp:ListItem>
                        <asp:ListItem Value="Technical" Text="Technical (Coding, Web-Dev, etc.)"></asp:ListItem>
                        <asp:ListItem Value="Cultural" Text="Cultural (Music, Dance, Drama)"></asp:ListItem>
                        <asp:ListItem Value="Sports" Text="Sports (Football, Chess, etc.)"></asp:ListItem>
                        <asp:ListItem Value="Management" Text="Management (Quiz, Pitching)"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator 
                        ID="rfvCategory" 
                        runat="server" 
                        ControlToValidate="ddlCategory" 
                        InitialValue="" 
                        ErrorMessage="Please select an event category" 
                        CssClass="val-error" 
                        Display="Dynamic">&bull; Category is required</asp:RequiredFieldValidator>
                </div>

                <%-- T-Shirt Size --%>
                <div class="form-group">
                    <label>T-Shirt Size <span class="required-star">*</span></label>
                    <div class="list-container">
                        <asp:RadioButtonList ID="rblTshirt" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal">
                            <asp:ListItem Value="S" Text="S"></asp:ListItem>
                            <asp:ListItem Value="M" Text="M" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="L" Text="L"></asp:ListItem>
                            <asp:ListItem Value="XL" Text="XL"></asp:ListItem>
                            <asp:ListItem Value="XXL" Text="XXL"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>

                <%-- Events Selection --%>
                <div class="form-group full-width">
                    <label>Specific Events to Join <span class="required-star">*</span></label>
                    <div class="list-container">
                        <asp:CheckBoxList ID="cblEvents" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal" RepeatColumns="3">
                            <asp:ListItem Value="Hackathon" Text="Hackathon"></asp:ListItem>
                            <asp:ListItem Value="Web Designing" Text="Web Designing"></asp:ListItem>
                            <asp:ListItem Value="Bug Hunt" Text="Bug Hunt"></asp:ListItem>
                            <asp:ListItem Value="Solo Dance" Text="Solo Dance"></asp:ListItem>
                            <asp:ListItem Value="Battle of Bands" Text="Battle of Bands"></asp:ListItem>
                            <asp:ListItem Value="Street Play" Text="Street Play"></asp:ListItem>
                            <asp:ListItem Value="Futsal" Text="Futsal"></asp:ListItem>
                            <asp:ListItem Value="Table Tennis" Text="Table Tennis"></asp:ListItem>
                            <asp:ListItem Value="Speed Chess" Text="Speed Chess"></asp:ListItem>
                        </asp:CheckBoxList>
                    </div>
                </div>

                <%-- Submit button --%>
                <div class="form-group full-width">
                    <asp:Button ID="btnRegister" runat="server" Text="Submit Registration" OnClick="btnRegister_Click" CssClass="btn-submit" />
                </div>

            </div>
        </form>
    </div>
</body>
</html>
