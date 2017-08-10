
Namespace VisitelBusiness.DataObject.EVV
    Public Class ClientDataObject
        Inherits BaseDataObject

        Public Property Id As Int64

        ''' <summary>
        ''' Required; [Vendor Client ID]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MyUniqueID As String

        ''' <summary>
        ''' Required; [Number is assigned by Vesta®. Send -1 for new records or actual value for updates.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ClientIdVesta As String

        ''' <summary>
        ''' [Number used to clock in/out.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EVVId As String

        ''' <summary>
        ''' Required
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LastName As String

        ''' <summary>
        ''' Required
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FirstName As String

        ''' <summary>
        ''' Required
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Medicaid As String

        ''' <summary>
        ''' Required; [Format:  MM/DD/YYYY]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DateOfBirth As String

        ''' <summary>
        ''' Required; [M = Male / F = Female]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Gender As String

        ''' <summary>
        ''' Required; [Phone used for LandLine calls]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Phone As String

        Public Property PhoneTwo As String

        Public Property PhoneThree As String

        Public Property AddressOne As String

        Public Property AddressTwo As String

        ''' <summary>
        ''' Required
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property City As String

        ''' <summary>
        ''' Required
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property State As String

        ''' <summary>
        ''' Required
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Zip As String

        ''' <summary>
        ''' [1 = 2 hr / 2 = 4 hr : Default value = 1]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Priority As String

        ''' <summary>
        ''' [LL = LandLine (Default) / GPS = GPS enabled smart phone / VCT = Visit Clock Token]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ClientDeviceType As String

        ''' <summary>
        ''' Required; [Format: MM/DD/YYYY]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MedicaidStartDate As String

        ''' <summary>
        ''' Required; [Format: MM/DD/YYYY]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MedicaidEndDate As String

        ''' <summary>
        ''' Required
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DADSIndividualRegion As String

        Public Property IndvMbrProgram As String

        Public Property MCOMbrSDA As String

        ''' <summary>
        ''' Required; [Format: MM/DD/YYYY]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MbrEnrollStartDate As String

        ''' <summary>
        ''' Required; [Format: MM/DD/YYYY]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MbrEnrollEndDate As String

        ''' <summary>
        ''' Branch Name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BranchName As String

        ''' <summary>
        ''' Vendor Supervisor ID
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property VendorSupervisorId As String

        ''' <summary>
        ''' Vendor Supervisor First Name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property VendorSupervisorFirstName As String

        ''' <summary>
        ''' Vendor Supervisor Last Name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property VendorSupervisorLastName As String

        Public Property [Error] As String

        Public Property ClientId As Int64

        Public Property UpdateDate As String

        Public Property UpdateBy As String

        Public Property Remarks As String

        Public Sub New()
            Me.MyUniqueID = InlineAssignHelper(Me.ClientIdVesta, InlineAssignHelper(Me.EVVId, InlineAssignHelper(Me.LastName,
                            InlineAssignHelper(Me.FirstName, InlineAssignHelper(Me.Medicaid, InlineAssignHelper(Me.DateOfBirth,
                            InlineAssignHelper(Me.Gender, InlineAssignHelper(Me.Phone, InlineAssignHelper(Me.PhoneTwo,
                            InlineAssignHelper(Me.PhoneThree, InlineAssignHelper(Me.AddressOne, InlineAssignHelper(Me.AddressTwo,
                            InlineAssignHelper(Me.City, InlineAssignHelper(Me.State, InlineAssignHelper(Me.Zip, InlineAssignHelper(Me.Priority,
                            InlineAssignHelper(Me.ClientDeviceType, InlineAssignHelper(Me.MedicaidStartDate, InlineAssignHelper(Me.MedicaidEndDate,
                            InlineAssignHelper(Me.DADSIndividualRegion, InlineAssignHelper(Me.IndvMbrProgram, InlineAssignHelper(Me.MCOMbrSDA,
                            InlineAssignHelper(Me.MbrEnrollStartDate, InlineAssignHelper(Me.MbrEnrollEndDate, InlineAssignHelper(Me.BranchName,
                            InlineAssignHelper(Me.VendorSupervisorId, InlineAssignHelper(Me.VendorSupervisorFirstName,
                            InlineAssignHelper(Me.VendorSupervisorLastName, InlineAssignHelper(Me.[Error], InlineAssignHelper(Me.UpdateDate,
                            InlineAssignHelper(Me.UpdateBy, InlineAssignHelper(Me.Remarks, String.Empty))))))))))))))))))))))))))))))))

            Me.ClientId = Int64.MinValue
        End Sub
    End Class
End Namespace



