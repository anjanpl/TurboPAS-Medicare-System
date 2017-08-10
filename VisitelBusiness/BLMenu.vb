
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Menu Inserting, Updating, Deleting, & Fetching
' Author: Anjan Kumar Paul
' Start Date: 18 Jan 2014
' End Date: 
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                18 Jan 2014     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA
Imports VisitelBusiness.VisitelBusiness.DataObject


<DataObjectAttribute()> _
Public Class BLMenu

    <DataObjectMethod(DataObjectMethodType.Insert, True)> _
    Public Sub InsertMenuInfo(ConVisitel As SqlConnection, objMenuDataObject As MenuDataObject)

        Dim parameters As New HybridDictionary()
        parameters.Add("@ParentID", objMenuDataObject.ParentId)
        parameters.Add("@PermissionID", objMenuDataObject.PermissionId)
        parameters.Add("@MenuText", objMenuDataObject.MenuText)
        parameters.Add("@NavigateUrl", objMenuDataObject.NavigateUrl)

        Dim objSharedSettings As New SharedSettings()

        objSharedSettings.ExecuteQuery("", "[TurboDB.InsertMenuInfo]", ConVisitel, parameters)

        objSharedSettings = Nothing
        parameters = Nothing
    End Sub


    <DataObjectMethod(DataObjectMethodType.Update)> _
    Public Sub UpdateMenuInfo(ConVisitel As SqlConnection, objMenuDataObject As MenuDataObject)

        Dim parameters As New HybridDictionary()

        parameters.Add("@MenuId", objMenuDataObject.MenuId)
        parameters.Add("@ParentID", objMenuDataObject.ParentId)
        parameters.Add("@PermissionID", objMenuDataObject.PermissionId)
        parameters.Add("@MenuText", objMenuDataObject.MenuText)
        parameters.Add("@NavigateUrl", objMenuDataObject.NavigateUrl)
        parameters.Add("@MenuOrder", objMenuDataObject.MenuOrder)
        parameters.Add("@IsVisible", objMenuDataObject.IsVisible)

        Dim objSharedSettings As New SharedSettings()

        objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateMenuInfo]", ConVisitel, parameters)

        objSharedSettings = Nothing
        parameters = Nothing

    End Sub

    <DataObjectMethod(DataObjectMethodType.Delete)> _
    Public Sub DeleteMenuInfo(ConVisitel As SqlConnection, MenuId As Integer)

        Dim parameters As New HybridDictionary()

        parameters.Add("@MenuId", MenuId)

        Dim objSharedSettings As New SharedSettings()

        objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteMenuInfo]", ConVisitel, parameters)

        objSharedSettings = Nothing
        parameters = Nothing

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ConVisitel"></param>
    ''' <param name="UserID"></param>
    ''' <returns>List containing the menu row</returns>
    ''' <remarks></remarks>
    <DataObjectMethod(DataObjectMethodType.[Select])> _
    Public Function SelectMenuInfo(ConVisitel As SqlConnection, UserID As Integer) As List(Of MenuDataObject)

        Dim drSql As SqlDataReader = Nothing

        Dim parameters As New HybridDictionary()

        parameters.Add("@UserID", UserID)

        Dim objSharedSettings As New SharedSettings()

        objSharedSettings.GetDataReader("", "[TurboDB.SelectMenuInfo]", drSql, ConVisitel, parameters)

        objSharedSettings = Nothing
        parameters = Nothing

        Dim MenuList As New List(Of MenuDataObject)()

        If drSql.HasRows Then
            Dim objMenuDataObject As MenuDataObject
            While drSql.Read()
                objMenuDataObject = New MenuDataObject()

                objMenuDataObject.MenuId = If(Convert.IsDBNull(drSql("MenuId")), 0, Convert.ToInt32(drSql("MenuId")))
                objMenuDataObject.ParentId = If(Convert.IsDBNull(drSql("ParentID")), -1, Convert.ToInt32(drSql("ParentID")))
                objMenuDataObject.PermissionId = If(Convert.IsDBNull(drSql("PermissionID")), 0, Convert.ToInt32(drSql("PermissionID")))
                objMenuDataObject.MenuText = If(Convert.IsDBNull(drSql("MenuText")), String.Empty, Convert.ToString(drSql("MenuText")))
                objMenuDataObject.NavigateUrl = If(Convert.IsDBNull(drSql("NavigateUrl")), String.Empty, Convert.ToString(drSql("NavigateUrl")))
                objMenuDataObject.MenuOrder = If(Convert.IsDBNull(drSql("MenuOrder")), 0, Convert.ToInt32(drSql("MenuOrder")))
                objMenuDataObject.IsVisible = Convert.ToBoolean(drSql("IsVisible"))
                objMenuDataObject.PermissionName = If(Convert.IsDBNull(drSql("PermissionName")), String.Empty, Convert.ToString(drSql("PermissionName")))

                MenuList.Add(objMenuDataObject)

            End While
            objMenuDataObject = Nothing
        End If
        Return MenuList
    End Function

End Class