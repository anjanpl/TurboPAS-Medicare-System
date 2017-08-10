
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: EDI Submission
' Author: Anjan Kumar Paul
' Start Date: 15 July 2015
' End Date: 
' History:
'      Version                  Author                      Date             Reason 
'      1.0.0                                                15 July 2015     Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Data.SqlClient
Imports System.Linq.Expressions
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Globalization
Imports Microsoft.Security.Application


Namespace Visitel.UserControl.EDISubmission
    Public Class EDISubmissionControl
        Inherits BaseUserControl

        Private ControlName As String, SelectedClientIdList As String = String.Empty
        Private TotalBillAmount As Double = 0.0, DoubleOutResult As Double
        Private objShared As SharedWebControls
        Private CurrentRow As GridViewRow
        Private EDILoginList As List(Of EDILoginDataObject)
        Private CultureInfo As CultureInfo

        Private MissingData As Boolean = False

        Private LabelClientId As Label, LabelId As Label, LabelProgram As Label, LabelReceiver As Label, LabelContract As Label, LabelClaimFrequencyTypeCode As Label

        Private CheckBoxSelect As CheckBox, chkAll As CheckBox

        Private DropDownListDiagnosisOne As DropDownList, DropDownListDiagnosisTwo As DropDownList, DropDownListDiagnosisThree As DropDownList,
            DropDownListDiagnosisFour As DropDownList, DropDownListPlaceOfService As DropDownList

        Private TextBoxStartDate As TextBox, TextBoxEndDate As TextBox, TextBoxName As TextBox, TextBoxAddress As TextBox, TextBoxReferralNumber As TextBox,
            TextBoxGender As TextBox, TextBoxDateOfBirth As TextBox, TextBoxMedicaidNumber As TextBox, TextBoxBillUnits As TextBox, TextBoxUnitRate As TextBox,
            TextBoxAmount As TextBox, TextBoxProcedureCode As TextBox, TextBoxModifierOne As TextBox, TextBoxModifierTwo As TextBox, TextBoxModifierThree As TextBox,
            TextBoxModifierFour As TextBox

        Private PayPeriods As List(Of PayPeriodDataObject)

        Private DiagnosisCodeList As New List(Of DiagnosisDataObject)()

        Private EDISubmissionList As List(Of EDISubmissionDataObject)

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            DirectCast(Me.Page.Master, IMyMasterPage).PageHeaderTitle = "EDI Submission"
            ControlName = "EDISubmissionControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            GetCaptionFromResource()
            InitializeControl()

        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)

            GetDiagnosisData()

            If Not IsPostBack Then
                GetData()
            End If

            ButtonIndividualBilling.Enabled = If((DetermineButtonInActivity(GridViewEDISubmissionDetail, "CheckBoxSelect")), True, False)
            ButtonIndividualDetail.Enabled = ButtonIndividualBilling.Enabled

        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("EDISubmission/" + ControlName)
            LoadJScript()
            TextBoxTotalBillAmount.Text = String.Format("${0}", TotalBillAmount.ToString("F2"))
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            TotalBillAmount = 0

            PayPeriods = Nothing

            DiagnosisCodeList = Nothing

            EDISubmissionList = Nothing

            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        ''' <summary>
        ''' Gridview Row Hover Color Set
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Render(writer As HtmlTextWriter)
            For Each r As GridViewRow In GridViewEDISubmissionDetail.Rows
                If r.RowType = DataControlRowType.DataRow Then
                    objShared.SetGridViewRowColor(r)
                End If
            Next
            MyBase.Render(writer)
        End Sub

        Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)

            Dim chk As CheckBox = TryCast(sender, CheckBox)

            If chk.ID.Equals("chkAll") Then
                GridViewAllSelectCheckboxSelectChange(chk, CurrentRow, GridViewEDISubmissionDetail, CheckBoxSelect, "CheckBoxSelect")
            End If

            GridViewControlsOnEdit(GridViewEDISubmissionDetail, "chkAll", "CheckBoxSelect")

            For Each row As GridViewRow In GridViewEDISubmissionDetail.Rows
                If row.RowType.Equals(DataControlRowType.DataRow) Then

                    Dim isChecked As Boolean = DirectCast(row.FindControl("CheckBoxSelect"), CheckBox).Checked

                    If (isChecked) Then
                        LabelClientId = DirectCast(row.FindControl("LabelClientId"), Label)
                        If ((Not HiddenFieldClientId Is Nothing) And (Not LabelClientId Is Nothing)) Then
                            HiddenFieldClientId.Value = LabelClientId.Text.Trim()
                        End If
                    End If
                End If
            Next

        End Sub

        Private Sub GridViewEDISubmissionDetail_RowDataBound(sender As Object, e As GridViewRowEventArgs)

            CurrentRow = DirectCast(e.Row, GridViewRow)

            If (e.Row.RowType.Equals(DataControlRowType.Header)) Then
                SetGridViewColumnHeaderText(CurrentRow)

                chkAll = DirectCast(CurrentRow.FindControl("chkAll"), CheckBox)
                chkAll.AutoPostBack = True
                chkAll.ClientIDMode = ClientIDMode.Static
            End If

            'e.Row.Attributes.Add("onmouseover", "this.style.cursor='pointer';this.style.backgroundColor='#D2E6F8'")
            'e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#ffffff'")

            'r.Attributes("onclick") = Me.Page.ClientScript.GetPostBackClientHyperlink(Me.GridViewPayPeriodDetail, "Select$" & Convert.ToString(r.RowIndex), True)

            If (CurrentRow.RowType.Equals(DataControlRowType.DataRow)) Then

                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.AutoPostBack = True

                LabelId = DirectCast(CurrentRow.FindControl("LabelId"), Label)
                LabelClientId = DirectCast(CurrentRow.FindControl("LabelClientId"), Label)
                LabelProgram = DirectCast(CurrentRow.FindControl("LabelProgram"), Label)
                LabelReceiver = DirectCast(CurrentRow.FindControl("LabelReceiver"), Label)
                LabelContract = DirectCast(CurrentRow.FindControl("LabelContract"), Label)
                LabelClaimFrequencyTypeCode = DirectCast(CurrentRow.FindControl("LabelClaimFrequencyTypeCode"), Label)

                'CurrentRow.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(GridViewEDISubmissionDetail, "Select$" _
                '                                        & Convert.ToString(e.Row.RowIndex), True) & "; SetClientId('" + LabelClientId.Text + "'); "
                'CurrentRow.Attributes("style") = "cursor:pointer"

                'e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(GridViewEDISubmissionDetail, "SetClientId('" + LabelClientId.Text + "'); Select$" & Convert.ToString(e.Row.RowIndex), True)
                'e.Row.Attributes("style") = "cursor:pointer"

                DropDownListPlaceOfService = DirectCast(CurrentRow.FindControl("DropDownListPlaceOfService"), DropDownList)
                BindPlaceOfServiceDropDownList()

                DropDownListDiagnosisOne = DirectCast(CurrentRow.FindControl("DropDownListDiagnosisOne"), DropDownList)
                BindDropDownListDiagnosisOne()

                DropDownListDiagnosisTwo = DirectCast(CurrentRow.FindControl("DropDownListDiagnosisTwo"), DropDownList)
                BindDropDownListDiagnosisTwo()

                DropDownListDiagnosisThree = DirectCast(CurrentRow.FindControl("DropDownListDiagnosisThree"), DropDownList)
                BindDropDownListDiagnosisThree()

                DropDownListDiagnosisFour = DirectCast(CurrentRow.FindControl("DropDownListDiagnosisFour"), DropDownList)
                BindDropDownListDiagnosisFour()

                DropDownListPlaceOfService.Enabled = InlineAssignHelper(DropDownListDiagnosisOne.Enabled, InlineAssignHelper(DropDownListDiagnosisTwo.Enabled,
                                                     InlineAssignHelper(DropDownListDiagnosisThree.Enabled, InlineAssignHelper(DropDownListDiagnosisFour.Enabled, False))))

                TextBoxStartDate = DirectCast(CurrentRow.FindControl("TextBoxStartDate"), TextBox)
                TextBoxEndDate = DirectCast(CurrentRow.FindControl("TextBoxEndDate"), TextBox)
                TextBoxName = DirectCast(CurrentRow.FindControl("TextBoxName"), TextBox)
                TextBoxAddress = DirectCast(CurrentRow.FindControl("TextBoxAddress"), TextBox)
                TextBoxReferralNumber = DirectCast(CurrentRow.FindControl("TextBoxReferralNumber"), TextBox)
                TextBoxGender = DirectCast(CurrentRow.FindControl("TextBoxGender"), TextBox)
                TextBoxDateOfBirth = DirectCast(CurrentRow.FindControl("TextBoxDateOfBirth"), TextBox)
                TextBoxMedicaidNumber = DirectCast(CurrentRow.FindControl("TextBoxMedicaidNumber"), TextBox)
                TextBoxBillUnits = DirectCast(CurrentRow.FindControl("TextBoxBillUnits"), TextBox)
                TextBoxUnitRate = DirectCast(CurrentRow.FindControl("TextBoxUnitRate"), TextBox)
                TextBoxAmount = DirectCast(CurrentRow.FindControl("TextBoxAmount"), TextBox)
                TextBoxProcedureCode = DirectCast(CurrentRow.FindControl("TextBoxProcedureCode"), TextBox)
                TextBoxModifierOne = DirectCast(CurrentRow.FindControl("TextBoxModifierOne"), TextBox)
                TextBoxModifierTwo = DirectCast(CurrentRow.FindControl("TextBoxModifierTwo"), TextBox)
                TextBoxModifierThree = DirectCast(CurrentRow.FindControl("TextBoxModifierThree"), TextBox)
                TextBoxModifierFour = DirectCast(CurrentRow.FindControl("TextBoxModifierFour"), TextBox)

                TextBoxStartDate.ReadOnly = InlineAssignHelper(TextBoxEndDate.ReadOnly, InlineAssignHelper(TextBoxName.ReadOnly,
                                            InlineAssignHelper(TextBoxAddress.ReadOnly, InlineAssignHelper(TextBoxReferralNumber.ReadOnly,
                                            InlineAssignHelper(TextBoxGender.ReadOnly, InlineAssignHelper(TextBoxDateOfBirth.ReadOnly,
                                            InlineAssignHelper(TextBoxMedicaidNumber.ReadOnly, InlineAssignHelper(TextBoxBillUnits.ReadOnly,
                                            InlineAssignHelper(TextBoxUnitRate.ReadOnly, InlineAssignHelper(TextBoxAmount.ReadOnly,
                                            InlineAssignHelper(TextBoxProcedureCode.ReadOnly, InlineAssignHelper(TextBoxModifierOne.ReadOnly,
                                            InlineAssignHelper(TextBoxModifierTwo.ReadOnly, InlineAssignHelper(TextBoxModifierThree.ReadOnly,
                                            InlineAssignHelper(TextBoxModifierFour.ReadOnly, True)))))))))))))))

                
                CheckBoxSelect.Attributes.Add("onclick", "Javascript:SetClaimsData('" _
                        & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(TextBoxMedicaidNumber.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(LabelClientId.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(TextBoxName.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(TextBoxProcedureCode.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(TextBoxAddress.Text.Trim().Replace("#", "LLL"))), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(TextBoxDateOfBirth.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(TextBoxGender.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(LabelProgram.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(TextBoxStartDate.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(TextBoxEndDate.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(TextBoxBillUnits.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(TextBoxUnitRate.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(TextBoxAmount.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(LabelContract.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(LabelReceiver.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(DropDownListDiagnosisOne.SelectedItem.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(DropDownListDiagnosisTwo.SelectedItem.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(DropDownListDiagnosisThree.SelectedItem.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(DropDownListDiagnosisFour.SelectedItem.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(TextBoxModifierOne.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(TextBoxModifierTwo.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(TextBoxModifierThree.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(TextBoxModifierFour.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(LabelId.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(LabelClaimFrequencyTypeCode.Text.Trim())), Nothing) & "'" _
                        & ",'" & Convert.ToString(Encoder.HtmlEncode(HttpUtility.HtmlEncode(TextBoxReferralNumber.Text.Trim())), Nothing) & "'" _
                        & " );")

                If (Double.TryParse(TextBoxAmount.Text.Trim().Replace("$", ""), DoubleOutResult)) Then
                    TotalBillAmount = TotalBillAmount + DoubleOutResult
                End If
            End If

        End Sub

        Private Sub GridViewEDISubmissionDetail_OnSelectedIndexChanged(sender As Object, e As EventArgs)

            'CurrentRow = DirectCast(e.Row, GridViewRow)

            'CurrentRow = GridViewEDISubmissionDetail.SelectedRow

            'LabelClientId = DirectCast(CurrentRow.FindControl("LabelClientId"), Label)

            'CurrentRow.Attributes.Add("OnClientClick", "SetClientId('" + LabelClientId.Text + "');")

            'Page.ClientScript.RegisterStartupScript(Me.[GetType](), "set", "SetClientId('" + LabelClientId.Text + "');", True)

            'UpdatePanelCaseInfoManage.Attributes.Add("OnClick", "SetCaseInfoId(" & index & ")")

        End Sub

        Private Sub DropDownListPayPeriodStartDate_OnSelectedIndexChanged(sender As Object, e As EventArgs)
            DropDownListPayPeriodEndDate.SelectedIndex = DropDownListPayPeriodStartDate.SelectedIndex
        End Sub

        Private Sub DropDownListUserName_OnSelectedIndexChanged(sender As Object, e As EventArgs)

            GetEDILoginList()

            Dim objEDILoginDataObject As EDILoginDataObject = (From p In EDILoginList Where p.IdNumber = DropDownListUserName.SelectedValue).SingleOrDefault

            TextBoxPassword.Text = If((objEDILoginDataObject Is Nothing), String.Empty, objEDILoginDataObject.Password)
            TextBoxFtpSite.Text = If((objEDILoginDataObject Is Nothing), String.Empty, objEDILoginDataObject.FTPAddress)
            TextBoxDirectory.Text = If((objEDILoginDataObject Is Nothing), String.Empty, objEDILoginDataObject.Directory)

            objEDILoginDataObject = Nothing

        End Sub

        Private Sub ButtonDeleteSubmissionList_Click(sender As Object, e As EventArgs)

            Dim objBLEDISubmission As New BLEDISubmission()

            Try
                objBLEDISubmission.DeleteEDISubmissionList(objShared.ConVisitel)

            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to Delete Submission List. Message: {0}", ex.Message))
            Finally
                objBLEDISubmission = Nothing
            End Try

            BindEDISubmissionGridView()

        End Sub

        Private Sub ButtonRefresh_Click(sender As Object, e As EventArgs)
            SetSystemDate()
            ButtonViewRecords_Click(sender, e)
        End Sub

        Private Sub ButtonIndividualBilling_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Pages/ClientInfoBilling.aspx?ClientId=" & HiddenFieldClientId.Value)
        End Sub

        Private Sub ButtonIndividualDetail_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Pages/ClientInfo.aspx?ClientId=" & HiddenFieldClientId.Value)
        End Sub

        Private Sub ButtonGenerate_Click(sender As Object, e As EventArgs)

            TextBoxSaveLocation.Text = Convert.ToString(ConfigurationManager.AppSettings("LocalDirectory"), Nothing)

            'Handle multi column drop down list

            If (String.IsNullOrEmpty(TextBoxSaveLocation.Text.Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select a File Destination/Name before continuing.")
                Return
            End If

            'Ensure that records are selected to build an EDI file.
            For Each row As GridViewRow In GridViewEDISubmissionDetail.Rows
                If (row.RowType.Equals(DataControlRowType.DataRow)) Then
                    CheckBoxSelect = DirectCast(row.FindControl("CheckBoxSelect"), CheckBox)
                    If (CheckBoxSelect.Checked) Then
                        Exit For
                    End If
                End If
            Next

            If (Not CheckBoxSelect.Checked) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select a date range which returns records before submitting.")
                Return
            End If

            Dim ErrorMesssage As String = String.Empty
            If (GenerateEDIFile(TextBoxSaveLocation.Text.Trim(), CheckBoxResubmission.Checked, ErrorMesssage)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(
                    String.Format("EDI File built successfully.  Please select file {0}  and upload to TMHP site.", TextBoxSaveLocation.Text))
            Else
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(
                    String.Format("The file was produced successfully, but had the following CRITICAL Errors: {0}", ErrorMesssage))
            End If

            'Update Care_Summary with SaveLocation,Update_Date and Update_By

            Dim objBLCareSummary As New BLCareSummary

            Try
                objBLCareSummary.UpdateCareSummaryInfo(objShared.ConVisitel, objShared.UserId, TextBoxSaveLocation.Text.Trim())
            Catch ex As Exception
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(ex.Message)
            Finally
                objBLCareSummary = Nothing
            End Try

        End Sub

        Private Function GenerateEDIFile(FilePath As String, IsResubmission As Boolean, ByRef ErrorMsg As String) As Boolean

            Dim ReturnResult As Boolean = False
            Dim EDIFileResult As New DataSet

            Dim objBLEDISubmission As New BLEDISubmission()

            Try
                ErrorMsg = objBLEDISubmission.GenerateEDIFile(objShared.ConVisitel, EDIFileResult, "1720343500", "", 1, 1, 5, 321)
                ReturnResult = True
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to generate EDI File. {0}", ex.Message))
            Finally
                objBLEDISubmission = Nothing
            End Try

            If (ReturnResult) Then
                'this code segment write data to file.
                Dim EDIFileName As String = DateTime.Now.ToString(ConfigurationManager.AppSettings("EDIFileDateFormat"))

                Dim objFileStream As New FileStream(String.Format("{0}{1}{2}", FilePath, EDIFileName, ".txt"), FileMode.OpenOrCreate, FileAccess.Write)
                Dim objStreamWriter As New StreamWriter(objFileStream)

                For Each table As DataTable In EDIFileResult.Tables
                    Select Case table.TableName
                        Case "Table"
                            For Each dRow As DataRow In table.Rows
                                objStreamWriter.Write(Convert.ToString(dRow("InterchangeControlHeader"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("gTMHP"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("strREF02_TRANS_TYPE"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("FunctionalGroupHeader"), Nothing))
                            Next
                            Exit Select

                        Case "Table1"
                            For Each dRow As DataRow In table.Rows
                                objStreamWriter.Write(Convert.ToString(dRow("TransactionSetHeader"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("BeginningOfHierarchicalTransaction"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("SubmitterName"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("SubmitterEDIContactInformation"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("ReceiverName"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("Billing_Pay_To_Provider_HL"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("Billing_Pay_To_Provider_Specialty_Info"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("BillProvName"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("BillProvAddress"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("BillProv_City_State_Zip"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("BillProvSecondaryID"), Nothing))
                            Next
                            Exit Select

                        Case "Table2"
                            For Each dRow As DataRow In table.Rows
                                objStreamWriter.Write(Convert.ToString(dRow("SubscriberHierarchicalLevel"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("SubscriberInformation"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("SubscriberName"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("SubscriberAddress"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("Subscriber_City_State_Zip"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("SubscriberDemographicInfo"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("PayerName"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("PayerAddress_1"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("PayerAddress_2"), Nothing))
                            Next
                            Exit Select

                        Case "Table3"
                            For Each dRow As DataRow In table.Rows
                                objStreamWriter.Write(Convert.ToString(dRow("ClaimInformation"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("ReferralNumber"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("DiagnosisCodes"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("ReferringProviderInfo"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("RenderingProviderInfo"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("RenderingProviderSpecialtyInfo"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("ServiceLine"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("ProfessionalService"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("ServiceDate"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("ReferenceIdentification"), Nothing))
                            Next
                            Exit Select

                        Case "Table4"
                            For Each dRow As DataRow In table.Rows
                                objStreamWriter.Write(Convert.ToString(dRow("TransactionSetTrailer"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("FunctionalGroupTrailer"), Nothing))
                                objStreamWriter.Write(Convert.ToString(dRow("InterchangeControlTrailer"), Nothing))
                            Next
                            Exit Select

                    End Select
                Next
                objStreamWriter.Close()
                objStreamWriter = Nothing
                objFileStream = Nothing

                TextBoxSaveLocation.Text = String.Format("{0}{1}{2}", TextBoxSaveLocation.Text.Trim(), EDIFileName, ".txt")

            End If

            EDIFileResult = Nothing

            Return ReturnResult

        End Function

        Private Sub ButtonViewRecords_Click(sender As Object, e As EventArgs)

            MultiSelect()

            Dim objBLEDISubmission As New BLEDISubmission()

            Try

                objBLEDISubmission.ProcessEDIViewRecords(objShared.ConVisitel,
                       If(DropDownListPayPeriodStartDate.SelectedValue.Equals("-1"), String.Empty, DropDownListPayPeriodStartDate.SelectedValue),
                       If(DropDownListPayPeriodEndDate.SelectedValue.Equals("-1"), String.Empty, DropDownListPayPeriodEndDate.SelectedValue),
                       If(DropDownListContractNumber.SelectedValue.Equals("-1"), String.Empty, DropDownListContractNumber.SelectedValue),
                            SelectedClientIdList)

            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to Process EDI View Records. Message: {0}", ex.Message))
            Finally
                BindEDISubmissionGridView()
                objBLEDISubmission = Nothing
            End Try

        End Sub

        Private Sub ButtonPopulateListBox_Click(sender As Object, e As EventArgs)

            If ((DropDownListPayPeriodStartDate.SelectedValue.Equals("-1")) Or (DropDownListPayPeriodEndDate.SelectedValue.Equals("-1")) _
                Or (DropDownListContractNumber.SelectedValue.Equals("-1"))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("To run this process you must enter contract number and pay periods")
                Return
            End If

            Dim objBLEDISubmission As New BLEDISubmission
            objBLEDISubmission.SelectFTPClientName(objShared.VisitelConnectionString, SqlDataSourceNamesList, DropDownListPayPeriodStartDate.SelectedValue,
                                                   DropDownListPayPeriodEndDate.SelectedValue, DropDownListContractNumber.SelectedValue)
            objBLEDISubmission = Nothing

            ListBoxNamesList.Items.Clear()

            ListBoxNamesList.DataSourceID = "SqlDataSourceNamesList"
            ListBoxNamesList.DataTextField = "Individual"
            ListBoxNamesList.DataValueField = "ClientId"
            ListBoxNamesList.DataBind()

        End Sub

        Private Sub ButtonClearList_Click(sender As Object, e As EventArgs)
            ClearNamesList()
        End Sub

        Private Sub ButtonFtpSend_Click(sender As Object, e As EventArgs)

            Dim LocalDirectoryName As String = String.Empty, LocalFileName As String = String.Empty

            LocalDirectoryName = New FileInfo(TextBoxSaveLocation.Text.Trim()).DirectoryName
            LocalFileName = Path.GetFileName(TextBoxSaveLocation.Text.Trim())

            If ((String.IsNullOrEmpty(LocalDirectoryName)) Or (String.IsNullOrEmpty(LocalFileName))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("You have not populated the destination box with the file you are transmiting.")
                Return
            End If

            If (DropDownListUserName.SelectedValue.Equals("-1")) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select your username")
                Return
            End If

            If (String.IsNullOrEmpty(TextBoxPassword.Text.Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please enter your password")
                Return
            End If

            If (String.IsNullOrEmpty(TextBoxFtpSite.Text.Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please enter the ftp site you are transmiting to")
                Return
            End If

            If (String.IsNullOrEmpty(TextBoxDirectory.Text.Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please enter the ftp directory you are transmiting to")
                Return
            End If

            Dim SFTPRequestsParam As New SFTPRequestsParam

            SFTPRequestsParam.FTPOperationType = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.FTPOperationType.UploadFile)
            SFTPRequestsParam.LocalPath = LocalDirectoryName
            SFTPRequestsParam.LocalFileName = LocalFileName
            SFTPRequestsParam.RemotePath = TextBoxDirectory.Text.Trim()
            SFTPRequestsParam.RemoteFileName = SFTPRequestsParam.LocalFileName

            If (SFTPOperations(TextBoxFtpSite.Text.Trim(), DropDownListUserName.SelectedItem.Text.Trim(), TextBoxPassword.Text.Trim(), SFTPRequestsParam)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("EDI File sent. Please verify that file has been received")
            End If

            SFTPRequestsParam = Nothing

            DropDownListUserName.SelectedIndex = 0
            TextBoxPassword.Text = String.Empty
            TextBoxFtpSite.Text = String.Empty
            TextBoxDirectory.Text = String.Empty

            TextBoxSaveLocation.Text = Convert.ToString(ConfigurationManager.AppSettings("LocalDirectory"), Nothing)

        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()

            CultureInfo = objShared.GetCultureInfo()

            LabelResubmission.Visible = False
            CheckBoxResubmission.Visible = False
            CheckBoxResubmission.Checked = False

            TextBoxTotalBillAmount.ReadOnly = True
            TextBoxSystemDate.ReadOnly = True
            SetSystemDate()

            ListBoxNamesList.SelectionMode = ListSelectionMode.Multiple

            AddHandler ButtonViewRecords.Click, AddressOf ButtonViewRecords_Click
            AddHandler ButtonPopulateListBox.Click, AddressOf ButtonPopulateListBox_Click
            AddHandler ButtonClearList.Click, AddressOf ButtonClearList_Click
            AddHandler ButtonFtpSend.Click, AddressOf ButtonFtpSend_Click
            AddHandler ButtonRefresh.Click, AddressOf ButtonRefresh_Click
            AddHandler ButtonDeleteSubmissionList.Click, AddressOf ButtonDeleteSubmissionList_Click

            AddHandler ButtonIndividualBilling.Click, AddressOf ButtonIndividualBilling_Click
            AddHandler ButtonIndividualDetail.Click, AddressOf ButtonIndividualDetail_Click
            AddHandler ButtonGenerate.Click, AddressOf ButtonGenerate_Click

            DropDownListPayPeriodStartDate.AutoPostBack = True
            AddHandler DropDownListPayPeriodStartDate.SelectedIndexChanged, AddressOf DropDownListPayPeriodStartDate_OnSelectedIndexChanged


            GridViewEDISubmissionDetail.AutoGenerateColumns = False
            GridViewEDISubmissionDetail.Width = Unit.Percentage(100)
            GridViewEDISubmissionDetail.ShowHeaderWhenEmpty = True
            GridViewEDISubmissionDetail.AllowPaging = True
            GridViewEDISubmissionDetail.AllowSorting = True

            GridViewEDISubmissionDetail.ShowFooter = True

            If (GridViewEDISubmissionDetail.AllowPaging) Then
                GridViewEDISubmissionDetail.PageSize = objShared.GridViewDefaultPageSize
            End If


            AddHandler GridViewEDISubmissionDetail.RowDataBound, AddressOf GridViewEDISubmissionDetail_RowDataBound
            'AddHandler GridViewEDISubmissionDetail.SelectedIndexChanged, AddressOf GridViewEDISubmissionDetail_OnSelectedIndexChanged

            DropDownListUserName.AutoPostBack = True
            AddHandler DropDownListUserName.SelectedIndexChanged, AddressOf DropDownListUserName_OnSelectedIndexChanged

            ButtonCorrectClaim.Attributes.Add("OnClick", "EDICorrectedClaims(); return false;")
            ButtonCorrectClaim.UseSubmitBehavior = False
            ButtonCorrectClaim.ClientIDMode = UI.ClientIDMode.Static

            ButtonSubmissionInfo.Attributes.Add("OnClick", "EDILoginInfo(); return false;")
            ButtonSubmissionInfo.UseSubmitBehavior = False
            ButtonSubmissionInfo.ClientIDMode = UI.ClientIDMode.Static

            ButtonViewRecords.ClientIDMode = UI.ClientIDMode.Static
            ButtonFtpSend.ClientIDMode = UI.ClientIDMode.Static
            ButtonPopulateListBox.ClientIDMode = UI.ClientIDMode.Static
            ButtonClearList.ClientIDMode = UI.ClientIDMode.Static
            ButtonRefresh.ClientIDMode = UI.ClientIDMode.Static
            ButtonDeleteSubmissionList.ClientIDMode = UI.ClientIDMode.Static
            ButtonGenerate.ClientIDMode = UI.ClientIDMode.Static

            HyperLinkTexMedConnect.NavigateUrl = Convert.ToString(ConfigurationManager.AppSettings("TexMedConnectLink"), Nothing)

        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EDISubmission", ControlName & Convert.ToString(".resx"))

            LabelEDISubmission.Text = Convert.ToString(ResourceTable("LabelEDISubmission"), Nothing)
            LabelEDISubmission.Text = If(String.IsNullOrEmpty(LabelEDISubmission.Text), "EDI Submission Form", LabelEDISubmission.Text)

            LabelViewRecords.Text = Convert.ToString(ResourceTable("LabelViewRecords"), Nothing)
            LabelViewRecords.Text = If(String.IsNullOrEmpty(LabelViewRecords.Text), "View Records", LabelViewRecords.Text)

            LabelPayPeriodStartDate.Text = Convert.ToString(ResourceTable("LabelPayPeriodStartDate"), Nothing)
            LabelPayPeriodStartDate.Text = If(String.IsNullOrEmpty(LabelPayPeriodStartDate.Text), "Start Date(>=)", LabelPayPeriodStartDate.Text)

            LabelPayPeriodEndDate.Text = Convert.ToString(ResourceTable("LabelPayPeriodEndDate"), Nothing)
            LabelPayPeriodEndDate.Text = If(String.IsNullOrEmpty(LabelPayPeriodEndDate.Text), "End Date (<=)", LabelPayPeriodEndDate.Text)

            LabelPayPeriod.Text = Convert.ToString(ResourceTable("LabelPayPeriod"), Nothing)
            LabelPayPeriod.Text = If(String.IsNullOrEmpty(LabelPayPeriod.Text), "Pay Period:", LabelPayPeriod.Text)

            LabelContractNumber.Text = Convert.ToString(ResourceTable("LabelContractNumber"), Nothing)
            LabelContractNumber.Text = If(String.IsNullOrEmpty(LabelContractNumber.Text), "Contract No:", LabelContractNumber.Text)

            ButtonViewRecords.Text = Convert.ToString(ResourceTable("ButtonViewRecords"), Nothing)
            ButtonViewRecords.Text = If(String.IsNullOrEmpty(ButtonViewRecords.Text), "View Records", ButtonViewRecords.Text)

            LabelFTPSend.Text = Convert.ToString(ResourceTable("LabelFTPSend"), Nothing)
            LabelFTPSend.Text = If(String.IsNullOrEmpty(LabelFTPSend.Text), "FTP Send", LabelFTPSend.Text)

            LabelUserName.Text = Convert.ToString(ResourceTable("LabelUserName"), Nothing)
            LabelUserName.Text = If(String.IsNullOrEmpty(LabelUserName.Text), "User Name:", LabelUserName.Text)

            ButtonFtpSend.Text = Convert.ToString(ResourceTable("ButtonFtpSend"), Nothing)
            ButtonFtpSend.Text = If(String.IsNullOrEmpty(ButtonFtpSend.Text), "FTP Send", ButtonFtpSend.Text)

            LabelPassword.Text = Convert.ToString(ResourceTable("LabelPassword"), Nothing)
            LabelPassword.Text = If(String.IsNullOrEmpty(LabelPassword.Text), "Password:", LabelPassword.Text)

            LabelFtpSite.Text = Convert.ToString(ResourceTable("LabelFtpSite"), Nothing)
            LabelFtpSite.Text = If(String.IsNullOrEmpty(LabelFtpSite.Text), "Ftp Site:", LabelFtpSite.Text)

            LabelDirectory.Text = Convert.ToString(ResourceTable("LabelDirectory"), Nothing)
            LabelDirectory.Text = If(String.IsNullOrEmpty(LabelDirectory.Text), "Directory:", LabelDirectory.Text)

            LabelComment.Text = Convert.ToString(ResourceTable("LabelComment"), Nothing)
            LabelComment.Text = If(String.IsNullOrEmpty(LabelComment.Text),
                                   "Click Choose File button to select a save location and then click ''Generate'' below to create an EDI file.", LabelComment.Text)

            ButtonCorrectClaim.Text = Convert.ToString(ResourceTable("ButtonCorrectClaim"), Nothing)
            ButtonCorrectClaim.Text = If(String.IsNullOrEmpty(ButtonCorrectClaim.Text), "Correct/Void Claim", ButtonCorrectClaim.Text)

            ButtonRefresh.Text = Convert.ToString(ResourceTable("ButtonRefresh"), Nothing)
            ButtonRefresh.Text = If(String.IsNullOrEmpty(ButtonRefresh.Text), "Refresh", ButtonRefresh.Text)

            ButtonDeleteSubmissionList.Text = Convert.ToString(ResourceTable("ButtonDeleteSubmissionList"), Nothing)
            ButtonDeleteSubmissionList.Text = If(String.IsNullOrEmpty(ButtonDeleteSubmissionList.Text), "Delete Submission List", ButtonDeleteSubmissionList.Text)

            ButtonGenerate.Text = Convert.ToString(ResourceTable("ButtonGenerate"), Nothing)
            ButtonGenerate.Text = If(String.IsNullOrEmpty(ButtonGenerate.Text), "Generate", ButtonGenerate.Text)

            ButtonSubmissionReport.Text = Convert.ToString(ResourceTable("ButtonSubmissionReport"), Nothing)
            ButtonSubmissionReport.Text = If(String.IsNullOrEmpty(ButtonSubmissionReport.Text), "Submission Report", ButtonSubmissionReport.Text)

            ButtonIndividualBilling.Text = Convert.ToString(ResourceTable("ButtonIndividualBilling"), Nothing)
            ButtonIndividualBilling.Text = If(String.IsNullOrEmpty(ButtonIndividualBilling.Text), "Individual Billing", ButtonIndividualBilling.Text)

            ButtonIndividualDetail.Text = Convert.ToString(ResourceTable("ButtonIndividualDetail"), Nothing)
            ButtonIndividualDetail.Text = If(String.IsNullOrEmpty(ButtonIndividualDetail.Text), "Individual Detail", ButtonIndividualDetail.Text)

            ButtonSubmissionInfo.Text = Convert.ToString(ResourceTable("ButtonSubmissionInfo"), Nothing)
            ButtonSubmissionInfo.Text = If(String.IsNullOrEmpty(ButtonSubmissionInfo.Text), "Submission Info", ButtonSubmissionInfo.Text)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' Reading GridView header text from resource file
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <remarks></remarks>
        Private Sub SetGridViewColumnHeaderText(ByRef CurrentRow As GridViewRow)

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EDISubmission", ControlName & Convert.ToString(".resx"))

            Dim LabelHeaderSerial As Label = DirectCast(CurrentRow.FindControl("LabelHeaderSerial"), Label)
            LabelHeaderSerial.Text = Convert.ToString(ResourceTable("LabelHeaderSerial"), Nothing).Trim()
            LabelHeaderSerial.Text = If(String.IsNullOrEmpty(LabelHeaderSerial.Text), "SI.", LabelHeaderSerial.Text)

            Dim LabelHeaderSelect As Label = DirectCast(CurrentRow.FindControl("LabelHeaderSelect"), Label)
            LabelHeaderSelect.Text = Convert.ToString(ResourceTable("LabelHeaderSelect"), Nothing).Trim()
            LabelHeaderSelect.Text = If(String.IsNullOrEmpty(LabelHeaderSelect.Text), "Select", LabelHeaderSelect.Text)

            Dim ButtonHeaderStartDate As Button = DirectCast(CurrentRow.FindControl("ButtonHeaderStartDate"), Button)
            ButtonHeaderStartDate.Text = Convert.ToString(ResourceTable("ButtonHeaderStartDate"), Nothing).Trim()
            ButtonHeaderStartDate.Text = If(String.IsNullOrEmpty(ButtonHeaderStartDate.Text), "Start Date", ButtonHeaderStartDate.Text)

            Dim ButtonHeaderEndDate As Button = DirectCast(CurrentRow.FindControl("ButtonHeaderEndDate"), Button)
            ButtonHeaderEndDate.Text = Convert.ToString(ResourceTable("ButtonHeaderEndDate"), Nothing).Trim()
            ButtonHeaderEndDate.Text = If(String.IsNullOrEmpty(ButtonHeaderEndDate.Text), "End Date", ButtonHeaderEndDate.Text)

            Dim ButtonHeaderName As Button = DirectCast(CurrentRow.FindControl("ButtonHeaderName"), Button)
            ButtonHeaderName.Text = Convert.ToString(ResourceTable("ButtonHeaderName"), Nothing).Trim()
            ButtonHeaderName.Text = If(String.IsNullOrEmpty(ButtonHeaderName.Text), "Name", ButtonHeaderName.Text)

            Dim ButtonHeaderAddress As Button = DirectCast(CurrentRow.FindControl("ButtonHeaderAddress"), Button)
            ButtonHeaderAddress.Text = Convert.ToString(ResourceTable("ButtonHeaderAddress"), Nothing).Trim()
            ButtonHeaderAddress.Text = If(String.IsNullOrEmpty(ButtonHeaderAddress.Text), "Address", ButtonHeaderAddress.Text)

            Dim LabelHeaderReferralNumber As Label = DirectCast(CurrentRow.FindControl("LabelHeaderReferralNumber"), Label)
            LabelHeaderReferralNumber.Text = Convert.ToString(ResourceTable("LabelHeaderReferralNumber"), Nothing).Trim()
            LabelHeaderReferralNumber.Text = If(String.IsNullOrEmpty(LabelHeaderReferralNumber.Text), "Referral No", LabelHeaderReferralNumber.Text)

            Dim LabelHeaderGender As Label = DirectCast(CurrentRow.FindControl("LabelHeaderGender"), Label)
            LabelHeaderGender.Text = Convert.ToString(ResourceTable("LabelHeaderGender"), Nothing).Trim()
            LabelHeaderGender.Text = If(String.IsNullOrEmpty(LabelHeaderGender.Text), "Gender", LabelHeaderGender.Text)

            Dim LabelHeaderDateOfBirth As Label = DirectCast(CurrentRow.FindControl("LabelHeaderDateOfBirth"), Label)
            LabelHeaderDateOfBirth.Text = Convert.ToString(ResourceTable("LabelHeaderDateOfBirth"), Nothing).Trim()
            LabelHeaderDateOfBirth.Text = If(String.IsNullOrEmpty(LabelHeaderDateOfBirth.Text), "DOB", LabelHeaderDateOfBirth.Text)

            Dim LabelHeaderMedicaidNumber As Label = DirectCast(CurrentRow.FindControl("LabelHeaderMedicaidNumber"), Label)
            LabelHeaderMedicaidNumber.Text = Convert.ToString(ResourceTable("LabelHeaderMedicaidNumber"), Nothing).Trim()
            LabelHeaderMedicaidNumber.Text = If(String.IsNullOrEmpty(LabelHeaderMedicaidNumber.Text), "Medicaid#", LabelHeaderMedicaidNumber.Text)

            Dim LabelHeaderBillUnits As Label = DirectCast(CurrentRow.FindControl("LabelHeaderBillUnits"), Label)
            LabelHeaderBillUnits.Text = Convert.ToString(ResourceTable("LabelHeaderBillUnits"), Nothing).Trim()
            LabelHeaderBillUnits.Text = If(String.IsNullOrEmpty(LabelHeaderBillUnits.Text), "Bill Units", LabelHeaderBillUnits.Text)

            Dim LabelHeaderUnitRate As Label = DirectCast(CurrentRow.FindControl("LabelHeaderUnitRate"), Label)
            LabelHeaderUnitRate.Text = Convert.ToString(ResourceTable("LabelHeaderUnitRate"), Nothing).Trim()
            LabelHeaderUnitRate.Text = If(String.IsNullOrEmpty(LabelHeaderUnitRate.Text), "Unit Rate", LabelHeaderUnitRate.Text)

            Dim LabelHeaderAmount As Label = DirectCast(CurrentRow.FindControl("LabelHeaderAmount"), Label)
            LabelHeaderAmount.Text = Convert.ToString(ResourceTable("LabelHeaderAmount"), Nothing).Trim()
            LabelHeaderAmount.Text = If(String.IsNullOrEmpty(LabelHeaderAmount.Text), "Amount", LabelHeaderAmount.Text)

            Dim LabelHeaderPlaceOfService As Label = DirectCast(CurrentRow.FindControl("LabelHeaderPlaceOfService"), Label)
            LabelHeaderPlaceOfService.Text = Convert.ToString(ResourceTable("LabelHeaderPlaceOfService"), Nothing).Trim()
            LabelHeaderPlaceOfService.Text = If(String.IsNullOrEmpty(LabelHeaderPlaceOfService.Text), "Place Of Service", LabelHeaderPlaceOfService.Text)

            Dim LabelHeaderProcedureCode As Label = DirectCast(CurrentRow.FindControl("LabelHeaderProcedureCode"), Label)
            LabelHeaderProcedureCode.Text = Convert.ToString(ResourceTable("LabelHeaderProcedureCode"), Nothing).Trim()
            LabelHeaderProcedureCode.Text = If(String.IsNullOrEmpty(LabelHeaderProcedureCode.Text), "Procedure Code", LabelHeaderProcedureCode.Text)

            Dim LabelHeaderModifiers As Label = DirectCast(CurrentRow.FindControl("LabelHeaderModifiers"), Label)
            LabelHeaderModifiers.Text = Convert.ToString(ResourceTable("LabelHeaderModifiers"), Nothing).Trim()
            LabelHeaderModifiers.Text = If(String.IsNullOrEmpty(LabelHeaderModifiers.Text), "Modifiers", LabelHeaderModifiers.Text)

            Dim LabelHeaderDiagnosis As Label = DirectCast(CurrentRow.FindControl("LabelHeaderDiagnosis"), Label)
            LabelHeaderDiagnosis.Text = Convert.ToString(ResourceTable("LabelHeaderDiagnosis"), Nothing).Trim()
            LabelHeaderDiagnosis.Text = If(String.IsNullOrEmpty(LabelHeaderDiagnosis.Text), "Diagnosis", LabelHeaderDiagnosis.Text)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' Importing Javascript
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadJScript()

            LoadJS("JavaScript/jquery.blockUI.js")

            Dim scriptBlock As String = String.Empty
            scriptBlock = "<script type='text/javascript'> " _
                            & " var CompanyId = " & objShared.CompanyId & "; " _
                            & " var MedicaidNumber =''; " _
                            & " var ClientId =''; " _
                            & " var IndividualName =''; " _
                            & " var ProcedureCode =''; " _
                            & " var Address =''; " _
                            & " var DateOfBirth =''; " _
                            & " var Gender =''; " _
                            & " var Program =''; " _
                            & " var StartDate =''; " _
                            & " var EndDate =''; " _
                            & " var BilledHours =''; " _
                            & " var UnitRate =''; " _
                            & " var BilledAmount =''; " _
                            & " var ContractNumber =''; " _
                            & " var Receiver =''; " _
                            & " var DiagnosisCodeOne =''; " _
                            & " var DiagnosisCodeTwo =''; " _
                            & " var DiagnosisCodeThree =''; " _
                            & " var DiagnosisCodeFour =''; " _
                            & " var ModifierOne =''; " _
                            & " var ModifierTwo =''; " _
                            & " var ModifierThree =''; " _
                            & " var ModifierFour =''; " _
                            & " var EDIId =''; " _
                            & " var ClaimFrequencyTypeCode =''; " _
                            & " var ClaimNumber =''; " _
                            & " var EDICorrectedClaimsPath ='" & objShared.GetPopupUrl("Pages/Popup/EDICorrectedClaimsPopup.aspx") & "'; " _
                            & " var EDIMissingDataPath ='" & objShared.GetPopupUrl("Pages/Popup/EDIMissingDataPopup.aspx") & "'; " _
                            & " var EDISubmissionInfoPath ='" & objShared.GetPopupUrl("Pages/Popup/EDILoginInfoPopup.aspx") & "'; " _
                            & " var LoaderImagePath ='" & objShared.GetLoaderImagePath & "'; " _
                            & " var ReportsViewPath ='" & objShared.GetPopupUrl("Reports/ReportsView.aspx") & "'; " _
                            & " var MissingData ='" & MissingData & "'; " _
                            & " var FtpTargetButton ='ButtonFtpSend'; " _
                            & " var FtpDialogHeader ='Cisco VPN Client'; " _
                            & " var FtpDialogConfirmMsg ='If transmiting to TMHP please make sure you have logged into Cisco VPN Client'; " _
                            & " var CustomTargetButton ='ButtonGenerate'; " _
                            & " var CustomDialogHeader ='TurboPAS'; " _
                            & " var CustomDialogConfirmMsg ='You must first print and review the EDI Submission Report. Have you done this?'; " _
                            & " var prm =''; " _
                            & " jQuery(document).ready(function () {" _
                            & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                            & "     prm.add_initializeRequest(InitializeRequest); " _
                            & "     prm.add_endRequest(EndRequest); " _
                            & "     FtpSend();" _
                            & "     EDIGeneration();" _
                            & "     prm.add_endRequest(FtpSend); " _
                            & "     prm.add_endRequest(EDIGeneration); " _
                            & "}); " _
                    & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/EDISubmission/" & ControlName & ".js")

        End Sub

        Private Sub GetData()

            TextBoxSaveLocation.Text = Convert.ToString(ConfigurationManager.AppSettings("LocalDirectory"), Nothing)

            BindPayPeriodStartEndDateDropDownList()
            BindContractDropDownList()
            BindUserNameDropDownList()
            BindEDISubmissionGridView()
            GetPlaceOfServiceData()

        End Sub

        Private Sub GetEDILoginList()

            Dim objBLEDISubmission As New BLEDISubmission
            EDILoginList = objBLEDISubmission.SelectEDILogin(objShared.ConVisitel)
            objBLEDISubmission = Nothing

        End Sub

        Private Sub SetSystemDate()
            TextBoxSystemDate.Text = DateTime.Today.ToString()
            TextBoxSystemDate.Text = Convert.ToDateTime(TextBoxSystemDate.Text, CultureInfo).ToString(objShared.DateFormat)
        End Sub

        Private Sub MultiSelect()

            Dim SelectedCount As Integer = 0
            If (ListBoxNamesList.Items.Count > 0) Then
                For Each li In ListBoxNamesList.Items
                    If (li.Selected) Then
                        SelectedClientIdList = If((SelectedCount = 0), SelectedClientIdList & li.Value, SelectedClientIdList & "," & li.Value)
                        SelectedCount += 1
                    End If
                Next
                If (SelectedCount = 0) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Nothing was selected from the list")
                End If
            End If

        End Sub

        ''' <summary>
        ''' Un-used
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub FTPUpload()
            'FTP Server URL.
            Dim ftp As String = "ftp://yourserver.com/"

            'FTP Folder name. Leave blank if you want to upload to root folder.
            Dim ftpFolder As String = "Uploads/"

            Dim fileBytes As Byte() = Nothing

            'Read the FileName and convert it to Byte array.
            Dim fileName As String = String.Empty
            'Dim fileName As String = Path.GetFileName(FileUpload1.FileName)
            'Using fileStream As New StreamReader(FileUpload1.PostedFile.InputStream)
            '    fileBytes = Encoding.UTF8.GetBytes(fileStream.ReadToEnd())
            '    fileStream.Close()
            'End Using

            Try
                'Create FTP Request.
                Dim request As FtpWebRequest = DirectCast(WebRequest.Create(ftp & ftpFolder & fileName), FtpWebRequest)
                request.Method = WebRequestMethods.Ftp.UploadFile

                'Enter FTP Server credentials.
                request.Credentials = New NetworkCredential("UserName", "Password")
                request.ContentLength = fileBytes.Length
                request.UsePassive = True
                request.UseBinary = True
                request.ServicePoint.ConnectionLimit = fileBytes.Length
                request.EnableSsl = False

                Using requestStream As Stream = request.GetRequestStream()
                    requestStream.Write(fileBytes, 0, fileBytes.Length)
                    requestStream.Close()
                End Using

                Dim response As FtpWebResponse = DirectCast(request.GetResponse(), FtpWebResponse)


                response.Close()
            Catch ex As WebException
                Throw New Exception(TryCast(ex.Response, FtpWebResponse).StatusDescription)
            End Try
        End Sub

        Private Sub ClearNamesList()
            For Each li In ListBoxNamesList.Items
                li.Selected = False
            Next
        End Sub

        Private Sub BindUserNameDropDownList()

            GetEDILoginList()

            DropDownListUserName.DataSource = EDILoginList
            DropDownListUserName.DataTextField = "Name"
            DropDownListUserName.DataValueField = "IdNumber"
            DropDownListUserName.DataBind()

            DropDownListUserName.Items.Insert(0, New ListItem(" ", "-1"))

        End Sub

        Private Sub GetDiagnosisData()

            Dim objBLClientInfo As New BLClientInfo
            objBLClientInfo.GetDiagnosis(objShared.ConVisitel, objShared.CompanyId, DiagnosisCodeList)
            objBLClientInfo = Nothing

            DiagnosisCodeList = DiagnosisCodeList.OrderBy(Function(x) x.DiagnosisCode).ToList()
        End Sub

        Private Sub BindDropDownListDiagnosisOne()

            DropDownListDiagnosisOne.DataSource = DiagnosisCodeList
            DropDownListDiagnosisOne.DataTextField = "DiagnosisCode"
            DropDownListDiagnosisOne.DataValueField = "DiagnosisId"
            DropDownListDiagnosisOne.DataBind()

            DropDownListDiagnosisOne.Items.Insert(0, New ListItem("Please Select...", "-1"))

            DropDownListDiagnosisOne.SelectedIndex = DropDownListDiagnosisOne.Items.IndexOf(DropDownListDiagnosisOne.Items.FindByText(
                                                   ((From p In EDISubmissionList Where p.Id = Convert.ToInt64(LabelId.Text)).SingleOrDefault).DiagnosisCodeOne))



        End Sub

        Private Sub BindDropDownListDiagnosisTwo()

            DropDownListDiagnosisTwo.DataSource = DiagnosisCodeList
            DropDownListDiagnosisTwo.DataTextField = "DiagnosisCode"
            DropDownListDiagnosisTwo.DataValueField = "DiagnosisId"
            DropDownListDiagnosisTwo.DataBind()

            DropDownListDiagnosisTwo.Items.Insert(0, New ListItem("Please Select...", "-1"))

            DropDownListDiagnosisTwo.SelectedIndex = DropDownListDiagnosisTwo.Items.IndexOf(DropDownListDiagnosisTwo.Items.FindByText(
                                                   ((From p In EDISubmissionList Where p.Id = Convert.ToInt64(LabelId.Text)).SingleOrDefault).DiagnosisCodeTwo))


        End Sub

        Private Sub BindDropDownListDiagnosisThree()

            DropDownListDiagnosisThree.DataSource = DiagnosisCodeList
            DropDownListDiagnosisThree.DataTextField = "DiagnosisCode"
            DropDownListDiagnosisThree.DataValueField = "DiagnosisId"
            DropDownListDiagnosisThree.DataBind()

            DropDownListDiagnosisThree.Items.Insert(0, New ListItem("Please Select...", "-1"))

            DropDownListDiagnosisThree.SelectedIndex = DropDownListDiagnosisThree.Items.IndexOf(DropDownListDiagnosisThree.Items.FindByText(
                                                   ((From p In EDISubmissionList Where p.Id = Convert.ToInt64(LabelId.Text)).SingleOrDefault).DiagnosisCodeThree))


        End Sub

        Private Sub BindDropDownListDiagnosisFour()

            DropDownListDiagnosisFour.DataSource = DiagnosisCodeList
            DropDownListDiagnosisFour.DataTextField = "DiagnosisCode"
            DropDownListDiagnosisFour.DataValueField = "DiagnosisId"
            DropDownListDiagnosisFour.DataBind()

            DropDownListDiagnosisFour.Items.Insert(0, New ListItem("Please Select...", "-1"))

            DropDownListDiagnosisFour.SelectedIndex = DropDownListDiagnosisFour.Items.IndexOf(DropDownListDiagnosisFour.Items.FindByText(
                                                  ((From p In EDISubmissionList Where p.Id = Convert.ToInt64(LabelId.Text)).SingleOrDefault).DiagnosisCodeFour))


        End Sub

        Private Sub GetPlaceOfServiceData()

            Dim objBLClientInfo As New BLClientInfo
            objBLClientInfo.GetPlaceOfServiceData(objShared.VisitelConnectionString, SqlDataSourceDropDownListPlaceOfService)
            objBLClientInfo = Nothing

        End Sub

        ''' <summary>
        ''' Binding Place Of Service Drop Down List
        ''' </summary>
        Private Sub BindPlaceOfServiceDropDownList()

            DropDownListPlaceOfService.DataSourceID = "SqlDataSourceDropDownListPlaceOfService"
            DropDownListPlaceOfService.DataTextField = "Description"
            DropDownListPlaceOfService.DataValueField = "pos_id"
            DropDownListPlaceOfService.DataBind()

            DropDownListPlaceOfService.Items.Insert(0, New ListItem("", "-1"))

        End Sub

        Private Sub GetPayPeriods()
            Dim objBLPayPeriodDetail As New BLPayPeriodDetail
            PayPeriods = objBLPayPeriodDetail.GetPayPeriodData(objShared.ConVisitel, EnumDataObject.EnumHelper.GetDescription(EnumDataObject.PayPeriodFor.EDISubmission))
            PayPeriods = PayPeriods.OrderByDescending(Function(x) x.StartDate).ToList()
            objBLPayPeriodDetail = Nothing
        End Sub

        Private Sub BindPayPeriodStartEndDateDropDownList()

            GetPayPeriods()

            DropDownListPayPeriodStartDate.DataSource = PayPeriods
            DropDownListPayPeriodStartDate.DataTextField = "StartDate"
            DropDownListPayPeriodStartDate.DataValueField = "StartDate"
            DropDownListPayPeriodStartDate.DataBind()

            DropDownListPayPeriodStartDate.Items.Insert(0, New ListItem("", "-1"))

            DropDownListPayPeriodEndDate.DataSource = PayPeriods
            DropDownListPayPeriodEndDate.DataTextField = "EndDate"
            DropDownListPayPeriodEndDate.DataValueField = "EndDate"
            DropDownListPayPeriodEndDate.DataBind()

            DropDownListPayPeriodEndDate.Items.Insert(0, New ListItem("", "-1"))

            PayPeriods = Nothing

        End Sub

        Private Sub BindContractDropDownList()

            Dim objBLEDICentral As New BLEDICentral

            Try

                objBLEDICentral.SelectContractInfo(objShared.VisitelConnectionString, SqlDataSourceContractInfo)

            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to fetch contract information")
            Finally
                objBLEDICentral = Nothing
            End Try

            DropDownListContractNumber.DataSourceID = "SqlDataSourceContractInfo"
            DropDownListContractNumber.DataTextField = "ContractNo"
            DropDownListContractNumber.DataValueField = "ContractNo"
            DropDownListContractNumber.DataBind()

            DropDownListContractNumber.Items.Insert(0, New ListItem(" ", "-1"))


        End Sub

        ''' <summary>
        ''' EDI Submission Grid Data Bind
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindEDISubmissionGridView()

            Dim objBLEDISubmission As New BLEDISubmission()

            Try
                EDISubmissionList = objBLEDISubmission.SelectEDIRecords(objShared.ConVisitel)
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to fetch EDI Data. Message: {0}", ex.Message))
            Finally
                objBLEDISubmission = Nothing
            End Try

            'GetSortedEDICodes(GridResults)

            If (EDISubmissionList.Count > 0) Then
                MissingData = EDISubmissionList.Item(0).MissingData
            End If

            GridViewEDISubmissionDetail.DataSource = EDISubmissionList
            GridViewEDISubmissionDetail.DataBind()

        End Sub

    End Class
End Namespace