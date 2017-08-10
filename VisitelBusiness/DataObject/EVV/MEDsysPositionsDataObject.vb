
Namespace VisitelBusiness.DataObject.EVV
    Public Class MEDsysPositionsDataObject
        Inherits BaseDataObject

        Public Property Id As Int64

        Public Property AccountId As String

        Public Property PositionCode As String

        Public Property PositionName As String

        Public Property UpdateDate As String

        Public Property UpdateBy As String

        Public Property Remarks As String

        Public Sub New()
            Me.AccountId = InlineAssignHelper(Me.PositionCode, InlineAssignHelper(Me.PositionName, InlineAssignHelper(Me.UpdateDate,
                           InlineAssignHelper(Me.UpdateBy, InlineAssignHelper(Me.Remarks, String.Empty)))))

            Me.Id = Int64.MinValue
        End Sub
    End Class
End Namespace