Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.Settings

Namespace VisitelBusiness
    Public Class BLOigExclusionsDatabase

        Private sqlConnection As SqlConnection

        Public Function GetOgiFederalReportData(VisitelConnectionString As String, QueryStringCollection As NameValueCollection) As DataSet

            Dim CrystalReportDataSet As New DataSet()

            Try
                sqlConnection = New SqlConnection(VisitelConnectionString)

                Dim companyId As Integer = If(String.IsNullOrEmpty(QueryStringCollection("CompanyId")),
                                              0, Convert.ToInt32(QueryStringCollection("CompanyId")))

                Dim blCompanySetting As BLCompanySetting = New BLCompanySetting()
                CrystalReportDataSet.Tables.Add(blCompanySetting.GetCompanyInformation(sqlConnection, companyId))
                blCompanySetting = Nothing

                Dim sqlCommand As SqlCommand = New SqlCommand("[TurboDB.SelectOigFederalSearchReport]", sqlConnection)
                sqlCommand.CommandType = CommandType.StoredProcedure

                Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter(sqlCommand)
                Dim dataTable As DataTable = New DataTable("SearchResult")
                dataAdapter.Fill(dataTable)
                CrystalReportDataSet.Tables.Add(dataTable)

            Catch ex As Exception

            Finally

            End Try

            Return CrystalReportDataSet

        End Function

        Public Function GetOgiStateReportData(VisitelConnectionString As String, QueryStringCollection As NameValueCollection) As DataSet

            Dim CrystalReportDataSet As New DataSet()

            Try
                sqlConnection = New SqlConnection(VisitelConnectionString)

                Dim companyId As Integer = If(String.IsNullOrEmpty(QueryStringCollection("CompanyId")),
                                              0, Convert.ToInt32(QueryStringCollection("CompanyId")))

                Dim blCompanySetting As BLCompanySetting = New BLCompanySetting()
                CrystalReportDataSet.Tables.Add(blCompanySetting.GetCompanyInformation(sqlConnection, companyId))
                blCompanySetting = Nothing

                Dim sqlCommand As SqlCommand = New SqlCommand("[TurboDB.SelectOigStateSearchReport]", sqlConnection)
                sqlCommand.CommandType = CommandType.StoredProcedure

                Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter(sqlCommand)
                Dim dataTable As DataTable = New DataTable("SearchResult")
                dataAdapter.Fill(dataTable)
                CrystalReportDataSet.Tables.Add(dataTable)

            Catch ex As Exception

            Finally

            End Try

            Return CrystalReportDataSet

        End Function

    End Class
End Namespace
