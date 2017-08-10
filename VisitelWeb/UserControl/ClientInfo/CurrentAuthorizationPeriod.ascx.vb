Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject

Namespace Visitel.UserControl.ClientInfo

    Public Class CurrentAuthorizationPeriodControl
        Inherits BaseUserControl

        Private ControlName As String, InvalidUnitRateMessage As String, ValidationGroup As String

        Private ValidationEnable As Boolean

        Public TextBoxDiagnosis As TextBox

        Private objShared As SharedWebControls

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "CurrentAuthorizationPeriodControl"
            objShared = New SharedWebControls()
            GetCaptionFromResource()

            objShared.ConnectionOpen()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
           
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("ClientInfo/" & ControlName)
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()
            TextBoxAuthorizationFromDate.ClientIDMode = objShared.InlineAssignHelper(TextBoxAuthorizationToDate.ClientIDMode, ClientIDMode.Static)

            SetRegularExpressionSetting()
            SetControlToolTip()
            SetControlTabIndex()
        End Sub

        Private Sub SetControlTabIndex()
            TextBoxAuthorizationFromDate.TabIndex = 38
            TextBoxAuthorizationToDate.TabIndex = 39
        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("ClientInfo", ControlName & Convert.ToString(".resx"))

            LabelAuthorization.Text = Convert.ToString(ResourceTable("LabelAuthorization"), Nothing).Trim()
            LabelAuthorization.Text = If(String.IsNullOrEmpty(LabelAuthorization.Text), "Current Authorization Period", LabelAuthorization.Text)

            LabelAuthorizationFromDate.Text = Convert.ToString(ResourceTable("LabelAuthorizationFromDate"), Nothing).Trim()
            LabelAuthorizationFromDate.Text = If(String.IsNullOrEmpty(LabelAuthorizationFromDate.Text), "From:", LabelAuthorizationFromDate.Text)

            LabelAuthorizationToDate.Text = Convert.ToString(ResourceTable("LabelAuthorizationToDate"), Nothing).Trim()
            LabelAuthorizationToDate.Text = If(String.IsNullOrEmpty(LabelAuthorizationToDate.Text), "To:", LabelAuthorizationToDate.Text)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Public Function ValidateData() As Boolean
            If ((Not String.IsNullOrEmpty(TextBoxAuthorizationFromDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxAuthorizationFromDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorAuthorizationFromDate.ErrorMessage)
                Return False
            End If

            If (Not String.IsNullOrEmpty(TextBoxAuthorizationFromDate.Text.Trim())) Then
                If (DateTime.Compare(DateTime.Now, Convert.ToDateTime(TextBoxAuthorizationFromDate.Text.Trim())) < 0) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Authorization From date should not cross the current date.")
                    Return False
                End If
            End If

            Return True
        End Function

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ClearControls()
            TextBoxAuthorizationFromDate.Text = objShared.InlineAssignHelper(TextBoxAuthorizationToDate.Text, String.Empty)
        End Sub

        Public Sub DataFoSave(ByRef objClientCaseCareInfoDataObject As ClientCaseCareInfoDataObject)
            objClientCaseCareInfoDataObject.PlanOfCareStartDate = Convert.ToString(TextBoxAuthorizationFromDate.Text, Nothing).Trim()
            objClientCaseCareInfoDataObject.PlanOfCareEndDate = Convert.ToString(TextBoxAuthorizationToDate.Text, Nothing).Trim()
        End Sub

        Private Sub SetControlToolTip()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            Dim DateToolTip As String = Convert.ToString(ResourceTable("DateFieldToolTip"), Nothing)
            'DateToolTip = If(String.IsNullOrEmpty(DateToolTip), "Example: August 21, 2014", DateToolTip)
            DateToolTip = If(String.IsNullOrEmpty(DateToolTip), "Example: 10/27/2015", DateToolTip)

            TextBoxAuthorizationFromDate.ToolTip = objShared.InlineAssignHelper(TextBoxAuthorizationToDate.ToolTip, DateToolTip)

            ResourceTable = Nothing

        End Sub

        Private Sub SetRegularExpressionSetting()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            Dim ErrorMessage As String = String.Empty, ErrorText As String = String.Empty

             ErrorMessage = Convert.ToString(ResourceTable("InvalidAuthorizationFromDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Authorization From Date.", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorAuthorizationFromDate, "TextBoxAuthorizationFromDate",
                                                           objShared.DateValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidAuthorizationToDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Authorization To Date.", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorAuthorizationToDate, "TextBoxAuthorizationToDate",
                                                           objShared.DateValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ResourceTable = Nothing

            ErrorMessage = Nothing
            ErrorText = Nothing
        End Sub

        ''' <summary>
        ''' Controls Filling Out with data
        ''' </summary>
        ''' <param name="objClientCaseCareInfoDataObject"></param>
        ''' <remarks></remarks>
        Public Sub FillOutData(ByRef objClientCaseCareInfoDataObject As ClientCaseCareInfoDataObject)
            TextBoxAuthorizationFromDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.PlanOfCareStartDate, Nothing))),
                                                       String.Empty, objClientCaseCareInfoDataObject.PlanOfCareStartDate)

            TextBoxAuthorizationToDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.PlanOfCareEndDate, Nothing))),
                                                 String.Empty, objClientCaseCareInfoDataObject.PlanOfCareEndDate)
        End Sub
    End Class
End Namespace
