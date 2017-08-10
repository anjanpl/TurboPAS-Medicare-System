
Imports System.ComponentModel

Namespace VisitelBusiness.DataObject
    <DataObjectAttribute()> _
    Public Class MenuDataObject
        Public Property MenuId As Int64

        Public Property ParentId As Int64

        Public Property PermissionId As Int64

        Public Property PermissionName As String

        Public Property MenuText As String

        Public Property NavigateUrl As String

        Public Property MenuOrder As Int16

        Public Property IsVisible As Boolean

        Public Property UpdateDate As String

        Public Property UpdateBy As String

        Public Property CompanyId As Integer

        Public Property UserId As Integer

        Public Sub New()
            Me.MenuId = Int64.MinValue
            Me.ParentId = Int64.MinValue
            Me.PermissionId = Int64.MinValue
            Me.PermissionName = String.Empty
            Me.MenuText = String.Empty
            Me.MenuOrder = Int16.MinValue
            Me.IsVisible = False
            Me.UpdateDate = String.Empty
            Me.UpdateBy = String.Empty
            Me.CompanyId = Integer.MinValue
            Me.UserId = Integer.MinValue
        End Sub
    End Class
End Namespace