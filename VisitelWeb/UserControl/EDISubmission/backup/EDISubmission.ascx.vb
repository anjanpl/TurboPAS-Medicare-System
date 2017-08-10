
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

        Private LabelClientId As Label, LabelId As Label, LabelProgram As Label, LabelReceiver As Label, LabelContract As Label, LabelClaimFrequencyTypeCode As Label
 
        Private CheckBoxSelect As CheckBox, chkAll As CheckBox

        Private DropDownListDiagnosisOne As DropDownList, DropDownListDiagnosisTwo As DropDownList, DropDownListDiagnosisThree As DropDownList,
            DropDownListDiagnosisFour As DropDownList, DropDownListPlaceOfService As DropDownList

        Private TextBoxStartDate As TextBox, TextBoxEndDate As TextBox, TextBoxName As TextBox, TextBoxAddress As TextBox, TextBoxReferralNumber As TextBox,
            TextBoxGender As TextBox, TextBoxDateOfBirth As TextBox, TextBoxMedicaidNumber As TextBox, TextBoxBillUnits As TextBox, TextBoxUnitRate As TextBox,
            TextBoxAmount As TextBox, TextBoxProcedureCode As TextBox, TextBoxModifierOne As TextBox, TextBoxModifierTwo As TextBox, TextBoxModifierThree As TextBox,
            TextBoxModifierFour As TextBox

        Private PayPeriods As List(Of PayPeriodDataObject)

        Private DiagonosisOneList As New List(Of DiagonosisDataObject)()
        Private DiagonosisOneCodeList As New List(Of DiagonosisDataObject)()

        Private DiagonosisTwoList As New List(Of DiagonosisDataObject)()
        Private DiagonosisTwoCodeList As New List(Of DiagonosisDataObject)()

        Private DiagonosisThreeList As New List(Of DiagonosisDataObject)()
        Private DiagonosisThreeCodeList As New List(Of DiagonosisDataObject)()

        Private DiagonosisFourList As New List(Of DiagonosisDataObject)()
        Private DiagonosisFourCodeList As New List(Of DiagonosisDataObject)()

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

            'If Request.Params.[Get]("__EVENTTARGET") IsNot Nothing Then
            '    If Request.Params.[Get]("__EVENTTARGET").Contains("ButtonViewRecords") Then
            '        ButtonViewRecords_Click(sender, e)
            '    End If
            'End If

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

            DiagonosisOneList = Nothing
            DiagonosisOneCodeList = Nothing

            DiagonosisTwoList = Nothing
            DiagonosisTwoCodeList = Nothing

            DiagonosisThreeList = Nothing
            DiagonosisThreeCodeList = Nothing

            DiagonosisFourList = Nothing
            DiagonosisFourCodeList = Nothing

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
                'SetGridViewColumnHeaderText(CurrentRow)

                chkAll = DirectCast(CurrentRow.FindControl("chkAll"), CheckBox)
                chkAll.AutoPostBack = True
                chkAll.ClientIDMode = UI.ClientIDMode.Static

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

            If (GenerateEDIFile(TextBoxSaveLocation.Text.Trim(), CheckBoxResubmission.Checked)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(
                    String.Format("EDI File built successfully.  Please select file {0}  and upload to TMHP site.", TextBoxSaveLocation.Text))
            Else
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(
                    String.Format("The file was produced successfully, but had the following CRITICAL Errors: {0}", ""))
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

        Private Function GenerateEDIFile(FilePath As String, IsResubmission As Boolean) As Boolean

            Dim ReturnResult As Boolean = False
            Dim EDIFileResult As New DataSet

            Dim objBLEDISubmission As New BLEDISubmission()

            Try
                ReturnResult = objBLEDISubmission.GenerateEDIFile(objShared.ConVisitel, EDIFileResult, "1720343500", "", 1, 1, 5, 321)
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to generate EDI File. {0}", ex.Message))
            Finally
                objBLEDISubmission = Nothing
            End Try

            If (ReturnResult) Then
                'this code segment write data to file.
                Dim EDIFileName As String = DateTime.Now.ToString("yyyyMMddHHmmss")

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

            'System.Threading.Thread.Sleep(5000)

            MultiSelect()

            Dim objBLEDISubmission As New BLEDISubmission()

            Try
                If (objBLEDISubmission.ProcessEDIViewRecords(objShared.ConVisitel,
                       If(DropDownListPayPeriodStartDate.SelectedValue.Equals("-1"), String.Empty, DropDownListPayPeriodStartDate.SelectedValue),
                       If(DropDownListPayPeriodEndDate.SelectedValue.Equals("-1"), String.Empty, DropDownListPayPeriodEndDate.SelectedValue),
                       If(DropDownListContractNumber.SelectedValue.Equals("-1"), String.Empty, DropDownListContractNumber.SelectedValue),
                            SelectedClientIdList)) Then

                    'Page.ClientScript.RegisterClientScriptBlock([GetType](), "Testing", "__doPostBack('ButtonMissingData', 'MyArgument');", True)

                    'Page.ClientScript.RegisterClientScriptBlock([GetType](), "Testing", "EDIMissingData();")

                End If
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Process EDI View Records")
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
            'System.Threading.Thread.Sleep(5000)
            Return

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

            ButtonFtpSend.OnClientClick = "javascript:return confirm('If transmiting to TMHP please make sure you have logged into Cisco VPN Client');"

            ButtonCorrectClaim.Attributes.Add("OnClick", "EDICorrectedClaims(); return false;")
            ButtonCorrectClaim.UseSubmitBehavior = False
            ButtonCorrectClaim.ClientIDMode = UI.ClientIDMode.Static

            ButtonMissingData.Attributes.Add("OnClick", "EDIMissingData(); return false;")
            ButtonMissingData.UseSubmitBehavior = False
            ButtonMissingData.ClientIDMode = UI.ClientIDMode.Static

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

            ButtonGenerate.OnClientClick = "Javascript:return confirm('You must first print and review the EDI Submission Report. Have you done this?');"

            HyperLinkTexMedConnect.NavigateUrl = Convert.ToString(ConfigurationManager.AppSettings("TexMedConnectLink"), Nothing)

            'upProgUpdatePanelViewRecords.ClientIDMode = UI.ClientIDMode.Static
            'upProgUpdatePanelFTPSend.ClientIDMode = UI.ClientIDMode.Static
            'upProgUpdatePanelViewFTP.ClientIDMode = UI.ClientIDMode.Static

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

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' Importing Javascript
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadJScript()

            Dim scriptBlock As String = Nothing
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
                            & " var prm =''; " _
                            & " jQuery(document).ready(function () {" _
                            & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                            & "     prm.add_initializeRequest(InitializeRequest); " _
                            & "     prm.add_endRequest(EndRequest); " _
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

        Private Sub ViewRecords()


            '   'Increment Control Number by 1
            '   strSql = "update InterchangeControlHeader set ISA13_Control_Num = " & _
            '                "Format(CStr(CInt(ISA13_Control_Num) + 1), '000000000'),ISA11_CONTROL_STANDARD = '^',ISA12_CONTROL_VERSION_ID = '00501'"
            '   DoCmd.RunSQL(strSql)

            '   strSql = "delete * from temp_edidata"
            '   DoCmd.RunSQL(strSql)

            '   'this query uses GROUP BY to compress care_sumary data
            '   DoCmd.OpenQuery("Q_InsertEDIData", acViewNormal, acAdd)

            '   'Do this only if multiselect has value
            '   If Me!NamesList.ItemsSelected.Count <> 0 Then

            '       Call MultiSelect()

            '       'delete rows NOT selected in the textbox
            '       strSql = "delete from temp_EdiData where client_id NOT in (" & sTemp & ")"
            '       DoCmd.RunSQL(strSql)

            '   End If

            '   'For clients whose service started later than the pay period start date then
            '   'set edi_temp start_date to client start of care date
            '   strSql = "update temp_EdiData t,client c set " & _
            '            "t.start_date = c.begin_date " & _
            '            "where c.client_id = t.client_id " & _
            '            "and t.start_date < c.begin_date "

            '   DoCmd.RunSQL(strSql)

            '   'strSql = "update temp_EdiData e, q_EDI_Receiver q " & _
            '"set e.auth_no = '' " & _
            '"where e.client_type_no = q.client_type_no " & _
            '"and q.receiver_primary_id = '745169157' "

            '   'Refreshes the recordset after date range is reselected or updated.
            '   DoCmd.Requery()

            '   Dim db As Database
            '   Dim rst As Object

            '   db = CurrentDb()
            '   rst = db.OpenRecordset("Q_EDI_MissingData")

            '   If Not rst.EOF Then
            '       DoCmd.OpenForm("EDI_MissingData")
            '   End If

            '   rst.Close()
            '   rst = Nothing
            '   db = Nothing

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

        Private Shared Sub UploadFileToFTP(source As String)
            'Try
            '    Dim filename As String = Path.GetFileName(source)
            '    Dim ftpfullpath As String = ftpurl
            '    Dim ftp As FtpWebRequest = DirectCast(FtpWebRequest.Create(ftpfullpath), FtpWebRequest)
            '    ftp.Credentials = New NetworkCredential(ftpusername, ftppassword)

            '    ftp.KeepAlive = True
            '    ftp.UseBinary = True
            '    ftp.Method = WebRequestMethods.Ftp.UploadFile

            '    Dim fs As FileStream = File.OpenRead(source)
            '    Dim buffer As Byte() = New Byte(fs.Length - 1) {}
            '    fs.Read(buffer, 0, buffer.Length)
            '    fs.Close()

            '    Dim ftpstream As Stream = ftp.GetRequestStream()
            '    ftpstream.Write(buffer, 0, buffer.Length)
            '    ftpstream.Close()
            'Catch ex As Exception
            '    Throw ex
            'End Try
        End Sub

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
            objBLClientInfo.GetDiagonosis(objShared.ConVisitel, objShared.CompanyId, DiagonosisOneList, DiagonosisOneCodeList,
                                          DiagonosisTwoList, DiagonosisTwoCodeList,
                                          DiagonosisThreeList, DiagonosisThreeCodeList,
                                          DiagonosisFourList, DiagonosisFourCodeList)
            objBLClientInfo = Nothing

            DiagonosisOneCodeList = DiagonosisOneCodeList.OrderBy(Function(x) x.DiagonosisOne).ToList()
            DiagonosisTwoCodeList = DiagonosisTwoCodeList.OrderBy(Function(x) x.DiagonosisTwo).ToList()
            DiagonosisThreeCodeList = DiagonosisThreeCodeList.OrderBy(Function(x) x.DiagonosisThree).ToList()
            DiagonosisFourCodeList = DiagonosisFourCodeList.OrderBy(Function(x) x.DiagonosisFour).ToList()

        End Sub

        Private Sub BindDropDownListDiagnosisOne()

            DropDownListDiagnosisOne.DataSource = DiagonosisOneCodeList
            DropDownListDiagnosisOne.DataTextField = "DiagonosisOneCode"
            DropDownListDiagnosisOne.DataValueField = "DiagonosisId"
            DropDownListDiagnosisOne.DataBind()

            DropDownListDiagnosisOne.Items.Insert(0, New ListItem("Please Select...", "-1"))

            DropDownListDiagnosisOne.SelectedIndex = DropDownListDiagnosisOne.Items.IndexOf(DropDownListDiagnosisOne.Items.FindByText(
                                                   ((From p In EDISubmissionList Where p.Id = Convert.ToInt64(LabelId.Text)).SingleOrDefault).DiagnosisCodeOne))



        End Sub

        Private Sub BindDropDownListDiagnosisTwo()

            DropDownListDiagnosisTwo.DataSource = DiagonosisTwoCodeList
            DropDownListDiagnosisTwo.DataTextField = "DiagonosisTwoCode"
            DropDownListDiagnosisTwo.DataValueField = "DiagonosisId"
            DropDownListDiagnosisTwo.DataBind()

            DropDownListDiagnosisTwo.Items.Insert(0, New ListItem("Please Select...", "-1"))

            DropDownListDiagnosisTwo.SelectedIndex = DropDownListDiagnosisTwo.Items.IndexOf(DropDownListDiagnosisTwo.Items.FindByText(
                                                   ((From p In EDISubmissionList Where p.Id = Convert.ToInt64(LabelId.Text)).SingleOrDefault).DiagnosisCodeTwo))


        End Sub

        Private Sub BindDropDownListDiagnosisThree()

            DropDownListDiagnosisThree.DataSource = DiagonosisThreeCodeList
            DropDownListDiagnosisThree.DataTextField = "DiagonosisThreeCode"
            DropDownListDiagnosisThree.DataValueField = "DiagonosisId"
            DropDownListDiagnosisThree.DataBind()

            DropDownListDiagnosisThree.Items.Insert(0, New ListItem("Please Select...", "-1"))

            DropDownListDiagnosisThree.SelectedIndex = DropDownListDiagnosisThree.Items.IndexOf(DropDownListDiagnosisThree.Items.FindByText(
                                                   ((From p In EDISubmissionList Where p.Id = Convert.ToInt64(LabelId.Text)).SingleOrDefault).DiagnosisCodeThree))


        End Sub

        Private Sub BindDropDownListDiagnosisFour()

            DropDownListDiagnosisFour.DataSource = DiagonosisFourCodeList
            DropDownListDiagnosisFour.DataTextField = "DiagonosisFourCode"
            DropDownListDiagnosisFour.DataValueField = "DiagonosisId"
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

                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to fetch EDI Data")
            Finally
                objBLEDISubmission = Nothing
            End Try

            'GetSortedEDICodes(GridResults)

            GridViewEDISubmissionDetail.DataSource = EDISubmissionList
            GridViewEDISubmissionDetail.DataBind()

        End Sub

    End Class
End Namespace