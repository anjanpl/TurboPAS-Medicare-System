Imports VisitelCommon.VisitelCommon
Imports VisitelWeb.TurboPasApi

Namespace Visitel.UserControl.TurboPAS
    Public Class VestaUploadsControl
        Inherits BaseUserControl

        Private ControlName As String
        Private objShared As SharedWebControls

        Private objService As TurboPASWebServiceSoapClient = New TurboPASWebServiceSoapClient()

        Private SecretKey As String
        Private InitialVisibility As Boolean = True
        Private CredentialPassed As Boolean

        Private ApiLogDetail As New List(Of ApiLog)()

        Private DoubleOutResult As Double

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            DirectCast(Me.Page.Master, IMyMasterPage).PageHeaderTitle = "Turbo PAS Vesta Uploads"

            ControlName = "VestaUploadsControl"

            SecretKey = ReadSetting("TurboPASApiSecretKey")

            objShared = New SharedWebControls()

            GetCaptionFromResource()
            InitializeControl()

        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)

            If Not IsPostBack Then
                UploadInitialize()
            End If

        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)

            LoadCss("TurboPAS/" & ControlName)
            LoadJScript()

            If (ApiLogDetail.Count = 0) Then
                GridViewUploadDownloadOperationDetail.DataSource = ApiLogDetail
                GridViewUploadDownloadOperationDetail.DataBind()
            End If

            SetControl()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared = Nothing
            objService = Nothing
            ApiLogDetail = Nothing
        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()

            GridViewUploadDownloadOperationDetail.AutoGenerateColumns = True
            GridViewUploadDownloadOperationDetail.ShowHeaderWhenEmpty = True
            GridViewUploadDownloadOperationDetail.Width = New Unit("99%")

            ButtonClientUpload.ClientIDMode = UI.ClientIDMode.Static
            ButtonAuthorizationUpload.ClientIDMode = UI.ClientIDMode.Static
            ButtonEmployeeUpload.ClientIDMode = UI.ClientIDMode.Static
            ButtonVisitsUpload.ClientIDMode = UI.ClientIDMode.Static
            ButtonUploadAll.ClientIDMode = UI.ClientIDMode.Static
            ButtonDownload.ClientIDMode = UI.ClientIDMode.Static

            AddHandler ButtonClientUpload.Click, AddressOf ButtonClientUpload_Click
            AddHandler ButtonAuthorizationUpload.Click, AddressOf ButtonAuthorizationUpload_Click
            AddHandler ButtonEmployeeUpload.Click, AddressOf ButtonEmployeeUpload_Click
            AddHandler ButtonVisitsUpload.Click, AddressOf ButtonVisitsUpload_Click
            AddHandler ButtonUploadAll.Click, AddressOf ButtonUploadAll_Click
            AddHandler ButtonDownload.Click, AddressOf ButtonDownload_Click

            CheckBoxIndividualUpload.AutoPostBack = True
            AddHandler CheckBoxIndividualUpload.CheckedChanged, AddressOf CheckBoxIndividualUpload_CheckedChanged

        End Sub

        Private Sub UploadInitialize()

            Session.Add(StaticSettings.VestaUploadProcess.IS_CLIENT_UPLOAD, 0)
            Session.Add(StaticSettings.VestaUploadProcess.IS_AUTHORIZATION_UPLOAD, 0)
            Session.Add(StaticSettings.VestaUploadProcess.IS_EMPLOYEE_UPLOAD, 0)
            Session.Add(StaticSettings.VestaUploadProcess.IS_VISITS_UPLOAD, 0)
            Session.Add(StaticSettings.VestaUploadProcess.IS_ALL_UPLOAD, 0)
            Session.Add(StaticSettings.VestaUploadProcess.AGENCY_ID, String.Empty)
            Session.Add(StaticSettings.VestaUploadProcess.AGENCY_PASSWORD, String.Empty)
            Session.Add(StaticSettings.VestaUploadProcess.BATCH_ID, String.Empty)
            Session.Add(StaticSettings.VestaUploadProcess.API_LOG_DETAIL_INITIAL, String.Empty)


            Dim VestaUploadInitializeRequest As New VestaUploadInitializeRequest()
            VestaUploadInitializeRequest.SecretKey = SecretKey
            VestaUploadInitializeRequest.TurboPASUser = objShared.UserName

            Dim re As New VestaUploadInitializeResponse()

            Try
                re = objService.VestaUploadInitialize(VestaUploadInitializeRequest)
                Session(StaticSettings.VestaUploadProcess.AGENCY_ID) = re.AgencyId
                Session(StaticSettings.VestaUploadProcess.AGENCY_PASSWORD) = re.AgencyPassword
                Session(StaticSettings.VestaUploadProcess.BATCH_ID) = re.BatchId
                Session(StaticSettings.VestaUploadProcess.BATCH_ID) = "11120"

                If (String.IsNullOrEmpty(re.TurboPASUserName)) Then
                    LabelCustomMessage.Text = "TurboPAS User Name Not Mapped"
                    Return
                End If

                ApiLogDetail = re.OperationLog.ToList()

                Session(StaticSettings.VestaUploadProcess.API_LOG_DETAIL_INITIAL) = ApiLogDetail
            Catch ex As Exception
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to Initialize Api Request. Exception: {0}", ex.Message))
            Finally
                VestaUploadInitializeRequest = Nothing
                re = Nothing
                FillOutGridViewUploadDownloadOperationDetail()
            End Try
            
            If ((String.IsNullOrEmpty(Convert.ToString(Session(StaticSettings.VestaUploadProcess.AGENCY_ID), Nothing))) _
                Or (String.IsNullOrEmpty(Convert.ToString(Session(StaticSettings.VestaUploadProcess.AGENCY_PASSWORD), Nothing)))) Then

                LabelCustomMessage.Text = "The upload process cannot be started since user credentials not found." _
                                            & vbCrLf & " Please check credentials & database location."

                InitialVisibility = False

            End If

            If ((String.IsNullOrEmpty((Convert.ToString(Session(StaticSettings.VestaUploadProcess.BATCH_ID), Nothing)))) And (InitialVisibility)) Then
                LabelCustomMessage.Text = "The upload process cannot be started since no valid Batch Id found." _
                                            & vbCrLf & " Please check credentials & database location."
                InitialVisibility = False
            End If

            If (Not (InitialVisibility)) Then
                ButtonClientUpload.Enabled = InlineAssignHelper(ButtonAuthorizationUpload.Enabled, InlineAssignHelper(ButtonEmployeeUpload.Enabled,
                                             InlineAssignHelper(ButtonVisitsUpload.Enabled, InlineAssignHelper(ButtonUploadAll.Enabled, InitialVisibility))))
            End If
        End Sub

        Private Sub ButtonClientUpload_Click(sender As Object, e As EventArgs)

            ButtonClientUpload.Enabled = False

            Dim VestaClientUploadRequest As New VestaClientUploadRequest()
            VestaClientUploadRequest.SecretKey = SecretKey
            VestaClientUploadRequest.AgencyId = Convert.ToString(Session(StaticSettings.VestaUploadProcess.AGENCY_ID), Nothing)
            VestaClientUploadRequest.AgencyPassword = Convert.ToString(Session(StaticSettings.VestaUploadProcess.AGENCY_PASSWORD), Nothing)

            VestaClientUploadRequest.BatchId = Convert.ToString(Session(StaticSettings.VestaUploadProcess.BATCH_ID), Nothing)

            Dim re As New VestaClientUploadResponse()

            Try
                re = objService.VestaClientDataUpload(VestaClientUploadRequest)
                ApiLogDetail = re.OperationLog.ToList()
            Catch ex As Exception
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to Upload Client Data. Exception: {0}", ex.Message))
            Finally
                VestaClientUploadRequest = Nothing
                re = Nothing
                FillOutGridViewUploadDownloadOperationDetail()

                Session(StaticSettings.VestaUploadProcess.IS_CLIENT_UPLOAD) = 1

                ButtonAuthorizationUpload.Enabled = Convert.ToBoolean(Session(StaticSettings.VestaUploadProcess.IS_CLIENT_UPLOAD))
            End Try
        End Sub

        Private Sub ButtonAuthorizationUpload_Click(sender As Object, e As EventArgs)

            ButtonAuthorizationUpload.Enabled = False

            Dim VestaAuthorizationUploadRequest As New VestaAuthorizationUploadRequest()
            VestaAuthorizationUploadRequest.SecretKey = SecretKey
            VestaAuthorizationUploadRequest.AgencyId = Convert.ToString(Session(StaticSettings.VestaUploadProcess.AGENCY_ID), Nothing)
            VestaAuthorizationUploadRequest.AgencyPassword = Convert.ToString(Session(StaticSettings.VestaUploadProcess.AGENCY_PASSWORD), Nothing)

            VestaAuthorizationUploadRequest.BatchId = Convert.ToString(Session(StaticSettings.VestaUploadProcess.BATCH_ID), Nothing)

            Dim re As New VestaAuthorizationUploadResponse()

            Try
                re = objService.VestaAuthorizationDataUpload(VestaAuthorizationUploadRequest)
                ApiLogDetail = re.OperationLog.ToList()
            Catch ex As Exception
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to Upload Authorization Data. Exception: {0}", ex.Message))
            Finally
                VestaAuthorizationUploadRequest = Nothing
                re = Nothing
                FillOutGridViewUploadDownloadOperationDetail()

                Session(StaticSettings.VestaUploadProcess.IS_AUTHORIZATION_UPLOAD) = 1

                ButtonEmployeeUpload.Enabled = Convert.ToBoolean(Session(StaticSettings.VestaUploadProcess.IS_AUTHORIZATION_UPLOAD))
            End Try
        End Sub

        Private Sub ButtonEmployeeUpload_Click(sender As Object, e As EventArgs)

            ButtonEmployeeUpload.Enabled = False

            Dim VestaEmployeeUploadRequest As New VestaEmployeeUploadRequest()
            VestaEmployeeUploadRequest.SecretKey = SecretKey
            VestaEmployeeUploadRequest.AgencyId = Convert.ToString(Session(StaticSettings.VestaUploadProcess.AGENCY_ID), Nothing)
            VestaEmployeeUploadRequest.AgencyPassword = Convert.ToString(Session(StaticSettings.VestaUploadProcess.AGENCY_PASSWORD), Nothing)

            VestaEmployeeUploadRequest.BatchId = Convert.ToString(Session(StaticSettings.VestaUploadProcess.BATCH_ID), Nothing)

            Dim re As New VestaEmployeeUploadResponse()

            Try
                re = objService.VestaEmployeeDataUpload(VestaEmployeeUploadRequest)
                ApiLogDetail = re.OperationLog.ToList()
            Catch ex As Exception
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to Upload Employee Data. Exception: {0}", ex.Message))
            Finally
                VestaEmployeeUploadRequest = Nothing
                re = Nothing
                FillOutGridViewUploadDownloadOperationDetail()

                Session(StaticSettings.VestaUploadProcess.IS_EMPLOYEE_UPLOAD) = 1

                ButtonVisitsUpload.Enabled = Convert.ToBoolean(Session(StaticSettings.VestaUploadProcess.IS_EMPLOYEE_UPLOAD))
            End Try
        End Sub

        Private Sub ButtonVisitsUpload_Click(sender As Object, e As EventArgs)

            Dim VestaVisitUploadRequest As New VestaVisitUploadRequest()
            VestaVisitUploadRequest.SecretKey = SecretKey
            VestaVisitUploadRequest.AgencyId = Convert.ToString(Session(StaticSettings.VestaUploadProcess.AGENCY_ID), Nothing)
            VestaVisitUploadRequest.AgencyPassword = Convert.ToString(Session(StaticSettings.VestaUploadProcess.AGENCY_PASSWORD), Nothing)

            VestaVisitUploadRequest.BatchId = Convert.ToString(Session(StaticSettings.VestaUploadProcess.BATCH_ID), Nothing)

            Dim re As New VestaVisitsUploadResponse()

            Try
                re = objService.VestaVisitsDataUpload(VestaVisitUploadRequest)
                ApiLogDetail = re.OperationLog.ToList()
            Catch ex As Exception
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to Upload Visits Data. Exception: {0}", ex.Message))
            Finally
                VestaVisitUploadRequest = Nothing
                re = Nothing
                FillOutGridViewUploadDownloadOperationDetail()

                Session(StaticSettings.VestaUploadProcess.IS_VISITS_UPLOAD) = 1

                ButtonVisitsUpload.Enabled = False
            End Try
        End Sub

        Private Sub ButtonUploadAll_Click(sender As Object, e As EventArgs)

            ButtonUploadAll.Enabled = False

            Dim VestaUploadRequest As New VestaUploadRequest()
            VestaUploadRequest.SecretKey = SecretKey
            VestaUploadRequest.AgencyId = Convert.ToString(Session(StaticSettings.VestaUploadProcess.AGENCY_ID), Nothing)
            VestaUploadRequest.AgencyPassword = Convert.ToString(Session(StaticSettings.VestaUploadProcess.AGENCY_PASSWORD), Nothing)

            VestaUploadRequest.BatchId = Convert.ToString(Session(StaticSettings.VestaUploadProcess.BATCH_ID), Nothing)

            Dim re As New VestaUploadResponse()

            Try
                re = objService.VestaDataUpload(VestaUploadRequest)
                ApiLogDetail = re.OperationLog.ToList()
            Catch ex As Exception
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to Upload Vesta Data. Exception: {0}", ex.Message))
            Finally
                VestaUploadRequest = Nothing
                re = Nothing
                FillOutGridViewUploadDownloadOperationDetail()
                Session(StaticSettings.VestaUploadProcess.IS_ALL_UPLOAD) = 1
            End Try
        End Sub

        Private Sub ButtonDownload_Click(sender As Object, e As EventArgs)

            'Dim VestaDataDownloadRequest As New VestaDataDownloadRequest()
            'VestaDataDownloadRequest.SecretKey = SecretKey
            'VestaDataDownloadRequest.AgencyId = Convert.ToString(Session(StaticSettings.VestaUploadProcess.AGENCY_ID), Nothing)
            'VestaDataDownloadRequest.AgencyPassword = Convert.ToString(Session(StaticSettings.VestaUploadProcess.AGENCY_PASSWORD), Nothing)

            'VestaDataDownloadRequest.BatchId = Convert.ToString(Session(StaticSettings.VestaUploadProcess.BATCH_ID), Nothing)

            'Dim re As New VestaDataDownloadResponse()

            'Try
            '    re = objService.VestaDataDownload(VestaDataDownloadRequest)
            '    ApiLogDetail = re.OperationLog.ToList()
            'Catch ex As Exception
            '    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to Download Vesta Data. Exception: {0}", ex.Message))
            'Finally
            '    VestaDataDownloadRequest = Nothing
            '    re = Nothing
            '    FillOutGridViewUploadDownloadOperationDetail()
            'End Try
        End Sub

        Private Sub CheckBoxIndividualUpload_CheckedChanged(sender As Object, e As EventArgs)
            ApiLogDetail = DirectCast(Session(StaticSettings.VestaUploadProcess.API_LOG_DETAIL_INITIAL), List(Of ApiLog))
            FillOutGridViewUploadDownloadOperationDetail()
        End Sub

        ''' <summary>
        ''' Control Enable / Disable
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetControl()

            CredentialPassed = ((Not String.IsNullOrEmpty(Convert.ToString(Session(StaticSettings.VestaUploadProcess.AGENCY_ID), Nothing))) _
                                  AndAlso (Not String.IsNullOrEmpty(Convert.ToString(Session(StaticSettings.VestaUploadProcess.AGENCY_PASSWORD), Nothing))))

            ButtonClientUpload.Enabled = If(((Not (Convert.ToBoolean(Session(StaticSettings.VestaUploadProcess.IS_CLIENT_UPLOAD)))) _
                                             AndAlso (Not String.IsNullOrEmpty(Convert.ToString(Session(StaticSettings.VestaUploadProcess.BATCH_ID), Nothing))) _
                                             AndAlso (CredentialPassed)), _
                                             CheckBoxIndividualUpload.Checked, False)

            ButtonAuthorizationUpload.Enabled = If(((Not (Convert.ToBoolean(Session(StaticSettings.VestaUploadProcess.IS_AUTHORIZATION_UPLOAD)))) _
                                                    AndAlso (Convert.ToBoolean(Session(StaticSettings.VestaUploadProcess.IS_CLIENT_UPLOAD))) _
                                                    AndAlso (Not String.IsNullOrEmpty(Convert.ToString(Session(StaticSettings.VestaUploadProcess.BATCH_ID), Nothing))) _
                                                    AndAlso (CredentialPassed)), _
                                                    CheckBoxIndividualUpload.Checked, False)

            ButtonEmployeeUpload.Enabled = If(((Not (Convert.ToBoolean(Session(StaticSettings.VestaUploadProcess.IS_EMPLOYEE_UPLOAD)))) _
                                               AndAlso (Convert.ToBoolean(Session(StaticSettings.VestaUploadProcess.IS_CLIENT_UPLOAD))) _
                                               AndAlso (Convert.ToBoolean(Session(StaticSettings.VestaUploadProcess.IS_AUTHORIZATION_UPLOAD))) _
                                               AndAlso (Not String.IsNullOrEmpty(Convert.ToString(Session(StaticSettings.VestaUploadProcess.BATCH_ID), Nothing))) _
                                               AndAlso (CredentialPassed)),
                                               CheckBoxIndividualUpload.Checked, False)

            ButtonVisitsUpload.Enabled = If(((Not (Convert.ToBoolean(Session(StaticSettings.VestaUploadProcess.IS_VISITS_UPLOAD)))) _
                                             AndAlso (Convert.ToBoolean(Session(StaticSettings.VestaUploadProcess.IS_CLIENT_UPLOAD))) _
                                             AndAlso (Convert.ToBoolean(Session(StaticSettings.VestaUploadProcess.IS_AUTHORIZATION_UPLOAD))) _
                                             AndAlso (Convert.ToBoolean(Session(StaticSettings.VestaUploadProcess.IS_EMPLOYEE_UPLOAD))) _
                                             AndAlso (Not String.IsNullOrEmpty(Convert.ToString(Session(StaticSettings.VestaUploadProcess.BATCH_ID), Nothing))) _
                                             AndAlso (CredentialPassed)),
                                             CheckBoxIndividualUpload.Checked, False)

            If ((Not ((Convert.ToBoolean(Session(StaticSettings.VestaUploadProcess.IS_ALL_UPLOAD))))) _
                    AndAlso (Not String.IsNullOrEmpty(Convert.ToString(Session(StaticSettings.VestaUploadProcess.BATCH_ID), Nothing))) _
                    AndAlso (CredentialPassed)) Then
                ButtonUploadAll.Enabled = Not CheckBoxIndividualUpload.Checked
            End If

            If ((Convert.ToBoolean(Session(StaticSettings.VestaUploadProcess.IS_CLIENT_UPLOAD))) _
                OrElse (Convert.ToBoolean(Session(StaticSettings.VestaUploadProcess.IS_AUTHORIZATION_UPLOAD))) _
                OrElse (Convert.ToBoolean(Session(StaticSettings.VestaUploadProcess.IS_EMPLOYEE_UPLOAD))) _
                OrElse (Convert.ToBoolean(Session(StaticSettings.VestaUploadProcess.IS_VISITS_UPLOAD))) _
                OrElse (String.IsNullOrEmpty(Convert.ToString(Session(StaticSettings.VestaUploadProcess.BATCH_ID), Nothing))) _
                OrElse (Not CredentialPassed)) Then
                ButtonUploadAll.Enabled = False
            End If

            If (Convert.ToBoolean(Session(StaticSettings.VestaUploadProcess.IS_ALL_UPLOAD))) Then
                ButtonClientUpload.Enabled = InlineAssignHelper(ButtonAuthorizationUpload.Enabled, InlineAssignHelper(ButtonEmployeeUpload.Enabled,
                                             InlineAssignHelper(ButtonVisitsUpload.Enabled, InlineAssignHelper(ButtonUploadAll.Enabled, False))))
            End If

            ButtonClientUpload.CssClass = If((ButtonClientUpload.Enabled), "ButtonClientUpload", "ButtonClientUpload ButtonDisable")
            ButtonAuthorizationUpload.CssClass = If((ButtonAuthorizationUpload.Enabled), "ButtonAuthorizationUpload", "ButtonAuthorizationUpload ButtonDisable")
            ButtonEmployeeUpload.CssClass = If((ButtonEmployeeUpload.Enabled), "ButtonEmployeeUpload", "ButtonEmployeeUpload ButtonDisable")
            ButtonVisitsUpload.CssClass = If((ButtonVisitsUpload.Enabled), "ButtonVisitsUpload", "ButtonVisitsUpload ButtonDisable")
            ButtonUploadAll.CssClass = If((ButtonUploadAll.Enabled), "ButtonUploadAll", "ButtonUploadAll ButtonDisable")

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
                            & " var LoaderImagePath ='" & objShared.GetLoaderImagePath & "'; " _
                            & " var prm =''; " _
                            & " jQuery(document).ready(function () {" _
                            & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                            & "     prm.add_initializeRequest(InitializeRequest); " _
                            & "     prm.add_endRequest(EndRequest); " _
                            & "}); " _
                    & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/TurboPAS/" & ControlName & ".js")

        End Sub

        ''' <summary>
        ''' Fill Out GridViewUploadDownloadOperationDetail
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub FillOutGridViewUploadDownloadOperationDetail()

            GridViewUploadDownloadOperationDetail.DataSource = ApiLogDetail
            GridViewUploadDownloadOperationDetail.DataBind()

        End Sub

        ''' <summary>
        ''' Reading App Setting Variable
        ''' </summary>
        ''' <param name="key"></param>
        ''' <returns></returns>
        Public Function ReadSetting(key As String) As String
            Dim result As String = String.Empty

            Try
                Dim appSettings = ConfigurationManager.AppSettings
                result = If(appSettings(key), "Not Found")
            Catch ex As ConfigurationErrorsException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(ex.Message)
            End Try

            Return result
        End Function

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("TurboPAS", ControlName & Convert.ToString(".resx"))

            LabelTurboPASVestaUpload.Text = Convert.ToString(ResourceTable("LabelTurboPASVestaUpload"), Nothing)
            LabelTurboPASVestaUpload.Text = If(String.IsNullOrEmpty(LabelTurboPASVestaUpload.Text), "TurboPAS-Vesta Upload", LabelTurboPASVestaUpload.Text)

            CheckBoxIndividualUpload.Text = Convert.ToString(ResourceTable("CheckBoxIndividualUpload"), Nothing)
            CheckBoxIndividualUpload.Text = If(String.IsNullOrEmpty(CheckBoxIndividualUpload.Text), "Individual Upload", CheckBoxIndividualUpload.Text)

            ButtonClientUpload.Text = Convert.ToString(ResourceTable("ButtonClientUpload"), Nothing)
            ButtonClientUpload.Text = If(String.IsNullOrEmpty(ButtonClientUpload.Text), "Client Upload", ButtonClientUpload.Text)

            ButtonAuthorizationUpload.Text = Convert.ToString(ResourceTable("ButtonAuthorizationUpload"), Nothing)
            ButtonAuthorizationUpload.Text = If(String.IsNullOrEmpty(ButtonAuthorizationUpload.Text), "Authorization Upload", ButtonAuthorizationUpload.Text)

            ButtonEmployeeUpload.Text = Convert.ToString(ResourceTable("ButtonEmployeeUpload"), Nothing)
            ButtonEmployeeUpload.Text = If(String.IsNullOrEmpty(ButtonEmployeeUpload.Text), "Employee Upload", ButtonEmployeeUpload.Text)

            ButtonVisitsUpload.Text = Convert.ToString(ResourceTable("ButtonVisitsUpload"), Nothing)
            ButtonVisitsUpload.Text = If(String.IsNullOrEmpty(ButtonVisitsUpload.Text), "Visits Upload", ButtonVisitsUpload.Text)

            ButtonUploadAll.Text = Convert.ToString(ResourceTable("ButtonUploadAll"), Nothing)
            ButtonUploadAll.Text = If(String.IsNullOrEmpty(ButtonUploadAll.Text), "Upload All", ButtonUploadAll.Text)

            ButtonDownload.Text = Convert.ToString(ResourceTable("ButtonDownload"), Nothing)
            ButtonDownload.Text = If(String.IsNullOrEmpty(ButtonDownload.Text), "Download", ButtonDownload.Text)

            ResourceTable = Nothing

        End Sub

    End Class
End Namespace