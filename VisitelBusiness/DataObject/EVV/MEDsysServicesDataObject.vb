Namespace VisitelBusiness.DataObject.EVV
    Public Class MEDsysServicesDataObject
        Inherits BaseDataObject

        Public Property Id As Int64

        Public Property AccountId As String

        Public Property ServiceCode As String

        Public Property ServiceName As String

        Public Property UpdateDate As String

        Public Property UpdateBy As String

        Public Property Remarks As String

        Public Sub New()
            Me.AccountId = InlineAssignHelper(Me.ServiceCode, InlineAssignHelper(Me.ServiceName, InlineAssignHelper(Me.UpdateDate,
                           InlineAssignHelper(Me.UpdateBy, InlineAssignHelper(Me.Remarks, String.Empty)))))

            Me.Id = Int64.MinValue
        End Sub
    End Class
End Namespace