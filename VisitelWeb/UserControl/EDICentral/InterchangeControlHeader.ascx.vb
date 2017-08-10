Imports VisitelCommon.VisitelCommon
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelCommon

Namespace Visitel.UserControl.EDICentral
    Public Class InterchangeControlHeaderControl
        Inherits CommonDataControl

        Private objShared As SharedWebControls

        Private ContractNumber As String

        Private ControlName As String, DeleteMessage As String, ValidationGroup As String, SaveMessage As String

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "InterchangeControlHeaderControl"
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

        Private Sub ButtonInterchangeControlHeaderClear_Click(sender As Object, e As EventArgs)
            ClearControl()
        End Sub

        Private Sub ButtonInterchangeControlHeaderSave_Click(sender As Object, e As EventArgs)

            Page.Validate()
            Page.Validate(ValidationGroup)

            If ((Page.IsValid) And (ValidateData())) Then

                Dim objInterchangeControlHeaderDataObject As New BLEDICentral.InterchangeControlHeaderDataObject()

                objInterchangeControlHeaderDataObject.AuthorizationInformationQualifier = Convert.ToString(DropDownListAuthorizationInformationQualifier.SelectedValue, Nothing).Trim()
                objInterchangeControlHeaderDataObject.AuthorizationInformation = Convert.ToString(TextBoxAuthorizationInformation.Text, Nothing).Trim()
                objInterchangeControlHeaderDataObject.SecurityInformationQualifier = Convert.ToString(DropDownListSecurityInformationQualifier.SelectedValue, Nothing).Trim()
                objInterchangeControlHeaderDataObject.SecurityInformation = Convert.ToString(TextBoxSecurityInformation.Text, Nothing).Trim()
                objInterchangeControlHeaderDataObject.InterchangeIdQualifier = Convert.ToString(DropDownListInterchangeIdQualifier.SelectedValue, Nothing).Trim()
                objInterchangeControlHeaderDataObject.SubmitterId = Convert.ToString(TextBoxSubmitterID.Text, Nothing).Trim()
                objInterchangeControlHeaderDataObject.ReceiverId = Convert.ToString(TextBoxReceiverId.Text, Nothing).Trim()
                objInterchangeControlHeaderDataObject.ControlVersionId = Convert.ToString(TextBoxControlVersionID.Text, Nothing).Trim()
                objInterchangeControlHeaderDataObject.ControlNumber = Convert.ToString(TextBoxControlNumber.Text, Nothing).Trim()
                objInterchangeControlHeaderDataObject.AcknowledgementRequested = Convert.ToString(DropDownListAcknowledgementRequested.SelectedValue, Nothing).Trim()
                objInterchangeControlHeaderDataObject.UsageIndicator = Convert.ToString(DropDownListUsageIndicator.SelectedValue, Nothing).Trim()
                objInterchangeControlHeaderDataObject.ContractNo = Convert.ToString(TextBoxContractNo.Text, Nothing).Trim()

                Dim objBLEDICentral As New BLEDICentral()

                objInterchangeControlHeaderDataObject.UserId = objShared.UserId
                objInterchangeControlHeaderDataObject.CompanyId = objShared.CompanyId

                Try
                    objBLEDICentral.InsertInterchangeControlHeaderInfo(objShared.ConVisitel, objInterchangeControlHeaderDataObject)
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(SaveMessage)
                Catch ex As SqlException
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to Save {0}", ex.Message))
                Finally
                    objBLEDICentral = Nothing
                    objInterchangeControlHeaderDataObject = Nothing
                End Try
            End If
        End Sub

        Private Sub ButtonInterchangeControlHeaderDelete_Click(sender As Object, e As EventArgs)
            Dim objBLEDICentral As New BLEDICentral()
            Try
                If (String.IsNullOrEmpty(LabelInterchangeControlHeaderId.Text)) Then
                    Return  
                End If

                objBLEDICentral.DeleteInterchangeControlHeaderInfo(objShared.ConVisitel, Convert.ToInt64(LabelInterchangeControlHeaderId.Text), objShared.UserId)
                LabelInterchangeControlHeaderId.Text = String.Empty
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
                         & " var LoaderImagePath ='" & objShared.GetLoaderImagePath & "'; " _
                         & " var DeleteTargetButton ='ButtonInterchangeControlHeaderDelete'; " _
                         & " var DeleteDialogHeader ='EDI Central: Interchange Control Header'; " _
                         & " var DeleteDialogConfirmMsg ='Are you sure you want to delete this record?'; " _
                         & " var prm =''; " _
                         & " jQuery(document).ready(function () {" _
                         & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                         & "     prm.add_beginRequest(SetButtonActionProgress); " _
                         & "     prm.add_endRequest(EndRequest); " _
                         & "     DeleteInterchangeControlHeader();" _
                         & "     prm.add_endRequest(DeleteInterchangeControlHeader); " _
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

            If (String.IsNullOrEmpty(Convert.ToString(DropDownListAuthorizationInformationQualifier.SelectedValue, Nothing).Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Authorization Information Qualifier.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxAuthorizationInformation.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Authorization Information.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(DropDownListSecurityInformationQualifier.SelectedValue, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Security Information Qualifier.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxSecurityInformation.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Security Information.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(DropDownListInterchangeIdQualifier.SelectedValue, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Interchange Id Qualifier.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxSubmitterID.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Submitter Id.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxReceiverId.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Receiver Id.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxControlVersionID.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Control Version Id.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxControlNumber.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Control Number.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(DropDownListAcknowledgementRequested.SelectedValue, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Acknowledgement Requested.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(DropDownListUsageIndicator.SelectedValue, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Usage Indicator.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxContractNo.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Contract No.")
                Return False
            End If

            If (Convert.ToString(Convert.ToString(TextBoxAuthorizationInformation.Text, Nothing).Trim()).Length > 10) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Authorization information cannot be more than 10 characters long.")
                Return False
            End If

            If (Convert.ToString(Convert.ToString(TextBoxSecurityInformation.Text, Nothing).Trim()).Length > 10) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Security Information cannot be more than 10 characters long.")
                Return False
            End If

            If (Convert.ToString(Convert.ToString(TextBoxSubmitterId.Text, Nothing).Trim()).Length > 15) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Submitter id cannot be more than 15 characters long.")
                Return False
            End If

            If (Convert.ToString(Convert.ToString(TextBoxReceiverId.Text, Nothing).Trim()).Length > 15) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Receiver id cannot be more than 15 characters long.")
                Return False
            End If

            If (Convert.ToString(Convert.ToString(TextBoxControlVersionId.Text, Nothing).Trim()).Length > 5) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Control version id cannot be more than 5 characters long.")
                Return False
            End If

            If (Convert.ToString(Convert.ToString(TextBoxControlNumber.Text, Nothing).Trim()).Length > 9) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Control number cannot be more than 9 characters long.")
                Return False
            End If

            If (Convert.ToString(Convert.ToString(TextBoxContractNo.Text, Nothing).Trim()).Length > 50) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Contract No cannot be more than 50 characters long.")
                Return False
            End If

            Return True

        End Function

        Private Sub InitializeControl()

            AddHandler ButtonInterchangeControlHeaderClear.Click, AddressOf ButtonInterchangeControlHeaderClear_Click
            AddHandler ButtonInterchangeControlHeaderSave.Click, AddressOf ButtonInterchangeControlHeaderSave_Click
            AddHandler ButtonInterchangeControlHeaderDelete.Click, AddressOf ButtonInterchangeControlHeaderDelete_Click

            ButtonInterchangeControlHeaderClear.ClientIDMode = ClientIDMode.Static
            ButtonInterchangeControlHeaderSave.ClientIDMode = ClientIDMode.Static
            ButtonInterchangeControlHeaderDelete.ClientIDMode = ClientIDMode.Static

            TextBoxUpdateBy.Enabled = False
            TextBoxUpdateDate.Enabled = False

            SetToolTip()
            ControlTextLength()
        End Sub

        Private Sub SetToolTip()
            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EDICentral", ControlName & Convert.ToString(".resx"))

            DropDownListAuthorizationInformationQualifier.ToolTip = Convert.ToString(ResourceTable("DropDownListAuthorizationInformationQualifierToolTip"), Nothing)
            DropDownListAuthorizationInformationQualifier.ToolTip = If(String.IsNullOrEmpty(DropDownListAuthorizationInformationQualifier.ToolTip),
                                                             "Code to identify the type of information in the Authorization Information",
                                                             DropDownListAuthorizationInformationQualifier.ToolTip)

            TextBoxAuthorizationInformation.ToolTip = Convert.ToString(ResourceTable("TextBoxAuthorizationInformationToolTip"), Nothing)
            TextBoxAuthorizationInformation.ToolTip = If(String.IsNullOrEmpty(TextBoxAuthorizationInformation.ToolTip),
                                                             "Information used for additional identification or authorization of the interchange sender or the data in " _
                                                             & " the interchange; the type of information is set by the Authorization Information Qualifier (I01)",
                                                             TextBoxAuthorizationInformation.ToolTip)

            DropDownListSecurityInformationQualifier.ToolTip = Convert.ToString(ResourceTable("DropDownListSecurityInformationQualifierToolTip"), Nothing)
            DropDownListSecurityInformationQualifier.ToolTip = If(String.IsNullOrEmpty(DropDownListSecurityInformationQualifier.ToolTip),
                                                             "Code to identify the type of information in the Security Information",
                                                             DropDownListSecurityInformationQualifier.ToolTip)

            TextBoxSecurityInformation.ToolTip = Convert.ToString(ResourceTable("TextBoxSecurityInformationToolTip"), Nothing)
            TextBoxSecurityInformation.ToolTip = If(String.IsNullOrEmpty(TextBoxSecurityInformation.ToolTip),
                                                             "This is used for identifying the security information about the interchange sender or the data in the " _
                                                             & "interchange; the type of information is set by the Security Information Qualifier (I03)",
                                                             TextBoxSecurityInformation.ToolTip)

            DropDownListInterchangeIdQualifier.ToolTip = Convert.ToString(ResourceTable("DropDownListInterchangeIdQualifierToolTip"), Nothing)
            DropDownListInterchangeIdQualifier.ToolTip = If(String.IsNullOrEmpty(DropDownListInterchangeIdQualifier.ToolTip),
                                                             "Qualifier to designate the system/method of code structure used to designate the sender or receiver " _
                                                             & "ID element being qualified",
                                                             DropDownListInterchangeIdQualifier.ToolTip)

            TextBoxSubmitterID.ToolTip = Convert.ToString(ResourceTable("TextBoxSubmitterIDToolTip"), Nothing)
            TextBoxSubmitterID.ToolTip = If(String.IsNullOrEmpty(TextBoxSubmitterID.ToolTip),
                                                             "Identification code published by the sender for other parties to use as the receiver ID to route data " _
                                                             & "to them; the sender always codes this value in the sender ID element",
                                                             TextBoxSubmitterID.ToolTip)

            TextBoxReceiverId.ToolTip = Convert.ToString(ResourceTable("DropDownListReceiverIDToolTip"), Nothing)
            TextBoxReceiverId.ToolTip = If(String.IsNullOrEmpty(TextBoxReceiverId.ToolTip),
                                                             "Identification code published by the receiver of the data; When sending, it is used by the sender as " _
                                                             & " their sending ID, thus other parties sending to them will use this as a receiving ID to route data to them",
                                                             TextBoxReceiverId.ToolTip)

            TextBoxControlVersionID.ToolTip = Convert.ToString(ResourceTable("TextBoxControlVersionIDToolTip"), Nothing)
            TextBoxControlVersionID.ToolTip = If(String.IsNullOrEmpty(TextBoxControlVersionID.ToolTip),
                                                             "Code specifying the version number of the interchange control segments",
                                                             TextBoxControlVersionID.ToolTip)


            TextBoxControlNumber.ToolTip = Convert.ToString(ResourceTable("TextBoxControlNumberToolTip"), Nothing)
            TextBoxControlNumber.ToolTip = If(String.IsNullOrEmpty(TextBoxControlNumber.ToolTip),
                                                             "A control number assigned by the interchange sender",
                                                             TextBoxControlNumber.ToolTip)


            DropDownListAcknowledgementRequested.ToolTip = Convert.ToString(ResourceTable("DropDownListAcknowledgementRequestedToolTip"), Nothing)
            DropDownListAcknowledgementRequested.ToolTip = If(String.IsNullOrEmpty(DropDownListAcknowledgementRequested.ToolTip),
                                                             "Code sent by the sender to request an interchange acknowledgment",
                                                             DropDownListAcknowledgementRequested.ToolTip)


            DropDownListUsageIndicator.ToolTip = Convert.ToString(ResourceTable("DropDownListUsageIndicatorToolTip"), Nothing)
            DropDownListUsageIndicator.ToolTip = If(String.IsNullOrEmpty(DropDownListUsageIndicator.ToolTip),
                                                             "Code to indicate whether data enclosed by this interchange envelope is test, production or information",
                                                             DropDownListUsageIndicator.ToolTip)

            ResourceTable = Nothing
        End Sub

        Private Sub ControlTextLength()
            objShared.SetControlTextLength(TextBoxAuthorizationInformation, 10)
            objShared.SetControlTextLength(TextBoxSecurityInformation, 10)
            objShared.SetControlTextLength(TextBoxSubmitterId, 15)
            objShared.SetControlTextLength(TextBoxReceiverId, 15)
            objShared.SetControlTextLength(TextBoxControlVersionId, 5)
            objShared.SetControlTextLength(TextBoxControlNumber, 9)
            objShared.SetControlTextLength(TextBoxContractNo, 50)
        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EDICentral", ControlName & Convert.ToString(".resx"))

            LabelAuthorizationInformationQualifier.Text = Convert.ToString(ResourceTable("LabelAuthorizationInformationQualifier"), Nothing)
            LabelAuthorizationInformationQualifier.Text = If(String.IsNullOrEmpty(LabelAuthorizationInformationQualifier.Text),
                                                             "Authorization Information Qualifier:", LabelAuthorizationInformationQualifier.Text)

            LabelAuthorizationInformation.Text = Convert.ToString(ResourceTable("LabelAuthorizationInformation"), Nothing)
            LabelAuthorizationInformation.Text = If(String.IsNullOrEmpty(LabelAuthorizationInformation.Text), "Authorization Information:", LabelAuthorizationInformation.Text)

            LabelSecurityInformationQualifier.Text = Convert.ToString(ResourceTable("LabelSecurityInformationQualifier"), Nothing)
            LabelSecurityInformationQualifier.Text = If(String.IsNullOrEmpty(LabelSecurityInformationQualifier.Text),
                                                        "Security Information Qualifier:", LabelSecurityInformationQualifier.Text)

            LabelSecurityInformation.Text = Convert.ToString(ResourceTable("LabelSecurityInformation"), Nothing)
            LabelSecurityInformation.Text = If(String.IsNullOrEmpty(LabelSecurityInformation.Text), "Security Information:", LabelSecurityInformation.Text)

            LabelInterchangeIDQualifier.Text = Convert.ToString(ResourceTable("LabelInterchangeIDQualifier"), Nothing)
            LabelInterchangeIDQualifier.Text = If(String.IsNullOrEmpty(LabelInterchangeIDQualifier.Text), "Interchange ID Qualifier:", LabelInterchangeIDQualifier.Text)

            LabelSubmitterId.Text = Convert.ToString(ResourceTable("LabelSubmitterId"), Nothing)
            LabelSubmitterId.Text = If(String.IsNullOrEmpty(LabelSubmitterId.Text), "Submitter Id:", LabelSubmitterId.Text)

            LabelReceiverId.Text = Convert.ToString(ResourceTable("LabelReceiverId"), Nothing)
            LabelReceiverId.Text = If(String.IsNullOrEmpty(LabelReceiverId.Text), "Receiver Id:", LabelReceiverId.Text)

            LabelControlVersionId.Text = Convert.ToString(ResourceTable("LabelControlVersionId"), Nothing)
            LabelControlVersionId.Text = If(String.IsNullOrEmpty(LabelControlVersionId.Text), "Control Version Id:", LabelControlVersionId.Text)

            LabelControlNumber.Text = Convert.ToString(ResourceTable("LabelControlNumber"), Nothing)
            LabelControlNumber.Text = If(String.IsNullOrEmpty(LabelControlNumber.Text), "Control Number:", LabelControlNumber.Text)

            LabelAcknowledgementRequested.Text = Convert.ToString(ResourceTable("LabelAcknowledgementRequested"), Nothing)
            LabelAcknowledgementRequested.Text = If(String.IsNullOrEmpty(LabelAcknowledgementRequested.Text), "Acknowledgement Requested:", LabelAcknowledgementRequested.Text)

            LabelUsageIndicator.Text = Convert.ToString(ResourceTable("LabelUsageIndicator"), Nothing)
            LabelUsageIndicator.Text = If(String.IsNullOrEmpty(LabelUsageIndicator.Text), "Usage Indicator:", LabelUsageIndicator.Text)

            LabelContractNo.Text = Convert.ToString(ResourceTable("LabelContractNo"), Nothing)
            LabelContractNo.Text = If(String.IsNullOrEmpty(LabelContractNo.Text), "Contract No:", LabelContractNo.Text)

            LabelUpdateDate.Text = Convert.ToString(ResourceTable("LabelUpdateDate"), Nothing)
            LabelUpdateDate.Text = If(String.IsNullOrEmpty(LabelUpdateDate.Text), "Update Date:", LabelUpdateDate.Text)

            LabelUpdateBy.Text = Convert.ToString(ResourceTable("LabelUpdateBy"), Nothing)
            LabelUpdateBy.Text = If(String.IsNullOrEmpty(LabelUpdateBy.Text), "Update By:", LabelUpdateBy.Text)

            ButtonInterchangeControlHeaderClear.Text = Convert.ToString(ResourceTable("ButtonInterchangeControlHeaderClear"), Nothing)
            ButtonInterchangeControlHeaderClear.Text = If(String.IsNullOrEmpty(ButtonInterchangeControlHeaderClear.Text), "Clear", ButtonInterchangeControlHeaderClear.Text)

            ButtonInterchangeControlHeaderSave.Text = Convert.ToString(ResourceTable("ButtonInterchangeControlHeaderSave"), Nothing)
            ButtonInterchangeControlHeaderSave.Text = If(String.IsNullOrEmpty(ButtonInterchangeControlHeaderSave.Text), "Save", ButtonInterchangeControlHeaderSave.Text)

            ButtonInterchangeControlHeaderDelete.Text = Convert.ToString(ResourceTable("ButtonInterchangeControlHeaderDelete"), Nothing)
            ButtonInterchangeControlHeaderDelete.Text = If(String.IsNullOrEmpty(ButtonInterchangeControlHeaderDelete.Text), "Delete", ButtonInterchangeControlHeaderDelete.Text)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing)
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "InterchangeControlHeader", ValidationGroup)

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

            DropDownListAuthorizationInformationQualifier.SelectedIndex = objShared.InlineAssignHelper(DropDownListSecurityInformationQualifier.SelectedIndex,
                                                                          objShared.InlineAssignHelper(DropDownListInterchangeIdQualifier.SelectedIndex,
                                                                          objShared.InlineAssignHelper(DropDownListAcknowledgementRequested.SelectedIndex,
                                                                          objShared.InlineAssignHelper(DropDownListUsageIndicator.SelectedIndex, 0))))

            TextBoxAuthorizationInformation.Text = objShared.InlineAssignHelper(TextBoxSecurityInformation.Text, objShared.InlineAssignHelper(TextBoxSubmitterId.Text,
                                                   objShared.InlineAssignHelper(TextBoxReceiverId.Text, objShared.InlineAssignHelper(TextBoxControlVersionId.Text,
                                                   objShared.InlineAssignHelper(TextBoxControlNumber.Text, objShared.InlineAssignHelper(TextBoxContractNo.Text,
                                                   objShared.InlineAssignHelper(TextBoxUpdateDate.Text, objShared.InlineAssignHelper(TextBoxUpdateBy.Text,
                                                    String.Empty))))))))

        End Sub

        Private Sub GetData()
            DirectCast(Me, ICommonDataControl).BindEDICodesDropDownList(objShared.ConVisitel, DropDownListAuthorizationInformationQualifier,
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesTableName.InterchangeControlHeader),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesColumnName.ISA01),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.AuthorizationInformationQualifier),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesType.EDICentral))

            DirectCast(Me, ICommonDataControl).BindEDICodesDropDownList(objShared.ConVisitel, DropDownListSecurityInformationQualifier,
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesTableName.InterchangeControlHeader),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesColumnName.ISA03),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.SecurityInformationQualifier),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesType.EDICentral))

            DirectCast(Me, ICommonDataControl).BindEDICodesDropDownList(objShared.ConVisitel, DropDownListInterchangeIdQualifier,
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesTableName.InterchangeControlHeader),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesColumnName.ISA05),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.InterchangeIdQualifier),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesType.EDICentral))

            DirectCast(Me, ICommonDataControl).BindEDICodesDropDownList(objShared.ConVisitel, DropDownListAcknowledgementRequested,
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesTableName.InterchangeControlHeader),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesColumnName.ISA14),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.AcknowledgementRequested),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesType.EDICentral))

            DirectCast(Me, ICommonDataControl).BindEDICodesDropDownList(objShared.ConVisitel, DropDownListUsageIndicator,
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesTableName.InterchangeControlHeader),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesColumnName.ISA15),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodes.UsageIndicator),
                                               EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesType.EDICentral))
        End Sub

        Public Sub LoadInterchangeControlHeader()

            Dim objBLEDICentral As New BLEDICentral()

            Dim objInterchangeControlHeaderDataObject As BLEDICentral.InterchangeControlHeaderDataObject

            objInterchangeControlHeaderDataObject = objBLEDICentral.SelectInterchangeControlHeaderInfo(objShared.ConVisitel, Me.ContractNumber)

            LabelInterchangeControlHeaderId.Text = Convert.ToString(objInterchangeControlHeaderDataObject.Id)

            DropDownListAuthorizationInformationQualifier.SelectedIndex = DropDownListAuthorizationInformationQualifier.Items.IndexOf(
                                                                          DropDownListAuthorizationInformationQualifier.Items.FindByValue(
                                                                          objInterchangeControlHeaderDataObject.AuthorizationInformationQualifier))



            TextBoxAuthorizationInformation.Text = objInterchangeControlHeaderDataObject.AuthorizationInformation.Trim()

            DropDownListSecurityInformationQualifier.SelectedIndex = DropDownListSecurityInformationQualifier.Items.IndexOf(
                                                                          DropDownListSecurityInformationQualifier.Items.FindByValue(
                                                                          objInterchangeControlHeaderDataObject.SecurityInformationQualifier))


            TextBoxSecurityInformation.Text = objInterchangeControlHeaderDataObject.SecurityInformation.Trim()

            DropDownListInterchangeIdQualifier.SelectedIndex = DropDownListInterchangeIdQualifier.Items.IndexOf(
                                                                          DropDownListInterchangeIdQualifier.Items.FindByValue(
                                                                          objInterchangeControlHeaderDataObject.InterchangeIdQualifier))

            TextBoxSubmitterId.Text = objInterchangeControlHeaderDataObject.SubmitterId.Trim()
            TextBoxReceiverId.Text = objInterchangeControlHeaderDataObject.ReceiverId.Trim()
            TextBoxControlVersionId.Text = objInterchangeControlHeaderDataObject.ControlVersionId.Trim()
            TextBoxControlNumber.Text = objInterchangeControlHeaderDataObject.ControlNumber.Trim()


            DropDownListAcknowledgementRequested.SelectedIndex = DropDownListAcknowledgementRequested.Items.IndexOf(
                                                                          DropDownListAcknowledgementRequested.Items.FindByValue(
                                                                          objInterchangeControlHeaderDataObject.AcknowledgementRequested))

            DropDownListUsageIndicator.SelectedIndex = DropDownListUsageIndicator.Items.IndexOf(DropDownListUsageIndicator.Items.FindByValue(
                                                            objInterchangeControlHeaderDataObject.UsageIndicator))

            TextBoxContractNo.Text = objInterchangeControlHeaderDataObject.ContractNo

            TextBoxUpdateBy.Text = objInterchangeControlHeaderDataObject.UpdateBy
            TextBoxUpdateDate.Text = objInterchangeControlHeaderDataObject.UpdateDate

            objInterchangeControlHeaderDataObject = Nothing
            objBLEDICentral = Nothing

        End Sub

        Public Sub SetContractNumber(ContractNumber As String)
            Me.ContractNumber = ContractNumber
        End Sub
    End Class
End Namespace