<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AppSecAssignment.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 43%;
        }
        .auto-style2 {
            width: 152px;
        }
    </style>
    <script src="https://www.google.com/recaptcha/api.js?render=6LdO5DYaAAAAAIbDWGw6w5L6Kfk_FT48OTsh9MqN"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lbl_error" runat="server" Visible="false" ForeColor="Red" />
            <table class="auto-style1">
                <tr>
                    <td class="auto-style2">
                        <asp:Label ID="Label1" runat="server" Text="Email:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tb_email" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <asp:Label ID="Label2" runat="server" Text="Password:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tb_pass" runat="server" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
            <asp:Button ID="loginBtn" runat="server" Text="Login" OnClick="loginBtn_Click" />
        </div>
        
    </form>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LdO5DYaAAAAAIbDWGw6w5L6Kfk_FT48OTsh9MqN', { action: "Login" }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
    
</body>
</html>
