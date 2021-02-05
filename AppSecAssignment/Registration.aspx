<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="AppSecAssignment.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 74%;
            height: 169px;
        }
        .auto-style3 {
            width: 185px;
        }
        .auto-style4 {
            width: 185px;
            height: 28px;
        }
        .auto-style5 {
            height: 28px;
        }
        .auto-style6 {
            width: 185px;
            height: 34px;
        }
        .auto-style7 {
            height: 34px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label ID="lbl_error" runat="server" Visible="false" ForeColor="Red" />
        <table class="auto-style1">
            <tr>
                <td class="auto-style3">
                    <asp:Label ID="Label1" runat="server" Text="First Name:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_fname" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">
                    <asp:Label ID="Label2" runat="server" Text="Last Name:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_lname" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style4">
                    <asp:Label ID="Label5" runat="server" Text="Email:"></asp:Label>
                </td>
                <td class="auto-style5">
                    <asp:TextBox ID="tb_email" runat="server"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="Email_Regex" runat="server" ControlToValidate="tb_email" ErrorMessage="Email must contain &quot;@&quot;, and &quot;.com&quot;, &quot;.sg&quot;, &quot;.uk&quot;, etc." ForeColor="Red" ValidationExpression="^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="auto-style6">
                    <asp:Label ID="Label6" runat="server" Text="Password:"></asp:Label>
                </td>
                <td class="auto-style7">
                    <asp:TextBox ID="tb_pass" runat="server" TextMode="Password" ></asp:TextBox>
                    <asp:RegularExpressionValidator ID="regex" runat="server" ControlToValidate="tb_pass" ValidationExpression="^[A-Za-z\d]{8,}$" ErrorMessage="Password must contain: Minimum 8 characters at least 1 Alphabet and 1 Number" ForeColor="Red"/>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">
                    <asp:Label ID="Label3" runat="server" Text="CNo:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_CNo" runat="server" TextMode="Number"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="CNo_Regex" runat="server" ControlToValidate="tb_CNo" ErrorMessage="Card Number must be 15 or 16 numbers long" ForeColor="Red" ValidationExpression="^[\d]{15,16}$"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">
                    <asp:Label ID="Label4" runat="server" Text="CVV:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_CVV" runat="server" TextMode="Password" MaxLength="3"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="CVV_Regex" runat="server" ControlToValidate="tb_CVV" Display="Dynamic" ErrorMessage="CVV must be 3 numbers long" ForeColor="Red" ValidationExpression="^[0-9]{3}$"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">
                    <asp:Label ID="Label7" runat="server" Text="Date Of Birth:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tb_dob" runat="server" TextMode="Date"></asp:TextBox>
                </td>
            </tr>
        </table>
        <asp:Button ID="registerBtn" runat="server" Text="Register" OnClick="registerBtn_Click" />
    </form>
    
</body>
</html>
