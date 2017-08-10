#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: FTP Requests Handling
' Author: Anjan Kumar Paul
' Start Date: 11 Sept 2015
' End Date: 11 Sept 2015
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                11 Sept 2015    Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports WinSCP
Imports System.Configuration
Imports VisitelBusiness.VisitelBusiness.DataObject

Namespace VisitelCommon

    Public Class SFTPRequests


        Private Host As String = Nothing
        Private User As String = Nothing
        Private Pass As String = Nothing

        ''' <summary>
        ''' Construct Object
        ''' </summary>
        ''' <param name="HostIP"></param>
        ''' <param name="UserName"></param>
        ''' <param name="Password"></param>
        ''' <remarks></remarks>
        Public Sub New(HostIP As String, UserName As String, Password As String)
            Host = HostIP
            User = UserName
            Pass = Password
        End Sub

        ''' <summary>
        ''' Upload File 
        ''' </summary>
        ''' <param name="SFTPRequestsParam"></param>
        ''' <remarks></remarks>
        Public Sub UploadFile(ByRef SFTPRequestsParam As SFTPRequestsParam)

            Dim SessionOptions As New SessionOptions()

            SessionOptions.Protocol = Protocol.Sftp
            SessionOptions.HostName = Host
            SessionOptions.UserName = User
            SessionOptions.Password = Pass
            SessionOptions.PortNumber = Convert.ToString(ConfigurationManager.AppSettings("SFTPPortNumber"), Nothing)
            SessionOptions.SshHostKeyFingerprint = Convert.ToString(ConfigurationManager.AppSettings("SFTPCertKey"), Nothing)

            Dim Session As New Session()

            Session.SessionLogPath = "your log path"

            Try
                Session.Open(SessionOptions) 'Attempts to connect to your sFtp site
            Catch ex As InvalidOperationException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionLocalException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionRemoteException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As TimeoutException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Finally
                SessionOptions = Nothing
            End Try


            Dim TransferOptions As New TransferOptions() 'Get Ftp File
            TransferOptions.TransferMode = TransferMode.Binary 'The Transfer Mode - <em style="font-size: 9pt;">Automatic, Binary, or Ascii  

            'Permissions applied to remote files;  
            TransferOptions.FilePermissions = Nothing 'null for default permissions.  Can set user, Group, or other Read/Write/Execute permissions. 

            'Set last write time of destination file to that of source file - basically change the timestamp to match destination and source files.   
            TransferOptions.PreserveTimestamp = False

            TransferOptions.ResumeSupport.State = TransferResumeSupportState.Off

            Dim TransferResult As TransferOperationResult

            Try
                'the parameter list is: local Path, Remote Path, Delete source file?, transfer Options  
                TransferResult = Session.PutFiles(String.Format("{0}\{1}", SFTPRequestsParam.LocalPath, SFTPRequestsParam.LocalFileName),
                                                  String.Format("{0}{1}", SFTPRequestsParam.RemotePath, SFTPRequestsParam.RemoteFileName), True, TransferOptions)
                TransferResult.Check()

            Catch ex As InvalidOperationException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionLocalException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionRemoteException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As TimeoutException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Finally
                TransferOptions = Nothing
                TransferResult = Nothing
                Session.Close()
                Session = Nothing
            End Try

        End Sub

        ''' <summary>
        ''' Download File 
        ''' </summary>
        ''' <param name="SFTPRequestsParam"></param>
        ''' <remarks></remarks>
        Public Sub DownloadFile(ByRef SFTPRequestsParam As SFTPRequestsParam)

            Dim SessionOptions As New SessionOptions()

            SessionOptions.Protocol = Protocol.Sftp
            SessionOptions.HostName = Host
            SessionOptions.UserName = User
            SessionOptions.Password = Pass
            SessionOptions.PortNumber = ConfigurationManager.AppSettings("SFTPPortNumber")
            SessionOptions.SshHostKeyFingerprint = ConfigurationManager.AppSettings("SFTPCertKey")

            Dim Session As New Session()

            Session.SessionLogPath = "your log path"

            Try
                Session.Open(SessionOptions) 'Attempts to connect to your sFtp site
            Catch ex As InvalidOperationException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionLocalException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionRemoteException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As TimeoutException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Finally
                SessionOptions = Nothing
            End Try

            Dim TransferOptions As New TransferOptions() 'Get Ftp File
            TransferOptions.TransferMode = TransferMode.Binary 'The Transfer Mode - <em style="font-size: 9pt;">Automatic, Binary, or Ascii  

            'Permissions applied to remote files;  
            TransferOptions.FilePermissions = Nothing 'null for default permissions.  Can set user, Group, or other Read/Write/Execute permissions. 

            'Set last write time of destination file to that of source file - basically change the timestamp to match destination and source files.   
            TransferOptions.PreserveTimestamp = False

            TransferOptions.ResumeSupport.State = TransferResumeSupportState.Off

            Dim TransferResult As TransferOperationResult

            Try
                'the parameter list is: local Path, Remote Path, Delete source file?, transfer Options  
                TransferResult = Session.GetFiles(String.Format("{0}{1}", SFTPRequestsParam.RemotePath, SFTPRequestsParam.RemoteFileName),
                                                  String.Format("{0}\{1}", SFTPRequestsParam.LocalPath, SFTPRequestsParam.LocalFileName), False, TransferOptions)
                TransferResult.Check()

            Catch ex As InvalidOperationException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionLocalException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionRemoteException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As TimeoutException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Finally
                TransferOptions = Nothing
                TransferResult = Nothing
                Session.Close()
                Session = Nothing
            End Try

        End Sub

        ''' <summary>
        ''' Delete File
        ''' </summary>
        ''' <param name="SFTPRequestsParam"></param>
        ''' <remarks></remarks>
        Public Sub DeleteFile(ByRef SFTPRequestsParam As SFTPRequestsParam)

            Dim SessionOptions As New SessionOptions()

            SessionOptions.Protocol = Protocol.Sftp
            SessionOptions.HostName = Host
            SessionOptions.UserName = User
            SessionOptions.Password = Pass
            SessionOptions.PortNumber = ConfigurationManager.AppSettings("SFTPPortNumber")
            SessionOptions.SshHostKeyFingerprint = ConfigurationManager.AppSettings("SFTPCertKey")

            Dim Session As New Session()

            Session.SessionLogPath = "your log path"

            Try
                Session.Open(SessionOptions) 'Attempts to connect to your sFtp site
            Catch ex As InvalidOperationException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionLocalException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionRemoteException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As TimeoutException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Finally
                SessionOptions = Nothing
            End Try

            Dim RemoveResult As RemovalOperationResult

            Try
                RemoveResult = Session.RemoveFiles(String.Format("{0}{1}", SFTPRequestsParam.RemotePath, SFTPRequestsParam.RemoteFileName))
                RemoveResult.Check()
            Catch ex As InvalidOperationException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionLocalException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionRemoteException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As TimeoutException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Finally
                RemoveResult = Nothing
                Session.Close()
                Session = Nothing
            End Try

        End Sub

        ''' <summary>
        ''' Rename File
        ''' </summary>
        ''' <param name="SFTPRequestsParam"></param>
        ''' <remarks></remarks>
        Public Sub RenameFile(ByRef SFTPRequestsParam As SFTPRequestsParam)

            Dim SessionOptions As New SessionOptions()

            SessionOptions.Protocol = Protocol.Sftp
            SessionOptions.HostName = Host
            SessionOptions.UserName = User
            SessionOptions.Password = Pass
            SessionOptions.PortNumber = ConfigurationManager.AppSettings("SFTPPortNumber")
            SessionOptions.SshHostKeyFingerprint = ConfigurationManager.AppSettings("SFTPCertKey")

            Dim Session As New Session()

            Session.SessionLogPath = "your log path"

            Try
                Session.Open(SessionOptions) 'Attempts to connect to your sFtp site
            Catch ex As InvalidOperationException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionLocalException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionRemoteException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As TimeoutException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Finally
                SessionOptions = Nothing
            End Try

            Try
                If (Not SFTPRequestsParam.RemoteFileName.Equals(SFTPRequestsParam.RemoteNewFileName)) Then
                    If (Session.FileExists(String.Format("{0}{1}", SFTPRequestsParam.RemotePath, SFTPRequestsParam.RemoteFileName))) Then
                        Session.MoveFile(String.Format("{0}{1}", SFTPRequestsParam.RemotePath, SFTPRequestsParam.RemoteFileName),
                                         String.Format("{0}{1}", SFTPRequestsParam.RemotePath, SFTPRequestsParam.RemoteNewFileName))
                    End If
                End If

            Catch ex As InvalidOperationException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionLocalException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionRemoteException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As TimeoutException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Finally
                Session.Close()
                Session = Nothing
            End Try

        End Sub

        ''' <summary>
        ''' Create Directory
        ''' </summary>
        ''' <param name="SFTPRequestsParam"></param>
        ''' <remarks></remarks>
        Public Sub CreateDirectory(ByRef SFTPRequestsParam As SFTPRequestsParam)

            Dim SessionOptions As New SessionOptions()

            SessionOptions.Protocol = Protocol.Sftp
            SessionOptions.HostName = Host
            SessionOptions.UserName = User
            SessionOptions.Password = Pass
            SessionOptions.PortNumber = ConfigurationManager.AppSettings("SFTPPortNumber")
            SessionOptions.SshHostKeyFingerprint = ConfigurationManager.AppSettings("SFTPCertKey")

            Dim Session As New Session()

            Session.SessionLogPath = "your log path"

            Try
                Session.Open(SessionOptions) 'Attempts to connect to your sFtp site
            Catch ex As InvalidOperationException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionLocalException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionRemoteException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As TimeoutException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Finally
                SessionOptions = Nothing
            End Try

            Try
                Session.CreateDirectory(String.Format("{0}{1}", SFTPRequestsParam.RemotePath, SFTPRequestsParam.DirectoryName))
            Catch ex As InvalidOperationException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionLocalException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionRemoteException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As TimeoutException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Finally
                Session.Close()
                Session = Nothing
            End Try

        End Sub

        ''' <summary>
        ''' Get File Size
        ''' </summary>
        ''' <param name="SFTPRequestsParam"></param>
        ''' <remarks></remarks>
        Public Sub GetFileSize(ByRef SFTPRequestsParam As SFTPRequestsParam)

            Dim SessionOptions As New SessionOptions()

            SessionOptions.Protocol = Protocol.Sftp
            SessionOptions.HostName = Host
            SessionOptions.UserName = User
            SessionOptions.Password = Pass
            SessionOptions.PortNumber = ConfigurationManager.AppSettings("SFTPPortNumber")
            SessionOptions.SshHostKeyFingerprint = ConfigurationManager.AppSettings("SFTPCertKey")

            Dim Session As New Session()

            Session.SessionLogPath = "your log path"

            Try
                Session.Open(SessionOptions) 'Attempts to connect to your sFtp site
            Catch ex As InvalidOperationException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionLocalException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionRemoteException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As TimeoutException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Finally
                SessionOptions = Nothing
            End Try

            Try
                If (Session.FileExists(String.Format("{0}{1}", SFTPRequestsParam.RemotePath, SFTPRequestsParam.RemoteFileName))) Then
                    SFTPRequestsParam.FileSize = Session.GetFileInfo(String.Format("{0}{1}", SFTPRequestsParam.RemotePath, SFTPRequestsParam.RemoteFileName)).Length
                End If

            Catch ex As InvalidOperationException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionLocalException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionRemoteException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As TimeoutException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Finally
                Session.Close()
                Session = Nothing
            End Try

        End Sub

        ''' <summary>
        ''' List Directory
        ''' </summary>
        ''' <param name="SFTPRequestsParam"></param>
        ''' <remarks></remarks>
        Public Sub ListDirectory(ByRef SFTPRequestsParam As SFTPRequestsParam)

            Dim SessionOptions As New SessionOptions()

            SessionOptions.Protocol = Protocol.Sftp
            SessionOptions.HostName = Host
            SessionOptions.UserName = User
            SessionOptions.Password = Pass
            SessionOptions.PortNumber = ConfigurationManager.AppSettings("SFTPPortNumber")
            SessionOptions.SshHostKeyFingerprint = ConfigurationManager.AppSettings("SFTPCertKey")

            Dim Session As New Session()

            Session.SessionLogPath = "your log path"

            Try
                Session.Open(SessionOptions) 'Attempts to connect to your sFtp site
            Catch ex As InvalidOperationException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionLocalException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionRemoteException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As TimeoutException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Finally
                SessionOptions = Nothing
            End Try

            Dim fileInfo As RemoteFileInfo
            Dim SFTPFileInfo As SFTPFileInfo = Nothing
            Dim directory As RemoteDirectoryInfo

            Try
                directory = Session.ListDirectory(SFTPRequestsParam.RemotePath)

                For Each fileInfo In directory.Files

                    SFTPFileInfo = New SFTPFileInfo()

                    SFTPFileInfo.FileName = fileInfo.Name
                    SFTPFileInfo.FileSize = fileInfo.Length
                    SFTPFileInfo.FilePermissions = Convert.ToString(fileInfo.FilePermissions, Nothing)
                    SFTPFileInfo.LastWriteTime = fileInfo.LastWriteTime

                    SFTPRequestsParam.SFTPFileInfoList.Add(SFTPFileInfo)
                Next
            Catch ex As InvalidOperationException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionLocalException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionRemoteException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As TimeoutException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Finally
                Session.Close()
                Session = Nothing

                fileInfo = Nothing
                SFTPFileInfo = Nothing
                directory = Nothing
            End Try

        End Sub

        ''' <summary>
        ''' Get File Created Date Time
        ''' </summary>
        ''' <param name="SFTPRequestsParam"></param>
        ''' <remarks></remarks>
        Public Sub GetFileCreatedDateTime(ByRef SFTPRequestsParam As SFTPRequestsParam)

            Dim SessionOptions As New SessionOptions()

            SessionOptions.Protocol = Protocol.Sftp
            SessionOptions.HostName = Host
            SessionOptions.UserName = User
            SessionOptions.Password = Pass
            SessionOptions.PortNumber = ConfigurationManager.AppSettings("SFTPPortNumber")
            SessionOptions.SshHostKeyFingerprint = ConfigurationManager.AppSettings("SFTPCertKey")

            Dim Session As New Session()

            Session.SessionLogPath = "your log path"

            Try
                Session.Open(SessionOptions) 'Attempts to connect to your sFtp site
            Catch ex As InvalidOperationException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionLocalException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionRemoteException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As TimeoutException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Finally
                SessionOptions = Nothing
            End Try

            Try
                SFTPRequestsParam.FileCreatedDateTime = Convert.ToString(Session.GetFileInfo(
                                                         String.Format("{0}{1}", SFTPRequestsParam.RemotePath, SFTPRequestsParam.RemoteFileName)).LastWriteTime, Nothing)

            Catch ex As InvalidOperationException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionLocalException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionRemoteException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As SessionException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Catch ex As TimeoutException
                SFTPRequestsParam.ErrorMessage = ex.ToString()
            Finally
                Session.Close()
                Session = Nothing
            End Try

        End Sub

    End Class

    Public Class SFTPRequestsParam
        Inherits BaseDataObject

        Private m_FTPOperationType As String
        Public Property FTPOperationType() As String
            Get
                Return m_FTPOperationType
            End Get
            Set(value As String)
                m_FTPOperationType = value
            End Set
        End Property

        Private m_LocalPath As String
        Public Property LocalPath() As String
            Get
                Return m_LocalPath
            End Get
            Set(value As String)
                m_LocalPath = value
            End Set
        End Property

        Private m_LocalFileName As String
        Public Property LocalFileName() As String
            Get
                Return m_LocalFileName
            End Get
            Set(value As String)
                m_LocalFileName = value
            End Set
        End Property

        Private m_RemotePath As String
        Public Property RemotePath() As String
            Get
                Return m_RemotePath
            End Get
            Set(value As String)
                m_RemotePath = value
            End Set
        End Property

        Private m_RemoteFileName As String
        Public Property RemoteFileName() As String
            Get
                Return m_RemoteFileName
            End Get
            Set(value As String)
                m_RemoteFileName = value
            End Set
        End Property

        Private m_RemoteNewFileName As String
        Public Property RemoteNewFileName() As String
            Get
                Return m_RemoteNewFileName
            End Get
            Set(value As String)
                m_RemoteNewFileName = value
            End Set
        End Property

        Private m_DirectoryName As String
        Public Property DirectoryName() As String
            Get
                Return m_DirectoryName
            End Get
            Set(value As String)
                m_DirectoryName = value
            End Set
        End Property

        Private m_NewRemoteDirectoryName As String
        Public Property NewRemoteDirectoryName() As String
            Get
                Return m_NewRemoteDirectoryName
            End Get
            Set(value As String)
                m_NewRemoteDirectoryName = value
            End Set
        End Property

        Private m_FileSize As Int64
        Public Property FileSize() As Int64
            Get
                Return m_FileSize
            End Get
            Set(value As Int64)
                m_FileSize = value
            End Set
        End Property

        Public m_SFTPFileInfoList As List(Of SFTPFileInfo)
        Public Property SFTPFileInfoList() As List(Of SFTPFileInfo)
            Get
                Return m_SFTPFileInfoList
            End Get
            Set(value As List(Of SFTPFileInfo))
                m_SFTPFileInfoList = value
            End Set
        End Property

        Private m_FileCreatedDateTime As String
        Public Property FileCreatedDateTime() As String
            Get
                Return m_FileCreatedDateTime
            End Get
            Set(value As String)
                m_FileCreatedDateTime = value
            End Set
        End Property

        Private m_ErrorMessage As String
        Public Property ErrorMessage() As String
            Get
                Return m_ErrorMessage
            End Get
            Set(value As String)
                m_ErrorMessage = value
            End Set
        End Property

        Public Sub New()
            Me.FTPOperationType = InlineAssignHelper(Me.LocalPath, InlineAssignHelper(Me.LocalFileName, InlineAssignHelper(Me.RemotePath,
                                  InlineAssignHelper(Me.RemoteFileName, InlineAssignHelper(Me.RemoteNewFileName, InlineAssignHelper(Me.DirectoryName,
                                  InlineAssignHelper(Me.NewRemoteDirectoryName, InlineAssignHelper(Me.ErrorMessage, InlineAssignHelper(Me.FileCreatedDateTime,
                                                                                      String.Empty)))))))))

            Me.FileSize = 0

            Me.SFTPFileInfoList = New List(Of SFTPFileInfo)()
        End Sub
    End Class

    Public Class SFTPFileInfo
        Inherits BaseDataObject

        Private m_FileName As String
        Public Property FileName() As String
            Get
                Return m_FileName
            End Get
            Set(value As String)
                m_FileName = value
            End Set
        End Property

        Private m_FileSize As String
        Public Property FileSize() As String
            Get
                Return m_FileSize
            End Get
            Set(value As String)
                m_FileSize = value
            End Set
        End Property

        Private m_FilePermissions As String
        Public Property FilePermissions() As String
            Get
                Return m_FilePermissions
            End Get
            Set(value As String)
                m_FilePermissions = value
            End Set
        End Property

        Private m_LastWriteTime As String
        Public Property LastWriteTime() As String
            Get
                Return m_LastWriteTime
            End Get
            Set(value As String)
                m_LastWriteTime = value
            End Set
        End Property

        Public Sub New()
            Me.FileName = InlineAssignHelper(Me.FileSize, InlineAssignHelper(Me.FilePermissions, InlineAssignHelper(Me.LastWriteTime, String.Empty)))
        End Sub

    End Class

End Namespace



