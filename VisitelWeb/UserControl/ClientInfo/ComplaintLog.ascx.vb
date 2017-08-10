Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Data.SqlClient

Namespace Visitel.UserControl.ClientInfo
    Public Class ComplaintLogControl
        Inherits BaseUserControl

        Private ControlName As String
        Private objShared As SharedWebControls
        Private objComplaintLog As New ComplaintLogDataObject()
        
        Private LabelLogIdText As String, LabelDateText As String, LabelComplainantText As String, LabelRegardingText As String, LabelOthersInvolvedText As String,
            LabelReportedByText As String, LabelNatureOfProblemsText As String, LabelPersonReceivingComplaintText As String, LabelActionTakenText As String,
            LabelPersonCompletingReportText As String, LabelResolutionMessageText As String, CheckResolutionAgreesText As String, CheckResolutionDisagreesText As String,
            LabelUpdateByText As String, LabelUpdateDateText As String, InsertMessage As String, UpdateMessage As String, DeleteMessage As String, SocialSecurityNumber As String,
            StateClientId As String

        Private RowCount As Integer

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            ControlName = "ComplaintLogControl"
            LabelTitle.Text = "Complaint Log"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            InitializeControl()
            GetCaptionFromResource()

        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("ClientInfo/" & ControlName)
            LoadJavascript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        Private Sub LoadJavascript()
            LoadComplaintLogJavaScript()

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                        & " var DeleteTargetButton3 ='ButtonDeleteComplaintLog'; " _
                        & " var DeleteDialogHeader3 ='Complaint Log'; " _
                        & " var DeleteDialogConfirmMsg3 ='Do you want to delete?'; " _
                        & " var prm =''; " _
                        & " jQuery(document).ready(function () {" _
                        & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                        & "     ComplaintLogDataDelete();" _
                        & "     prm.add_endRequest(ComplaintLogDataDelete); " _
                        & " }); " _
                 & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/ClientInfo/" & ControlName & ".js")
        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()
            HiddenFieldComplaintLogId.ClientIDMode = ClientIDMode.Static
            HiddenFieldComplaintLogIsNew.ClientIDMode = ClientIDMode.Static
            HiddenFieldComplaintLogIndex.ClientIDMode = ClientIDMode.Static

            ButtonClearComplaintLog.ClientIDMode = ClientIDMode.Static
            ButtonOpenReportComplaintLog.ClientIDMode = ClientIDMode.Static
            ButtonSaveComplaintLog.ClientIDMode = ClientIDMode.Static
            ButtonDeleteComplaintLog.ClientIDMode = ClientIDMode.Static

            AddHandler ButtonSaveComplaintLog.Click, AddressOf ButtonSaveComplaintLog_Click
            AddHandler ButtonDeleteComplaintLog.Click, AddressOf ButtonDeleteComplaintLog_Click
            AddHandler ButtonClearComplaintLog.Click, AddressOf ButtonClearComplaintLog_Click

        End Sub

        Public Sub LoadComplaintLogJavaScript()

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                        & " var ComplaintRowCounter = " & RowCount & "; " _
                        & " var CompanyId = " & objShared.CompanyId & "; " _
                        & " var ReportsViewPath ='" & objShared.GetPopupUrl("Reports/ReportsView.aspx") & "'; " _
                        & " var ComplaintCalendarImagePath='" & objShared.GetCalendarImagePath & "'; " _
                        & " var ComplaintCalendarDateFormat='" & objShared.CalendarDateFormat & "'; " _
                        & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

        End Sub

        Public Sub LoadDynamicJavascript(ByRef Control As Control)
            ScriptManager.RegisterClientScriptBlock(Control, Me.GetType(), "ComplaintLogDateFields", "ComplaintRowCounter = " & RowCount & ";ComplaintLogDateFieldsEvent();", True)
        End Sub

        Private Sub ButtonSaveComplaintLog_Click(sender As Object, e As EventArgs)
            Dim isValid As Boolean
            Dim complaintLogs As New List(Of ComplaintLogDataObject)()

            isValid = ValidateAllLogs(complaintLogs)

            If isValid Then
                Dim objBLComplaint As New BLComplaintLog()
                Dim IsSaved As Boolean = False
                Dim SaveErrorMessage As String = String.Empty
                Dim itemNo As Integer = 0

                For Each complaintLog As ComplaintLogDataObject In complaintLogs
                    Try
                        If (complaintLog.ComplaintId = Integer.MinValue) Then
                            SaveErrorMessage = "Unable to Insert New Complaint Log"
                            objBLComplaint.InsertComplaintLog(objShared.ConVisitel, complaintLog)
                            IsSaved = True
                        Else
                            SaveErrorMessage = "Unable to Update Complaint Log with id " & complaintLog.ComplaintId
                            complaintLog.UpdateBy = Convert.ToString(objShared.UserId)
                            objBLComplaint.UpdateComplaintLog(objShared.ConVisitel, complaintLog)
                            IsSaved = True
                        End If
                    Catch ex As SqlException
                        Exit For
                    End Try
                Next

                objBLComplaint = Nothing

                If IsSaved Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("All Complaint Logs Saved Successfully")
                    SetHiddenFields()
                    Session(StaticSettings.SessionField.COMPLAINT_CLEAR_CLICKED) = Nothing
                    GetData()
                Else
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(SaveErrorMessage)
                End If
            Else
                Page.Validate()
            End If
        End Sub

        Private Sub ButtonDeleteComplaintLog_Click(sender As Object, e As EventArgs)

            If (Convert.ToInt64(HiddenFieldComplaintLogId.Value) < 0) Then
                Return
            End If

            Dim ErrorMessage As String = "Unable to Delete"

            Try
                Dim complaintLog As New ComplaintLogDataObject()
                complaintLog.ComplaintId = Convert.ToInt32(HiddenFieldComplaintLogId.Value)

                Dim index As Integer
                index = Convert.ToInt32(HiddenFieldComplaintLogIndex.Value)

                Dim divComplaint As HtmlGenericControl
                divComplaint = MainDiv.FindControl("DivComplaintLog" & index)

                complaintLog.ClientId = objComplaintLog.ClientId
                complaintLog.CompanyId = objShared.CompanyId
                complaintLog.UserId = objShared.UserId

                If complaintLog.ClientId > 0 Then
                    Dim objBLComplaint As New BLComplaintLog()
                    objBLComplaint.DeleteComplaintLog(objShared.ConVisitel, complaintLog)
                    objBLComplaint = Nothing

                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DeleteMessage)
                    GetData()
                End If

            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(ErrorMessage)
            End Try
        End Sub

        Private Sub ButtonClearComplaintLog_Click(sender As Object, e As EventArgs)
            ClearControls()
            SetHiddenFields()
            Session(StaticSettings.SessionField.COMPLAINT_CLEAR_CLICKED) = 1

            GetData()
        End Sub

        Public Sub SetClientId(ByVal clientId As Integer)
            objComplaintLog = New ComplaintLogDataObject()
            objComplaintLog.ClientId = clientId
            objComplaintLog.CompanyId = objShared.CompanyId
            objComplaintLog.UserId = objShared.UserId
        End Sub

        ''' <summary>
        ''' Setting StateClientId from Parent Page
        ''' </summary>
        ''' <param name="StateClientId"></param>
        ''' <remarks></remarks>
        Public Sub SetStateClientId(StateClientId As String)
            Me.StateClientId = StateClientId
        End Sub

        ''' <summary>
        ''' Setting SocialSecurityNumber from Parent Page
        ''' </summary>
        ''' <param name="SocialSecurityNumber"></param>
        ''' <remarks></remarks>
        Public Sub SetSocialSecurityNumber(SocialSecurityNumber As String)
            Me.SocialSecurityNumber = SocialSecurityNumber
        End Sub

        Public Sub LoadComplaintLogs()
            GetData()
        End Sub

        Private Sub SaveSingleLog()
            Dim complaintLog As New ComplaintLogDataObject()
            complaintLog.ComplaintId = Convert.ToInt32(If(String.IsNullOrEmpty(HiddenFieldComplaintLogId.Value), "0", HiddenFieldComplaintLogId.Value))
            complaintLog.CompanyId = objShared.CompanyId
            complaintLog.UserId = objShared.UserId

            Dim textValue As String
            Dim isValid As Boolean = True
            Dim index As Integer
            index = Convert.ToInt32(HiddenFieldComplaintLogIndex.Value)

            Dim divComplaint As HtmlGenericControl
            divComplaint = MainDiv.FindControl("DivComplaintLog" & index)

            textValue = GetTextBoxValue(divComplaint, "TextLogDate" & index).ToString().Trim()

            If String.IsNullOrEmpty(textValue) Then
                SetRequiredControl(divComplaint, "TextLogDate" & index)
                isValid = False
            Else
                complaintLog.ComplaintDate = Convert.ToDateTime(textValue, Nothing)
            End If

            textValue = GetTextBoxValue(divComplaint, "TextComplainant" & index).ToString().Trim()

            If String.IsNullOrEmpty(textValue) Then
                SetRequiredControl(divComplaint, "TextComplainant" & index)
                isValid = False
            Else
                complaintLog.ComplainantName = textValue
            End If

            If isValid Then

                complaintLog.ClientId = Convert.ToInt32(GetHiddenFieldValue(divComplaint, "HiddenComplaintLogClientId" & index), Nothing)
                complaintLog.Regarding = GetTextBoxValue(divComplaint, "TextRegarding" & index)
                complaintLog.OthersInvolved = GetTextBoxValue(divComplaint, "TextOthersInvolved" & index)

                complaintLog.ReportedBy = GetTextBoxValue(divComplaint, "TextReportedBy" & index)
                complaintLog.NatureOfProblems = GetTextBoxValue(divComplaint, "TextNatureOfProblems" & index)
                complaintLog.ComplaintReceiver = GetTextBoxValue(divComplaint, "TextPersonReceivingComplaint" & index)
                complaintLog.ActionTaken = GetTextBoxValue(divComplaint, "TextActionTaken" & index)

                complaintLog.ReportCompletedBy = GetTextBoxValue(divComplaint, "TextPersonCompletingReport" & index)
                complaintLog.Agree = GetCheckBoxValue(divComplaint, "CheckResolutionAgrees" & index)
                complaintLog.Disagree = GetCheckBoxValue(divComplaint, "CheckResolutionDisagrees" & index)
                complaintLog.UpdateBy = GetTextBoxValue(divComplaint, "TextUpdateBy" & index)

                Dim objBLComplaint As New BLComplaintLog()
                Dim IsSaved As Boolean = False
                Dim ErrorMessage As String = String.Empty

                Try
                    If (Convert.ToBoolean(HiddenFieldComplaintLogIsNew.Value, Nothing)) Then
                        ErrorMessage = "Unable to Insert"
                        objBLComplaint.InsertComplaintLog(objShared.ConVisitel, complaintLog)
                        IsSaved = True
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(InsertMessage)
                    Else
                        ErrorMessage = "Unable to Update"
                        complaintLog.UpdateBy = Convert.ToString(objShared.UserId)
                        objBLComplaint.UpdateComplaintLog(objShared.ConVisitel, complaintLog)
                        IsSaved = True
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(UpdateMessage)
                    End If
                Catch ex As SqlException
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(ErrorMessage)
                End Try

                objBLComplaint = Nothing

                If IsSaved Then
                    SetHiddenFields()
                    GetData()
                End If
            Else
                Page.Validate()
            End If
        End Sub

        Private Function ValidateAllLogs(ByRef complaintLogs As List(Of ComplaintLogDataObject))

            Dim isValid As Boolean = True

            For index As Integer = 0 To RowCount
                Dim complaintLog As New ComplaintLogDataObject()
                complaintLog.CompanyId = objShared.CompanyId
                complaintLog.UserId = objShared.UserId

                Dim textValue As String

                Dim divComplaint As HtmlGenericControl
                divComplaint = MainDiv.FindControl("DivComplaintLog" & index)

                textValue = GetTextBoxValue(divComplaint, "TextLogDate" & index).ToString().Trim()

                If String.IsNullOrEmpty(textValue) Then
                    SetRequiredControl(divComplaint, "TextLogDate" & index)
                    isValid = False
                Else
                    complaintLog.ComplaintDate = Convert.ToDateTime(textValue, Nothing)
                End If

                textValue = GetTextBoxValue(divComplaint, "TextComplainant" & index).ToString().Trim()

                If String.IsNullOrEmpty(textValue) Then
                    SetRequiredControl(divComplaint, "TextComplainant" & index)
                    isValid = False
                Else
                    complaintLog.ComplainantName = textValue
                End If

                If (isValid Or index = RowCount) Then
                    GetUpdatedComplaintLog(complaintLog, divComplaint, index)

                    If index = RowCount Then
                        If (complaintLog.ComplaintDate = DateTime.MinValue And String.IsNullOrEmpty(complaintLog.ComplainantName) _
                            And String.IsNullOrEmpty(complaintLog.Regarding) And String.IsNullOrEmpty(complaintLog.OthersInvolved) _
                            And String.IsNullOrEmpty(complaintLog.ReportedBy) And String.IsNullOrEmpty(complaintLog.NatureOfProblems) _
                            And String.IsNullOrEmpty(complaintLog.ComplaintReceiver) And String.IsNullOrEmpty(complaintLog.ActionTaken) _
                            And String.IsNullOrEmpty(complaintLog.ReportCompletedBy) And complaintLog.Agree = False _
                            And complaintLog.Disagree = False And String.IsNullOrEmpty(complaintLog.UpdateBy)) Then

                            isValid = True
                        Else
                            isValid = isValid
                            If isValid Then
                                complaintLogs.Add(complaintLog)
                            End If
                        End If
                    Else
                        complaintLogs.Add(complaintLog)
                    End If
                Else
                    Exit For
                End If
            Next

            ValidateAllLogs = isValid
        End Function

        Private Sub GetUpdatedComplaintLog(ByRef complaintLog As ComplaintLogDataObject, ByVal divComplaint As HtmlGenericControl, ByVal index As Integer)
            Dim textValue As Object

            textValue = GetTextBoxValue(divComplaint, "TextLogId" & index)
            complaintLog.ComplaintId = If(String.IsNullOrEmpty(textValue.ToString()), Integer.MinValue, Convert.ToInt32(textValue, Nothing))
            complaintLog.ClientId = Convert.ToInt32(GetHiddenFieldValue(divComplaint, "HiddenComplaintLogClientId" & index), Nothing)
            complaintLog.Regarding = GetTextBoxValue(divComplaint, "TextRegarding" & index)
            complaintLog.OthersInvolved = GetTextBoxValue(divComplaint, "TextOthersInvolved" & index)

            complaintLog.ReportedBy = GetTextBoxValue(divComplaint, "TextReportedBy" & index)
            complaintLog.NatureOfProblems = GetTextBoxValue(divComplaint, "TextNatureOfProblems" & index)
            complaintLog.ComplaintReceiver = GetTextBoxValue(divComplaint, "TextPersonReceivingComplaint" & index)
            complaintLog.ActionTaken = GetTextBoxValue(divComplaint, "TextActionTaken" & index)

            complaintLog.ReportCompletedBy = GetTextBoxValue(divComplaint, "TextPersonCompletingReport" & index)
            complaintLog.Agree = GetCheckBoxValue(divComplaint, "CheckResolutionAgrees" & index)
            complaintLog.Disagree = GetCheckBoxValue(divComplaint, "CheckResolutionDisagrees" & index)
            complaintLog.UpdateBy = GetTextBoxValue(divComplaint, "TextUpdateBy" & index)

        End Sub

        Private Sub GetData()

            Dim complaintLogs As New List(Of ComplaintLogDataObject)()

            If objComplaintLog.ClientId > 0 And (Session(StaticSettings.SessionField.COMPLAINT_CLEAR_CLICKED) Is Nothing Or Not Session(StaticSettings.SessionField.COMPLAINT_CLEAR_CLICKED) = 1) Then
                Dim objBLComplaintLog As New BLComplaintLog()
                complaintLogs = objBLComplaintLog.SelectComplaintLogs(objShared.ConVisitel, objComplaintLog)
                objBLComplaintLog = Nothing
            End If

            RowCount = complaintLogs.Count
            ClearControls()

            RowCount = 0
            For Each item As ComplaintLogDataObject In complaintLogs
                CreateControls(item, RowCount)
                RowCount = RowCount + 1
            Next

            Dim blankComplaint As New ComplaintLogDataObject()
            blankComplaint.ClientId = objComplaintLog.ClientId

            CreateControls(blankComplaint, RowCount)
            RowCount = complaintLogs.Count

        End Sub

        Private Sub CreateControls(complaintLog As ComplaintLogDataObject, index As Integer)

            Dim row60Css As String = "column060 columnLeft", row80Css As String = "column080 columnLeft",
                row100Css As String = "column100 columnLeft", row150Css As String = "column150 columnLeft",
                row200Css As String = "column200 columnLeft", row250Css As String = "column250 columnLeft",
                row300Css As String = "column300 columnLeft", row350Css As String = "column350 columnLeft",
                autoColumnCss As String = "columnAuto columnLeft", dateField As String = "dateField",
                text100Css As String = "width100", text150Css As String = "width150",
                text200Css As String = "width200", text250Css As String = "width250",
                text300Css As String = "width300", text350Css As String = "width350",
                text400Css As String = "width400", text450Css As String = "width450",
                text500Css As String = "width500", text550Css As String = "width550",
                text600Css As String = "width600", text650Css As String = "width650",
                text700Css As String = "width700", text750Css As String = "width750",
                text800Css As String = "width800", text850Css As String = "width850"

            Dim ComplaintLogBox As New HtmlGenericControl(objShared.GenericControlRow)
            ComplaintLogBox.Attributes.Add("class", "ComplaintLogServiceBox")
            MainDiv.Controls.Add(ComplaintLogBox)

            Dim UpdatePanelComplaintLog As New UpdatePanel()
            UpdatePanelComplaintLog.ID = "UpdatePanelComplaintLog" & index
            UpdatePanelComplaintLog.UpdateMode = UpdatePanelUpdateMode.Always
            UpdatePanelComplaintLog.Attributes.Add("OnClick", "SetComplaintLogId(" & index & ")")
            ComplaintLogBox.Controls.Add(UpdatePanelComplaintLog)

            Dim ServiceBoxComplaintLog As New HtmlGenericControl(objShared.GenericControlRow)
            ServiceBoxComplaintLog.ID = "DivComplaintLog" & index
            ServiceBoxComplaintLog.Attributes.Add("class", "ComplaintLogBoxStyle ServiceFullComplaintLog ServiceBoxComplaintLog")
            UpdatePanelComplaintLog.ContentTemplateContainer.Controls.Add(ServiceBoxComplaintLog)

            Dim parentRow As HtmlGenericControl
            parentRow = New HtmlGenericControl(objShared.GenericControlRow)
            parentRow.Attributes.Add("class", "newRow")

            Dim complaintValueStr As String
            complaintValueStr = If(complaintLog.ComplaintId = Integer.MinValue, String.Empty, complaintLog.ComplaintId.ToString())
            parentRow.Controls.Add(AddLabel(index, autoColumnCss, "LabelLogId", LabelLogIdText, ""))
            parentRow.Controls.Add(AddTextBox(index, row150Css, "TextLogId", complaintValueStr, text100Css, True, False, True))

            complaintValueStr = If(complaintLog.ComplaintDate = DateTime.MinValue, String.Empty, complaintLog.ComplaintDate.ToString(objShared.DateFormat))
            parentRow.Controls.Add(AddLabel(index, autoColumnCss, "LabelLogDate", LabelDateText, ""))
            parentRow.Controls.Add(AddTextBox(index, row200Css, "TextLogDate", complaintValueStr, dateField, False, True, True))
            ServiceBoxComplaintLog.Controls.Add(parentRow)

            parentRow = New HtmlGenericControl(objShared.GenericControlRow)
            parentRow.Attributes.Add("class", "newRow")
            parentRow.Controls.Add(AddLabel(index, row150Css, "LabelComplainant", LabelComplainantText, ""))
            parentRow.Controls.Add(AddTextBox(index, autoColumnCss, "TextComplainant", complaintLog.ComplainantName, text750Css, False, True))
            ServiceBoxComplaintLog.Controls.Add(parentRow)

            parentRow = New HtmlGenericControl(objShared.GenericControlRow)
            parentRow.Attributes.Add("class", "newRow")
            parentRow.Controls.Add(AddLabel(index, row150Css, "LabelRegarding", LabelRegardingText, ""))
            parentRow.Controls.Add(AddTextBox(index, autoColumnCss, "TextRegarding", complaintLog.Regarding, text750Css, False, False))
            ServiceBoxComplaintLog.Controls.Add(parentRow)

            parentRow = New HtmlGenericControl(objShared.GenericControlRow)
            parentRow.Attributes.Add("class", "newRow")
            parentRow.Controls.Add(AddLabel(index, row250Css, "LabelOthersInvolved", LabelOthersInvolvedText, ""))
            parentRow.Controls.Add(AddTextBox(index, autoColumnCss, "TextOthersInvolved", complaintLog.OthersInvolved, text650Css, False, False))
            ServiceBoxComplaintLog.Controls.Add(parentRow)

            parentRow = New HtmlGenericControl(objShared.GenericControlRow)
            parentRow.Attributes.Add("class", "newRow")
            parentRow.Controls.Add(AddLabel(index, row150Css, "LabelReportedBy", LabelReportedByText, ""))
            parentRow.Controls.Add(AddTextBox(index, autoColumnCss, "TextReportedBy", complaintLog.ReportedBy, text750Css, False, False))
            ServiceBoxComplaintLog.Controls.Add(parentRow)

            parentRow = New HtmlGenericControl(objShared.GenericControlRow)
            parentRow.Attributes.Add("class", "newRow")
            parentRow.Controls.Add(AddLabel(index, row150Css, "LabelNatureOfProblems", LabelNatureOfProblemsText, ""))
            parentRow.Controls.Add(AddTextBox(index, autoColumnCss, "TextNatureOfProblems", complaintLog.NatureOfProblems, text750Css, False, False))
            ServiceBoxComplaintLog.Controls.Add(parentRow)

            parentRow = New HtmlGenericControl(objShared.GenericControlRow)
            parentRow.Attributes.Add("class", "newRow")
            parentRow.Controls.Add(AddLabel(index, row300Css, "LabelPersonReceivingComplaint", LabelPersonReceivingComplaintText, ""))
            parentRow.Controls.Add(AddTextBox(index, autoColumnCss, "TextPersonReceivingComplaint", complaintLog.ComplaintReceiver, text600Css, False, False))
            ServiceBoxComplaintLog.Controls.Add(parentRow)

            parentRow = New HtmlGenericControl(objShared.GenericControlRow)
            parentRow.Attributes.Add("class", "newRow")
            parentRow.Controls.Add(AddLabel(index, row150Css, "LabelActionTaken", LabelActionTakenText, ""))
            parentRow.Controls.Add(AddTextBox(index, autoColumnCss, "TextActionTaken", complaintLog.ActionTaken, text750Css, False, False))
            ServiceBoxComplaintLog.Controls.Add(parentRow)

            parentRow = New HtmlGenericControl(objShared.GenericControlRow)
            parentRow.Attributes.Add("class", "newRow")
            parentRow.Controls.Add(AddLabel(index, row200Css, "LabelPersonCompletingReport", LabelPersonCompletingReportText, ""))
            parentRow.Controls.Add(AddTextBox(index, autoColumnCss, "TextPersonCompletingReport", complaintLog.ReportCompletedBy, text700Css, False, False))
            ServiceBoxComplaintLog.Controls.Add(parentRow)

            parentRow = New HtmlGenericControl(objShared.GenericControlRow)
            parentRow.Attributes.Add("class", "newRow")
            parentRow.Controls.Add(AddLabel(index, row200Css, "LabelResolutionMessage", LabelResolutionMessageText, ""))
            ServiceBoxComplaintLog.Controls.Add(parentRow)

            parentRow = New HtmlGenericControl(objShared.GenericControlRow)
            parentRow.Attributes.Add("class", "newRow")
            parentRow.Controls.Add(AddCheckBox(index, autoColumnCss, "CheckResolutionAgrees", CheckResolutionAgreesText, complaintLog.Agree, ""))
            parentRow.Controls.Add(AddCheckBox(index, autoColumnCss, "CheckResolutionDisagrees", CheckResolutionDisagreesText, complaintLog.Disagree, ""))
            ServiceBoxComplaintLog.Controls.Add(parentRow)

            parentRow = New HtmlGenericControl(objShared.GenericControlRow)
            parentRow.Attributes.Add("class", "newRow")
            parentRow.Controls.Add(AddLabel(index, row80Css, "LabelUpdateBy", LabelUpdateByText, ""))
            parentRow.Controls.Add(AddTextBox(index, row150Css, "TextUpdateBy", complaintLog.UpdateBy, text150Css, True, False))
            parentRow.Controls.Add(AddRow(autoColumnCss, "&nbsp;"))

            complaintValueStr = If(complaintLog.UpdateDate = DateTime.MinValue, String.Empty, complaintLog.UpdateDate.ToString(objShared.DateFormat))
            parentRow.Controls.Add(AddLabel(index, row80Css, "LabelUpdateDate", LabelUpdateDateText, ""))
            parentRow.Controls.Add(AddTextBox(index, row150Css, "TextUpdateDate", complaintValueStr, text150Css, True, False))
            ServiceBoxComplaintLog.Controls.Add(parentRow)

            parentRow = New HtmlGenericControl(objShared.GenericControlRow)
            parentRow.Attributes.Add("class", "newRow")
            Dim divSpace As New HtmlGenericControl(objShared.GenericControlRow)
            divSpace.Attributes.Add("class", "divSpace03")
            parentRow.Controls.Add(divSpace)

            Dim hiddenClientId As New HiddenField()
            hiddenClientId.ID = "HiddenComplaintLogClientId" & index
            hiddenClientId.Value = complaintLog.ClientId.ToString()
            parentRow.Controls.Add(hiddenClientId)

            ServiceBoxComplaintLog.Controls.Add(parentRow)

        End Sub

        Private Function AddRow(ByVal rowCss As String, ByVal text As String)
            Dim childRow = New HtmlGenericControl(objShared.GenericControlRow)
            childRow.Attributes.Add("class", rowCss)
            childRow.InnerHtml = text

            AddRow = childRow
        End Function

        Private Function AddLabel(ByVal index As Integer, ByVal rowCss As String, ByVal controlId As String, ByVal controlText As String, ByVal controlCss As String)
            Dim labelCaption As New Label
            labelCaption.ID = controlId & index
            labelCaption.Text = controlText

            If (Not String.IsNullOrEmpty(controlCss)) Then
                labelCaption.CssClass = controlCss
            End If

            Dim childRow = AddRow(rowCss, "")
            childRow.Controls.Add(labelCaption)

            AddLabel = childRow
        End Function

        Private Function AddTextBox(ByVal index As Integer, ByVal rowCss As String, ByVal controlId As String, ByVal controlText As String, ByVal controlCss As String,
                                    ByVal isReadOnly As Boolean, ByVal isRequired As Boolean, Optional isStatic As Boolean = False)
            Dim textValue As New TextBox
            textValue.ID = controlId & index
            textValue.Text = controlText
            textValue.ReadOnly = isReadOnly

            If (Not String.IsNullOrEmpty(controlCss)) Then
                textValue.CssClass = controlCss
            End If

            If (isStatic) Then
                textValue.ClientIDMode = UI.ClientIDMode.Static
            End If

            Dim childRow = AddRow(rowCss, "")
            childRow.Controls.Add(textValue)

            If (isRequired) Then
                Dim RequiredFieldValidatorControl = New RequiredFieldValidator()
                RequiredFieldValidatorControl.ID = "Required" & controlId & index
                RequiredFieldValidatorControl.ControlToValidate = textValue.ID
                RequiredFieldValidatorControl.ErrorMessage = "*"
                RequiredFieldValidatorControl.SetFocusOnError = True
                RequiredFieldValidatorControl.ForeColor = Drawing.Color.Red
                RequiredFieldValidatorControl.Enabled = False
                childRow.Controls.Add(RequiredFieldValidatorControl)
            End If

            AddTextBox = childRow
        End Function

        Private Function AddCheckBox(ByVal index As Integer, ByVal rowCss As String, ByVal controlId As String, ByVal controlText As String, ByVal isChecked As Boolean,
                                     ByVal controlCss As String)
            Dim checkValue As New CheckBox
            checkValue.ID = controlId & index
            checkValue.CssClass = controlCss
            checkValue.Text = controlText
            checkValue.Checked = isChecked

            Dim childRow = AddRow(rowCss, "")
            childRow.Controls.Add(checkValue)

            AddCheckBox = childRow
        End Function

        Private Function GetTextBoxValue(ByVal parentControl As HtmlGenericControl, ByVal textBoxId As String)
            Dim textValue As TextBox
            textValue = parentControl.FindControl(textBoxId)

            GetTextBoxValue = textValue.Text
        End Function

        Private Function GetCheckBoxValue(ByVal parentControl As HtmlGenericControl, ByVal checkBoxId As String)
            Dim checkValue As CheckBox
            checkValue = parentControl.FindControl(checkBoxId)

            GetCheckBoxValue = checkValue.Checked
        End Function

        Private Function GetHiddenFieldValue(ByVal parentControl As HtmlGenericControl, ByVal hiddenFieldId As String)
            Dim hiddenValue As HiddenField
            hiddenValue = parentControl.FindControl(hiddenFieldId)

            GetHiddenFieldValue = hiddenValue.Value
        End Function

        Private Sub SetRequiredControl(ByVal parentControl As HtmlGenericControl, ByVal controlId As String)
            Dim requiredControl As RequiredFieldValidator
            requiredControl = parentControl.FindControl("Required" & controlId)
            requiredControl.Enabled = True
        End Sub

        Public Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("ClientInfo", "ComplaintLogControl" & Convert.ToString(".resx"))

            LabelLogIdText = Convert.ToString(ResourceTable("LabelLogId"), Nothing)
            LabelLogIdText = If(LabelLogIdText.Trim().Equals(String.Empty), "Log Id:", LabelLogIdText)

            LabelDateText = Convert.ToString(ResourceTable("LabelDate"), Nothing)
            LabelDateText = If(LabelDateText.Trim().Equals(String.Empty), "Date:", LabelDateText)

            LabelComplainantText = Convert.ToString(ResourceTable("LabelComplainant"), Nothing)
            LabelComplainantText = If(LabelComplainantText.Trim().Equals(String.Empty), "Complainant:", LabelComplainantText)

            LabelRegardingText = Convert.ToString(ResourceTable("LabelRegarding"), Nothing)
            LabelRegardingText = If(LabelRegardingText.Trim().Equals(String.Empty), "Regarding:", LabelRegardingText)

            LabelOthersInvolvedText = Convert.ToString(ResourceTable("LabelOthersInvolved"), Nothing)
            LabelOthersInvolvedText = If(LabelOthersInvolvedText.Trim().Equals(String.Empty), "Others Involved (If Applicable):", LabelOthersInvolvedText)

            LabelReportedByText = Convert.ToString(ResourceTable("LabelReportedBy"), Nothing)
            LabelReportedByText = If(LabelReportedByText.Trim().Equals(String.Empty), "Reported By:", LabelReportedByText)

            LabelNatureOfProblemsText = Convert.ToString(ResourceTable("LabelNatureOfProblems"), Nothing)
            LabelNatureOfProblemsText = If(LabelNatureOfProblemsText.Trim().Equals(String.Empty), "Nature of Problems:", LabelNatureOfProblemsText)

            LabelPersonReceivingComplaintText = Convert.ToString(ResourceTable("LabelPersonReceivingComplaint"), Nothing)
            LabelPersonReceivingComplaintText = If(LabelPersonReceivingComplaintText.Trim().Equals(String.Empty),
                                                   "Person Receiving Complaint (Name _ Title):", LabelPersonReceivingComplaintText)

            LabelActionTakenText = Convert.ToString(ResourceTable("LabelActionTaken"), Nothing)
            LabelActionTakenText = If(LabelActionTakenText.Trim().Equals(String.Empty), "Action Taken:", LabelActionTakenText)

            LabelPersonCompletingReportText = Convert.ToString(ResourceTable("LabelPersonCompletingReport"), Nothing)
            LabelPersonCompletingReportText = If(LabelPersonCompletingReportText.Trim().Equals(String.Empty), "Person Completing Report:", LabelPersonCompletingReportText)

            LabelResolutionMessageText = Convert.ToString(ResourceTable("LabelResolutionMessage"), Nothing)
            LabelResolutionMessageText = If(LabelResolutionMessageText.Trim().Equals(String.Empty),
                                            "The nature of the complaint and it's resolution have been discussed with complaint who", LabelResolutionMessageText)

            CheckResolutionAgreesText = Convert.ToString(ResourceTable("CheckResolutionAgrees"), Nothing)
            CheckResolutionAgreesText = If(CheckResolutionAgreesText.Trim().Equals(String.Empty), "Agrees with it's resolution", CheckResolutionAgreesText)

            CheckResolutionDisagreesText = Convert.ToString(ResourceTable("CheckResolutionDisagrees"), Nothing)
            CheckResolutionDisagreesText = If(CheckResolutionDisagreesText.Trim().Equals(String.Empty), "Disagrees with it's resolution", CheckResolutionDisagreesText)

            LabelUpdateByText = Convert.ToString(ResourceTable("LabelUpdateBy"), Nothing)
            LabelUpdateByText = If(LabelUpdateByText.Trim().Equals(String.Empty), "Update By:", LabelUpdateByText)

            LabelUpdateDateText = Convert.ToString(ResourceTable("LabelUpdateDate"), Nothing)
            LabelUpdateDateText = If(LabelUpdateDateText.Trim().Equals(String.Empty), "Update Date:", LabelUpdateDateText)

            ButtonSaveComplaintLog.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)
            ButtonSaveComplaintLog.Text = If(ButtonSaveComplaintLog.Text.Trim().Equals(String.Empty), "Save", ButtonSaveComplaintLog.Text)

            ButtonDeleteComplaintLog.Text = Convert.ToString(ResourceTable("ButtonDelete"), Nothing)
            ButtonDeleteComplaintLog.Text = If(ButtonDeleteComplaintLog.Text.Trim().Equals(String.Empty), "Delete", ButtonDeleteComplaintLog.Text)

            ButtonClearComplaintLog.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing)
            ButtonClearComplaintLog.Text = If(ButtonClearComplaintLog.Text.Trim().Equals(String.Empty), "Clear", ButtonClearComplaintLog.Text)

            ButtonOpenReportComplaintLog.Text = Convert.ToString(ResourceTable("ButtonOpenReport"), Nothing)
            ButtonOpenReportComplaintLog.Text = If(ButtonOpenReportComplaintLog.Text.Trim().Equals(String.Empty), "Open Report", ButtonOpenReportComplaintLog.Text)

            InsertMessage = Convert.ToString(ResourceTable("InsertMessage"), Nothing)
            InsertMessage = If(InsertMessage.Trim().Equals(String.Empty), "Inserted Successfully", InsertMessage)

            UpdateMessage = Convert.ToString(ResourceTable("UpdateMessage"), Nothing)
            UpdateMessage = If(UpdateMessage.Trim().Equals(String.Empty), "Updated Successfully", UpdateMessage)

            DeleteMessage = Convert.ToString(ResourceTable("DeleteMessage"), Nothing)
            DeleteMessage = If(DeleteMessage.Trim().Equals(String.Empty), "Deleted Successfully", DeleteMessage)

            ResourceTable = Nothing

            'ButtonDeleteComplaintLog.Attributes("onClick") = String.Format("return ComplaintDeleteConfirmation('Do you want to delete? ');")

        End Sub

        Private Sub ClearControls()
            MainDiv.Controls.Clear()

            If Not IsPostBack Then
                SetHiddenFields()
            End If

        End Sub

        Private Sub SetHiddenFields()
            HiddenFieldComplaintLogId.Value = Convert.ToString(Integer.MinValue)
            HiddenFieldComplaintLogIsNew.Value = Convert.ToString(True, Nothing)
            HiddenFieldComplaintLogIndex.Value = Convert.ToString(RowCount)
        End Sub
    End Class
End Namespace