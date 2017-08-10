#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Diagnosis Information Data Fetching
' Author: Anjan Kumar Paul
' Start Date: 06 Sept 2014
' End Date: 06 Sept 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                06 Sept 2014     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness
    Public Class BLDiagnosis

        Public Sub SelectDiagnosis(ConVisitel As SqlConnection, ByRef drSql As SqlDataReader)
            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectDiagnosis]", drSql, ConVisitel, Nothing)

            objSharedSettings = Nothing
        End Sub
    End Class
End Namespace

