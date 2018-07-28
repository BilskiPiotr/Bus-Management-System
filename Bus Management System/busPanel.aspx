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
        <script>
            var lat = document.getElementById("Latitude");
            var lon = document.getElementById("Longitude");

            setInterval(getLocation, 5000);

        function getLocation()
        {
            if (navigator.geolocation)
            {
                navigator.geolocation.getCurrentPosition(showCoordinates, function () { }, {enableHighAccuracy: true});
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

        function showError(error) {
            switch (error.code) {
                case error.PREMISSION_DENIED:
                    lat.innerHTML = "Użytkownik odmówił pobrania lokalizacji"
                    break;
                case error.POSITION_UNAVAILABLE:
                    lat.innerHTML = "Informacje o lokalizacji są niedostępne"
                    break;
                case error.TIMEOUT:
                    lat.innerHTML = "Przekroczenie czasu oczekiwania na pobranie współrzędnych"
                    break;
                case error.UNKNOWN_ERROR:
                    lat.innerHTML = "Wystąpił nieznany błąd"
                    break;
            }
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
                <div class="singleCol">
                    <table id="bus-table" style="width: auto; height: auto;">
                        <tr>
                            <td>Panel Kierowcy</td>
                        </tr>
                    </table>
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
                                <td class="dane-C2" colspan="2"><asp:Button ID="bt_GetPosition" runat="server" Text=" GetPosition " Width="150px" OnClientClick="getLocation()" OnClick="bt_GetPosition_Click"></asp:Button></td>
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
                <div class="singleCol">
                    <table id="alocator-table" style="width: auto; height: auto;">
                        <tr>
                            <td>Panel Alokatora</td>
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
