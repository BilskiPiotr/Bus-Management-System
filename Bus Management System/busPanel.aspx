<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="busPanel.aspx.cs" Inherits="Bus_Management_System.busPanel" %>

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
<script >
    setInterval(getLocation, 5000);

function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showCoordinates, function () { }, { enableHighAccuracy: true });
    }
    else {
        lat.innerHTML = "Ta przeglądarka nie obsługuje geolokacji.";
    }
}

function showCoordinates(coordinates) {

    document.getElementById("<%=lb_Latitude.ClientID %>").innerHTML = coordinates.coords.latitude;
    document.getElementById("<%=lb_Longitude.ClientID %>").innerHTML = coordinates.coords.longitude;
    document.getElementById("<%=lb_Accuracy.ClientID %>").innerHTML = coordinates.coords.accuracy;
}

function ShowCurrentTime() {
    PageMethods.GetCurrentTime(document.getElementById("<%=tb_JakiesDane.ClientID%>").value, OnSuccess);
}

function OnSuccess(response, userContext, methodName) {
    document.getElementById("<%=lb_ActualTime.ClientID %>").innerHTML = response;
}
</script>
    
    <div class="content">
        <form id="login_form" runat="server" enctype="multipart/form-data">
            <div class="header">
                <div class="topnav">
                    <asp:Menu   ID="mineMenu" 
                                CssClass="mine"
                                StaticMenuStyle-CssClass="sms"
                                StaticMenuItemStyle-CssClass="smis"
                                Orientation="Horizontal" 
                                MaximumDynamicDisplayLevels="1" 
                                runat="server" 
                                OnMenuItemClick="MineMenu_MenuItemClick" StaticEnableDefaultPopOutImage="false">
                        <StaticMenuItemStyle />

                        <Items>
                            <asp:MenuItem Text="Home" Value="1"></asp:MenuItem>
                            <asp:MenuItem Text="Detail" Value="2"></asp:MenuItem>
                            <asp:MenuItem Text="Airport" Value="3"></asp:MenuItem>
                            <asp:MenuItem Text="Country" Value="4"></asp:MenuItem>
                            <asp:MenuItem Text="Employee" Value="5"></asp:MenuItem>
                            <asp:MenuItem Text="Vehicle" Value="6"></asp:MenuItem>
                            <asp:MenuItem Text="Logout" Value="7"></asp:MenuItem>
                        </Items>

                        <StaticMenuItemStyle CssClass="smis" HorizontalPadding="15px" />
                        <StaticMenuStyle CssClass="sms" />
                    </asp:Menu>
                </div>
                <div class="siteInfo">
                    <a class="right-lbl"><asp:Label ID="lb_loggedUser" runat="server"></asp:Label></a>
                    <a class="right-lbl"><asp:Label ID="lb_loggedUserDesc" runat="server" Text="Zalogowano jako:  "></asp:Label></a>
                </div>
            </div>


        <div>
        <asp:MultiView ID="BusManagement" runat="server" ActiveViewIndex="0">



            <asp:View ID="Admin" runat="server">
                <div class="singleCol">
                    <table id="adminTable" style="width: auto; height: auto;">
                        <tr>
                            <td>Panel Administratora</td>
                        </tr>
                    </table>
                </div>
            </asp:View>



            <asp:View ID="About" runat="server">
                <div class="singleCol">
                    <table>
                        <tr>
                            <td class="odstep" >&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="auto-style6"><asp:Label ID="Label1" runat="server" Text="Internetowy"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="auto-style6"><asp:Label ID="Label5" runat="server" Text="SYSTEM ZARZĄDZANIA PRACOWNIKAMI"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="odstep" >&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="auto-style5"><asp:Label ID="Label2" runat="server" Text="opracowany na potrzeby zaliczenia przedmiotu:"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="odstep" >&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="auto-style5"><asp:Label ID="Label3" runat="server" Text="PROJEKTOWANIE WIELOWARSTWOWYCH SERWISOW INTERNETOWYCH"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="odstep" >&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="auto-style5"><asp:Label ID="Label4" runat="server" Text="zaimplementowano: Logowanie, zemienną sesyjną typu 'Object', 3 warstwową architekturę, walidację kontrolek TextBox dla wartości 'numeric', ect" Font-Italic="True" ForeColor="#660066" Width="300px"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="odstep" >&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="auto-style5"><asp:Button ID="Button4" runat="server" Text="potwierdzenie przeczytania" OnClick="Bt_error_Click1" /></td>
                        </tr>
                        <tr>
                            <td class="odstep" >&nbsp;</td>
                        </tr>
                    </table>
                </div>
            </asp:View>



            <asp:View ID="Airport" runat="server">
                <div class="singleCol">
                    <table id="airport-table" style="width: auto; height: auto;">
                        <tr>
                            <td>Panel Airport</td>
                        </tr>
                    </table>
                </div>
            </asp:View>



            <asp:View ID="Country" runat="server">
                <div class="singleCol">
                    <table id="country-table" style="width: auto; height: auto;">
                        <tr>
                            <td>Panel Country</td>
                        </tr>
                    </table>
                </div>
            </asp:View>



            <asp:View ID="Employee" runat="server">
                <div class="singleCol">
                    <table id="employee-table" style="width: auto; height: auto;">
                        <tr>
                            <td>Panel Employee</td>
                        </tr>
                    </table>
                </div>
            </asp:View>



            <asp:View ID="Vehicle" runat="server">
                <div class="singleCol">
                    <table id="vechicle-table" style="width: auto; height: auto;">
                        <tr>
                            <td>Panel Vehicle</td>
                        </tr>
                    </table>
                </div>
            </asp:View>




            <asp:View ID="Bus" runat="server">
<div style="text-align:center">
  <h2>Responsive Zig Zag Layout Example</h2>
  <p>Resize the browser window to see the effect.</p>
</div>

<!-- The App Section -->
<div class="container">
  <div class="row">
    <div class="column-66">
      <h1 class="xlarge-font"><b>The App</b></h1>
      <h1 class="large-font" style="color:MediumSeaGreen;"><b>Why buy it?</b></h1>
      <p><span style="font-size:36px">Take photos like a pro.</span> You should buy this app because lorem ipsum consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
      <button class="button">Download Application</button>
    </div>
    <div class="column-33">
        <img src="/w3images/img_app.jpg" width="335" height="471" />
    </div>
  </div>
</div>

<!-- Clarity Section -->
<div class="container" style="background-color:#f1f1f1">
  <div class="row">
    <div class="column-33">
      <img src="/w3images/app5.jpg" alt="App" width="335" height="471" />
    </div>
    <div class="column-66">
      <h1 class="xlarge-font"><b>Clarity</b></h1>
      <h1 class="large-font" style="color:red;"><b>Pixels, who?</b></h1>
      <p><span style="font-size:24px">A revolution in resolution.</span> Sharp and clear photos with the world's best photo engine, incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquipex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.</p>
      <button class="button" style="background-color:red">Read More</button>
    </div>
  </div>
</div>

<!-- The App Section -->
<div class="container">
  <div class="row">
    <div class="column-66">
      <h1 class="xlarge-font"><b>The App</b></h1>
      <h1 class="large-font" style="color:MediumSeaGreen;"><b>Why buy it?</b></h1>
      <p><span style="font-size:36px">Take photos like a pro.</span> You should buy this app because lorem ipsum consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.</p>
      <button class="button">Download Application</button>
    </div>
    <div class="column-33">
        <img src="/w3images/img_app.jpg" width="335" height="471" />
    </div>
  </div>
</div>
            </asp:View>



            <asp:View ID="BusDetail" runat="server">
                <div class="singleCol">
                    <table id="busDetail-table" style="width: auto; height: auto;">
                        <tr>
                            <td>Panel Detali Kierowcy</td>
                        </tr>
                    </table>
                </div>

                <h2>Szablon 4-ro kolumnowy responsive</h2>
                <p><strong>Zmiany zależna od szerokości okna przeglądarki.</strong> Dla rozdzielczości (szerokośc) 992px lub mniej, liczba kolumn zmniejszy się z 4 do 2. Dla rozdzielczości (szerokości ekranu) 600px lub mniej, wszystkie kolumny ułożą sie jedna nad drugą (powstanie 1 kolumna!.</p>

                <div class="row">
                    <div class="column" style="background-color:#aaa;">
                        <h2>Column 1</h2>
                        <p>..........</p>
                        <table id="c1-table" style="width: auto; height: auto;">
                            <tr>
  					            <th colspan="2">Współrzędne</th>
  				            </tr>
                            <tr>
                                <td class="odstep" colspan="2"></td>
                            </tr>
                            <tr>
                                <td class="dane-C1"><asp:Label ID="lb_OpisLatitude" runat="server" Text="Latitude :"></asp:Label></td>
                                <td class="dane-C2"><asp:Label ID="lb_Latitude" runat="server" Text=" 00000000 " Width="150px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="odstep" colspan="2"></td>
                            </tr>
                            <tr>
                                <td class="dane-C1"><asp:Label ID="lb_OpisLongitude" runat="server" Text="Longitude :"></asp:Label></td>
                                <td class="dane-C2"><asp:Label ID="lb_Longitude" runat="server" Text=" 00000000 " Width="150px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="odstep" colspan="2"></td>
                            </tr>
                            <tr>
                                <td class="dane-C1"><asp:Label ID="lb_OpisAccuracy" runat="server" Text="Akuracy :"></asp:Label></td>
                                <td class="dane-C2"><asp:Label ID="lb_Accuracy" runat="server" Text=" 00000000 " Width="150px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="odstep" colspan="2"></td>
                            </tr>
                            <tr>
                                <td class="dane-C1"><asp:Label ID="lb_PodajImie" runat="server" Text="Podaj Imie :"></asp:Label></td>
                                <td class="dane-C2"><asp:TextBox ID="tb_JakiesDane" runat="server" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="odstep" colspan="2"></td>
                            </tr>
                            <tr>
                                <td class="dane-C1"><asp:Label ID="lb_PobranyCzas" runat="server" Text="Czas Serwera :"></asp:Label></td>
                                <td class="dane-C2"><asp:Label ID="lb_ActualTime" runat="server" Width="150px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="odstep" colspan="2"></td>
                            </tr>
                            <tr>
                                <asp:ScriptManager runat="server" id="ScriptManager1"/>
                                    <asp:UpdatePanel runat="server" id="UpdatePanel2">
                                        <ContentTemplate>
                                            <asp:Timer runat="server" id="Timer2" Interval="10000" OnTick="Timer1_Tick"></asp:Timer>
                                            <asp:Label runat="server" Text="Długość Geograficzna" id="Label6"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                            </tr>
                            <tr>
                                <td class="odstep" colspan="2"></td>
                            </tr>
                        </table>
                    </div>
                    <div class="column" style="background-color:#bbb;">
                        <h2>Column 2</h2>
                        <p>................</p>
                    </div>
                    <div class="column" style="background-color:#ccc;">
                        <h2>Column 3</h2>
                        <p>................</p>
                    </div>
                    <div class="column" style="background-color:#ddd;">
                        <h2>Column 4</h2>
                        <p>................</p>
                    </div>
                </div>
            </asp:View>




            <asp:View ID="Alocator" runat="server">
                <div id="alocator">
                    <div >
                        <table style="width: 100%;" cellpadding="2">
                            <tbody>
                                <tr>
                                    <td class="center"><asp:Label ID="lb_Vehicle001" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle002" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle003" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle004" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle005" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle006" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle007" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle008" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle009" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle010" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle011" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle012" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="center"><asp:Label ID="lb_Vehicle013" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle014" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle015" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle016" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle017" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle018" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle019" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle020" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle021" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle022" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle023" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle024" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <table id="alocator-table" style="width: auto; height: auto;">
                        <tr>
                            <td>
                                @NSTB
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:View>



            <asp:View ID="_ErrorPage" runat="server">
                <div class="singleCol">
                <br />
                    Potwierdzenie wprowadzenia danych nowego użytkownika  
                <br />
                    <p><asp:Label ID="lb_error" runat="server" Text="Label"></asp:Label></p>
                <br />
                    <p><asp:Button class="singleBt" ID="bt_errorConfirm" runat="server" Text="Powrót" OnClick="Bt_errorConfirm_Click"/></p>
                <br />
                </div>
            </asp:View>


            </asp:MultiView>

        </div>
        </form>
    </div>

    <div class= "footer">
		<p>Projekt Inżynierski - Piotr Bilski - index 43335</p>
    </div>
</body>
</html>
