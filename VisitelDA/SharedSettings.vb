
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Data
Imports System.Data.SqlClient

Namespace VisitelDA
    Public Class SharedSettings

        Public _CommandTimeout As Integer = 0

        Public Sub ExecuteQuery(sql As String, sqlCon As SqlConnection)
            ExecuteQueryPerform(sql, sqlCon)
        End Sub

        Protected Sub ExecuteQueryPerform(sql As String, sqlCon As SqlConnection)
            Dim strSQLInsert As [String]
            Dim sqlCmd As SqlCommand
            strSQLInsert = sql
            sqlCmd = New SqlCommand(strSQLInsert, sqlCon)
            sqlCmd.CommandTimeout = _CommandTimeout
            sqlCmd.ExecuteNonQuery()
            sqlCmd.Dispose()
            sqlCmd = Nothing
        End Sub

        Public Sub ExecuteQuery(sql As String, sqlCon As SqlConnection, parameters As HybridDictionary)
            ExecuteQueryPerform(sql, sqlCon, parameters)
        End Sub

        Protected Sub ExecuteQueryPerform(sql As String, sqlCon As SqlConnection, parameters As HybridDictionary)
            Dim sqlCmd As SqlCommand

            sqlCmd = New SqlCommand(sql, sqlCon)

            If parameters IsNot Nothing Then
                For Each de As DictionaryEntry In parameters
                    sqlCmd.Parameters.AddWithValue(de.Key.ToString(), de.Value.ToString())
                Next
            End If

            sqlCmd.CommandTimeout = _CommandTimeout
            sqlCmd.ExecuteNonQuery()
            sqlCmd.Dispose()
            sqlCmd = Nothing
        End Sub

        Public Sub ExecuteQuery(Sql As String, ProcedureName As String, SqlCon As SqlConnection, Parameters As HybridDictionary)
            ExecuteQueryPerform(Sql, ProcedureName, SqlCon, Parameters)
        End Sub

        Protected Sub ExecuteQueryPerform(Sql As String, ProcedureName As String, SqlCon As SqlConnection, Parameters As HybridDictionary)
            Dim SqlCmd As SqlCommand

            SqlCmd = New SqlCommand(Sql, SqlCon)

            If Parameters IsNot Nothing Then
                For Each de As DictionaryEntry In Parameters
                    If de.Value.[GetType]() = GetType(Byte()) Then
                        SqlCmd.Parameters.AddWithValue(de.Key.ToString(), de.Value)
                    ElseIf de.Value.[GetType]() = GetType(Guid) Then
                        SqlCmd.Parameters.AddWithValue(de.Key.ToString(), de.Value)
                    Else
                        SqlCmd.Parameters.AddWithValue(de.Key.ToString(), de.Value.ToString())
                    End If
                Next
            End If

            SqlCmd.CommandText = ProcedureName
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.CommandTimeout = _CommandTimeout

            SqlCmd.ExecuteNonQuery()
            SqlCmd.Dispose()
            SqlCmd = Nothing
        End Sub

        Public Sub GetDataReader(Sql As String, ByRef Sdr As SqlDataReader, Conn As SqlConnection)
            ExecuteDataReader(Sql, Sdr, Conn)
        End Sub

        Protected Sub ExecuteDataReader(Sql As String, ByRef Sdr As SqlDataReader, Conn As SqlConnection)
            Dim SqlCmd As New SqlCommand(Sql, Conn)
            SqlCmd.CommandTimeout = _CommandTimeout
            Sdr = SqlCmd.ExecuteReader()
            SqlCmd.Dispose()
            SqlCmd = Nothing
        End Sub

        Public Sub GetDataReader(Sql As String, ByRef Sdr As SqlDataReader, Conn As SqlConnection, Parameters As HybridDictionary)
            ExecuteDataReader(Sql, Sdr, Conn, Parameters)
        End Sub

        Protected Sub ExecuteDataReader(Sql As String, ByRef Sdr As SqlDataReader, Conn As SqlConnection, Parameters As HybridDictionary)
            Dim SqlCmd As New SqlCommand(Sql, Conn)
            If Parameters IsNot Nothing Then
                For Each de As DictionaryEntry In Parameters
                    SqlCmd.Parameters.AddWithValue(Convert.ToString(de.Key, Nothing), Convert.ToString(de.Value, Nothing))
                Next
            End If

            SqlCmd.CommandTimeout = _CommandTimeout
            Sdr = SqlCmd.ExecuteReader()
            SqlCmd.Dispose()
            SqlCmd = Nothing
        End Sub

        Public Sub GetDataReader(Sql As String, ProcedureName As String, ByRef Sdr As SqlDataReader, Conn As SqlConnection)
            ExecuteDataReader(Sql, ProcedureName, Sdr, Conn)
        End Sub

        Protected Sub ExecuteDataReader(Sql As String, ProcedureName As String, ByRef Sdr As SqlDataReader, Conn As SqlConnection)
            Dim SqlCmd As New SqlCommand(Sql, Conn)

            SqlCmd.CommandText = ProcedureName
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.CommandTimeout = _CommandTimeout

            Sdr = SqlCmd.ExecuteReader()
            SqlCmd.Dispose()
            SqlCmd = Nothing
        End Sub

        Public Sub GetDataReader(Sql As String, ProcedureName As String, ByRef Sdr As SqlDataReader, Conn As SqlConnection, Parameters As HybridDictionary)
            ExecuteDataReader(Sql, ProcedureName, Sdr, Conn, Parameters)
        End Sub

        Protected Sub ExecuteDataReader(Sql As String, ProcedureName As String, ByRef Sdr As SqlDataReader, Conn As SqlConnection, Parameters As HybridDictionary)
            Dim SqlCmd As New SqlCommand(Sql, Conn)
            If Parameters IsNot Nothing Then
                For Each de As DictionaryEntry In Parameters
                    If de.Value.[GetType]() = GetType(DateTime) Then
                        SqlCmd.Parameters.AddWithValue(de.Key.ToString(), de.Value)
                    Else
                        SqlCmd.Parameters.AddWithValue(Convert.ToString(de.Key, Nothing), Convert.ToString(de.Value, Nothing))
                    End If
                Next
            End If

            SqlCmd.CommandText = ProcedureName
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.CommandTimeout = _CommandTimeout

            Sdr = SqlCmd.ExecuteReader()
            SqlCmd.Dispose()
            SqlCmd = Nothing
        End Sub

        Public Sub GetDataSet(Sql As String, ByRef Sds As DataSet, Conn As SqlConnection, Parameters As HybridDictionary)
            ExecuteDataSet(Sql, Sds, Conn, Parameters)
        End Sub

        Protected Sub ExecuteDataSet(Sql As String, ByRef Sds As DataSet, Conn As SqlConnection, Parameters As HybridDictionary)
            Dim SqlCmd As New SqlCommand(Sql, Conn)

            If Parameters IsNot Nothing Then
                For Each de As DictionaryEntry In Parameters
                    SqlCmd.Parameters.AddWithValue(de.Key.ToString(), de.Value.ToString())
                Next
            End If

            SqlCmd.CommandTimeout = _CommandTimeout

            Dim SqlDa As New SqlDataAdapter(SqlCmd)

            SqlDa.Fill(Sds)

            SqlCmd.Dispose()
            SqlCmd = Nothing
            SqlDa.Dispose()
            SqlDa = Nothing
        End Sub

        Public Sub GetDataSet(Sql As String, ProcedureName As String, ByRef Sds As DataSet, Conn As SqlConnection, Parameters As HybridDictionary)
            ExecuteDataSet(Sql, ProcedureName, Sds, Conn, Parameters)
        End Sub

        Public Sub ExecuteDataSet(Sql As String, ProcedureName As String, ByRef Sds As DataSet, Conn As SqlConnection, Parameters As HybridDictionary)
            Dim SqlCmd As New SqlCommand(Sql, Conn)

            If Parameters IsNot Nothing Then
                For Each de As DictionaryEntry In Parameters
                    SqlCmd.Parameters.AddWithValue(de.Key.ToString(), de.Value.ToString())
                Next
            End If

            SqlCmd.CommandText = ProcedureName
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.CommandTimeout = _CommandTimeout

            Dim SqlDa As New SqlDataAdapter(SqlCmd)
            SqlDa.Fill(Sds)

            SqlCmd.Dispose()
            SqlCmd = Nothing
            SqlDa.Dispose()
            SqlDa = Nothing
        End Sub

        Public Function GetDataSet(Sql As String, ProcedureName As String, ByRef Sds As DataSet, Conn As SqlConnection, Parameters As HybridDictionary _
                              , OutPutParameter As String) As String

            Return ExecuteDataSet(Sql, ProcedureName, Sds, Conn, Parameters, OutPutParameter)

        End Function

        Public Function ExecuteDataSet(Sql As String, ProcedureName As String, ByRef Sds As DataSet, Conn As SqlConnection, Parameters As HybridDictionary _
                                       , OutPutParameter As String) As String

            Dim ReturnValue As String = String.Empty

            Dim SqlCmd As New SqlCommand(Sql, Conn)

            If Parameters IsNot Nothing Then
                For Each de As DictionaryEntry In Parameters
                    SqlCmd.Parameters.AddWithValue(de.Key.ToString(), de.Value.ToString())
                Next
            End If

            Dim OutParameter As New SqlParameter
            If (Not String.IsNullOrEmpty(String.Format("@{0}", OutPutParameter))) Then

                OutParameter.ParameterName = OutPutParameter
                OutParameter.DbType = SqlDbType.NVarChar
                OutParameter.Size = -1
                OutParameter.Direction = ParameterDirection.Output
                SqlCmd.Parameters.Add(OutParameter)
            End If

            SqlCmd.CommandText = ProcedureName
            SqlCmd.CommandType = CommandType.StoredProcedure
            SqlCmd.CommandTimeout = _CommandTimeout

            Dim SqlDa As New SqlDataAdapter(SqlCmd)
            SqlDa.Fill(Sds)

            ReturnValue = Convert.ToString(OutParameter.Value, Nothing)

            OutParameter = Nothing

            SqlCmd.Dispose()
            SqlCmd = Nothing
            SqlDa.Dispose()
            SqlDa = Nothing

            Return ReturnValue

        End Function
    End Class
End Namespace

