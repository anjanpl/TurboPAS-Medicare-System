
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: EDI Corrected Claims
' Author: Anjan Kumar Paul
' Start Date: 24 July 2015
' End Date: 
' History:
'      Version                  Author                      Date             Reason 
'      1.0.0                                                24 July 2015     Initial Development
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
Imports VisitelCommon
Imports Microsoft.Security.Application


Namespace Visitel.UserControl.EDISubmission
    Public Class EDICorrectedClaimsControl
        Inherits CommonDataControl

        Private ControlName As String
        Private objShared As SharedWebControls
        Private QueryStringCollection As NameValueCollection
        Private objEDISubmissionDataObject As EDISubmissionDataObject

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            DirectCast(Me.Page.Master, IMyMasterPage).PageHeaderTitle = "EDI Corrected Claims"
            ControlName = "EDICorrectedClaimsControl"
            objShared = New SharedWebControls()

            QueryStringCollection = Request.QueryString

            objShared.ConnectionOpen()
            InitializeControl()

        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)

            If Not IsPostBack Then
                GetData()
            End If
            GetClaimsData()
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("EDISubmission/" + ControlName)
            LoadJS("JavaScript/EDISubmission/" & ControlName & ".js")
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
            objEDISubmissionDataObject = Nothing
        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)

            Dim objBLEDISubmission As New BLEDISubmission()

            Try

                objBLEDISubmission.CorrectEDIClaims(objShared.ConVisitel, objEDISubmissionDataObject.Id,
                                                    DropDownListClaimFrequencyTypeCode.SelectedValue, TextBoxClaimNumber.Text.Trim())

            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(ex.Message)
            Finally
                objBLEDISubmission = Nothing
            End Try

        End Sub

        Private Sub ButtonDelete_Click(sender As Object, e As EventArgs)

            Dim objBLEDISubmission As New BLEDISubmission()

            Try
                objBLEDISubmission.DeleteEDIClaim(objShared.ConVisitel, objEDISubmissionDataObject.Id)
                ClearControlsData()
                ButtonSave.Enabled = InlineAssignHelper(ButtonDelete.Enabled, False)
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(ex.Message)
            Finally
                objBLEDISubmission = Nothing
            End Try

        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()

            AddHandler ButtonSave.Click, AddressOf ButtonSave_Click
            AddHandler ButtonDelete.Click, AddressOf ButtonDelete_Click

            TextBoxMedicaidNumber.ReadOnly = InlineAssignHelper(TextBoxClientId.ReadOnly, InlineAssignHelper(TextBoxProcedureCode.ReadOnly,
                                             InlineAssignHelper(TextBoxProcedureCode.ReadOnly, InlineAssignHelper(TextBoxAddress.ReadOnly,
                                             InlineAssignHelper(TextBoxAddress.ReadOnly, InlineAssignHelper(TextBoxDateOfBirth.ReadOnly,
                                             InlineAssignHelper(TextBoxGender.ReadOnly, InlineAssignHelper(TextBoxProgram.ReadOnly,
                                             InlineAssignHelper(TextBoxStartDate.ReadOnly, InlineAssignHelper(TextBoxEndDate.ReadOnly,
                                             InlineAssignHelper(TextBoxBilledHours.ReadOnly, InlineAssignHelper(TextBoxUnitRate.ReadOnly,
                                             InlineAssignHelper(TextBoxBilledAmount.ReadOnly, InlineAssignHelper(TextBoxContractNumber.ReadOnly,
                                             InlineAssignHelper(TextBoxDiagnosisCodeOne.ReadOnly, InlineAssignHelper(TextBoxDiagnosisCodeTwo.ReadOnly,
                                             InlineAssignHelper(TextBoxDiagnosisCodeTwo.ReadOnly, InlineAssignHelper(TextBoxDiagnosisCodeThree.ReadOnly,
                                             InlineAssignHelper(TextBoxDiagnosisCodeFour.ReadOnly, InlineAssignHelper(TextBoxModifierOne.ReadOnly,
                                             InlineAssignHelper(TextBoxModifierTwo.ReadOnly, InlineAssignHelper(TextBoxModifierThree.ReadOnly,
                                             InlineAssignHelper(TextBoxModifierFour.ReadOnly, True)))))))))))))))))))))))


            TextBoxMedicaidNumber.Enabled = InlineAssignHelper(TextBoxClientId.Enabled, InlineAssignHelper(TextBoxProcedureCode.Enabled,
                                             InlineAssignHelper(TextBoxProcedureCode.Enabled, InlineAssignHelper(TextBoxAddress.Enabled,
                                             InlineAssignHelper(TextBoxAddress.Enabled, InlineAssignHelper(TextBoxDateOfBirth.Enabled,
                                             InlineAssignHelper(TextBoxGender.Enabled, InlineAssignHelper(TextBoxProgram.Enabled,
                                             InlineAssignHelper(TextBoxStartDate.Enabled, InlineAssignHelper(TextBoxEndDate.Enabled,
                                             InlineAssignHelper(TextBoxBilledHours.Enabled, InlineAssignHelper(TextBoxUnitRate.Enabled,
                                             InlineAssignHelper(TextBoxBilledAmount.Enabled, InlineAssignHelper(TextBoxContractNumber.Enabled,
                                             InlineAssignHelper(TextBoxDiagnosisCodeOne.Enabled, InlineAssignHelper(TextBoxDiagnosisCodeTwo.Enabled,
                                             InlineAssignHelper(TextBoxDiagnosisCodeTwo.Enabled, InlineAssignHelper(TextBoxDiagnosisCodeThree.Enabled,
                                             InlineAssignHelper(TextBoxDiagnosisCodeFour.Enabled, InlineAssignHelper(TextBoxModifierOne.Enabled,
                                             InlineAssignHelper(TextBoxModifierTwo.Enabled, InlineAssignHelper(TextBoxModifierThree.Enabled,
                                             InlineAssignHelper(TextBoxModifierFour.Enabled, False)))))))))))))))))))))))

            DropDownListIndividual.Enabled = InlineAssignHelper(DropDownListReceiver.Enabled, False)

        End Sub


        Private Sub GetData()

            objShared.BindClientDropDownList(DropDownListIndividual, objShared.CompanyId, EnumDataObject.ClientListFor.EDICorrectedClaims)
            objShared.BindReceiverDropDownList(DropDownListReceiver, objShared.CompanyId)

            DirectCast(Me, ICommonDataControl).BindEDICodesDropDownList(objShared.ConVisitel, DropDownListClaimFrequencyTypeCode, String.Empty, String.Empty, String.Empty,
                                             EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesType.EDICorrectedClaims))

            GetClaimsData()
            FillOutClaimsData()

        End Sub

        Private Sub ClearControlsData()

            TextBoxMedicaidNumber.Text = InlineAssignHelper(TextBoxClientId.Text, InlineAssignHelper(TextBoxProcedureCode.Text, InlineAssignHelper(TextBoxAddress.Text,
                                         InlineAssignHelper(TextBoxDateOfBirth.Text, InlineAssignHelper(TextBoxGender.Text, InlineAssignHelper(TextBoxProgram.Text,
                                         InlineAssignHelper(TextBoxStartDate.Text, InlineAssignHelper(TextBoxEndDate.Text, InlineAssignHelper(TextBoxBilledHours.Text,
                                         InlineAssignHelper(TextBoxUnitRate.Text, InlineAssignHelper(TextBoxBilledAmount.Text, InlineAssignHelper(TextBoxContractNumber.Text,
                                         InlineAssignHelper(TextBoxDiagnosisCodeOne.Text, InlineAssignHelper(TextBoxDiagnosisCodeTwo.Text,
                                         InlineAssignHelper(TextBoxDiagnosisCodeThree.Text, InlineAssignHelper(TextBoxDiagnosisCodeFour.Text,
                                         InlineAssignHelper(TextBoxModifierOne.Text, InlineAssignHelper(TextBoxModifierTwo.Text, InlineAssignHelper(TextBoxModifierThree.Text,
                                         InlineAssignHelper(TextBoxModifierFour.Text, InlineAssignHelper(TextBoxClaimNumber.Text,
                                                                                                     String.Empty)))))))))))))))))))))

            DropDownListIndividual.SelectedIndex = InlineAssignHelper(DropDownListReceiver.SelectedIndex, InlineAssignHelper(DropDownListClaimFrequencyTypeCode.SelectedIndex, 0))

        End Sub

        Private Sub GetClaimsData()

            objEDISubmissionDataObject = New EDISubmissionDataObject()

            objEDISubmissionDataObject.StateClientId = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("MedicaidNumber"))), Nothing)
            objEDISubmissionDataObject.ClientId = Convert.ToInt64(Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("ClientId"))), Nothing))
            objEDISubmissionDataObject.ClientFullName = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("IndividualName"))), Nothing)
            objEDISubmissionDataObject.ProcedureId = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("ProcedureCode"))), Nothing)
            objEDISubmissionDataObject.ClientFullAddress = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("Address").Replace("LLL", "#"))), Nothing)
            objEDISubmissionDataObject.DateOfBirth = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("DateOfBirth"))), Nothing)
            objEDISubmissionDataObject.Gender = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("Gender"))), Nothing)
            objEDISubmissionDataObject.Name = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("Program"))), Nothing)
            objEDISubmissionDataObject.StartDate = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("StartDate"))), Nothing)
            objEDISubmissionDataObject.EndDate = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("EndDate"))), Nothing)
            objEDISubmissionDataObject.BillHours = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("BilledHours"))), Nothing)
            objEDISubmissionDataObject.UnitRate = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("UnitRate"))), Nothing)
            objEDISubmissionDataObject.MonetaryAmount = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("BilledAmount"))), Nothing)
            objEDISubmissionDataObject.ContractNumber = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("ContractNumber"))), Nothing)
            objEDISubmissionDataObject.ReceiverId = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("Receiver"))), Nothing)

            objEDISubmissionDataObject.DiagnosisCodeOne = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("DiagnosisCodeOne"))), Nothing)
            objEDISubmissionDataObject.DiagnosisCodeTwo = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("DiagnosisCodeTwo"))), Nothing)
            objEDISubmissionDataObject.DiagnosisCodeThree = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("DiagnosisCodeThree"))), Nothing)
            objEDISubmissionDataObject.DiagnosisCodeFour = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("DiagnosisCodeFour"))), Nothing)

            objEDISubmissionDataObject.ModifierOne = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("ModifierOne"))), Nothing)
            objEDISubmissionDataObject.ModifierTwo = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("ModifierTwo"))), Nothing)
            objEDISubmissionDataObject.ModifierThree = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("ModifierThree"))), Nothing)
            objEDISubmissionDataObject.ModifierFour = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("ModifierFour"))), Nothing)

            objEDISubmissionDataObject.Id = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("EDIId"))), Nothing)
            objEDISubmissionDataObject.CLM0503ClmFrequencyTypeCode = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("ClaimFrequencyTypeCode"))), Nothing)
            objEDISubmissionDataObject.AuthorizationNumber = Convert.ToString(HttpUtility.HtmlDecode(Encoder.HtmlEncode(QueryStringCollection("ClaimNumber"))), Nothing)


        End Sub

        Private Sub FillOutClaimsData()

            TextBoxMedicaidNumber.Text = objEDISubmissionDataObject.StateClientId
            TextBoxClientId.Text = Convert.ToString(objEDISubmissionDataObject.ClientId)

            DropDownListIndividual.SelectedIndex = DropDownListIndividual.Items.IndexOf(DropDownListIndividual.Items.FindByText(objEDISubmissionDataObject.ClientFullName))

            TextBoxProcedureCode.Text = objEDISubmissionDataObject.ProcedureId
            TextBoxAddress.Text = objEDISubmissionDataObject.ClientFullAddress
            TextBoxDateOfBirth.Text = objEDISubmissionDataObject.DateOfBirth
            TextBoxGender.Text = objEDISubmissionDataObject.Gender
            TextBoxProgram.Text = objEDISubmissionDataObject.Name
            TextBoxStartDate.Text = objEDISubmissionDataObject.StartDate
            TextBoxEndDate.Text = objEDISubmissionDataObject.EndDate
            TextBoxBilledHours.Text = objEDISubmissionDataObject.BillHours
            TextBoxUnitRate.Text = objEDISubmissionDataObject.UnitRate
            TextBoxBilledAmount.Text = objEDISubmissionDataObject.MonetaryAmount
            TextBoxContractNumber.Text = objEDISubmissionDataObject.ContractNumber

            DropDownListReceiver.SelectedIndex = DropDownListReceiver.Items.IndexOf(DropDownListReceiver.Items.FindByValue(objEDISubmissionDataObject.ReceiverId))

            TextBoxDiagnosisCodeOne.Text = objEDISubmissionDataObject.DiagnosisCodeOne
            TextBoxDiagnosisCodeTwo.Text = objEDISubmissionDataObject.DiagnosisCodeTwo
            TextBoxDiagnosisCodeThree.Text = objEDISubmissionDataObject.DiagnosisCodeThree
            TextBoxDiagnosisCodeFour.Text = objEDISubmissionDataObject.DiagnosisCodeFour

            TextBoxModifierOne.Text = objEDISubmissionDataObject.ModifierOne
            TextBoxModifierTwo.Text = objEDISubmissionDataObject.ModifierTwo
            TextBoxModifierThree.Text = objEDISubmissionDataObject.ModifierThree
            TextBoxModifierFour.Text = objEDISubmissionDataObject.ModifierFour

            DropDownListClaimFrequencyTypeCode.SelectedIndex = DropDownListClaimFrequencyTypeCode.Items.IndexOf(DropDownListClaimFrequencyTypeCode.Items.FindByValue(
                                                                                                                objEDISubmissionDataObject.CLM0503ClmFrequencyTypeCode))

            TextBoxClaimNumber.Text = objEDISubmissionDataObject.AuthorizationNumber

        End Sub

    End Class
End Namespace