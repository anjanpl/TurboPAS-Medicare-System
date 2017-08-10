Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Data.SqlClient

Namespace Visitel.UserControl.ClientInfo

    Public Class ServiceControl
        Inherits BaseUserControl

        Private ControlName As String, InvalidUnitRateMessage As String, ValidationGroup As String
        Private CurrentRow As GridViewRow

        Private ValidationEnable As Boolean

        Private TextBoxName As TextBox, TextBoxServiceCodeDescription As TextBox, TextBoxClientInfoId As TextBox, TextBoxClientId As TextBox, TextBoxStatus As TextBox, _
            TextBoxAuthorizationNumber As TextBox, TextBoxAuthorizationStartDate As TextBox, TextBoxAuthorizationEndDate As TextBox

        Private LabelHeaderName As Label, LabelHeaderServiceCodeDescription As Label, LabelHeaderClientInfoId As Label, LabelHeaderClientId As Label, _
            LabelHeaderStatus As Label, LabelHeaderAuthorizationNumber As Label, LabelHeaderAuthorizationStartDate As Label, LabelHeaderAuthorizationEndDate As Label

        Private IndividualId As Int64

        Private CheckBoxSelect As CheckBox, chkAll As CheckBox

        Private GridResults As List(Of ClientServiceInfoDataObject)

        Private objShared As SharedWebControls

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "ServiceControl"
            objShared = New SharedWebControls()
            GetCaptionFromResource()
            objShared.ConnectionOpen()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If Not IsPostBack Then
                Session.Add("SortingExpression", "Name")
                GetData()
            End If
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("ClientInfo/" & ControlName)
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Public Sub GetData()
            BindServiceGridView()
        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()
            GridViewService.AutoGenerateColumns = False
            GridViewService.ShowHeaderWhenEmpty = True
            GridViewService.AllowPaging = True
            GridViewService.AllowSorting = True

            GridViewService.ShowFooter = False

            If (GridViewService.AllowPaging) Then
                GridViewService.PageSize = objShared.GridViewDefaultPageSize
            End If

            AddHandler GridViewService.RowDataBound, AddressOf GridViewService_RowDataBound
            AddHandler GridViewService.PageIndexChanging, AddressOf GridViewService_PageIndexChanging
            AddHandler GridViewService.Sorting, AddressOf GridViewService_Sorting
        End Sub

        'Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)

        '    Dim isUpdateVisible As Boolean = False

        '    Dim chk As CheckBox = TryCast(sender, CheckBox)

        '    'ButtonSave.Enabled = If((chk.Checked), True, False)

        '    If chk.ID = "chkAll" Then
        '        For Each row As GridViewRow In GridViewService.Rows
        '            If row.RowType = DataControlRowType.DataRow Then
        '                row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked = chk.Checked
        '            End If
        '        Next

        '        CurrentRow = GridViewService.FooterRow
        '        CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
        '        CheckBoxSelect.Checked = chk.Checked

        '    End If

        '    Dim chkAll As CheckBox = TryCast(GridViewService.HeaderRow.FindControl("chkAll"), CheckBox)
        '    'chkAll.Checked = True
        '    For Each row As GridViewRow In GridViewService.Rows
        '        If row.RowType = DataControlRowType.DataRow Then

        '            Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

        '            'ControlsOnSelect(row, isChecked)

        '            If (isChecked) Then
        '                'SetHiddenControlValue(row)
        '            End If

        '            For i As Integer = 1 To row.Cells.Count - 1

        '                If row.Cells(i).Controls.OfType(Of DropDownList)().ToList().Count > 0 Then
        '                    row.Cells(i).Controls.OfType(Of DropDownList)().FirstOrDefault().Visible = isChecked
        '                End If
        '                If isChecked AndAlso Not isUpdateVisible Then
        '                    isUpdateVisible = True
        '                End If
        '                If Not isChecked Then
        '                    chkAll.Checked = False
        '                End If
        '            Next
        '        End If
        '    Next

        '    'DetermineButtonInActivity()

        'End Sub

        ''' <summary>
        ''' Reading GridView header text from resource file
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <remarks></remarks>
        Private Sub SetGridViewColumnHeaderText(ByRef CurrentRow As GridViewRow)

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EDICodes", ControlName & Convert.ToString(".resx"))

            'Dim LabelHeaderSerial As Label = DirectCast(CurrentRow.FindControl("LabelHeaderSerial"), Label)
            'LabelHeaderSerial.Text = Convert.ToString(ResourceTable("LabelHeaderSerial"), Nothing).Trim()
            'LabelHeaderSerial.Text = If(String.IsNullOrEmpty(LabelHeaderSerial.Text), "SI.", LabelHeaderSerial.Text)

            'Dim LabelHeaderSelect As Label = DirectCast(CurrentRow.FindControl("LabelHeaderSelect"), Label)
            'LabelHeaderSelect.Text = Convert.ToString(ResourceTable("LabelHeaderSelect"), Nothing).Trim()
            'LabelHeaderSelect.Text = If(String.IsNullOrEmpty(LabelHeaderSelect.Text), "Select", LabelHeaderSelect.Text)

            LabelHeaderName = DirectCast(CurrentRow.FindControl("LabelHeaderName"), Label)
            LabelHeaderName.Text = Convert.ToString(ResourceTable("LabelHeaderName"), Nothing).Trim()
            LabelHeaderName.Text = If(String.IsNullOrEmpty(LabelHeaderName.Text), "Name", LabelHeaderName.Text)

            LabelHeaderServiceCodeDescription = DirectCast(CurrentRow.FindControl("LabelHeaderServiceCodeDescription"), Label)
            LabelHeaderServiceCodeDescription.Text = Convert.ToString(ResourceTable("LabelHeaderServiceCodeDescription"), Nothing).Trim()
            LabelHeaderServiceCodeDescription.Text = If(String.IsNullOrEmpty(LabelHeaderServiceCodeDescription.Text), "Service Code Description",
                                                         LabelHeaderServiceCodeDescription.Text)

            LabelHeaderClientInfoId = DirectCast(CurrentRow.FindControl("LabelHeaderClientInfoId"), Label)
            LabelHeaderClientInfoId.Text = Convert.ToString(ResourceTable("LabelHeaderClientInfoId"), Nothing).Trim()
            LabelHeaderClientInfoId.Text = If(String.IsNullOrEmpty(LabelHeaderClientInfoId.Text), "Client Info Id", LabelHeaderClientInfoId.Text)

            LabelHeaderClientId = DirectCast(CurrentRow.FindControl("LabelHeaderClientId"), Label)
            LabelHeaderClientId.Text = Convert.ToString(ResourceTable("LabelHeaderClientId"), Nothing).Trim()
            LabelHeaderClientId.Text = If(String.IsNullOrEmpty(LabelHeaderClientId.Text), "Client Id", LabelHeaderClientId.Text)

            LabelHeaderStatus = DirectCast(CurrentRow.FindControl("LabelHeaderStatus"), Label)
            LabelHeaderStatus.Text = Convert.ToString(ResourceTable("LabelHeaderStatus"), Nothing).Trim()
            LabelHeaderStatus.Text = If(String.IsNullOrEmpty(LabelHeaderStatus.Text), "Status", LabelHeaderStatus.Text)

            LabelHeaderAuthorizationNumber = DirectCast(CurrentRow.FindControl("LabelHeaderAuthorizationNumber"), Label)
            LabelHeaderAuthorizationNumber.Text = Convert.ToString(ResourceTable("LabelHeaderAuthorizationNumber"), Nothing).Trim()
            LabelHeaderAuthorizationNumber.Text = If(String.IsNullOrEmpty(LabelHeaderAuthorizationNumber.Text), "Auth_No", LabelHeaderAuthorizationNumber.Text)


            LabelHeaderAuthorizationStartDate = DirectCast(CurrentRow.FindControl("LabelHeaderAuthorizationStartDate"), Label)
            LabelHeaderAuthorizationStartDate.Text = Convert.ToString(ResourceTable("LabelHeaderAuthorizationStartDate"), Nothing).Trim()
            LabelHeaderAuthorizationStartDate.Text = If(String.IsNullOrEmpty(LabelHeaderAuthorizationStartDate.Text), "AuthStart",
                                                         LabelHeaderAuthorizationStartDate.Text)

            LabelHeaderAuthorizationEndDate = DirectCast(CurrentRow.FindControl("LabelHeaderAuthorizationEndDate"), Label)
            LabelHeaderAuthorizationEndDate.Text = Convert.ToString(ResourceTable("LabelHeaderAuthorizationEndDate"), Nothing).Trim()
            LabelHeaderAuthorizationEndDate.Text = If(String.IsNullOrEmpty(LabelHeaderAuthorizationEndDate.Text), "AuthEnd",
                                                         LabelHeaderAuthorizationEndDate.Text)

            ResourceTable = Nothing

        End Sub

        Private Sub GridViewService_RowDataBound(sender As Object, e As GridViewRowEventArgs)

            CurrentRow = DirectCast(e.Row, GridViewRow)

            If (CurrentRow.RowType.Equals(DataControlRowType.Header)) Then
                SetGridViewColumnHeaderText(CurrentRow)

                'chkAll = DirectCast(CurrentRow.FindControl("chkAll"), CheckBox)
                'chkAll.AutoPostBack = True
                'chkAll.ClientIDMode = UI.ClientIDMode.Static

                'ButtonHeaderName = DirectCast(CurrentRow.FindControl("ButtonHeaderName"), Button)
                'ButtonHeaderName.CommandName = "Sort"
                'ButtonHeaderName.CommandArgument = "Name"

                'ButtonHeaderServiceCodeDescription = DirectCast(CurrentRow.FindControl("ButtonHeaderServiceCodeDescription"), Button)
                'ButtonHeaderServiceCodeDescription.CommandName = "Sort"
                'ButtonHeaderServiceCodeDescription.CommandArgument = "ServiceCodeDescription"

                'ButtonHeaderClientInfoId = DirectCast(CurrentRow.FindControl("ButtonHeaderClientInfoId"), Button)
                'ButtonHeaderClientInfoId.CommandName = "Sort"
                'ButtonHeaderClientInfoId.CommandArgument = "ClientInfoId"

                'ButtonHeaderClientId = DirectCast(CurrentRow.FindControl("ButtonHeaderClientId"), Button)
                'ButtonHeaderClientId.CommandName = "Sort"
                'ButtonHeaderClientId.CommandArgument = "ClientId"

                'ButtonHeaderClientId = DirectCast(CurrentRow.FindControl("ButtonHeaderClientId"), Button)
                'ButtonHeaderClientId.CommandName = "Sort"
                'ButtonHeaderClientId.CommandArgument = "ClientId"

                'ButtonHeaderStatus = DirectCast(CurrentRow.FindControl("ButtonHeaderStatus"), Button)
                'ButtonHeaderStatus.CommandName = "Sort"
                'ButtonHeaderStatus.CommandArgument = "Status"

                'ButtonHeaderAuthorizationNumber = DirectCast(CurrentRow.FindControl("ButtonHeaderAuthorizationNumber"), Button)
                'ButtonHeaderAuthorizationNumber.CommandName = "Sort"
                'ButtonHeaderAuthorizationNumber.CommandArgument = "AuthorizationNumber"

                'ButtonHeaderAuthorizationStartDate = DirectCast(CurrentRow.FindControl("ButtonHeaderAuthorizationStartDate"), Button)
                'ButtonHeaderAuthorizationStartDate.CommandName = "Sort"
                'ButtonHeaderAuthorizationStartDate.CommandArgument = "AuthorizationStartDate"

                'ButtonHeaderAuthorizationEndDate = DirectCast(CurrentRow.FindControl("ButtonHeaderAuthorizationEndDate"), Button)
                'ButtonHeaderAuthorizationEndDate.CommandName = "Sort"
                'ButtonHeaderAuthorizationEndDate.CommandArgument = "AuthorizationEndDate"

            End If

            If (CurrentRow.RowType.Equals(DataControlRowType.DataRow)) Then
                'CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                'CheckBoxSelect.AutoPostBack = True

                TextBoxName = DirectCast(CurrentRow.FindControl("TextBoxName"), TextBox)
                TextBoxName.ReadOnly = True

                TextBoxServiceCodeDescription = DirectCast(CurrentRow.FindControl("TextBoxServiceCodeDescription"), TextBox)
                TextBoxServiceCodeDescription.ReadOnly = True

                TextBoxClientInfoId = DirectCast(CurrentRow.FindControl("TextBoxClientInfoId"), TextBox)
                TextBoxClientInfoId.ReadOnly = True

                TextBoxClientId = DirectCast(CurrentRow.FindControl("TextBoxClientId"), TextBox)
                TextBoxClientId.ReadOnly = True

                TextBoxStatus = DirectCast(CurrentRow.FindControl("TextBoxStatus"), TextBox)
                TextBoxStatus.ReadOnly = True

                TextBoxAuthorizationNumber = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationNumber"), TextBox)
                TextBoxAuthorizationNumber.ReadOnly = True

                TextBoxAuthorizationStartDate = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationStartDate"), TextBox)
                TextBoxAuthorizationStartDate.ReadOnly = True

                TextBoxAuthorizationEndDate = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationEndDate"), TextBox)
                TextBoxAuthorizationEndDate.ReadOnly = True
            End If

            If (CurrentRow.RowType.Equals(DataControlRowType.Footer)) Then
                'CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                'CheckBoxSelect.AutoPostBack = True
            End If

        End Sub

        Private Sub GridViewService_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewService.PageIndex = e.NewPageIndex
            'BindEDICodesGridView()
        End Sub

        Private Sub GridViewService_Sorting(sender As Object, e As GridViewSortEventArgs)
            Session.Add("SortingExpression", e.SortExpression)
            BindServiceGridView()
        End Sub

        ''' <summary>
        ''' EDI Codes Grid Data Bind
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindServiceGridView()

            Dim objBLClientInfo As New BLClientInfo()

            Try
                GridResults = objBLClientInfo.SelectClientService(objShared.ConVisitel, Me.IndividualId)
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to fetch Client Services")
            Finally
                'GridViewEDICodes.ShowFooter = objBLEDICodes.ShowFooter

                'If (Not objBLEDICodes.ShowFooter) Then
                '    GridResults.Clear()
                'End If

                'objBLClientInfo = Nothing
            End Try

            'GetSortedEDICodes(GridResults)

            GridViewService.DataSource = GridResults
            GridViewService.DataBind()

            'DetermineButtonInActivity()

        End Sub

        Private Sub SetControlTabIndex()
            'TextBoxAuthorizationFromDate.TabIndex = 38
            'TextBoxAuthorizationToDate.TabIndex = 39
        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("ClientInfo", ControlName & Convert.ToString(".resx"))

            LabelService.Text = Convert.ToString(ResourceTable("LabelService"), Nothing).Trim()
            LabelService.Text = If(String.IsNullOrEmpty(LabelService.Text), "Service", LabelService.Text)

            'LabelAuthorizationFromDate.Text = Convert.ToString(ResourceTable("LabelAuthorizationFromDate"), Nothing).Trim()
            'LabelAuthorizationFromDate.Text = If(String.IsNullOrEmpty(LabelAuthorizationFromDate.Text), "From:", LabelAuthorizationFromDate.Text)

            'LabelAuthorizationToDate.Text = Convert.ToString(ResourceTable("LabelAuthorizationToDate"), Nothing).Trim()
            'LabelAuthorizationToDate.Text = If(String.IsNullOrEmpty(LabelAuthorizationToDate.Text), "To:", LabelAuthorizationToDate.Text)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Public Function ValidateData() As Boolean
            'If ((Not String.IsNullOrEmpty(TextBoxAuthorizationFromDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxAuthorizationFromDate.Text.Trim()))) Then
            '    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorAuthorizationFromDate.ErrorMessage)
            '    Return False
            'End If

            'If (Not String.IsNullOrEmpty(TextBoxAuthorizationFromDate.Text.Trim())) Then
            '    If (DateTime.Compare(DateTime.Now, Convert.ToDateTime(TextBoxAuthorizationFromDate.Text.Trim())) < 0) Then
            '        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Authorization From date should not cross the current date.")
            '        Return False
            '    End If
            'End If

            Return True
        End Function

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ClearControls()
            GridResults = New List(Of ClientServiceInfoDataObject)()
            GridViewService.DataSource = GridResults
            GridViewService.DataBind()
        End Sub

        Public Sub DataFoSave(ByRef objClientCaseCareInfoDataObject As ClientCaseCareInfoDataObject)
            'objClientCaseCareInfoDataObject.PlanOfCareStartDate = Convert.ToString(TextBoxAuthorizationFromDate.Text, Nothing).Trim()
            'objClientCaseCareInfoDataObject.PlanOfCareEndDate = Convert.ToString(TextBoxAuthorizationToDate.Text, Nothing).Trim()
        End Sub

        Private Sub SetControlToolTip()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            Dim DateToolTip As String = Convert.ToString(ResourceTable("DateFieldToolTip"), Nothing)
            'DateToolTip = If(String.IsNullOrEmpty(DateToolTip), "Example: August 21, 2014", DateToolTip)
            DateToolTip = If(String.IsNullOrEmpty(DateToolTip), "Example: 10/27/2015", DateToolTip)

            'TextBoxAuthorizationFromDate.ToolTip = objShared.InlineAssignHelper(TextBoxAuthorizationToDate.ToolTip, DateToolTip)

            ResourceTable = Nothing

        End Sub

        Private Sub SetRegularExpressionSetting()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            Dim ErrorMessage As String = String.Empty, ErrorText As String = String.Empty

            ErrorMessage = Convert.ToString(ResourceTable("InvalidAuthorizationFromDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Authorization From Date.", ErrorMessage)

            'objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorAuthorizationFromDate, "TextBoxAuthorizationFromDate",
            '                                               objShared.DateValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            'ErrorMessage = Convert.ToString(ResourceTable("InvalidAuthorizationToDateMessage"), Nothing)
            'ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Authorization To Date.", ErrorMessage)

            'objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorAuthorizationToDate, "TextBoxAuthorizationToDate",
            '                                               objShared.DateValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ResourceTable = Nothing

            ErrorMessage = Nothing
            ErrorText = Nothing
        End Sub

        ''' <summary>
        ''' Controls Filling Out with data
        ''' </summary>
        ''' <param name="objClientCaseCareInfoDataObject"></param>
        ''' <remarks></remarks>
        Public Sub FillOutData(ByRef objClientCaseCareInfoDataObject As ClientCaseCareInfoDataObject)
            'TextBoxAuthorizationFromDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.PlanOfCareStartDate, Nothing))),
            '                                           String.Empty, objClientCaseCareInfoDataObject.PlanOfCareStartDate)

            'TextBoxAuthorizationToDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.PlanOfCareEndDate, Nothing))),
            'String.Empty, objClientCaseCareInfoDataObject.PlanOfCareEndDate)
        End Sub

        Public Sub SetIndividualId(intIndividualId As Integer)
            Me.IndividualId = intIndividualId
        End Sub
    End Class
End Namespace
