Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA
Imports System.Web.UI.WebControls

Namespace VisitelBusiness.Settings
    Public Class BLCompanySetting
        Public Function GetCompanyInformation(ConVisitel As SqlConnection, CompanyId As Integer) As DataTable

            Dim dataTable As DataTable = New DataTable("CompanyInformation")

            Try
                Dim sqlCommand As SqlCommand = New SqlCommand("[TurboDB.SelectCompanyInformation]", ConVisitel)
                sqlCommand.CommandType = CommandType.StoredProcedure
                sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId)

                Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter(sqlCommand)
                dataAdapter.Fill(dataTable)
            Catch ex As Exception

            Finally

            End Try

            Return dataTable

        End Function

        Public Function GetCompanyInfoOrderedDictionary(VisitelConnectionString As String, CompanyId As Integer) As OrderedDictionary

            Dim companyInformation As OrderedDictionary = Nothing

            Try
                Dim ConVisitel As SqlConnection = New SqlConnection(VisitelConnectionString)
                Dim dataTable As DataTable = GetCompanyInformation(ConVisitel, CompanyId)

                If (dataTable.Rows.Count > 0) Then
                    companyInformation = New OrderedDictionary()
                End If

                Dim columnName As String
                For counter As Integer = 0 To dataTable.Columns.Count
                    columnName = dataTable.Columns(counter).ColumnName
                    companyInformation.Add(columnName, dataTable.Rows(0)(columnName))
                Next
            Catch ex As Exception

            Finally

            End Try

            Return companyInformation

        End Function

        Public Function GetDefaultTimesheet(ConVisitel As SqlConnection, CompanyId As Integer) As Integer

            Dim defaultTimesheet As Integer = 1

            Try
                Dim sqlCommand As SqlCommand = New SqlCommand("[TurboDB.SelectDefaultTimesheet]", ConVisitel)
                sqlCommand.CommandType = CommandType.StoredProcedure
                sqlCommand.Parameters.AddWithValue("@CompanyId", CompanyId)

                Dim timesheet As Object = sqlCommand.ExecuteScalar()
                If (Not IsDBNull(timesheet)) Then
                    defaultTimesheet = Convert.ToInt32(timesheet)
                End If
            Catch ex As Exception

            Finally

            End Try

            Return defaultTimesheet

        End Function

        Public Sub UpdateDefaultTimesheet(ConVisitel As SqlConnection, CompanyId As Integer, DefaultTimesheet As Integer)

            Dim parameters As New HybridDictionary()
            Dim objSharedSettings As New SharedSettings()

            parameters.Add("@DefaultTimesheet", DefaultTimesheet)
            parameters.Add("@CompanyId", CompanyId)

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateDefaultTimesheet]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub SelectCompanyInfo(VisitelConnectionString As String, ByRef SqlDataSourceCompanyInfo As SqlDataSource)

            SqlDataSourceCompanyInfo.ProviderName = "System.Data.SqlClient"
            SqlDataSourceCompanyInfo.ConnectionString = VisitelConnectionString

            SqlDataSourceCompanyInfo.SelectCommandType = SqlDataSourceCommandType.StoredProcedure

            SqlDataSourceCompanyInfo.SelectParameters.Clear()

            SqlDataSourceCompanyInfo.SelectCommand = "[TurboDB.SelectCompanyInformation]"
            SqlDataSourceCompanyInfo.DataBind()

        End Sub
    End Class
End Namespace

