<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="about.aspx.cs" Inherits="Bus_Management_System.about" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SysBus Management System</title>
	<meta http-equiv="content-type" content="text/html"; charset="utf-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="SysBus_Management" content:"" />
	<link rel="stylesheet" href="css/style.css" />
</head>
<body>
    <div class="content">
        <form id="login_form" runat="server" >
            <div class="header">
                <div class="topnav">
                    <asp:Menu   ID="loginMenu" 
                                CssClass="login"
                                StaticMenuStyle-CssClass="sms"
                                StaticMenuItemStyle-CssClass="smis"
                                StaticHoverStyle-CssClass="menuHover" 
                                DynamicHoverStyle-CssClass="menuHover"
                                Orientation="Horizontal" 
                                MaximumDynamicDisplayLevels="0" 
                                runat="server">
                        <Items>
                            <asp:MenuItem NavigateUrl="~/global.aspx" Text="Home" Value="Home"></asp:MenuItem>
                            <asp:MenuItem NavigateUrl="~/kontakt.aspx" Text="Kontakt" Value="Kontakt"></asp:MenuItem>
                            <asp:MenuItem NavigateUrl="~/about.aspx" Selected="True" Text="About" Value="About"></asp:MenuItem>
                        </Items>
                    </asp:Menu>
                </div>
            </div>
            <div>
                <p> Strona About - niezależna od serwisu</p>
                <a href="testmenu.aspx">menu testowe</a>
            </div>
        </form>
    </div>

    <div class= "footer">
		<p>Projekt Inżynierski - Piotr Bilski - index 43335</p>
	</div>
</body>
</html>
