<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="~/UserControl/ClientInfo/Tasks.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.ClientInfo.TasksControl" %>
<asp:UpdatePanel ID="UpdatePanelTasks" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <div class="SectionDiv-header">
            <asp:Label ID="LabelTasks" runat="server"></asp:Label>
            <uc1:EditSetting ID="UserControlEditSettingTasks" runat="server" />
        </div>
        <div class="newRow">
            <div class="column200 columnLeft">
                <asp:CheckBox ID="CheckBathing" runat="server"></asp:CheckBox>
            </div>
            <div class="column200 columnLeft">
                <asp:CheckBox ID="CheckLaundry" runat="server"></asp:CheckBox>
            </div>
            <div class="column200 columnLeft">
                <asp:CheckBox ID="CheckMealPreparation" runat="server"></asp:CheckBox>
            </div>
            <div class="columnAuto columnLeft">
                <asp:CheckBox ID="CheckDressing" runat="server"></asp:CheckBox>
            </div>
        </div>
        <div class="newRow">
            <div class="column200 columnLeft">
                <asp:CheckBox ID="CheckToileting" runat="server"></asp:CheckBox>
            </div>
            <div class="column200 columnLeft">
                <asp:CheckBox ID="CheckEscort" runat="server"></asp:CheckBox>
            </div>
            <div class="column200 columnLeft">
                <asp:CheckBox ID="CheckExcercisng" runat="server"></asp:CheckBox>
            </div>
            <div class="columnAuto columnLeft">
                <asp:CheckBox ID="CheckTransfer" runat="server"></asp:CheckBox>
                <asp:CheckBox ID="CheckAmbulation" runat="server"></asp:CheckBox>
            </div>
        </div>
        <div class="newRow">
            <div class="column200 columnLeft">
                <asp:CheckBox ID="CheckShopping" runat="server"></asp:CheckBox>
            </div>
            <div class="column200 columnLeft">
                <asp:CheckBox ID="CheckFeeding" runat="server"></asp:CheckBox>
            </div>
            <div class="column200 columnLeft">
                <asp:CheckBox ID="CheckCleaning" runat="server"></asp:CheckBox>
            </div>
            <div class="columnAuto columnLeft">
                <asp:CheckBox ID="CheckAsst" runat="server"></asp:CheckBox>
            </div>
        </div>
        <div class="newRow">
            <div class="column200 columnLeft">
                <asp:CheckBox ID="CheckGrooming" runat="server"></asp:CheckBox>
            </div>
            <div class="column200 columnLeft">
                <asp:CheckBox ID="CheckHairSkin" runat="server"></asp:CheckBox>
            </div>
            <div class="columnAuto columnLeft">
                <asp:CheckBox ID="CheckWalking" runat="server"></asp:CheckBox>
            </div>
        </div>
        <div class="newRow">
            <div class="column200 columnLeft">
                <asp:CheckBox ID="CheckOther" runat="server" ClientIDMode="Static"></asp:CheckBox>
            </div>
        </div>
        <div class="newRow">
            <div class="columnAuto columnLeft">
                <asp:TextBox ID="TextOtherSpec" runat="server" TextMode="MultiLine" CssClass="width900 whiteSpaceNormal"
                    ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="column060 columnLeft">
                <asp:Label ID="LabelTasksUpdateBy" runat="server"></asp:Label>
            </div>
            <div class="column150 columnLeft">
                <asp:TextBox ID="TextTasksUpdateBy" runat="server" CssClass="width150"></asp:TextBox>
            </div>
            <div class="columnAuto columnLeft">
                &nbsp;</div>
            <div class="column080 columnLeft">
                <asp:Label ID="LabelTasksUpdateDate" runat="server"></asp:Label>
            </div>
            <div class="column150 columnLeft">
                <asp:TextBox ID="TextTasksUpdateDate" runat="server" CssClass="width150"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="divSpace10">
            </div>
        </div>
        <asp:HiddenField ID="HiddenFieldTaskId" runat="server"/>
        <asp:HiddenField ID="HiddenFieldTasksIsNew" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
<div class="newRow DivButtonCenter">
    <asp:UpdatePanel ID="UpdatePanelActionButtons" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="ButtonClearTasks" runat="server" CssClass="ButtonClearTasks"></asp:Button>
            <asp:Button ID="ButtonSaveTasks" runat="server" CssClass="ButtonSaveTasks"></asp:Button>
            <asp:Button ID="ButtonDeleteTasks" runat="server" CssClass="ButtonDeleteTasks"></asp:Button>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonClearTasks" />
            <asp:AsyncPostBackTrigger ControlID="ButtonSaveTasks" />
            <asp:AsyncPostBackTrigger ControlID="ButtonDeleteTasks" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="DivSpace10">
</div>
