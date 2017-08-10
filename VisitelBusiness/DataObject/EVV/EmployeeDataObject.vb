
Namespace VisitelBusiness.DataObject.EVV
    Public Class EmployeeDataObject
        Inherits BaseDataObject

        Public Property Id As Int64

        ''' <summary>
        ''' Required; [Vendor Employee ID]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MyUniqueId As String

        ''' <summary>
        ''' [Number used to clock in/out]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EVVId As String

        ''' <summary>
        ''' Required; [Provider Number - Send -1 for new records and actual value for updates.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EmployeeNumberVesta As String

        ''' <summary>
        ''' Required; [Last Name]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EmployeeLastName As String

        ''' <summary>
        ''' Required; [First Name]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EmployeeFirstName As String

        ''' <summary>
        ''' Required; [Phone]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EmployeePhone As String

        ''' <summary>
        ''' Required; [Entire Social Security Number or Last Four Numbers of Social Security are required.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EmployeeSSNumber As String

        ''' <summary>
        ''' Required; [Only required if Social Security or Last Four Numbers of Social Security are not available.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EmployeePassport As String

        ''' <summary>
        ''' Required; Format:  MM/DD/YYYY
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EmployeeStartDate As String

        ''' <summary>
        ''' Required; Format:  MM/DD/YYYY
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EmployeeEndDate As String

        ''' <summary>
        ''' Required; [Discipline Credentials Nurse/PT/OT/Attendant/CNA/etc.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EmployeeDiscipline As String

        ''' <summary>
        ''' Branch Name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BranchName As String

        Public Property [Error] As String

        Public Property EmployeeId As Int64

        Public Property UpdateDate As String

        Public Property UpdateBy As String

        Public Property Remarks As String

        Public Sub New()
            Me.MyUniqueId = InlineAssignHelper(Me.EVVId, InlineAssignHelper(Me.EmployeeNumberVesta, InlineAssignHelper(Me.EmployeeLastName,
                            InlineAssignHelper(Me.EmployeeFirstName, InlineAssignHelper(Me.EmployeePhone, InlineAssignHelper(Me.EmployeeStartDate,
                            InlineAssignHelper(Me.EmployeeEndDate, InlineAssignHelper(Me.EmployeeDiscipline, InlineAssignHelper(Me.BranchName,
                            InlineAssignHelper(Me.[Error], InlineAssignHelper(Me.UpdateDate, InlineAssignHelper(Me.UpdateBy,
                            InlineAssignHelper(Me.Remarks, String.Empty)))))))))))))

            Me.EmployeeId = Int64.MinValue
        End Sub
    End Class
End Namespace