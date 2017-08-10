Namespace VisitelBusiness.DataObject
    Public Class ClientGroupDataObject
        Public Property ClientGroupId As Integer

        Public Property ClientGroupName As String
        
        Public Sub New()
            Me.ClientGroupId = Int32.MinValue
            Me.ClientGroupName = String.Empty
        End Sub
    End Class
End Namespace

