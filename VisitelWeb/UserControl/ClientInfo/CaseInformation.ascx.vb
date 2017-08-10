
Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Data.SqlClient

Namespace Visitel.UserControl.ClientInfo
    Public Class CaseInformationControl
        Inherits BaseUserControl

        Public CaseInfoValidationGroup As String

        Private ValidationEnable As Boolean

        Private LabelCaseInformationCommentsText As String, SocialSecurityNumber As String, StateClientId As String

        Public IndividualId As Int32, CompanyId As Int32, UserId As Int32

        Public RowCount As Integer = 0

        Private CaseInfoList As List(Of CaseInfoDataObject)
        Private CaseWorkerId As Integer

#Region "Dynamic Controls"

        Private UpdatePanelCaseInfoManage As UpdatePanel

        Private DropDownListCaseInformationCaseWorker As DropDownList, DropDownListCaseInformationEmployee As DropDownList
        Private TextBoxCaseInformationDate As TextBox, TextBoxCaseInformationReceiverOrganization As TextBox, TextBoxCaseInformationUpdateDate As TextBox,
            TextBoxCaseInformationUpdateBy As TextBox, TextBoxCaseInformationComments As TextBox
        Private LabelCaseInformationComments As Label

        Private RegularExpressionValidatorCaseInformationDate As RegularExpressionValidator

#End Region

        Private newRow As HtmlGenericControl, SecondColumn As HtmlGenericControl, CaseInfoServiceBox As HtmlGenericControl, ServiceBoxCaseInformation As HtmlGenericControl,
            DivSpace As HtmlGenericControl

        Private DynamicHiddenFieldCaseInfoId As HiddenField

        Private ControlName As String = "CaseInfoControl"
        Private objShared As SharedWebControls

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            InitializeControl()
            GetCaptionFromResource()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)

            If Not IsPostBack Then
                CaseInfoList = New List(Of CaseInfoDataObject)()
                Me.IndividualId = Int32.MinValue
                Me.CaseWorkerId = Int32.MinValue
            End If

            GetData()

        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("ClientInfo/" & ControlName)
            LoadJScript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        Private Sub ButtonCaseInformationSave_Click(sender As Object, e As EventArgs)
            SaveData()
            objShared.TabSelection(4)
        End Sub

        Private Sub ButtonCaseInformationDelete_Click(sender As Object, e As EventArgs)
            If (Convert.ToInt64(HiddenFieldCaseInfoId.Value) > 0) Then
                DeleteData()
            End If
            objShared.TabSelection(4)
        End Sub

        Private Sub ButtonCaseInformationClear_Click(sender As Object, e As EventArgs)

            IndividualId = Int32.MinValue
            CaseWorkerId = Int32.MinValue
            CaseInfoList.Clear()
            ClearControl()
            LoadControlsWithCaseInfoData()
            HiddenFieldCaseInfoId.Value = String.Empty
            objShared.TabSelection(4)

        End Sub

        Private Sub GetData()
            LoadControlsWithCaseInfoData()
        End Sub

        ''' <summary>
        ''' Initializing Controls
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitializeControl()
            ButtonFaxCoverPageReport.Attributes.Add("OnClick", "ShowFaxCoverPageReport(); return false;")
            ButtonFaxCoverPageReport.UseSubmitBehavior = False
            ButtonFaxCoverPageReport.ClientIDMode = ClientIDMode.Static

            ButtonOpenReport.Attributes.Add("OnClick", "ShowCaseInformationReport(); return false;")
            ButtonOpenReport.UseSubmitBehavior = False
            ButtonOpenReport.ClientIDMode = ClientIDMode.Static

            ButtonCaseInformationSave.CausesValidation = True
            'ButtonCaseInformationSave.ValidationGroup = CaseInformationControl.CaseInfoValidationGroup

            ButtonCaseInformationDelete.CausesValidation = objShared.InlineAssignHelper(ButtonCaseInformationClear.CausesValidation, False)

            AddHandler ButtonCaseInformationSave.Click, AddressOf ButtonCaseInformationSave_Click
            AddHandler ButtonCaseInformationDelete.Click, AddressOf ButtonCaseInformationDelete_Click
            AddHandler ButtonCaseInformationClear.Click, AddressOf ButtonCaseInformationClear_Click

            HiddenFieldCaseInfoId.ClientIDMode = ClientIDMode.Static
            ButtonFaxCoverPageReport.ClientIDMode = ClientIDMode.Static
            ButtonOpenReport.ClientIDMode = ClientIDMode.Static

            ButtonCaseInformationClear.ClientIDMode = ClientIDMode.Static
            ButtonCaseInformationSave.ClientIDMode = ClientIDMode.Static
            ButtonCaseInformationDelete.ClientIDMode = ClientIDMode.Static
        End Sub

        ''' <summary>
        ''' Importing Javascript
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadJScript()

            LoadCaseInfoJavaScript()

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                        & "     var DeleteTargetButton2 ='ButtonCaseInformationDelete'; " _
                        & "     var DeleteDialogHeader2 ='Case Information'; " _
                        & "     var DeleteDialogConfirmMsg2 ='Do you want to delete?'; " _
                        & "     var prm =''; " _
                        & "     jQuery(document).ready(function () {" _
                        & "         prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                        & "         CaseInfoDataDelete();" _
                        & "         prm.add_endRequest(CaseInfoDataDelete); " _
                        & "     }); " _
                        & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/ClientInfo/" & ControlName & ".js")

        End Sub


        Public Sub LoadCaseInfoJavaScript()

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                        & "     var CompanyId=" & CompanyId & "; " _
                        & "     var RowCounter = " & RowCount & "; " _
                        & "     var ReportsViewPath ='" & objShared.GetPopupUrl("Reports/ReportsView.aspx") & "'; " _
                        & "     var CaseInfoCalendarDateFormat='" & objShared.CalendarDateFormat & "'; " _
                        & "     var CaseInfoCalendarImagePath='" & objShared.GetCalendarImagePath & "'; " _
                        & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

        End Sub

        ''' <summary>
        ''' Set Client Id
        ''' </summary>
        ''' <param name="clientId"></param>
        ''' <remarks></remarks>
        Public Sub SetClientId(ByVal ClientId As Integer)
            IndividualId = ClientId
            CompanyId = objShared.CompanyId
            UserId = objShared.UserId
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

        Public Sub SetCaseworkerId(ByVal CaseWorkerId As Integer)
            Me.CaseWorkerId = CaseWorkerId
        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Public Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("ClientInfo", ControlName & ".resx")

            LabelCaseInformation.Text = Convert.ToString(ResourceTable("LabelCaseInformation"), Nothing).Trim()
            LabelCaseInformation.Text = If(String.IsNullOrEmpty(LabelCaseInformation.Text), "Case Information", LabelCaseInformation.Text)

            LabelCaseInformationCaseWorker.Text = Convert.ToString(ResourceTable("LabelCaseInformationCaseWorker"), Nothing).Trim()
            LabelCaseInformationCaseWorker.Text = If(String.IsNullOrEmpty(LabelCaseInformationCaseWorker.Text), "Case Worker", LabelCaseInformationCaseWorker.Text)

            LabelCaseInformationEmployee.Text = Convert.ToString(ResourceTable("LabelCaseInformationEmployee"), Nothing).Trim()
            LabelCaseInformationEmployee.Text = If(String.IsNullOrEmpty(LabelCaseInformationEmployee.Text), "Employee", LabelCaseInformationEmployee.Text)

            LabelCaseInformationDate.Text = Convert.ToString(ResourceTable("LabelCaseInformationDate"), Nothing).Trim()
            LabelCaseInformationDate.Text = If(String.IsNullOrEmpty(LabelCaseInformationDate.Text), "Date", LabelCaseInformationDate.Text)

            LabelCaseInformationReceiverOrganization.Text = Convert.ToString(ResourceTable("LabelCaseInformationReceiverOrganization"), Nothing).Trim()
            LabelCaseInformationReceiverOrganization.Text = If(String.IsNullOrEmpty(LabelCaseInformationReceiverOrganization.Text), "Receiver Organization",
                                                               LabelCaseInformationReceiverOrganization.Text)

            LabelCaseInformationUpdateDate.Text = Convert.ToString(ResourceTable("LabelCaseInformationUpdateDate"), Nothing).Trim()
            LabelCaseInformationUpdateDate.Text = If(String.IsNullOrEmpty(LabelCaseInformationUpdateDate.Text), "Update Date", LabelCaseInformationUpdateDate.Text)

            LabelCaseInformationUpdateBy.Text = Convert.ToString(ResourceTable("LabelCaseInformationUpdateBy"), Nothing).Trim()
            LabelCaseInformationUpdateBy.Text = If(String.IsNullOrEmpty(LabelCaseInformationUpdateBy.Text), "Update By", LabelCaseInformationUpdateBy.Text)

            LabelCaseInformationCommentsText = Convert.ToString(ResourceTable("LabelCaseInformationComments"), Nothing).Trim()
            LabelCaseInformationCommentsText = If(String.IsNullOrEmpty(LabelCaseInformationCommentsText), "Comments:", LabelCaseInformationCommentsText)

            ButtonFaxCoverPageReport.Text = Convert.ToString(ResourceTable("ButtonFaxCoverPageReport"), Nothing).Trim()
            ButtonFaxCoverPageReport.Text = If(String.IsNullOrEmpty(ButtonFaxCoverPageReport.Text), "Fax Cover Page", ButtonFaxCoverPageReport.Text)

            ButtonOpenReport.Text = Convert.ToString(ResourceTable("ButtonOpenReport"), Nothing).Trim()
            ButtonOpenReport.Text = If(String.IsNullOrEmpty(ButtonOpenReport.Text), "Open Report", ButtonOpenReport.Text)

            ButtonCaseInformationSave.Text = Convert.ToString(ResourceTable("ButtonCaseInformationSave"), Nothing).Trim()
            ButtonCaseInformationSave.Text = If(String.IsNullOrEmpty(ButtonCaseInformationSave.Text), "Save", ButtonCaseInformationSave.Text)

            ButtonCaseInformationClear.Text = Convert.ToString(ResourceTable("ButtonCaseInformationClear"), Nothing).Trim()
            ButtonCaseInformationClear.Text = If(String.IsNullOrEmpty(ButtonCaseInformationClear.Text), "Clear", ButtonCaseInformationClear.Text)

            ButtonCaseInformationDelete.Text = Convert.ToString(ResourceTable("ButtonCaseInformationDelete"), Nothing).Trim()
            ButtonCaseInformationDelete.Text = If(String.IsNullOrEmpty(ButtonCaseInformationDelete.Text), "Delete", ButtonCaseInformationDelete.Text)

            Dim ResultOut As Boolean

            ValidationEnable = If((Boolean.TryParse(ResourceTable("ValidationEnable"), ResultOut)), ResultOut, True)

            CaseInfoValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing)
            CaseInfoValidationGroup = If(String.IsNullOrEmpty(CaseInfoValidationGroup), "CaseInfo", CaseInfoValidationGroup)

            ButtonCaseInformationSave.ValidationGroup = CaseInfoValidationGroup

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' Creating Dynamic Controls
        ''' </summary>
        ''' <param name="index"></param>
        ''' <remarks></remarks>
        Private Sub CreateControls(index As Integer)

            UpdatePanelCaseInfoManage = New UpdatePanel()
            UpdatePanelCaseInfoManage.ID = "UpdatePanelCaseInfoManage_" & index
            UpdatePanelCaseInfoManage.UpdateMode = UpdatePanelUpdateMode.Always
            UpdatePanelCaseInfoManage.Attributes.Add("OnClick", "SetCaseInfoId(" & index & ")")

            CaseInfoServiceBox = New HtmlGenericControl(objShared.GenericControlRow)
            CaseInfoServiceBox.Attributes.Add("class", "CaseInfoServiceBox")

            ServiceBoxCaseInformation = New HtmlGenericControl(objShared.GenericControlRow)
            ServiceBoxCaseInformation.Attributes.Add("class", "CaseInfoBoxStyle ServiceFull ServiceBoxCaseInformation")

            newRow = New HtmlGenericControl(objShared.GenericControlRow)
            newRow.Attributes.Add("class", "newRow")

            DynamicHiddenFieldCaseInfoId = New HiddenField()
            DynamicHiddenFieldCaseInfoId.ID = "DynamicHiddenFieldCaseInfoId_" & index
            DynamicHiddenFieldCaseInfoId.ClientIDMode = ClientIDMode.Static
            newRow.Controls.Add(DynamicHiddenFieldCaseInfoId)

            DropDownListCaseInformationCaseWorker = objShared.AddControls(index, EnumDataObject.ControlType.DropDownList, newRow,
                                                                "DropDownListCaseInformationCaseWorker", "DropDownListCaseInformationCaseWorker", True,
                                                                "SecondColumn DivCaseInformationCaseWorker")

            DropDownListCaseInformationEmployee = objShared.AddControls(index, EnumDataObject.ControlType.DropDownList, newRow,
                                                              "DropDownListCaseInformationEmployee", "DropDownListCaseInformationEmployee", True,
                                                              "SecondColumn DivCaseInformationEmployee")

            TextBoxCaseInformationDate = objShared.AddControls(index, EnumDataObject.ControlType.TextBox, newRow,
                                                     "TextBoxCaseInformationDate", "dateField", True,
                                                     "SecondColumn DivCaseInformationDate")

            RegularExpressionValidatorCaseInformationDate = New RegularExpressionValidator
            RegularExpressionValidatorCaseInformationDate.ID = "RegularExpressionValidatorCaseInformationDate_" & index
            newRow.Controls.Add(RegularExpressionValidatorCaseInformationDate)

            TextBoxCaseInformationReceiverOrganization = objShared.AddControls(index, EnumDataObject.ControlType.TextBox, newRow,
                                                                     "TextBoxCaseInformationReceiverOrganization", "TextBoxCaseInformationReceiverOrganization", True,
                                                        "SecondColumn DivCaseInformationReceiverOrganization")

            TextBoxCaseInformationUpdateDate = objShared.AddControls(index, EnumDataObject.ControlType.TextBox, newRow,
                                                           "TextBoxCaseInformationUpdateDate", "TextBoxCaseInformationUpdateDate", True,
                                                           "SecondColumn DivCaseInformationUpdateDate")

            TextBoxCaseInformationUpdateBy = objShared.AddControls(index, EnumDataObject.ControlType.TextBox, newRow,
                                                         "TextBoxCaseInformationUpdateBy", "TextBoxCaseInformationUpdateBy", True,
                                                         "SecondColumn DivCaseInformationUpdateBy")

            ServiceBoxCaseInformation.Controls.Add(newRow)

            newRow = New HtmlGenericControl(objShared.GenericControlRow)
            newRow.Attributes.Add("class", "newRow")

            LabelCaseInformationComments = objShared.AddControls(index, EnumDataObject.ControlType.Label, newRow,
                                                       "LabelCaseInformationComments", "LabelCaseInformationComments", True, "SecondColumn")

            TextBoxCaseInformationComments = objShared.AddControls(index, EnumDataObject.ControlType.TextBox, newRow,
                                                         "TextBoxCaseInformationComments", "TextBoxCaseInformationComments", True, "SecondColumn DivCaseInformationComments")
            TextBoxCaseInformationComments.TextMode = TextBoxMode.MultiLine

            DivSpace = New HtmlGenericControl(objShared.GenericControlRow)
            DivSpace.Attributes.Add("class", "DivSpace3")

            newRow.Controls.Add(DivSpace)

            ServiceBoxCaseInformation.Controls.Add(newRow)

            CaseInfoServiceBox.Controls.Add(ServiceBoxCaseInformation)

            UpdatePanelCaseInfoManage.ContentTemplateContainer.Controls.Add(CaseInfoServiceBox)

            PlaceHolderCaseInformation.Controls.Add(UpdatePanelCaseInfoManage)

            GetDynamicControlCaption()

            InitializeDynamicControl(index)

            BindDynamicDropDownList()

        End Sub

        Private Sub GetDynamicControlCaption()
            LabelCaseInformationComments.Text = LabelCaseInformationCommentsText
        End Sub

        ''' <summary>
        ''' Retrieving Case Info by Client Id 
        ''' </summary>
        ''' <returns></returns>
        Public Function GetCaseInformation() As List(Of CaseInfoDataObject)

            Dim objBLCaseInfo As New BLCaseInfo()

            Dim CaseInfoList As List(Of CaseInfoDataObject)

            CaseInfoList = objBLCaseInfo.SelectCaseInfo(objShared.ConVisitel, IndividualId, CompanyId)

            objBLCaseInfo = Nothing

            Return CaseInfoList

        End Function

        ''' <summary>
        ''' Dynamic Control Index Assign
        ''' </summary>
        ''' <param name="index"></param>
        ''' <remarks></remarks>
        Private Sub ControlReferenceAssign(index As Integer)

            DropDownListCaseInformationCaseWorker = DirectCast(PlaceHolderCaseInformation.FindControl("DropDownListCaseInformationCaseWorker_" & index), DropDownList)
            DropDownListCaseInformationEmployee = DirectCast(PlaceHolderCaseInformation.FindControl("DropDownListCaseInformationEmployee_" & index), DropDownList)

            TextBoxCaseInformationDate = DirectCast(PlaceHolderCaseInformation.FindControl("TextBoxCaseInformationDate_" & index), TextBox)
            TextBoxCaseInformationReceiverOrganization = DirectCast(PlaceHolderCaseInformation.FindControl("TextBoxCaseInformationReceiverOrganization_" & index), TextBox)

            TextBoxCaseInformationUpdateDate = DirectCast(PlaceHolderCaseInformation.FindControl("TextBoxCaseInformationUpdateDate_" & index), TextBox)
            TextBoxCaseInformationUpdateBy = DirectCast(PlaceHolderCaseInformation.FindControl("TextBoxCaseInformationUpdateBy_" & index), TextBox)

            TextBoxCaseInformationComments = DirectCast(PlaceHolderCaseInformation.FindControl("TextBoxCaseInformationComments_" & index), TextBox)

        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Public Sub ClearControl()

            DropDownListCaseInformationCaseWorker.SelectedIndex = 0
            DropDownListCaseInformationEmployee.SelectedIndex = 0

            TextBoxCaseInformationDate.Text = String.Empty
            TextBoxCaseInformationReceiverOrganization.Text = String.Empty
            TextBoxCaseInformationUpdateDate.Text = String.Empty
            TextBoxCaseInformationUpdateBy.Text = String.Empty
            TextBoxCaseInformationComments.Text = String.Empty

        End Sub

        ''' <summary>
        ''' Fill out dynamic controls with data
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub LoadControlsWithCaseInfoData()

            PlaceHolderCaseInformation.Controls.Clear()

            CaseInfoList = GetCaseInformation()

            RowCount = CaseInfoList.Count + 1

            Dim RowIndex As Integer = 1
            For Each CaseInfo In CaseInfoList
                CreateControls(RowIndex)
                HiddenFieldCaseInfoId.Value = CaseInfo.Id
                LoadSelectedCaseInfo()
                RowIndex = RowIndex + 1
            Next
            CreateControls(RowIndex)
        End Sub

        ''' <summary>
        ''' Retrieving Selected Case Information
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadSelectedCaseInfo()

            Dim objCaseInfoDataObject As New CaseInfoDataObject
            objCaseInfoDataObject = (From p In CaseInfoList Where p.Id = HiddenFieldCaseInfoId.Value).SingleOrDefault

            If (objCaseInfoDataObject Is Nothing) Then
                Return
            End If

            DynamicHiddenFieldCaseInfoId.Value = Convert.ToString(objCaseInfoDataObject.Id)

            DropDownListCaseInformationCaseWorker.SelectedIndex = DropDownListCaseInformationCaseWorker.Items.IndexOf(DropDownListCaseInformationCaseWorker.Items.FindByValue(
                                                                                               Convert.ToString(objCaseInfoDataObject.CaseWorkerId)))

            DropDownListCaseInformationEmployee.SelectedIndex = DropDownListCaseInformationEmployee.Items.IndexOf(DropDownListCaseInformationEmployee.Items.FindByValue(
                                                                                               Convert.ToString(objCaseInfoDataObject.EmployeeId)))

            TextBoxCaseInformationDate.Text = Convert.ToString(objCaseInfoDataObject.CaseInfoDate, Nothing)

            TextBoxCaseInformationReceiverOrganization.Text = objCaseInfoDataObject.ReceiverOrganization

            TextBoxCaseInformationUpdateDate.Text = Convert.ToString(objCaseInfoDataObject.UpdateDate, Nothing)

            TextBoxCaseInformationUpdateBy.Text = objCaseInfoDataObject.UpdateBy
            TextBoxCaseInformationComments.Text = objCaseInfoDataObject.Comments

        End Sub

        ''' <summary>
        ''' Set data before going for save
        ''' </summary>
        ''' <param name="CaseWorkerId"></param>
        ''' <param name="EmployeeId"></param>
        ''' <param name="objCaseInfoDataObject"></param>
        ''' <remarks></remarks>
        Private Sub SetData(CaseWorkerId As Integer, EmployeeId As Integer, objCaseInfoDataObject As CaseInfoDataObject)

            objCaseInfoDataObject.CaseWorkerId = CaseWorkerId
            objCaseInfoDataObject.EmployeeId = EmployeeId

            objCaseInfoDataObject.CaseInfoDate = Convert.ToString(TextBoxCaseInformationDate.Text, Nothing).Trim()
            objCaseInfoDataObject.ReceiverOrganization = Convert.ToString(TextBoxCaseInformationReceiverOrganization.Text, Nothing).Trim()
            objCaseInfoDataObject.Comments = Convert.ToString(TextBoxCaseInformationComments.Text, Nothing).Trim()

        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData() As Boolean

            If ((Not String.IsNullOrEmpty(TextBoxCaseInformationDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxCaseInformationDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorCaseInformationDate.ErrorMessage)
                Return False
            End If

            Return True

        End Function

        ''' <summary>
        ''' Importing Dynamic Javascript
        ''' </summary>
        ''' <param name="Control"></param>
        ''' <remarks></remarks>
        Public Sub LoadDynamicJavascript(ByRef Control As Control)
            ScriptManager.RegisterClientScriptBlock(Control, Me.GetType(), "CaseInfoDateFields", "RowCounter = " & RowCount & ";CaseInfoDateFieldsEvent();", True)
        End Sub

        ''' <summary>
        ''' This would take care of save functionality
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SaveData()

            If (IndividualId.Equals(-1)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select one client")
                Return
            End If

            Page.Validate()
            Page.Validate(CaseInfoValidationGroup)

            Dim IsSaved As Boolean = False
            Dim objBLCaseInfo As New BLCaseInfo()
            Dim index As Integer = 1

            For Each CaseInfo In CaseInfoList

                ControlReferenceAssign(index)

                If ((Page.IsValid) And (ValidateData())) Then

                    SetData(Convert.ToInt32(DropDownListCaseInformationCaseWorker.SelectedValue), Convert.ToInt32(DropDownListCaseInformationEmployee.SelectedValue), CaseInfo)
                    IsSaved = False

                    CaseInfo.UpdateBy = Convert.ToString(UserId)
                    CaseInfo.CompanyId = CompanyId

                    Try

                        objBLCaseInfo.UpdateCaseInfo(objShared.ConVisitel, CaseInfo)
                        IsSaved = True

                    Catch ex As SqlException

                        If ex.Message.Contains("Duplicate Case Information") Then
                            DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Duplicate Case Information")
                        Else
                            DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Update")
                        End If

                    End Try

                    If (IsSaved) Then
                        index = index + 1
                    Else
                        objBLCaseInfo = Nothing
                        Return
                    End If

                Else
                    IsSaved = False
                    Return
                End If

            Next

            Dim objCaseInfoDataObject As CaseInfoDataObject
            If ((Page.IsValid) And (ValidateData())) Then

                ControlReferenceAssign(index)

                If (DropDownListCaseInformationCaseWorker.SelectedValue.Equals("-1") And DropDownListCaseInformationEmployee.SelectedValue.Equals("-1")) Then

                Else

                    objCaseInfoDataObject = New CaseInfoDataObject()

                    SetData(Convert.ToInt32(DropDownListCaseInformationCaseWorker.SelectedValue), Convert.ToInt32(DropDownListCaseInformationEmployee.SelectedValue),
                            objCaseInfoDataObject)

                    IsSaved = False

                    objCaseInfoDataObject.ClientId = IndividualId
                    objCaseInfoDataObject.UserId = UserId
                    objCaseInfoDataObject.CompanyId = CompanyId

                    Try
                        objBLCaseInfo.InsertCaseInfo(objShared.ConVisitel, objCaseInfoDataObject)
                        IsSaved = True

                    Catch ex As SqlException

                        If ex.Message.Contains("Duplicate Case Information") Then
                            DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Duplicate Case Information")
                        Else
                            DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Insert")
                        End If

                    End Try

                    If (IsSaved) Then

                    Else
                        objBLCaseInfo = Nothing
                        objCaseInfoDataObject = Nothing
                        Return
                    End If

                End If

            End If

            If (IsSaved) Then

                objBLCaseInfo = Nothing
                objCaseInfoDataObject = Nothing
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Saved Successfully")
                LoadControlsWithCaseInfoData()
            Else
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Save")
            End If

        End Sub

        ''' <summary>
        ''' Deleting particular case information
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub DeleteData()

            Dim IsDeleted As Boolean = False
            Dim objBLCaseInfo As New BLCaseInfo()

            Try
                objBLCaseInfo.DeleteCaseInfo(objShared.ConVisitel, Convert.ToInt32(HiddenFieldCaseInfoId.Value), UserId)
                IsDeleted = True

            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Delete")
            End Try

            objBLCaseInfo = Nothing

            If (IsDeleted) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Deleted Successfully")
                ClearControl()
                HiddenFieldCaseInfoId.Value = Convert.ToString(Int32.MinValue)
                LoadControlsWithCaseInfoData()
            End If

        End Sub

        ''' <summary>
        ''' Initializing Dynamic Controls
        ''' </summary>
        ''' <param name="index"></param>
        ''' <remarks></remarks>
        Private Sub InitializeDynamicControl(index As Integer)

            DropDownListCaseInformationCaseWorker.ClientIDMode = ClientIDMode.Static
            DropDownListCaseInformationEmployee.ClientIDMode = ClientIDMode.Static

            TextBoxCaseInformationDate.ClientIDMode = ClientIDMode.Static

            TextBoxCaseInformationUpdateDate.ReadOnly = objShared.InlineAssignHelper(TextBoxCaseInformationUpdateBy.ReadOnly, True)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorCaseInformationDate, "TextBoxCaseInformationDate_" & index,
                                                           objShared.DateValidationExpression, "Invalid Date",
                                                    "Invalid Date", ValidationEnable, CaseInfoValidationGroup)


            objShared.SetControlTextLength(TextBoxCaseInformationDate, 17)
            objShared.SetControlTextLength(TextBoxCaseInformationReceiverOrganization, 255)
            objShared.SetControlTextLength(TextBoxCaseInformationComments, 4000)

        End Sub

        ''' <summary>
        ''' This is for filling out dynamic drop down list
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindDynamicDropDownList()

            objShared.BindCaseWorkerDropDownList(DropDownListCaseInformationCaseWorker, CompanyId)

            DropDownListCaseInformationCaseWorker.SelectedIndex = DropDownListCaseInformationCaseWorker.Items.IndexOf(
                DropDownListCaseInformationCaseWorker.Items.FindByValue(CaseWorkerId))

            objShared.BindEmployeeDropDownList(DropDownListCaseInformationEmployee, CompanyId, True)
        End Sub

    End Class
End Namespace