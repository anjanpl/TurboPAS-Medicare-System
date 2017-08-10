Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.ComponentModel
Imports System.Text.RegularExpressions

Namespace VisitelBusiness
    Public Class BLUserInfo


        Public Sub LoginHistoryInsert(ByRef ConVisitel As SqlConnection, UserId As Integer, SessionId As String, LoginTime As DateTime,
                                           SessionLength As Integer, UserIp As String)

            Dim parameters As New HybridDictionary()
            parameters.Add("@UserId", UserId)
            parameters.Add("@SessionId", SessionId)
            parameters.Add("@LoginTime", Convert.ToDateTime(LoginTime.ToString("dd MMM, yyyy HH:mm:ss ttt")))
            parameters.Add("@SessionLength", SessionLength)
            parameters.Add("@UserIp", UserIp)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertLoginHistory]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub LastLoginDateUpdate(ByRef ConVisitel As SqlConnection, UserId As Integer)

            Dim parameters As New HybridDictionary()
            parameters.Add("@UserId", UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UserInfoLastLoginDateUpdate]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        <DataObjectMethod(DataObjectMethodType.[Select])> _
        Public Function UserInfoGetByUserName(ConVisitel As SqlConnection, UserName As String) As UserInfoDataObject

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()
            parameters.Add("@UserName", UserName)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.UserInfoRetrieveByUserName]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim objUserInfoDataObject As UserInfoDataObject
            Dim UserInfoList As New List(Of UserInfoDataObject)()
            FillDataList(drSql, UserInfoList)

            objUserInfoDataObject = UserInfoList(0)
            UserInfoList = Nothing

            Return objUserInfoDataObject

        End Function

        Public Function UserInfoGetByUserId(ConVisitel As SqlConnection, UserId As Int32) As List(Of UserInfoDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()
            parameters.Add("@UserId", UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.UserInfoRetrieveByUserId]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim UserInfoList As New List(Of UserInfoDataObject)()
            FillDataList(drSql, UserInfoList)

            Return UserInfoList

        End Function

        Private Sub FillDataList(ByRef drSql As SqlDataReader, ByRef UserInfoList As List(Of UserInfoDataObject))

            Dim objUserInfoDataObject As UserInfoDataObject

            If drSql.HasRows Then
                While drSql.Read()

                    objUserInfoDataObject = New UserInfoDataObject()

                    objUserInfoDataObject.UserId = If((DBNull.Value.Equals(drSql("UserId"))), objUserInfoDataObject.UserId, Convert.ToInt64(drSql("UserId")))
                    objUserInfoDataObject.FullName = Convert.ToString(drSql("FullName"), Nothing).Trim()
                    objUserInfoDataObject.UserName = Convert.ToString(drSql("UserName"), Nothing).Trim()
                    objUserInfoDataObject.Password = Convert.ToString(drSql("Password"), Nothing).Trim()

                    objUserInfoDataObject.RoleId = If((DBNull.Value.Equals(drSql("RoleId"))), objUserInfoDataObject.RoleId, Convert.ToInt32(drSql("RoleId")))
                    objUserInfoDataObject.RoleName = Convert.ToString(drSql("RoleName"), Nothing).Trim()

                    objUserInfoDataObject.SupervisorUserId = If((DBNull.Value.Equals(drSql("SupervisorUserId"))),
                                                                objUserInfoDataObject.SupervisorUserId, Convert.ToInt32(drSql("SupervisorUserId")))

                    objUserInfoDataObject.SupervisorUserName = Convert.ToString(drSql("SupervisorUserName"), Nothing).Trim()

                    objUserInfoDataObject.IsActive = If((DBNull.Value.Equals(drSql("IsActive"))), objUserInfoDataObject.IsActive, Convert.ToBoolean(drSql("IsActive"), Nothing))

                    objUserInfoDataObject.Email = Convert.ToString(drSql("Email"), Nothing).Trim()
                    objUserInfoDataObject.PasswordSalt = Convert.ToString(drSql("PasswordSalt"), Nothing).Trim()
                    objUserInfoDataObject.SaltedHash = Convert.ToString(drSql("SaltedHash"), Nothing).Trim()

                    objUserInfoDataObject.CompanyId = If((DBNull.Value.Equals(drSql("CompanyId"))), objUserInfoDataObject.CompanyId, Convert.ToInt64(drSql("CompanyId")))

                    objUserInfoDataObject.CompanyName = Convert.ToString(drSql("CompanyName"), Nothing).Trim()
                    objUserInfoDataObject.HomeAddress = Convert.ToString(drSql("HomeAddress"), Nothing).Trim()
                    objUserInfoDataObject.HomePhone = Convert.ToString(drSql("HomePhone"), Nothing).Trim()
                    objUserInfoDataObject.OfficePhone = Convert.ToString(drSql("OfficePhone"), Nothing).Trim()
                    objUserInfoDataObject.MobileNumber = Convert.ToString(drSql("MobileNumber"), Nothing).Trim()
                    objUserInfoDataObject.OfficeAddress = Convert.ToString(drSql("OfficeAddress"), Nothing).Trim()
                    objUserInfoDataObject.ThemeName = Convert.ToString(drSql("ThemeName"), Nothing).Trim()

                    objUserInfoDataObject.IsLocked = If((DBNull.Value.Equals(drSql("IsLocked"))), objUserInfoDataObject.IsLocked, Convert.ToBoolean(drSql("IsLocked"), Nothing))

                    objUserInfoDataObject.LastLockoutDate = Convert.ToString(drSql("LastLockoutDate"), Nothing).Trim()

                    objUserInfoDataObject.IsApproved = If((DBNull.Value.Equals(drSql("IsApproved"))),
                                                          objUserInfoDataObject.IsApproved, Convert.ToBoolean(drSql("IsApproved"), Nothing))

                    objUserInfoDataObject.CreateDate = Convert.ToString(drSql("CreateDate"), Nothing).Trim()
                    objUserInfoDataObject.LastLoginDate = Convert.ToString(drSql("LastLoginDate"), Nothing).Trim()

                    objUserInfoDataObject.FailedPasswordAttemptCount = If((DBNull.Value.Equals(drSql("FailedPasswordAttemptCount"))),
                                                                          objUserInfoDataObject.FailedPasswordAttemptCount, Convert.ToInt32(drSql("FailedPasswordAttemptCount")))

                    objUserInfoDataObject.FailedPasswordAttemptWindowStart = Convert.ToString(drSql("FailedPasswordAttemptWindowStart"), Nothing).Trim()
                    objUserInfoDataObject.LastPasswordChangedDate = Convert.ToString(drSql("LastPasswordChangedDate"), Nothing).Trim()

                    objUserInfoDataObject.GenderId = If((DBNull.Value.Equals(drSql("GenderId"))), objUserInfoDataObject.GenderId, Convert.ToInt64(drSql("GenderId")))

                    objUserInfoDataObject.GenderName = Convert.ToString(drSql("GenderName"), Nothing).Trim()

                    objUserInfoDataObject.CreatedBy = If((DBNull.Value.Equals(drSql("CreatedBy"))), objUserInfoDataObject.CreatedBy, Convert.ToInt64(drSql("CreatedBy")))
                    objUserInfoDataObject.ApprovedBy = If((DBNull.Value.Equals(drSql("ApprovedBy"))), objUserInfoDataObject.ApprovedBy, Convert.ToInt64(drSql("ApprovedBy")))

                    objUserInfoDataObject.ApprovedDate = Convert.ToString(drSql("ApprovedDate"), Nothing).Trim()

                    UserInfoList.Add(objUserInfoDataObject)

                End While
            End If

            objUserInfoDataObject = Nothing

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing
        End Sub

        Public Sub FailedPasswordAttemptWindowStartUpdate(ByRef ConVisitel As SqlConnection, UserId As Integer)

            Dim parameters As New HybridDictionary()
            parameters.Add("@UserId", UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.FailedPasswordAttemptWindowStartUpdate]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub FailedPasswordAttemptCountIncrease(ByRef ConVisitel As SqlConnection, UserId As Integer)

            Dim parameters As New HybridDictionary()
            parameters.Add("@UserID", UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.FailedPasswordAttemptCountIncrease]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub LockUser(ByRef ConVisitel As SqlConnection, UserId As Integer)

            Dim parameters As New HybridDictionary()
            parameters.Add("@UserId", UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.LockUser]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Function UserInfoGetAll(ConVisitel As SqlConnection) As List(Of UserInfoDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.UserInfoGetAll]", drSql, ConVisitel, Nothing)

            objSharedSettings = Nothing

            Dim UserInfoList As New List(Of UserInfoDataObject)()
            FillDataList(drSql, UserInfoList)

            Return UserInfoList

        End Function

        Public Function RolesGetAll(ConVisitel As SqlConnection) As List(Of UserRoleDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.RolesGetAll]", drSql, ConVisitel, Nothing)

            objSharedSettings = Nothing

            Dim UserRoleList As New List(Of UserRoleDataObject)()

            Dim objUserRoleDataObject As UserRoleDataObject

            If drSql.HasRows Then
                While drSql.Read()
                    objUserRoleDataObject = New UserRoleDataObject()

                    objUserRoleDataObject.RoleId = If((DBNull.Value.Equals(drSql("RoleId"))), objUserRoleDataObject.RoleId, Convert.ToInt32(drSql("RoleId")))
                    objUserRoleDataObject.RoleName = Convert.ToString(drSql("RoleName"), Nothing).Trim()

                    UserRoleList.Add(objUserRoleDataObject)
                End While
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return UserRoleList

        End Function

        Public Function UserInfoFilter(ConVisitel As SqlConnection, SearchBy As Integer, SearchString As String, ApprovalStatus As Integer, _
                                       LockedStatus As Integer) As List(Of UserInfoDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim objSharedSettings As New SharedSettings()

            Dim parameters As New HybridDictionary()
            parameters.Add("@SearchBy", SearchBy)
            parameters.Add("@SearchString", SearchString)
            parameters.Add("@ApprovalStatus", ApprovalStatus)
            parameters.Add("@LockedStatus", LockedStatus)

            objSharedSettings.GetDataReader("", "[TurboDB.UserInfoFilter]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim UserInfoList As New List(Of UserInfoDataObject)()
            FillDataList(drSql, UserInfoList)

            Return UserInfoList

        End Function

        Public Function UserInfoStatistics(ConVisitel As SqlConnection) As UserStatistics
            Dim drSql As SqlDataReader = Nothing

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.UserInfoStatistics]", drSql, ConVisitel, Nothing)
            objSharedSettings = Nothing

            Dim objUserStatistics As New UserStatistics

            If drSql.HasRows Then
                If drSql.Read() Then
                    objUserStatistics.NoOfUsers = If((DBNull.Value.Equals(drSql("NoOfUsers"))), objUserStatistics.NoOfUsers, Convert.ToInt32(drSql("NoOfUsers")))
                    objUserStatistics.ActiveUsers = If((DBNull.Value.Equals(drSql("ActiveUsers"))), objUserStatistics.ActiveUsers, Convert.ToInt32(drSql("ActiveUsers")))
                    objUserStatistics.ApprovedUsers = If((DBNull.Value.Equals(drSql("ApprovedUsers"))), objUserStatistics.ApprovedUsers, Convert.ToInt32(drSql("ApprovedUsers")))
                    objUserStatistics.LockedUsers = If((DBNull.Value.Equals(drSql("LockedUsers"))), objUserStatistics.LockedUsers, Convert.ToInt32(drSql("LockedUsers")))
                    objUserStatistics.NotLoggedinWithinThreeMonths = If((DBNull.Value.Equals(drSql("NotLoggedinWithinThreeMonths"))) _
                                                                        , objUserStatistics.NotLoggedinWithinThreeMonths, Convert.ToInt32(drSql("NotLoggedinWithinThreeMonths")))
                    objUserStatistics.NotLoggedinWithinSixMonths = If((DBNull.Value.Equals(drSql("NotLoggedinWithinSixMonths"))) _
                                                                        , objUserStatistics.NotLoggedinWithinSixMonths, Convert.ToInt32(drSql("NotLoggedinWithinSixMonths")))
                    objUserStatistics.NotLoggedinWithinTwelveMonths = If((DBNull.Value.Equals(drSql("NotLoggedinWithinTwelveMonths"))) _
                                                                        , objUserStatistics.NotLoggedinWithinTwelveMonths, Convert.ToInt32(drSql("NotLoggedinWithinTwelveMonths")))
                End If
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return objUserStatistics

        End Function

        Public Sub InsertUserInfo(ConVisitel As SqlConnection, ByRef objUserInfoDataObject As UserInfoDataObject)
            Dim PasswordSalt As Integer = BLPassword.CreateRandomSalt()
            Dim objPassword As New BLPassword(objUserInfoDataObject.Password, PasswordSalt)
            Dim SaltedHash As String = objPassword.ComputeSaltedHash()

            Dim parameters As New HybridDictionary()

            parameters.Add("@PasswordSalt", PasswordSalt)
            parameters.Add("@SaltedHash", SaltedHash)

            SetParameter(parameters, objUserInfoDataObject)

            parameters.Add("@CreatedBy", objUserInfoDataObject.CreatedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UserInfoInsert]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Sub UpdateUserInfo(ConVisitel As SqlConnection, ByRef objUserInfoDataObject As UserInfoDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@UserId", objUserInfoDataObject.UserId)

            SetParameter(parameters, objUserInfoDataObject)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UserInfoUpdate]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteUserInfo(ConVisitel As SqlConnection, UserId As Int64)

            Dim parameters As New HybridDictionary()

            parameters.Add("@UserId", UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UserInfoDelete]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetParameter(ByRef parameters As HybridDictionary, ByRef objUserInfoDataObject As UserInfoDataObject)
            parameters.Add("@FullName", objUserInfoDataObject.FullName)
            parameters.Add("@UserName", objUserInfoDataObject.UserName)
            parameters.Add("@Password", objUserInfoDataObject.Password)
            parameters.Add("@RoleId", objUserInfoDataObject.RoleId)
            parameters.Add("@SupervisorUserId", objUserInfoDataObject.SupervisorUserId)
            parameters.Add("@CompanyId", objUserInfoDataObject.CompanyId)
            parameters.Add("@IsActive", If((objUserInfoDataObject.IsActive), 1, 0))
            parameters.Add("@Email", objUserInfoDataObject.Email)

            parameters.Add("@HomeAddress", objUserInfoDataObject.HomeAddress)
            parameters.Add("@HomePhone", objUserInfoDataObject.HomePhone)
            parameters.Add("@OfficePhone", objUserInfoDataObject.OfficePhone)
            parameters.Add("@MobileNumber", objUserInfoDataObject.MobileNumber)
            parameters.Add("@OfficeAddress", objUserInfoDataObject.OfficeAddress)
            parameters.Add("@ThemeName", objUserInfoDataObject.ThemeName)
            parameters.Add("@IsLocked", If((objUserInfoDataObject.IsLocked), 1, 0))
            parameters.Add("@Gender", objUserInfoDataObject.GenderId)
        End Sub

        Public Sub UnLockUser(ConVisitel As SqlConnection, UserId As Int64)

            Dim parameters As New HybridDictionary()

            parameters.Add("@UserId", UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UserInfoUnLockUser]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Sub ApproveUser(ConVisitel As SqlConnection, UserId As Int64, ApprovedBy As Int64)
            Dim parameters As New HybridDictionary()

            parameters.Add("@UserId", UserId)
            parameters.Add("@ApprovedBy", ApprovedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UserInfoApproveUser]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        <DataObjectMethod(DataObjectMethodType.Update)> _
        Public Sub ChangePassword(ConVisitel As SqlConnection, ByRef objUserInfoDataObject As UserInfoDataObject)
            Dim parameters As New HybridDictionary()

            Dim PasswordSalt As Integer = BLPassword.CreateRandomSalt()
            Dim objPassword As New BLPassword(objUserInfoDataObject.Password, PasswordSalt)
            Dim SaltedHash As String = objPassword.ComputeSaltedHash()

            parameters.Add("@UserId", objUserInfoDataObject.UserId)
            parameters.Add("@PasswordSalt", PasswordSalt)
            parameters.Add("@SaltedHash", SaltedHash)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateChangePassword]", ConVisitel, parameters)
            objSharedSettings = Nothing

            parameters = Nothing
        End Sub


        Public Function PasswordHistoryGet(ConVisitel As SqlConnection, UserId As String, NoOfRecords As String) As DataTable

            Dim ds As New DataSet

            Dim parameters As New HybridDictionary()
            parameters.Add("@UserId", UserId)
            parameters.Add("@NoOfRecords", NoOfRecords)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataSet("", "[TurboDB.PasswordHistoryRetrieve]", ds, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Return ds.Tables(0)

        End Function

        Public Function ValidatePasswordFormat(ConVisitel As SqlConnection, sPassword As String, UserId As String) As String
            Try
                Dim objBLGlobalConfiguration As New BLGlobalConfiguration()

                Dim objGlobalConfigurationDataObject As GlobalConfigurationDataObject = objBLGlobalConfiguration.GlobalConfigurationGetAll(ConVisitel)

                objBLGlobalConfiguration = Nothing

                If objGlobalConfigurationDataObject.minRequiredPasswordLength > sPassword.Length Then
                    Return "password must be at least " & objGlobalConfigurationDataObject.minRequiredPasswordLength.ToString() & " characters long"
                End If

                'the code below checks for presence of at least n number of alphabetic characters
                Dim sPattern As String = "[a-zA-Z]{" & objGlobalConfigurationDataObject.minRequiredAlphaCharacters & ",}"

                Dim oReg As New Regex(sPattern, RegexOptions.IgnoreCase)
                If oReg.IsMatch(sPassword) = False Then
                    Return "Password should contain mimimum " & objGlobalConfigurationDataObject.minRequiredAlphaCharacters & " alphabetic character"
                End If

                'the code below checks for presence of at least n number of numeric characters
                sPattern = "[0-9]{" & objGlobalConfigurationDataObject.minRequiredNumericCharacters & ",}"
                oReg = New Regex(sPattern, RegexOptions.IgnoreCase)
                If oReg.IsMatch(sPassword) = False Then
                    Return "Password should contain mimimum " & objGlobalConfigurationDataObject.minRequiredNumericCharacters.ToString() & " numeric character"
                End If

                'the code below checks for presence of at least n number of upper case characters
                sPattern = "[A-Z]{" & objGlobalConfigurationDataObject.minRequiredUpperCaseCharacter & ",}"

                oReg = New Regex(sPattern)
                If oReg.IsMatch(sPassword) = False Then
                    Return "Password should contain mimimum " & objGlobalConfigurationDataObject.minRequiredUpperCaseCharacter & " upper case character"
                End If

                'the code below checks for presence of at least n number of lower case characters
                sPattern = "[a-z]{" & objGlobalConfigurationDataObject.minRequiredLowerCaseCharacter & ",}"

                oReg = New Regex(sPattern)
                If oReg.IsMatch(sPassword) = False Then
                    Return "Password should contain mimimum " & objGlobalConfigurationDataObject.minRequiredLowerCaseCharacter & " lower case character"
                End If

                'the code below checks for presence of atleast one non-alphanumeric character 
                sPattern = "[^a-zA-Z0-9" & vbLf & vbCr & vbTab & " ]{" & objGlobalConfigurationDataObject.minRequiredNonalphanumericCharacters & ",}"
                oReg = New Regex(sPattern, RegexOptions.IgnoreCase)

                If oReg.IsMatch(sPassword) = False Then
                    Return "Password should contain mimimum " & objGlobalConfigurationDataObject.minRequiredNonalphanumericCharacters & " non-alphanumeric character"
                End If

                Dim dt As DataTable = PasswordHistoryGet(ConVisitel, UserId, objGlobalConfigurationDataObject.NumberOfOldPasswordsRestricted.ToString())

                Dim passwordHistoryFailed As Boolean = False

                If dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows
                        Dim objPassword As New BLPassword(sPassword, Convert.ToInt32(dr("PasswordSalt")))
                        Dim SaltedHash As String = objPassword.ComputeSaltedHash()

                        If SaltedHash = dr("SaltedHash").ToString() Then
                            passwordHistoryFailed = True
                        End If
                    Next
                End If

                dt.Dispose()
                dt = Nothing

                If (passwordHistoryFailed) Then
                    Return "Last " & objGlobalConfigurationDataObject.NumberOfOldPasswordsRestricted.ToString() & " passwords can not be re-used"
                End If

                Return "valid"
            Catch ex As Exception
                Return ex.Message
            Finally
                
            End Try
        End Function

    End Class
End Namespace

