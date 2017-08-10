<%@ Control Language="vb" CodeBehind="MedSysUploads.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.TurboPAS.MedSysUploadsControl" %>
<div class="ServiceBox">
    <div class="BoxStyle ServiceLeft TurboPASMedSysUpload SectionDiv">
        <div id="Div9" class="SectionDiv-header">
            <asp:Label ID="LabelTurboPASMedSysUpload" runat="server"></asp:Label>
        </div>
        <div class="newRow DivButtonCenter">
            <asp:UpdatePanel ID="UpdatePanelUploadChoice" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:CheckBox ID="CheckBoxIndividualUpload" runat="server" CssClass="CheckBoxIndividualUpload"
                        Checked="true" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="CheckBoxIndividualUpload" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="newRow DivButtonCenter">
            <asp:UpdatePanel ID="UpdatePanelTurboPASMedSysUploadIndividual" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:Button ID="ButtonClientUpload" runat="server" CssClass="ButtonClientUpload" />
                    <asp:Button ID="ButtonAuthorizationUpload" runat="server" CssClass="ButtonAuthorizationUpload" />
                    <asp:Button ID="ButtonStaffUpload" runat="server" CssClass="ButtonStaffUpload" />
                    <asp:Button ID="ButtonScheduleUpload" runat="server" CssClass="ButtonScheduleUpload" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ButtonClientUpload" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonAuthorizationUpload" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonStaffUpload" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonScheduleUpload" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="DivSpace10">
        </div>
        <div class="newRow DivButtonCenter">
            <asp:UpdatePanel ID="UpdatePanelTurboPASMedSysUploadAll" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:Button ID="ButtonUploadAll" runat="server" CssClass="ButtonUploadAll" />
                    <asp:Button ID="ButtonDownload" runat="server" CssClass="ButtonDownload" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ButtonUploadAll" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonDownload" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="DivSpace10">
        </div>
        <div class="newRow DivButtonCenter">
            <asp:Label ID="LabelCustomMessage" runat="server" CssClass="LabelCustomMessage"></asp:Label>
        </div>
        <div class="DivSpace10">
        </div>
    </div>
</div>
<div class="ServiceBox">
    <div class="BoxStyle ServiceRight TurboPASMedSysUploadStatus SectionDiv">
        <div id="Div1" class="SectionDiv-header">
            <asp:Label ID="LabelTurboPASMedSysUploadStatus" runat="server" Text="TurboPAS-Med Sys Upload Status"></asp:Label>
        </div>
        <div class="newRow DivTextBoxOperationStatus">
            <asp:UpdatePanel ID="UpdatePanelUploadDownloadOperationDetail" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:GridView ID="GridViewUploadDownloadOperationDetail" runat="server">
                        <EmptyDataTemplate>
                            <asp:Label ID="LabelNoDataFound" Text="No Data Found" runat="server"></asp:Label>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="GridViewUploadDownloadOperationDetail" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="DivSpace10">
    </div>
</div>
