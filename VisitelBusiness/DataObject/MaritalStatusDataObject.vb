Namespace VisitelBusiness.DataObject
    Public Class MaritalStatusDataObject
        Public Property MaritalStatusId As Integer
        
        Public Property MaritalStatusName As String
        
        Public Sub New()
            Me.MaritalStatusId = Int32.MinValue
            Me.MaritalStatusName = String.Empty
        End Sub
    End Class
End Namespace

