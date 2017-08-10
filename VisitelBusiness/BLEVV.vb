#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Santrax Information Data Fetching
' Author: Anjan Kumar Paul
' Start Date: 17 Sept 2014
' End Date: 17 Sept 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                17 Sept 2014     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness
    Public Class BLEVV

        Public Sub SelectEVV(ConVisitel As SqlConnection, ByRef drSql As SqlDataReader)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectEVV]", drSql, ConVisitel, Nothing)

            objSharedSettings = Nothing

        End Sub
    End Class
End Namespace

