#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: City Setup
' Author: Anjan Kumar Paul
' Start Date: 23 Aug 2014
' End Date: 23 Aug 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                23 Aug 2014     Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness.Settings
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings

Namespace Visitel.UserControl.Settings
    Public Class CitySetting
        Inherits BaseUserControl

        Private ControlName As String, EditText As String, DeleteText As String, InsertMessage As String, UpdateMessage As String, DeleteMessage As String, _
      DuplicateCityNameMessage As String, EmptyCityNameMessage As String
        Private objShared As SharedWebControls

        Private UserId As Integer
        Private CompanyId As Integer
        Private ConVisitel As SqlConnection

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "CitySetting"
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

            TextBoxCityName.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelCityName"), Label).Text, Nothing)

            HiddenFieldCityId.Value = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelCityId"), Label).Text, Nothing)

            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)
        End Sub

        Protected Sub ButtonDelete_Click(sender As Object, e As EventArgs)
            Dim ButtonDelete As Button = DirectCast(sender, Button)
            Dim dtgItem As GridViewRow = DirectCast(ButtonDelete.NamingContainer, GridViewRow)
            Dim LabelCityId As Label = DirectCast(dtgItem.Cells(0).FindControl("LabelCityId"), Label)

            Dim objBLCitySetting As New BLCitySetting()
            Try
                objBLCitySetting.DeleteCityInfo(objShared.ConVisitel, Convert.ToInt32(LabelCityId.Text), UserId)
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DeleteMessage)
            Catch ex As Exception
                If (ex.Message.Contains("REFERENCE constraint")) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to delete City Data. Message: {0}",
                                                                                                 "This City is already used."))
                Else
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to delete City Data. Message: {0}", ex.Message))
                End If
            Finally
                objBLCitySetting = Nothing
            End Try

            BindCityInformationGridView()
            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)
            HiddenFieldCityId.Value = Convert.ToString(Int32.MinValue)
        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)
            If ValidateData() Then
                Dim objCitySettingDataObject As New CitySettingDataObject()

                objCitySettingDataObject.CityName = Convert.ToString(TextBoxCityName.Text, Nothing).Trim()


                Dim objBLCitySetting As New BLCitySetting()

                Select Case Convert.ToBoolean(HiddenFieldIsNew.Value, Nothing)
                    Case True
                        objCitySettingDataObject.UserId = UserId
                        objCitySettingDataObject.CompanyId = CompanyId
                        Try
                            objBLCitySetting.InsertCityInfo(ConVisitel, objCitySettingDataObject)

                        Catch ex As SqlException
                            If ex.Message.Contains("Violation of UNIQUE KEY constraint 'UQ_CityName'") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateCityNameMessage)
                                Return
                            End If
                        End Try
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(InsertMessage)
                        Exit Select
                    Case False
                        objCitySettingDataObject.CityId = Convert.ToInt32(HiddenFieldCityId.Value)
                        objCitySettingDataObject.UpdateBy = Convert.ToString(UserId)
                        Try
                            objBLCitySetting.UpdateCityInfo(ConVisitel, objCitySettingDataObject)
                        Catch ex As SqlException
                            If ex.Message.Contains("Violation of UNIQUE KEY constraint 'UQ_CityName'") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateCityNameMessage)
                                Return
                            End If
                        End Try
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(UpdateMessage)
                        Exit Select
                End Select
                objBLCitySetting = Nothing

                HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
                HiddenFieldCityId.Value = Convert.ToString(Int32.MinValue)
                BindCityInformationGridView()
            End If
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            ClearControl()
        End Sub

        Private Sub ButtonSearch_Click(sender As Object, e As EventArgs)
            HiddenFieldIsSearched.Value = Convert.ToString(True, Nothing)
            BindCityInformationGridView()
            HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
        End Sub

        Private Sub GridViewCityInformation_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewCityInformation.PageIndex = e.NewPageIndex
            BindCityInformationGridView()
        End Sub

        Private Sub GridViewCityInformation_RowDataBound(sender As [Object], e As GridViewRowEventArgs)
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
            TextBoxSearchByCityName.MaxLength = 20

            AddHandler ButtonSave.Click, AddressOf ButtonSave_Click
            AddHandler ButtonClear.Click, AddressOf ButtonClear_Click
            AddHandler ButtonSearch.Click, AddressOf ButtonSearch_Click

            AddHandler GridViewCityInformation.PageIndexChanging, AddressOf GridViewCityInformation_PageIndexChanging
            AddHandler GridViewCityInformation.RowDataBound, AddressOf GridViewCityInformation_RowDataBound

            GridViewCityInformation.AutoGenerateColumns = False
            GridViewCityInformation.ShowHeaderWhenEmpty = True
            GridViewCityInformation.AllowPaging = True
            GridViewCityInformation.PageSize = objShared.GridViewDefaultPageSize

            ButtonSave.ClientIDMode = ClientIDMode.Static
            ButtonClear.ClientIDMode = ClientIDMode.Static
            ButtonSearch.ClientIDMode = ClientIDMode.Static
        End Sub

        Private Sub LoadJavascript()
            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                              & " var DeleteDialogHeader ='City'; " _
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

            LabelCityName.Text = Convert.ToString(ResourceTable("LabelCityName"), Nothing)
            LabelCityName.Text = If(String.IsNullOrEmpty(LabelCityName.Text), "City", LabelCityName.Text)

            LabelSearchByCityName.Text = LabelCityName.Text

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)
            ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            ButtonClear.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing)
            ButtonClear.Text = If(String.IsNullOrEmpty(ButtonClear.Text), "Clear", ButtonClear.Text)

            ButtonSearch.Text = Convert.ToString(ResourceTable("ButtonSearch"), Nothing)
            ButtonSearch.Text = If(String.IsNullOrEmpty(ButtonSearch.Text), "Search", ButtonSearch.Text)

            LabelCityInformationEntry.Text = Convert.ToString(ResourceTable("LabelCityInformationEntry"), Nothing)
            LabelCityInformationEntry.Text = If(String.IsNullOrEmpty(LabelCityInformationEntry.Text), "City Information Entry", LabelCityInformationEntry.Text)

            LabelSearchCityInfo.Text = Convert.ToString(ResourceTable("LabelSearchCityInfo"), Nothing)
            LabelSearchCityInfo.Text = If(String.IsNullOrEmpty(LabelSearchCityInfo.Text), "Search City Information", LabelSearchCityInfo.Text)

            LabelCityInformationList.Text = Convert.ToString(ResourceTable("LabelCityInformationList"), Nothing)
            LabelCityInformationList.Text = If(String.IsNullOrEmpty(LabelCityInformationList.Text), "City Information List", LabelCityInformationList.Text)

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

            DuplicateCityNameMessage = Convert.ToString(ResourceTable("DuplicateCityNameMessage"), Nothing)
            DuplicateCityNameMessage = If(String.IsNullOrEmpty(DuplicateCityNameMessage), "The same city already exists.", DuplicateCityNameMessage)

            EmptyCityNameMessage = Convert.ToString(ResourceTable("EmptyCityNameMessage"), Nothing)
            EmptyCityNameMessage = If(String.IsNullOrEmpty(EmptyCityNameMessage), "City Name Cannot be Blank", EmptyCityNameMessage)

            ResourceTable = Nothing
        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData() As Boolean

            If String.IsNullOrEmpty(TextBoxCityName.Text.Trim()) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(EmptyCityNameMessage)
                Return False
            End If
            Return True
        End Function

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Private Sub GetData()
            BindCityInformationGridView()
        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Private Sub ClearControl()
            TextBoxCityName.Text = String.Empty
            TextBoxSearchByCityName.Text = String.Empty

            HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
            HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
            HiddenFieldCityId.Value = Convert.ToString(Int32.MinValue)
        End Sub

        ''' <summary>
        ''' Binding City Information GridView
        ''' </summary>
        Private Sub BindCityInformationGridView()

            Dim CitySettingList As List(Of CitySettingDataObject)

            Dim objBLCitySetting As New BLCitySetting()

            CitySettingList = If(Convert.ToBoolean(HiddenFieldIsSearched.Value, Nothing),
                                 objBLCitySetting.SearchCityInfo(ConVisitel, CompanyId, TextBoxSearchByCityName.Text.Trim()),
                                 objBLCitySetting.SelectCityInfo(ConVisitel, CompanyId))


            objBLCitySetting = Nothing

            GridViewCityInformation.DataSource = CitySettingList
            GridViewCityInformation.DataBind()

            CitySettingList = Nothing
        End Sub
    End Class
End Namespace