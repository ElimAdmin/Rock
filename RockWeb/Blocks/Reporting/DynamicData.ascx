﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DynamicData.ascx.cs" Inherits="RockWeb.Blocks.Reporting.DynamicData" %>

<asp:UpdatePanel ID="upnlContent" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <ContentTemplate>

        <%-- View Panel --%>
        <asp:Panel ID="pnlView" runat="server">
            <div class="grid"><Rock:Grid ID="gReport" runat="server" AllowSorting="true" EmptyDataText="No Results" /></div>
            <asp:PlaceHolder ID="phHtml" runat="server" Visible="false" />
        </asp:Panel>

        <%-- Edit Panel --%>
        <asp:Panel ID="pnlEditModel" runat="server" Visible="false">
            <Rock:ModalDialog ID="mdEdit" runat="server" OnSaveClick="lbSave_Click" Title="Dynamic Data Block">
                <Content>

                    <asp:UpdatePanel runat="server" ID="upnlEdit">
                        <ContentTemplate>

                            <Rock:NotificationBox ID="nbError" runat="server" Heading="Error" Title="Query Error!" NotificationBoxType="Danger" Visible="false" />

                            <div class="row">
                                <div class="col-md-6">
                                    <Rock:RockTextBox ID="tbName" runat="server" Label="Page Name" CssClass="input-large" Help="The current page's title" />
                                </div>
                                <div class="col-md-6">
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <Rock:RockTextBox ID="tbDesc" runat="server" Label="Page Description" TextMode="MultiLine" Rows="1" CssClass="input-xlarge" Help="The current page's description" />
                                    <Rock:CodeEditor ID="ceQuery" EditorHeight="100" EditorMode="Sql" EditorTheme="Rock" runat="server" Label="Query"
                                        Help="The SQL query or stored procedure name to execute.  If parameters are included below, this should be the name of a stored procedure, otherwise it can be any SQL text.
                                            By default, a grid will be displayed showing all the rows and columns returned by the query.  However, if a 'Formatted Ouput' value is included below, the results will be formatted
                                            according to the 'Formatted Ouput' value." />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <div class="control-label">
                                            <asp:DropDownList ID="ddlHideShow" runat="server" CssClass="input-small">
                                                <asp:ListItem Text="Hide" Value="False"></asp:ListItem>
                                                <asp:ListItem Text="Show" Value="True"></asp:ListItem>
                                            </asp:DropDownList>
                                            Columns<Rock:HelpBlock ID="hbHideShow" runat="server" Text="If using 'Show Columns,' only the columns specified will be displayed.  If using 'Hide Columns,' all columns except the columns specified will be displayed." />
                                        </div>
                                        <div class="controls">
                                            <Rock:RockTextBox ID="tbColumns" runat="server" TextMode="MultiLine" Rows="1" CssClass="input-xlarge" />
                                        </div>
                                    </div>
                                    <Rock:RockTextBox ID="tbParams" runat="server" Label="Parameters" TextMode="MultiLine" Rows="1" CssClass="input-xlarge"
                                        Help="The parameters that the stored procedure expects in the format of 'param1=value;param2=value'.  Any parameter with the same name as a page parameter (i.e. querystring, 
                                            form, or page route) will have it's value replaced with the page's current value.  A parameter with the name of 'CurrentPersonId' will have it's value replaced with the currently logged in person's id." />
                                    <Rock:RockCheckBox ID="cbPersonReport" runat="server" Text="Person Report"
                                        Help="Does this query return a list of people? If it does, then additional options will be available from the result grid.  (i.e. Communicate, etc).  Note: A column named 'Id' that contains the person's Id is required for a person report." />
                                    <Rock:RockTextBox ID="tbMergeFields" runat="server" Label="Communication Merge Fields" TextMode="MultiLine" Rows="1" CssClass="input-xlarge"
                                        Help="When creating a new communication from a person report, additional fields from the report can be used as merge fields on the communication.  Enter any column names that you'd like to be available for the communication." />
                                </div>
                                <div class="col-md-6">
                                    <Rock:RockTextBox ID="tbUrlMask" runat="server" Label="Selection Url" CssClass="input-large"
                                        Help="The Url to redirect user to when they click on a row in the grid.  Any column's value can be used in the url by including it in braces.  For example if the grid includes an 'Id' column that contains Person Ids, you can link to the Person view, by specifying a value here of '~/Person/{Id}" />
                                    <Rock:CodeEditor ID="ceFormattedOutput" runat="server" Label="Formatted Output" CssClass="input-large" EditorHeight="180"
                                        Help="Optional formatting to apply to the returned results.  If left blank, a grid will be displayed. Example: {% for row in rows %} {{ row.FirstName }} {% endfor %}" />
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                </Content>
            </Rock:ModalDialog>

        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>