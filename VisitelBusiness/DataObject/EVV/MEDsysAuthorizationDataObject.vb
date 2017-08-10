Namespace VisitelBusiness.DataObject.EVV
    Public Class MEDsysAuthorizationDataObject
        Inherits BaseDataObject

        Public Property Id As Int64

        Public Property ClientId As Int64

        ''' <summary>
        ''' Required; [MEDsys assigned Account ID]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AccountId As Int64

        ''' <summary>
        ''' Required; [The Unique ID (Primary Key) for this Authorization from the system sending the record. External Primary Key]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ExternalId As String

        ''' <summary>
        ''' [MEDsys assigned Authorization ID. Primary Key]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AuthorizationId As Int64

        ''' <summary>
        ''' Required; [Agency defined Authorization (Reference) Number]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AuthorizationNumber As String

        ''' <summary>
        ''' [Agency defined Control Number]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ControlNumber As String

        ''' <summary>
        ''' Required; [Authorization Begin Date]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DateBegin As String

        ''' <summary>
        ''' Required; [Authorization End Date]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DateEnd As String

        ''' <summary>
        ''' Required; [Service Code. Acceptable codes are assigned during implementation]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ServiceCode As String

        ''' <summary>
        ''' [Activity Code. Acceptable codes are assigned during implementation]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ActivityCode As String

        ''' <summary>
        ''' Required; [The Unique ID of the Client assigned to this schedule. External Primary Key]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ClientExternalId As String

        ''' <summary>
        ''' '[The Unique ID of the Payor to which the authorization should be assigned.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PayorExternalId As String

        ''' <summary>
        ''' Payor Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PayorId As String

        ''' <summary>
        ''' [The full name of the Payor to which the authorization should be assigned. Either PayorExternalID or PayorName is required.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PayorName As String

        ''' <summary>
        ''' [Bill type for this schedule. Acceptable values are:
        ''' 1 – Hourly
        ''' 2 – Visit
        ''' 3 – Unit
        ''' If no value is supplied, 1 (Hourly) is assumed]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AuthType As String

        ''' <summary>
        ''' Required; [Maximum (by AuthType) allowed for this authorization]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Maximum As String

        ''' <summary>
        ''' [Manner with witch to restrict this Authorization.
        ''' Acceptable values are:
        ''' 1 – None
        ''' 2 – Day
        ''' 3 – Week
        ''' 4 – Month
        ''' 5 – Year
        ''' If no value is supplied, 1 (None) is assumed]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LimitBy As String

        ''' <summary>
        ''' [What action to perform with this record. Acceptable
        ''' values are:
        ''' N – New Record
        ''' U – Updated Record]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Action As String

        Public Property UpdateDate As String

        Public Property UpdateBy As String

        Public Property Remarks As String

        Public Sub New()
            Me.AccountId = InlineAssignHelper(Me.AuthorizationId, InlineAssignHelper(Me.ClientId, Int64.MinValue))

            Me.ExternalId = InlineAssignHelper(Me.AuthorizationNumber, InlineAssignHelper(Me.ControlNumber,
                            InlineAssignHelper(Me.DateBegin, InlineAssignHelper(Me.DateEnd, InlineAssignHelper(Me.ServiceCode,
                            InlineAssignHelper(Me.ActivityCode, InlineAssignHelper(Me.ClientExternalId, InlineAssignHelper(Me.PayorExternalId,
                            InlineAssignHelper(Me.PayorId, InlineAssignHelper(Me.PayorName, InlineAssignHelper(Me.AuthType,
                            InlineAssignHelper(Me.Maximum, InlineAssignHelper(Me.LimitBy, InlineAssignHelper(Me.Action, String.Empty))))))))))))))
        End Sub
    End Class
End Namespace







