Namespace VisitelBusiness.DataObject
    Public Class CaseInfoDataObject

        Public Property Id As Integer

        Public Property ClientId As Integer

        Public Property CaseWorkerId As Integer

        Public Property EmployeeId As Integer

        Public Property CaseInfoDate As String

        Public Property ReceiverOrganization As String

        Public Property Comments As String

        Public Property UpdateBy As String

        Public Property UpdateDate As String

        Public Property CompanyId As Integer

        Public Property UserId As Integer

        Public Sub New()

            Me.Id = Me.ClientId = Me.CaseWorkerId = Me.EmployeeId = Me.CompanyId = Me.UserId = Integer.MinValue
            'Me.CaseInfoDate = Me.Comments = Me.UpdateBy = Me.UpdateDate = String.Empty
            Me.CaseInfoDate = String.Empty
            Me.ReceiverOrganization = String.Empty
            Me.Comments = String.Empty
            Me.UpdateBy = String.Empty
            Me.UpdateDate = String.Empty
        End Sub
    End Class
End Namespace

