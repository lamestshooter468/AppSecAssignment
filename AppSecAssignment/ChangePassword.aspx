<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="AppSecAssignment.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="errorLbl" runat="server" Visible="false" ForeColor="Red"></asp:Label>
        </div>
        <div>
            <asp:Label ID="Label1" runat="server">Current Password</asp:Label>
            <asp:TextBox ID="curr_pass" runat="server" TextMode="Password"></asp:TextBox>
        </div>
        <div>
            <asp:Label ID="Label2" runat="server">New Password</asp:Label>
            <asp:TextBox ID="new_pass" runat="server" TextMode="Password"></asp:TextBox>
        </div>
        <div>
            <asp:Label ID="Label3" runat="server">Confirm Password</asp:Label>
            <asp:TextBox ID="pass_retype" runat="server" TextMode="Password"></asp:TextBox>
        </div>
        <div>
            <asp:Button ID="ChgedPass" runat="server" Text="Change Password" OnClick="ChgedPass_Click" />
        </div>
    </form>
</body>
</html>
