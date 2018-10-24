<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="busPanel.aspx.cs" Inherits="Bus_Management_System.busPanel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SysBus Management System</title>
	<meta http-equiv="content-type" content="text/html"; charset="utf-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="SysBus_Management" content:"" />
	<link rel="stylesheet" href="css/style.css" />
    <style type="text/css">
        .auto-style1 {
            height: 4px;
        }
    </style>
</head>
<body runat="server" id="BodyTag">
<%--    <script type="text/javascript">

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
</script>--%>
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

                <div class="bus-Panel">
	                <div class="busHeader">
                        <p> nagłówek </p>
	                </div>
	                <div class="busLeft">
                        <p>lewa część ekranu </p>
	                </div>
	                <div class="busRight">
		                <div class="busRightTop">
<%--                            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                            <asp:HiddenField ID="HiddenField2" runat="server" />
                            <asp:HiddenField ID="HiddenField3" runat="server" />--%>

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
                        <table class="alocator_TopTable">
                            <tbody>
                                <tr>
                                    <td class="center"><asp:Label ID="lb_Vehicle1" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle2" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle3" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle4" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle5" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle6" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle7" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle8" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle9" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle10" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle11" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle12" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="center"><asp:Label ID="lb_Vehicle13" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle14" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle15" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle16" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle17" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle18" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle19" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle20" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle21" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle22" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle23" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                    <td class="center"><asp:Label ID="lb_Vehicle24" runat="server" Text="- - -" style="text-align: center; background-color: #999999;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="alocator-Panel">
                        <div class="alocator-Left">
                            <table id="alocator-LeftTable">
                                <tr class="tr_26">
                                    <th colspan="4">ZARZĄDZANIE</th>
                                </tr>
                                <tr class="tr_26">
                                    <td class="td_center">
                                        <asp:Label ID="lb_AlocatorBusOpis" runat="server" EnableViewState="false" Text="Free Bus :"></asp:Label>
                                    </td>
                                    <td class="td_center">
                                        <asp:Label ID="lb_AlocatorBusCount" runat="server" EnableViewState="false" Text="000"></asp:Label>
                                    </td>
                                    <td class="td_center">
                                        <asp:Label ID="lb_AlocatorDate" runat="server" EnableViewState="false" Text="DATA"></asp:Label>
                                    </td>
                                    <td class="td_center">
                                        <asp:Label ID="lb_AlocatorHour" runat="server" EnableViewState="false" Text="GODZINA"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="tr_26">
                                    <td colspan="2" class="td_center">
                                        <asp:RadioButton ID="rb_Przylot" runat="server" Text="Przylot" Checked="True" OnCheckedChanged="Rb_Przylot_CheckedChanged" />
                                    </td>
                                    <td colspan="2" class="td_center">
                                        <asp:RadioButton ID="rb_Odlot" runat="server" Text="Odlot" OnCheckedChanged="Rb_Odlot_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr class="tr_26">
                                    <td class="td_left">
                                        <asp:Label ID="lb_AlocatorFNb1" runat="server" Text="Flight Nb :"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="table_Tb" ID="tb_AlocatorFNb" runat="server" AutoCompleteType="Disabled" MaxLength="7"></asp:TextBox>
                                    </td>
                                    <td class="td_left">
                                        <asp:Label ID="lb_Pax" runat="server" Text="Pax"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="table_Tb" ID="tb_Pax" runat="server" AutoCompleteType="Disabled" MaxLength="3" onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="tr_26">
                                    <td class="td_center">
                                        <asp:Label ID="lb_Port" runat="server" Text="Port"></asp:Label>
                                    </td>
                                    <td class="td_center">
                                        <asp:Label ID="lb_PPS" runat="server" Text="PPS"></asp:Label>
                                    </td>
                                    <td class="td_center">
                                        <asp:Label ID="lb_Gate" runat="server" Text="Gate"></asp:Label>
                                    </td>
                                    <td class="td_center">
                                        <asp:Label ID="lb_Bus" runat="server" Text="Bus" ></asp:Label>
                                    </td>
                                </tr>
                                <tr class="tr_90">
                                    <td class="td_center td_top">
                                        <asp:DropDownList   CssClass="ddl" ID="ddl_Port" runat="server"
                                                            onMouseOver="this.size=5;"
                                                            onClick="this.size=1;" 
                                                            onMouseOut="this.size=1;">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="td_center td_top">
                                        <asp:DropDownList   CssClass="ddl" ID="ddl_PPS" runat="server"
                                                            onMouseOver="this.size=5;"
                                                            onClick="this.size=1;"
                                                            onMouseOut="this.size=1;">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="td_center td_top">
                                        <asp:DropDownList   CssClass="ddl" ID="ddl_Gate" runat="server"
                                                            onMouseOver="this.size=5;"
                                                            onClick="this.size=1;" 
                                                            onMouseOut="this.size=1;">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="td_center td_top">
                                        <asp:DropDownList   CssClass="ddl" ID="ddl_Bus" runat="server"
                                                            onMouseOver="this.size=5;"
                                                            onClick="this.size=1;" 
                                                            onMouseOut="this.size=1;">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr class="tr_26">
                                    <td class="td_left">
                                        <asp:Label ID="lb_RadioNeon" runat="server" Text="Neon"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="table_Tb" ID="tb_RadioNeon" runat="server" AutoCompleteType="Disabled" MaxLength="3" onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;"></asp:TextBox>
                                    </td>
                                    <td class="td_left">
                                        <asp:Label ID="lb_RadioGate" runat="server" Text="Gate"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox CssClass="table_Tb" ID="tb_RadioGate" runat="server" AutoCompleteType="Disabled" MaxLength="3" onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="tr_26">
                                    <td colspan="2">
                                        <asp:Button CssClass="table_Bt" ID="bt_AlocatorReset" runat="server" Text="CLEAR" OnClick="Bt_AlocatorReset_Click" />
                                    </td>
                                    <td colspan="2">
                                        <asp:Button CssClass="table_Bt" ID="bt_AlocatorAccept" runat="server" Text="ACCEPT" OnClick="Bt_AlocatorAccept_Click" />
                                    </td>
                                </tr>

                            </table>
                        </div>
                        <div class ="alocator-Right">
                            <table id="alocator-RightTable">
                                <tr>
                                    <td class="center">
                                        <asp:GridView ID="gv_OperationList" runat="server" AutoGenerateColumns="false" AllowSorting="True" EnableSortingAndPagingCallbacks="True">
                                            <Columns>
                                                <asp:BoundField DataField="airPort" HeaderText="Air Port" ItemStyle-Width="30px" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="flightNb" HeaderText="Rejs No:" ItemStyle-Width="30px" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="operationId" HeaderText="Operation" ItemStyle-Width="30px" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="gate" HeaderText="Gate" ItemStyle-Width="30px" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="pps" HeaderText="PPS" ItemStyle-Width="30px" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="paxCount" HeaderText="Pax" ItemStyle-Width="30px" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="radioNeon" HeaderText="Neon Radio" ItemStyle-Width="30px" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="radioGate" HeaderText="Gate Radio" ItemStyle-Width="30px" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="created" HeaderText="Begin" ItemStyle-Width="30px" >
                                                </asp:BoundField>
                                                <asp:BoundField DataField="bus" HeaderText="Bus" ItemStyle-Width="30px" >
                                                </asp:BoundField>
                                            </Columns>
                                            <HeaderStyle BackColor="#FF6600" Font-Bold="True" Font-Names="Tahoma" Font-Size="Small" ForeColor="#FFFFCC" />
                                        </asp:GridView>
                                    </td>
                                </tr> 
                                <tr>
                                    <td class="center">
                                        <asp:Button CssClass="table_Bt" ID="Button1" runat="server" Text="ACCEPT" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </asp:View>



            <asp:View ID="_ErrorPage" runat="server">
                <div class="singleCol">
                <br />
                    Potwierdzenie wprowadzenia danych nowego użytkownika  
                <br />
                    <p><asp:Label ID="lb_error" runat="server" Text="Label"></asp:Label></p>
                <br />
                    <p><asp:Button CssClass="singleBt" ID="bt_errorConfirm" runat="server" Text="Powrót" OnClick="Bt_errorConfirm_Click"/></p>
                <br />
                </div>
            </asp:View>


            </asp:MultiView>
        </form>

    <div class= "footer">
		<p>Projekt Inżynierski - Piotr Bilski - index 43335</p>
    </div>
</body>
</html>
