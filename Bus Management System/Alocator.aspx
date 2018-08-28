<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Alocator.aspx.cs" Inherits="Bus_Management_System.Alocator" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="GridView1" runat="server" ShowHeaderWhenEmpty="True"
                                                        AutoGenerateColumns="False" 
                                                        onrowdeleting="RowDeleting"
                                                        OnRowCancelingEdit="cancelRecord" 
                                                        OnRowEditing="editRecord"
                                                        OnRowUpdating="updateRecord" 
                                                        CellPadding="1"
                                                        EnableModelValidation="True" 
                                                        GridLines="None" 
                                                        Width="1350px"
                                                        ForeColor="#333333" >
                <RowStyle HorizontalAlign="Center" />
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
 
<%-- DODANO --%>
            <Columns>

            <asp:TemplateField>
                <HeaderTemplate>Dodano</HeaderTemplate>         
                <ItemTemplate>
                    <asp:Label ID ="lblId" runat="server"
                                           Text='<%#Bind("created")%>'
                                           Width="30px">
                    </asp:Label>
                </ItemTemplate>
            </asp:TemplateField>


<%-- OPERACJA --%>
            <asp:TemplateField>
                <HeaderTemplate>Operacja</HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID ="lblName" runat="server" 
                                             Text='<%#Bind("operationId") %>'
                                             Width="30px">
                    </asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtName" runat="server" 
                                              Text='<%#Bind("operationId") %>' 
                                              Width="30px">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvtxtName" runat="server"  
                                                                Text="*" 
                                                                ToolTip="Ustal typ operacji" 
                                                                ControlToValidate="txtName">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revtxtName" runat="server" 
                                                                    Text="*" 
                                                                    ToolTip="Enter alphabate" 
                                                                    ControlToValidate="txtName"  
                                                                    ValidationExpression="^[a-zA-Z'.\s]{1,40}$">
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>

                <FooterTemplate>
                    <asp:TextBox ID="txtNewName" runat="server" 
                                                 Width="30px">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator ID="rfvtxtNewName" runat="server" 
                                                                   Text="*" 
                                                                   ToolTip="Ustal typ operacji" 
                                                                   ControlToValidate="txtNewName">
                    </asp:RequiredFieldValidator>
    
                    <asp:RegularExpressionValidator ID="revtxtNewName" runat="server" 
                                                                       Text="*" 
                                                                       ToolTip="Enter alphabate" 
                                                                       ControlToValidate="txtNewName" 
                                                                       ValidationExpression="^[a-zA-Z'.\s]{1,40}$">
                    </asp:RegularExpressionValidator>
                </FooterTemplate>
            </asp:TemplateField>


<%-- REJS --%>         
            <asp:TemplateField>
                <HeaderTemplate>Rejs</HeaderTemplate>

                <ItemTemplate>
                    <asp:Label ID="lblAge" runat ="server" 
                                           Text='<%#Bind("flightNb") %>'>
                    </asp:Label>
                </ItemTemplate>

                <EditItemTemplate>
                    <asp:TextBox ID ="txtAge" runat="server" 
                                              Text='<%#Bind("flightNb") %>' 
                                              MaxLength="7">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator ID="rfvtxtAge" runat="server" 
                                                               Text="*" 
                                                               ToolTip="Wprowadź numer rejsu" 
                                                               ControlToValidate="txtAge">
                    </asp:RequiredFieldValidator>

                    <asp:RegularExpressionValidator ID="revtxtAge" runat="server" 
                                                                   Text="*" 
                                                                   ToolTip="Pole nie może zawierać spacji" 
                                                                   ControlToValidate="txtAge" 
                                                                   ValidationExpression="^[0-9]+$">
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>

                <FooterTemplate>
                    <asp:TextBox ID="txtNewAge" runat="server" 
                                                MaxLength="7">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvtxtNewAge" runat="server" 
                                                                  Text="*" 
                                                                  ToolTip="Wprowadź numer rejsu" 
                                                                  ControlToValidate="txtNewAge">
                    </asp:RequiredFieldValidator>
                    
                    <asp:RegularExpressionValidator ID="revNewtxtAge" runat="server" 
                                                                      Text="*" 
                                                                      ToolTip="Pole nie może zawierać spacji" 
                                                                      ControlToValidate="txtNewAge" 
                                                                      ValidationExpression="^[0-9]+$">
                    </asp:RegularExpressionValidator>
                </FooterTemplate>
            </asp:TemplateField>


<%-- PAX --%>
            <asp:TemplateField>
                <HeaderTemplate>Pax</HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID = "lblSalary" runat="server" 
                                                Text='<%#Bind("paxCount") %>'>
                    </asp:Label>
                </ItemTemplate>

                <EditItemTemplate>
                    <asp:TextBox ID="txtSalary" runat="server" 
                                                Text='<%#Bind("paxCount") %>'  
                                                MaxLength="3">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvtxtSalary" runat="server" 
                                                                  Text="*"  
                                                                  ToolTip="Wprowadź liczbę pasażerów" 
                                                                  ControlToValidate="txtSalary">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revtxtSalary" runat="server" 
                                                                      Text="*" 
                                                                      ToolTip="Tylko cyfry" 
                                                                      ControlToValidate="txtSalary" 
                                                                      ValidationExpression="^[0-9]+$">
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>

                <FooterTemplate>
                    <asp:TextBox ID="txtNewSalary" runat="server"           
                                                   MaxLength="3">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvtxtNewSalary" runat="server" 
                                                                     Text="*"  
                                                                     ToolTip="Wprowadź liczbę pasażerów" 
                                                                     ControlToValidate="txtNewSalary">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revtxtNewSalary" runat="server" 
                                                                         Text="*" 
                                                                         ToolTip="Tylko cyfry" 
                                                                         ControlToValidate="txtNewSalary" 
                                                                         ValidationExpression="^[0-9]+$">
                    </asp:RegularExpressionValidator>
                </FooterTemplate>
            </asp:TemplateField>


<%-- PORT --%>
            <asp:TemplateField>
                <HeaderTemplate>Port</HeaderTemplate>

                <ItemTemplate>
                    <asp:Label ID = "lblCountry" runat="server" 
                                                 Text='<%#Bind("airPort") %>'>
                    </asp:Label>
                </ItemTemplate>

                <EditItemTemplate>
                    <asp:TextBox ID="txtCountry" runat="server" 
                                                 Text='<%#Bind("airPort") %>' 
                                                 MaxLength="20">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator ID="rfvtxtCountry" runat="server" 
                                                                   Text="*" 
                                                                   ToolTip="Wybierz Air Port" 
                                                                   ControlToValidate="txtCountry">
                    </asp:RequiredFieldValidator>
                </EditItemTemplate>

                <FooterTemplate>
                    <asp:TextBox ID="txtNewCountry" runat="server" 
                                                    MaxLength="20">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator ID="rfvtxtNewCountry" runat="server" 
                                                                      Text="*" 
                                                                      ToolTip="Wybierz Air Port" 
                                                                      ControlToValidate="txtNewCountry">
                    </asp:RequiredFieldValidator>
                </FooterTemplate>
            </asp:TemplateField>
 

<%-- GATE --%>
            <asp:TemplateField>
                <HeaderTemplate>Gate</HeaderTemplate>

                <ItemTemplate>
                    <asp:Label ID = "lblCity" runat="server" 
                                              Text='<%#Bind("gate") %>'>
                    </asp:Label>
                </ItemTemplate>

                <EditItemTemplate>
                    <asp:TextBox ID="txtCity" runat="server" 
                                              Text='<%#Bind("gate") %>' 
                                              MaxLength="20">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator ID="rfvtxtCity" runat="server" 
                                                                Text="*" 
                                                                ToolTip="WYbierz Gate" 
                                                                ControlToValidate="txtCity">
                    </asp:RequiredFieldValidator>
                </EditItemTemplate>

                <FooterTemplate>
                    <asp:TextBox ID="txtNewCity" runat="server" 
                                                 MaxLength="20">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator ID="rfvtxtNewCity" runat="server" 
                                                                   Text="*" 
                                                                   ToolTip="Wybierz Gate" 
                                                                   ControlToValidate="txtNewCity">
                    </asp:RequiredFieldValidator>
                </FooterTemplate>
            </asp:TemplateField>
 

                    
<%-- PPS --%>
            <asp:TemplateField>
                <HeaderTemplate>PPS</HeaderTemplate>

                <ItemTemplate>
                    <asp:Label ID ="lb_port" runat="server" 
                                             Text='<%#Bind("pps") %>'>
                    </asp:Label>
                </ItemTemplate>

                <EditItemTemplate>
                    <asp:DropDownList ID="ddlPorts" runat="server" 
                                                    Width="30px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvdldlPorts" runat="server" 
                                                                  ErrorMessage="*" 
                                                                  ToolTip="Wybierz pole postojowe samolotu" 
                                                                  ControlToValidate="ddlPorts">
                    </asp:RequiredFieldValidator>
                </EditItemTemplate>

                <FooterTemplate>
                    <asp:DropDownList ID="ddlEditPorts" runat="server" 
                                                        Width="30px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvdldlEditPorts" runat="server" 
                                                                      ErrorMessage="*" 
                                                                      ToolTip="Wybierz pole postojowe samolotu" 
                                                                      ControlToValidate="ddlEditPorts">
                    </asp:RequiredFieldValidator>
                </FooterTemplate>
            </asp:TemplateField>


 
            <asp:TemplateField>
                <HeaderTemplate>Operation</HeaderTemplate>

                <ItemTemplate>
                    <asp:Button ID="btnEdit" runat="server" 
                                             CommandName="Edit" 
                                             Text="Edit">
                    </asp:Button>

                    <asp:Button ID="btnDelete" runat="server" 
                                               CommandName="Delete" 
                                               Text="Delete" 
                                               CausesValidation="true" 
                                               OnClientClick="return confirm('Are you sure?')">
                    </asp:Button>
                </ItemTemplate>

                <EditItemTemplate>      
                    <asp:Button ID="btnUpdate" runat="server" 
                                               CommandName="Update" 
                                               Text="Update">
                    </asp:Button>

                    <asp:Button ID="btnCancel" runat="server" 
                                               CommandName="Cancel" 
                                               Text="Cancel" 
                                               CausesValidation="false">
                    </asp:Button>
                </EditItemTemplate>
 
                <FooterTemplate>
                    <asp:Button ID="btnNewInsert" runat="server" 
                                                  Text="Insert" 
                                                  OnClick="InsertNewRecord">
                    </asp:Button>

                    <asp:Button ID="btnNewCancel" runat="server" 
                                                  Text="Cancel" 
                                                  OnClick="AddNewCancel" 
                                                  CausesValidation="false">
                    </asp:Button>
                </FooterTemplate>        
            </asp:TemplateField> 
                
            </Columns>

            <EmptyDataTemplate>
                  No record available                    
            </EmptyDataTemplate>       
            </asp:GridView>

            <br />
            <asp:Button ID="btnAdd" runat="server" 
                                    Text="Add New Record" 
                                    OnClick="AddNewRecord">
            </asp:Button>
        </div>
    </form>
</body>
</html>
