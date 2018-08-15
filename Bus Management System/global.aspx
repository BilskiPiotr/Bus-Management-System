<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="global.aspx.cs" Inherits="Bus_Management_System.global" %>

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
        <form id="login_form" runat="server" DefaultButton="bt_submitLogin">
            <div class="header">
                <div class="topnav">

                </div>
                <div class="siteInfo">
                    <a class="right-lbl"><asp:Label ID="lb_errorMsg" runat="server"></asp:Label></a>
                </div>
            </div>


            <div class="singleCol">
                <table>
  				    <tr>
  					    <th colspan="2">LOGOWANIE</th>
  				    </tr>
                    <tr>
                        <td class="odstep" colspan="2"></td>
                    </tr>
                    <tr>
                        <td class="dane-C1"><asp:Label ID="lb_name" runat="server" Text="Name :"></asp:Label></td>
                        <td class="dane-C2"><asp:TextBox ID="inp_name" runat="server" Width="150px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="odstep" colspan="2"></td>
                    </tr>
                    <tr>
                        <td class="dane-C1"><asp:Label ID="lb_2ndName" runat="server" Text="2'nd Name :"></asp:Label></td>
                        <td class="dane-C2"><asp:TextBox ID="inp_2ndName" runat="server" Width="150px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="odstep" colspan="2"></td>
                    </tr>
                    <tr>
                        <td class="dane-C1"><asp:Label ID="lb_pesel" runat="server" Text="PESEL :"></asp:Label></td>
                        <td class="dane-C2"><asp:TextBox ID="inp_pesel" runat="server" TextMode="Password" Width="150px" MaxLength="11" onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="odstep" colspan="2"></td>
                    </tr>
                    <tr>                    
                        <td colspan="2">
                            <asp:Button class="dualBt" ID="bt_resetLogin" runat="server" Text="Reset" TabIndex="1" OnClick="Bt_resetLogin_Click" />
                            <asp:Button class="dualBt" ID="bt_submitLogin" runat="server" Text="Login" OnClick="Bt_submitLogin_Click"/>
                        </td>
                    </tr>
                </table>
            </div>
        </form>
    </div>

    <div class= "footer">
		<p>Projekt Inżynierski - Piotr Bilski - index 43335</p>
	</div>
</body>
</html>
