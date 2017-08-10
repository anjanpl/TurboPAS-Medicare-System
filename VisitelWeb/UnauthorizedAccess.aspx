<%@ Page Language="VB" MasterPageFile="~/VisitelMasterPage.Master" AutoEventWireup="true"
    CodeBehind="~/UnauthorizedAccess.aspx.vb" Inherits="VisitelWeb.Visitel.UnauthorizedAccess" %>

<%@ MasterType VirtualPath="~/VisitelMasterPage.Master" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentMain" runat="Server">
    <div style="width: 500px; height: 100px; position: absolute; top:0; bottom: 0; left: 0; right: 0; margin: auto;">
        <span style="font-size: 22px; color: Maroon;">You are unauthorized to access this page.</span>
    </div>
</asp:Content>
