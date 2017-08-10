Namespace VisitelBusiness.DataObject
    Public Class ClientCommentsDataObject
        Public Property CommentId As Integer

        Public Property ClientId As Integer

        Public Property Comment As String

        Public Property CommentDate As String

        Public Property CommunicationNotes As Boolean

        Public Property EntryTime As String

        Public Property UpdateDate As String

        Public Property UpdateBy As String

        Public Property CompanyId As Integer

        Public Property UserId As Integer

        Public Property SSMATimeStamp As TimeSpan

        Public Sub New()
            Me.CommentId = Int32.MinValue
            Me.ClientId = Int32.MinValue
            Me.Comment = String.Empty
            Me.CommentDate = String.Empty
            Me.CommunicationNotes = False
            Me.EntryTime = String.Empty
            Me.UpdateDate = String.Empty
            Me.UpdateBy = String.Empty
            Me.CompanyId = Int32.MinValue
            Me.UserId = Int32.MinValue
            Me.SSMATimeStamp = TimeSpan.MinValue
        End Sub
    End Class
End Namespace