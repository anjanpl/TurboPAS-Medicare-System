#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Employee Information Data Fetching
' Author: Anjan Kumar Paul
' Start Date: 03 Oct 2014
' End Date: 03 Oct 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                03 Oct 2014     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports System.Web.UI.WebControls

Namespace VisitelBusiness
    Public Class BLEmployee
        ''' <summary>
        ''' Get Employee Information
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectEmployee(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of EmployeeSettingDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()
            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectEmployee]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing


            Dim EmployeeList As New List(Of EmployeeSettingDataObject)

            If drSql.HasRows Then
                Dim objEmployeeSettingDataObject As EmployeeSettingDataObject
                While drSql.Read()
                    objEmployeeSettingDataObject = New EmployeeSettingDataObject()

                    objEmployeeSettingDataObject.EmployeeId = If((DBNull.Value.Equals(drSql("EmployeeId"))),
                                                                 objEmployeeSettingDataObject.EmployeeId,
                                                                 Convert.ToInt32(drSql("EmployeeId")))

                    objEmployeeSettingDataObject.EmployeeName = Convert.ToString(drSql("EmployeeName"), Nothing)
                    objEmployeeSettingDataObject.SocialSecurityNumber = Convert.ToString(drSql("SocialSecurityNumber"), Nothing)
                    objEmployeeSettingDataObject.Name = Convert.ToString(drSql("Name"), Nothing)

                    objEmployeeSettingDataObject.Title = If((DBNull.Value.Equals(drSql("Title"))), objEmployeeSettingDataObject.Title, Convert.ToInt32(drSql("Title")))

                    EmployeeList.Add(objEmployeeSettingDataObject)
                End While
                objEmployeeSettingDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return EmployeeList

        End Function

        Public Function SelectEmploymentStatus(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of String)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()
            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectEmploymentStatus]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim EmploymentStatusList As New List(Of String)()

            If drSql.HasRows Then
                While drSql.Read()
                    EmploymentStatusList.Add(Convert.ToString(drSql("EmploymentStatus"), Nothing))
                End While
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return EmploymentStatusList

        End Function

        Public Sub GetTitles(VisitelConnectionString As String, ByRef SqlDataSourceDropDownTitle As SqlDataSource)
            SqlDataSourceDropDownTitle.ProviderName = "System.Data.SqlClient"
            SqlDataSourceDropDownTitle.ConnectionString = VisitelConnectionString
            SqlDataSourceDropDownTitle.SelectCommandType = SqlDataSourceCommandType.StoredProcedure
            SqlDataSourceDropDownTitle.SelectCommand = "SelectTitle"
            SqlDataSourceDropDownTitle.DataBind()
        End Sub

        Public Function SelectEmployeeZipcodes(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of String)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()
            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectEmployeeZipcode]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim EmployeeZipcodeList As New List(Of String)()

            If drSql.HasRows Then
                While drSql.Read()
                    EmployeeZipcodeList.Add(Convert.ToString(drSql("Zip"), Nothing))
                End While
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return EmployeeZipcodeList

        End Function

        Public Function SelectEmployeeOigResults(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of String)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()
            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectEmployeeOigResult]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim EmployeeOigResultList As New List(Of String)()

            If drSql.HasRows Then
                While drSql.Read()
                    EmployeeOigResultList.Add(Convert.ToString(drSql("OIGResult"), Nothing))
                End While
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return EmployeeOigResultList

        End Function
    End Class
End Namespace

