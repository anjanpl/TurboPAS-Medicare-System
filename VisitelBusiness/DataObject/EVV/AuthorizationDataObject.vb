Namespace VisitelBusiness.DataObject.EVV

    Public Class AuthorizationDataObject
        Inherits BaseDataObject

        Public Property Id As Int64

        ''' <summary>
        ''' Required; [Vendor Authorization ID]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MyUniqueId As String

        ''' <summary>
        ''' Required; [Send -1 for new records and actual value for updates.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AuthorizationId As String

        ''' <summary>
        ''' Required; [Needed to link authorization to client.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ClientIdVesta As String

        ''' <summary>
        ''' [DADS Contract Number Required for DADS clients]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DadsContractNo As String

        ''' <summary>
        ''' Required; [Contract Type (Ex: PHC, FC, CA, CLASS, SPW, PAS)]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ProgramType As String

        ''' <summary>
        ''' Required; [MCO Payor Unique ID (ex: Superior, Amerigroup, Molina, DADS, etc.)]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AuthorizationPayer As String

        ''' <summary>
        ''' Required; [Format:  MM/DD/YYYY]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AuthorizationStartDate As String

        ''' <summary>
        ''' Required; [Format:  MM/DD/YYYY]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AuthorizationEndDate As String

        ''' <summary>
        ''' Required; [Example: 14.50]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AuthorizationUnits As String

        ''' <summary>
        ''' Required; [Example: Weekly, Monthly, Per Auth, Daily]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AuthorizationUnitsType As String

        Public Property AuthorizationNumber As String

        Public Property [Error] As String

        Public Property ClientId As Int64

        Public Property ClientInfoId As Integer

        Public Property ClientName As String

        Public Property PayerId As Int64

        Public Property UpdateDate As String

        Public Property UpdateBy As String

        Public Property Remarks As String

        Public Sub New()
            Me.MyUniqueId = InlineAssignHelper(Me.AuthorizationId, InlineAssignHelper(Me.ClientIdVesta,
                            InlineAssignHelper(Me.DadsContractNo, InlineAssignHelper(Me.AuthorizationPayer,
                            InlineAssignHelper(Me.AuthorizationStartDate, InlineAssignHelper(Me.AuthorizationEndDate,
                            InlineAssignHelper(Me.AuthorizationUnits, InlineAssignHelper(Me.AuthorizationUnitsType,
                            InlineAssignHelper(Me.AuthorizationNumber, InlineAssignHelper(Me.[Error],
                            InlineAssignHelper(Me.UpdateDate, InlineAssignHelper(Me.UpdateBy, InlineAssignHelper(Me.ClientName,
                            InlineAssignHelper(Me.Remarks, String.Empty))))))))))))))

            Me.ClientId = InlineAssignHelper(Me.Id, InlineAssignHelper(Me.PayerId, Int64.MinValue))
            Me.ClientInfoId = Integer.MinValue
        End Sub
    End Class

End Namespace
