Namespace VisitelBusiness.DataObject
    Public Class EVVDataObject
        Inherits BaseDataObject

        Public Property ID As Integer

        Public Property ProgramService As String

        Public Property ServiceGroup As String

        Public Property BillCode As String

        Public Property ServiceCode As String

        Public Property ServiceCodeDescription As String

        Public Property ProcedureCodeQualifier As String

        Public Property HCPCS As String

        Public Sub New()
            Me.ID = Int32.MinValue

            Me.ProgramService = InlineAssignHelper(Me.ServiceGroup, InlineAssignHelper(Me.BillCode,
                                InlineAssignHelper(Me.ServiceCode, InlineAssignHelper(Me.ServiceCodeDescription,
                                InlineAssignHelper(Me.ProcedureCodeQualifier, InlineAssignHelper(Me.HCPCS, String.Empty))))))
        End Sub
    End Class
End Namespace

