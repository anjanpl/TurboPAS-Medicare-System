#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Diagnosis Setup 
' Author: Anjan Kumar Paul
' Start Date: 06 Sept 2014
' End Date: 06 Sept 2014
' History:
'      Version                  Author                      Date             Reason 
'      1.0.0                                                06 Sept 2014     Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports VisitelCommon.VisitelCommon
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.Settings
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings

Namespace Visitel.UserControl.Settings

    Public Class DiagnosisSetting
        Inherits System.Web.UI.UserControl

        Private objShared As SharedWebControls

        Private UserId As Integer
        Private CompanyId As Integer
        Private ConVisitel As SqlConnection

        Private ControlName As String, EditText As String, DeleteText As String, InsertMessage As String, UpdateMessage As String, DeleteMessage As String, _
           DuplicateCityNameMessage As String, EmptyCityNameMessage As String


        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "DiagnosisSetting"
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

            'TextBoxDiagnosisOne.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelDiagnosisOne"), Label).Text, Nothing)
            'TextBoxDiagnosisOneCode.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelDiagnosisOneCode"), Label).Text, Nothing)

            'TextBoxDiagnosisTwo.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelDiagnosisTwo"), Label).Text, Nothing)
            'TextBoxDiagnosisTwoCode.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelDiagnosisTwoCode"), Label).Text, Nothing)

            'TextBoxDiagnosisThree.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelDiagnosisThree"), Label).Text, Nothing)
            'TextBoxDiagnosisThreeCode.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelDiagnosisThreeCode"), Label).Text, Nothing)

            'TextBoxDiagnosisFour.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelDiagnosisThree"), Label).Text, Nothing)
            'TextBoxDiagnosisFourCode.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelDiagnosisFourCode"), Label).Text, Nothing)

            HiddenFieldDiagnosisId.Value = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelDiagnosisId"), Label).Text, Nothing)

            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)

        End Sub

        Protected Sub ButtonDelete_Click(sender As Object, e As EventArgs)
            Dim ButtonDelete As Button = DirectCast(sender, Button)
            Dim dtgItem As GridViewRow = DirectCast(ButtonDelete.NamingContainer, GridViewRow)
            Dim LabelDiagnosisId As Label = DirectCast(dtgItem.Cells(0).FindControl("LabelDiagnosisId"), Label)

            Dim objBLDiagnosisSetting As New BLDiagnosisSetting()
            Try
                objBLDiagnosisSetting.DeleteDiagnosisInfo(ConVisitel, Convert.ToInt32(LabelDiagnosisId.Text), UserId)
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DeleteMessage)
            Catch ex As Exception

            Finally
                objBLDiagnosisSetting = Nothing
            End Try

            BindDiagnosisInformationGridView()
            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)
            HiddenFieldDiagnosisId.Value = Convert.ToString(Int32.MinValue)
        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)
            If ValidateData() Then
                Dim objDiagnosisSettingDataObject As New DiagnosisSettingDataObject()

                'objDiagnosisSettingDataObject.DiagnosisOne = Convert.ToString(TextBoxDiagnosisOne.Text, Nothing).Trim()
                'objDiagnosisSettingDataObject.DiagnosisOneCode = Convert.ToString(TextBoxDiagnosisOneCode.Text, Nothing).Trim()

                'objDiagnosisSettingDataObject.DiagnosisTwo = Convert.ToString(TextBoxDiagnosisTwo.Text, Nothing).Trim()
                'objDiagnosisSettingDataObject.DiagnosisTwoCode = Convert.ToString(TextBoxDiagnosisTwoCode.Text, Nothing).Trim()

                'objDiagnosisSettingDataObject.DiagnosisTwo = Convert.ToString(TextBoxDiagnosisTwo.Text, Nothing).Trim()
                'objDiagnosisSettingDataObject.DiagnosisTwoCode = Convert.ToString(TextBoxDiagnosisTwoCode.Text, Nothing).Trim()

                'objDiagnosisSettingDataObject.DiagnosisThree = Convert.ToString(TextBoxDiagnosisThree.Text, Nothing).Trim()
                'objDiagnosisSettingDataObject.DiagnosisThreeCode = Convert.ToString(TextBoxDiagnosisThreeCode.Text, Nothing).Trim()

                'objDiagnosisSettingDataObject.DiagnosisFour = Convert.ToString(TextBoxDiagnosisFour.Text, Nothing).Trim()
                'objDiagnosisSettingDataObject.DiagnosisFourCode = Convert.ToString(TextBoxDiagnosisFourCode.Text, Nothing).Trim()

                Dim objBLDiagnosisSetting As New BLDiagnosisSetting()

                Select Case Convert.ToBoolean(HiddenFieldIsNew.Value, Nothing)
                    Case True
                        objDiagnosisSettingDataObject.UserId = UserId

                        Try
                            objBLDiagnosisSetting.InsertDiagnosisInfo(ConVisitel, objDiagnosisSettingDataObject)

                        Catch ex As SqlException
                            'If ex.Message.Contains("Violation of UNIQUE KEY constraint 'UQ_CityName'") Then
                            '    Master.DisplayHeaderMessage(DuplicateCityNameMessage)
                            '    Return
                            'End If
                        End Try

                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(InsertMessage)

                        Exit Select
                    Case False
                        objDiagnosisSettingDataObject.DiagnosisId = Convert.ToInt32(HiddenFieldDiagnosisId.Value)
                        objDiagnosisSettingDataObject.UpdateBy = Convert.ToString(UserId)
                        Try
                            objBLDiagnosisSetting.UpdateDiagnosisInfo(ConVisitel, objDiagnosisSettingDataObject)
                        Catch ex As SqlException
                            'If ex.Message.Contains("Violation of UNIQUE KEY constraint 'UQ_CityName'") Then
                            '    Master.DisplayHeaderMessage(DuplicateCityNameMessage)
                            '    Return
                            'End If
                        End Try

                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(UpdateMessage)
                        Exit Select
                End Select
                objBLDiagnosisSetting = Nothing

                HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
                HiddenFieldDiagnosisId.Value = Convert.ToString(Int32.MinValue)
                BindDiagnosisInformationGridView()
            End If
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            ClearControl()
        End Sub

        Private Sub ButtonSearch_Click(sender As Object, e As EventArgs)
            HiddenFieldIsSearched.Value = Convert.ToString(True, Nothing)
            BindDiagnosisInformationGridView()
            HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
        End Sub

        Private Sub GridViewDiagnosisInformation_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewDiagnosisInformation.PageIndex = e.NewPageIndex
            BindDiagnosisInformationGridView()
        End Sub

        Private Sub GridViewDiagnosisInformation_RowDataBound(sender As [Object], e As GridViewRowEventArgs)
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

            'objShared.SetControlTextLength(TextBoxDiagnosisOne, 16)
            'objShared.SetControlTextLength(TextBoxDiagnosisOneCode, 4)
            'objShared.SetControlTextLength(TextBoxDiagnosisTwo, 16)
            'objShared.SetControlTextLength(TextBoxDiagnosisTwoCode, 4)
            'objShared.SetControlTextLength(TextBoxDiagnosisThree, 16)
            'objShared.SetControlTextLength(TextBoxDiagnosisThreeCode, 4)
            'objShared.SetControlTextLength(TextBoxDiagnosisFour, 16)
            'objShared.SetControlTextLength(TextBoxDiagnosisFourCode, 4)

            'AddHandler ButtonSave.Click, AddressOf ButtonSave_Click
            'AddHandler ButtonClear.Click, AddressOf ButtonClear_Click
            'AddHandler ButtonSearch.Click, AddressOf ButtonSearch_Click

            AddHandler GridViewDiagnosisInformation.PageIndexChanging, AddressOf GridViewDiagnosisInformation_PageIndexChanging
            'AddHandler GridViewDiagnosisInformation.RowDataBound, AddressOf GridViewDiagnosisInformation_RowDataBound

            GridViewDiagnosisInformation.AutoGenerateColumns = False
            GridViewDiagnosisInformation.ShowHeaderWhenEmpty = True
            GridViewDiagnosisInformation.PageSize = objShared.GridViewDefaultPageSize
            GridViewDiagnosisInformation.AllowPaging = True

        End Sub

        Private Sub LoadJavascript()
            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                              & " var DeleteDialogHeader ='Diagnosis'; " _
                              & " var DeleteDialogConfirmMsg ='Do you want to delete this record?'; " _
                          & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))
        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            'LabelDiagnosisOne.Text = Convert.ToString(ResourceTable("LabelDiagnosisOne"), Nothing)
            'LabelDiagnosisOne.Text = If(String.IsNullOrEmpty(LabelDiagnosisOne.Text), "Diagnosis One", LabelDiagnosisOne.Text)

            'LabelDiagnosisTwo.Text = Convert.ToString(ResourceTable("LabelDiagnosisTwo"), Nothing)
            'LabelDiagnosisTwo.Text = If(String.IsNullOrEmpty(LabelDiagnosisTwo.Text), "Diagnosis Two", LabelDiagnosisTwo.Text)

            'LabelDiagnosisThree.Text = Convert.ToString(ResourceTable("LabelDiagnosisThree"), Nothing)
            'LabelDiagnosisThree.Text = If(String.IsNullOrEmpty(LabelDiagnosisThree.Text), "Diagnosis Three", LabelDiagnosisThree.Text)

            'LabelDiagnosisFour.Text = Convert.ToString(ResourceTable("LabelDiagnosisFour"), Nothing)
            'LabelDiagnosisFour.Text = If(String.IsNullOrEmpty(LabelDiagnosisFour.Text), "Diagnosis Four", LabelDiagnosisFour.Text)

            'ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)
            'ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            'ButtonClear.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing)
            'ButtonClear.Text = If(String.IsNullOrEmpty(ButtonClear.Text), "Clear", ButtonClear.Text)

            'ButtonSearch.Text = Convert.ToString(ResourceTable("ButtonSearch"), Nothing)
            'ButtonSearch.Text = If(String.IsNullOrEmpty(ButtonSearch.Text), "Search", ButtonSearch.Text)

            'LabelDiagnosisInformationEntry.Text = Convert.ToString(ResourceTable("LabelDiagnosisInformationEntry"), Nothing)
            'LabelDiagnosisInformationEntry.Text = If(String.IsNullOrEmpty(LabelDiagnosisInformationEntry.Text),
            '                                          "Diagnosis Information Entry", LabelDiagnosisInformationEntry.Text)

            LabelDiagnosisInformationList.Text = Convert.ToString(ResourceTable("LabelDiagnosisInformationList"), Nothing)
            LabelDiagnosisInformationList.Text = If(String.IsNullOrEmpty(LabelDiagnosisInformationList.Text),
                                                     "Diagnosis Information List", LabelDiagnosisInformationList.Text)

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

            ResourceTable = Nothing
        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData() As Boolean

            'If String.IsNullOrEmpty(TextBoxCityName.Text.Trim()) Then
            '    Master.DisplayHeaderMessage(EmptyCityNameMessage)
            '    Return False
            'End If
            Return True
        End Function

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Private Sub GetData()
            BindDiagnosisInformationGridView()
        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Private Sub ClearControl()

            'TextBoxDiagnosisOne.Text = objShared.InlineAssignHelper(TextBoxDiagnosisOneCode.Text, objShared.InlineAssignHelper(TextBoxDiagnosisTwo.Text,
            '                            objShared.InlineAssignHelper(TextBoxDiagnosisTwoCode.Text, objShared.InlineAssignHelper(TextBoxDiagnosisThree.Text,
            '                            objShared.InlineAssignHelper(TextBoxDiagnosisThreeCode.Text, objShared.InlineAssignHelper(TextBoxDiagnosisFour.Text,
            '                            objShared.InlineAssignHelper(TextBoxDiagnosisFourCode.Text, String.Empty)))))))
      

            HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
            HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
            HiddenFieldDiagnosisId.Value = Convert.ToString(Int32.MinValue)
        End Sub

        ''' <summary>
        ''' Binding Diagnosis Information GridView
        ''' </summary>
        Private Sub BindDiagnosisInformationGridView()

            Dim DiagnosisSettingList As List(Of DiagnosisSettingDataObject)

            Dim objBLDiagnosisSetting As New BLDiagnosisSetting()

            Dim objDiagnosisSettingDataObject As DiagnosisSettingDataObject

            If (Convert.ToBoolean(HiddenFieldIsSearched.Value, Nothing)) Then

                objDiagnosisSettingDataObject = New DiagnosisSettingDataObject()

                'objDiagnosisSettingDataObject.DiagnosisOne = TextBoxDiagnosisOne.Text.Trim()
                'objDiagnosisSettingDataObject.DiagnosisOneCode = TextBoxDiagnosisOneCode.Text.Trim()
                'objDiagnosisSettingDataObject.DiagnosisTwo = TextBoxDiagnosisTwo.Text.Trim()
                'objDiagnosisSettingDataObject.DiagnosisTwoCode = TextBoxDiagnosisTwoCode.Text.Trim()
                'objDiagnosisSettingDataObject.DiagnosisThree = TextBoxDiagnosisThree.Text.Trim()
                'objDiagnosisSettingDataObject.DiagnosisThreeCode = TextBoxDiagnosisThreeCode.Text.Trim()
                'objDiagnosisSettingDataObject.DiagnosisFour = TextBoxDiagnosisFour.Text.Trim()
                'objDiagnosisSettingDataObject.DiagnosisFourCode = TextBoxDiagnosisFourCode.Text.Trim()

                DiagnosisSettingList = objBLDiagnosisSetting.SearchDiagnosisInfo(ConVisitel, objDiagnosisSettingDataObject)

                objDiagnosisSettingDataObject = Nothing
            Else
                DiagnosisSettingList = objBLDiagnosisSetting.SelectDiagnosisInfo(ConVisitel, CompanyId)
            End If

            objBLDiagnosisSetting = Nothing

            GridViewDiagnosisInformation.DataSource = DiagnosisSettingList
            GridViewDiagnosisInformation.DataBind()

            DiagnosisSettingList = Nothing
        End Sub

    End Class
End Namespace