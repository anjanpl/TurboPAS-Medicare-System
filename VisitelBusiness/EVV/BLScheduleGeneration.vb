#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Vesta Schedule Generation
' Author: Anjan Kumar Paul
' Start Date: 08 Jan 2016
' End Date: 08 Jan 2016
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                08 Jan 2016     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA
Imports VisitelBusiness.VisitelBusiness.DataObject

Namespace VisitelBusiness
    Public Class BLScheduleGeneration

        Public Sub GenerateVestaSchedule(ByRef ConVisitel As SqlConnection, CompanyId As Integer, PayPeriodFrom As String, PayPeriodTo As String, DaysBack As Integer,
                                         ClientId As Int64, ClientType As Integer, ClientGroup As Integer, UpdateBy As String, EVVName As String)

            Dim objSharedSettings As New SharedSettings()
            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)
            parameters.Add("@PayPeriodFrom", PayPeriodFrom)
            parameters.Add("@PayPeriodTo", PayPeriodTo)
            parameters.Add("@DaysBack", DaysBack)

            If (ClientId > 0) Then
                parameters.Add("@ClientId", ClientId)
            End If

            If (ClientType > 0) Then
                parameters.Add("@ClientType", ClientType)
            End If

            If (ClientGroup > 0) Then
                parameters.Add("@ClientGroup", ClientGroup)
            End If

            parameters.Add("@UpdateBy", UpdateBy)

            Select Case EVVName
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EVVName.Vesta)
                    objSharedSettings.ExecuteQuery("", "[TurboDB.GenerateVestaSchedule]", ConVisitel, parameters)
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EVVName.MEDsys)
                    objSharedSettings.ExecuteQuery("", "[TurboDB.GenerateMEDsysSchedule]", ConVisitel, parameters)
                    Exit Select
            End Select

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub SyncRecords(ByRef ConVisitel As SqlConnection, UpdateBy As String, EVVName As String)

            Dim objSharedSettings As New SharedSettings()
            Dim parameters As New HybridDictionary()

            parameters.Add("@UpdateBy", UpdateBy)

            Select Case EVVName
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EVVName.Vesta)
                    objSharedSettings.ExecuteQuery("", "[TurboDB.VestaScheduleGenerationSyncTables]", ConVisitel, parameters)
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EVVName.MEDsys)
                    objSharedSettings.ExecuteQuery("", "[TurboDB.MEDsysScheduleGenerationSyncTables]", ConVisitel, parameters)
                    Exit Select
            End Select

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub GenerateEmployeeEVVId(ByRef ConVisitel As SqlConnection)
            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.GenerateEmployeeEVVId]", ConVisitel, Nothing)
            objSharedSettings = Nothing
        End Sub

        Public Sub GenerateClientEVVId(ByRef ConVisitel As SqlConnection)
            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.GenerateClientEVVId]", ConVisitel, Nothing)
            objSharedSettings = Nothing
        End Sub
    End Class
End Namespace

