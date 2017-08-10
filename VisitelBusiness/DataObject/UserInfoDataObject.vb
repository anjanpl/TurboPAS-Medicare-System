Namespace VisitelBusiness.DataObject
    Public Class UserInfoDataObject
        Inherits BaseDataObject

        Public Property UserId As Int64

        Public Property FullName As String

        Public Property UserName As String

        Public Property Password As String

        Public Property RoleId As Integer

        Public Property RoleName As String

        Public Property SupervisorUserId As Integer

        Public Property SupervisorUserName As String

        Public Property IsActive As Boolean

        Public Property Email As String

        Public Property PasswordSalt As String

        Public Property SaltedHash As String

        Public Property CompanyId As Int64

        Public Property CompanyName As String

        Public Property HomeAddress As String

        Public Property HomePhone As String

        Public Property OfficePhone As String

        Public Property MobileNumber As String

        Public Property OfficeAddress As String

        Public Property ThemeName As String

        Public Property IsLocked As Boolean

        Public Property LastLockoutDate As String

        Public Property IsApproved As Boolean

        Public Property CreateDate As String

        Public Property LastLoginDate As String

        Public Property FailedPasswordAttemptCount As Integer

        Public Property FailedPasswordAttemptWindowStart As String

        Public Property LastPasswordChangedDate As String

        Public Property GenderId As Int32

        Public Property GenderName As String

        Public Property CreatedBy As Int64

        Public Property ApprovedBy As Int64

        Public Property ApprovedDate As String

        Public Sub New()
            Me.UserId = InlineAssignHelper(Me.CompanyId, InlineAssignHelper(Me.CreatedBy, InlineAssignHelper(Me.ApprovedBy, Int64.MinValue)))

            Me.FullName = InlineAssignHelper(Me.UserName, InlineAssignHelper(Me.Password, InlineAssignHelper(Me.Email,
                          InlineAssignHelper(Me.PasswordSalt, InlineAssignHelper(Me.SaltedHash, InlineAssignHelper(Me.HomeAddress,
                          InlineAssignHelper(Me.HomePhone, InlineAssignHelper(Me.OfficePhone, InlineAssignHelper(Me.MobileNumber,
                          InlineAssignHelper(Me.OfficeAddress, InlineAssignHelper(Me.ThemeName, InlineAssignHelper(Me.LastLockoutDate,
                          InlineAssignHelper(Me.CreateDate, InlineAssignHelper(Me.LastLoginDate, InlineAssignHelper(Me.FailedPasswordAttemptWindowStart,
                          InlineAssignHelper(Me.LastPasswordChangedDate, InlineAssignHelper(Me.GenderName, InlineAssignHelper(Me.ApprovedDate,
                          InlineAssignHelper(Me.CompanyName, InlineAssignHelper(Me.SupervisorUserName, InlineAssignHelper(Me.RoleName, String.Empty)))))))))))))))))))))

            Me.RoleId = InlineAssignHelper(Me.SupervisorUserId, InlineAssignHelper(Me.FailedPasswordAttemptCount, InlineAssignHelper(Me.GenderId, Integer.MinValue)))

            Me.IsActive = InlineAssignHelper(Me.IsLocked, InlineAssignHelper(Me.IsApproved, False))
        End Sub
    End Class

    Public Class UserRoleDataObject
        Public Property RoleId As Int32

        Public Property RoleName As String

        Public Sub New()
            Me.RoleId = Int32.MinValue
            Me.RoleName = String.Empty
        End Sub
    End Class

    Public Class UserStatistics
        Inherits BaseDataObject

        Public Property NoOfUsers As Int32

        Public Property ActiveUsers As Int32

        Public Property ApprovedUsers As Int32

        Public Property LockedUsers As Int32

        Public Property NotLoggedinWithinThreeMonths As Int32

        Public Property NotLoggedinWithinSixMonths As Int32

        Public Property NotLoggedinWithinTwelveMonths As Int32

        Public Sub New()
            Me.NoOfUsers = InlineAssignHelper(Me.ActiveUsers, InlineAssignHelper(Me.ApprovedUsers, InlineAssignHelper(Me.LockedUsers,
                           InlineAssignHelper(Me.NotLoggedinWithinThreeMonths, InlineAssignHelper(Me.NotLoggedinWithinSixMonths,
                           InlineAssignHelper(Me.NotLoggedinWithinTwelveMonths, Int32.MinValue))))))
        End Sub
    End Class
End Namespace