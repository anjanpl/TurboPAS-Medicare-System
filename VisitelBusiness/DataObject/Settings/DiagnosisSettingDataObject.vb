Namespace VisitelBusiness.DataObject.Settings
    Public Class DiagnosisSettingDataObject
        Inherits BaseDataObject

        Public Property DiagnosisId As Integer

        Public Property Code As String

        Public Property Description As String

        Public Property UpdateBy As String

        Public Property UpdateDate As String

        Public Property UserId As Integer

        Public Sub New()
            Me.DiagnosisId = InlineAssignHelper(Me.UserId, Int32.MinValue)
            Me.Code = InlineAssignHelper(Me.Description, InlineAssignHelper(Me.UpdateBy, InlineAssignHelper(Me.UpdateDate, String.Empty)))
        End Sub
    End Class
End Namespace

