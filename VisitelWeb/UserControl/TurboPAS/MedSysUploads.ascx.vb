Imports VisitelCommon.VisitelCommon
Imports VisitelWeb.TurboPasApi

Namespace Visitel.UserControl.TurboPAS
    Public Class MedSysUploadsControl
        Inherits BaseUserControl

        Private ControlName As String
        Private objShared As SharedWebControls

        Private objService As TurboPASWebServiceSoapClient = New TurboPASWebServiceSoapClient()

        Private SecretKey As String
        Private CredentialPassed As Boolean

        Private ApiLogDetail As New List(Of ApiLog)()

        Private DoubleOutResult As Double

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            DirectCast(Me.Page.Master, IMyMasterPage).PageHeaderTitle = "Turbo PAS Med Sys Uploads"

            ControlName = "MedSysUploadsControl"

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
            ButtonStaffUpload.ClientIDMode = UI.ClientIDMode.Static
            ButtonScheduleUpload.ClientIDMode = UI.ClientIDMode.Static
            ButtonUploadAll.ClientIDMode = UI.ClientIDMode.Static
            ButtonDownload.ClientIDMode = UI.ClientIDMode.Static

            AddHandler ButtonClientUpload.Click, AddressOf ButtonClientUpload_Click
            AddHandler ButtonAuthorizationUpload.Click, AddressOf ButtonAuthorizationUpload_Click
            AddHandler ButtonStaffUpload.Click, AddressOf ButtonStaffUpload_Click
            AddHandler ButtonScheduleUpload.Click, AddressOf ButtonScheduleUpload_Click
            AddHandler ButtonUploadAll.Click, AddressOf ButtonUploadAll_Click
            AddHandler ButtonDownload.Click, AddressOf ButtonDownload_Click

            CheckBoxIndividualUpload.AutoPostBack = True
            AddHandler CheckBoxIndividualUpload.CheckedChanged, AddressOf CheckBoxIndividualUpload_CheckedChanged

        End Sub

        Private Sub UploadInitialize()

            Session.Add(StaticSettings.MedSysUploadProcess.IS_CLIENT_UPLOAD, 0)
            Session.Add(StaticSettings.MedSysUploadProcess.IS_AUTHORIZATION_UPLOAD, 0)
            Session.Add(StaticSettings.MedSysUploadProcess.IS_STAFF_UPLOAD, 0)
            Session.Add(StaticSettings.MedSysUploadProcess.IS_SCHEDULE_UPLOAD, 0)
            Session.Add(StaticSettings.MedSysUploadProcess.IS_ALL_UPLOAD, 0)
            Session.Add(StaticSettings.MedSysUploadProcess.ACCOUNT_ID, String.Empty)
            Session.Add(StaticSettings.MedSysUploadProcess.AGENCY_PASSWORD, String.Empty)
            Session.Add(StaticSettings.VestaUploadProcess.API_LOG_DETAIL_INITIAL, String.Empty)

            Dim MedSysUploadInitializeRequest As New MedSysUploadInitializeRequest()
            MedSysUploadInitializeRequest.SecretKey = SecretKey
            MedSysUploadInitializeRequest.TurboPASUser = objShared.UserName

            Dim re As New MedSysUploadInitializeResponse()

            Try
                re = objService.MedSysUploadInitialize(MedSysUploadInitializeRequest)
                Session(StaticSettings.MedSysUploadProcess.ACCOUNT_ID) = re.AccountId
                Session(StaticSettings.MedSysUploadProcess.AGENCY_PASSWORD) = re.AgencyPassword

                If (String.IsNullOrEmpty(re.TurboPASUserName)) Then
                    LabelCustomMessage.Text = "TurboPAS User Name Not Mapped"
                    Return
                End If

                ApiLogDetail = re.OperationLog.ToList()

                Session(StaticSettings.VestaUploadProcess.API_LOG_DETAIL_INITIAL) = ApiLogDetail
            Catch ex As Exception
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to Initialize Api Request. Exception: {0}", ex.Message))
            Finally
                MedSysUploadInitializeRequest = Nothing
                re = Nothing
                FillOutGridViewUploadDownloadOperationDetail()
            End Try
            
            If ((String.IsNullOrEmpty(Convert.ToString(Session(StaticSettings.MedSysUploadProcess.ACCOUNT_ID), Nothing))) _
                Or (String.IsNullOrEmpty(Convert.ToString(Session(StaticSettings.MedSysUploadProcess.AGENCY_PASSWORD), Nothing)))) Then

                LabelCustomMessage.Text = "The upload process cannot be started since user credentials not found." _
                                            & vbCrLf & " Please check credentials & database location."

                ButtonClientUpload.Enabled = InlineAssignHelper(ButtonAuthorizationUpload.Enabled, InlineAssignHelper(ButtonStaffUpload.Enabled,
                                             InlineAssignHelper(ButtonScheduleUpload.Enabled, InlineAssignHelper(ButtonUploadAll.Enabled, False))))

            End If
        End Sub

        Private Sub ButtonClientUpload_Click(sender As Object, e As EventArgs)

            ButtonClientUpload.Enabled = False

            Dim MedSysClientUploadRequest As New MedSysClientUploadRequest()
            MedSysClientUploadRequest.SecretKey = SecretKey
            MedSysClientUploadRequest.AccountId = Convert.ToString(Session(StaticSettings.MedSysUploadProcess.ACCOUNT_ID), Nothing)

            Dim re As New MedSysClientUploadResponse()

            Try
                re = objService.MedSysClientDataUpload(MedSysClientUploadRequest)
                ApiLogDetail = re.OperationLog.ToList()
            Catch ex As Exception
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to Upload Client Data. Exception: {0}", ex.Message))
            Finally
                MedSysClientUploadRequest = Nothing
                re = Nothing
                FillOutGridViewUploadDownloadOperationDetail()

                Session(StaticSettings.MedSysUploadProcess.IS_CLIENT_UPLOAD) = 1

                ButtonAuthorizationUpload.Enabled = Convert.ToBoolean(Session(StaticSettings.MedSysUploadProcess.IS_CLIENT_UPLOAD))
            End Try
        End Sub

        Private Sub ButtonAuthorizationUpload_Click(sender As Object, e As EventArgs)

            ButtonAuthorizationUpload.Enabled = False

            Dim MedSysAuthorizationUploadRequest As New MedSysAuthorizationUploadRequest()
            MedSysAuthorizationUploadRequest.SecretKey = SecretKey
            MedSysAuthorizationUploadRequest.AccountId = Convert.ToString(Session(StaticSettings.MedSysUploadProcess.ACCOUNT_ID), Nothing)

            Dim re As New MedSysAuthorizationUploadResponse()

            Try
                re = objService.MedSysAuthorizationDataUpload(MedSysAuthorizationUploadRequest)
                ApiLogDetail = re.OperationLog.ToList()
            Catch ex As Exception
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to Upload Authorization Data. Exception: {0}", ex.Message))
            Finally
                MedSysAuthorizationUploadRequest = Nothing
                re = Nothing
                FillOutGridViewUploadDownloadOperationDetail()

                Session(StaticSettings.MedSysUploadProcess.IS_AUTHORIZATION_UPLOAD) = 1

                ButtonStaffUpload.Enabled = Convert.ToBoolean(Session(StaticSettings.MedSysUploadProcess.IS_AUTHORIZATION_UPLOAD))
            End Try
        End Sub

        Private Sub ButtonStaffUpload_Click(sender As Object, e As EventArgs)

            ButtonStaffUpload.Enabled = False

            Dim MedSysStaffUploadRequest As New MedSysStaffUploadRequest()
            MedSysStaffUploadRequest.SecretKey = SecretKey
            MedSysStaffUploadRequest.AccountId = Convert.ToString(Session(StaticSettings.MedSysUploadProcess.ACCOUNT_ID), Nothing)

            Dim re As New MedSysStaffUploadResponse()

            Try
                re = objService.MedSysStaffDataUpload(MedSysStaffUploadRequest)
                ApiLogDetail = re.OperationLog.ToList()
            Catch ex As Exception
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to Upload Staff Data. Exception: {0}", ex.Message))
            Finally
                MedSysStaffUploadRequest = Nothing
                re = Nothing
                FillOutGridViewUploadDownloadOperationDetail()

                Session(StaticSettings.MedSysUploadProcess.IS_STAFF_UPLOAD) = 1

                ButtonScheduleUpload.Enabled = Convert.ToBoolean(Session(StaticSettings.MedSysUploadProcess.IS_STAFF_UPLOAD))
            End Try
        End Sub

        Private Sub ButtonScheduleUpload_Click(sender As Object, e As EventArgs)

            Dim MedSysScheduleUploadRequest As New MedSysScheduleUploadRequest()
            MedSysScheduleUploadRequest.SecretKey = SecretKey
            MedSysScheduleUploadRequest.AccountId = Convert.ToString(Session(StaticSettings.MedSysUploadProcess.ACCOUNT_ID), Nothing)

            Dim re As New MedSysScheduleUploadResponse()

            Try
                re = objService.MedSysScheduleDataUpload(MedSysScheduleUploadRequest)
                ApiLogDetail = re.OperationLog.ToList()
            Catch ex As Exception
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to Upload Schedule Data. Exception: {0}", ex.Message))
            Finally
                MedSysScheduleUploadRequest = Nothing
                re = Nothing
                FillOutGridViewUploadDownloadOperationDetail()

                Session(StaticSettings.MedSysUploadProcess.IS_SCHEDULE_UPLOAD) = 1

                ButtonScheduleUpload.Enabled = False
            End Try
        End Sub

        Private Sub ButtonUploadAll_Click(sender As Object, e As EventArgs)

            ButtonUploadAll.Enabled = False

            Dim MedSysUploadRequest As New MedSysUploadRequest()
            MedSysUploadRequest.SecretKey = SecretKey
            MedSysUploadRequest.AccountId = Convert.ToString(Session(StaticSettings.MedSysUploadProcess.ACCOUNT_ID), Nothing)

            Dim re As New MedSysUploadResponse()

            Try
                re = objService.MedSysDataUpload(MedSysUploadRequest)
                ApiLogDetail = re.OperationLog.ToList()
            Catch ex As Exception
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to Upload Med Sys Data. Exception: {0}", ex.Message))
            Finally
                MedSysUploadRequest = Nothing
                re = Nothing
                FillOutGridViewUploadDownloadOperationDetail()

                Session(StaticSettings.VestaUploadProcess.IS_ALL_UPLOAD) = 1
            End Try
        End Sub

        Private Sub ButtonDownload_Click(sender As Object, e As EventArgs)

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

            CredentialPassed = ((Not String.IsNullOrEmpty(Convert.ToString(Session(StaticSettings.MedSysUploadProcess.ACCOUNT_ID), Nothing))) _
                                  AndAlso (Not String.IsNullOrEmpty(Convert.ToString(Session(StaticSettings.MedSysUploadProcess.AGENCY_PASSWORD), Nothing))))

            ButtonClientUpload.Enabled = If(((Not (Convert.ToBoolean(Session(StaticSettings.MedSysUploadProcess.IS_CLIENT_UPLOAD)))) _
                                            AndAlso (CredentialPassed)), _
                                             CheckBoxIndividualUpload.Checked, False)

            ButtonAuthorizationUpload.Enabled = If(((Not (Convert.ToBoolean(Session(StaticSettings.MedSysUploadProcess.IS_AUTHORIZATION_UPLOAD)))) _
                                                    AndAlso (Convert.ToBoolean(Session(StaticSettings.MedSysUploadProcess.IS_CLIENT_UPLOAD))) _
                                                    AndAlso (CredentialPassed)), _
                                                    CheckBoxIndividualUpload.Checked, False)

            ButtonStaffUpload.Enabled = If(((Not (Convert.ToBoolean(Session(StaticSettings.MedSysUploadProcess.IS_STAFF_UPLOAD)))) _
                                               AndAlso (Convert.ToBoolean(Session(StaticSettings.MedSysUploadProcess.IS_CLIENT_UPLOAD))) _
                                               AndAlso (Convert.ToBoolean(Session(StaticSettings.MedSysUploadProcess.IS_AUTHORIZATION_UPLOAD))) _
                                               AndAlso (CredentialPassed)), _
                                               CheckBoxIndividualUpload.Checked, False)

            ButtonScheduleUpload.Enabled = If(((Not (Convert.ToBoolean(Session(StaticSettings.MedSysUploadProcess.IS_SCHEDULE_UPLOAD)))) _
                                             AndAlso (Convert.ToBoolean(Session(StaticSettings.MedSysUploadProcess.IS_CLIENT_UPLOAD))) _
                                             AndAlso (Convert.ToBoolean(Session(StaticSettings.MedSysUploadProcess.IS_AUTHORIZATION_UPLOAD))) _
                                             AndAlso (Convert.ToBoolean(Session(StaticSettings.MedSysUploadProcess.IS_STAFF_UPLOAD))) _
                                             AndAlso (CredentialPassed)), _
                                             CheckBoxIndividualUpload.Checked, False)

            If ((Not ((Convert.ToBoolean(Session(StaticSettings.MedSysUploadProcess.IS_ALL_UPLOAD))))) _
                AndAlso (CredentialPassed)) Then
                ButtonUploadAll.Enabled = Not CheckBoxIndividualUpload.Checked
            End If

            If ((Convert.ToBoolean(Session(StaticSettings.MedSysUploadProcess.IS_CLIENT_UPLOAD))) _
                OrElse (Convert.ToBoolean(Session(StaticSettings.MedSysUploadProcess.IS_AUTHORIZATION_UPLOAD))) _
                OrElse (Convert.ToBoolean(Session(StaticSettings.MedSysUploadProcess.IS_STAFF_UPLOAD))) _
                OrElse (Convert.ToBoolean(Session(StaticSettings.MedSysUploadProcess.IS_SCHEDULE_UPLOAD))) _
                OrElse (Not CredentialPassed)) Then
                ButtonUploadAll.Enabled = False
            End If

            If (Convert.ToBoolean(Session(StaticSettings.MedSysUploadProcess.IS_ALL_UPLOAD))) Then
                ButtonClientUpload.Enabled = InlineAssignHelper(ButtonAuthorizationUpload.Enabled, InlineAssignHelper(ButtonStaffUpload.Enabled,
                                             InlineAssignHelper(ButtonScheduleUpload.Enabled, InlineAssignHelper(ButtonUploadAll.Enabled, False))))
            End If

            ButtonClientUpload.CssClass = If((ButtonClientUpload.Enabled), "ButtonClientUpload", "ButtonClientUpload ButtonDisable")
            ButtonAuthorizationUpload.CssClass = If((ButtonAuthorizationUpload.Enabled), "ButtonAuthorizationUpload", "ButtonAuthorizationUpload ButtonDisable")
            ButtonStaffUpload.CssClass = If((ButtonStaffUpload.Enabled), "ButtonStaffUpload", "ButtonStaffUpload ButtonDisable")
            ButtonScheduleUpload.CssClass = If((ButtonScheduleUpload.Enabled), "ButtonScheduleUpload", "ButtonScheduleUpload ButtonDisable")
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

            LabelTurboPASMedSysUpload.Text = Convert.ToString(ResourceTable("LabelTurboPASMedSysUpload"), Nothing)
            LabelTurboPASMedSysUpload.Text = If(String.IsNullOrEmpty(LabelTurboPASMedSysUpload.Text), "TurboPAS-Med Sys Upload", LabelTurboPASMedSysUpload.Text)

            CheckBoxIndividualUpload.Text = Convert.ToString(ResourceTable("CheckBoxIndividualUpload"), Nothing)
            CheckBoxIndividualUpload.Text = If(String.IsNullOrEmpty(CheckBoxIndividualUpload.Text), "Individual Upload", CheckBoxIndividualUpload.Text)

            ButtonClientUpload.Text = Convert.ToString(ResourceTable("ButtonClientUpload"), Nothing)
            ButtonClientUpload.Text = If(String.IsNullOrEmpty(ButtonClientUpload.Text), "Client Upload", ButtonClientUpload.Text)

            ButtonAuthorizationUpload.Text = Convert.ToString(ResourceTable("ButtonAuthorizationUpload"), Nothing)
            ButtonAuthorizationUpload.Text = If(String.IsNullOrEmpty(ButtonAuthorizationUpload.Text), "Authorization Upload", ButtonAuthorizationUpload.Text)

            ButtonStaffUpload.Text = Convert.ToString(ResourceTable("ButtonStaffUpload"), Nothing)
            ButtonStaffUpload.Text = If(String.IsNullOrEmpty(ButtonStaffUpload.Text), "Staff Upload", ButtonStaffUpload.Text)

            ButtonScheduleUpload.Text = Convert.ToString(ResourceTable("ButtonScheduleUpload"), Nothing)
            ButtonScheduleUpload.Text = If(String.IsNullOrEmpty(ButtonScheduleUpload.Text), "Schedule Upload", ButtonScheduleUpload.Text)

            ButtonUploadAll.Text = Convert.ToString(ResourceTable("ButtonUploadAll"), Nothing)
            ButtonUploadAll.Text = If(String.IsNullOrEmpty(ButtonUploadAll.Text), "Upload All", ButtonUploadAll.Text)

            ButtonDownload.Text = Convert.ToString(ResourceTable("ButtonDownload"), Nothing)
            ButtonDownload.Text = If(String.IsNullOrEmpty(ButtonDownload.Text), "Download", ButtonDownload.Text)

            ResourceTable = Nothing

        End Sub

    End Class
End Namespace