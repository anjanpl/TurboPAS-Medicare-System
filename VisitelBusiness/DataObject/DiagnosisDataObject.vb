Namespace VisitelBusiness.DataObject
    Public Class DiagnosisDataObject
        Inherits BaseDataObject

        Public Property DiagnosisId As Integer

        Public Property DiagnosisCode As String

        Public Property DiagnosisDescription As String

        Public Sub New()
            Me.DiagnosisId = Int32.MinValue

            Me.DiagnosisCode = InlineAssignHelper(Me.DiagnosisDescription, String.Empty)
        End Sub
    End Class
End Namespace

