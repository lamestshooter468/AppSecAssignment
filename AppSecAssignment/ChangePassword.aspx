<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="AppSecAssignment.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 174px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="errorLbl" runat="server" Visible="false" ForeColor="Red"></asp:Label>
        </div>
        <table>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label1" runat="server">Current Password</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="curr_pass" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label2" runat="server">New Password</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="new_pass" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="regex" runat="server" ControlToValidate="tb_pass" ValidationExpression="^[A-Za-z\d]{8,}$" ErrorMessage="Password must contain: Minimum 8 characters at least 1 Alphabet and 1 Number" ForeColor="Red"/>
                </td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label3" runat="server">Confirm Password</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="pass_retype" runat="server" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div>
            <asp:Button ID="ChgedPass" runat="server" Text="Change Password" OnClick="ChgedPass_Click" />
        </div>
    </form>
</body>
</html>
