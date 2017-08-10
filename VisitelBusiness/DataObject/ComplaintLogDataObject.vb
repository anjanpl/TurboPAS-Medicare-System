Namespace VisitelBusiness.DataObject
    Public Class ComplaintLogDataObject
        Public Property ComplaintId As Integer

        Public Property ClientId As Integer

        Public Property ComplainantName As String

        Public Property ComplaintDate As DateTime

        Public Property Regarding As String

        Public Property OthersInvolved As String

        Public Property ReportedBy As String

        Public Property NatureOfProblems As String

        Public Property ComplaintReceiver As String

        Public Property ActionTaken As String

        Public Property ReportCompletedBy As String

        Public Property Agree As Boolean

        Public Property Disagree As Boolean

        Public Property UpdateDate As DateTime

        Public Property UpdateBy As String

        Public Property CompanyId As Integer

        Public Property UserId As Integer

        Public Property SSMATimeStamp As TimeSpan

        Public Sub New()
            Me.ComplaintId = Int32.MinValue
            Me.ClientId = Int32.MinValue
            Me.ComplainantName = String.Empty
            Me.ComplaintDate = DateTime.MinValue
            Me.Regarding = String.Empty
            Me.OthersInvolved = String.Empty
            Me.ReportedBy = String.Empty
            Me.NatureOfProblems = String.Empty
            Me.ComplaintReceiver = String.Empty
            Me.ActionTaken = String.Empty
            Me.ReportCompletedBy = String.Empty
            Me.Agree = False
            Me.Disagree = False
            Me.UpdateDate = DateTime.MinValue
            Me.CompanyId = Me.UserId = Int32.MinValue
            Me.SSMATimeStamp = TimeSpan.MinValue
        End Sub
    End Class
End Namespace