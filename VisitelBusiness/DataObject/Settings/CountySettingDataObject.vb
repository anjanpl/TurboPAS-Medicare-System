
Namespace VisitelBusiness.DataObject.Settings
    Public Class CountySettingDataObject
        Inherits BaseDataObject

        Public Property CountyId As Integer

        Public Property CountyCode As String

        Public Property CountyName As String

        Public Property UpdateBy As String

        Public Property UpdateDate As String

        Public Property CompanyId As Integer

        Public Property UserId As Integer
        
        Public Sub New()

            Me.CountyId = InlineAssignHelper(Me.CompanyId, InlineAssignHelper(Me.UserId, Int32.MinValue))

            Me.CountyCode = InlineAssignHelper(Me.CountyName, InlineAssignHelper(Me.UpdateBy, InlineAssignHelper(Me.UpdateDate, String.Empty)))
        End Sub
    End Class
End Namespace

