
Namespace VisitelBusiness.DataObject.Settings
    Public Class HospitalStaySettingDataObject
        Inherits BaseDataObject

        Public Property HospitalStayId As Int64

        Public Property IndividualId As Int64

        Public Property IndividualName As String

        Public Property StartDate As String

        Public Property EndDate As String

        Public Property Reason As String

        Public Property MedicaidNo As String

        Public Property Comment As String

        Public Property UpdateBy As String

        Public Property UpdateDate As String

        Public Property CompanyId As Int32

        Public Property UserId As Int64

        Public Sub New()
            Me.HospitalStayId = InlineAssignHelper(Me.IndividualId, InlineAssignHelper(Me.UserId, Int64.MinValue))
            Me.StartDate = InlineAssignHelper(Me.EndDate, InlineAssignHelper(Me.Reason, InlineAssignHelper(Me.UpdateBy, InlineAssignHelper(Me.UpdateDate, String.Empty))))
            Me.CompanyId = Int32.MinValue
        End Sub
    End Class
End Namespace

