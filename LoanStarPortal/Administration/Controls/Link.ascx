<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Link.ascx.cs" Inherits="LoanStarPortal.Administration.Controls.Link" %>
<%@ Register Assembly="RadGrid.Net2" Namespace="Telerik.WebControls" TagPrefix="radG" %>
<table width="100%" border="0" cellspacing="0" cellpadding="5">
    <tr>
        <td>      
            <radG:RadGrid ID="grid" runat="server" EnableAJAX="True" AutoGenerateColumns="false" BorderStyle="Solid" BorderWidth="1px" Skin="WebBlue" Width="100%" 
             AllowPaging="False" AllowSorting="True"
             AllowAutomaticDeletes="True" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
             OnItemCommand="grid_ItemCommand" OnNeedDataSource="grid_NeedDataSource"
            >
                <MasterTableView DataKeyNames="ID" CommandItemDisplay="Bottom" CommandItemSettings-AddNewRecordText="Add new link">
                    <ExpandCollapseColumn Visible="False">
                    <HeaderStyle Width="19px"></HeaderStyle>
                    </ExpandCollapseColumn>
                    <RowIndicatorColumn Visible="False">
                    <HeaderStyle Width="20px"></HeaderStyle>
                    </RowIndicatorColumn>                    
                    <Columns>
                        <radg:GridBoundColumn HeaderText="Title" DataField="Title" UniqueName="Title" AllowSorting="true" SortExpression="Title">
                            <HeaderStyle Width="35%"/>
                        </radg:GridBoundColumn>
                        <radG:GridTemplateColumn HeaderText="URL" UniqueName="URL">
                            <ItemTemplate>
                                <a href='<%# DataBinder.Eval(Container.DataItem, "URL")%>' target="_blank"><%# DataBinder.Eval(Container.DataItem, "URL") %></a>
                            </ItemTemplate>
                        </radG:GridTemplateColumn>                            
                        <radg:GridBoundColumn HeaderText="Description" DataField="Description" UniqueName="Description">
                            <HeaderStyle Width="30%"/>
                        </radg:GridBoundColumn>                        
                        <radg:GridEditCommandColumn ButtonType="ImageButton" 
                            UpdateImageUrl="~/Images/Update.gif" EditImageUrl="~/Images/Edit.gif"
                            InsertImageUrl="~/Images/Insert.gif" CancelImageUrl="~/Images/Cancel.gif"
                            UniqueName="EditCommandColumn">
                        <HeaderStyle Width="20px" /> 
                        </radg:GridEditCommandColumn>
                        <radG:GridButtonColumn ConfirmText="Delete this link?" ButtonType="ImageButton" ImageUrl="~/images/btn_grd_delete.gif" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn">
                        <HeaderStyle Width="20px" />
                        <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton" />
                    </radG:GridButtonColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                    <EditColumn UniqueName="EditCommandColumn1">
                    </EditColumn>
                    <FormTemplate>
                        <table cellspacing="2" cellpadding="1" style="width:100%;" border="1">
                            <tr>
                                <td width="50%" valign="top">
                                    <table cellspacing="1" cellpadding="1" width="100%" border="0">
                                        <tr>
                                            <td><b>Title:</b>&nbsp;<br />
                                            <asp:TextBox ID="txtTitle" runat="server" Text='<%# Bind( "Title" ) %>'></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" Display="Dynamic" ControlToValidate="txtTitle"></asp:RequiredFieldValidator></td>
                                        </tr>
                                        <tr>
                                            <td><b>URL:</b>&nbsp;<br />
                                            <asp:TextBox ID="txtURL" runat="server" Text='<%# Bind( "URL" ) %>'></asp:TextBox><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtURL" Display="Dynamic" ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"></asp:RegularExpressionValidator></td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="50%"><b>Description:</b><br />
                                                <asp:TextBox ID="txtDescription" Text='<%# Bind( "Description") %>' runat="server" TextMode="MultiLine" Rows="5" Columns="40" TabIndex="3"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:Button ID="btnUpdate" Text='<%# (Container as GridItem).OwnerTableView.IsItemInserted ? "Insert" : "Update" %>'
                                        runat="server" CommandName='<%# (Container as GridItem).OwnerTableView.
IsItemInserted ? "PerformInsert" : "Update" %>'>
                                   </asp:Button>&nbsp;
<asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
CommandName="Cancel"></asp:Button></td>
                            </tr>
                        </table>
                    </FormTemplate>
                </EditFormSettings>
                </MasterTableView>
                <PagerStyle Mode="NumericPages" PagerTextFormat="" />
            </radG:RadGrid>        
	    </td>
    </tr>
</table>
