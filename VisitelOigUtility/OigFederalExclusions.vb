Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Net
Imports System.IO
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA
Imports ICSharpCode.SharpZipLib.Zip

Namespace VisitelOigUtility
    Public Class OigFederalExclusions

        Private tableName As String = "[TurboDB.OigFederalExclusions]"

        ''' <summary>
        ''' Downloads and saves OIG Federal Exclusions Database and saves on specified location
        ''' </summary>
        ''' <param name="downloadUrl">OIG Federal Exclusions Database url</param>
        ''' <param name="downloadDirectory">Directory path of downloaded file</param>
        ''' <param name="fileName">Downloaded file name</param>
        ''' <returns>Error Message</returns>
        ''' <remarks></remarks>
        Public Function DownloadOigFile(downloadUrl As String, downloadDirectory As String, fileName As String) As String
            Dim errorMessage As String = String.Empty

            Try
                If (Not Directory.Exists(downloadDirectory)) Then
                    Directory.CreateDirectory(downloadDirectory)
                End If
                Dim webClient As New WebClient()
                webClient.DownloadFile(downloadUrl, String.Format("{0}{1}", downloadDirectory, fileName))
            Catch ex As Exception
                errorMessage = "Can not download file"
            End Try

            Return errorMessage
        End Function

        ''' <summary>
        ''' Unzips downloaded .zip file
        ''' </summary>
        ''' <param name="zipFileDirectory">Zip file directory</param>
        ''' <param name="zipFileName">Zip file name</param>
        ''' <returns>Error Message</returns>
        ''' <remarks></remarks>
        Public Function UnzipOigFile(zipFileDirectory As String, zipFileName As String) As String
            Dim errorMessage As String = String.Empty

            Try
                Using zipInputStream As ZipInputStream = New ZipInputStream(File.OpenRead(String.Format("{0}{1}", zipFileDirectory, zipFileName)))
                    Dim objEntry As ZipEntry

                    objEntry = zipInputStream.GetNextEntry()
                    While IsNothing(objEntry) = False

                        Dim directoryName As String = Path.GetDirectoryName(objEntry.Name)
                        Dim fileName As String = Path.GetFileName(objEntry.Name)

                        'create directory
                        If (directoryName.Length > 0) Then
                            Directory.CreateDirectory(directoryName)
                        End If

                        If (Not String.IsNullOrEmpty(fileName)) Then
                            Using streamWriter As FileStream = File.Create(String.Format("{0}{1}", zipFileDirectory, objEntry.Name))
                                Dim size As Integer = 2048
                                Dim data(size) As Byte

                                While True
                                    size = zipInputStream.Read(data, 0, data.Length)
                                    If (size > 0) Then
                                        streamWriter.Write(data, 0, size)
                                    Else
                                        Exit While
                                    End If
                                End While
                            End Using
                        End If

                        objEntry = zipInputStream.GetNextEntry()

                    End While
                End Using

                File.Delete(String.Format("{0}{1}", zipFileDirectory, zipFileName))
            Catch ex As Exception
                errorMessage = "Can not unzip file"
            End Try

            Return errorMessage
        End Function

        ''' <summary>
        ''' Reads Federal Execlusions Database (.dbf) file and updates Federal OIG table
        ''' </summary>
        ''' <param name="fileDirectory">File (.dbf) Directory</param>
        ''' <param name="fileNameWithoutExtension">File (.dbf) name without extension</param>
        ''' <param name="ConVisitel">SqlConnection</param>
        ''' <returns>Error Message</returns>
        ''' <remarks></remarks>
        Public Function ReadAndUpdateOigExclusionsDatabase(fileDirectory As String, fileNameWithoutExtension As String, ConVisitel As SqlConnection) As String
            Dim errorMessage As String = String.Empty

            Try
                Dim dataTable As DataTable
                errorMessage = "Can not read Federal Exclusions Database"

                If (Environment.Is64BitProcess) Then
                    ''Requires Microsoft Access Database Engine 2010
                    ''errorMessage = errorMessage + " using 64Bit"
                    dataTable = ReadDbfUsingAceOledb(fileDirectory, fileNameWithoutExtension)
                Else
                    dataTable = ReadDbfUsingJetOledb(fileDirectory, fileNameWithoutExtension)
                    ''dataTable = ReadDbfUsingOdbc(fileDirectory, fileNameWithoutExtension)
                End If

                errorMessage = "Can not update Federal OIG table"
                WriteToDatabase(ConVisitel, dataTable)
                errorMessage = String.Empty
            Catch ex As Exception
                errorMessage = errorMessage + " because: " + ex.Message
            End Try

            Return errorMessage
        End Function

        Private Function ReadDbfUsingJetOledb(fileDirectory As String, fileNameWithoutExtension As String) As DataTable
            Dim dataTable As DataTable = Nothing

            Try
                Dim connectionString As String = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=dBASE IV;User ID=Admin;Password=;", fileDirectory)
                Dim selectQuery = String.Format("SELECT * FROM {0}", fileNameWithoutExtension)

                Using oConn As OleDbConnection = New OleDbConnection(connectionString)
                    dataTable = New DataTable("UpdatedTable")
                    Dim dataAdapter As OleDbDataAdapter = New OleDbDataAdapter(selectQuery, oConn)
                    dataAdapter.Fill(dataTable)
                    dataAdapter.Dispose()
                End Using
            Catch ex As Exception
                Throw ex
            End Try

            Return dataTable
        End Function

        Private Function ReadDbfUsingOdbc(fileDirectory As String, fileNameWithoutExtension As String) As DataTable
            Dim dataTable As DataTable = Nothing

            Try
                Dim connectionString As String = "Driver={Microsoft dBASE Driver (*.dbf)};Driverid=277;" + String.Format("Dbq={0}", fileDirectory)
                Dim selectQuery = String.Format("SELECT * FROM {0}", fileNameWithoutExtension)

                Using oConn As OleDbConnection = New OleDbConnection(connectionString)
                    dataTable = New DataTable("UpdatedTable")
                    Dim dataAdapter As OleDbDataAdapter = New OleDbDataAdapter(selectQuery, oConn)
                    dataAdapter.Fill(dataTable)
                End Using
            Catch ex As Exception
                Throw ex
            End Try

            Return dataTable
        End Function

        Private Function ReadDbfUsingAceOledb(fileDirectory As String, fileNameWithoutExtension As String) As DataTable
            Dim dataTable As DataTable = Nothing

            Try
                Dim connectionString As String = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=dBASE IV;User ID=Admin;", fileDirectory)
                Dim selectQuery = String.Format("SELECT * FROM {0}", fileNameWithoutExtension)

                Using oConn As OleDbConnection = New OleDbConnection(connectionString)
                    dataTable = New DataTable("UpdatedTable")
                    Dim dataAdapter As OleDbDataAdapter = New OleDbDataAdapter(selectQuery, oConn)
                    dataAdapter.Fill(dataTable)
                End Using
            Catch ex As Exception
                Throw ex
            End Try

            Return dataTable
        End Function

        Private Function WriteToDatabase(ConVisitel As SqlConnection, dataTable As DataTable) As String
            Dim errorMessage As String = String.Empty

            Try
                Dim objSharedSettings As New SharedSettings()

                objSharedSettings.ExecuteQuery(String.Format("TRUNCATE TABLE {0}", tableName), ConVisitel)

                Dim bulkCopy As SqlBulkCopy = New SqlBulkCopy(ConVisitel)
                bulkCopy.DestinationTableName = tableName
                bulkCopy.WriteToServer(dataTable)

                Dim parameters As New HybridDictionary()
                parameters.Add("@TableName", tableName)
                parameters.Add("@UpdateBy", "")
                objSharedSettings.ExecuteQuery("", "[TurboDB.InsertTableUpdateLog]", ConVisitel, parameters)

                objSharedSettings = Nothing
                parameters = Nothing
            Catch ex As Exception
                Throw ex
            End Try

            Return errorMessage
        End Function

        Public Function GetLastUpdatedDateTime(ConVisitel As SqlConnection) As DateTime
            Dim returnValue As DateTime = DateTime.MinValue

            Try
                Dim objSharedSettings As New SharedSettings()
                Dim drSql As SqlDataReader = Nothing
                Dim parameters As New HybridDictionary()

                parameters.Add("@TableName", tableName)
                objSharedSettings.GetDataReader("", "[TurboDB.SelectTableUpdateLog]", drSql, ConVisitel, parameters)

                objSharedSettings = Nothing
                parameters = Nothing

                If drSql.HasRows Then
                    While drSql.Read()
                        returnValue = If((DBNull.Value.Equals(drSql("UpdateDate"))),
                                              DateTime.MinValue, Convert.ToDateTime(drSql("UpdateDate"), Nothing))
                    End While
                End If

            Catch ex As Exception
                Throw ex
            End Try

            Return returnValue
        End Function

    End Class
End Namespace
