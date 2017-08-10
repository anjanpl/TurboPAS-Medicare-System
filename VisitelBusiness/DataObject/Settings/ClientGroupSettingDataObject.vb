Namespace VisitelBusiness.DataObject.Settings
    Public Class ClientGroupSettingDataObject
        Inherits BaseDataObject

        Public Property GroupId As Integer
        
        Public Property GroupName As String

        Public Property UpdateBy As String

        Public Property UpdateDate As String

        Public Property CompanyId As Integer

        Public Property UserId As Integer
        
        Public Sub New()
            Me.GroupId = InlineAssignHelper(Me.CompanyId, InlineAssignHelper(Me.UserId, Int32.MinValue))
            Me.GroupName = InlineAssignHelper(Me.UpdateBy, InlineAssignHelper(Me.UpdateDate, String.Empty))
        End Sub
    End Class
End Namespace

