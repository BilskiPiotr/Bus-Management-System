<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Alocator.aspx.cs" Inherits="Bus_Management_System.Alocator" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SysBus Management System</title>
	<meta http-equiv="content-type" content="text/html"; charset="utf-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="SysBus_Management" content:"" />
	<link rel="stylesheet" href="css/alocator.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <div class="topnav">
                <asp:Menu   ID="alocatorMenu"
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
                            <asp:MenuItem Text="Logout" Value="2"></asp:MenuItem>
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
        <div id="alocator">
            <div">
                <table class="alocator_TopTable">
                    <tbody>
                        <tr>
                            <td class="center"><asp:Label ID="lb_Vehicle1" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle2" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle3" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle4" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle5" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle6" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle7" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle8" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle9" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle10" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle11" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle12" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="center"><asp:Label ID="lb_Vehicle13" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle14" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle15" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle16" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle17" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle18" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle19" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle20" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle21" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle22" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle23" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                            <td class="center"><asp:Label ID="lb_Vehicle24" runat="server" Text="- - -" style="text-align: center;" Height="100%" Width="100%" Visible="False"></asp:Label></td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="alocator-Panel">
                <asp:GridView ID="gv_Alocator" runat="server"   AutoGenerateColumns ="False"
                                                                ShowHeaderWhenEmpty="True"
                                                                ondatabound="Gv_CheckSecurityZone"
                                                                onrowdatabound="Gv_Alocator_RowDataBound" 
                                                                onrowdeleting="Gv_Alocator_RowDeleting"
                                                                onrowediting="Gv_Alocator_RowEditing" 
                                                                DataKeyNames="Id"
                                                                onrowcommand="Gv_Alocator_RowCommand" 
                                                                onrowupdating="Gv_Alocator_RowUpdating"
                                                                CellPadding="4" 
                                                                ForeColor="#333333"
                                                                onrowcancelingedit="Gv_Alocator_CancelEdit">

                                        <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#E3EAEB" HorizontalAlign="Center"/>
                                        <AlternatingRowStyle BackColor="White" />
                                        <EditRowStyle BackColor="#7C6F57" />
                                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                        

<%-- Data i godzina utworzenia zadania --%>
                    <Columns >
                        <asp:TemplateField HeaderText="Utworzono" ItemStyle-Width="12%">
                            <ItemTemplate>
                                <%# Eval("Created") %>
                            </ItemTemplate>
                            <EditItemTemplate >
                                <asp:Label ID="lbleid" runat="server" Text='<%#Eval("Created") %>' width ="100%"></asp:Label>
                            </EditItemTemplate>
                            <FooterTemplate >
                                <asp:Button ID="bt_insert" runat="server" Text= "Dodaj" CommandName="Insert" width ="100%" ></asp:Button>
                            </FooterTemplate>
                        </asp:TemplateField>
        
<%-- Opreślenie typu operacji --%>
                        <asp:TemplateField HeaderText="Operacja" ItemStyle-Width="6%">
                            <ItemTemplate>
                                <asp:Label ID="lb_operation" runat="server" Text='<%#Eval("Operation") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate >
                                <asp:DropDownList ID="ddl_operationEdit" runat="server" width ="100%"></asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate >
                                <asp:DropDownList ID="ddl_operationAdd" runat="server" Width="100%"></asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>

<%-- Nr rejsu wg przewoźnika --%>
                        <asp:TemplateField HeaderText="Rejs Nr." ItemStyle-Width="6%">
                            <ItemTemplate>
                                <%#Eval("FlightNb") %>
                            </ItemTemplate>
                            <EditItemTemplate >
                                <asp:TextBox ID="tb_flightNbEdit" runat="server" Width="100%" Text='<%#Eval("FlightNb") %>'></asp:TextBox>
                            </EditItemTemplate> 
                            <FooterTemplate >
                                <asp:TextBox ID="tb_flightNbAdd" runat="server" Width="100%"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
        
<%-- Oznaczenie Portu Lotniczego wg nomenklatury IATA --%>
                        <asp:TemplateField HeaderText="Port" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <%#Eval("IATA_Name") %>
                            </ItemTemplate>
                            <EditItemTemplate >
                                <asp:DropDownList ID="ddl_airPortEdit" runat="server" Width="100%"></asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate >
                                <asp:DropDownList ID="ddl_airPortAdd" runat="server" Width="100%"></asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>
        
<%-- Znacznik strefy przylotu jeśli występuje --%>            
                        <asp:TemplateField HeaderText="Strefa" Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lb_zone" runat="server" Text='<%#Eval("Shengen") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

<%-- Zadeklarowana liczba pasażerów --%>
                        <asp:TemplateField HeaderText="PAX" ItemStyle-Width="4%">
                            <ItemTemplate>
                                <%#Eval("Pax") %>
                            </ItemTemplate>
                            <EditItemTemplate >
                                <asp:TextBox ID="tb_paxEdit" runat="server" Width="100%" Text='<%#Eval("Pax") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate >
                                <asp:TextBox ID="tb_paxAdd" runat="server" Width="100%"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>

<%-- Numer Gate dla odlotu lub miejsce wysadzenia pasażerów dla przylotów --%>
                        <asp:TemplateField HeaderText="Gate" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <%#Eval("GateNb") %>
                            </ItemTemplate>
                            <EditItemTemplate >
                                <asp:DropDownList ID="ddl_gateEdit" runat="server" Width="100%"></asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate >
                                <asp:DropDownList ID="ddl_gateAdd" runat="server" Width="100%"></asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>

<%-- Id operacji z bazy danych do rozpoznania rekordu przy operacjach na danych --%>
                        <asp:TemplateField HeaderText="Id" Visible="False">
                            <ItemTemplate>
                                <%# Eval("Id") %>
                            </ItemTemplate>
                            <EditItemTemplate >
                                <asp:Label ID="lb_id" runat="server" Text='<%#Eval("id") %>'></asp:Label>
                            </EditItemTemplate>
                            <FooterTemplate >
                                <asp:LinkButton ID="idinsert" runat="server" Text= "Insert" CommandName="Insert" Width="100%"></asp:LinkButton>
                            </FooterTemplate>
                        </asp:TemplateField>

<%-- Opreślenie miejsca postoju statku powietrznego --%>
                        <asp:TemplateField HeaderText="PPS"  ItemStyle-Width="5%">
                            <ItemTemplate>
                                <%#Eval("StationNb") %>
                            </ItemTemplate>
                            <EditItemTemplate >
                                <asp:DropDownList ID="ddl_ppsEdit" runat="server" Width="100%"></asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate >
                                <asp:DropDownList ID="ddl_ppsAdd" runat="server" Width="100%"></asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>

<%-- Określenie autobusu wyznaczonego do wykonania zadania --%>
                        <asp:TemplateField HeaderText="Bus" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <asp:Label ID="lb_bus" runat="server" Text='<%#Eval("VehicleNb") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate >
                                <asp:DropDownList ID="ddl_busEdit" runat="server" Width="100%"></asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate >
                                <asp:DropDownList ID="ddl_busAdd" runat="server" Width="100%"></asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>

<%-- Numer radia do obsługi pasażerskiej w Gate --%>
                        <asp:TemplateField HeaderText="Radio Gate" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <%#Eval("RadioGate") %>
                            </ItemTemplate>
                            <EditItemTemplate >
                                <asp:TextBox ID="tb_radioGateEdit" runat="server" Width="100%" Text='<%#Eval("RadioGate") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate >
                                <asp:TextBox ID="tb_radioGateAdd" runat="server" Width="100%"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>

<%-- Numer radia do Neona odpowiedzialnego za lot --%>
                        <asp:TemplateField HeaderText="Radio Neon" ItemStyle-Width="5%">
                            <ItemTemplate>
                                <%#Eval("RadioNeon") %>
                            </ItemTemplate>
                            <EditItemTemplate >
                                <asp:TextBox ID="tb_radioNeonEdit" runat="server" Width="100%" Text='<%#Eval("RadioNeon") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate >
                                <asp:TextBox ID="tb_radioNeonAdd" runat="server" Width="100%"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>

<%-- Godzina przyjęcia zlecenia przez operatora Autobusu --%>
                        <asp:TemplateField HeaderText="Accepted" ItemStyle-Width="6%">
                            <ItemTemplate>
                                <%# Eval("Accepted") %>
                            </ItemTemplate>
                            <EditItemTemplate >
                                <asp:Label ID="lb_accepted" runat="server" Text='<%#Eval("Accepted") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>

<%-- Godzina rozpoczęcia załadunku pasażerów --%>
                        <asp:TemplateField HeaderText="Load" ItemStyle-Width="6%">
                            <ItemTemplate>
                                <%# Eval("StartLoad") %>
                            </ItemTemplate>
                            <EditItemTemplate >
                                <asp:Label ID="lb_startLoad" runat="server" Text='<%#Eval("StartLoad") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>

<%-- rozpoczęcie operacji przewozu pasażerów --%>
                        <asp:TemplateField HeaderText="Drive" ItemStyle-Width="6%">
                            <ItemTemplate>
                                <%# Eval("StartDrive") %>
                            </ItemTemplate>
                            <EditItemTemplate >
                                <asp:Label ID="lb_startDrive" runat="server" Text='<%#Eval("StartDrive") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>

<%-- Rozpoczęcie operacji wyładowania pasażerów z Autobusu --%>
                        <asp:TemplateField HeaderText="Unload" ItemStyle-Width="6%">
                            <ItemTemplate>
                                <%# Eval("StartUnload") %>
                            </ItemTemplate>
                            <EditItemTemplate >
                                <asp:Label ID="lb_startUnload" runat="server" Text='<%#Eval("StartUnload") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>

<%-- Zakończenie operacji transportowej --%>
                        <asp:TemplateField HeaderText="End Op" ItemStyle-Width="6%">
                            <ItemTemplate>
                                <%# Eval("EndOp") %>
                            </ItemTemplate>
                            <EditItemTemplate >
                                <asp:Label ID="lb_andOp" runat="server" Text='<%#Eval("EndOp") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>

<%-- Konfiguracja kolumny z kontrolkami kontrolnymi dostępnych operacji --%>
                        <asp:CommandField ControlStyle-ForeColor ="Blue " ButtonType="Button" ShowEditButton="True" ShowDeleteButton="True"  ItemStyle-Width="10%" UpdateText="Popraw">
                            <ControlStyle ForeColor="Blue"></ControlStyle>
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
            <br />
                <asp:Button ID="btn_addNewOperation" runat="server" 
                                                     Text="Dodaj Operację"
                                                     OnClick="Gv_Alocator_NewOperation">
                </asp:Button>
                <asp:Button ID="btn_cancelNewOperation" runat="server" 
                                                        Visible="false"
                                                        Text="Anuluj"
                                                        OnClick="Gv_Alocator_CancelNewOperation">
                </asp:Button>
            </div>
        </div>
        <div class= "footer">
		    <p>Projekt Inżynierski - Piotr Bilski - index 43335</p>
        </div>
    </form>
</body>
</html>
