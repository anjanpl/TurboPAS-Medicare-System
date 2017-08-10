
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Globalization
Imports System.IO
Imports System.Collections
Imports System.Security.Permissions
Imports System.Resources
Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration
Imports System.Diagnostics
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelBusiness.VisitelBusiness
Imports System.Web.UI.WebControls
Imports System.Text.RegularExpressions
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports System.Collections.Generic
Imports System.Web
Imports System.Reflection
Imports System.Web.Security

Namespace VisitelCommon
    Public Class SharedWebControls
        Inherits System.Web.UI.Page

        Public ConVisitel As SqlConnection
        Public VisitelConnectionString As String


        Public DateFormat As String = "M/d/yy" '"MMMM d, yyyy"  "M/d/yy"

        Public CalendarDateFormat As String = "mm/dd/yy" '"m/d/y" '"MM dd, yy" "m/d/y"

        Public DateFormatWithTime12Hour As String = "MM/dd/yyyy hh:mm:ss tt"

        Public HourMinuteValidationExpression As String = "([01]?[0-9]|2[0-3]):[0-5][0-9]" 'Format: HH:MM
        Public TimeValidationExpression As String = "^(?:(?:([0-9]?\d|2[0-9]):)?([0-5]?\d):)?([0-5]?\d)$" 'Format: HH:MM:SS
        Public TimeValidationWithAMPMExpression As String = "^(0?[1-9]|1[012])(:[0-5]\d) [APap][mM]$" 'Format: HH:MM AM|PM
        Public DecimalValueWithDollarSign As String = "^\$?[0-9]\d{0,9}(\.\d{1,2})?%?$"
        'Public SocialSecurityNumberValidationExpression As String = "^\d{3}-?\d{2}-?\d{4}$"
        Public SocialSecurityNumberValidationExpression As String = "^\d{3}-?\d{2}-?\d{4}$"
        Public PhoneValidationExpression As String = "^\(?(\d{3})\)?[- ]?(\d{3})[- ]?(\d{4})$"
        'Public PhoneValidationExpression As String = "^\(?(\d{3})\)?[- ]?(\d{2})[- ]?(\d{4})$"
        Public ZipCodeValidationExpression As String = "^\d{5}$"

        'Public DateValidationExpression As String = "^(?:(((Jan(uary)?|Ma(r(ch)?|y)|Jul(y)?|Aug(ust)?|Oct(ober)?|Dec(ember)?)\ 31)|((Jan(uary)?|Ma(r(ch)?|y)" _
        '                                            & "|Apr(il)?|Ju((ly?)|(ne?))|Aug(ust)?|Oct(ober)?|(Sept|Nov|Dec)(ember)?)\ (0?[1-9]|([12]\d)|30))" _
        '                                            & "|(Feb(ruary)?\ (0?[1-9]|1\d|2[0-8]|(29(?=,\ ((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])" _
        '                                            & "|((16|[2468][048]|[3579][26])00)))))))\,\ ((1[6-9]|[2-9]\d)\d{2}))"

        Public DateValidationExpression As String = "^(?:(?:(?:0?[13578]|1[02])(\/|-|\.)31)\1|(?:(?:0?[13-9]|1[0-2])(\/|-|\.)(?:29|30)\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$" _
                                                     & "|^(?:0?2(\/|-|\.)29\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$" _
                                                     & "|^(?:(?:0?[1-9])|(?:1[0-2]))(\/|-|\.)(?:0?[1-9]|1\d|2[0-8])\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$" 'Does Support 10/10/2015 & 10/10/15

        Public GenericControlRow As String = "div", GenericControlColumn As String = "div"

        Protected divInfoMsg As HtmlGenericControl, lblMessage As Label, divErrMesg As HtmlGenericControl, lblErrMsg As Label

        Protected Overrides Sub OnPreInit(e As EventArgs)

            'Page.Theme = "ui-darkness"

            Page.Theme = "redmond"

            'Page.Theme = "blitzer"

            Return

            Try
                If (Session(StaticSettings.SessionField.USER_PREF_THEME) Is Nothing) Then
                    If ((Not Request.Cookies("theme") Is Nothing) And (Not Request.Cookies("theme").Value.Length.Equals(0))) Then
                        Page.Theme = Request.Cookies("theme").Value.ToLower()
                    Else
                        Page.Theme = "redmond"
                    End If

                Else
                    Me.Theme = Session(StaticSettings.SessionField.USER_PREF_THEME).ToString()
                End If
            Catch
                Page.Theme = "redmond"
            Finally

                Response.Buffer = True
                Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
                Response.Expires = -1500
                Response.CacheControl = "no-cache"

                If (Request.Url.AbsoluteUri.ToLower().IndexOf("LoginPage.aspx") < 0) Then
                    If (Session(StaticSettings.SessionField.USER_PREF_THEME) Is Nothing) Then
                        Response.Redirect("~/LoginPage.aspx")
                    ElseIf (Convert.ToInt32(Session(StaticSettings.SessionField.USER_ID)).Equals(0)) Then
                        Response.Redirect("~/LoginPage.aspx")
                    End If
                End If

            End Try

        End Sub

        Private m_PermissionName As String
        Public Property PermissionName() As String
            Get
                Return m_PermissionName
            End Get
            Set(value As String)
                m_PermissionName = Value
            End Set
        End Property


        Public Sub PageInitialize()

            Dim PermissionList As List(Of UserPrivilegeDataObject) = DirectCast(Session(StaticSettings.SessionField.PRIVILEGE_COLLECTION), List(Of UserPrivilegeDataObject))

            If PermissionList IsNot Nothing AndAlso PermissionList.Count > 0 Then
                Dim item = PermissionList.FirstOrDefault(Function(i) i.PermissionName = Me.PermissionName And i.HasPermission = True)

                If item IsNot Nothing Then
                    'Me.IsAdd = item.CanInsert
                    'Me.IsEdit = item.CanEdit
                    'Me.IsDelete = item.CanDelete
                    'Me.IsView = True
                    'Me.IsProcess = item.CanInsert
                    'Me.IsPullDataFromE2E = item.CanInsert

                    'If IsAdd OrElse IsEdit Then
                    '    CRUD.VisibleSaveButton = InlineAssignHelper(CRUD.EnableSaveButton, True)
                    '    CRUD.EnablePullDataE2EButton = InlineAssignHelper(CRUD.VisiblePullDataE2EButton, True)
                    '    CRUD.EnableProcessButton = InlineAssignHelper(CRUD.VisibleProcessButton, True)
                    'End If
                    'CRUD.VisibleAddButton = IsAdd
                    'CRUD.VisibleDeleteButton = IsDelete
                    'CRUD.VisibleExportButton = IsView
                    'CRUD.VisibleProcessButton = IsProcess
                    'CRUD.VisiableReportShowButton = True


                    'Me.RemitCRUD = CRUD
                    'ScriptManager.GetCurrent(Me.Page).RegisterPostBackControl(CRUD.ExportFile)
                Else
                    'CacheUtility.ClearCacheForUser(SessionUtility.UserLoginID);
                    'SessionUtility.ClearSession();
                    'FormsAuthentication.SignOut();
                    'Response.Redirect(FormsAuthentication.DefaultUrl, true);
                    'HttpContext.Current.Response.Redirect("UnauthorizedAccess.aspx", false);
                    'HttpContext.Current.Response.Redirect(Server.MapPath("~/UnauthorizedAccess"), false);
                    Response.Redirect("~/UnauthorizedAccess.aspx")
                End If
            Else
                'If user give hard coded url to open page , system will go to logout

                'CacheUtility.ClearCacheForUser(SessionUtility.UserLoginID);
                'SessionUtility.ClearSession();
                'FormsAuthentication.SignOut();
                'Response.Redirect(FormsAuthentication.DefaultUrl, true);
                'HttpContext.Current.Response.Redirect("~UnauthorizedAccess.aspx", false);
                'HttpContext.Current.Response.Redirect(Server.MapPath("~/UnauthorizedAccess"), false);

                Dim sUserName As String = Convert.ToString(Session(StaticSettings.SessionField.USER_NAME))
                Dim sKey As String = sUserName & sUserName
                HttpContext.Current.Cache.Remove(sKey)
                FormsAuthentication.SignOut()
                Session.Abandon()
                ' clear authentication cookie
                Dim cookie1 As New HttpCookie(FormsAuthentication.FormsCookieName, "")
                cookie1.Expires = DateTime.Now.AddYears(-1)
                Response.Cookies.Add(cookie1)

                Dim cookie2 As New HttpCookie("ASP.NET_SessionId", "")
                cookie2.Expires = DateTime.Now.AddYears(-1)
                Response.Cookies.Add(cookie2)
                Response.Redirect("~/LoginPage.aspx")
            End If

        End Sub

        Public Overridable ReadOnly Property StyleSheetTheme() As [String]

            Get
                Try
                    If Session(StaticSettings.SessionField.USER_PREF_THEME) Is Nothing Then
                        If (Request.Cookies("theme") IsNot Nothing) AndAlso (Request.Cookies("theme").Value.Length <> 0) Then
                            Return Request.Cookies("theme").Value.ToLower()
                        Else
                            Return "redmond"
                        End If
                    Else
                        Return Session(StaticSettings.SessionField.USER_PREF_THEME).ToString().ToLower()
                    End If
                Catch
                    Return "redmond"
                End Try
            End Get
        End Property

        ''' <summary>
        ''' This is for database connection openning
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ConnectionOpen()
            Try
                GetConnectionString()
                If Not String.IsNullOrEmpty(VisitelConnectionString) Then
                    ConVisitel = New SqlConnection(VisitelConnectionString)

                    If ConVisitel.State.Equals(ConnectionState.Closed) Then
                        ConVisitel.Open()
                    End If
                End If
            Catch ex As SqlException
                System.Web.HttpContext.Current.Response.Redirect("~/ConnectionError.html")

                'Response.Redirect("~/ConnectionError.html")
            End Try

        End Sub

        ''' <summary>
        ''' This is for database connection closing
        ''' </summary>
        Public Sub ConnectionClose()
            Try
                If ConVisitel.State.Equals(ConnectionState.Open) Then
                    ConVisitel.Close()
                    ConVisitel.Dispose()
                    ConVisitel = Nothing
                End If

            Catch ex As Exception
            End Try

        End Sub

        Private Sub GetConnectionString()
            VisitelConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnString").ConnectionString

        End Sub

        Protected Sub EncryptConnectionString(Encrypt As Boolean, FileName As String)
            Dim configuration As Configuration = Nothing
            Try
                ' Open the configuration file and retrieve the connectionStrings section.
                configuration = ConfigurationManager.OpenExeConfiguration(FileName)
                Dim configSection As ConnectionStringsSection = DirectCast(configuration.GetSection("connectionStrings"), ConnectionStringsSection)
                If (Not (configSection.ElementInformation.IsLocked)) AndAlso (Not (configSection.SectionInformation.IsLocked)) Then
                    If Encrypt AndAlso Not configSection.SectionInformation.IsProtected Then
                        'this line will Encrypt the file
                        configSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider")
                    End If

                    If Not Encrypt AndAlso configSection.SectionInformation.IsProtected Then
                        'Encrypt is true so Encrypt
                        'if (configSection.SectionInformation.IsProtected)//Encrypt is true so Encrypt
                        'this line will decrypt the file. 
                        configSection.SectionInformation.UnprotectSection()
                    End If
                    're-save the configuration file section
                    configSection.SectionInformation.ForceSave = True
                    ' Save the current configuration

                    configuration.Save()
                    'configFile.FilePath 
                    Process.Start("notepad.exe", configuration.FilePath)
                End If
                'Response.Write(ex.Message);
            Catch ex As Exception
            End Try
        End Sub


        ''' <summary>
        ''' Display info message at the top of the body
        ''' </summary>
        ''' <param name="pstrMsg"></param>
        Public Sub DisplayHeaderMessage(pstrMsg As String)
            ClearMessages()

            divInfoMsg.Visible = True
            lblMessage.Text = pstrMsg

            'upnlMsg.Update()
        End Sub

        ''' <summary>
        ''' Clear message at the top of the body
        ''' </summary>
        ''' <param name="pstrMsg"></param>
        Public Sub ClearMessages()
            divInfoMsg.Visible = False
            lblMessage.Text = ""

            divErrMesg.Visible = False
            lblErrMsg.Text = ""
        End Sub


        ''' <summary>
        ''' Displays custom alert from server end.; Added By: Anjan, Date: 09062014
        ''' </summary>
        ''' <param name="Msg"> Message which will be displayed as alert</param>
        Public Sub ShowClientMessage(Msg As String)
            If Msg <> "" Then
                ' Define the JavaScript function for the specified control.
                Dim focusScript As String = (Convert.ToString("<script language='javascript'>" + "alert('") & Msg) + "')</script>"

                ' Add the JavaScript code to the page.
                Page.RegisterStartupScript("Message Alert", focusScript)
            End If
        End Sub

        Public Function GetCultureInfo() As CultureInfo
            Dim webCultureInfo1 As New CultureInfo("en-us")

            Return webCultureInfo1
        End Function

        ''' <summary>
        ''' Loading css file
        ''' </summary>
        ''' <param name="fileName"></param>
        Public Sub LoadCss(FileName As String)
            Dim css As New HtmlLink()
            css.Href = ResolveClientUrl((Convert.ToString("Css/") & FileName) + ".css")
            css.Attributes("type") = "text/css"
            css.Attributes("rel") = "stylesheet"
            css.Attributes("media") = "screen"

            Page.Header.Controls.Add(css)

        End Sub

        ''' <summary>
        ''' Setting page title
        ''' </summary>
        Protected Sub SetTitle(Title As String)
            Me.Title = Title
        End Sub

        ''' <summary>
        ''' Loading javascript file
        ''' </summary>
        ''' <param name="fileName"></param>
        Public Sub LoadJS(FileName As String)

            Dim myJs As New HtmlGenericControl()
            myJs.TagName = "script"
            myJs.Attributes.Add("type", "text/javascript")
            myJs.Attributes.Add("language", "javascript")
            myJs.Attributes.Add("src", ResolveUrl(Convert.ToString("~/") & FileName))

            Page.Header.Controls.Add(myJs)

        End Sub

        ''' <summary>
        ''' Loading javascript file with specified directory
        ''' </summary>
        ''' <param name="directoryName"></param>
        ''' <param name="fileName"></param>
        Public Sub LoadJS(DirectoryName As String, FileName As String)

            Dim myJs As New HtmlGenericControl()
            myJs.TagName = "script"
            myJs.Attributes.Add("type", "text/javascript")
            myJs.Attributes.Add("language", "javascript")
            myJs.Attributes.Add("src", ResolveUrl(Convert.ToString((Convert.ToString("~/") & DirectoryName) + "/") & FileName))

            Page.Header.Controls.Add(myJs)

        End Sub

        ''' <summary>
        ''' Read the resource file and put into hashtable
        ''' </summary>
        ''' <param name="folder"></param>
        ''' <param name="resourceFile"></param>
        ''' <returns></returns>
        <PermissionSetAttribute(SecurityAction.Demand, Name:="FullTrust")> _
        Public Function GetResourceValue(Folder As [String], ResourceFile As [String]) As Hashtable
            Dim resourceFilePath As String = String.Empty

            resourceFilePath = Server.MapPath(GetResources(Folder, ResourceFile))

            Dim resourceTable As New Hashtable()
            If (File.Exists(resourceFilePath)) Then
                Dim reader As New ResXResourceReader(resourceFilePath)
                For Each d As DictionaryEntry In reader
                    resourceTable.Add(d.Key.ToString(), d.Value.ToString())
                Next
                reader.Close()
            End If
            Return resourceTable
        End Function

        Public Function AddControls(index As Integer, ControlType As EnumDataObject.ControlType, ByRef newRow As HtmlGenericControl, ControlId As String, ControlCss As String,
                                       IsNewColumn As Boolean, ColumnCss As String) As Control
            Dim Control As Control = Nothing

            Select Case ControlType
                Case EnumDataObject.ControlType.Label
                    Control = AddLabel(index, newRow, ControlId, ControlCss, IsNewColumn, ColumnCss)
                    Exit Select

                Case EnumDataObject.ControlType.TextBox
                    Control = AddTextBox(index, newRow, ControlId, ControlCss, IsNewColumn, ColumnCss, False, Nothing, String.Empty)
                    Exit Select

                Case EnumDataObject.ControlType.DropDownList
                    Control = AddDropDownList(index, newRow, ControlId, ControlCss, IsNewColumn, ColumnCss)
                    Exit Select

                Case EnumDataObject.ControlType.CheckBox
                    Control = AddCheckBox(index, newRow, ControlId, ControlCss, IsNewColumn, ColumnCss)
                    Exit Select
            End Select

            Return Control

        End Function

        Public Function AddColumn(index As Integer, ColumnId As String, ColumnCss As String, Optional ColumnText As String = "") As HtmlGenericControl
            Dim NewColumn As HtmlGenericControl = Nothing

            NewColumn = New HtmlGenericControl(GenericControlColumn)
            NewColumn.Attributes.Add("class", ColumnCss)
            NewColumn.ID = GenericControlColumn & ColumnId & "_" & index
            NewColumn.InnerHtml = ColumnText

            Return NewColumn
        End Function

        Public Function AddLabel(index As Integer, ByRef newRow As HtmlGenericControl, ControlId As String, ControlCss As String, IsNewColumn As Boolean, ColumnCss As String) As Label

            Dim LabelControl As Label = New Label()
            LabelControl.ID = ControlId & "_" & index

            If (Not String.IsNullOrEmpty(ControlCss)) Then
                LabelControl.CssClass = ControlCss
            End If

            Dim NewColumn As HtmlGenericControl = Nothing

            If (IsNewColumn) Then
                NewColumn = AddColumn(index, ControlId, ColumnCss)
                NewColumn.Controls.Add(LabelControl)
            End If

            If (IsNewColumn) Then
                newRow.Controls.Add(NewColumn)
            Else
                newRow.Controls.Add(LabelControl)
            End If

            Return LabelControl

        End Function

        Public Function AddTextBox(index As Integer, ByRef newRow As HtmlGenericControl, ControlId As String, ControlCss As String, IsNewColumn As Boolean, ColumnCss As String,
                                    IsRequiredField As Boolean, ByRef RequiredFieldValidatorControl As RequiredFieldValidator,
                                    RequiredFieldValidatorControlId As String, Optional ByVal ValidationGroup As String = "",
                                    Optional EnableRequiredFieldValidator As Boolean = False) As TextBox

            Dim TextBoxControl As TextBox = New TextBox()
            TextBoxControl.ID = ControlId & "_" & index

            If (Not String.IsNullOrEmpty(ControlCss)) Then
                TextBoxControl.CssClass = ControlCss
            End If

            Dim NewColumn As HtmlGenericControl = Nothing

            If (IsNewColumn) Then
                NewColumn = AddColumn(index, ControlId, ColumnCss)
                NewColumn.Controls.Add(TextBoxControl)
            End If

            If (IsRequiredField) Then
                If (IsNewColumn) Then
                    RequiredFieldValidatorControl = AddRequiredFieldValidator(index, NewColumn, RequiredFieldValidatorControlId,
                                                                          TextBoxControl.ID, ValidationGroup, EnableRequiredFieldValidator)
                Else
                    RequiredFieldValidatorControl = AddRequiredFieldValidator(index, newRow, RequiredFieldValidatorControlId,
                                                                          TextBoxControl.ID, ValidationGroup, EnableRequiredFieldValidator)
                End If
            End If

            If (IsNewColumn) Then
                newRow.Controls.Add(NewColumn)
            Else
                newRow.Controls.Add(TextBoxControl)
            End If

            Return TextBoxControl

        End Function

        Public Function AddDropDownList(index As Integer, ByRef newRow As HtmlGenericControl, ControlId As String, ControlCss As String, IsNewColumn As Boolean, ColumnCss As String) As DropDownList

            Dim DropDownListControl As DropDownList = New DropDownList()

            DropDownListControl.ID = ControlId & "_" & index

            If (Not String.IsNullOrEmpty(ControlCss)) Then
                DropDownListControl.CssClass = ControlCss
            End If

            Dim NewColumn As HtmlGenericControl = Nothing

            If (IsNewColumn) Then
                NewColumn = AddColumn(index, ControlId, ColumnCss)
                NewColumn.Controls.Add(DropDownListControl)
            End If

            If (IsNewColumn) Then
                newRow.Controls.Add(NewColumn)
            Else
                newRow.Controls.Add(DropDownListControl)
            End If

            Return DropDownListControl

        End Function

        Public Function AddCheckBox(index As Integer, ByRef newRow As HtmlGenericControl, ControlId As String, ControlCss As String, IsNewColumn As Boolean, ColumnCss As String, Optional IsChecked As Boolean = False) As CheckBox

            Dim CheckBoxControl As CheckBox = New CheckBox()
            CheckBoxControl.ID = ControlId & "_" & index
            CheckBoxControl.Checked = IsChecked

            If (Not String.IsNullOrEmpty(ControlCss)) Then
                CheckBoxControl.CssClass = ControlCss
            End If

            Dim NewColumn As HtmlGenericControl = Nothing

            If (IsNewColumn) Then
                NewColumn = AddColumn(index, ControlId, ColumnCss)
                NewColumn.Controls.Add(CheckBoxControl)
            End If

            If (IsNewColumn) Then
                newRow.Controls.Add(NewColumn)
            Else
                newRow.Controls.Add(CheckBoxControl)
            End If

            Return CheckBoxControl

        End Function

        Public Function AddRequiredFieldValidator(index As Integer, ByRef ContainerControl As HtmlGenericControl, ControlId As String,
                                    ControlToValidateId As String, Optional ByVal ValidationGroup As String = "",
                                    Optional IsEnable As Boolean = False) As RequiredFieldValidator

            Dim RequiredFieldValidatorControl As RequiredFieldValidator = New RequiredFieldValidator()
            RequiredFieldValidatorControl.ID = ControlId & "_" & index
            RequiredFieldValidatorControl.ControlToValidate = ControlToValidateId
            RequiredFieldValidatorControl.ErrorMessage = "*"
            RequiredFieldValidatorControl.SetFocusOnError = True
            RequiredFieldValidatorControl.ForeColor = Drawing.Color.Red
            RequiredFieldValidatorControl.Display = ValidatorDisplay.Dynamic
            RequiredFieldValidatorControl.ValidationGroup = ValidationGroup
            RequiredFieldValidatorControl.Enabled = IsEnable

            ContainerControl.Controls.Add(RequiredFieldValidatorControl)
            Return RequiredFieldValidatorControl

        End Function

        Public Function AddHiddenField(index As Integer, ByRef newRow As HtmlGenericControl, ControlId As String, IsNewColumn As Boolean, ColumnCss As String) As HiddenField

            Dim HiddenFieldControl As HiddenField = New HiddenField()
            HiddenFieldControl.ID = ControlId & "_" & index

            Dim NewColumn As HtmlGenericControl = Nothing

            If (IsNewColumn) Then
                NewColumn = AddColumn(index, ControlId, ColumnCss)
                NewColumn.Controls.Add(HiddenFieldControl)
            End If

            If (IsNewColumn) Then
                newRow.Controls.Add(NewColumn)
            Else
                newRow.Controls.Add(HiddenFieldControl)
            End If

            Return HiddenFieldControl

        End Function

        Public Function GetLabel(index As Integer, ContainerControl As Control, ControlId As String) As Label

            Dim LabelControl As Label = Nothing
            LabelControl = DirectCast(ContainerControl.FindControl(ControlId & "_" & index), Label)
            Return LabelControl

        End Function

        Public Function GetLabelValue(index As Integer, ContainerControl As Control, ControlId As String) As String

            Dim LabelControl As Label = GetLabel(index, ContainerControl, ControlId)
            Return If(LabelControl Is Nothing, String.Empty, LabelControl.Text)

        End Function

        Public Function GetTextBox(index As Integer, ContainerControl As Control, ControlId As String) As TextBox

            Dim TextBoxControl As TextBox = Nothing
            TextBoxControl = DirectCast(ContainerControl.FindControl(ControlId & "_" & index), TextBox)
            Return TextBoxControl

        End Function

        Public Function GetTextBoxValue(index As Integer, ContainerControl As Control, ControlId As String) As String

            Dim TextBoxControl As TextBox = GetTextBox(index, ContainerControl, ControlId)
            Return If(TextBoxControl Is Nothing, String.Empty, TextBoxControl.Text)

        End Function

        Public Function GetDropDownList(index As Integer, ContainerControl As Control, ControlId As String) As DropDownList

            Dim DropDownControl As DropDownList = Nothing
            DropDownControl = DirectCast(ContainerControl.FindControl(ControlId & "_" & index), DropDownList)
            Return DropDownControl

        End Function

        Public Function GetCheckBox(index As Integer, ContainerControl As Control, ControlId As String) As CheckBox

            Dim CheckBoxControl As CheckBox = Nothing
            CheckBoxControl = DirectCast(ContainerControl.FindControl(ControlId & "_" & index), CheckBox)
            Return CheckBoxControl

        End Function

        Public Function GetRequiredFieldValidator(index As Integer, ContainerControl As Control, ControlId As String) As RequiredFieldValidator

            Dim RequiredFieldValidatorControl As RequiredFieldValidator = Nothing
            RequiredFieldValidatorControl = DirectCast(ContainerControl.FindControl(ControlId & "_" & index), RequiredFieldValidator)
            Return RequiredFieldValidatorControl

        End Function

        Public Sub EnableRequiredFieldValidator(index As Integer, ContainerControl As Control, ControlId As String, IsEnable As Boolean)

            Dim RequiredFieldValidatorControl As RequiredFieldValidator = GetRequiredFieldValidator(index, ContainerControl, ControlId)
            If RequiredFieldValidatorControl IsNot Nothing Then
                RequiredFieldValidatorControl.Enabled = IsEnable
            End If

        End Sub

        Public Function GetHiddenField(index As Integer, ContainerControl As Control, ControlId As String) As HiddenField

            Dim HiddenFieldControl As HiddenField = Nothing
            HiddenFieldControl = DirectCast(ContainerControl.FindControl(ControlId & "_" & index), HiddenField)
            Return HiddenFieldControl

        End Function

        Public Function GetHiddenFieldValue(index As Integer, ContainerControl As Control, ControlId As String) As String

            Dim HiddenFieldControl As HiddenField = GetHiddenField(index, ContainerControl, ControlId)
            Return If(HiddenFieldControl Is Nothing, String.Empty, HiddenFieldControl.Value)

        End Function

        ''' <summary>
        ''' Get the resource file from here
        ''' </summary>
        ''' <param name="folderName"></param>
        ''' <param name="fileName"></param>
        ''' <returns></returns>
        Protected Function GetResources(FolderName As String, FileName As String) As String
            Dim strResources As String = String.Empty

            If String.IsNullOrEmpty(strResources) Then
                If File.Exists(Server.MapPath(Convert.ToString((Convert.ToString("~/Resource/") & FolderName) + "\") & FileName)) = True Then
                    strResources = Convert.ToString((Convert.ToString("~/Resource/" + "/") & FolderName) + "\") & FileName
                End If
            End If
            Return strResources
        End Function

        <PermissionSetAttribute(SecurityAction.Demand, Name:="FullTrust")> _
        Protected Sub ReadResourceFileContent(FilePath As String, tblResource As Table, PlaceHolderResource As PlaceHolder)
            Dim i As Integer = 1

            If File.Exists(FilePath) Then
                Dim reader As New ResXResourceReader(FilePath)
                Dim id As IDictionaryEnumerator = reader.GetEnumerator()


                Dim tr As TableRow
                Dim tc As TableCell

                For Each d As DictionaryEntry In reader
                    tr = New TableRow()

                    tc = New TableCell()
                    Dim lbl As New Label()
                    lbl.ID = "lblKey" & Convert.ToString(i)
                    lbl.Text = d.Key.ToString()
                    tc.Controls.Add(lbl)
                    tr.Cells.Add(tc)

                    tc = New TableCell()
                    Dim txt As New TextBox()
                    txt.ID = "txtValue" & Convert.ToString(i)
                    txt.Width = Unit.Pixel(250)
                    txt.Text = d.Value.ToString()
                    tc.Controls.Add(txt)
                    tr.Cells.Add(tc)

                    tblResource.Controls.Add(tr)

                    i = i + 1
                Next
                reader.Close()

                PlaceHolderResource.Controls.Add(tblResource)
            End If

        End Sub

        <PermissionSetAttribute(SecurityAction.Demand, Name:="FullTrust")> _
        Protected Sub UpdateResourceFile(path As [String], PlaceHolderResource As PlaceHolder)
            Dim i As Integer = 1
            Dim resourceWriter As New ResXResourceWriter(path)
            For Each ctrl As Control In PlaceHolderResource.Controls
                Dim tbl As Table = DirectCast(ctrl, Table)
                For Each tr As TableRow In tbl.Rows
                    Dim lblKey As Label = DirectCast(tr.FindControl("lblKey" + Convert.ToString(i)), Label)
                    Dim txtBox As TextBox = DirectCast(tr.FindControl("txtValue" + Convert.ToString(i)), TextBox)
                    resourceWriter.AddResource(lblKey.Text.Trim(), txtBox.Text)
                    i = i + 1
                Next
            Next
            resourceWriter.Generate()
            resourceWriter.Close()
        End Sub

        Protected Sub RemoveResourceContent(PlaceHolderResource As PlaceHolder)
            While PlaceHolderResource.Controls.Count > 0
                PlaceHolderResource.Controls.RemoveAt(0)
            End While
        End Sub

        ''' <summary>
        ''' Bind Case Worker Drop Down List
        ''' </summary>
        Public Sub BindCaseWorkerDropDownList(ByRef DropDownListCaseWorker As DropDownList, CompanyId As Integer)

            Dim CaseWorkerList As List(Of CaseWorkerDataObject)

            Dim objBLCaseWorker As New BLCaseWorker()
            CaseWorkerList = objBLCaseWorker.SelectCaseWorker(ConVisitel, CompanyId)
            objBLCaseWorker = Nothing

            DropDownListCaseWorker.DataSource = CaseWorkerList
            DropDownListCaseWorker.DataTextField = "CaseWorkerName"
            DropDownListCaseWorker.DataValueField = "CaseWorkerId"
            DropDownListCaseWorker.DataBind()

            CaseWorkerList = Nothing

            DropDownListCaseWorker.Items.Insert(0, New ListItem("N/A", "-1"))
        End Sub

        ''' <summary>
        ''' Binding City Drop Down List
        ''' </summary>
        Public Sub BindCityDropDownList(ByRef DropDownListCity As DropDownList, CompanyId As Integer)

            Dim CityList As List(Of CityDataObject)

            Dim objBLCity As New BLCity()
            CityList = objBLCity.SelectCity(ConVisitel, CompanyId)
            objBLCity = Nothing

            DropDownListCity.DataSource = CityList
            DropDownListCity.DataTextField = "CityName"
            DropDownListCity.DataValueField = "CityId"
            DropDownListCity.DataBind()

            CityList = Nothing

            'DropDownListCity.Items.Insert(0, New ListItem("Please Select...", "-1"))

            DropDownListCity.Items.Insert(0, New ListItem("", "-1"))

        End Sub

        ''' <summary>
        ''' Binding State Drop Down List
        ''' </summary>
        Public Sub BindStateDropDownList(ByRef DropDownListState As DropDownList, CompanyId As Integer)

            Dim StateList As List(Of StateDataObject)
            Dim objBLState As New BLState
            StateList = objBLState.SelectState(ConVisitel, CompanyId)
            objBLState = Nothing

            DropDownListState.DataSource = StateList
            DropDownListState.DataTextField = "StateFullName"
            DropDownListState.DataValueField = "StateId"
            DropDownListState.DataBind()

            StateList = Nothing

            'DropDownListState.Items.Insert(0, New ListItem("Please Select...", "-1"))

            DropDownListState.Items.Insert(0, New ListItem("", "-1"))
        End Sub

        ''' <summary>
        ''' Binding Status Drop Down List
        ''' </summary>
        Public Sub BindStatusDropDownList(ByRef DropDownListStatus As DropDownList)
            DropDownListStatus.DataSource = EnumDataObject.Enumeration.GetAll(Of EnumDataObject.STATUS)()
            DropDownListStatus.DataTextField = "Value"
            DropDownListStatus.DataValueField = "Key"
            DropDownListStatus.DataBind()

            'DropDownListStatus.Items.Insert(0, New ListItem("Please Select...", "-1"))

            DropDownListStatus.SelectedIndex = 1
        End Sub

        ''' <summary>
        ''' Binding Region Drop Down List
        ''' </summary>
        Public Sub BindRegionDropDownList(ByRef DropDownListRegion As DropDownList)
            DropDownListRegion.DataSource = " 01 02 03 04 05 06 07 08 09 10".Split(" "c)
            DropDownListRegion.DataBind()
            DropDownListRegion.SelectedIndex = 0
        End Sub

        ''' <summary>
        ''' Binding Priority Drop Down List 
        ''' </summary>
        Public Sub BindPriorityDropDownList(ByRef DropDownListPriority As DropDownList)
            DropDownListPriority.DataSource = EnumDataObject.Enumeration.GetAll(Of EnumDataObject.PRIORITY)()
            DropDownListPriority.DataTextField = "Value"
            DropDownListPriority.DataValueField = "Key"
            DropDownListPriority.DataBind()

            DropDownListPriority.Items.Insert(0, New ListItem("Please Select...", "-1"))
        End Sub

        ''' <summary>
        '''  Binding Priority Drop Down List By Description
        ''' </summary>
        ''' <param name="DropDownListPriority"></param>
        ''' <remarks></remarks>
        Protected Sub BindPriorityDropDownListByDescription(ByRef DropDownListPriority As DropDownList)

            DropDownListPriority.DataSource = GetType(EnumDataObject.PRIORITY).ToExtendedList(Of EnumDataObject.PRIORITY)()
            DropDownListPriority.DataTextField = "Value"
            DropDownListPriority.DataValueField = "NumericKey"
            DropDownListPriority.DataBind()

            DropDownListPriority.Items.Insert(0, " ")
            DropDownListPriority.SelectedIndex = 0

        End Sub

        ''' <summary>
        ''' Binding EVV Priority Drop Down List
        ''' </summary>
        Public Sub BindEVVPriorityDropDownList(ByRef DropDownListEVVPriority As DropDownList)
            DropDownListEVVPriority.DataSource = EnumDataObject.Enumeration.GetAll(Of EnumDataObject.EVVPriority)()
            DropDownListEVVPriority.DataTextField = "Key"
            DropDownListEVVPriority.DataValueField = "Key"
            DropDownListEVVPriority.DataBind()

            DropDownListEVVPriority.Items.Insert(0, New ListItem("", "-1"))
        End Sub

        ''' <summary>
        ''' Binding Marital Status Drop Down List
        ''' </summary>
        Public Sub BindMaritalStatusDropDownList(ByRef DropDownListMaritalStatus As DropDownList, CompanyId As Integer)

            Dim MaritalStatusList As List(Of MaritalStatusDataObject)
            Dim objBLMaritalStatus As New BLMaritalStatus
            MaritalStatusList = objBLMaritalStatus.SelectMaritalStatus(ConVisitel, CompanyId)
            objBLMaritalStatus = Nothing

            DropDownListMaritalStatus.DataSource = MaritalStatusList
            DropDownListMaritalStatus.DataTextField = "MaritalStatusName"
            DropDownListMaritalStatus.DataValueField = "MaritalStatusId"
            DropDownListMaritalStatus.DataBind()

            MaritalStatusList = Nothing

            DropDownListMaritalStatus.Items.Insert(0, New ListItem("Please Select...", "-1"))
        End Sub

        ''' <summary>
        ''' Binding Gender Drop Down List
        ''' </summary>
        Public Sub BindGenderDropDownList(ByRef DropDownListGender As DropDownList)
            DropDownListGender.DataSource = EnumDataObject.Enumeration.GetAll(Of EnumDataObject.GENDER)()
            DropDownListGender.DataTextField = "Value"
            DropDownListGender.DataValueField = "Key"
            DropDownListGender.DataBind()

            DropDownListGender.Items.Insert(0, New ListItem("Please Select...", "0"))
        End Sub

        ''' <summary>
        ''' Binding Agent Type Drop Down List
        ''' </summary>
        Public Sub BindAgentTypeDropDownList(ByRef DropDownListAgentType As DropDownList)
            DropDownListAgentType.DataSource = EnumDataObject.Enumeration.GetAll(Of EnumDataObject.EVVName)()
            DropDownListAgentType.DataTextField = "Value"
            DropDownListAgentType.DataValueField = "Key"
            DropDownListAgentType.DataBind()

            DropDownListAgentType.Items.Insert(0, New ListItem("Please Select...", "0"))
        End Sub

        ''' <summary>
        ''' Binding Receiver Drop Down List
        ''' </summary>
        Public Sub BindReceiverDropDownList(ByRef DropDownListReceiver As DropDownList, CompanyId As Integer)

            Dim ReceiverList As List(Of ReceiverDataObject)

            Dim objBLReceiver As New BLReceiver
            ReceiverList = objBLReceiver.SelectReceiver(ConVisitel, CompanyId)
            objBLReceiver = Nothing

            DropDownListReceiver.DataSource = ReceiverList
            DropDownListReceiver.DataTextField = "ReceiverName"
            DropDownListReceiver.DataValueField = "ReceiverId"
            DropDownListReceiver.DataBind()

            ReceiverList = Nothing

            'DropDownListReceiver.Items.Insert(0, New ListItem("Please Select...", "-1"))
            DropDownListReceiver.Items.Insert(0, New ListItem("", "-1"))

        End Sub

        ''' <summary>
        ''' Binding Payer Drop Down List
        ''' </summary>
        ''' <param name="DropDownListPayer"></param>
        ''' <remarks></remarks>
        Public Sub BindPayerDropDownList(ByRef DropDownListPayer As DropDownList)

            Dim PayerList As List(Of PayerDataObject)
            Dim objBLPayer As New BLPayer()
            PayerList = objBLPayer.SelectPayer(ConVisitel)
            objBLPayer = Nothing

            DropDownListPayer.DataSource = PayerList
            DropDownListPayer.DataTextField = "PayerName"
            DropDownListPayer.DataValueField = "PayerId"
            DropDownListPayer.DataBind()

            PayerList = Nothing

            'DropDownListPayer.Items.Insert(0, New ListItem("Please Select...", "-1"))
            DropDownListPayer.Items.Insert(0, New ListItem("", "-1"))
        End Sub

        ''' <summary>
        ''' Binding Client Group Drop Down List
        ''' </summary>
        Public Sub BindClientGroupDropDownList(ByRef DropDownListClientGroup As DropDownList, CompanyId As Integer)

            Dim ClientGroupList As List(Of ClientGroupDataObject)
            Dim objBLClientGroup As New BLClientGroup
            ClientGroupList = objBLClientGroup.SelectClientGroup(ConVisitel, CompanyId)
            objBLClientGroup = Nothing

            DropDownListClientGroup.DataSource = ClientGroupList
            DropDownListClientGroup.DataTextField = "ClientGroupName"
            DropDownListClientGroup.DataValueField = "ClientGroupId"
            DropDownListClientGroup.DataBind()

            ClientGroupList = Nothing

            'DropDownListClientGroup.Items.Insert(0, New ListItem("Please Select...", "-1"))
            DropDownListClientGroup.Items.Insert(0, New ListItem("", "-1"))
        End Sub

        ''' <summary>
        ''' Binding Service Type Drop Down List
        ''' </summary>
        Public Sub BindServiceTypeDropDownList(ByRef DropDownListServiceType As DropDownList)

            Dim ServiceTypeList As List(Of ServiceTypeDataObject)
            Dim objBLServiceType As New BLServiceType
            ServiceTypeList = objBLServiceType.SelectServiceType(ConVisitel)
            objBLServiceType = Nothing

            DropDownListServiceType.DataSource = ServiceTypeList
            DropDownListServiceType.DataTextField = "ProgramService"
            DropDownListServiceType.DataValueField = "Id"
            DropDownListServiceType.DataBind()

            ServiceTypeList = Nothing

            'DropDownListServiceType.Items.Insert(0, New ListItem("Please Select...", "-1"))
            DropDownListServiceType.Items.Insert(0, New ListItem("", "-1"))
        End Sub

        ''' <summary>
        ''' Binding Client Drop Down List
        ''' </summary>
        Public Sub BindClientDropDownList(ByRef DropDownListClient As DropDownList, CompanyId As Integer, ClientListFor As EnumDataObject.ClientListFor)

            Dim ClientList As List(Of ClientDataObject)
            Dim objBLClient As New BLClient
            ClientList = objBLClient.SelectClient(ConVisitel, CompanyId, ClientListFor)
            objBLClient = Nothing

            DropDownListClient.DataSource = ClientList
            DropDownListClient.DataTextField = "ClientName"
            DropDownListClient.DataValueField = "ClientInfoId"
            DropDownListClient.DataBind()

            ClientList = Nothing

            If ((ClientListFor.Equals(EnumDataObject.ClientListFor.EDICorrectedClaims)) Or (ClientListFor.Equals(EnumDataObject.ClientListFor.VestaClient))) Then
                DropDownListClient.Items.Insert(0, New ListItem("", "-1"))
            Else
                DropDownListClient.Items.Insert(0, New ListItem("Please Select...", "-1"))
            End If


        End Sub

        ''' <summary>
        ''' Binding Employee Drop Down List
        ''' </summary>
        Public Sub BindEmployeeDropDownList(ByRef DropDownListEmployee As DropDownList, CompanyId As Integer, Filtered As Boolean, Optional ByVal TitleId As Integer = 0)

            Dim EmployeeList As List(Of EmployeeSettingDataObject)

            Dim objBLEmployee As New BLEmployee
            EmployeeList = objBLEmployee.SelectEmployee(ConVisitel, CompanyId)
            objBLEmployee = Nothing

            Dim EmployeeListNew As List(Of EmployeeSettingDataObject) = EmployeeList

            If (Filtered) Then
                EmployeeList = (From p In EmployeeListNew Where p.Title = 2 Or p.Title = 3).ToList()
            End If

            If (TitleId > 0) Then
                EmployeeList = (From p In EmployeeListNew Where p.Title = TitleId).ToList()
            End If

            DropDownListEmployee.DataSource = EmployeeList
            DropDownListEmployee.DataTextField = "EmployeeName"
            DropDownListEmployee.DataValueField = "EmployeeId"
            DropDownListEmployee.DataBind()

            EmployeeList = Nothing
            EmployeeListNew = Nothing

            'DropDownListEmployee.Items.Insert(0, New ListItem("Please Select...", "-1"))

            DropDownListEmployee.Items.Insert(0, New ListItem("", "-1"))

        End Sub

        ''' <summary>
        ''' This is for Time validation using regular expression
        ''' HH:MM:SS
        ''' </summary>
        ''' <param name="Time"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateHourMinte(Time As String) As Boolean
            Dim result As Boolean = False
            Dim TimeExpression As New Regex(HourMinuteValidationExpression)
            If TimeExpression.IsMatch(Time) Then
                result = True
            End If
            Return result
        End Function

        ''' <summary>
        ''' This is for Time validation using regular expression
        ''' HH:MM:SS
        ''' </summary>
        ''' <param name="Time"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateTimeWithAMPM(Time As String) As Boolean
            Dim result As Boolean = False
            Dim TimeExpression As New Regex(TimeValidationWithAMPMExpression)
            If TimeExpression.IsMatch(Time) Then
                result = True
            End If
            Return result
        End Function

        ''' <summary>
        ''' This is for Time validation using regular expression
        ''' HH:MM:SS
        ''' </summary>
        ''' <param name="Time"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateTime(Time As String) As Boolean
            Dim result As Boolean = False
            Dim TimeExpression As New Regex(TimeValidationExpression)
            If TimeExpression.IsMatch(Time) Then
                result = True
            End If
            Return result
        End Function

        ''' <summary>
        ''' This is for Payrate validation using regular expression
        ''' xxx-xx-xxxx
        ''' </summary>
        ''' <param name="Payrate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidatePayrate(Payrate As String) As Boolean
            Dim result As Boolean = False
            Dim PayrateExpression As New Regex(DecimalValueWithDollarSign)
            If PayrateExpression.IsMatch(Payrate) Then
                result = True
            End If
            Return result
        End Function

        ''' <summary>
        '''  This is for validating email using regular expression
        ''' </summary>
        ''' <param name="email"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateEmail(email As String) As Boolean
            Dim result As Boolean = False
            Dim EmailExpression As New Regex("^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")
            If EmailExpression.IsMatch(email) Then
                result = True
            End If
            Return result
        End Function

        ''' <summary>
        ''' This is for date validation using regular expression
        ''' xxx-xx-xxxx
        ''' </summary>
        ''' <param name="DateValue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateDate(DateValue As String) As Boolean
            Dim result As Boolean = False
            Dim DateExpression As New Regex(DateValidationExpression)
            If DateExpression.IsMatch(DateValue) Then
                result = True
            End If
            Return result
        End Function

        ''' <summary>
        ''' This is for social security number validation using regular expression
        ''' xxx-xx-xxxx
        ''' </summary>
        ''' <param name="SocialSecurityNumber"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateSocialSecurityNumber(SocialSecurityNumber As String) As Boolean
            Dim result As Boolean = False
            Dim SocialSecurityNumberExpression As New Regex(SocialSecurityNumberValidationExpression)
            If SocialSecurityNumberExpression.IsMatch(SocialSecurityNumber) Then
                result = True
            End If
            Return result
        End Function

        ''' <summary>
        ''' Formatted the social security number number as xxx-xx-xxxx
        ''' </summary>
        ''' <param name="SocialSecurityNumber"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function GetFormattedSocialSecurityNumber(SocialSecurityNumber As String) As String
            Dim result As String = String.Empty
            If (Not String.IsNullOrEmpty(SocialSecurityNumber)) Then
                result = SocialSecurityNumber.Substring(0, 3) + "-" + SocialSecurityNumber.Substring(3, 2) + "-" + SocialSecurityNumber.Substring(5, 4)
            End If
            Return result
        End Function

        ''' <summary>
        ''' Re-Formatted the social security number as xxxxxxxxx
        ''' </summary>
        ''' <param name="SocialSecurityNumber"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetReFormattedSocialSecurityNumber(SocialSecurityNumber As String) As String
            Dim result As String = String.Empty
            If (Not String.IsNullOrEmpty(SocialSecurityNumber)) Then
                result = IIf((SocialSecurityNumber.Contains("-")),
                             SocialSecurityNumber.Substring(0, 3) + SocialSecurityNumber.Substring(7, 2) + SocialSecurityNumber.Substring(7, 4), SocialSecurityNumber)
            End If
            Return result
        End Function

        ''' <summary>
        ''' This is for decimal value checking
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsDecimal(value As String) As Boolean
            Dim result As Boolean = False
            Dim outResult As Decimal
            result = IIf((Decimal.TryParse(value, outResult)), True, False)
            Return result
        End Function

        ''' <summary>
        ''' This is for integer value checking
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsInteger(value As String) As Boolean
            Dim result As Boolean = False
            Dim outResult As Integer
            If (Integer.TryParse(value, outResult)) Then
                result = True
            End If
            Return result
        End Function

        ''' <summary>
        ''' This is for phone validation using regular expression
        ''' xxx-xx-xxxx
        ''' </summary>
        ''' <param name="PhoneNumber"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidatePhone(PhoneNumber As String) As Boolean
            Dim result As Boolean = False
            Dim PhoneNumberExpression As New Regex(PhoneValidationExpression)
            If PhoneNumberExpression.IsMatch(PhoneNumber) Then
                result = True
            End If
            Return result
        End Function

        ''' <summary>
        ''' This is for Zip Code Validation using regular expression
        ''' xxxxx
        ''' </summary>
        ''' <param name="Zip"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateZip(Zip As String) As Boolean
            Dim result As Boolean = False
            Dim ZipCodeExpression As New Regex(ZipCodeValidationExpression)
            If ZipCodeExpression.IsMatch(Zip) Then
                result = True
            End If
            Return result
        End Function

        ''' <summary>
        ''' Formatted the mobile number as (xxx) xxx-xxxx
        ''' </summary>
        ''' <param name="MobileNumber"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetFormattedMobileNumber(MobileNumber As String) As String
            Dim result As String = String.Empty
            If (Not String.IsNullOrEmpty(MobileNumber)) Then
                result = "(" + MobileNumber.Substring(0, 3) + ") " + MobileNumber.Substring(3, 3) + "-" + MobileNumber.Substring(6, 4)
            End If
            Return result
        End Function

        ''' <summary>
        ''' Re-Formatted the mobile number as xxxxxxxxxx
        ''' </summary>
        ''' <param name="MobileNumber"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetReFormattedMobileNumber(MobileNumber As String) As String
            Dim result As String = String.Empty
            If (Not String.IsNullOrEmpty(MobileNumber)) Then
                result = If((MobileNumber.Contains("(")), MobileNumber.Substring(1, 3) + MobileNumber.Substring(6, 3) + MobileNumber.Substring(10, 4), MobileNumber)
            End If
            Return result
        End Function

        ''' <summary>
        ''' Formatted the payrate as $x.xx
        ''' </summary>
        ''' <param name="Payrate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetFormattedPayrate(Payrate As Decimal) As String
            Dim result As String = String.Empty
            If (Not String.IsNullOrEmpty(Payrate)) Then
                result = "$" & Payrate.ToString("#.##").Trim()
            End If
            Return result
        End Function

        ''' <summary>
        ''' Re-Formatted the Payrate as x.xx
        ''' </summary>
        ''' <param name="Payrate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetReFormattedPayrate(Payrate As String) As String
            Dim result As String = String.Empty
            result = If((Not String.IsNullOrEmpty(Payrate) And Payrate.IndexOf("$").Equals(0)), Payrate.Substring(1, Payrate.Length - 1), Payrate)
            Return result
        End Function

        ''' <summary>
        ''' This is for initializing controls with respecting value
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="target"></param>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function

        Public Function FormatTime(Time As String) As String

            Dim CultureSource = New CultureInfo("en-US", False)
            Dim CultureDestination = New CultureInfo("de-DE", False)

            Return DateTime.Parse(Convert.ToString(Time, Nothing).Trim(), CultureSource).ToString("t", CultureDestination)

        End Function


        ''' <summary>
        ''' Setting control's regular expression validator setting
        ''' </summary>
        ''' <param name="RegularExpressionValidatorControl"></param>
        ''' <param name="ControlToValidate"></param>
        ''' <param name="ValidationExpression"></param>
        ''' <param name="ErrorMessage"></param>
        ''' <param name="ErrorText"></param>
        ''' <param name="ValidationEnable"></param>
        ''' <param name="ValidationGroup"></param>
        ''' <remarks></remarks>
        Public Sub SetRegularExpressionValidatorSetting(ByRef RegularExpressionValidatorControl As RegularExpressionValidator, ControlToValidate As String,
                                                         ValidationExpression As String, ErrorMessage As String, Optional ErrorText As String = "",
                                                         Optional ValidationEnable As Boolean = True, Optional ValidationGroup As String = "")

            If (Not RegularExpressionValidatorControl Is Nothing) Then

                RegularExpressionValidatorControl.Enabled = ValidationEnable

                RegularExpressionValidatorControl.ControlToValidate = ControlToValidate
                RegularExpressionValidatorControl.ValidationExpression = ValidationExpression
                RegularExpressionValidatorControl.ErrorMessage = ErrorMessage
                RegularExpressionValidatorControl.Text = ErrorText
                RegularExpressionValidatorControl.SetFocusOnError = True
                RegularExpressionValidatorControl.ValidationGroup = ValidationGroup
                RegularExpressionValidatorControl.Display = ValidatorDisplay.Dynamic
                RegularExpressionValidatorControl.ForeColor = Drawing.Color.Red

            End If

        End Sub

        ''' <summary>
        ''' Setting control's required field validator setting
        ''' </summary>
        ''' <param name="RequiredFieldValidatorControl"></param>
        ''' <param name="ControlToValidate"></param>
        ''' <param name="ErrorMessage"></param>
        ''' <param name="ErrorText"></param>
        ''' <param name="ValidationEnable"></param>
        ''' <param name="ValidationGroup"></param>
        ''' <remarks></remarks>
        Public Sub SetRequiredFieldValidatorSetting(ByRef RequiredFieldValidatorControl As RequiredFieldValidator, ControlToValidate As String,
                                                        ErrorMessage As String, Optional ErrorText As String = "",
                                                        Optional ValidationEnable As Boolean = True, Optional ValidationGroup As String = "")

            If (Not RequiredFieldValidatorControl Is Nothing) Then

                RequiredFieldValidatorControl.Enabled = ValidationEnable
                RequiredFieldValidatorControl.ControlToValidate = ControlToValidate
                RequiredFieldValidatorControl.ErrorMessage = ErrorMessage
                RequiredFieldValidatorControl.Text = ErrorText
                RequiredFieldValidatorControl.SetFocusOnError = True
                RequiredFieldValidatorControl.ValidationGroup = ValidationGroup
                RequiredFieldValidatorControl.Display = ValidatorDisplay.Dynamic
                RequiredFieldValidatorControl.ForeColor = Drawing.Color.Red
            End If


        End Sub

        ''' <summary>
        ''' Setting GridView data sort direction
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GridViewSortDirection() As SortDirection
            Get
                If ViewState("sortDirection") Is Nothing Then
                    ViewState("sortDirection") = SortDirection.Ascending
                End If

                Return DirectCast(ViewState("sortDirection"), SortDirection)
            End Get
            Set(value As SortDirection)
                ViewState("sortDirection") = value
            End Set
        End Property

        ''' <summary>
        ''' Defining textbox control's maximum text length
        ''' </summary>
        ''' <param name="TextBoxControl"></param>
        ''' <param name="MaxLength"></param>
        ''' <remarks></remarks>
        Public Sub SetControlTextLength(ByRef TextBoxControl As TextBox, MaxLength As Integer)
            If (Not TextBoxControl Is Nothing) Then
                TextBoxControl.MaxLength = MaxLength
            End If
        End Sub

        ''' <summary>
        ''' Setting shadow text for Hour:Minute textbox controls 
        ''' </summary>
        ''' <param name="TextBoxControl"></param>
        ''' <remarks></remarks>
        Public Sub SetShadowTextIndicatorHourMinute(TextBoxControl As TextBox, HourMinuteFormat As String)
            If (Not TextBoxControl Is Nothing) Then
                TextBoxControl.Attributes.Add("placeholder", HourMinuteFormat)
            End If
        End Sub

        ''' <summary>
        ''' Setting shadow text for time textbox controls 
        ''' </summary>
        ''' <param name="TextBoxControl"></param>
        ''' <remarks></remarks>
        Public Sub SetShadowTextIndicatorTime(ByRef TextBoxControl As TextBox, TimeFormat As String)
            If (Not TextBoxControl Is Nothing) Then
                TextBoxControl.Attributes.Add("placeholder", TimeFormat)
            End If
        End Sub

        ''' <summary>
        ''' Get Client Status Information
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetClientStatus(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of ClientStatusDataObject)

            Dim ClientStatusList As List(Of ClientStatusDataObject)

            Dim objBLClientStatus As New BLClientStatus()

            ClientStatusList = objBLClientStatus.SelectClientStatus(ConVisitel, CompanyId)

            objBLClientStatus = Nothing

            Return ClientStatusList

        End Function

        ''' <summary>
        ''' Binding Client Type Drop Down List
        ''' </summary>
        ''' <param name="DropDownListClientType"></param>
        ''' <param name="CompanyId"></param>
        ''' <param name="ClientTypeListFor"></param>
        ''' <remarks></remarks>
        Public Sub BindClientTypeDropDownList(ByRef DropDownListClientType As DropDownList, CompanyId As Integer, ClientTypeListFor As EnumDataObject.ClientTypeListFor)

            DropDownListClientType.DataSource = GetClientType(ConVisitel, CompanyId, ClientTypeListFor)

            DropDownListClientType.DataTextField = "Name"
            DropDownListClientType.DataValueField = "IdNumber"
            DropDownListClientType.DataBind()

            If (ClientTypeListFor.Equals(EnumDataObject.ClientTypeListFor.Vesta)) Then
                DropDownListClientType.Items.Insert(0, New ListItem("", "-1"))
            Else
                DropDownListClientType.Items.Insert(0, New ListItem("Please Select...", "-1"))
            End If
        End Sub

        ''' <summary>
        ''' Get Client Type Information
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetClientType(ConVisitel As SqlConnection, CompanyId As Integer, ClientTypeListFor As EnumDataObject.ClientTypeListFor) As List(Of ClientTypeDataObject)

            Dim ClientTypeList As List(Of ClientTypeDataObject)

            Dim objBLClientType As New BLClientType()
            ClientTypeList = objBLClientType.SelectClientType(ConVisitel, CompanyId, ClientTypeListFor)
            objBLClientType = Nothing

            Return ClientTypeList

        End Function

        Public Sub TabSelection(TabIndex As Integer)

            Dim scriptBlock As String

            Select Case TabIndex
                Case 1
                    'scriptBlock = "$('#tabs').tabs({ selected: 0}); "
                    scriptBlock = "$('#tabs').tabs('option', 'active', 0); "    'for jquery-ui >= 1.9
                    Exit Select
                Case 2
                    'scriptBlock = "$('#tabs').tabs({ selected: 1}); "
                    scriptBlock = "$('#tabs').tabs('option', 'active', 1); "    'for jquery-ui >= 1.9
                    Exit Select
                Case 3
                    'scriptBlock = "$('#tabs').tabs({ selected: 2}); "
                    scriptBlock = "$('#tabs').tabs('option', 'active', 2); "    'for jquery-ui >= 1.9
                    Exit Select
                Case 4
                    'scriptBlock = "$('#tabs').tabs({ selected: 3}); "
                    scriptBlock = "$('#tabs').tabs('option', 'active', 3); "    'for jquery-ui >= 1.9
                    Exit Select
                Case 5
                    'scriptBlock = "$('#tabs').tabs({ selected: 4}); "
                    scriptBlock = "$('#tabs').tabs('option', 'active', 4); "    'for jquery-ui >= 1.9
                    Exit Select
                Case 6
                    'scriptBlock = "$('#tabs').tabs({ selected: 5}); "
                    scriptBlock = "$('#tabs').tabs('option', 'active', 5); "    'for jquery-ui >= 1.9
                    Exit Select
                Case Else
                    'scriptBlock = "$('#tabs').tabs({ selected: 0}); "
                    scriptBlock = "$('#tabs').tabs('option', 'active', 0); "    'for jquery-ui >= 1.9
                    Exit Select
            End Select

            'ScriptManager.RegisterStartupScript(Page, Me.[GetType](), "ClientScript", scriptBlock, True)

        End Sub

        Public Function ToDataTable(Of TSource)(data As IList(Of TSource)) As DataTable
            Dim dataTable As New DataTable(GetType(TSource).Name)
            Dim props As PropertyInfo() = GetType(TSource).GetProperties(BindingFlags.[Public] Or BindingFlags.Instance)
            For Each prop As PropertyInfo In props
                dataTable.Columns.Add(prop.Name, If(Nullable.GetUnderlyingType(prop.PropertyType), prop.PropertyType))
            Next

            For Each item As TSource In data
                Dim values = New Object(props.Length - 1) {}
                For i As Integer = 0 To props.Length - 1
                    values(i) = props(i).GetValue(item, Nothing)
                Next
                dataTable.Rows.Add(values)
            Next
            Return dataTable
        End Function

        'Public Shared Function ToDataTable(Of T)(data As IList(Of T)) As DataTable
        '    Dim properties As PropertyDescriptorCollection = TypeDescriptor.GetProperties(GetType(T))
        '    Dim dt As New DataTable()
        '    For i As Integer = 0 To properties.Count - 1
        '        Dim [property] As PropertyDescriptor = properties(i)
        '        dt.Columns.Add([property].Name, [property].PropertyType)
        '    Next
        '    Dim values As Object() = New Object(properties.Count - 1) {}
        '    For Each item As T In data
        '        For i As Integer = 0 To values.Length - 1
        '            values(i) = properties(i).GetValue(item)
        '        Next
        '        dt.Rows.Add(values)
        '    Next
        '    Return dt
        'End Function

        Public Sub ExportExcel(dt As DataTable, ReportName As String, ReportCriteria As String, ReportFileName As String)

            Context.Response.Clear()
            Context.Response.AddHeader("content-disposition", (Convert.ToString("attachment;filename=") & ReportFileName) + ".xls")
            Context.Response.Charset = ""
            Context.Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Context.Response.ContentType = "application/vnd.ms-excel"

            Context.Response.Write(New ExcelDump().ConvertExcel(dt, ReportName, ReportCriteria))
            Context.Response.End()

        End Sub

        Public Function GetPopupUrl(PageUrl As String) As String

            Dim VirtualDirectory As String = System.Web.HttpContext.Current.Request.ApplicationPath

            PageUrl = If((VirtualDirectory.Equals("/")), PageUrl, "/" & PageUrl)

            Return String.Format(System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) & VirtualDirectory & PageUrl)

        End Function


        Public ReadOnly Property GetLoaderImagePath() As String
            Get
                Return System.Web.VirtualPathUtility.ToAbsolute("~/Images/ajax-loading.gif")
            End Get
        End Property

        Public ReadOnly Property GetCalendarImagePath() As String
            Get
                'If (File.Exists(String.Format(Page.ResolveUrl("~/") & "Images/calendar-blue.gif"))) Then
                'Return String.Format(Page.ResolveUrl("~/") & "Images/calendar-blue.gif")
                'Else
                Return System.Web.VirtualPathUtility.ToAbsolute("~/Images/calendar-blue.gif")
                'End If
            End Get

        End Property

        Public Sub SetGridViewRowColor(ByRef r As GridViewRow)

            r.Attributes("onmouseover") = "this.style.cursor='pointer';this.style.textDecoration='underline';this.style.backgroundColor='#D2E6F8';"
            r.Attributes("onmouseout") = "this.style.textDecoration='none';this.style.backgroundColor='#ffffff';"
            'r.ToolTip = "Click to select row"

            'e.Row.Attributes.Add("onmouseover", "this.style.cursor='pointer';this.style.backgroundColor='#D2E6F8'")
            'e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#ffffff'")

            'r.Attributes("onclick") = Me.Page.ClientScript.GetPostBackClientHyperlink(Me.GridViewPayPeriodDetail, "Select$" & Convert.ToString(r.RowIndex), True)

        End Sub

        Public Sub SuccessMessage(strMessage As String)
            'Display success message.
            Dim message As String = strMessage 'Your details have been saved successfully.
            Dim script As String = "window.onload = function(){ jQuery.prompt('"
            script &= message
            script &= "', { prefix: 'cleanblue' })};"

            ClientScript.RegisterStartupScript(Me.GetType(), "SuccessMessage", script, True)
        End Sub


        Public ReadOnly Property UserId() As Integer
            Get
                Return Convert.ToInt32(Session(StaticSettings.SessionField.USER_ID), Nothing)
            End Get
        End Property

        Public ReadOnly Property UserName() As String
            Get
                Return Convert.ToString(Session(StaticSettings.SessionField.USER_NAME), Nothing)
            End Get
        End Property

        Public ReadOnly Property CompanyId() As Integer
            Get
                Return Convert.ToInt32(Session(StaticSettings.SessionField.COMPANY_ID), Nothing)
            End Get
        End Property

        Public ReadOnly Property CompanyName() As String
            Get
                Return Convert.ToString(Session(StaticSettings.SessionField.COMPANY_NAME), Nothing)
            End Get
        End Property

        Public ReadOnly Property GridViewDefaultPageSize() As Integer
            Get
                Return 50
            End Get
        End Property

    End Class
End Namespace

