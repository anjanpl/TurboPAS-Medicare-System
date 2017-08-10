Imports System.ComponentModel
Imports System.Collections.Specialized
Imports System.Data.SqlClient
Imports VisitelDA.VisitelDA
Imports VisitelBusiness.VisitelBusiness.DataObject

Namespace VisitelBusiness

    Public Class BLUserPrivilege

        Public Sub UserPrivilegeAdd(ByRef ConVisitel As SqlConnection, Xml As String, RoleId As Integer, UserId As Integer)

            Dim parameters As New HybridDictionary()

            parameters.Add("@xml", Xml)

            If (RoleId > 0) Then
                parameters.Add("@RoleId", RoleId)
            End If

            If (UserId > 0) Then
                parameters.Add("@UserId", UserId)
            End If

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UserPrivilegeAdd]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        ''' <summary>
        ''' Get role based user privilege
        ''' </summary>
        ''' <returns></returns>
        Public Function UserPrivilegeGet(ByRef ConVisitel As SqlConnection, RoleId As Integer, UserId As Integer) As List(Of UserPrivilegeDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            If RoleId > 0 Then
                parameters.Add("@RoleID", RoleId)
            End If

            If UserId > 0 Then
                parameters.Add("@UserID", UserId)
            End If

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.UserPrivilegeGet]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim UserPrivileges As New List(Of UserPrivilegeDataObject)
            FillDataList(drSql, UserPrivileges)

            Return UserPrivileges

        End Function

        ''' <summary>
        ''' Get user privilege
        ''' </summary>
        ''' <returns></returns>
        <DataObjectMethod(DataObjectMethodType.[Select])> _
        Public Function UserPrivilegeGetByUserId(ByRef ConVisitel As SqlConnection, UserId As Integer) As IList(Of UserPrivilegeDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            If UserId > 0 Then
                parameters.Add("@UserId", UserId)
            End If

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.UserPrivilegeGetByUserId]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim UserPrivileges As New List(Of UserPrivilegeDataObject)
            FillDataList(drSql, UserPrivileges)

            Return UserPrivileges

        End Function

        Private Sub FillDataList(ByRef drSql As SqlDataReader, ByRef UserPrivileges As List(Of UserPrivilegeDataObject))

            If drSql.HasRows Then
                Dim objUserPrivilegeDataObject As UserPrivilegeDataObject
                While drSql.Read()
                    objUserPrivilegeDataObject = New UserPrivilegeDataObject()

                    objUserPrivilegeDataObject.PermissionId = If((DBNull.Value.Equals(drSql("PermissionId"))),
                                                                 objUserPrivilegeDataObject.PermissionId, Convert.ToInt32(drSql("PermissionId")))

                    objUserPrivilegeDataObject.HasPermission = If((DBNull.Value.Equals(drSql("HasPermission"))),
                                                                  objUserPrivilegeDataObject.HasPermission, Convert.ToBoolean(drSql("HasPermission"), Nothing))

                    objUserPrivilegeDataObject.PermissionName = Convert.ToString(drSql("PermissionName"), Nothing).Trim()

                    objUserPrivilegeDataObject.RoleId = If((DBNull.Value.Equals(drSql("RoleId"))),
                                                                 objUserPrivilegeDataObject.RoleId, Convert.ToInt32(drSql("RoleId")))

                    objUserPrivilegeDataObject.ModuleName = Convert.ToString(drSql("ModuleName"), Nothing).Trim()

                    objUserPrivilegeDataObject.UserId = If((DBNull.Value.Equals(drSql("UserId"))),
                                                                 objUserPrivilegeDataObject.UserId, Convert.ToInt32(drSql("UserId")))

                    objUserPrivilegeDataObject.ViewOrder = If((DBNull.Value.Equals(drSql("ViewOrder"))),
                                                                 objUserPrivilegeDataObject.ViewOrder, Convert.ToInt32(drSql("ViewOrder")))

                    UserPrivileges.Add(objUserPrivilegeDataObject)
                End While
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing
        End Sub

        ''' <summary>
        ''' Get role based user privilege
        ''' </summary>
        ''' <returns></returns>
        '<DataObjectMethod(DataObjectMethodType.[Select])> _
        'Public Function UserPrivilegeGetDataTable(RoleID As Integer, UserID As Integer) As DataTable
        '    Dim arlParams As New ArrayList()
        '    If RoleID > 0 Then
        '        arlParams.Add(New SqlParameter("@RoleID", RoleID))
        '    End If

        '    If UserID > 0 Then
        '        arlParams.Add(New SqlParameter("@UserID", UserID))
        '    End If


        '    Using dt As DataTable = ExecuteStoredProcedureDataTable("UserPrivilegeGet", arlParams)
        '        Return dt
        '    End Using
        'End Function


        ''' <summary>
        ''' Add user to privilege
        ''' </summary>
        ''' <param name="xml">User group previleged xml
        '''         <UserPrivileges>
        '''             <UserPrivilege>
        '''                 <PermissionID>1</PermissionID>
        '''             </UserPrivilege>
        '''             <UserPrivilege>
        '''                 <PermissionID>4</PermissionID>
        '''             </UserPrivilege>
        '''             <UserPrivilege>
        '''                 <PermissionID>8</PermissionID>
        '''             </UserPrivilege>
        '''         </UserPrivileges>
        ''' </param>
        ''' <param name="RoleID"></param>
        ''' <returns></returns>
        '<DataObjectMethod(DataObjectMethodType.Insert)> _
        'Public Function UserPrivilegeAdd(xml As String, RoleID As Integer, UserID As Integer) As DataTable
        '    Dim dt As New DataTable()
        '    Dim arlParams As New ArrayList()
        '    arlParams.Add(New SqlParameter("@xml", xml))
        '    If RoleID > 0 Then
        '        arlParams.Add(New SqlParameter("@RoleID", RoleID))
        '    End If

        '    If UserID > 0 Then
        '        arlParams.Add(New SqlParameter("@UserID", UserID))
        '    End If

        '    Return InlineAssignHelper(dt, ExecuteStoredProcedureDataTable("UserPrivilegeAdd", arlParams))
        'End Function

        ''' <summary>
        ''' Fill object collection with data from datatable
        ''' </summary>
        ''' <param name="dt">Datatable</param>
        ''' <param name="list">UserPrivilege collection</param>
        'Private Shared Sub Fill(dt As DataTable, list As IList(Of UserPrivilege))
        '    For Each dr As DataRow In dt.Rows
        '        Dim entity As New UserPrivilege()

        '        Fill(entity, dr)

        '        list.Add(entity)
        '    Next
        'End Sub

        ''' <summary>
        ''' Fill Tokens object with data from datarow
        ''' </summary>
        ''' <param name="_UserPrivilege">UserPrivilege object</param>
        ''' <param name="dRow">Datarow</param>
        'Private Shared Sub Fill(_UserPrivilege As UserPrivilege, dRow As DataRow)
        '    _UserPrivilege.PermissionID = Convert.ToInt32(dRow("PermissionID"))
        '    _UserPrivilege.PermissionName = Convert.ToString(dRow("PermissionName"))
        '    _UserPrivilege.HasPermission = Convert.ToBoolean(dRow("HasPermission"))
        '    _UserPrivilege.RoleID = Convert.ToInt32(dRow("RoleID"))
        '    _UserPrivilege.ModuleName = Convert.ToString(dRow("ModuleName"))
        'End Sub

    End Class
End Namespace