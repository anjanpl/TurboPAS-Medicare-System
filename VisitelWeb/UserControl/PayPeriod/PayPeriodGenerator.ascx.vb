
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Hospital Stay Popup 
' Author: Anjan Kumar Paul
' Start Date: 28 Feb 2015
' End Date: 28 Feb 2015
' History:
'      Version                  Author                      Date             Reason 
'      1.0.0                                                28 Feb 2015      Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports VisitelCommon.VisitelCommon
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.Settings
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject

Namespace Visitel.UserControl.PayPeriod

    Public Class PayPeriodGeneratorControl
        Inherits BaseUserControl

        Private ValidationEnable As Boolean
        Private ValidationGroup As String

        Private ControlName As String

        Private objShared As SharedWebControls

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            DirectCast(Me.Page.Master, IMyMasterPage).PageHeaderTitle = "Pay Period Generator"
            ControlName = "PayPeriodGeneratorControl"
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
            LoadCss("PayPeriod/" + ControlName)
            LoadJScript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        Private Sub ButtonRun_Click(sender As Object, e As EventArgs)

            Page.Validate()
            Page.Validate(ValidationGroup)

            If ((Page.IsValid) And (ValidateData())) Then

                Dim objBLPayPeriodGenerator As New BLPayPeriodGenerator()

                Dim IsSaved As Boolean = False

                Try
                    objBLPayPeriodGenerator.CalculatePayPeriod(objShared.ConVisitel, Convert.ToString(TextBoxPayPeriodFromDate.Text, Nothing).Trim(),
                                                               Convert.ToString(TextBoxPayPeriodToDate.Text, Nothing).Trim(), objShared.UserId,
                                                               Convert.ToInt64(DropDownListClient.SelectedValue))
                    IsSaved = True
                Catch ex As SqlException
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to complete the process")
                End Try

                If (IsSaved) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Pay Period Generation Completed Successfully.")
                End If

                objBLPayPeriodGenerator = Nothing

            End If

        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            TextBoxPayPeriodFromDate.Text = String.Empty
            TextBoxPayPeriodToDate.Text = String.Empty
            DropDownListClient.SelectedIndex = 0
        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()

            TextBoxPayPeriodFromDate.ClientIDMode = ClientIDMode.Static
            TextBoxPayPeriodToDate.ClientIDMode = ClientIDMode.Static
            DropDownListClient.ClientIDMode = ClientIDMode.Static
            ButtonRun.ClientIDMode = ClientIDMode.Static

            ButtonRun.ValidationGroup = ValidationGroup
            ButtonRun.CausesValidation = True
            ButtonClear.CausesValidation = False

            AddHandler ButtonRun.Click, AddressOf ButtonRun_Click
            AddHandler ButtonClear.Click, AddressOf ButtonClear_Click

            SetRequiredFieldSetting()
            SetRegularExpressionSetting()

            objShared.SetControlTextLength(TextBoxPayPeriodFromDate, 17)
            objShared.SetControlTextLength(TextBoxPayPeriodToDate, 17)

        End Sub

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Private Sub GetData()
            objShared.BindClientDropDownList(DropDownListClient, objShared.CompanyId, EnumDataObject.ClientListFor.Individual)
        End Sub

        Private Sub LoadJScript()

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                          & " var CalendarDateFormat='" & objShared.CalendarDateFormat & "'; " _
                          & " var CalendarImagePath='" & objShared.GetCalendarImagePath & "'; " _
                          & " var prm =''; " _
                          & " jQuery(document).ready(function () {" _
                          & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                          & "     DateFieldsEvent();" _
                          & "     prm.add_endRequest(DateFieldsEvent); " _
                          & "}); " _
                   & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/PayPeriod/" & ControlName & ".js")

        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("PayPeriod", ControlName & Convert.ToString(".resx"))

            LabelPayPeriodGeneratorInfo.Text = Convert.ToString(ResourceTable("LabelPayPeriodGeneratorInfo"), Nothing).Trim()
            LabelPayPeriodGeneratorInfo.Text = If(String.IsNullOrEmpty(LabelPayPeriodGeneratorInfo.Text), "Pay Period Generator", LabelPayPeriodGeneratorInfo.Text)

            LabelPayPeriod.Text = Convert.ToString(ResourceTable("LabelPayPeriod"), Nothing).Trim()
            LabelPayPeriod.Text = If(String.IsNullOrEmpty(LabelPayPeriod.Text), "Pay Period:", LabelPayPeriod.Text)

            LabelEmployeeName.Text = Convert.ToString(ResourceTable("LabelEmployeeName"), Nothing).Trim()
            LabelEmployeeName.Text = If(String.IsNullOrEmpty(LabelEmployeeName.Text), "Name:", LabelEmployeeName.Text)

            ButtonRun.Text = Convert.ToString(ResourceTable("ButtonRun"), Nothing).Trim()
            ButtonRun.Text = If(String.IsNullOrEmpty(ButtonRun.Text), "Run", ButtonRun.Text)

            ButtonClear.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing).Trim()
            ButtonClear.Text = If(String.IsNullOrEmpty(ButtonClear.Text), "Clear", ButtonClear.Text)

            Dim ResultOut As Boolean

            ValidationEnable = If((Boolean.TryParse(ResourceTable("ValidationEnable"), ResultOut)), ResultOut, True)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing)
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "PayPeriod", ValidationGroup)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' Configuring Required Field Controls
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetRequiredFieldSetting()

            objShared.SetRequiredFieldValidatorSetting(RequiredFieldValidatorPayPeriodFromDate, "TextBoxPayPeriodFromDate", "*", "Invalid Date", ValidationEnable, ValidationGroup)
            objShared.SetRequiredFieldValidatorSetting(RequiredFieldValidatorPayPeriodToDate, "TextBoxPayPeriodToDate", "*", "Invalid Date", ValidationEnable, ValidationGroup)

        End Sub

        ''' <summary>
        ''' Configuring Regular Expression Field Controls
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetRegularExpressionSetting()

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorPayPeriodFromDate, "TextBoxPayPeriodFromDate",
                                                           objShared.DateValidationExpression, "Invalid Date",
                                                            "Invalid Date", ValidationEnable, ValidationGroup)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorPayPeriodToDate, "TextBoxPayPeriodToDate",
                                                           objShared.DateValidationExpression, "Invalid Date",
                                                            "Invalid Date", ValidationEnable, ValidationGroup)
        End Sub

        Private Function ValidateData() As Boolean

            If (String.IsNullOrEmpty(TextBoxPayPeriodFromDate.Text.Trim()) Or String.IsNullOrEmpty(TextBoxPayPeriodToDate.Text.Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please correctly set your pay period")
                Return False
            End If

            'If (String.IsNullOrEmpty(TextBoxPayPeriodFromDate.Text.Trim())) Then
            '    Master.DisplayHeaderMessage("Pay Period start date cannot be empty.")
            '    Return False
            'End If

            'If (String.IsNullOrEmpty(TextBoxPayPeriodToDate.Text.Trim())) Then
            '    Master.DisplayHeaderMessage("Pay Period end date cannot be empty.")
            '    Return False
            'End If

            If ((Not String.IsNullOrEmpty(TextBoxPayPeriodFromDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxPayPeriodFromDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorPayPeriodFromDate.ErrorMessage)
                Return False
            End If

            If ((Not String.IsNullOrEmpty(TextBoxPayPeriodToDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxPayPeriodToDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorPayPeriodToDate.ErrorMessage)
                Return False
            End If

            If (Not String.IsNullOrEmpty(TextBoxPayPeriodFromDate.Text.Trim())) Then
                If (DateTime.Compare(DateTime.Now, Convert.ToDateTime(TextBoxPayPeriodFromDate.Text)) < 0) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Pay Period start date should not cross the current date.")
                    Return False
                End If
            End If

            If (Not String.IsNullOrEmpty(TextBoxPayPeriodToDate.Text.Trim())) Then
                If (DateTime.Compare(DateTime.Now, Convert.ToDateTime(TextBoxPayPeriodToDate.Text)) < 0) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Pay Period end date should not cross the current date.")
                    Return False
                End If
            End If

            If (Not String.IsNullOrEmpty(TextBoxPayPeriodFromDate.Text.Trim()) And Not String.IsNullOrEmpty(TextBoxPayPeriodToDate.Text.Trim())) Then
                If (Convert.ToDateTime(TextBoxPayPeriodFromDate.Text) > Convert.ToDateTime(TextBoxPayPeriodToDate.Text)) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Your End Date is earlier than your Start Date.")
                    Return False
                End If
            End If

            Return True

        End Function

    End Class
End Namespace