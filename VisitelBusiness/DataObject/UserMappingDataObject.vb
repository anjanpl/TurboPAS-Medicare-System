Namespace VisitelBusiness.DataObject

    Public Class UserMappingDataObject
        Inherits BaseDataObject

        Public Property UserId As Int64

        Public Property UserName As String

        Public Property Password As String

        Public Property Email As String

        Public Property UserType As String

        Public Property UserTypeId As Integer

        Public Property TurboPASUserName As String

        Public Property UpdateDate As String

        Public Property UpdateBy As String

        Public Property Remarks As String

        Public Sub New()
            Me.UserName = InlineAssignHelper(Me.Password, InlineAssignHelper(Me.Email, InlineAssignHelper(Me.UserType,
                          InlineAssignHelper(Me.TurboPASUserName, InlineAssignHelper(Me.UpdateDate, InlineAssignHelper(Me.UpdateBy,
                          InlineAssignHelper(Me.Remarks, String.Empty)))))))

            Me.UserId = Integer.MinValue
        End Sub
    End Class

End Namespace
