Namespace VisitelBusiness.DataObject.Settings
    Public Class CaseWorkerSettingDataObject
        Inherits BaseDataObject

        Public Property CaseWorkerId As Integer

        Public Property CaseWorkerCode As String

        Public Property FirstName As String

        Public Property LastName As String

        Public Property Phone As String

        Public Property AlternatePhone As String

        Public Property Address As String

        Public Property Suite As String

        Public Property CityId As Integer

        Public Property CityName As String

        Public Property State As String

        Public Property Zip As String

        Public Property Fax As String

        Public Property MailCode As String

        Public Property EntryDate As String

        Public Property UpdateBy As String

        Public Property Status As Int16

        Public Property Comments As String

        Public Property UpdateDate As String

        Public Property Email As String

        Public Property FullAddress As String

        Public Property CompanyId As Integer

        Public Property UserId As Integer

        Public Sub New()
            Me.CaseWorkerId = InlineAssignHelper(Me.CompanyId, InlineAssignHelper(Me.UserId, InlineAssignHelper(Me.CityId, Int32.MinValue)))
            Me.FirstName = InlineAssignHelper(Me.CaseWorkerCode, InlineAssignHelper(Me.LastName, InlineAssignHelper(Me.Phone,
                           InlineAssignHelper(Me.AlternatePhone, InlineAssignHelper(Me.Address, InlineAssignHelper(Me.Suite,
                           InlineAssignHelper(Me.CityName, InlineAssignHelper(Me.State, InlineAssignHelper(Me.Zip, InlineAssignHelper(Me.Fax,
                           InlineAssignHelper(Me.MailCode, InlineAssignHelper(Me.EntryDate, InlineAssignHelper(Me.UpdateBy,
                           InlineAssignHelper(Me.Comments, InlineAssignHelper(Me.UpdateDate, InlineAssignHelper(Me.Email,
                           InlineAssignHelper(Me.FullAddress, String.Empty)))))))))))))))))

            Me.Status = Int16.MinValue
        End Sub
    End Class
End Namespace
