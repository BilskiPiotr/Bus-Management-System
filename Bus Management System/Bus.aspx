﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bus.aspx.cs" Inherits="Bus_Management_System.Bus" %>

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
    
    <script src="http://code.jquery.com/jquery-1.9.1.min.js""></script>
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

            var txt = "";
                txt += "<p>Total width/height: " + screen.width + "*" + screen.height + "</p>";
                txt += "<p>Available width/height: " + screen.availWidth + "*" + screen.availHeight + "</p>";
                document.getElementById("<%=lblDim.ClientID %>").innerHTML = txt;
        }

</script>


    <form id="form1" runat="server">
        <asp:ScriptManager ID="BusRefresh" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <div class="bus-header">
            <asp:Menu ID="busMenu"
                      StaticMenuStyle-CssClass="sms"
                      StaticMenuItemStyle-CssClass="smis"
                      Orientation="Horizontal" 
                      MaximumDynamicDisplayLevels="1" 
                      runat="server" 
                      OnMenuItemClick="MineMenu_MenuItemClick" 
                      StaticEnableDefaultPopOutImage="false">
                <Items>
                    <asp:MenuItem Text="Home" Value="1" Selectable="false"></asp:MenuItem>
                    <asp:MenuItem Text="Details" Value ="2" Selectable="false"></asp:MenuItem>
                    <asp:MenuItem Text="Logout" Value="3"></asp:MenuItem>
                </Items>
                <StaticMenuItemStyle CssClass="smis" HorizontalPadding="15px" />
                <StaticMenuStyle CssClass="sms" />
            </asp:Menu>
        </div>

        <div class="content">
            <asp:MultiView ID="BusManagement" runat="server">

<%-- Panel wyboru pojazdu --%>
                <asp:View ID="BusSelection" runat="server">
                    <div class="bus-Panel" style="  position: absolute;
                                                    margin: auto;
                                                    top: 0;
                                                    right: 0;
                                                    bottom: 0;
                                                    left: 0;
                                                    width: 250px;
                                                    height: 50px;
                                                    text-align: center">
                        <asp:DropDownList ID="ddl_busSelect" runat="server" CssClass="bus-DdlSelect" OnSelectedIndexChanged="Ddl_busSelect_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                        <asp:Button ID="bt_busSelect" runat="server" CssClass="bus-SingleBt" OnClick="Bt_busSelect_Click" Text="OK" Enabled="false"/>
                    </div>
                </asp:View>


<%-- Panel podstawowy operatora --%>
                <asp:View ID="Home" runat="server">
                    <asp:UpdatePanel runat="server" Id="BusHomeUP">
                        <ContentTemplate>
                            <asp:Timer runat="server" Id="BusHomeTimer" Interval="5000" OnTick="BusHomeTimer_Tick"></asp:Timer>
                                <div class="bus-row">
                                    <div class="bus-left">
                                        <asp:Table ID="busMINEtable" runat="server" CssClass="busTable" 
                                                                        BorderColor="#FF0066" 
                                                                        BorderStyle="Solid" 
                                                                        BorderWidth="10px" 
                                                                        CellPadding="5" 
                                                                        CellSpacing="5" 
                                                                        EnableTheming="False" 
                                                                        ForeColor="#FFCC66" 
                                                                        GridLines="Horizontal">

                                        <asp:TableRow runat="server">
                                            <asp:TableCell runat="server" RowSpan="5" Width="50px" BackColor="SkyBlue"></asp:TableCell>
                                            <asp:TableCell Id="R1C3" runat="server" ColSpan="3" ForeColor="DarkBlue" Font-Size="36px" Font-Bold="true" HorizontalAlign="Center">00:00:00</asp:TableCell>
                                            <asp:TableCell runat="server" RowSpan="5" Width="50px" BackColor="SkyBlue"></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow runat="server">
                                            <asp:TableCell ID="R2C2" runat="server" ForeColor="Black" Font-Size="12px" Width="">Nr Rejsu</asp:TableCell>
                                            <asp:TableCell Id="R2C3" runat="server" ForeColor="Black" Font-Size="12px">Godzina</asp:TableCell>
                                            <asp:TableCell Id="R2C4" runat="server" ForeColor="Black" Font-Size="12px">liczba PAX</asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow runat="server">
                                            <asp:TableCell runat="server"></asp:TableCell>
                                            <asp:TableCell Id="R3C3" runat="server">Rząd 3</asp:TableCell>
                                            <asp:TableCell runat="server"></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow runat="server">
                                            <asp:TableCell runat="server"></asp:TableCell>
                                            <asp:TableCell Id="R4C3" runat="server">Rząd 4</asp:TableCell>
                                            <asp:TableCell runat="server"></asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow runat="server">
                                            <asp:TableCell runat="server"></asp:TableCell>
                                            <asp:TableCell Id="R5C3" runat="server">Rząd 5</asp:TableCell>
                                            <asp:TableCell runat="server"></asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </div>
                                <div class="bus-right">
                                    <div class="bus-1stLine">
                                        <asp:Button CssClass="bus-Buttons" runat="server" Text="Button L1-B1"/>
                                        <asp:Button CssClass="bus-Buttons" runat="server" Text="Button L1-B2"/>
                                        <asp:Button CssClass="bus-Buttons" runat="server" Text="Button L1-B3"/>
                                    </div>
                                    <div class="bus-2ndLine">
                                        <asp:Button CssClass="bus-Buttons" runat="server" Text="Button L2-B1"/>
                                        <asp:Button CssClass="bus-Buttons" runat="server" Text="Button L2-B2"/>
                                        <asp:Button CssClass="bus-Buttons" runat="server" Text="Button L2-B3"/>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                            

                </asp:View>


<%-- Panel z informacjami dodatkowymi --%>
                <asp:View ID="Detail" runat="server">

                    <div class="singleCol">

                        <asp:Label ID="lblDim" runat="server" Text=""></asp:Label>
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                        <asp:HiddenField ID="HiddenField2" runat="server" />
                        <asp:HiddenField ID="HiddenField3" runat="server" />

                        <table id="busDetail-table" style="width: auto; height: auto;">
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
                </asp:View>

            </asp:MultiView>


            <div class= "bus-footer">
                <a class="right-lbl"><asp:Label ID="Label2" runat="server" Text="Zalogowano jako:  "></asp:Label></a>
            	<a class="right-lbl"><asp:Label ID="lb_loggedUser" runat="server"></asp:Label></a>
            </div>
        </div>
    </form>
</body>
</html>
