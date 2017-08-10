
Imports VisitelCommon.VisitelCommon
Imports VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports System.Data.SqlClient

Namespace Visitel.UserControl.EDICentral
    Public Class FunctionalGroupHeaderControl
        Inherits CommonDataControl

        Private ContractNumber As String
        Private objShared As SharedWebControls

        Private ControlName As String, DeleteMessage As String, ValidationGroup As String, SaveMessage As String

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "FunctionalGroupHeaderControl"
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

        Private Sub ButtonFunctionalGroupHeaderClear_Click(sender As Object, e As EventArgs)
            ClearControl()
        End Sub

        Private Sub ButtonFunctionalGroupHeaderSave_Click(sender As Object, e As EventArgs)

            Page.Validate()
            Page.Validate(ValidationGroup)

            If ((Page.IsValid) And (ValidateData())) Then

                Dim objFunctionalGroupHeaderDataObject As New BLEDICentral.FunctionalGroupHeaderDataObject()

                objFunctionalGroupHeaderDataObject.FunctionalIdentifierCode = Convert.ToString(TextBoxFunctionalIdentifierCode.Text, Nothing).Trim()
                objFunctionalGroupHeaderDataObject.ApplicationSenderCode = Convert.ToString(TextBoxApplicationSenderCode.Text, Nothing).Trim()
                objFunctionalGroupHeaderDataObject.ApplicationReceiverCode = Convert.ToString(DropDownListApplicationReceiverCode.SelectedValue, Nothing).Trim()
                objFunctionalGroupHeaderDataObject.GroupControlNumber = Convert.ToString(TextBoxGroupControlNumber.Text, Nothing).Trim()
                objFunctionalGroupHeaderDataObject.ResponsibleAgencyCode = Convert.ToString(TextBoxResponsibleAgencyCode.Text, Nothing).Trim()
                objFunctionalGroupHeaderDataObject.IndustryIdentifierCode = Convert.ToString(TextBoxIndustryIdentifierCode.Text, Nothing).Trim()
                objFunctionalGroupHeaderDataObject.ContractNo = Convert.ToString(TextBoxContractNo.Text, Nothing).Trim()
                objFunctionalGroupHeaderDataObject.UpdateDate = Convert.ToString(TextBoxUpdateDate.Text, Nothing).Trim()
                objFunctionalGroupHeaderDataObject.UpdateBy = Convert.ToString(TextBoxUpdateBy.Text, Nothing).Trim()

                Dim objBLEDICentral As New BLEDICentral()

                objFunctionalGroupHeaderDataObject.UserId = objShared.UserId
                objFunctionalGroupHeaderDataObject.CompanyId = objShared.CompanyId

                Try
                    objBLEDICentral.InsertFunctionalGroupHeaderInfo(objShared.ConVisitel, objFunctionalGroupHeaderDataObject)
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(SaveMessage)
                Catch ex As SqlException
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to Save {0}", ex.Message))
                Finally
                    objBLEDICentral = Nothing
                    objFunctionalGroupHeaderDataObject = Nothing
                End Try
            End If
        End Sub

        Private Sub ButtonFunctionalGroupHeaderDelete_Click(sender As Object, e As EventArgs)
            Dim objBLEDICentral As New BLEDICentral()
            Try
                If (String.IsNullOrEmpty(LabelFunctionalGroupHeaderId.Text)) Then
                    Return
                End If

                objBLEDICentral.DeleteFunctionalGroupHeaderInfo(objShared.ConVisitel, Convert.ToInt64(LabelFunctionalGroupHeaderId.Text), objShared.UserId)
                LabelFunctionalGroupHeaderId.Text = String.Empty
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
                         & " var LoaderImagePath1 ='" & objShared.GetLoaderImagePath & "'; " _
                         & " var DeleteTargetButton1 ='ButtonFunctionalGroupHeaderDelete'; " _
                         & " var DeleteDialogHeader1 ='EDI Central: Functional Group Header'; " _
                         & " var DeleteDialogConfirmMsg1 ='Are you sure to delete this record?'; " _
                         & " var prm =''; " _
                         & " jQuery(document).ready(function () {" _
                         & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                         & "     prm.add_beginRequest(SetButtonActionProgress1); " _
                         & "     prm.add_endRequest(EndRequest); " _
                         & "     DeleteFunctionalGroupHeader();" _
                         & "     prm.add_endRequest(DeleteFunctionalGroupHeader); " _
                         & "}); " _
                  & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/EDICentral/" & ControlName & ".js")

        End Sub

        Private Sub InitializeControl()

            AddHandler ButtonFunctionalGroupHeaderClear.Click, AddressOf ButtonFunctionalGroupHeaderClear_Click
            AddHandler ButtonFunctionalGroupHeaderSave.Click, AddressOf ButtonFunctionalGroupHeaderSave_Click
            AddHandler ButtonFunctionalGroupHeaderDelete.Click, AddressOf ButtonFunctionalGroupHeaderDelete_Click

            ButtonFunctionalGroupHeaderClear.ClientIDMode = ClientIDMode.Static
            ButtonFunctionalGroupHeaderSave.ClientIDMode = ClientIDMode.Static
            ButtonFunctionalGroupHeaderDelete.ClientIDMode = ClientIDMode.Static

            TextBoxUpdateBy.Enabled = False
            TextBoxUpdateDate.Enabled = False

            SetToolTip()

            ControlTextLength()

        End Sub

        Private Sub ControlTextLength()
            objShared.SetControlTextLength(TextBoxFunctionalIdentifierCode, 2)
            objShared.SetControlTextLength(TextBoxApplicationSenderCode, 15)
            objShared.SetControlTextLength(TextBoxResponsibleAgencyCode, 2)
            objShared.SetControlTextLength(TextBoxIndustryIdentifierCode, 12)
            objShared.SetControlTextLength(TextBoxContractNo, 50)
        End Sub

        Private Sub SetToolTip()
            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EDICentral", ControlName & Convert.ToString(".resx"))

            TextBoxFunctionalIdentifierCode.ToolTip = Convert.ToString(ResourceTable("TextBoxFunctionalIdentifierCodeToolTip"), Nothing)
            TextBoxFunctionalIdentifierCode.ToolTip = If(String.IsNullOrEmpty(TextBoxFunctionalIdentifierCode.ToolTip),
                                                             "Code identifying a group of application related transaction sets",
                                                             TextBoxFunctionalIdentifierCode.ToolTip)

            TextBoxApplicationSenderCode.ToolTip = Convert.ToString(ResourceTable("TextBoxApplicationSenderCodeToolTip"), Nothing)
            TextBoxApplicationSenderCode.ToolTip = If(String.IsNullOrEmpty(TextBoxApplicationSenderCode.ToolTip),
                                                             "Code identifying party sending transmission; codes agreed to by trading partners",
                                                             TextBoxApplicationSenderCode.ToolTip)

            DropDownListApplicationReceiverCode.ToolTip = Convert.ToString(ResourceTable("DropDownListApplicationReceiverCodeToolTip"), Nothing)
            DropDownListApplicationReceiverCode.ToolTip = If(String.IsNullOrEmpty(DropDownListApplicationReceiverCode.ToolTip),
                                                             "Code identifying party receiving transmission; codes agreed to by trading partners",
                                                             DropDownListApplicationReceiverCode.ToolTip)

            TextBoxGroupControlNumber.ToolTip = Convert.ToString(ResourceTable("TextBoxGroupControlNumberToolTip"), Nothing)
            TextBoxGroupControlNumber.ToolTip = If(String.IsNullOrEmpty(TextBoxGroupControlNumber.ToolTip),
                                                             "Assigned number originated and maintained by the sender",
                                                             TextBoxGroupControlNumber.ToolTip)

            TextBoxResponsibleAgencyCode.ToolTip = Convert.ToString(ResourceTable("TextBoxResponsibleAgencyCodeToolTip"), Nothing)
            TextBoxResponsibleAgencyCode.ToolTip = If(String.IsNullOrEmpty(TextBoxResponsibleAgencyCode.ToolTip),
                                                             "Code identifying the issuer of the standard; this code is used in conjunction with Data Element 480",
                                                             TextBoxResponsibleAgencyCode.ToolTip)

            TextBoxIndustryIdentifierCode.ToolTip = Convert.ToString(ResourceTable("TextBoxIndustryIdentifierCodeToolTip"), Nothing)
            TextBoxIndustryIdentifierCode.ToolTip = If(String.IsNullOrEmpty(TextBoxIndustryIdentifierCode.ToolTip),
                                                             "Code indicating the version, release, subrelease, and industry identifier of the EDI standard being used.",
                                                             TextBoxIndustryIdentifierCode.ToolTip)

            ResourceTable = Nothing
        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData() As Boolean

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxFunctionalIdentifierCode.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Functional Identifier Code.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxApplicationSenderCode.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Application Sender Code.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(DropDownListApplicationReceiverCode.SelectedValue, Nothing).Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Application Receiver Code.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxGroupControlNumber.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Group Control Number.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxResponsibleAgencyCode.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Responsible Agency Code.")
                Return False
            End If

            If (String.IsNullOrEmpty(Convert.ToString(Convert.ToString(TextBoxIndustryIdentifierCode.Text, Nothing).Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Enter Industry Identifier Code.")
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

            LabelFunctionalIdentifierCode.Text = Convert.ToString(ResourceTable("LabelFunctionalIdentifierCode"), Nothing)
            LabelFunctionalIdentifierCode.Text = If(String.IsNullOrEmpty(LabelFunctionalIdentifierCode.Text), "Functional Identifier Code:", LabelFunctionalIdentifierCode.Text)

            LabelApplicationSenderCode.Text = Convert.ToString(ResourceTable("LabelApplicationSenderCode"), Nothing)
            LabelApplicationSenderCode.Text = If(String.IsNullOrEmpty(LabelApplicationSenderCode.Text), "Application Sender's Code:", LabelApplicationSenderCode.Text)

            LabelApplicationReceiverCode.Text = Convert.ToString(ResourceTable("LabelApplicationReceiverCode"), Nothing)
            LabelApplicationReceiverCode.Text = If(String.IsNullOrEmpty(LabelApplicationReceiverCode.Text), "Application Receiver's Code:", LabelApplicationReceiverCode.Text)

            LabelGroupControlNumber.Text = Convert.ToString(ResourceTable("LabelGroupControlNumber"), Nothing)
            LabelGroupControlNumber.Text = If(String.IsNullOrEmpty(LabelGroupControlNumber.Text), "Group Control Number:", LabelGroupControlNumber.Text)

            LabelResponsibleAgencyCode.Text = Convert.ToString(ResourceTable("LabelResponsibleAgencyCode"), Nothing)
            LabelResponsibleAgencyCode.Text = If(String.IsNullOrEmpty(LabelResponsibleAgencyCode.Text), "Responsible Agency Code:", LabelResponsibleAgencyCode.Text)

            LabelIndustryIdentifierCode.Text = Convert.ToString(ResourceTable("LabelIndustryIdentifierCode"), Nothing)
            LabelIndustryIdentifierCode.Text = If(String.IsNullOrEmpty(LabelIndustryIdentifierCode.Text), "Industry Identifier Code:", LabelIndustryIdentifierCode.Text)

            LabelContractNo.Text = Convert.ToString(ResourceTable("LabelContractNo"), Nothing)
            LabelContractNo.Text = If(String.IsNullOrEmpty(LabelContractNo.Text), "Contract No:", LabelContractNo.Text)

            LabelUpdateDate.Text = Convert.ToString(ResourceTable("LabelUpdateDate"), Nothing)
            LabelUpdateDate.Text = If(String.IsNullOrEmpty(LabelUpdateDate.Text), "Update Date:", LabelUpdateDate.Text)

            LabelUpdateBy.Text = Convert.ToString(ResourceTable("LabelUpdateBy"), Nothing)
            LabelUpdateBy.Text = If(String.IsNullOrEmpty(LabelUpdateBy.Text), "Update By:", LabelUpdateBy.Text)

            ButtonFunctionalGroupHeaderClear.Text = Convert.ToString(ResourceTable("ButtonFunctionalGroupHeaderClear"), Nothing)
            ButtonFunctionalGroupHeaderClear.Text = If(String.IsNullOrEmpty(ButtonFunctionalGroupHeaderClear.Text), "Clear", ButtonFunctionalGroupHeaderClear.Text)

            ButtonFunctionalGroupHeaderSave.Text = Convert.ToString(ResourceTable("ButtonFunctionalGroupHeaderSave"), Nothing)
            ButtonFunctionalGroupHeaderSave.Text = If(String.IsNullOrEmpty(ButtonFunctionalGroupHeaderSave.Text), "Save", ButtonFunctionalGroupHeaderSave.Text)

            ButtonFunctionalGroupHeaderDelete.Text = Convert.ToString(ResourceTable("ButtonFunctionalGroupHeaderDelete"), Nothing)
            ButtonFunctionalGroupHeaderDelete.Text = If(String.IsNullOrEmpty(ButtonFunctionalGroupHeaderDelete.Text), "Delete", ButtonFunctionalGroupHeaderDelete.Text)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing)
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "FunctionalGroupHeader", ValidationGroup)

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

            TextBoxFunctionalIdentifierCode.Text = objShared.InlineAssignHelper(TextBoxApplicationSenderCode.Text, objShared.InlineAssignHelper(TextBoxGroupControlNumber.Text,
                                                   objShared.InlineAssignHelper(TextBoxResponsibleAgencyCode.Text, objShared.InlineAssignHelper(TextBoxIndustryIdentifierCode.Text,
                                                   objShared.InlineAssignHelper(TextBoxContractNo.Text, objShared.InlineAssignHelper(TextBoxUpdateDate.Text,
                                                   objShared.InlineAssignHelper(TextBoxUpdateBy.Text, String.Empty)))))))


            DropDownListApplicationReceiverCode.SelectedIndex = 0

        End Sub

        Private Sub GetData()
            BindApplicationReceiverCodeDropDownList()
        End Sub

        Public Sub LoadFunctionalGroupHeader()

            Dim objBLEDICentral As New BLEDICentral()

            Dim objFunctionalGroupHeaderDataObject As BLEDICentral.FunctionalGroupHeaderDataObject

            objFunctionalGroupHeaderDataObject = objBLEDICentral.SelectFunctionalGroupHeaderInfo(objShared.ConVisitel, Me.ContractNumber)

            LabelFunctionalGroupHeaderId.Text = objFunctionalGroupHeaderDataObject.Id

            TextBoxFunctionalIdentifierCode.Text = objFunctionalGroupHeaderDataObject.FunctionalIdentifierCode
            TextBoxApplicationSenderCode.Text = objFunctionalGroupHeaderDataObject.ApplicationSenderCode

            DropDownListApplicationReceiverCode.SelectedIndex = DropDownListApplicationReceiverCode.Items.IndexOf(
                                                                          DropDownListApplicationReceiverCode.Items.FindByValue(
                                                                          objFunctionalGroupHeaderDataObject.ApplicationReceiverCode))

            TextBoxGroupControlNumber.Text = objFunctionalGroupHeaderDataObject.GroupControlNumber
            TextBoxResponsibleAgencyCode.Text = objFunctionalGroupHeaderDataObject.ResponsibleAgencyCode
            TextBoxIndustryIdentifierCode.Text = objFunctionalGroupHeaderDataObject.IndustryIdentifierCode
            TextBoxContractNo.Text = objFunctionalGroupHeaderDataObject.ContractNo
            TextBoxUpdateDate.Text = objFunctionalGroupHeaderDataObject.UpdateDate
            TextBoxUpdateBy.Text = objFunctionalGroupHeaderDataObject.UpdateBy

            objFunctionalGroupHeaderDataObject = Nothing
            objBLEDICentral = Nothing

        End Sub
        Private Sub BindApplicationReceiverCodeDropDownList()

            Dim objBLEDICentral As New BLEDICentral

            Try

                objBLEDICentral.SelectApplicationReceiverCodeInfo(objShared.VisitelConnectionString, SqlDataSourceApplicationReceiverCode)

            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Fetch Application Receiver Code information")
            Finally
                objBLEDICentral = Nothing
            End Try


            DropDownListApplicationReceiverCode.DataSource = SqlDataSourceApplicationReceiverCode
            DropDownListApplicationReceiverCode.DataTextField = "GS03_APP_RECEIVER_CODE"
            DropDownListApplicationReceiverCode.DataValueField = "GS03_APP_RECEIVER_CODE"
            DropDownListApplicationReceiverCode.DataBind()

            DropDownListApplicationReceiverCode.Items.Insert(0, New ListItem(" ", "-1"))

        End Sub

        Public Sub SetContractNumber(ContractNumber As String)
            Me.ContractNumber = ContractNumber
        End Sub

    End Class
End Namespace