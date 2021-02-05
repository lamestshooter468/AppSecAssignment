<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="AppSecAssignment.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 28px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Name" runat="server"></asp:Label>
        </div>
        <div class="auto-style1">
            
            <asp:Button ID="chgPassBtn" runat="server" OnClick="chgPassBtn_Click" Text="Change Password" />
            <br />
            <asp:Button ID="logOutBtn" runat="server" OnClick="logOutBtn_Click" Text="Log Out" />
            
        </div>
        
    </form>
</body>
</html>
