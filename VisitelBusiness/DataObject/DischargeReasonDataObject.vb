Namespace VisitelBusiness.DataObject
    Public Class DischargeReasonDataObject
        Public Property IdNumber As Integer

        Public Property Name As String

        Public Sub New()
            Me.IdNumber = Int32.MinValue
            Me.Name = String.Empty
        End Sub
    End Class
End Namespace

