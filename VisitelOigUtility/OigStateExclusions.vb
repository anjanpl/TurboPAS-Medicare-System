Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA
Imports System.IO

Namespace VisitelOigUtility
    Public Class OigStateExclusions
        Private tableName As String = "[TurboDB.OigSateExclusions]"

        Public Function ReadAndUpdateOigExclusionsDatabase(filePath As String, ConVisitel As SqlConnection) As String
            Dim errorMessage As String = String.Empty

            Try
                If (File.Exists(filePath)) Then
                    Dim dataTable As DataTable
                    errorMessage = "Can not read State Exclusions Database"

                    dataTable = ReadTextFile(filePath)

                    errorMessage = "Can not update State OIG table"
                    WriteToDatabase(ConVisitel, dataTable)
                    errorMessage = String.Empty
                Else
                    errorMessage = "File not found"
                End If

            Catch ex As Exception
            End Try

            Return errorMessage
        End Function

        Private Function ReadTextFile(filePath As String) As DataTable
            Dim dataTable As DataTable = Nothing

            Dim file As System.IO.StreamWriter
            file = My.Computer.FileSystem.OpenTextFileWriter("D:\task\Johnny Nosike\Bitbucket\Visitel\VisitelWeb\OIG\test.txt", True)

            Dim file__1 As String = System.IO.File.ReadAllText(filePath).Replace(vbCr & vbLf, " ")

            Dim file1 As New FileStream(filePath, FileMode.Open, FileAccess.ReadWrite)

            Dim wr As New StreamWriter(file1)

            ' Write your content here
            wr.Write(file__1)

            wr.Close()
            file1.Close()


            Try
                Dim line As String
                Dim lineNo As Integer = 0

                Using reader As StreamReader = New StreamReader(filePath)
                    line = reader.ReadLine()
                    lineNo = lineNo + 1

                    While (Not line Is Nothing)
                        Dim columnValues() As String = line.Split(New String() {vbTab}, StringSplitOptions.None)
                        Dim dataRow As DataRow = Nothing
                        Dim columnIndex As Integer = 0

                        For Each value As String In columnValues
                            If (lineNo = 1) Then
                                If (dataTable Is Nothing) Then
                                    dataTable = New DataTable("UpdatedTable")
                                End If

                                dataTable.Columns.Add(value.Replace("""", ""))
                            Else
                                If (dataRow Is Nothing) Then
                                    dataRow = dataTable.NewRow()
                                    dataTable.Rows.Add(dataRow)
                                End If

                                dataRow(columnIndex) = value.Replace("""", "")
                            End If

                            columnIndex = columnIndex + 1
                        Next

                        Try
                            line = reader.ReadLine()
                            file.WriteLine(line)
                        Catch ex As Exception

                        End Try





                        lineNo = lineNo + 1
                    End While
                End Using
            Catch ex As Exception
                File.Close()
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
