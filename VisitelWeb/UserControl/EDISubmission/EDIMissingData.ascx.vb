
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: EDI Missing Data
' Author: Anjan Kumar Paul
' Start Date: 01 Aug 2015
' End Date: 
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                01 Aug 2015     Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Data.SqlClient
Imports System.Linq.Expressions


Namespace Visitel.UserControl.EDISubmission

    Public Class EDIMissingDataControl
        Inherits BaseUserControl

        Private ControlName As String, ValidationGroup As String, SaveMessage As String, DeleteMessage As String, DeleteConfirmationMessage As String, ListFor As String
        Private ValidationEnable As Boolean

        Private CheckBoxSelect As CheckBox, chkAll As CheckBox

        Private TextBoxName As TextBox, TextBoxClientId As TextBox, TextBoxStateClientId As TextBox, TextBoxSocialSecurityNumber As TextBox, TextBoxMissingData As TextBox

        Private CurrentRow As GridViewRow
        Private objShared As SharedWebControls

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            DirectCast(Me.Page.Master, IMyMasterPage).PageHeaderTitle = "EDI Missing Data"
            ControlName = "EDIMissingDataControl"
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
            LoadCss("EDISubmission/" + ControlName)

            ButtonIndividual.Enabled = If((DetermineButtonInActivity(GridViewEDIMissingData, "CheckBoxSelect")), True, False)
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        Private Sub GridViewEDIMissingData_RowDataBound(sender As Object, e As GridViewRowEventArgs)

            CurrentRow = DirectCast(e.Row, GridViewRow)

            If (e.Row.RowType.Equals(DataControlRowType.Header)) Then
                SetGridViewColumnHeaderText(CurrentRow)

                chkAll = DirectCast(CurrentRow.FindControl("chkAll"), CheckBox)
                chkAll.AutoPostBack = True
                chkAll.ClientIDMode = UI.ClientIDMode.Static

            End If

            If (CurrentRow.RowType.Equals(DataControlRowType.DataRow)) Then

                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.AutoPostBack = True

                TextBoxName = DirectCast(CurrentRow.FindControl("TextBoxName"), TextBox)
                TextBoxClientId = DirectCast(CurrentRow.FindControl("TextBoxClientId"), TextBox)
                TextBoxStateClientId = DirectCast(CurrentRow.FindControl("TextBoxStateClientId"), TextBox)
                TextBoxSocialSecurityNumber = DirectCast(CurrentRow.FindControl("TextBoxSocialSecurityNumber"), TextBox)
                TextBoxMissingData = DirectCast(CurrentRow.FindControl("TextBoxMissingData"), TextBox)

                TextBoxMissingData.TextMode = TextBoxMode.MultiLine

                TextBoxName.ReadOnly = InlineAssignHelper(TextBoxClientId.ReadOnly, InlineAssignHelper(TextBoxStateClientId.ReadOnly,
                                       InlineAssignHelper(TextBoxSocialSecurityNumber.ReadOnly, InlineAssignHelper(TextBoxMissingData.ReadOnly, True))))

            End If

        End Sub

        Private Sub GridViewEDIMissingData_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewEDIMissingData.PageIndex = e.NewPageIndex
            BindEDIMissingDataGridView()
        End Sub

        Private Sub ButtonIndividualDetail_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Pages/ClientInfo.aspx?ClientId=" & HiddenFieldClientId.Value)
        End Sub

        Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)

            Dim chk As CheckBox = DirectCast(sender, CheckBox)

            If chk.ID.Equals("chkAll") Then
                GridViewAllSelectCheckboxSelectChange(chk, CurrentRow, GridViewEDIMissingData, CheckBoxSelect, "CheckBoxSelect")
            End If

            GridViewControlsOnEdit(GridViewEDIMissingData, "chkAll", "CheckBoxSelect")

            For Each row As GridViewRow In GridViewEDIMissingData.Rows
                If row.RowType.Equals(DataControlRowType.DataRow) Then

                    Dim isChecked As Boolean = DirectCast(row.FindControl("CheckBoxSelect"), CheckBox).Checked

                    If (isChecked) Then
                        TextBoxClientId = DirectCast(row.FindControl("TextBoxClientId"), TextBox)
                        If ((Not HiddenFieldClientId Is Nothing) And (Not TextBoxClientId Is Nothing)) Then
                            HiddenFieldClientId.Value = TextBoxClientId.Text.Trim()
                        End If

                    End If
                End If
            Next
        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()

            GridViewEDIMissingData.AutoGenerateColumns = False
            GridViewEDIMissingData.Width = Unit.Percentage(100)
            GridViewEDIMissingData.ShowHeaderWhenEmpty = True
            GridViewEDIMissingData.AllowPaging = True

            If (GridViewEDIMissingData.AllowPaging) Then
                GridViewEDIMissingData.PageSize = objShared.GridViewDefaultPageSize
            End If

            AddHandler GridViewEDIMissingData.RowDataBound, AddressOf GridViewEDIMissingData_RowDataBound
            AddHandler GridViewEDIMissingData.PageIndexChanging, AddressOf GridViewEDIMissingData_PageIndexChanging

            AddHandler ButtonIndividual.Click, AddressOf ButtonIndividualDetail_Click

        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EDISubmission", ControlName & Convert.ToString(".resx"))

            LabelEDIMissingDataNote.Text = Convert.ToString(ResourceTable("LabelEDIMissingDataNote"), Nothing)
            LabelEDIMissingDataNote.Text = If(String.IsNullOrEmpty(LabelEDIMissingDataNote.Text),
                                              "***The listed missing data are critical values that are required for EDI processing. Please populate and re-run***",
                                              LabelEDIMissingDataNote.Text)

            ButtonIndividual.Text = Convert.ToString(ResourceTable("ButtonIndividual"), Nothing)
            ButtonIndividual.Text = If(String.IsNullOrEmpty(ButtonIndividual.Text), "Individual", ButtonIndividual.Text)

            ButtonRefresh.Text = Convert.ToString(ResourceTable("ButtonRefresh"), Nothing)
            ButtonRefresh.Text = If(String.IsNullOrEmpty(ButtonRefresh.Text), "Refresh", ButtonRefresh.Text)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Private Sub GetData()
            BindEDIMissingDataGridView()
        End Sub

        ''' <summary>
        ''' EDI Missing Data Grid Data Bind
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindEDIMissingDataGridView()

            Dim objBLEDISubmission As New BLEDISubmission()
            Dim EDIMissingDataList As New List(Of EDIMissingDataObject)

            Try
                EDIMissingDataList = objBLEDISubmission.SelectEDIMissingData(objShared.ConVisitel)
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to fetch EDI Missing Data")
            Finally
                objBLEDISubmission = Nothing
            End Try

            GridViewEDIMissingData.DataSource = EDIMissingDataList
            GridViewEDIMissingData.DataBind()

            EDIMissingDataList = Nothing

        End Sub

        ''' <summary>
        ''' Reading GridView header text from resource file
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <remarks></remarks>
        Private Sub SetGridViewColumnHeaderText(ByRef CurrentRow As GridViewRow)

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EDICodes", ControlName & Convert.ToString(".resx"))

            Dim LabelHeaderSerial As Label = DirectCast(CurrentRow.FindControl("LabelHeaderSerial"), Label)
            LabelHeaderSerial.Text = Convert.ToString(ResourceTable("LabelHeaderSerial"), Nothing).Trim()
            LabelHeaderSerial.Text = If(String.IsNullOrEmpty(LabelHeaderSerial.Text), "SI.", LabelHeaderSerial.Text)

            Dim LabelHeaderSelect As Label = DirectCast(CurrentRow.FindControl("LabelHeaderSelect"), Label)
            LabelHeaderSelect.Text = Convert.ToString(ResourceTable("LabelHeaderSelect"), Nothing).Trim()
            LabelHeaderSelect.Text = If(String.IsNullOrEmpty(LabelHeaderSelect.Text), "Select", LabelHeaderSelect.Text)

            Dim LabelHeaderName As Label = DirectCast(CurrentRow.FindControl("LabelHeaderName"), Label)
            LabelHeaderName.Text = Convert.ToString(ResourceTable("LabelHeaderName"), Nothing).Trim()
            LabelHeaderName.Text = If(String.IsNullOrEmpty(LabelHeaderName.Text), "Name", LabelHeaderName.Text)

            Dim LabelHeaderClientId As Label = DirectCast(CurrentRow.FindControl("LabelHeaderClientId"), Label)
            LabelHeaderClientId.Text = Convert.ToString(ResourceTable("LabelHeaderClientId"), Nothing).Trim()
            LabelHeaderClientId.Text = If(String.IsNullOrEmpty(LabelHeaderClientId.Text), "Client Id", LabelHeaderClientId.Text)

            Dim LabelHeaderStateClientId As Label = DirectCast(CurrentRow.FindControl("LabelHeaderStateClientId"), Label)
            LabelHeaderStateClientId.Text = Convert.ToString(ResourceTable("LabelHeaderStateClientId"), Nothing).Trim()
            LabelHeaderStateClientId.Text = If(String.IsNullOrEmpty(LabelHeaderStateClientId.Text), "State Client Id", LabelHeaderStateClientId.Text)

            Dim LabelHeaderSocialSecurityNumber As Label = DirectCast(CurrentRow.FindControl("LabelHeaderSocialSecurityNumber"), Label)
            LabelHeaderSocialSecurityNumber.Text = Convert.ToString(ResourceTable("LabelHeaderSocialSecurityNumber"), Nothing).Trim()
            LabelHeaderSocialSecurityNumber.Text = If(String.IsNullOrEmpty(LabelHeaderSocialSecurityNumber.Text), "SS#", LabelHeaderSocialSecurityNumber.Text)

            Dim LabelHeaderMissingData As Label = DirectCast(CurrentRow.FindControl("LabelHeaderMissingData"), Label)
            LabelHeaderMissingData.Text = Convert.ToString(ResourceTable("LabelHeaderMissingData"), Nothing).Trim()
            LabelHeaderMissingData.Text = If(String.IsNullOrEmpty(LabelHeaderMissingData.Text), "Missing Data", LabelHeaderMissingData.Text)


            ResourceTable = Nothing

        End Sub

    End Class

End Namespace