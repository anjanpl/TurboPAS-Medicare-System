Namespace VisitelBusiness.DataObject
    Public Class CaseWorkerDataObject

        Public Property CaseWorkerId As Integer

        Public Property CaseWorkerName As String

        Public Sub New()
            Me.CaseWorkerId = Int32.MinValue
            Me.CaseWorkerName = String.Empty
        End Sub
    End Class
End Namespace

