Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject

Namespace Visitel
    Public Class LoginPageControl
        Inherits SharedWebControls

        Protected MembershipUser As MembershipUser
        Protected UserNameId As String
        Protected PasswordId As String
        Protected ButtonLoginId As String
        'Private cfgDA As New GlobalConfigurationDA()
        Private ControlName As String

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            ControlName = "LoginPageControl"
            ConnectionOpen()
            ''LoadCss("dummy");
            ''SetTitle("dummy");
            ''LoadJS("dummy");
            InitializeControl()
            GetCaptionFromResource()

        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            '#region Dummy code for auto login
            'UserInfoDA usrDA = new UserInfoDA();
            'HttpContext.Current.Session[StaticSettings.SessionField.USER_ID] = 1;
            'HttpContext.Current.Session[StaticSettings.SessionField.ROLE_ID] = 1;
            'HttpContext.Current.Session[StaticSettings.SessionField.USER_NAME] = "Anjan";
            'IList<UserInfo> ui = usrDA.UserInfoGetByUserName("Anjan");
            'UserPrivilegeDA UPDA = new UserPrivilegeDA();
            'List<UserPrivilege> userPriv = (List<UserPrivilege>)UPDA.UserPrivilegeGet(ui[0].RoleID);
            'Session[StaticSettings.SessionField.PRIVILEGE_COLLECTION] = userPriv;
            'HttpContext.Current.Response.Redirect("Default.aspx", true);
            '#endregion

            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1500))
            Response.Cache.SetNoStore()

            Dim gConfig As New GlobalConfigurationDataObject()

            divErrMesg.Visible = False

            Dim TextBoxUserName As TextBox = DirectCast(Login1.FindControl("TextBoxUserName"), TextBox)
            Dim TextBoxPassword As TextBox = DirectCast(Login1.FindControl("TextBoxPassword"), TextBox)
            Dim ButtonLogin As Button = DirectCast(Login1.FindControl("ButtonLogin"), Button)

            ButtonLoginId = ButtonLogin.UniqueID
            UserNameId = TextBoxUserName.UniqueID
            PasswordId = TextBoxPassword.UniqueID

            If Request.QueryString("op") IsNot Nothing AndAlso Request.QueryString("op") = "Logout" Then
                SignOut()
            End If

            If Session(StaticSettings.SessionField.USER_ID) IsNot Nothing Then
                Response.Redirect("Default.aspx")
            End If

            Me.Page.Title = GlobalConfig.LoginPageTitle & " : " & " Login"

        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)

            LoadCss("Login/" & ControlName)

            LoadJS("JavaScript/jquery-1.10.2.js")
            LoadJS("JavaScript/jquery-ui.js")
            LoadJS("JavaScript/easySlider1.5.js")

            LoadJS("JavaScript/Login/" & ControlName & ".js")

        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            ConnectionClose()
        End Sub


        ''' <summary>
        ''' Signout user
        ''' </summary>
        Private Sub SignOut()

            Dim UserName As String = Convert.ToString(Session(StaticSettings.SessionField.USER_NAME))
            Dim CacheKey As String = UserName & UserName
            HttpContext.Current.Cache.Remove(CacheKey)
            FormsAuthentication.SignOut()
            Session.Abandon()

            ' clear authentication cookie
            Dim Cookie1 As New HttpCookie(FormsAuthentication.FormsCookieName, "")
            Cookie1.Expires = DateTime.Now.AddYears(-1)
            Response.Cookies.Add(Cookie1)

            Dim Cookie2 As New HttpCookie("ASP.NET_SessionId", "")
            Cookie2.Expires = DateTime.Now.AddYears(-1)
            Response.Cookies.Add(Cookie2)

            Dim nextpage As String = ResolveUrl("~") + "LoginPage.aspx"
            Response.Write("<script language=""javascript"">")
            Response.Write("{")
            Response.Write(" var Backlen=history.length;")
            Response.Write(" history.go(-Backlen);")
            Response.Write((Convert.ToString(" window.location.href='") & nextpage) + "'; ")
            Response.Write("}")
            Response.Write("</script>")

        End Sub

        Private Sub ButtonLogin_OnClick(sender As Object, e As EventArgs)

            'Response.Redirect("CreateEditViewUser.aspx?UserName=anjan&Mode=Insert")

            'Session(StaticSettings.SessionField.USER_ID) = 100
            'Session(StaticSettings.SessionField.COMPANY_ID) = 1
            'Session(StaticSettings.SessionField.COMPANY_NAME) = "KDS HEALTHCARE COMPANY"

            'HttpContext.Current.Response.Redirect("Default.aspx", False)

            Dim objUserInfoDataObject As UserInfoDataObject
            Dim objBLUserInfo As BLUserInfo
            Dim objBLUserPrivilege As BLUserPrivilege
            Dim UserPrivilege As List(Of UserPrivilegeDataObject)
            Dim objPassword As BLPassword

            Try
                objBLUserInfo = New BLUserInfo()
                objUserInfoDataObject = objBLUserInfo.UserInfoGetByUserName(ConVisitel, TextBoxUserName.Text)

                If (Not objUserInfoDataObject Is Nothing) Then
                    If (objUserInfoDataObject.IsLocked) Then
                        FailureText.Text = "Your account has been locked. Contact with IT HELP LINE."
                        Return
                    End If
                    If (Not objUserInfoDataObject.IsActive) Then
                        FailureText.Text = "Your account is not active. Contact with IT HELP LINE."
                        Return
                    End If
                    If (Not objUserInfoDataObject.IsApproved) Then
                        FailureText.Text = "Your account is not yet approved. Contact with IT HELP LINE."
                        Return
                    End If

                    objPassword = New BLPassword(TextBoxPassword.Text, Convert.ToInt32(objUserInfoDataObject.PasswordSalt))

                    Dim SaltedHash As String = objPassword.ComputeSaltedHash()

                    If (SaltedHash.Equals(objUserInfoDataObject.SaltedHash)) Then
                        'If (1) Then
                        Dim SessionId As String = Session.SessionID
                        Dim Ip As String

                        Ip = Request.ServerVariables("HTTP_X_FORWARDED_FOR")

                        If Ip = String.Empty OrElse Ip Is Nothing Then
                            Ip = Request.ServerVariables("REMOTE_ADDR")
                        End If

                        If Ip Is Nothing Then
                            Ip = String.Empty
                        End If

                        'Read user privilege from database and store it in session

                        objBLUserPrivilege = New BLUserPrivilege()
                        UserPrivilege = objBLUserPrivilege.UserPrivilegeGetByUserId(ConVisitel, objUserInfoDataObject.UserId)

                        Session(StaticSettings.SessionField.PRIVILEGE_COLLECTION) = UserPrivilege

                        Session(StaticSettings.SessionField.USER_ID) = objUserInfoDataObject.UserId
                        Session(StaticSettings.SessionField.ROLE_ID) = objUserInfoDataObject.RoleId
                        Session(StaticSettings.SessionField.USER_NAME) = objUserInfoDataObject.UserName
                        Session(StaticSettings.SessionField.USER_FULL_NAME) = objUserInfoDataObject.FullName
                        Session(StaticSettings.SessionField.COMPANY_ID) = objUserInfoDataObject.CompanyId
                        Session(StaticSettings.SessionField.COMPANY_NAME) = objUserInfoDataObject.CompanyName

                        If (CheckMultipleLogin()) Then
                            Return
                        End If

                        objBLUserInfo.LoginHistoryInsert(ConVisitel, objUserInfoDataObject.UserId, SessionId, DateTime.Now, Session.Timeout, Ip)

                        'Dim RCDA As New ReferanceCodesDA()
                        'Dim Rcode As IList(Of ReferanceCodes)

                        'If usrColl(0).ThemeName <> "0" Then
                        '    Rcode = RCDA.ReferanceCodesGetByID(Convert.ToInt32(usrColl(0).ThemeName))
                        '    HttpContext.Current.Session(StaticSettings.SessionField.USER_PREF_THEME) = Rcode(0).Code
                        'End If

                        'If usrColl(0).ProductTitle <> "0" Then
                        '    Rcode = RCDA.ReferanceCodesGetByID(Convert.ToInt32(usrColl(0).ProductTitle))
                        '    HttpContext.Current.Session(StaticSettings.SessionField.USER_PROJECT_NAME) = Rcode(0).Code
                        'End If

                        objBLUserInfo.LastLoginDateUpdate(ConVisitel, objUserInfoDataObject.UserId)

                        HttpContext.Current.Response.Redirect("Default.aspx", False)

                    Else
                        FailureText.Text = "Invalid User Name/Password."

                        If GlobalUtil.MinutesDifference(DateTime.Now, If(String.IsNullOrEmpty(objUserInfoDataObject.FailedPasswordAttemptWindowStart),
                                                                         DateTime.Now, objUserInfoDataObject.FailedPasswordAttemptWindowStart)) _
                                        > GlobalConfig.InvalidPasswordToleranceTimeInMinutes Then
                            objBLUserInfo.FailedPasswordAttemptWindowStartUpdate(ConVisitel, objUserInfoDataObject.UserId)
                        Else
                            If objUserInfoDataObject.FailedPasswordAttemptCount + 1 > GlobalConfig.maxInvalidPasswordAttempts Then
                                objBLUserInfo.LockUser(ConVisitel, objUserInfoDataObject.UserId)
                                FailureText.Text = "Your account has been locked. Contact with IT-HELP LINE."
                            Else
                                objBLUserInfo.FailedPasswordAttemptCountIncrease(ConVisitel, objUserInfoDataObject.UserId)
                            End If
                        End If

                    End If
                Else
                    FailureText.Text = "Invalid User Name"
                End If

            Catch ex As Exception
                divErrMesg.Visible = True
                lblErrMsg.Text = "The system encountered some problems. Please try again later. If the problem continues contact with the IT-HELP LINE."
            Finally
                objUserInfoDataObject = Nothing
                objBLUserInfo = Nothing
                objBLUserPrivilege = Nothing
                UserPrivilege = Nothing
                objPassword = Nothing
            End Try
        End Sub

        Protected Function CheckMultipleLogin() As Boolean
            Dim UserName As String = Convert.ToString(Session(StaticSettings.SessionField.USER_NAME))
            Dim CacheKey As String = UserName & UserName
            Dim User As String = Convert.ToString(Cache(CacheKey))
            If User Is Nothing OrElse User = [String].Empty Then
                Dim SessionTimeOut As New TimeSpan(0, 0, HttpContext.Current.Session.Timeout, 0, 0)
                HttpContext.Current.Cache.Insert(CacheKey, CacheKey, Nothing, DateTime.MaxValue, SessionTimeOut, System.Web.Caching.CacheItemPriority.Normal, Nothing)
                Return False
            Else
                Session.Abandon()
                FailureText.Text = "This user is already logged-in."
                Return True
            End If
        End Function

        Public Property GlobalConfig() As GlobalConfigurationDataObject
            Get
                Dim gConfig As New GlobalConfigurationDataObject()

                If Cache("GlobalConfig") Is Nothing Then
                    'Dim cfgDA As New GlobalConfigurationDA()

                    'gConfig = cfgDA.GlobalConfigurationGetAll()

                    Cache("GlobalConfig") = gConfig
                Else
                    gConfig = DirectCast(Cache("GlobalConfig"), GlobalConfigurationDataObject)
                End If

                Return gConfig
            End Get
            Set(value As GlobalConfigurationDataObject)
                Cache("GlobalConfig") = value
            End Set
        End Property

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()

            If (Request.QueryString("op") Is Nothing) Then
                Session.Add(StaticSettings.SessionField.PRIVILEGE_COLLECTION, Nothing)
                Session.Add(StaticSettings.SessionField.USER_ID, Nothing)
                Session.Add(StaticSettings.SessionField.ROLE_ID, Nothing)
                Session.Add(StaticSettings.SessionField.USER_NAME, Nothing)
                Session.Add(StaticSettings.SessionField.USER_FULL_NAME, Nothing)
                Session.Add(StaticSettings.SessionField.COMPANY_ID, Nothing)
                Session.Add(StaticSettings.SessionField.COMPANY_NAME, Nothing)
            End If

            AddHandler ButtonLogin.Click, AddressOf ButtonLogin_OnClick
        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()
            GetResources("dummy", "dummy")
        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData() As Boolean
            Dim IsValid As Boolean = False
            '''Put Code Here
            Return IsValid
        End Function

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Private Sub GetData()

        End Sub

        Private Sub ClearControl()

        End Sub



    End Class
End Namespace
