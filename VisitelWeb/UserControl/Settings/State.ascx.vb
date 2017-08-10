
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: State Setup 
' Author: Anjan Kumar Paul
' Start Date: 23 Aug 2014
' End Date: 23 Aug 2014
' History:
'      Version                  Author                      Date             Reason 
'      1.0.0                                                23 Aug 2014      Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports VisitelCommon.VisitelCommon
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.Settings
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings

Namespace Visitel.UserControl.Settings

    Public Class StateSetting
        Inherits BaseUserControl
        Private ControlName As String, EditText As String, DeleteText As String, InsertMessage As String, UpdateMessage As String, DeleteMessage As String, _
         DuplicateStateShortNameMessage As String, DuplicateStateFullNameMessage As String, EmptyStateShortNameMessage As String, EmptyStateFullNameMessage As String

        Private objShared As SharedWebControls

        Private UserId As Integer
        Private CompanyId As Integer
        Private ConVisitel As SqlConnection

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "StateSetting"

            objShared = New SharedWebControls()

            UserId = objShared.UserId
            CompanyId = objShared.CompanyId

            objShared.ConnectionOpen()
            ConVisitel = objShared.ConVisitel

            InitializeControl()

            GetCaptionFromResource()

        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If Not IsPostBack Then
                HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
                HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
                GetData()
            End If
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadJavascript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
        End Sub

        Protected Sub LinkButtonEdit_Click(sender As Object, e As EventArgs)
            Dim LinkButtonDelete As LinkButton = DirectCast(sender, LinkButton)
            Dim GridViewItemRow As GridViewRow = DirectCast(LinkButtonDelete.NamingContainer, GridViewRow)

            TextBoxStateShortName.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelStateShortName"), Label).Text, Nothing)
            TextBoxStateFullName.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelStateFullName"), Label).Text, Nothing)


            HiddenFieldStateId.Value = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelStateId"), Label).Text, Nothing)

            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)

        End Sub

        Protected Sub ButtonDelete_Click(sender As Object, e As EventArgs)
            Dim ButtonDelete As Button = DirectCast(sender, Button)
            Dim dtgItem As GridViewRow = DirectCast(ButtonDelete.NamingContainer, GridViewRow)
            Dim LabelStateId As Label = DirectCast(dtgItem.Cells(0).FindControl("LabelStateId"), Label)

            Dim objBLStateSetting As New BLStateSetting()
            Try
                objBLStateSetting.DeleteStateInfo(ConVisitel, Convert.ToInt32(LabelStateId.Text), UserId)
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DeleteMessage)
            Catch ex As Exception
                If (ex.Message.Contains("REFERENCE constraint")) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to delete State Data. Message: {0}",
                                                                                                 "This State is already used."))
                Else
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to delete State Data. Message: {0}", ex.Message))
                End If
            Finally
                objBLStateSetting = Nothing
            End Try

            BindStateInformationGridView()
            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)
            HiddenFieldStateId.Value = Convert.ToString(Int32.MinValue)
        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)
            If ValidateData() Then
                Dim objStateSettingDataObject As New StateSettingDataObject()

                objStateSettingDataObject.StateShortName = Convert.ToString(TextBoxStateShortName.Text, Nothing).Trim()
                objStateSettingDataObject.StateFullName = Convert.ToString(TextBoxStateFullName.Text, Nothing).Trim()


                Dim objBLStateSetting As New BLStateSetting()

                Select Case Convert.ToBoolean(HiddenFieldIsNew.Value, Nothing)
                    Case True
                        objStateSettingDataObject.UserId = UserId
                        objStateSettingDataObject.CompanyId = CompanyId
                        Try
                            objBLStateSetting.InsertStateInfo(ConVisitel, objStateSettingDataObject)

                        Catch ex As SqlException
                            If ex.Message.Contains("Violation of UNIQUE KEY constraint 'UQ_StateShortName'") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateStateShortNameMessage)
                                Return
                            End If
                            If ex.Message.Contains("Violation of UNIQUE KEY constraint 'UQ_StateFullName'") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateStateFullNameMessage)
                                Return
                            End If

                        End Try
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(InsertMessage)

                        Exit Select
                    Case False
                        objStateSettingDataObject.StateId = Convert.ToInt32(HiddenFieldStateId.Value)
                        objStateSettingDataObject.UpdateBy = Convert.ToString(UserId)
                        Try
                            objBLStateSetting.UpdateStateInfo(ConVisitel, objStateSettingDataObject)
                        Catch ex As SqlException
                            If ex.Message.Contains("Violation of UNIQUE KEY constraint 'UQ_StateShortName'") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateStateShortNameMessage)
                                Return
                            End If
                            If ex.Message.Contains("Violation of UNIQUE KEY constraint 'UQ_StateFullName'") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateStateFullNameMessage)
                                Return
                            End If

                        End Try
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(UpdateMessage)
                        Exit Select
                End Select
                objBLStateSetting = Nothing

                HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
                HiddenFieldStateId.Value = Convert.ToString(Int32.MinValue)
                BindStateInformationGridView()
            End If
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            ClearControl()
        End Sub

        Private Sub ButtonSearch_Click(sender As Object, e As EventArgs)
            HiddenFieldIsSearched.Value = Convert.ToString(True, Nothing)
            BindStateInformationGridView()
            HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
        End Sub

        Private Sub GridViewStateInformation_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewStateInformation.PageIndex = e.NewPageIndex
            BindStateInformationGridView()
        End Sub

        Private Sub GridViewStateInformation_RowDataBound(sender As [Object], e As GridViewRowEventArgs)
            If e.Row.RowType.Equals(DataControlRowType.DataRow) Then
                Dim LinkButtonEdit As LinkButton = DirectCast(e.Row.Cells(0).FindControl("LinkButtonEdit"), LinkButton)
                LinkButtonEdit.Text = EditText
                LinkButtonEdit.CssClass = "ui-state-default ui-corner-all buttonLink"

                Dim ButtonDelete As Button = DirectCast(e.Row.Cells(0).FindControl("ButtonDelete"), Button)
                ButtonDelete.Text = DeleteText
            End If
        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()
            TextBoxSearchByStateFullName.MaxLength = objShared.InlineAssignHelper(TextBoxSearchByStateShortName.MaxLength, 20)

            AddHandler ButtonSave.Click, AddressOf ButtonSave_Click
            AddHandler ButtonClear.Click, AddressOf ButtonClear_Click
            AddHandler ButtonSearch.Click, AddressOf ButtonSearch_Click

            AddHandler GridViewStateInformation.PageIndexChanging, AddressOf GridViewStateInformation_PageIndexChanging
            AddHandler GridViewStateInformation.RowDataBound, AddressOf GridViewStateInformation_RowDataBound

            GridViewStateInformation.AutoGenerateColumns = False
            GridViewStateInformation.ShowHeaderWhenEmpty = True
            GridViewStateInformation.AllowPaging = True
            GridViewStateInformation.PageSize = objShared.GridViewDefaultPageSize

            ButtonSave.ClientIDMode = ClientIDMode.Static
            ButtonClear.ClientIDMode = ClientIDMode.Static
            ButtonSearch.ClientIDMode = ClientIDMode.Static
        End Sub

        Private Sub LoadJavascript()
            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                              & " var DeleteDialogHeader ='State: Delete'; " _
                              & " var DeleteDialogConfirmMsg ='Do you want to delete this record?'; " _
                              & " var LoaderImagePath ='" & objShared.GetLoaderImagePath & "'; " _
                              & " var prm =''; " _
                              & " jQuery(document).ready(function () {" _
                              & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                              & "     prm.add_beginRequest(SetButtonActionProgress); " _
                              & "     prm.add_endRequest(EndRequest); " _
                              & "}); " _
                          & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/Settings/" & ControlName & ".js")
        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            LabelStateShortName.Text = Convert.ToString(ResourceTable("LabelStateShortName"), Nothing)
            LabelStateShortName.Text = If(String.IsNullOrEmpty(LabelStateShortName.Text), "Short Name", LabelStateShortName.Text)

            LabelStateFullName.Text = Convert.ToString(ResourceTable("LabelStateFullName"), Nothing)
            LabelStateFullName.Text = If(String.IsNullOrEmpty(LabelStateFullName.Text), "Full Name", LabelStateFullName.Text)

            LabelSearchByStateShortName.Text = LabelStateShortName.Text
            LabelSearchByStateFullName.Text = LabelStateFullName.Text

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)
            ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            ButtonClear.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing)
            ButtonClear.Text = If(String.IsNullOrEmpty(ButtonClear.Text), "Clear", ButtonClear.Text)

            ButtonSearch.Text = Convert.ToString(ResourceTable("ButtonSearch"), Nothing)
            ButtonSearch.Text = If(String.IsNullOrEmpty(ButtonSearch.Text), "Search", ButtonSearch.Text)

            LabelStateInformationEntry.Text = Convert.ToString(ResourceTable("LabelStateInformationEntry"), Nothing)
            LabelStateInformationEntry.Text = If(String.IsNullOrEmpty(LabelStateInformationEntry.Text), "State Information Entry", LabelStateInformationEntry.Text)

            LabelSearchStateInfo.Text = Convert.ToString(ResourceTable("LabelSearchStateInfo"), Nothing)
            LabelSearchStateInfo.Text = If(String.IsNullOrEmpty(LabelSearchStateInfo.Text), "Search State Information", LabelSearchStateInfo.Text)

            LabelStateInformationList.Text = Convert.ToString(ResourceTable("LabelStateInformationList"), Nothing)
            LabelStateInformationList.Text = If(String.IsNullOrEmpty(LabelStateInformationList.Text), "State Information List", LabelStateInformationList.Text)

            EditText = Convert.ToString(ResourceTable("EditText"), Nothing)
            EditText = If(String.IsNullOrEmpty(EditText), "Edit", EditText)

            DeleteText = Convert.ToString(ResourceTable("DeleteText"), Nothing)
            DeleteText = If(String.IsNullOrEmpty(DeleteText), "Delete", DeleteText)

            InsertMessage = Convert.ToString(ResourceTable("InsertMessage"), Nothing)
            InsertMessage = If(String.IsNullOrEmpty(InsertMessage), "Inserted Successfully", InsertMessage)

            UpdateMessage = Convert.ToString(ResourceTable("UpdateMessage"), Nothing)
            UpdateMessage = If(String.IsNullOrEmpty(UpdateMessage), "Updated Successfully", UpdateMessage)

            DeleteMessage = Convert.ToString(ResourceTable("DeleteMessage"), Nothing)
            DeleteMessage = If(String.IsNullOrEmpty(DeleteMessage), "Deleted Successfully", DeleteMessage)

            DuplicateStateFullNameMessage = Convert.ToString(ResourceTable("DuplicateStateFullNameMessage"), Nothing)
            DuplicateStateFullNameMessage = If(String.IsNullOrEmpty(DuplicateStateFullNameMessage), "The same full name already exists.", DuplicateStateFullNameMessage)

            DuplicateStateShortNameMessage = Convert.ToString(ResourceTable("DuplicateStateShortNameMessage"), Nothing)
            DuplicateStateShortNameMessage = If(String.IsNullOrEmpty(DuplicateStateShortNameMessage), "The same short name already exists.", DuplicateStateShortNameMessage)

            EmptyStateShortNameMessage = Convert.ToString(ResourceTable("EmptyStateShortNameMessage"), Nothing)
            EmptyStateShortNameMessage = If(String.IsNullOrEmpty(EmptyStateShortNameMessage), "State Short Name Cannot be Blank", EmptyStateShortNameMessage)

            EmptyStateFullNameMessage = Convert.ToString(ResourceTable("EmptyStateFullNameMessage"), Nothing)
            EmptyStateFullNameMessage = If(String.IsNullOrEmpty(EmptyStateFullNameMessage), "State Full Name Cannot be Blank", EmptyStateFullNameMessage)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData() As Boolean
            If String.IsNullOrEmpty(TextBoxStateShortName.Text.Trim()) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(EmptyStateShortNameMessage)
                Return False
            End If
            If String.IsNullOrEmpty(TextBoxStateFullName.Text.Trim()) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(EmptyStateFullNameMessage)
                Return False
            End If

            Return True
        End Function

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Private Sub GetData()
            BindStateInformationGridView()
        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Private Sub ClearControl()
            TextBoxStateFullName.Text = objShared.InlineAssignHelper(TextBoxStateShortName.Text, String.Empty)

            TextBoxSearchByStateShortName.Text = String.Empty
            TextBoxSearchByStateFullName.Text = String.Empty

            HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
            HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
            HiddenFieldStateId.Value = Convert.ToString(Int32.MinValue)
        End Sub

        ''' <summary>
        ''' Binding State Information GridView
        ''' </summary>
        Private Sub BindStateInformationGridView()

            Dim StateSettingList As List(Of StateSettingDataObject)

            Dim objBLStateSetting As New BLStateSetting()

            StateSettingList = If((Convert.ToBoolean(HiddenFieldIsSearched.Value, Nothing)),
                                objBLStateSetting.SearchStateInfo(ConVisitel, CompanyId, TextBoxSearchByStateShortName.Text.Trim(), TextBoxSearchByStateFullName.Text.Trim()),
                                objBLStateSetting.SelectStateInfo(ConVisitel, CompanyId))

            objBLStateSetting = Nothing

            GridViewStateInformation.DataSource = StateSettingList
            GridViewStateInformation.DataBind()

            StateSettingList = Nothing
        End Sub

    End Class
End Namespace