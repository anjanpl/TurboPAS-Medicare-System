Namespace VisitelBusiness.DataObject
    Public Class ReceiverDataObject
        Public Property ReceiverId As Integer

        Public Property ReceiverName As String
        
        Public Sub New()
            Me.ReceiverId = Int32.MinValue
            Me.ReceiverName = String.Empty
        End Sub
    End Class
End Namespace

