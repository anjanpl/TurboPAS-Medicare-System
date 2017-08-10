
Namespace VisitelBusiness.DataObject.Settings
    Public Class StateSettingDataObject
        Inherits BaseDataObject

        Public Property StateId As Integer

        Public Property StateShortName As String

        Public Property StateFullName As String

        Public Property UpdateBy As String

        Public Property UpdateDate As String

        Public Property CompanyId As Integer

        Public Property UserId As Integer
        
        Public Sub New()
            Me.StateId = InlineAssignHelper(Me.CompanyId, InlineAssignHelper(Me.UserId, Int32.MinValue))
            Me.StateShortName = InlineAssignHelper(Me.StateFullName, InlineAssignHelper(Me.UpdateBy, InlineAssignHelper(Me.UpdateDate, String.Empty)))
        End Sub
    End Class
End Namespace

