<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewFields.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.ViewFields" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td>Filter by 
            <asp:TextBox ID="txtEntity" runat="server" MaxLength="100" Width="100px"></asp:TextBox>
            entity
            <asp:Button ID="btnSearch" runat="server" Text="OK" OnClick="btnSearch_Click" />
        </td>
    </tr>
    <tr>
        <td>
        <radG:RadGrid ID="G" Skin="WebBlue" GridLines="None" EnableAJAX="false" runat="server"        
            Width="100%" PageSize="20"  AllowPaging="False" AutoGenerateColumns="False" 
            OnItemCommand="G_ItemCommand" 
            AllowMultiRowEdit="false" OnItemCreated="G_ItemCreated" 
            >
            <ClientSettings>
            <Resizing AllowColumnResize="True" EnableRealTimeResize="True"/>
            </ClientSettings>
                <MasterTableView AllowNaturalSort="False" CommandItemDisplay="Bottom" DataKeyNames="ID">
                    <ItemStyle Wrap="False" BackColor="White" />
                    <AlternatingItemStyle Wrap="False" BackColor="LightGray"/>
                    <Columns>
                        <radG:GridCheckBoxColumn HeaderText="Added to DB" DataField="AddedToDB" UniqueName="AddedToDB" ReadOnly="True"></radG:GridCheckBoxColumn>
                        <radG:GridCheckBoxColumn HeaderText="Added to UI" DataField="AddedToUI" UniqueName="AddedToUI" ReadOnly="True"></radG:GridCheckBoxColumn>
                        <radG:GridTemplateColumn HeaderText="Signed Off" SortExpression="SignedOff" UniqueName="SignedOff">
                            <ItemTemplate>
                                <asp:CheckBox ID="chbSignedOff" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem,"SignedOff")%>' Enabled="false" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chbSignedOff" runat="server"/>
                            </EditItemTemplate>                            
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="Entity" SortExpression="Entity" UniqueName="TemplateColumn">
                            <ItemTemplate>
                                <asp:Label ID="lblEntity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Entity") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEntity" runat="server" MaxLength="100" SkinID="AdminInput" Text='<%# Eval("Entity") %>' Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator id="RequiredfieldvalidatorEntity" runat="server" controltovalidate="txtEntity" errormessage="*" display="Dynamic" setfocusonerror="true"></asp:RequiredFieldValidator>
                            </EditItemTemplate>                            
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="Label" SortExpression="Label" UniqueName="TemplateColumn1">
                            <ItemTemplate>
                                <asp:Label ID="lblLabel" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Label") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField ID="hdnMortgageProfileID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"MortgageProfileID") %>' />
                                <asp:TextBox ID="txtLabel" runat="server" MaxLength="100" SkinID="AdminInput" Text='<%# Eval("Label") %>' Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator id="Requiredfieldvalidator1" runat="server" controltovalidate="txtLabel" errormessage="*" display="Dynamic" setfocusonerror="true"></asp:RequiredFieldValidator>
                            </EditItemTemplate>                            
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="UI control type" SortExpression="UIControl" UniqueName="TemplateColumn2">
                            <ItemTemplate>
                                <asp:Label ID="lblUIControl" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"UIControl") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlUIControl" runat="server"  DataValueField="ID" DataTextField="Name"></asp:DropDownList>
                            </EditItemTemplate>                            
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="Value(s)" SortExpression="Value" UniqueName="TemplateColumn3">
                            <ItemTemplate>
                                <asp:Label ID="lblValue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Value") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtValue" runat="server" MaxLength="100" SkinID="AdminInput" Text='<%# Eval("Value") %>' Width="100px"></asp:TextBox>
                            </EditItemTemplate>                            
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="Field name" SortExpression="FieldName" UniqueName="TemplateColumn4">
                            <ItemTemplate>
                                <asp:Label ID="lblFieldName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FieldName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtFieldName" runat="server" MaxLength="100" SkinID="AdminInput" Text='<%# Eval("FieldName") %>' Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator id="Requiredfieldvalidator3" runat="server" controltovalidate="txtFieldName" errormessage="*" display="Dynamic" setfocusonerror="true"></asp:RequiredFieldValidator>
                            </EditItemTemplate>                            
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="General purpose" SortExpression="GeneralPurpose" UniqueName="TemplateColumn5">
                            <ItemTemplate>
                                <asp:Label ID="lblGeneralPurpose" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"GeneralPurpose") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlGeneralPurpose" runat="server"  DataValueField="ID" DataTextField="Name"></asp:DropDownList>
                            </EditItemTemplate>                            
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="Value type" SortExpression="ValueType" UniqueName="TemplateColumn6">
                            <ItemTemplate>
                                <asp:Label ID="lblValueType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ValueType") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlValueType" runat="server" DataValueField="ID" DataTextField="Name"></asp:DropDownList>
                            </EditItemTemplate>                            
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="Description" SortExpression="Description" UniqueName="TemplateColumn7">
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Description") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDescription" runat="server" MaxLength="100" SkinID="AdminInput" Text='<%# Eval("Description") %>' Width="100px"></asp:TextBox>
                            </EditItemTemplate>                            
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="Admin/Public" SortExpression="SiteType" UniqueName="TemplateColumn8">
                            <ItemTemplate>
                                <asp:Label ID="lblSiteType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SiteType") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlSiteType" runat="server"  DataValueField="ID" DataTextField="Name"></asp:DropDownList>
                            </EditItemTemplate>                            
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="Path 1" SortExpression="Path1" UniqueName="TemplateColumn9">
                            <ItemTemplate>
                                <asp:Label ID="lblPath1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Path1") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPath1" runat="server" MaxLength="100" SkinID="AdminInput" Text='<%# Eval("Path1") %>' Width="100px"></asp:TextBox>
                            </EditItemTemplate>                            
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="Path 2" SortExpression="Path2" UniqueName="TemplateColumn10">
                            <ItemTemplate>
                                <asp:Label ID="lblPath2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Path2") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPath2" runat="server" MaxLength="100" SkinID="AdminInput" Text='<%# Eval("Path2") %>' Width="100px"></asp:TextBox>
                            </EditItemTemplate>                            
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="Path 3" SortExpression="Path3" UniqueName="TemplateColumn11">
                            <ItemTemplate>
                                <asp:Label ID="lblPath3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Path3") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPath3" runat="server" MaxLength="100" SkinID="AdminInput" Text='<%# Eval("Path3") %>' Width="100px"></asp:TextBox>
                            </EditItemTemplate>                            
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="Path 4" SortExpression="Path4" UniqueName="TemplateColumn12">
                            <ItemTemplate>
                                <asp:Label ID="lblPath4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Path4") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPath4" runat="server" MaxLength="100" SkinID="AdminInput" Text='<%# Eval("Path4") %>' Width="100px"></asp:TextBox>
                            </EditItemTemplate>                            
                        </radG:GridTemplateColumn>
                        <radG:GridTemplateColumn HeaderText="Path 5" SortExpression="Path5" UniqueName="TemplateColumn13">
                            <ItemTemplate>
                                <asp:Label ID="lblPath5" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Path5") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPath5" runat="server" MaxLength="100" SkinID="AdminInput" Text='<%# Eval("Path5") %>' Width="100px"></asp:TextBox>
                            </EditItemTemplate>                            
                        </radG:GridTemplateColumn>
                        <radg:GridEditCommandColumn ButtonType="ImageButton" UpdateImageUrl="~/Images/Update.gif" 
                            EditImageUrl="~/Images/Edit.gif" InsertImageUrl="~/Images/Insert.gif" 
                            CancelImageUrl="~/Images/Cancel.gif">
                                <HeaderStyle Width="20px" /> 
                        </radg:GridEditCommandColumn>
                    </Columns>
                    <EditFormSettings>
                        <EditColumn UniqueName="EditCommandColumn1">
                        </EditColumn>
                    </EditFormSettings>
                    <ExpandCollapseColumn Visible="False">
                        <HeaderStyle Width="19px" />
                    </ExpandCollapseColumn>
                    <RowIndicatorColumn Visible="False">
                        <HeaderStyle Width="20px" />
                    </RowIndicatorColumn>
                </MasterTableView>
                <PagerStyle HorizontalAlign="Left" Mode="NumericPages" />
            <AlternatingItemStyle BackColor="LightBlue" />
            <ItemStyle BackColor="White" />
            </radG:RadGrid>        
        </td>
    </tr>
</table>
