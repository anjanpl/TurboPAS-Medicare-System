Namespace VisitelBusiness.DataObject.Settings
    Public Class ClientTypeSettingDataObject
        Inherits BaseDataObject

        Public Property IdNumber As Integer

        Public Property ClientCode As String

        Public Property Name As String

        Public Property ContractNo As String

        Public Property Region As String

        Public Property ServiceTypeId As Integer

        Public Property ServiceType As String

        Public Property ProgramType As String

        Public Property Status As Int16

        Public Property ClientGroupId As Integer

        Public Property ClientGroup As String

        Public Property ReceiverId As Integer

        Public Property ReceiverName As String

        Public Property PayerId As Integer

        Public Property Payer As String

        Public Property UnitRate As Decimal

        Public Property SantraxYN As Int16

        Public Property CM2000YN As Int16

        Public Property UpdateBy As String

        Public Property UpdateDate As String

        Public Property CompanyId As Integer

        Public Property UserId As Integer

        Public Property Remarks As String

        Public Sub New()
            Me.IdNumber = InlineAssignHelper(Me.ClientGroupId, InlineAssignHelper(Me.PayerId, InlineAssignHelper(Me.ReceiverId,
                          InlineAssignHelper(Me.CompanyId, InlineAssignHelper(Me.UserId, InlineAssignHelper(Me.ServiceTypeId, Int32.MinValue))))))

            Me.Name = InlineAssignHelper(Me.ClientCode, InlineAssignHelper(Me.ReceiverName, InlineAssignHelper(Me.Payer,
                      InlineAssignHelper(Me.ContractNo, InlineAssignHelper(Me.ClientGroup, InlineAssignHelper(Me.UpdateDate,
                      InlineAssignHelper(Me.UpdateBy, InlineAssignHelper(Me.Remarks, InlineAssignHelper(Me.Region,
                      InlineAssignHelper(Me.ServiceType, InlineAssignHelper(Me.ProgramType, String.Empty)))))))))))

            Me.Status = InlineAssignHelper(Me.SantraxYN, InlineAssignHelper(Me.CM2000YN, Int16.MinValue))

            Me.UnitRate = Decimal.MinValue
        End Sub
    End Class
End Namespace

