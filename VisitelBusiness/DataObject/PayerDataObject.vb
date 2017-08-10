Namespace VisitelBusiness.DataObject
    Public Class PayerDataObject
        Public Property PayerId As Integer

        Public Property PayerName As String
        
        Public Sub New()
            Me.PayerId = Int32.MinValue
            Me.PayerName = String.Empty
        End Sub
    End Class
End Namespace

