
Namespace VisitelBusiness.DataObject
    Public Class BaseDataObject
        Protected Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function
    End Class
End Namespace

