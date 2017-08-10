#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: County Setup 
' Author: Anjan Kumar Paul
' Start Date: 05 Sept 2014
' End Date: 05 Sept 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                05 Sept 2014     Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports VisitelCommon.VisitelCommon
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.Settings
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings

Namespace Visitel.UserControl.Settings

    Public Class CountySetting
        Inherits BaseUserControl

        Private ControlName As String, EditText As String, DeleteText As String, InsertMessage As String, UpdateMessage As String, DeleteMessage As String, _
         DuplicateCountyNameMessage As String, EmptyCountyCodeMessage As String, EmptyCountyNameMessage As String, StatusSelectionMessage As String

        Private objShared As SharedWebControls

        Private UserId As Integer
        Private CompanyId As Integer
        Private ConVisitel As SqlConnection

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "CountySetting"

            objShared = New SharedWebControls()

            UserId = objShared.UserId
            CompanyId = objShared.CompanyId

            objShared.ConnectionOpen()
            ConVisitel = objShared.ConVisitel

            'LoadCss("Settings/" + ControlName)
            'Master.PageHeaderTitle = "County Setting"

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

            TextBoxCountyCode.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelCountyCode"), Label).Text, Nothing)
            TextBoxCountyName.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelCountyName"), Label).Text, Nothing)

            HiddenFieldIdCountyId.Value = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelCountyId"), Label).Text, Nothing)

            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)

        End Sub

        Protected Sub ButtonDelete_Click(sender As Object, e As EventArgs)
            Dim ButtonDelete As Button = DirectCast(sender, Button)
            Dim dtgItem As GridViewRow = DirectCast(ButtonDelete.NamingContainer, GridViewRow)
            Dim LabelCountyId As Label = DirectCast(dtgItem.Cells(0).FindControl("LabelCountyId"), Label)

            Dim objBLCountySetting As New BLCountySetting()
            Try
                objBLCountySetting.DeleteCountyInfo(ConVisitel, Convert.ToInt32(LabelCountyId.Text), UserId)
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DeleteMessage)
            Catch ex As Exception
                If (ex.Message.Contains("REFERENCE constraint")) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to delete County Data. Message: {0}",
                                                                                                 "This County is already used."))
                Else
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to delete County Data. Message: {0}", ex.Message))
                End If
            Finally
                objBLCountySetting = Nothing
            End Try

            BindCountyInformationGridView()
            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)
            HiddenFieldIdCountyId.Value = Convert.ToString(Int32.MinValue)
        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)
            If ValidateData() Then
                Dim objCountySettingDataObject As New CountySettingDataObject()

                objCountySettingDataObject.CountyCode = Convert.ToString(TextBoxCountyCode.Text, Nothing).Trim()
                objCountySettingDataObject.CountyName = Convert.ToString(TextBoxCountyName.Text, Nothing).Trim()

                Dim objBLCountySetting As New BLCountySetting()

                Select Case Convert.ToBoolean(HiddenFieldIsNew.Value, Nothing)
                    Case True
                        objCountySettingDataObject.UserId = UserId
                        objCountySettingDataObject.CompanyId = CompanyId
                        Try
                            objBLCountySetting.InsertCountyInfo(ConVisitel, objCountySettingDataObject)

                        Catch ex As SqlException
                            If ex.Message.Contains("Violation of UNIQUE KEY constraint 'UQ_CountyName'") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateCountyNameMessage)
                                Return
                            End If
                        End Try
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(InsertMessage)
                        Exit Select
                    Case False
                        objCountySettingDataObject.CountyId = Convert.ToInt32(HiddenFieldIdCountyId.Value)
                        objCountySettingDataObject.UpdateBy = Convert.ToString(UserId)
                        Try
                            objBLCountySetting.UpdateCountyInfo(ConVisitel, objCountySettingDataObject)
                        Catch ex As SqlException
                            If ex.Message.Contains("Violation of UNIQUE KEY constraint 'UQ_CountyName'") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateCountyNameMessage)
                                Return
                            End If
                        End Try
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(UpdateMessage)
                        Exit Select
                End Select
                objBLCountySetting = Nothing

                HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
                HiddenFieldIdCountyId.Value = Convert.ToString(Int32.MinValue)
                BindCountyInformationGridView()
            End If
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            ClearControl()
        End Sub

        Private Sub ButtonSearch_Click(sender As Object, e As EventArgs)
            HiddenFieldIsSearched.Value = Convert.ToString(True, Nothing)
            BindCountyInformationGridView()
            HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
        End Sub

        Private Sub GridViewCountyInformation_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewCountyInformation.PageIndex = e.NewPageIndex
            BindCountyInformationGridView()
        End Sub

        Private Sub GridViewCountyInformation_RowDataBound(sender As [Object], e As GridViewRowEventArgs)
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

            TextBoxCountyCode.MaxLength = 4
            TextBoxCountyName.MaxLength = 20

            AddHandler ButtonSave.Click, AddressOf ButtonSave_Click
            AddHandler ButtonClear.Click, AddressOf ButtonClear_Click
            AddHandler ButtonSearch.Click, AddressOf ButtonSearch_Click

            AddHandler GridViewCountyInformation.PageIndexChanging, AddressOf GridViewCountyInformation_PageIndexChanging
            AddHandler GridViewCountyInformation.RowDataBound, AddressOf GridViewCountyInformation_RowDataBound

            GridViewCountyInformation.AutoGenerateColumns = False
            GridViewCountyInformation.ShowHeaderWhenEmpty = True
            GridViewCountyInformation.AllowPaging = True
            GridViewCountyInformation.PageSize = objShared.GridViewDefaultPageSize

            ButtonSave.ClientIDMode = ClientIDMode.Static
            ButtonClear.ClientIDMode = ClientIDMode.Static
            ButtonSearch.ClientIDMode = ClientIDMode.Static

        End Sub


        Private Sub LoadJavascript()
            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                              & " var DeleteDialogHeader ='County'; " _
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

            LabelCountyCode.Text = Convert.ToString(ResourceTable("LabelCountyCode"), Nothing)
            LabelCountyCode.Text = If(String.IsNullOrEmpty(LabelCountyCode.Text), "County Code:", LabelCountyCode.Text)

            LabelCountyName.Text = Convert.ToString(ResourceTable("LabelCountyName"), Nothing)
            LabelCountyName.Text = If(String.IsNullOrEmpty(LabelCountyName.Text), "County Name:", LabelCountyName.Text)

            LabelSearchByCountyName.Text = LabelCountyName.Text

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)
            ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            ButtonClear.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing)
            ButtonClear.Text = If(String.IsNullOrEmpty(ButtonClear.Text), "Clear", ButtonClear.Text)

            ButtonSearch.Text = Convert.ToString(ResourceTable("ButtonSearch"), Nothing)
            ButtonSearch.Text = If(String.IsNullOrEmpty(ButtonSearch.Text), "Search", ButtonSearch.Text)

            LabelCountyInformationEntry.Text = Convert.ToString(ResourceTable("LabelCountyInformationEntry"), Nothing)
            LabelCountyInformationEntry.Text = If(String.IsNullOrEmpty(LabelCountyInformationEntry.Text), "County Information Entry", LabelCountyInformationEntry.Text)

            LabelSearchCountyInfo.Text = Convert.ToString(ResourceTable("LabelSearchCountyInfo"), Nothing)
            LabelSearchCountyInfo.Text = If(String.IsNullOrEmpty(LabelSearchCountyInfo.Text), "Search County Information", LabelSearchCountyInfo.Text)

            LabelCountyInformationList.Text = Convert.ToString(ResourceTable("LabelCountyInformationList"), Nothing)
            LabelCountyInformationList.Text = If(String.IsNullOrEmpty(LabelCountyInformationList.Text), "County Information List", LabelCountyInformationList.Text)

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

            DuplicateCountyNameMessage = Convert.ToString(ResourceTable("DuplicateCountyNameMessage"), Nothing)
            DuplicateCountyNameMessage = If(String.IsNullOrEmpty(DuplicateCountyNameMessage), "The same county already exists.", DuplicateCountyNameMessage)

            EmptyCountyCodeMessage = Convert.ToString(ResourceTable("EmptyCountyCodeMessage"), Nothing)
            EmptyCountyCodeMessage = If(String.IsNullOrEmpty(EmptyCountyCodeMessage), "County Code Cannot be Blank", EmptyCountyCodeMessage)

            EmptyCountyNameMessage = Convert.ToString(ResourceTable("EmptyCountyNameMessage"), Nothing)
            EmptyCountyNameMessage = If(String.IsNullOrEmpty(EmptyCountyNameMessage), "County Name Cannot be Blank", EmptyCountyNameMessage)

            ResourceTable = Nothing
        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData() As Boolean

            If String.IsNullOrEmpty(TextBoxCountyCode.Text.Trim()) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(EmptyCountyCodeMessage)
                Return False
            End If

            If String.IsNullOrEmpty(TextBoxCountyName.Text.Trim()) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(EmptyCountyNameMessage)
                Return False
            End If

            Return True
        End Function

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Private Sub GetData()
            BindCountyInformationGridView()
        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Private Sub ClearControl()
            TextBoxCountyName.Text = String.Empty
            TextBoxCountyCode.Text = String.Empty
            TextBoxSearchByCountyName.Text = String.Empty

            HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
            HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
            HiddenFieldIdCountyId.Value = Convert.ToString(Int32.MinValue)
        End Sub

        ''' <summary>
        ''' Binding County Information GridView
        ''' </summary>
        Private Sub BindCountyInformationGridView()

            Dim CountySettingList As List(Of CountySettingDataObject)

            Dim objBLCountySetting As New BLCountySetting()

            CountySettingList = If((Convert.ToBoolean(HiddenFieldIsSearched.Value, Nothing)),
                                    objBLCountySetting.SearchCountyInfo(ConVisitel, CompanyId, TextBoxSearchByCountyName.Text.Trim()),
                                    objBLCountySetting.SelectCountyInfo(ConVisitel, CompanyId))

            objBLCountySetting = Nothing

            GridViewCountyInformation.DataSource = CountySettingList
            GridViewCountyInformation.DataBind()

            CountySettingList = Nothing

        End Sub

    End Class
End Namespace