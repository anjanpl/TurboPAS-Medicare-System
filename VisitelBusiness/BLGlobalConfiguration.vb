#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Global Configuration
' Author: Anjan Kumar Paul
' Start Date: 09 Feb 2016
' End Date: 09 Feb 2016
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                09 Feb 2016    Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness
    Public Class BLGlobalConfiguration

        '<DataObjectMethod(DataObjectMethodType.[Select])> _
        Public Function GlobalConfigurationGetAll(ConVisitel As SqlConnection) As GlobalConfigurationDataObject

            Dim drSql As SqlDataReader = Nothing

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.GlobalConfigurationRetrieve]", drSql, ConVisitel, Nothing)

            objSharedSettings = Nothing

            Dim objGlobalConfiguration As New GlobalConfigurationDataObject

            If drSql.HasRows Then
                If drSql.Read() Then
                    objGlobalConfiguration.LoginPageTitle = If(Convert.IsDBNull(drSql("LoginPageTitle")), String.Empty, Convert.ToString(drSql("LoginPageTitle")))
                    objGlobalConfiguration.CurrentVersion = If(Convert.IsDBNull(drSql("CurrentVersion")), String.Empty, Convert.ToString(drSql("CurrentVersion")))
                    objGlobalConfiguration.LastPublishDate = If(Convert.IsDBNull(drSql("LastPublishDate")), DateTime.MinValue, Convert.ToDateTime(drSql("LastPublishDate")))
                    objGlobalConfiguration.ProjectName = If(Convert.IsDBNull(drSql("ProjectName")), String.Empty, Convert.ToString(drSql("ProjectName")))
                    objGlobalConfiguration.maxInvalidPasswordAttempts = If(Convert.IsDBNull(drSql("maxInvalidPasswordAttempts")), 0,
                                                                           Convert.ToInt32(drSql("maxInvalidPasswordAttempts")))

                    objGlobalConfiguration.minRequiredPasswordLength = If(Convert.IsDBNull(drSql("minRequiredPasswordLength")), 0,
                                                                          Convert.ToInt32(drSql("minRequiredPasswordLength")))

                    objGlobalConfiguration.minRequiredAlphaCharacters = If(Convert.IsDBNull(drSql("minRequiredAlphaCharacters")), 0,
                                                                           Convert.ToInt32(drSql("minRequiredAlphaCharacters")))

                    objGlobalConfiguration.minRequiredNumericCharacters = If(Convert.IsDBNull(drSql("minRequiredNumericCharacters")), 0,
                                                                             Convert.ToInt32(drSql("minRequiredNumericCharacters")))

                    objGlobalConfiguration.minRequiredNonalphanumericCharacters = If(Convert.IsDBNull(drSql("minRequiredNonalphanumericCharacters")), 0,
                                                                                     Convert.ToInt32(drSql("minRequiredNonalphanumericCharacters")))

                    objGlobalConfiguration.minRequiredUpperCaseCharacter = If(Convert.IsDBNull(drSql("minRequiredUpperCaseCharacter")), 0,
                                                                              Convert.ToInt32(drSql("minRequiredUpperCaseCharacter")))

                    objGlobalConfiguration.minRequiredLowerCaseCharacter = If(Convert.IsDBNull(drSql("minRequiredLowerCaseCharacter")), 0,
                                                                              Convert.ToInt32(drSql("minRequiredLowerCaseCharacter")))

                    objGlobalConfiguration.PasswordChangeAfterNumberOfDays = If(Convert.IsDBNull(drSql("PasswordChangeAfterNumberOfDays")), 0,
                                                                                Convert.ToInt32(drSql("PasswordChangeAfterNumberOfDays")))

                    objGlobalConfiguration.EnforcePasswordExpiration = Convert.ToBoolean(drSql("EnforcePasswordExpiration"))
                    objGlobalConfiguration.InvalidPasswordToleranceTimeInMinutes = If(Convert.IsDBNull(drSql("InvalidPasswordToleranceTimeInMinutes")), 0,
                                                                                      Convert.ToInt32(drSql("InvalidPasswordToleranceTimeInMinutes")))

                    objGlobalConfiguration.NumberOfOldPasswordsRestricted = If(Convert.IsDBNull(drSql("NumberOfOldPasswordsRestricted")), 0,
                                                                               Convert.ToInt32(drSql("NumberOfOldPasswordsRestricted")))
                End If
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return objGlobalConfiguration
        End Function
    End Class

    Public Class GlobalConfigurationDataObject
        '''Property for getting and setting LoginPageTitle
        Private _LoginPageTitle As [String]
        Public Property LoginPageTitle() As [String]
            Get
                Return _LoginPageTitle
            End Get
            Set(value As [String])
                _LoginPageTitle = value
            End Set
        End Property

        '''Property for getting and setting CurrentVersion
        Private _CurrentVersion As [String]
        Public Property CurrentVersion() As [String]
            Get
                Return _CurrentVersion
            End Get
            Set(value As [String])
                _CurrentVersion = value
            End Set
        End Property

        '''Property for getting and setting LastPublishDate
        Private _LastPublishDate As DateTime
        Public Property LastPublishDate() As DateTime
            Get
                Return _LastPublishDate
            End Get
            Set(value As DateTime)
                _LastPublishDate = value
            End Set
        End Property

        '''Property for getting and setting ProjectName
        Private _ProjectName As [String]
        Public Property ProjectName() As [String]
            Get
                Return _ProjectName
            End Get
            Set(value As [String])
                _ProjectName = value
            End Set
        End Property

        '''Property for getting and setting maxInvalidPasswordAttempts
        Private _maxInvalidPasswordAttempts As Int32
        Public Property maxInvalidPasswordAttempts() As Int32
            Get
                Return _maxInvalidPasswordAttempts
            End Get
            Set(value As Int32)
                _maxInvalidPasswordAttempts = value
            End Set
        End Property

        '''Property for getting and setting minRequiredPasswordLength
        Private _minRequiredPasswordLength As Int32
        Public Property minRequiredPasswordLength() As Int32
            Get
                Return _minRequiredPasswordLength
            End Get
            Set(value As Int32)
                _minRequiredPasswordLength = value
            End Set
        End Property

        '''Property for getting and setting minRequiredNonalphanumericCharacters
        Private _minRequiredNonalphanumericCharacters As Int32
        Public Property minRequiredNonalphanumericCharacters() As Int32
            Get
                Return _minRequiredNonalphanumericCharacters
            End Get
            Set(value As Int32)
                _minRequiredNonalphanumericCharacters = value
            End Set
        End Property

        '''Property for getting and setting PasswordChangeAfterNumberOfDays
        Private _PasswordChangeAfterNumberOfDays As Int32
        Public Property PasswordChangeAfterNumberOfDays() As Int32
            Get
                Return _PasswordChangeAfterNumberOfDays
            End Get
            Set(value As Int32)
                _PasswordChangeAfterNumberOfDays = value
            End Set
        End Property

        Private _EnforcePasswordExpiration As Boolean

        Public Property EnforcePasswordExpiration() As Boolean
            Get
                Return _EnforcePasswordExpiration
            End Get
            Set(value As Boolean)
                _EnforcePasswordExpiration = value
            End Set
        End Property

        Private _InvalidPasswordToleranceTimeInMinutes As Integer

        Public Property InvalidPasswordToleranceTimeInMinutes() As Integer
            Get
                Return _InvalidPasswordToleranceTimeInMinutes
            End Get
            Set(value As Integer)
                _InvalidPasswordToleranceTimeInMinutes = value
            End Set
        End Property

        Private _minRequiredAlphaCharacters As Integer

        Public Property minRequiredAlphaCharacters() As Integer
            Get
                Return _minRequiredAlphaCharacters
            End Get
            Set(value As Integer)
                _minRequiredAlphaCharacters = value
            End Set
        End Property

        Private _minRequiredNumericCharacters As Integer

        Public Property minRequiredNumericCharacters() As Integer
            Get
                Return _minRequiredNumericCharacters
            End Get
            Set(value As Integer)
                _minRequiredNumericCharacters = value
            End Set
        End Property

        Private _minRequiredUpperCaseCharacter As Integer

        Public Property minRequiredUpperCaseCharacter() As Integer
            Get
                Return _minRequiredUpperCaseCharacter
            End Get
            Set(value As Integer)
                _minRequiredUpperCaseCharacter = value
            End Set
        End Property

        Private _minRequiredLowerCaseCharacter As Integer

        Public Property minRequiredLowerCaseCharacter() As Integer
            Get
                Return _minRequiredLowerCaseCharacter
            End Get
            Set(value As Integer)
                _minRequiredLowerCaseCharacter = value
            End Set
        End Property

        Private _NumberOfOldPasswordsRestricted As Integer

        Public Property NumberOfOldPasswordsRestricted() As Integer
            Get
                Return _NumberOfOldPasswordsRestricted
            End Get
            Set(value As Integer)
                _NumberOfOldPasswordsRestricted = value
            End Set
        End Property
    End Class
End Namespace

