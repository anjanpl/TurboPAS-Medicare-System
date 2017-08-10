Namespace VisitelBusiness.DataObject
    Public Class ClientStatusDataObject
        Public Property ClientStatusId As Integer

        Public Property ClientStatusName As String

        Public Sub New()
            Me.ClientStatusId = Int32.MinValue
            Me.ClientStatusName = String.Empty
        End Sub
    End Class
End Namespace

