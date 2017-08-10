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
    Public Class ClientGroupSetting
        Inherits BaseUserControl

        Private ControlName As String, EditText As String, DeleteText As String, InsertMessage As String, UpdateMessage As String, DeleteMessage As String, _
            DuplicateClientGroupNameMessage As String, EmptyClientGroupNameMessage As String

        Private objShared As SharedWebControls

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            DirectCast(Me.Page.Master, IMyMasterPage).PageHeaderTitle = "Client Group Setting"
            ControlName = "ClientGroupSetting"
            objShared = New SharedWebControls()

            objShared.ConnectionOpen()
            LoadCss("Settings/" + ControlName)

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
            objShared = Nothing
        End Sub

        Protected Sub LinkButtonEdit_Click(sender As Object, e As EventArgs)
            Dim LinkButtonDelete As LinkButton = DirectCast(sender, LinkButton)
            Dim GridViewItemRow As GridViewRow = DirectCast(LinkButtonDelete.NamingContainer, GridViewRow)

            TextBoxClientGroupName.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelClientGroupName"), Label).Text, Nothing)

            HiddenFieldClientGroupId.Value = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelClientGroupId"), Label).Text, Nothing)

            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)
        End Sub

        Protected Sub ButtonDelete_Click(sender As Object, e As EventArgs)
            Dim ButtonDelete As Button = DirectCast(sender, Button)
            Dim dtgItem As GridViewRow = DirectCast(ButtonDelete.NamingContainer, GridViewRow)
            Dim LabelClientGroupId As Label = DirectCast(dtgItem.Cells(0).FindControl("LabelClientGroupId"), Label)

            Dim objBLClientGroupSetting As New BLClientGroupSetting()
            Try
                objBLClientGroupSetting.DeleteClientGroupInfo(objShared.ConVisitel, Convert.ToInt32(LabelClientGroupId.Text), objShared.UserId)
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DeleteMessage)
            Catch ex As Exception
                If (ex.Message.Contains("REFERENCE constraint")) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to delete Client Group Data. Message: {0}",
                                                                                                 "This Client Group is already used."))
                Else
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to delete Client Group Data. Message: {0}", ex.Message))
                End If
            Finally
                objBLClientGroupSetting = Nothing
            End Try

            BindClientGroupInformationGridView()
            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)
            HiddenFieldClientGroupId.Value = Convert.ToString(Int32.MinValue)
        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)
            If ValidateData() Then
                Dim objClientGroupSettingDataObject As New ClientGroupSettingDataObject()

                objClientGroupSettingDataObject.GroupName = Convert.ToString(TextBoxClientGroupName.Text, Nothing).Trim()


                Dim objBLClientGroupSetting As New BLClientGroupSetting()

                Select Case Convert.ToBoolean(HiddenFieldIsNew.Value, Nothing)
                    Case True
                        objClientGroupSettingDataObject.UserId = objShared.UserId
                        objClientGroupSettingDataObject.CompanyId = objShared.CompanyId
                        Try
                            objBLClientGroupSetting.InsertClientGroupInfo(objShared.ConVisitel, objClientGroupSettingDataObject)

                        Catch ex As SqlException
                            If ex.Message.Contains("Violation of UNIQUE KEY constraint 'UQ_ClientGroupName'") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateClientGroupNameMessage)
                                Return
                            End If
                        End Try

                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(InsertMessage)
                        Exit Select
                    Case False
                        objClientGroupSettingDataObject.GroupId = Convert.ToInt32(HiddenFieldClientGroupId.Value)
                        objClientGroupSettingDataObject.UpdateBy = Convert.ToString(objShared.UserId)
                        Try
                            objBLClientGroupSetting.UpdateClientGroupInfo(objShared.ConVisitel, objClientGroupSettingDataObject)
                        Catch ex As SqlException
                            If ex.Message.Contains("Violation of UNIQUE KEY constraint 'UQ_ClientGroupName'") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateClientGroupNameMessage)
                                Return
                            End If
                        End Try

                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(UpdateMessage)
                        Exit Select
                End Select
                objBLClientGroupSetting = Nothing

                HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
                HiddenFieldClientGroupId.Value = Convert.ToString(Int32.MinValue)
                BindClientGroupInformationGridView()
            End If
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            ClearControl()
        End Sub

        Private Sub ButtonSearch_Click(sender As Object, e As EventArgs)
            HiddenFieldIsSearched.Value = Convert.ToString(True, Nothing)
            BindClientGroupInformationGridView()
            HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
        End Sub

        Private Sub GridViewClientGroupInformation_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewClientGroupInformation.PageIndex = e.NewPageIndex
            BindClientGroupInformationGridView()
        End Sub

        Private Sub GridViewClientGroupInformation_RowDataBound(sender As [Object], e As GridViewRowEventArgs)
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
            TextBoxClientGroupName.MaxLength = 20

            AddHandler ButtonSave.Click, AddressOf ButtonSave_Click
            AddHandler ButtonClear.Click, AddressOf ButtonClear_Click
            AddHandler ButtonSearch.Click, AddressOf ButtonSearch_Click

            AddHandler GridViewClientGroupInformation.PageIndexChanging, AddressOf GridViewClientGroupInformation_PageIndexChanging
            AddHandler GridViewClientGroupInformation.RowDataBound, AddressOf GridViewClientGroupInformation_RowDataBound

            GridViewClientGroupInformation.AutoGenerateColumns = False
            GridViewClientGroupInformation.ShowHeaderWhenEmpty = True
            GridViewClientGroupInformation.AllowPaging = True
            GridViewClientGroupInformation.PageSize = objShared.GridViewDefaultPageSize

            ButtonSave.ClientIDMode = ClientIDMode.Static
            ButtonClear.ClientIDMode = ClientIDMode.Static
            ButtonSearch.ClientIDMode = ClientIDMode.Static
        End Sub

        Private Sub LoadJavascript()
            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                              & " var DeleteDialogHeader ='Client Group'; " _
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

            LabelClientGroupName.Text = Convert.ToString(ResourceTable("LabelClientGroupName"), Nothing)
            LabelClientGroupName.Text = If(String.IsNullOrEmpty(LabelClientGroupName.Text), "Client Group Name", LabelClientGroupName.Text)

            LabelSearchByClientGroupName.Text = LabelClientGroupName.Text

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)
            ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            ButtonClear.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing)
            ButtonClear.Text = If(String.IsNullOrEmpty(ButtonClear.Text), "Clear", ButtonClear.Text)

            ButtonSearch.Text = Convert.ToString(ResourceTable("ButtonSearch"), Nothing)
            ButtonSearch.Text = If(String.IsNullOrEmpty(ButtonSearch.Text), "Search", ButtonSearch.Text)

            LabelClientGroupInformationEntry.Text = Convert.ToString(ResourceTable("LabelClientGroupInformationEntry"), Nothing)
            LabelClientGroupInformationEntry.Text = If(String.IsNullOrEmpty(LabelClientGroupInformationEntry.Text),
                                                       "Client Group Information Entry", LabelClientGroupInformationEntry.Text)


            LabelSearchClientGroupInfo.Text = Convert.ToString(ResourceTable("LabelSearchClientGroupInfo"), Nothing)
            LabelSearchClientGroupInfo.Text = If(String.IsNullOrEmpty(LabelSearchClientGroupInfo.Text),
                                                       "Search Client Group Information", LabelSearchClientGroupInfo.Text)

            LabelClientGroupInformationList.Text = Convert.ToString(ResourceTable("LabelClientGroupInformationList"), Nothing)
            LabelClientGroupInformationList.Text = If(String.IsNullOrEmpty(LabelClientGroupInformationList.Text),
                                                       "Client Group Information List", LabelClientGroupInformationList.Text)

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

            DuplicateClientGroupNameMessage = Convert.ToString(ResourceTable("DuplicateClientGroupNameMessage"), Nothing)
            DuplicateClientGroupNameMessage = If(String.IsNullOrEmpty(DuplicateClientGroupNameMessage), "The same client group already exists.", DuplicateClientGroupNameMessage)

            EmptyClientGroupNameMessage = Convert.ToString(ResourceTable("EmptyClientGroupNameMessage"), Nothing)
            EmptyClientGroupNameMessage = If(String.IsNullOrEmpty(EmptyClientGroupNameMessage), "Client Group Name Cannot be Blank", EmptyClientGroupNameMessage)

            ResourceTable = Nothing
        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData() As Boolean

            If String.IsNullOrEmpty(TextBoxClientGroupName.Text.Trim()) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(EmptyClientGroupNameMessage)
                Return False
            End If
            Return True
        End Function

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Private Sub GetData()
            BindClientGroupInformationGridView()
        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Private Sub ClearControl()
            TextBoxClientGroupName.Text = String.Empty
            TextBoxSearchByClientGroupName.Text = String.Empty

            HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
            HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
            HiddenFieldClientGroupId.Value = Convert.ToString(Int32.MinValue)
        End Sub

        ''' <summary>
        ''' Binding ClientGroup Information GridView
        ''' </summary>
        Private Sub BindClientGroupInformationGridView()

            Dim ClientGroupSettingList As New List(Of ClientGroupSettingDataObject)

            Dim objBLClientGroupSetting As New BLClientGroupSetting()

            ClientGroupSettingList = If(Convert.ToBoolean(HiddenFieldIsSearched.Value, Nothing),
                                         objBLClientGroupSetting.SearchClientGroupInfo(objShared.ConVisitel, objShared.CompanyId, TextBoxSearchByClientGroupName.Text.Trim()),
                                         objBLClientGroupSetting.SelectClientGroupInfo(objShared.ConVisitel, objShared.CompanyId))


            objBLClientGroupSetting = Nothing

            GridViewClientGroupInformation.DataSource = ClientGroupSettingList
            GridViewClientGroupInformation.DataBind()

            ClientGroupSettingList = Nothing
        End Sub
    End Class
End Namespace