Namespace VisitelBusiness.DataObject
    Public Class StateDataObject
        Public Property StateId As Integer
        
        Public Property StateFullName As String
        
        Public Sub New()
            Me.StateId = Int32.MinValue
            Me.StateFullName = String.Empty
        End Sub
    End Class
End Namespace

