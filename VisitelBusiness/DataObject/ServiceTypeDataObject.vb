Namespace VisitelBusiness.DataObject
    Public Class ServiceTypeDataObject
        Inherits BaseDataObject

        Public Property Id As Integer

        Public Property ProgramService As String

        Public Property ServiceGroup As String

        Public Property BillCode As String

        Public Property Description As String

        Public Property ServiceCode As String

        Public Property MEDsysServiceCode As String

        Public Property ProcedureCodeQualifier As String

        Public Property HCPCS As String

        Public Property ModifierOne As String

        Public Property ModifierTwo As String

        Public Property ModifierThree As String

        Public Property ModifierFour As String

        Public Sub New()
            Me.Id = Integer.MinValue

            Me.ProgramService = InlineAssignHelper(Me.ServiceGroup, InlineAssignHelper(Me.BillCode, InlineAssignHelper(Me.Description, _
                                InlineAssignHelper(Me.ServiceCode, InlineAssignHelper(Me.MEDsysServiceCode, InlineAssignHelper(Me.ProcedureCodeQualifier, _
                                InlineAssignHelper(Me.HCPCS, InlineAssignHelper(Me.ModifierOne, InlineAssignHelper(Me.ModifierTwo, InlineAssignHelper(Me.ModifierThree, _
                                InlineAssignHelper(Me.ModifierFour, InlineAssignHelper(Me.HCPCS, String.Empty))))))))))))
        End Sub
    End Class
End Namespace

