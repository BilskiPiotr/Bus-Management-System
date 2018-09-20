<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bus.aspx.cs" Inherits="Bus_Management_System.Bus" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SysBus Management System</title>
	<meta http-equiv="content-type" content="text/html"; charset="utf-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="SysBus_Management" content:"" />
	<link rel="stylesheet" href="css/bus.css" />
</head>
<body runat="server" id="BodyTag">
    <script type="text/javascript">

        var wartosc1 = "";
        var wartosc2 = "";
        var wartosc3 = "";
        var error = "";
        var dane = new Array();

        setInterval(getLocation, 5000);

        function getLocation() {
            if (navigator.geolocation)
            {
                navigator.geolocation.getCurrentPosition(KonstruujArray, function () { }, { enableHighAccuracy: true });
            }
            else {
                error.innerHTML = "Ta przeglądarka nie obsługuje geolokacji.";
            }
        }

        function KonstruujArray(coordinates) {
            wartosc1 = coordinates.coords.latitude;
            wartosc2 = coordinates.coords.longitude;
            wartosc3 = coordinates.coords.accuracy;
            document.getElementById("<%=HiddenField1.ClientID%>").value = wartosc1;
            document.getElementById("<%=HiddenField2.ClientID%>").value = wartosc2;
            document.getElementById("<%=HiddenField3.ClientID%>").value = wartosc3;
            dane[0] = document.getElementById("<%=HiddenField1.ClientID%>").value;
            dane[1] = document.getElementById("<%=HiddenField2.ClientID%>").value;
            dane[2] = document.getElementById("<%=HiddenField3.ClientID%>").value;
            PageMethods.PrzeliczArray(dane, OnSuccess);
        }
        function OnSuccess(response, userContext, methodName) {
            document.getElementById("<%=lb_BusLatitude.ClientID %>").innerHTML = response[0];
            document.getElementById("<%=lb_BusLongitude.ClientID %>").innerHTML = response[1];
            document.getElementById("<%=lb_BusDistance.ClientID %>").innerHTML = response[2];
            document.getElementById("<%=lb_BusAccuracy.ClientID %>").innerHTML = wartosc3;
            wartosc1 = "";
            wartosc2 = "";
            wartosc3 = "";
        }
</script>
    <form id="form1" runat="server">
        <div class="header">
            <div class="topnav">
                <asp:Menu   ID="busMenu"
                            StaticMenuStyle-CssClass="sms"
                            StaticMenuItemStyle-CssClass="smis"
                            Orientation="Horizontal" 
                            MaximumDynamicDisplayLevels="1" 
                            runat="server" 
                            OnMenuItemClick="MineMenu_MenuItemClick" 
                            StaticEnableDefaultPopOutImage="false">
                    <StaticMenuItemStyle />
                        <Items>
                            <asp:MenuItem Text="Home" Value="1"></asp:MenuItem>
                            <asp:MenuItem Text="Details" Value ="2"></asp:MenuItem>
                            <asp:MenuItem Text="Logout" Value="3"></asp:MenuItem>
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
        <div id="bus">
            <asp:MultiView ID="BusManagement" runat="server">


                <asp:View ID="Home" runat="server">

                    <div class="bus-Panel">
	                    <div class="busHeader">
                            <p> nagłówek </p>
	                    </div>
	                    <div class="busLeft">
                            <p>lewa część ekranu </p>
	                    </div>
	                    <div class="busRight">
		                    <div class="busRightTop">
                                <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                <asp:HiddenField ID="HiddenField2" runat="server" />
                                <asp:HiddenField ID="HiddenField3" runat="server" />

                                <table id="currentLoc-table" style="width: auto; height: auto;">
                                    <tr>
                                        <td class="dane-C1"><asp:Label ID="lb_LatOpis" runat="server" Text="Lat :"></asp:Label></td>
                                        <td class="dane-C2"><asp:Label ID="lb_BusLatitude" runat="server" Text="" Width="100px"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="odstep" colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td class="dane-C1"><asp:Label ID="lb_LonOpis" runat="server" Text="Lon :"></asp:Label></td>
                                        <td class="dane-C2"><asp:Label ID="lb_BusLongitude" runat="server" Text="" Width="100px"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="odstep" colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td class="dane-C1"><asp:Label ID="lb_DisOpis" runat="server" Text="Dis :"></asp:Label></td>
                                        <td class="dane-C2"><asp:Label ID="lb_BusDistance" runat="server" Text="" Width="100px"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="odstep" colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td class="dane-C1"><asp:Label ID="lb_AccOpis" runat="server" Text="Acc :"></asp:Label></td>
                                        <td class="dane-C2"><asp:Label ID="lb_BusAccuracy" runat="server" Text="" Width="100px"></asp:Label></td>
                                    </tr>
                                </table>
		                    </div>		                      
                            <div class="busRightMiddle">
                                <p>prawa middle</p>
		                    </div>
		                    <div class="busRightBottom">
                                <p>prawa bottom</p>
                            </div>
	                    </div>
                    </div>

                </asp:View>


                <asp:View ID="Detail" runat="server">

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
                                    <asp:UpdatePanel runat="server" id="UpdatePanel2">
                                        <ContentTemplate>

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

            </asp:MultiView>

        </div>
        <div class= "footer">
		    <p>Projekt Inżynierski - Piotr Bilski - index 43335</p>
        </div>
    </form>
</body>
</html>
