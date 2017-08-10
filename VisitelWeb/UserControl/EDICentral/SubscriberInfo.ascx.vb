Imports VisitelCommon.VisitelCommon
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelCommon

Namespace Visitel.UserControl.EDICentral
    Public Class SubscriberInfoControl
        Inherits CommonDataControl

        Private ContractNumber As String
        Private objShared As SharedWebControls

        Private ControlName As String, DeleteMessage As String, ValidationGroup As String, SaveMessage As String

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "SubscriberInfoControl"
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

        Protected Sub ButtonSubscriberInfoClear_Click(sender As Object, e As EventArgs)
            ClearControl()
        End Sub

        Private Sub ButtonSubscriberInfoSave_Click(sender As Object, e As EventArgs)
            Page.Validate()
            Page.Validate(ValidationGroup)

            If ((Page.IsValid) And (ValidateData())) Then

                Dim objSubscriberInfoDataObject As New BLEDICentral.SubscriberInfoDataObject()

                objSubscriberInfoDataObject.PayerResponsibilitySequenceNumberCode = Convert.ToString(DropDownListPayerResponsibilitySequenceNumberCode.SelectedValue, Nothing).Trim()
                objSubscriberInfoDataObject.RelationshipCode = Convert.ToString(DropDownListRelationshipCode.SelectedValue, Nothing).Trim()
                objSubscriberInfoDataObject.GroupOrPolicyNumber = Convert.ToString(TextBoxGroupOrPolicyNumber.Text, Nothing).Trim()
                objSubscriberInfoDataObject.GroupOrPlanName = Convert.ToString(TextBoxGroupOrPlanName.Text, Nothing).Trim()
                objSubscriberInfoDataObject.InsuranceTypeCode = Convert.ToString(TextBoxInsuranceTypeCode.Text, Nothing).Trim()
                objSubscriberInfoDataObject.ClaimFilingIndicatorCode = Convert.ToString(DropDownListClaimFilingIndicatorCode.SelectedValue, Nothing).Trim()
                objSubscriberInfoDataObject.EntityIdentificationCode = Convert.ToString(TextBoxEntityIdentificationCode.Text, Nothing).Trim()
                objSubscriberInfoDataObject.EntityTypeQualifier = Convert.ToString(TextBoxEntityTypeQualifier.Text, Nothing).Trim()
                objSubscriberInfoDataObject.IdCodeQualifier = Convert.ToString(TextBoxIdCodeQualifier.Text, Nothing).Trim()
                objSubscriberInfoDataObject.ContractNo = Convert.ToString(TextBoxContractNo.Text, Nothing).Trim()

                Dim objBLEDICentral As New BLEDICentral()

                objSubscriberInfoDataObject.UserId = objShared.UserId
                objSubscriberInfoDataObject.CompanyId = objShared.CompanyId

                Try
                    objBLEDICentral.InsertSubscriberInfo(objShared.ConVisitel, objSubscriberInfoDataObject)
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(SaveMessage)
                Catch ex As SqlException
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to Save {0}", ex.Message))
                Finally
                    objBLEDICentral = Nothing
                    objSubscriberInfoDataObject = Nothing
                End Try
            End If
        End Sub

        Private Sub ButtonSubscriberInfoDelete_Click(sender As Object, e As EventArgs)
            Dim objBLEDICentral As New BLEDICentral()

            Try
                If (String.IsNullOrEmpty(LabelSubscriberInfoId.Text)) Then
                    Return
                End If

                objBLEDICentral.DeleteSubscriberInfo(objShared.ConVisitel, Convert.ToInt64(LabelSubscriberInfoId.Text), objShared.UserId)
                LabelSubscriberInfoId.Text = String.Empty
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DeleteMessage)
                ClearControl()
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DeleteMessage)
            Finally
                objBLEDICentral = Nothing
            End Try
        End Sub

        Private Sub InitializeControl()
            AddHandler ButtonSubscriberInfoClear.Click, AddressOf ButtonSubscriberInfoClear_Click
            AddHandler ButtonSubscriberInfoSave.Click, AddressOf ButtonSubscriberInfoSave_Click
            AddHandler ButtonSubscriberInfoDelete.Click, AddressOf ButtonSubscriberInfoDelete_Click

            ButtonSubscriberInfoClear.ClientIDMode = ClientIDMode.Static
            ButtonSubscriberInfoSave.ClientIDMode = ClientIDMode.Static
            ButtonSubscriberInfoDelete.ClientIDMode = ClientIDMode.Static

            TextBoxSubscriberInfoUpdateBy.Enabled = False
            TextBoxSubscriberInfoUpdateDate.Enabled = False

            ControlTextLength()

        End Sub

        Private Sub ControlTextLength()
            objShared.SetControlTextLength(TextBoxGroupOrPolicyNumber, 30)
            objShared.SetControlTextLength(TextBoxGroupOrPlanName, 60)
            objShared.SetControlTextLength(TextBoxInsuranceTypeCode, 3)
            objShared.SetControlTextLength(TextBoxEntityIdentificationCode, 3)
            objShared.SetControlTextLength(TextBoxEntityTypeQualifier, 1)
            objShared.SetControlTextLength(TextBoxIdCodeQualifier, 2)
            objShared.SetControlTextLength(TextBoxContractNo, 25)
        End Sub

        Private Sub LoadJScript()

            LoadJS("JavaScript/jquery.blockUI.js")

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                         & " var LoaderImagePath7 ='" & objShared.GetLoaderImagePath & "'; " _
                         & " var DeleteTargetButton7 ='ButtonSubscriberInfoDelete'; " _
                         & " var DeleteDialogHeader7 ='EDI Central: Subscriber Info'; " _
                         & " var DeleteDialogConfirmMsg7 ='Are you sure you want to delete this record?'; " _
                         & " var prm =''; " _
                         & " jQuery(document).ready(function () {" _
                         & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                         & "     prm.add_beginRequest(SetButtonActionProgress7); " _
                         & "     prm.add_endRequest(EndRequest); " _
                         & "     DeleteSubscriberInfo();" _
                         & "     prm.add_endRequest(DeleteSubscriberInfo); " _
                         & "}); " _
                  & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/EDICentral/" & ControlName & ".js")

        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData() As Boolean

            If (String.IsNullOrEmpty(Convert.ToString(DropDownListPayerResponsibilitySequenceNumberCode.SelectedValue, Nothing).Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Payer Responsibility Sequence Number Code.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(DropDownListRelationshipCode.SelectedValue, Nothing).Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Relationship Code.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxGroupOrPolicyNumber.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Group Or Policy Number.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxGroupOrPlanName.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Group Or Plan Name.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxInsuranceTypeCode.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Insurance Type Code.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxInsuranceTypeCode.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Insurance Type Code.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(DropDownListClaimFilingIndicatorCode.SelectedValue, Nothing).Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Claim Filing Indicator Code.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxEntityIdentificationCode.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Entity Identification Code.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxEntityTypeQualifier.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Entity Identification Code.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxIdCodeQualifier.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Id Code Qualifier.")
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

            LabelPayerResponsibilitySequenceNumberCode.Text = Convert.ToString(ResourceTable("LabelPayerResponsibilitySequenceNumberCode"), Nothing)
            LabelPayerResponsibilitySequenceNumberCode.Text = If(String.IsNullOrEmpty(LabelPayerResponsibilitySequenceNumberCode.Text),
                                                                 "Payer Responsibility Sequence Number Code:", LabelPayerResponsibilitySequenceNumberCode.Text)

            LabelRelationshipCode.Text = Convert.ToString(ResourceTable("LabelRelationshipCode"), Nothing)
            LabelRelationshipCode.Text = If(String.IsNullOrEmpty(LabelRelationshipCode.Text), "Relationship Code:", LabelRelationshipCode.Text)

            LabelGroupOrPolicyNumber.Text = Convert.ToString(ResourceTable("LabelGroupOrPolicyNumber"), Nothing)
            LabelGroupOrPolicyNumber.Text = If(String.IsNullOrEmpty(LabelGroupOrPolicyNumber.Text), "Group or Policy Number:", LabelGroupOrPolicyNumber.Text)

            LabelGroupOrPlanName.Text = Convert.ToString(ResourceTable("LabelGroupOrPlanName"), Nothing)
            LabelGroupOrPlanName.Text = If(String.IsNullOrEmpty(LabelGroupOrPlanName.Text), "Group or Plan Name:", LabelGroupOrPlanName.Text)

            LabelInsuranceTypeCode.Text = Convert.ToString(ResourceTable("LabelInsuranceTypeCode"), Nothing)
            LabelInsuranceTypeCode.Text = If(String.IsNullOrEmpty(LabelInsuranceTypeCode.Text), "Insurance Type Code:", LabelInsuranceTypeCode.Text)

            LabelClaimFilingIndicatorCode.Text = Convert.ToString(ResourceTable("LabelClaimFilingIndicatorCode"), Nothing)
            LabelClaimFilingIndicatorCode.Text = If(String.IsNullOrEmpty(LabelClaimFilingIndicatorCode.Text), "Claim Filing Indicator Code:", LabelClaimFilingIndicatorCode.Text)

            LabelEntityIdentificationCode.Text = Convert.ToString(ResourceTable("LabelEntityIdentificationCode"), Nothing)
            LabelEntityIdentificationCode.Text = If(String.IsNullOrEmpty(LabelEntityIdentificationCode.Text), "Entity Identification Code:", LabelEntityIdentificationCode.Text)

            LabelEntityTypeQualifier.Text = Convert.ToString(ResourceTable("LabelEntityTypeQualifier"), Nothing)
            LabelEntityTypeQualifier.Text = If(String.IsNullOrEmpty(LabelEntityTypeQualifier.Text), "Entity Type Qualifier:", LabelEntityTypeQualifier.Text)

            LabelIdCodeQualifier.Text = Convert.ToString(ResourceTable("LabelIdCodeQualifier"), Nothing)
            LabelIdCodeQualifier.Text = If(String.IsNullOrEmpty(LabelIdCodeQualifier.Text), "Id Code Qualifier:", LabelIdCodeQualifier.Text)

            LabelContractNo.Text = Convert.ToString(ResourceTable("LabelContractNo"), Nothing)
            LabelContractNo.Text = If(String.IsNullOrEmpty(LabelContractNo.Text), "Contract No:", LabelContractNo.Text)

            LabelSubscriberInfoUpdateDate.Text = Convert.ToString(ResourceTable("LabelSubscriberInfoUpdateDate"), Nothing)
            LabelSubscriberInfoUpdateDate.Text = If(String.IsNullOrEmpty(LabelSubscriberInfoUpdateDate.Text), "Update Date:", LabelSubscriberInfoUpdateDate.Text)

            LabelSubscriberInfoUpdateBy.Text = Convert.ToString(ResourceTable("LabelSubscriberInfoUpdateBy"), Nothing)
            LabelSubscriberInfoUpdateBy.Text = If(String.IsNullOrEmpty(LabelSubscriberInfoUpdateBy.Text), "Update By:", LabelSubscriberInfoUpdateBy.Text)

            ButtonSubscriberInfoClear.Text = Convert.ToString(ResourceTable("ButtonSubscriberInfoClear"), Nothing)
            ButtonSubscriberInfoClear.Text = If(String.IsNullOrEmpty(ButtonSubscriberInfoClear.Text), "Clear", ButtonSubscriberInfoClear.Text)

            ButtonSubscriberInfoSave.Text = Convert.ToString(ResourceTable("ButtonSubscriberInfoSave"), Nothing)
            ButtonSubscriberInfoSave.Text = If(String.IsNullOrEmpty(ButtonSubscriberInfoSave.Text), "Save", ButtonSubscriberInfoSave.Text)

            ButtonSubscriberInfoDelete.Text = Convert.ToString(ResourceTable("ButtonSubscriberInfoDelete"), Nothing)
            ButtonSubscriberInfoDelete.Text = If(String.IsNullOrEmpty(ButtonSubscriberInfoDelete.Text), "Delete", ButtonSubscriberInfoDelete.Text)

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

            DropDownListPayerResponsibilitySequenceNumberCode.SelectedIndex = objShared.InlineAssignHelper(DropDownListRelationshipCode.SelectedIndex,
                                                                              objShared.InlineAssignHelper(DropDownListClaimFilingIndicatorCode.SelectedIndex, 0))


            TextBoxGroupOrPolicyNumber.Text = objShared.InlineAssignHelper(TextBoxGroupOrPlanName.Text, objShared.InlineAssignHelper(TextBoxInsuranceTypeCode.Text,
                                              objShared.InlineAssignHelper(TextBoxEntityIdentificationCode.Text, objShared.InlineAssignHelper(TextBoxEntityTypeQualifier.Text,
                                              objShared.InlineAssignHelper(TextBoxIdCodeQualifier.Text, objShared.InlineAssignHelper(TextBoxContractNo.Text,
                                              objShared.InlineAssignHelper(TextBoxSubscriberInfoUpdateDate.Text,
                                              objShared.InlineAssignHelper(TextBoxSubscriberInfoUpdateBy.Text, String.Empty))))))))
        End Sub

        Private Sub GetData()
            DirectCast(Me, ICommonDataControl).BindEDICodesDropDownList(objShared.ConVisitel, DropDownListPayerResponsibilitySequenceNumberCode,
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesTableName.Subscriber),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesColumnName.SBR01),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.PayerResponsibilitySequenceNumberCode),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesType.EDICentral))

            DirectCast(Me, ICommonDataControl).BindEDICodesDropDownList(objShared.ConVisitel, DropDownListRelationshipCode,
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesTableName.Subscriber),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesColumnName.SBR02),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.RelationshipCode),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesType.EDICentral))

            DirectCast(Me, ICommonDataControl).BindEDICodesDropDownList(objShared.ConVisitel, DropDownListClaimFilingIndicatorCode,
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesTableName.Subscriber),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesColumnName.SBR09),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.ClaimFilingIndicatorCode),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesType.EDICentral))

        End Sub

        Public Sub LoadSubscriber()

            Dim objBLEDICentral As New BLEDICentral()

            Dim objSubscriberInfoDataObject As BLEDICentral.SubscriberInfoDataObject

            objSubscriberInfoDataObject = objBLEDICentral.SelectSubscriberInfo(objShared.ConVisitel, Me.ContractNumber)

            LabelSubscriberInfoId.Text = objSubscriberInfoDataObject.Id

            DropDownListPayerResponsibilitySequenceNumberCode.SelectedIndex =
                DropDownListPayerResponsibilitySequenceNumberCode.Items.IndexOf(DropDownListPayerResponsibilitySequenceNumberCode.Items.FindByValue(
                                                                          objSubscriberInfoDataObject.PayerResponsibilitySequenceNumberCode))

            DropDownListRelationshipCode.SelectedIndex = DropDownListRelationshipCode.Items.IndexOf(DropDownListRelationshipCode.Items.FindByValue(
                                                                          objSubscriberInfoDataObject.RelationshipCode))

            TextBoxGroupOrPolicyNumber.Text = objSubscriberInfoDataObject.GroupOrPolicyNumber
            TextBoxGroupOrPlanName.Text = objSubscriberInfoDataObject.GroupOrPlanName
            TextBoxInsuranceTypeCode.Text = objSubscriberInfoDataObject.InsuranceTypeCode

            DropDownListClaimFilingIndicatorCode.SelectedIndex = DropDownListClaimFilingIndicatorCode.Items.IndexOf(DropDownListClaimFilingIndicatorCode.Items.FindByValue(
                                                                          objSubscriberInfoDataObject.ClaimFilingIndicatorCode))

            TextBoxEntityIdentificationCode.Text = objSubscriberInfoDataObject.EntityIdentificationCode
            TextBoxEntityTypeQualifier.Text = objSubscriberInfoDataObject.EntityTypeQualifier
            TextBoxIdCodeQualifier.Text = objSubscriberInfoDataObject.IdCodeQualifier
            TextBoxContractNo.Text = objSubscriberInfoDataObject.ContractNo

            TextBoxSubscriberInfoUpdateDate.Text = objSubscriberInfoDataObject.UpdateDate
            TextBoxSubscriberInfoUpdateBy.Text = objSubscriberInfoDataObject.UpdateBy

            objSubscriberInfoDataObject = Nothing
            objBLEDICentral = Nothing

        End Sub

        Public Sub SetContractNumber(ContractNumber As String)
            Me.ContractNumber = ContractNumber
        End Sub

    End Class
End Namespace