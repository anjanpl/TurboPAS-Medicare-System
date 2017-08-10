Namespace VisitelBusiness.DataObject
    Public Class DoctorDataObject
        Public Property DoctorId As Integer

        Public Property DoctorName As String

        Public Sub New()
            Me.DoctorId = Int32.MinValue
            Me.DoctorName = String.Empty
        End Sub
    End Class
End Namespace

