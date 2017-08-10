Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelCommon

Namespace Visitel.UserControl.EDICentral
    Public Class SubmitterControl
        Inherits CommonDataControl

        Private ContractNumber As String
        Private objShared As SharedWebControls

        Private ControlName As String, DeleteMessage As String, ValidationGroup As String, SaveMessage As String

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "SubmitterControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            GetCaptionFromResource()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If Not IsPostBack Then
                GetData()
            End If
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("EDICentral/" & ControlName)
            LoadJScript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        Private Sub ButtonSubmitterClear_Click(sender As Object, e As EventArgs)
            ClearControl()
        End Sub

        Private Sub ButtonSubmitterSave_Click(sender As Object, e As EventArgs)

            Page.Validate()
            Page.Validate(ValidationGroup)

            If ((Page.IsValid) And (ValidateData())) Then

                Dim objSubmitterDataObject As New BLEDICentral.SubmitterDataObject()

                objSubmitterDataObject.EntityIdentifierCode = Convert.ToString(DropDownListEntityIdentifierCode.SelectedValue, Nothing).Trim()
                objSubmitterDataObject.EntityTypeQualifier = Convert.ToString(DropDownListEntityTypeQualifier.SelectedValue, Nothing).Trim()
                objSubmitterDataObject.NameLastOrOrganizationName = Convert.ToString(TextBoxNameLastOrOrganizationName.Text, Nothing).Trim()
                objSubmitterDataObject.FirstName = Convert.ToString(TextBoxFirstName.Text, Nothing).Trim()
                objSubmitterDataObject.MiddleName = Convert.ToString(TextBoxMiddleName.Text, Nothing).Trim()
                objSubmitterDataObject.Prefix = Convert.ToString(TextBoxPrefix.Text, Nothing).Trim()
                objSubmitterDataObject.Suffix = Convert.ToString(TextBoxSuffix.Text, Nothing).Trim()
                objSubmitterDataObject.PrimaryIdentificationNumber = Convert.ToString(TextBoxPrimaryIdentificationNumber.Text, Nothing).Trim()
                objSubmitterDataObject.ContractName = Convert.ToString(TextBoxContractName.Text, Nothing).Trim()
                objSubmitterDataObject.Phone = Convert.ToString(TextBoxPhone.Text, Nothing).Trim()
                objSubmitterDataObject.ContractNo = Convert.ToString(TextBoxContractNo.Text, Nothing).Trim()

                Dim objBLEDICentral As New BLEDICentral()

                objSubmitterDataObject.UserId = objShared.UserId
                objSubmitterDataObject.CompanyId = objShared.CompanyId

                Try
                    objBLEDICentral.InsertSubmitterInfo(objShared.ConVisitel, objSubmitterDataObject)
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(SaveMessage)
                Catch ex As SqlException
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to Save {0}", ex.Message))
                Finally
                    objBLEDICentral = Nothing
                    objSubmitterDataObject = Nothing
                End Try
            End If
        End Sub

        Private Sub ButtonSubmitterDelete_Click(sender As Object, e As EventArgs)
            Dim objBLEDICentral As New BLEDICentral()
            Try
                If (String.IsNullOrEmpty(LabelSubmitterId.Text)) Then
                    Return
                End If

                objBLEDICentral.DeleteSubmitterInfo(objShared.ConVisitel, Convert.ToInt64(LabelSubmitterId.Text), objShared.UserId)
                LabelSubmitterId.Text = String.Empty
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DeleteMessage)
                ClearControl()
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to Delete {0}", ex.Message))
            Finally
                objBLEDICentral = Nothing
            End Try
        End Sub

        Private Sub LoadJScript()

            LoadJS("JavaScript/jquery.blockUI.js")

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                         & " var LoaderImagePath5 ='" & objShared.GetLoaderImagePath & "'; " _
                         & " var DeleteTargetButton5 ='ButtonSubmitterDelete'; " _
                         & " var DeleteDialogHeader5 ='EDI Central: Submitter'; " _
                         & " var DeleteDialogConfirmMsg5 ='Are you sure to delete this record?'; " _
                         & " var prm =''; " _
                         & " jQuery(document).ready(function () {" _
                         & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                         & "     prm.add_beginRequest(SetButtonActionProgress5); " _
                         & "     prm.add_endRequest(EndRequest); " _
                         & "     DeleteSubmitter();" _
                         & "     prm.add_endRequest(DeleteSubmitter); " _
                         & "}); " _
                  & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/EDICentral/" & ControlName & ".js")

        End Sub

        Private Sub InitializeControl()

            AddHandler ButtonSubmitterClear.Click, AddressOf ButtonSubmitterClear_Click
            AddHandler ButtonSubmitterSave.Click, AddressOf ButtonSubmitterSave_Click
            AddHandler ButtonSubmitterDelete.Click, AddressOf ButtonSubmitterDelete_Click

            ButtonSubmitterClear.ClientIDMode = ClientIDMode.Static
            ButtonSubmitterSave.ClientIDMode = ClientIDMode.Static
            ButtonSubmitterDelete.ClientIDMode = ClientIDMode.Static

            TextBoxUpdateBy.Enabled = False
            TextBoxUpdateDate.Enabled = False

            SetToolTip()
            ControlTextLength()
        End Sub

        Private Sub ControlTextLength()
            objShared.SetControlTextLength(TextBoxNameLastOrOrganizationName, 35)
            objShared.SetControlTextLength(TextBoxFirstName, 25)
            objShared.SetControlTextLength(TextBoxMiddleName, 25)
            objShared.SetControlTextLength(TextBoxPrefix, 10)
            objShared.SetControlTextLength(TextBoxSuffix, 10)
            objShared.SetControlTextLength(TextBoxPrimaryIdentificationNumber, 80)
            objShared.SetControlTextLength(TextBoxContractName, 60)
            objShared.SetControlTextLength(TextBoxPhone, 80)
            objShared.SetControlTextLength(TextBoxContractNo, 50)
        End Sub

        Private Sub SetToolTip()
            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EDICentral", ControlName & Convert.ToString(".resx"))

            DropDownListEntityIdentifierCode.ToolTip = Convert.ToString(ResourceTable("DropDownListEntityIdentifierCodeToolTip"), Nothing)
            DropDownListEntityIdentifierCode.ToolTip = If(String.IsNullOrEmpty(DropDownListEntityIdentifierCode.ToolTip),
                                                  "Code identifying an organizational entity, a physical location, property or an individual",
                                                  DropDownListEntityIdentifierCode.ToolTip)

            DropDownListEntityTypeQualifier.ToolTip = Convert.ToString(ResourceTable("DropDownListEntityTypeQualifierToolTip"), Nothing)
            DropDownListEntityTypeQualifier.ToolTip = If(String.IsNullOrEmpty(DropDownListEntityTypeQualifier.ToolTip),
                                                  "Code qualifying the type of entity",
                                                  DropDownListEntityTypeQualifier.ToolTip)

            TextBoxNameLastOrOrganizationName.ToolTip = Convert.ToString(ResourceTable("TextBoxNameLastOrOrganizationNameToolTip"), Nothing)
            TextBoxNameLastOrOrganizationName.ToolTip = If(String.IsNullOrEmpty(TextBoxNameLastOrOrganizationName.ToolTip),
                                                  "Submitter Last or Organization Name",
                                                  TextBoxNameLastOrOrganizationName.ToolTip)

            TextBoxFirstName.ToolTip = Convert.ToString(ResourceTable("TextBoxFirstNameToolTip"), Nothing)
            TextBoxFirstName.ToolTip = If(String.IsNullOrEmpty(TextBoxFirstName.ToolTip), "Submitter First Name", TextBoxFirstName.ToolTip)

            TextBoxMiddleName.ToolTip = Convert.ToString(ResourceTable("TextBoxMiddleNameToolTip"), Nothing)
            TextBoxMiddleName.ToolTip = If(String.IsNullOrEmpty(TextBoxMiddleName.ToolTip), "Submitter Middle Name", TextBoxMiddleName.ToolTip)

            TextBoxPrefix.ToolTip = Convert.ToString(ResourceTable("TextBoxPrefixToolTip"), Nothing)
            TextBoxPrefix.ToolTip = If(String.IsNullOrEmpty(TextBoxPrefix.ToolTip), "Submitter Name Prefix", TextBoxPrefix.ToolTip)

            TextBoxSuffix.ToolTip = Convert.ToString(ResourceTable("TextBoxSuffixToolTip"), Nothing)
            TextBoxSuffix.ToolTip = If(String.IsNullOrEmpty(TextBoxSuffix.ToolTip), "Submitter Name Suffix", TextBoxSuffix.ToolTip)

            TextBoxPrimaryIdentificationNumber.ToolTip = Convert.ToString(ResourceTable("TextBoxPrimaryIdentificationNumberToolTip"), Nothing)
            TextBoxPrimaryIdentificationNumber.ToolTip = If(String.IsNullOrEmpty(TextBoxPrimaryIdentificationNumber.ToolTip),
                                                            "Code identifying a party or other code",
                                                            TextBoxPrimaryIdentificationNumber.ToolTip)

            ResourceTable = Nothing
        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData() As Boolean

            If (String.IsNullOrEmpty(Convert.ToString(DropDownListEntityIdentifierCode.SelectedValue, Nothing).Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Entity Identifier Code.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(DropDownListEntityTypeQualifier.SelectedValue, Nothing).Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Entity Type Qualifier.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxNameLastOrOrganizationName.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Name Last Or Organization Name.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxFirstName.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter First Name.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxMiddleName.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Middle Name.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxPrefix.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Prefix.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxSuffix.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Suffix.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxPrimaryIdentificationNumber.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Primary Identification Number.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxContractName.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Contract Name.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxPhone.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Phone.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxContractNo.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Contract No.")
                Return False
            End If

            Return True

        End Function

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EDICentral", ControlName & Convert.ToString(".resx"))

            LabelEntityIdentifierCode.Text = Convert.ToString(ResourceTable("LabelEntityIdentifierCode"), Nothing)
            LabelEntityIdentifierCode.Text = If(String.IsNullOrEmpty(LabelEntityIdentifierCode.Text), "Entity Identifier Code:", LabelEntityIdentifierCode.Text)

            LabelEntityTypeQualifier.Text = Convert.ToString(ResourceTable("LabelEntityTypeQualifier"), Nothing)
            LabelEntityTypeQualifier.Text = If(String.IsNullOrEmpty(LabelEntityTypeQualifier.Text), "Entity Type Qualifier:", LabelEntityTypeQualifier.Text)

            LabelNameLastOrOrganizationName.Text = Convert.ToString(ResourceTable("LabelNameLastOrOrganizationName"), Nothing)
            LabelNameLastOrOrganizationName.Text = If(String.IsNullOrEmpty(LabelNameLastOrOrganizationName.Text),
                                                      "Name Last or Organization Name:", LabelNameLastOrOrganizationName.Text)

            LabelFirstName.Text = Convert.ToString(ResourceTable("LabelFirstName"), Nothing)
            LabelFirstName.Text = If(String.IsNullOrEmpty(LabelFirstName.Text), "First Name:", LabelFirstName.Text)

            LabelMiddleName.Text = Convert.ToString(ResourceTable("LabelMiddleName"), Nothing)
            LabelMiddleName.Text = If(String.IsNullOrEmpty(LabelMiddleName.Text), "Middle Name:", LabelMiddleName.Text)

            LabelPrefix.Text = Convert.ToString(ResourceTable("LabelPrefix"), Nothing)
            LabelPrefix.Text = If(String.IsNullOrEmpty(LabelPrefix.Text), "Prefix:", LabelPrefix.Text)

            LabelSuffix.Text = Convert.ToString(ResourceTable("LabelSuffix"), Nothing)
            LabelSuffix.Text = If(String.IsNullOrEmpty(LabelSuffix.Text), "Suffix:", LabelSuffix.Text)

            LabelPrimaryIdentificationNumber.Text = Convert.ToString(ResourceTable("LabelPrimaryIdentificationNumber"), Nothing)
            LabelPrimaryIdentificationNumber.Text = If(String.IsNullOrEmpty(LabelPrimaryIdentificationNumber.Text),
                                                       "Primary Identification Number:", LabelPrimaryIdentificationNumber.Text)

            LabelContractName.Text = Convert.ToString(ResourceTable("LabelContractName"), Nothing)
            LabelContractName.Text = If(String.IsNullOrEmpty(LabelContractName.Text), "Contract Name:", LabelContractName.Text)

            LabelPhone.Text = Convert.ToString(ResourceTable("LabelPhone"), Nothing)
            LabelPhone.Text = If(String.IsNullOrEmpty(LabelPhone.Text), "Phone:", LabelPhone.Text)

            LabelContractNo.Text = Convert.ToString(ResourceTable("LabelContractNo"), Nothing)
            LabelContractNo.Text = If(String.IsNullOrEmpty(LabelContractNo.Text), "Contract No:", LabelContractNo.Text)

            LabelUpdateDate.Text = Convert.ToString(ResourceTable("LabelUpdateDate"), Nothing)
            LabelUpdateDate.Text = If(String.IsNullOrEmpty(LabelUpdateDate.Text), "Update Date:", LabelUpdateDate.Text)

            LabelUpdateBy.Text = Convert.ToString(ResourceTable("LabelUpdateBy"), Nothing)
            LabelUpdateBy.Text = If(String.IsNullOrEmpty(LabelUpdateBy.Text), "Update By:", LabelUpdateBy.Text)

            ButtonSubmitterClear.Text = Convert.ToString(ResourceTable("ButtonSubmitterClear"), Nothing)
            ButtonSubmitterClear.Text = If(String.IsNullOrEmpty(ButtonSubmitterClear.Text), "Clear", ButtonSubmitterClear.Text)

            ButtonSubmitterSave.Text = Convert.ToString(ResourceTable("ButtonSubmitterSave"), Nothing)
            ButtonSubmitterSave.Text = If(String.IsNullOrEmpty(ButtonSubmitterSave.Text), "Save", ButtonSubmitterSave.Text)

            ButtonSubmitterDelete.Text = Convert.ToString(ResourceTable("ButtonSubmitterDelete"), Nothing)
            ButtonSubmitterDelete.Text = If(String.IsNullOrEmpty(ButtonSubmitterDelete.Text), "Delete", ButtonSubmitterDelete.Text)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing)
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "BillingProvider", ValidationGroup)

            SaveMessage = Convert.ToString(ResourceTable("SaveMessage"), Nothing)
            SaveMessage = If(String.IsNullOrEmpty(SaveMessage), "Saved Successfully", SaveMessage)

            DeleteMessage = Convert.ToString(ResourceTable("DeleteMessage"), Nothing)
            DeleteMessage = If(String.IsNullOrEmpty(DeleteMessage), "Deleted Successfully", DeleteMessage)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Private Sub ClearControl()

            DropDownListEntityIdentifierCode.SelectedIndex = objShared.InlineAssignHelper(DropDownListEntityTypeQualifier.SelectedIndex, 0)


            TextBoxNameLastOrOrganizationName.Text = objShared.InlineAssignHelper(TextBoxFirstName.Text, objShared.InlineAssignHelper(TextBoxMiddleName.Text,
                                                     objShared.InlineAssignHelper(TextBoxPrefix.Text, objShared.InlineAssignHelper(TextBoxSuffix.Text,
                                                     objShared.InlineAssignHelper(TextBoxPrimaryIdentificationNumber.Text, objShared.InlineAssignHelper(TextBoxContractName.Text,
                                                     objShared.InlineAssignHelper(TextBoxPhone.Text, objShared.InlineAssignHelper(TextBoxContractNo.Text,
                                                     objShared.InlineAssignHelper(TextBoxUpdateDate.Text, objShared.InlineAssignHelper(TextBoxUpdateBy.Text, String.Empty))))))))))

        End Sub

        Private Sub GetData()
            DirectCast(Me, ICommonDataControl).BindEDICodesDropDownList(objShared.ConVisitel, DropDownListEntityIdentifierCode,
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesTableName.Submitter),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesColumnName.NM101),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.EntityIdentifierCode),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesType.EDICentral))

            DirectCast(Me, ICommonDataControl).BindEDICodesDropDownList(objShared.ConVisitel, DropDownListEntityTypeQualifier,
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesTableName.Provider),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesColumnName.NM102),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.EntityTypeQualifier),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesType.EDICentral))
        End Sub

        Public Sub LoadSubmitter()

            Dim objBLEDICentral As New BLEDICentral()

            Dim objSubmitterDataObject As BLEDICentral.SubmitterDataObject

            objSubmitterDataObject = objBLEDICentral.SelectSubmitterInfo(objShared.ConVisitel, Me.ContractNumber)

            LabelSubmitterId.Text = objSubmitterDataObject.Id

            DropDownListEntityIdentifierCode.SelectedIndex = DropDownListEntityIdentifierCode.Items.IndexOf(DropDownListEntityIdentifierCode.Items.FindByValue(
                                                                          objSubmitterDataObject.EntityIdentifierCode))

            DropDownListEntityTypeQualifier.SelectedIndex = DropDownListEntityTypeQualifier.Items.IndexOf(DropDownListEntityTypeQualifier.Items.FindByValue(
                                                                          objSubmitterDataObject.EntityTypeQualifier))

            TextBoxNameLastOrOrganizationName.Text = objSubmitterDataObject.NameLastOrOrganizationName
            TextBoxFirstName.Text = objSubmitterDataObject.FirstName
            TextBoxMiddleName.Text = objSubmitterDataObject.MiddleName
            TextBoxPrefix.Text = objSubmitterDataObject.Prefix
            TextBoxSuffix.Text = objSubmitterDataObject.Suffix
            TextBoxPrimaryIdentificationNumber.Text = objSubmitterDataObject.PrimaryIdentificationNumber
            TextBoxContractName.Text = objSubmitterDataObject.ContractName
            TextBoxPhone.Text = objSubmitterDataObject.Phone
            TextBoxContractNo.Text = objSubmitterDataObject.ContractNo
            TextBoxUpdateDate.Text = objSubmitterDataObject.UpdateDate
            TextBoxUpdateBy.Text = objSubmitterDataObject.UpdateBy

            objSubmitterDataObject = Nothing
            objBLEDICentral = Nothing

        End Sub

        Public Sub SetContractNumber(ContractNumber As String)
            Me.ContractNumber = ContractNumber
        End Sub
    End Class
End Namespace