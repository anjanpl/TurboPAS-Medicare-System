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

Imports System.Net
Imports System.IO

Namespace VisitelCommon

    Public Class FTPRequests

        Private Host As String = Nothing
        Private User As String = Nothing
        Private Pass As String = Nothing
        Private FTPRequest As FtpWebRequest = Nothing
        Private FTPResponse As FtpWebResponse = Nothing
        Private FTPStream As Stream = Nothing
        Private BufferSize As Integer = 2048

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
        ''' Download File 
        ''' </summary>
        ''' <param name="RemoteFile"></param>
        ''' <param name="LocalFile"></param>
        ''' <remarks></remarks>
        Public Sub DownloadFile(RemoteFile As String, LocalFile As String)

            Dim LocalFileStream As FileStream = Nothing

            Try

                'Create an FTP Request 
                FTPRequest = DirectCast(FtpWebRequest.Create(Convert.ToString(Host & Convert.ToString("/", Nothing), Nothing) & RemoteFile), FtpWebRequest)

                FTPRequest.Credentials = New NetworkCredential(User, Pass) 'Log in to the FTP Server with the User Name and Password Provided 

                FTPRequest.UseBinary = True
                FTPRequest.UsePassive = True
                FTPRequest.KeepAlive = True

                FTPRequest.Method = WebRequestMethods.Ftp.DownloadFile 'Specify the Type of FTP Request 

                FTPResponse = DirectCast(FTPRequest.GetResponse(), FtpWebResponse) 'Establish Return Communication with the FTP Server 

                FTPStream = FTPResponse.GetResponseStream() 'Get the FTP Server's Response Stream 

                LocalFileStream = New FileStream(LocalFile, FileMode.Create) 'Open a File Stream to Write the Downloaded File 

                Dim ByteBuffer As Byte() = New Byte(BufferSize - 1) {} 'Buffer for the Downloaded Data 
                Dim BytesRead As Integer = FTPStream.Read(ByteBuffer, 0, BufferSize)

                'Download the File by Writing the Buffered Data Until the Transfer is Complete 
                Try
                    While BytesRead > 0
                        LocalFileStream.Write(ByteBuffer, 0, BytesRead)
                        BytesRead = FTPStream.Read(ByteBuffer, 0, BufferSize)
                    End While
                Catch ex As Exception
                    'Console.WriteLine(Convert.ToString(ex, Nothing))
                End Try

            Catch ex As Exception
                'Console.WriteLine(Convert.ToString(ex, Nothing))
            Finally
                'Resource Cleanup 
                LocalFileStream.Close()
                LocalFileStream = Nothing

                FTPStream.Close()
                FTPResponse.Close()
                FTPRequest = Nothing
            End Try

            Return

        End Sub

        ''' <summary>
        ''' Upload File
        ''' </summary>
        ''' <param name="RemoteFile"></param>
        ''' <param name="LocalFile"></param>
        ''' <remarks></remarks>
        Public Sub UploadFile(RemoteFile As String, LocalFile As String)

            Dim LocalFileStream As FileStream = Nothing

            Try
                'Create an FTP Request 
                FTPRequest = DirectCast(FtpWebRequest.Create(Convert.ToString(Host & Convert.ToString("/", Nothing), Nothing) & RemoteFile), FtpWebRequest)

                FTPRequest.Credentials = New NetworkCredential(User, Pass) 'Log in to the FTP Server with the User Name and Password Provided 

                FTPRequest.UseBinary = True
                FTPRequest.UsePassive = True
                FTPRequest.KeepAlive = True

                FTPRequest.Method = WebRequestMethods.Ftp.UploadFile 'Specify the Type of FTP Request 

                FTPStream = FTPRequest.GetRequestStream() 'Establish Return Communication with the FTP Server 

                LocalFileStream = New FileStream(LocalFile, FileMode.Create) 'Open a File Stream to Read the File for Upload 

                Dim ByteBuffer As Byte() = New Byte(BufferSize - 1) {} 'Buffer for the Downloaded Data 
                Dim BytesSent As Integer = LocalFileStream.Read(ByteBuffer, 0, BufferSize)

                'Upload the File by Sending the Buffered Data Until the Transfer is Complete 
                Try
                    While BytesSent <> 0
                        FTPStream.Write(ByteBuffer, 0, BytesSent)
                        BytesSent = LocalFileStream.Read(ByteBuffer, 0, BufferSize)
                    End While
                Catch ex As Exception
                    'Console.WriteLine(Convert.ToString(ex, Nothing))
                Finally
                    'Resource Cleanup 
                    LocalFileStream.Close()
                    LocalFileStream = Nothing

                    FTPStream.Close()
                    FTPRequest = Nothing
                End Try

            Catch ex As Exception
                'Console.WriteLine(Convert.ToString(ex, Nothing))
            End Try

            Return

        End Sub

        ''' <summary>
        ''' Delete File 
        ''' </summary>
        ''' <param name="DeleteFile"></param>
        ''' <remarks></remarks>
        Public Sub DeleteFile(DeleteFile As String)

            Try

                'Create an FTP Request 
                FTPRequest = DirectCast(WebRequest.Create(Convert.ToString(Host & Convert.ToString("/", Nothing), Nothing) & DeleteFile), FtpWebRequest)

                FTPRequest.Credentials = New NetworkCredential(User, Pass) 'Log in to the FTP Server with the User Name and Password Provided 

                FTPRequest.UseBinary = True
                FTPRequest.UsePassive = True
                FTPRequest.KeepAlive = True

                FTPRequest.Method = WebRequestMethods.Ftp.DeleteFile 'Specify the Type of FTP Request 

                FTPResponse = DirectCast(FTPRequest.GetResponse(), FtpWebResponse) 'Establish Return Communication with the FTP Server 

            Catch ex As Exception
                'Console.WriteLine(Convert.ToString(ex, Nothing))
            Finally
                ' Resource Cleanup 
                FTPResponse.Close()
                FTPRequest = Nothing
            End Try

            Return

        End Sub

        ''' <summary>
        ''' Rename File
        ''' </summary>
        ''' <param name="CurrentFileNameAndPath"></param>
        ''' <param name="NewFileName"></param>
        ''' <remarks></remarks>
        Public Sub RenameFile(CurrentFileNameAndPath As String, NewFileName As String)

            Try
                'Create an FTP Request 
                FTPRequest = DirectCast(WebRequest.Create(Convert.ToString(Host & Convert.ToString("/", Nothing), Nothing) & CurrentFileNameAndPath), FtpWebRequest)

                FTPRequest.Credentials = New NetworkCredential(User, Pass) 'Log in to the FTP Server with the User Name and Password Provided 

                FTPRequest.UseBinary = True
                FTPRequest.UsePassive = True
                FTPRequest.KeepAlive = True

                FTPRequest.Method = WebRequestMethods.Ftp.Rename 'Specify the Type of FTP Request 

                FTPRequest.RenameTo = NewFileName 'Rename the File 

                FTPResponse = DirectCast(FTPRequest.GetResponse(), FtpWebResponse) 'Establish Return Communication with the FTP Server 

            Catch ex As Exception
                'Console.WriteLine(Convert.ToString(ex, Nothing))
            Finally
                'Resource Cleanup 
                FTPResponse.Close()
                FTPRequest = Nothing
            End Try

            Return

        End Sub

        ''' <summary>
        ''' Create a New Directory on the FTP Server 
        ''' </summary>
        ''' <param name="NewDirectory"></param>
        ''' <remarks></remarks>
        Public Sub CreateDirectory(NewDirectory As String)

            Try

                'Create an FTP Request 
                FTPRequest = DirectCast(WebRequest.Create(Convert.ToString(Host & Convert.ToString("/", Nothing), Nothing) & NewDirectory), FtpWebRequest)

                FTPRequest.Credentials = New NetworkCredential(User, Pass) 'Log in to the FTP Server with the User Name and Password Provided 

                FTPRequest.UseBinary = True
                FTPRequest.UsePassive = True
                FTPRequest.KeepAlive = True

                FTPRequest.Method = WebRequestMethods.Ftp.MakeDirectory 'Specify the Type of FTP Request 

                FTPResponse = DirectCast(FTPRequest.GetResponse(), FtpWebResponse) 'Establish Return Communication with the FTP Server 

            Catch ex As Exception
                'Console.WriteLine(Convert.ToString(ex, Nothing))
            Finally
                'Resource Cleanup 
                FTPResponse.Close()
                FTPRequest = Nothing
            End Try

            Return

        End Sub

        ''' <summary>
        ''' Get the Date/Time a File was Created 
        ''' </summary>
        ''' <param name="FileName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetFileCreatedDateTime(FileName As String) As String

            Dim FTPReader As StreamReader = Nothing

            Try

                'Create an FTP Request 
                FTPRequest = DirectCast(FtpWebRequest.Create(Convert.ToString(Host & Convert.ToString("/", Nothing), Nothing) & FileName), FtpWebRequest)

                FTPRequest.Credentials = New NetworkCredential(User, Pass) 'Log in to the FTP Server with the User Name and Password Provided 

                FTPRequest.UseBinary = True
                FTPRequest.UsePassive = True
                FTPRequest.KeepAlive = True

                FTPRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp 'Specify the Type of FTP Request 

                FTPResponse = DirectCast(FTPRequest.GetResponse(), FtpWebResponse) 'Establish Return Communication with the FTP Server 

                FTPStream = FTPResponse.GetResponseStream() 'Establish Return Communication with the FTP Server 

                FTPReader = New StreamReader(FTPStream) 'Get the FTP Server's Response Stream

                Dim FileInfo As String = Nothing

                'Read the Full Response Stream 
                Try
                    FileInfo = FTPReader.ReadToEnd()
                Catch ex As Exception
                    'Console.WriteLine(Convert.ToString(ex, Nothing))
                End Try

                Return FileInfo 'Return File Created Date Time 

            Catch ex As Exception
                'Console.WriteLine(Convert.ToString(ex, Nothing))
            Finally
                'Resource Cleanup 
                FTPReader.Close()
                FTPStream.Close()
                FTPResponse.Close()
                FTPRequest = Nothing
            End Try

            Return String.Empty 'Return an Empty string Array if an Exception Occurs 

        End Function

        ''' <summary>
        ''' Get the Size of a File 
        ''' </summary>
        ''' <param name="FileName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetFileSize(FileName As String) As String

            Try
                'Create an FTP Request 
                FTPRequest = DirectCast(FtpWebRequest.Create(Convert.ToString(Host & Convert.ToString("/", Nothing), Nothing) & FileName), FtpWebRequest)

                FTPRequest.Credentials = New NetworkCredential(User, Pass) 'Log in to the FTP Server with the User Name and Password Provided 

                FTPRequest.UseBinary = True
                FTPRequest.UsePassive = True
                FTPRequest.KeepAlive = True

                FTPRequest.Method = WebRequestMethods.Ftp.GetFileSize 'Specify the Type of FTP Request 

                FTPResponse = DirectCast(FTPRequest.GetResponse(), FtpWebResponse) 'Establish Return Communication with the FTP Server 

                FTPStream = FTPResponse.GetResponseStream() 'Establish Return Communication with the FTP Server 

                Dim FTPReader As New StreamReader(FTPStream) 'Get the FTP Server's Response Stream 

                Dim FileInfo As String = Nothing

                'Read the Full Response Stream 
                Try
                    While FTPReader.Peek() <> -1
                        FileInfo = FTPReader.ReadToEnd()
                    End While
                Catch ex As Exception
                    'Console.WriteLine(Convert.ToString(ex, Nothing))
                Finally
                    'Resource Cleanup 
                    FTPReader.Close()
                    FTPStream.Close()
                    FTPResponse.Close()
                    FTPRequest = Nothing
                End Try

                Return FileInfo 'Return File Size 

            Catch ex As Exception
                'Console.WriteLine(Convert.ToString(ex, Nothing))
            End Try

            Return String.Empty 'Return an Empty string Array if an Exception Occurs 

        End Function

        ''' <summary>
        ''' List Directory Contents File/Folder Name Only 
        ''' </summary>
        ''' <param name="Directory"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DirectoryListSimple(Directory As String) As String()

            Try

                'Create an FTP Request 
                FTPRequest = DirectCast(FtpWebRequest.Create(Convert.ToString(Host & Convert.ToString("/", Nothing), Nothing) & Directory), FtpWebRequest)

                FTPRequest.Credentials = New NetworkCredential(User, Pass) 'Log in to the FTP Server with the User Name and Password Provided 

                FTPRequest.UseBinary = True
                FTPRequest.UsePassive = True
                FTPRequest.KeepAlive = True

                FTPRequest.Method = WebRequestMethods.Ftp.ListDirectory 'Specify the Type of FTP Request 

                FTPResponse = DirectCast(FTPRequest.GetResponse(), FtpWebResponse) 'Establish Return Communication with the FTP Server 

                FTPStream = FTPResponse.GetResponseStream() 'Establish Return Communication with the FTP Server 

                Dim FTPReader As New StreamReader(FTPStream) 'Get the FTP Server's Response Stream 

                Dim DirectoryRaw As String = Nothing

                'Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing 
                Try
                    While FTPReader.Peek() <> -1
                        DirectoryRaw += FTPReader.ReadLine() + "|"
                    End While
                Catch ex As Exception
                    'Console.WriteLine(Convert.ToString(ex, Nothing))
                Finally
                    ' Resource Cleanup 
                    FTPReader.Close()
                    FTPStream.Close()
                    FTPResponse.Close()
                    FTPRequest = Nothing
                End Try

                'Return the Directory Listing as a string Array by Parsing 'DirectoryRaw' with the Delimiter you Append (I use | in This Example) 
                Dim DirectoryList As String()
                Try
                    DirectoryList = DirectoryRaw.Split("|".ToCharArray())
                    Return DirectoryList
                Catch ex As Exception
                    'Console.WriteLine(Convert.ToString(ex, Nothing))
                End Try

                DirectoryList = Nothing

            Catch ex As Exception
                'Console.WriteLine(Convert.ToString(ex, Nothing))
            End Try

            Return New String() {String.Empty} 'Return an Empty string Array if an Exception Occurs 

        End Function

        ''' <summary>
        ''' List Directory Contents in Detail (Name, Size, Created, etc.) 
        ''' </summary>
        ''' <param name="Directory"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DirectoryListDetailed(Directory As String) As String()

            Try

                ' Create an FTP Request 
                FTPRequest = DirectCast(FtpWebRequest.Create(Convert.ToString(Host & Convert.ToString("/", Nothing), Nothing) & Directory), FtpWebRequest)

                FTPRequest.Credentials = New NetworkCredential(User, Pass) ' Log in to the FTP Server with the User Name and Password Provided 

                FTPRequest.UseBinary = True
                FTPRequest.UsePassive = True
                FTPRequest.KeepAlive = True

                FTPRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails ' Specify the Type of FTP Request 

                FTPResponse = DirectCast(FTPRequest.GetResponse(), FtpWebResponse) ' Establish Return Communication with the FTP Server 

                FTPStream = FTPResponse.GetResponseStream() ' Get the FTP Server's Response Stream 

                Dim FTPReader As New StreamReader(FTPStream) ' Store the Raw Response 

                Dim DirectoryRaw As String = Nothing

                ' Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing 
                Try
                    While FTPReader.Peek() <> -1
                        DirectoryRaw += FTPReader.ReadLine() + "|"
                    End While
                Catch ex As Exception
                    'Console.WriteLine(Convert.ToString(ex, Nothing))
                Finally
                    ' Resource Cleanup 
                    FTPReader.Close()
                    FTPStream.Close()
                    FTPResponse.Close()
                    FTPRequest = Nothing
                End Try

                ' Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) 
                Dim DirectoryList As String()
                Try
                    DirectoryList = DirectoryRaw.Split("|".ToCharArray())
                    Return DirectoryList
                Catch ex As Exception
                    'Console.WriteLine(Convert.ToString(ex, Nothing))
                Finally
                    DirectoryList = Nothing
                End Try

            Catch ex As Exception
                'Console.WriteLine(Convert.ToString(ex, Nothing))
            End Try

            Return New String() {String.Empty} ' Return an Empty string Array if an Exception Occurs 

        End Function

    End Class

End Namespace



