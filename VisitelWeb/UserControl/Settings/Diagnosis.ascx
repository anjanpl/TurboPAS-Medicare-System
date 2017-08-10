<%@ Control Language="vb" CodeBehind="~/UserControl/Settings/Diagnosis.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.Settings.DiagnosisSetting" %>
<div>
    <%-- <div id="Div3" class="SectionDiv-header">
        <asp:Label ID="LabelDiagnosisInformationEntry" runat="server"></asp:Label>
    </div>
    <div class="newRow">
    </div>
    <div class="newRow">
        <div class="DiagnosisColumn1">
            <asp:Label ID="LabelDiagnosisOne" runat="server"></asp:Label>
        </div>
        <div class="DiagnosisColumn2">
            <asp:TextBox ID="TextBoxDiagnosisOne" runat="server"></asp:TextBox>
        </div>
        <div class="DiagnosisColumn3">
            Code
        </div>
        <div class="DiagnosisColumn4">
            <asp:TextBox ID="TextBoxDiagnosisOneCode" runat="server" CssClass="TextBoxDiagnosisOneCode"></asp:TextBox>
        </div>
        <div class="DiagnosisColumn1">
            <asp:Label ID="LabelDiagnosisTwo" runat="server"></asp:Label>
        </div>
        <div class="DiagnosisColumn2">
            <asp:TextBox ID="TextBoxDiagnosisTwo" runat="server"></asp:TextBox>
        </div>
        <div class="DiagnosisColumn3">
            Code
        </div>
        <div class="DiagnosisColumn4">
            <asp:TextBox ID="TextBoxDiagnosisTwoCode" runat="server" CssClass="TextBoxDiagnosisTwoCode"></asp:TextBox>
        </div>
    </div>
    <div class="newRow">
        <div class="DiagnosisColumn1">
            <asp:Label ID="LabelDiagnosisThree" runat="server"></asp:Label>
        </div>
        <div class="DiagnosisColumn2">
            <asp:TextBox ID="TextBoxDiagnosisThree" runat="server"></asp:TextBox>
        </div>
        <div class="DiagnosisColumn3">
            Code
        </div>
        <div class="DiagnosisColumn4">
            <asp:TextBox ID="TextBoxDiagnosisThreeCode" runat="server" CssClass="TextBoxDiagnosisThreeCode"></asp:TextBox>
        </div>
        <div class="DiagnosisColumn1">
            <asp:Label ID="LabelDiagnosisFour" runat="server"></asp:Label>
        </div>
        <div class="DiagnosisColumn2">
            <asp:TextBox ID="TextBoxDiagnosisFour" runat="server"></asp:TextBox>
        </div>
        <div class="DiagnosisColumn3">
            Code
        </div>
        <div class="DiagnosisColumn4">
            <asp:TextBox ID="TextBoxDiagnosisFourCode" runat="server" CssClass="TextBoxDiagnosisFourCode"></asp:TextBox>
        </div>
    </div>
    <div class="DivSpace10">
    </div>
    <div class="newRow DivButtonCenter">
        <asp:Button ID="ButtonSave" runat="server" CssClass="ButtonSave" />
        <asp:Button ID="ButtonClear" runat="server" CssClass="ButtonClear" />
        <asp:Button ID="ButtonSearch" runat="server" CssClass="ButtonSearch" />
    </div>
    <div class="DivSpace10">
    </div>--%>
    <div id="tabs-System">
        <div id="Div20" class="SectionDiv" runat="server">
            <div id="Div1" class="SectionDiv-header">
                <asp:Label ID="LabelDiagnosisInformationList" runat="server"></asp:Label>
            </div>
            <asp:UpdatePanel ID="UpdatePanelBillingDiagnosis" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:HiddenField ID="HiddenFieldDiagnosisId" runat="server" />
                    <asp:HiddenField ID="HiddenFieldIsNew" runat="server" />
                    <asp:HiddenField ID="HiddenFieldIsSearched" runat="server" />
                    <asp:GridView ID="GridViewDiagnosisInformation" runat="server" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="DiagnosisId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelDiagnosisId" Text='<%#Bind("DiagnosisId") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Code">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelDiagnosisCode" Text='<%#Bind("Code") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelDiagnosisDescription" Text='<%#Bind("Description") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Update By">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelUpdateBy" Text='<%#Bind("UpdateBy") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Update Date">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelUpdateDate" Text='<%#Bind("UpdateDate") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Edit">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButtonEdit" runat="server" OnClick="LinkButtonEdit_Click"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="center" Width="5%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete">
                        <ItemTemplate>
                            <asp:Button ID="ButtonDelete" runat="server" CssClass="ButtonDelete" OnClick="ButtonDelete_Click"
                                OnClientClick="javascript:return DeleteConfirmationFromGrid(this.name,this.alt);">
                            </asp:Button>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="center" Width="5%" />
                    </asp:TemplateField>--%>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="LabelNoDataFound" Text="No Data Found" runat="server"></asp:Label>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="GridViewDiagnosisInformation" />
                    <asp:AsyncPostBackTrigger ControlID="HiddenFieldDiagnosisId" />
                    <asp:AsyncPostBackTrigger ControlID="HiddenFieldIsNew" />
                    <asp:AsyncPostBackTrigger ControlID="HiddenFieldIsSearched" />
                </Triggers>
            </asp:UpdatePanel>
            <br />
            <br />
        </div>
    </div>
</div>
