Namespace VisitelBusiness.DataObject
    Public Class ClientDataObject
        Inherits BaseDataObject

        Public Property ClientInfoId As Int64
        
        Public Property ClientName As String

        Public Property Address As String

        Public Property StateClientId As Integer

        Public Property SocialSecurityNumber As String

        Public Property Name As String

        Public Property CaseWorkerId As Integer

        Public Property InsuranceNumber As String

        Public Sub New()
            Me.StateClientId = InlineAssignHelper(Me.CaseWorkerId, Int32.MinValue)

            Me.ClientInfoId = Int64.MinValue

            Me.ClientName = InlineAssignHelper(Me.Name, InlineAssignHelper(Me.SocialSecurityNumber, InlineAssignHelper(Me.InsuranceNumber, String.Empty)))
        End Sub
    End Class
End Namespace

