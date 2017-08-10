Namespace VisitelBusiness.DataObject.Settings
    Public Class MaritalStatusSettingDataObject
        Inherits BaseDataObject

        Public Property IdNumber As Integer

        Public Property Name As String

        Public Property Status As Int16

        Public Property UpdateBy As String

        Public Property UpdateDate As String

        Public Property CompanyId As Integer

        Public Property UserId As Integer

        Public Sub New()
            Me.IdNumber = InlineAssignHelper(Me.CompanyId, InlineAssignHelper(Me.UserId, Int32.MinValue))
            Me.Name = InlineAssignHelper(Me.UpdateBy, InlineAssignHelper(Me.UpdateDate, String.Empty))

            Me.Status = Int16.MinValue
        End Sub
    End Class
End Namespace

