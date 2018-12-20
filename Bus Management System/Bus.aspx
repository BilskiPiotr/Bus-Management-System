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
<body runat="server" id="BodyTag" onclick="clicked=true;">
    
<script src="http://code.jquery.com/jquery-1.9.1.min.js""></script>
<script type="text/javascript">
    {
        var lat = "";
        var lon = "";
        var acc = "";
        var spe = "";

        var error = "";
        var dane = new Array();

        setInterval(getLocation, 1000);

        function getLocation() 
        {
            if (navigator.geolocation)
            {
                //navigator.geolocation.getCurrentPosition(KonstruujArray, function () { }, { enableHighAccuracy: true });
                navigator.geolocation.watchPosition(KonstruujArray, function () { }, { enableHighAccuracy: true });
            }
            else 
            {
                error.innerHTML = "Ta przeglądarka nie obsługuje geolokacji.";
            }
        }

        function KonstruujArray(coordinates) 
        {
            lat = coordinates.coords.latitude;
            lon = coordinates.coords.longitude;
            acc = coordinates.coords.accuracy;
            spe = coordinates.coords.speed;
            document.getElementById("<%=HiddenField1.ClientID%>").value = lat;
            document.getElementById("<%=HiddenField2.ClientID%>").value = lon;
            document.getElementById("<%=HiddenField3.ClientID%>").value = acc;
            document.getElementById("<%=HiddenField4.ClientID%>").value = spe;
            dane[0] = document.getElementById("<%=HiddenField1.ClientID%>").value;
            dane[1] = document.getElementById("<%=HiddenField2.ClientID%>").value;
            dane[2] = document.getElementById("<%=HiddenField3.ClientID%>").value;
            dane[3] = document.getElementById("<%=HiddenField4.ClientID%>").value;
            PageMethods.PrzeliczArray(dane, OnSuccess);
        }
        function OnSuccess(response, userContext, methodName)
        {
            lat = "";
            lon = "";
            acc = "";
            spe = "";
        }
    }
</script>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="BusRefresh" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        
        <div class="bus-header">
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:HiddenField ID="HiddenField2" runat="server" />
            <asp:HiddenField ID="HiddenField3" runat="server" />
            <asp:HiddenField ID="HiddenField4" runat="server" />
            <div class="bus-Menu">
                <asp:Menu   ID="busMenu"
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
                    <StaticMenuItemStyle CssClass="smis" HorizontalPadding="10px" />
                    <StaticMenuStyle CssClass="sms" />
                </asp:Menu>
            </div>
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
                        <asp:DropDownList ID="ddl_busSelect" CssClass="ddlBusSelect" runat="server" OnSelectedIndexChanged="Ddl_busSelect_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                        <asp:Button ID="bt_busSelect" runat="server" CssClass="bus-SingleBt" OnClick="Bt_busSelect_Click" Text="OK" Enabled="false"/>
                    </div>
                </asp:View>


<%-- Panel podstawowy operatora --%>
                <asp:View ID="Home" runat="server">
                    <asp:UpdatePanel runat="server" Id="BusHomeUP" >
                        <ContentTemplate>
                            <asp:Timer runat="server" Id="BusHomeTimer" Interval="2000" OnTick="BusHomeTimer_Tick"></asp:Timer>
                                <div class="bus-row">
                                    <div class="bus-left">

                                        <asp:Table ID="busMINEtable" runat="server" CssClass="busTable" 
                                                                     EnableTheming="False" 
                                                                     ForeColor="#FFCC66" 
                                                                     GridLines="Horizontal"
                                                                     Visible="true">
                                            <asp:TableRow CssClass="busTable1Row1" runat="server">
                                                <asp:TableCell Id="R1C2" runat="server" RowSpan="5" Width="15%" BackColor="SkyBlue"></asp:TableCell>
                                                <asp:TableCell Id="R1C3" runat="server" ColSpan="3" ForeColor="DarkBlue" Font-Size="36px" Font-Bold="true" HorizontalAlign="Center">00:00</asp:TableCell>
                                                <asp:TableCell Id="R1C4" runat="server" RowSpan="5" Width="15%" BackColor="SkyBlue"></asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow CssClass="busTable1Row2" runat="server">
                                                <asp:TableCell Id="R2C2" runat="server" ForeColor="Black" Font-Size="10px" Width="19%" HorizontalAlign="Center">Nr Rejsu</asp:TableCell>
                                                <asp:TableCell Id="R2C3" runat="server" ForeColor="Black" Font-Size="10px" Width="19%" HorizontalAlign="Center">Godzina</asp:TableCell>
                                                <asp:TableCell Id="R2C4" runat="server" ForeColor="Black" Font-Size="10px" Width="19%" HorizontalAlign="Center">liczba PAX</asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow CssClass="busTable1Row3" runat="server">
                                                <asp:TableCell Id="R3C2" runat="server" ForeColor="DarkBlue" Font-Size="16px" Font-Bold="true" Width="19%" HorizontalAlign="Center">XX0000</asp:TableCell>
                                                <asp:TableCell Id="R3C3" runat="server" ForeColor="DarkBlue" Font-Size="16px" Font-Bold="true" Width="19%" HorizontalAlign="Center">00:00</asp:TableCell>
                                                <asp:TableCell Id="R3C4" runat="server" ForeColor="DarkBlue" Font-Size="16px" Font-Bold="true" Width="19%" HorizontalAlign="Center">000</asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow CssClass="busTable1Row4" runat="server">
                                                <asp:TableCell Id="R4C2" runat="server" Back-Color="" ForeColor="Black" Font-Size="20px" Font-Bold="true" HorizontalAlign="right">XXX</asp:TableCell>
                                                <asp:TableCell Id="R4C3" runat="server" ForeColor="Black" Font-Size="24px" Font-Bold="true" HorizontalAlign="center">>>></asp:TableCell>
                                                <asp:TableCell Id="R4C4" runat="server" ForeColor="Black" Font-Size="20px" Font-Bold="true" HorizontalAlign="left">XXX</asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow CssClass="busTable1Row5" runat="server">
                                                <asp:TableCell Id="R5C3" runat="server" ColSpan="3" ForeColor="DarkBlue" Font-Size="36px" Font-Bold="true" HorizontalAlign="Center">PPS-GATE</asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>

                                        <asp:Table ID="busDriveTable" runat="server" CssClass="busTable" 
                                                                                     EnableTheming="False" 
                                                                                     ForeColor="#FFCC66" 
                                                                                     GridLines="Horizontal"
                                                                                     Visible="false">
                                            <asp:TableRow CssClass="busTable2Row1" runat="server">
                                                <asp:TableCell Id="Dr1C2" runat="server" RowSpan="5" Width="15%" BackColor="SkyBlue"></asp:TableCell>
                                                <asp:TableCell Id="Dr1C3" runat="server" ColSpan="3" ForeColor="Black" Font-Size="12px" Font-Bold="true" HorizontalAlign="Center">FROM LOCATION</asp:TableCell>
                                                <asp:TableCell Id="Dr1C4" runat="server" RowSpan="5" Width="15%" BackColor="SkyBlue"></asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow CssClass="busTable2Row2" runat="server">
                                                <asp:TableCell Id="Dr2C2" runat="server"  ForeColor="DarkGreen" Font-Size="16px" Font-Bold="true" HorizontalAlign="Center">.....</asp:TableCell>
                                                <asp:TableCell Id="Dr2C3" runat="server"  ForeColor="DarkGreen" Font-Size="20px" Font-Bold="true" HorizontalAlign="Center">.....</asp:TableCell>
                                                <asp:TableCell Id="Dr2C4" runat="server"  ForeColor="DarkGreen" Font-Size="16px" Font-Bold="true" HorizontalAlign="Center">.....</asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow CssClass="busTable2Row3" runat="server">
                                                <asp:TableCell Id="Dr3C3" runat="server" ColSpan="3" ForeColor="Purple" Font-Size="36px" Font-Bold="true" HorizontalAlign="Center">...distance...</asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow CssClass="busTable2Row4" runat="server">
                                                <asp:TableCell Id="Dr4C3" runat="server" ColSpan="3" ForeColor="Black" Font-Size="12px" Font-Bold="true" HorizontalAlign="Center">TO LOCATION</asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow CssClass="busTable2Row5" runat="server">
                                                <asp:TableCell Id="Dr5C3" runat="server" ColSpan="3" ForeColor="DarkBlue" Font-Size="48px" Font-Bold="true" HorizontalAlign="Center">.....</asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </div>
                                    <div class="bus-right">
                                        <div class="bus-1stLine">
                                            <asp:Button Id="busAccept" CssClass="bus-Buttons" runat="server" Text="ACCEPT" BackColor="Silver" />
                                            <asp:Button Id="busStartLoad" CssClass="bus-Buttons" runat="server" Text="LOAD" BackColor="Silver" />
                                            <asp:Button Id="busStartDrive" CssClass="bus-Buttons" runat="server" Text="DRIVE" BackColor="Silver"/>
                                        </div>
                                        <div class="bus-2ndLine">
                                            <asp:Button Id="busStartUnload" CssClass="bus-Buttons" runat="server" Text="UNLOAD" BackColor="Silver"/>
                                            <asp:Button Id="busEndOp" CssClass="bus-Buttons" runat="server" Text="END" BackColor="Silver"/>
                                            <asp:Button Id="busPause" CssClass="bus-Buttons" runat="server" Text="PAUSE" BackColor="Silver" OnClientClick="return confirm('Are you sure?');" OnClick="busPause_Click"/>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="BusHomeTimer" EventName="Tick"></asp:AsyncPostBackTrigger>
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:View>


<%-- Panel z informacjami dodatkowymi --%>
                <asp:View ID="Detail" runat="server">

                    <div class="singleCol">

                        <asp:Label ID="lblDim" runat="server" Text=""></asp:Label>
                        

                        <table id="busDetail-table" style="width: auto; height: auto;">
                            <tr>
                                <td class="dane-C1"><asp:Label ID="lb_distanceT" runat="server" Text="DistanceT :"></asp:Label></td>
                                <td class="dane-C2"><asp:Label ID="lb_BusDistanceT" runat="server" Text="" Width="100px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="odstep" colspan="2"></td>
                            </tr>
                            <tr>
                                <td class="dane-C1"><asp:Label ID="lb_oldDistanceT" runat="server" Text="Old DistanceT :"></asp:Label></td>
                                <td class="dane-C2"><asp:Label ID="lb_OldBusDistanceT" runat="server" Text="" Width="100px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="odstep" colspan="2"></td>
                            </tr>
                            <tr>
                                <td class="dane-C1"><asp:Label ID="lb_distanceS" runat="server" Text="DistanceS :"></asp:Label></td>
                                <td class="dane-C2"><asp:Label ID="lb_BusDistanceS" runat="server" Text="" Width="100px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="odstep" colspan="2"></td>
                            </tr>
                            <tr>
                                <td class="dane-C1"><asp:Label ID="lb_oldDistanceS" runat="server" Text="Old DistanceS :"></asp:Label></td>
                                <td class="dane-C2"><asp:Label ID="lb_OldBusDistanceS" runat="server" Text="" Width="100px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="odstep" colspan="2"></td>
                            </tr>
                            <tr>
                                <td class="dane-C1"><asp:Label ID="lb_distanceN" runat="server" Text="DistanceN :"></asp:Label></td>
                                <td class="dane-C2"><asp:Label ID="lb_BusDistanceN" runat="server" Text="" Width="100px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="odstep" colspan="2"></td>
                            </tr>
                            <tr>
                                <td class="dane-C1"><asp:Label ID="lb_oldDistanceN" runat="server" Text="Old DistanceN :"></asp:Label></td>
                                <td class="dane-C2"><asp:Label ID="lb_OldBusDistanceN" runat="server" Text="" Width="100px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="odstep" colspan="2"></td>
                            </tr>
                            <tr>
                                <td class="dane-C1"><asp:Label ID="lb_SpeedOpis" runat="server" Text="Acc :"></asp:Label></td>
                                <td class="dane-C2"><asp:Label ID="lb_BusSpeed" runat="server" Text="" Width="100px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="odstep" colspan="2"></td>
                            </tr>
                            <tr>
                                <td class="dane-C1"><asp:Label ID="lb_predictedDistanceT" runat="server" Text="Predicted distance T :"></asp:Label></td>
                                <td class="dane-C2"><asp:Label ID="lb_BudPredictedDistanceT" runat="server" Text="" Width="100px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="odstep" colspan="2"></td>
                            </tr>
                            <tr>
                                <td class="dane-C1"><asp:Label ID="lb_predictedDistanceS" runat="server" Text="Predicted distance S :"></asp:Label></td>
                                <td class="dane-C2"><asp:Label ID="lb_BusPredictedDistanceS" runat="server" Text="" Width="100px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="odstep" colspan="2"></td>
                            </tr>
                            <tr>
                                <td class="dane-C1"><asp:Label ID="lb_predictedDistanceN" runat="server" Text="Predicted distance N :"></asp:Label></td>
                                <td class="dane-C2"><asp:Label ID="lb_BusPredictedDistanceN" runat="server" Text="" Width="100px"></asp:Label></td>
                            </tr>
                        </table>
                    </div>
                </asp:View>

            </asp:MultiView>


            <div class= "bus-footer">
                <a class="right-lbl"><asp:Label ID="lb_zalogowany" runat="server" Text="Zalogowano jako:  "></asp:Label></a>
            	<a class="right-lbl"><asp:Label ID="lb_loggedUser" runat="server"></asp:Label></a>
            </div>
        </div>
    </form>
</body>
</html>
