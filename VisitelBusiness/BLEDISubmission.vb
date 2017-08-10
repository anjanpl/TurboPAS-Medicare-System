
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: EDI Submission
' Author: Anjan Kumar Paul
' Start Date: 17 July 2015
' End Date: 
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                17 July 2015     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Web.UI.WebControls

Namespace VisitelBusiness

    Public Class BLEDISubmission
        Inherits BLCommon


        Public Sub ProcessEDIViewRecords(ByRef ConVisitel As SqlConnection, StartDate As String, EndDate As String,
                                         ContractNo As String, Optional SelectedClientId As String = "")

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary

            parameters.Add("@StartDate", StartDate)
            parameters.Add("@EndDate", EndDate)
            parameters.Add("@ContractNo", ContractNo)

            If (Not String.IsNullOrEmpty(SelectedClientId)) Then
                parameters.Add("@SelectedClientId", SelectedClientId)
            End If

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.ProcessEDIViewRecords]", ConVisitel, parameters)

            parameters = Nothing
            objSharedSettings = Nothing

        End Sub

        Public Function SelectEDIRecords(ByRef ConVisitel As SqlConnection) As List(Of EDISubmissionDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectEDIViewRecords]", drSql, ConVisitel, Nothing)
            objSharedSettings = Nothing

            Dim EDISubmissionList As New List(Of EDISubmissionDataObject)
            Dim objEDISubmissionDataObject As EDISubmissionDataObject

            If drSql.HasRows Then

                While drSql.Read()

                    objEDISubmissionDataObject = New EDISubmissionDataObject()

                    objEDISubmissionDataObject.Id = If((DBNull.Value.Equals(drSql("Id"))), objEDISubmissionDataObject.Id, Convert.ToInt32(drSql("Id")))
                    objEDISubmissionDataObject.ClientId = If((DBNull.Value.Equals(drSql("ClientId"))), objEDISubmissionDataObject.ClientId, Convert.ToInt64(drSql("ClientId")))
                    objEDISubmissionDataObject.FirstName = Convert.ToString(drSql("FirstName"), Nothing).Trim()
                    objEDISubmissionDataObject.MiddleNameInitial = Convert.ToString(drSql("MiddleNameInitial"), Nothing).Trim()
                    objEDISubmissionDataObject.LastName = Convert.ToString(drSql("LastName"), Nothing).Trim()
                    objEDISubmissionDataObject.Address = Convert.ToString(drSql("Address"), Nothing).Trim()
                    objEDISubmissionDataObject.ApartmentNumber = Convert.ToString(drSql("ApartmentNumber"), Nothing).Trim()
                    objEDISubmissionDataObject.City = Convert.ToString(drSql("City"), Nothing).Trim()
                    objEDISubmissionDataObject.State = Convert.ToString(drSql("State"), Nothing).Trim()
                    objEDISubmissionDataObject.Zip = Convert.ToString(drSql("Zip"), Nothing).Trim()

                    objEDISubmissionDataObject.DateOfBirth = Convert.ToString(drSql("DateOfBirth"), Nothing).Trim()
                    objEDISubmissionDataObject.DateOfBirth = If((Not String.IsNullOrEmpty(objEDISubmissionDataObject.DateOfBirth)),
                                                         Convert.ToDateTime(objEDISubmissionDataObject.DateOfBirth, GetCultureInfo()).ToString(DateFormat),
                                                         objEDISubmissionDataObject.DateOfBirth)

                    objEDISubmissionDataObject.Gender = Convert.ToString(drSql("Gender"), Nothing).Trim()
                    objEDISubmissionDataObject.Name = Convert.ToString(drSql("Name"), Nothing).Trim()
                    objEDISubmissionDataObject.StateClientId = Convert.ToString(drSql("StateClientId"), Nothing).Trim()

                    objEDISubmissionDataObject.StartDate = Convert.ToString(drSql("StartDate"), Nothing).Trim()
                    objEDISubmissionDataObject.StartDate = If((Not String.IsNullOrEmpty(objEDISubmissionDataObject.StartDate)),
                                                         Convert.ToDateTime(objEDISubmissionDataObject.StartDate, GetCultureInfo()).ToString(DateFormat),
                                                         objEDISubmissionDataObject.StartDate)

                    objEDISubmissionDataObject.EndDate = Convert.ToString(drSql("EndDate"), Nothing).Trim()
                    objEDISubmissionDataObject.EndDate = If((Not String.IsNullOrEmpty(objEDISubmissionDataObject.EndDate)),
                                                         Convert.ToDateTime(objEDISubmissionDataObject.EndDate, GetCultureInfo()).ToString(DateFormat),
                                                         objEDISubmissionDataObject.EndDate)

                    objEDISubmissionDataObject.ActualMinutes = Convert.ToString(drSql("ActualMinutes"), Nothing).Trim()
                    objEDISubmissionDataObject.UnitRate = Convert.ToString(drSql("UnitRate"), Nothing).Trim()

                    objEDISubmissionDataObject.ClientType = If((DBNull.Value.Equals(drSql("ClientType"))),
                                                               objEDISubmissionDataObject.ClientType, Convert.ToInt64(drSql("ClientType")))

                    objEDISubmissionDataObject.ContractNumber = Convert.ToString(drSql("ContractNo"), Nothing).Trim()

                    objEDISubmissionDataObject.ReceiverId = If((DBNull.Value.Equals(drSql("ReceiverId"))),
                                                               objEDISubmissionDataObject.ReceiverId, Convert.ToInt64(drSql("ReceiverId")))

                    objEDISubmissionDataObject.ActualHours = Convert.ToString(drSql("ActualHours"), Nothing).Trim()
                    objEDISubmissionDataObject.MonetaryAmount = Convert.ToString(drSql("MonetaryAmount"), Nothing).Trim()
                    objEDISubmissionDataObject.PlaceOfServiceId = Convert.ToString(drSql("PlaceOfServiceId"), Nothing).Trim()
                    objEDISubmissionDataObject.DiagnosisCodeOne = Convert.ToString(drSql("DiagnosisCodeOne"), Nothing).Trim()
                    objEDISubmissionDataObject.DiagnosisCodeTwo = Convert.ToString(drSql("DiagnosisCodeTwo"), Nothing).Trim()
                    objEDISubmissionDataObject.DiagnosisCodeThree = Convert.ToString(drSql("DiagnosisCodeThree"), Nothing).Trim()
                    objEDISubmissionDataObject.DiagnosisCodeFour = Convert.ToString(drSql("DiagnosisCodeFour"), Nothing).Trim()
                    objEDISubmissionDataObject.ProcedureId = Convert.ToString(drSql("ProcedureId"), Nothing).Trim()
                    objEDISubmissionDataObject.ClientFullName = Convert.ToString(drSql("ClientFullName"), Nothing).Trim()
                    objEDISubmissionDataObject.ClientFullAddress = Convert.ToString(drSql("ClientFullAddress"), Nothing).Trim()
                    objEDISubmissionDataObject.BillHours = Convert.ToString(drSql("BillHours"), Nothing).Trim()
                    objEDISubmissionDataObject.BillHoursTwo = Convert.ToString(drSql("BillHoursTwo"), Nothing).Trim()
                    objEDISubmissionDataObject.ModifierOne = Convert.ToString(drSql("ModifierOne"), Nothing).Trim()
                    objEDISubmissionDataObject.ModifierTwo = Convert.ToString(drSql("ModifierTwo"), Nothing).Trim()
                    objEDISubmissionDataObject.ModifierThree = Convert.ToString(drSql("ModifierThree"), Nothing).Trim()
                    objEDISubmissionDataObject.ModifierFour = Convert.ToString(drSql("ModifierFour"), Nothing).Trim()

                    objEDISubmissionDataObject.CLM0503ClmFrequencyTypeCode = Convert.ToString(drSql("CLM0503ClmFrequencyTypeCode"), Nothing).Trim()
                    objEDISubmissionDataObject.CLM06ProviderSignatureOnFile = Convert.ToString(drSql("CLM06ProviderSignatureOnFile"), Nothing).Trim()
                    objEDISubmissionDataObject.CLM07MedicareAssignmentCode = Convert.ToString(drSql("CLM07MedicareAssignmentCode"), Nothing).Trim()
                    objEDISubmissionDataObject.CLM08AssignmentOfBenefitsIndicator = Convert.ToString(drSql("CLM08AssignmentOfBenefitsIndicator"), Nothing).Trim()
                    objEDISubmissionDataObject.CLM09ReleaseOfInfoCode = Convert.ToString(drSql("CLM09ReleaseOfInfoCode"), Nothing).Trim()
                    objEDISubmissionDataObject.CLM10PatientSignatureCode = Convert.ToString(drSql("CLM10PatientSignatureCode"), Nothing).Trim()
                    objEDISubmissionDataObject.AuthorizationNumber = Convert.ToString(drSql("AuthorizationNumber"), Nothing).Trim()

                    objEDISubmissionDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing).Trim()

                    objEDISubmissionDataObject.UpdateDate = If((Not String.IsNullOrEmpty(objEDISubmissionDataObject.UpdateDate)),
                                                          Convert.ToDateTime(objEDISubmissionDataObject.UpdateDate, GetCultureInfo()).ToString(DateFormat),
                                                          objEDISubmissionDataObject.UpdateDate)

                    objEDISubmissionDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing)

                    objEDISubmissionDataObject.MissingData = Convert.ToBoolean(drSql("IsMissing"), Nothing)

                    EDISubmissionList.Add(objEDISubmissionDataObject)

                End While
                objEDISubmissionDataObject = Nothing

            Else
                objEDISubmissionDataObject = New EDISubmissionDataObject()
                EDISubmissionList.Add(objEDISubmissionDataObject)
                objEDISubmissionDataObject = Nothing
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return EDISubmissionList

        End Function

        Public Sub SelectFTPClientName(VisitelConnectionString As String, ByRef SqlDataSourceNamesList As SqlDataSource, StartDate As String, EndDate As String,
                                       ContractNumber As String)

            SqlDataSourceNamesList.ProviderName = "System.Data.SqlClient"
            SqlDataSourceNamesList.ConnectionString = VisitelConnectionString

            SqlDataSourceNamesList.SelectCommandType = SqlDataSourceCommandType.StoredProcedure
            SqlDataSourceNamesList.SelectParameters.Clear()
            SqlDataSourceNamesList.SelectParameters.Add("StartDate", StartDate)
            SqlDataSourceNamesList.SelectParameters.Add("EndDate", EndDate)
            SqlDataSourceNamesList.SelectParameters.Add("ContractNo", ContractNumber)

            SqlDataSourceNamesList.SelectCommand = "[TurboDB.SelectFTPClientNameList]"
            SqlDataSourceNamesList.DataBind()

        End Sub

        Public Function SelectEDILogin(ByRef ConVisitel As SqlConnection) As List(Of EDILoginDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectEDILogin]", drSql, ConVisitel, Nothing)
            objSharedSettings = Nothing

            Dim EDILoginList As New List(Of EDILoginDataObject)
            Dim objEDILoginDataObject As EDILoginDataObject

            If drSql.HasRows Then

                While drSql.Read()

                    objEDILoginDataObject = New EDILoginDataObject()

                    objEDILoginDataObject.IdNumber = If((DBNull.Value.Equals(drSql("id_number"))), objEDILoginDataObject.IdNumber, Convert.ToInt32(drSql("id_number")))

                    objEDILoginDataObject.Name = Convert.ToString(drSql("name"), Nothing).Trim()
                    objEDILoginDataObject.FTPAddress = Convert.ToString(drSql("ftp_address"), Nothing).Trim()
                    objEDILoginDataObject.Directory = Convert.ToString(drSql("directory"), Nothing).Trim()
                    objEDILoginDataObject.SubmitterId = Convert.ToString(drSql("submitter_id"), Nothing).Trim()
                    objEDILoginDataObject.Password = Convert.ToString(drSql("password"), Nothing).Trim()
                    objEDILoginDataObject.Status = Convert.ToString(drSql("status"), Nothing).Trim()
                    objEDILoginDataObject.UpdateDate = Convert.ToString(drSql("update_date"), Nothing).Trim()

                    objEDILoginDataObject.UpdateDate = If((Not String.IsNullOrEmpty(objEDILoginDataObject.UpdateDate)),
                                                         Convert.ToDateTime(objEDILoginDataObject.UpdateDate, GetCultureInfo()).ToString(DateFormat),
                                                         objEDILoginDataObject.UpdateDate)

                    objEDILoginDataObject.UpdateBy = Convert.ToString(drSql("update_by"), Nothing).Trim()

                    objEDILoginDataObject.CompanyId = If((DBNull.Value.Equals(drSql("company_id"))), objEDILoginDataObject.CompanyId, Convert.ToInt64(drSql("company_id")))
                    objEDILoginDataObject.UserId = If((DBNull.Value.Equals(drSql("user_id"))), objEDILoginDataObject.UserId, Convert.ToInt64(drSql("user_id")))

                    EDILoginList.Add(objEDILoginDataObject)

                End While
                objEDILoginDataObject = Nothing

            Else
                objEDILoginDataObject = New EDILoginDataObject()
                EDILoginList.Add(objEDILoginDataObject)
                objEDILoginDataObject = Nothing
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return EDILoginList

        End Function

        Public Sub DeleteEDISubmissionList(ConVisitel As SqlConnection)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteEDISubmissionList]", ConVisitel, Nothing)

            objSharedSettings = Nothing

        End Sub

        Public Sub CorrectEDIClaims(ConVisitel As SqlConnection, EDIId As Int64, ClaimFrequencyTypeCode As String, ClaimNumber As String)

            Dim objSharedSettings As New SharedSettings()

            Dim parameters As New HybridDictionary

            parameters.Add("@EDIId", EDIId)
            parameters.Add("@ClaimFrequencyTypeCode", ClaimFrequencyTypeCode)
            parameters.Add("@ClaimNumber", ClaimNumber)

            objSharedSettings.ExecuteQuery("", "[TurboDB.CorrectEDIClaims]", ConVisitel, parameters)
            parameters = Nothing
            objSharedSettings = Nothing

        End Sub

        Public Sub DeleteEDIClaim(ConVisitel As SqlConnection, EDIId As Int64)

            Dim objSharedSettings As New SharedSettings()

            Dim parameters As New HybridDictionary
            parameters.Add("@EDIId", EDIId)

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteEDIClaim]", ConVisitel, parameters)
            parameters = Nothing

            objSharedSettings = Nothing

        End Sub

        Public Sub InsertEDILoginInfo(ConVisitel As SqlConnection, objEDILoginDataObject As EDILoginDataObject)

            Dim parameters As New HybridDictionary()

            SetParameters(parameters, objEDILoginDataObject)

            parameters.Add("@CompanyId", objEDILoginDataObject.CompanyId)
            parameters.Add("@UserId", objEDILoginDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertEDILoginInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateEDILoginInfo(ConVisitel As SqlConnection, objEDILoginDataObject As EDILoginDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@IdNumber", objEDILoginDataObject.IdNumber)

            SetParameters(parameters, objEDILoginDataObject)

            parameters.Add("@UpdateBy", objEDILoginDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateEDILoginInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetParameters(parameters As HybridDictionary, objEDILoginDataObject As EDILoginDataObject)

            parameters.Add("@Name", objEDILoginDataObject.Name)
            parameters.Add("@FtpAddress", objEDILoginDataObject.FTPAddress)
            parameters.Add("@Directory", objEDILoginDataObject.Directory)
            parameters.Add("@SubmitterId", objEDILoginDataObject.SubmitterId)
            parameters.Add("@Password", objEDILoginDataObject.Password)
            parameters.Add("@Status", objEDILoginDataObject.Status)
           
        End Sub

        Public Sub DeleteEDILoginInfo(ConVisitel As SqlConnection, Id As Int64, DeletedBy As String)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", Id)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteEDILoginInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Function SelectEDIMissingData(ByRef ConVisitel As SqlConnection) As List(Of EDIMissingDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectEDIMissingData]", drSql, ConVisitel, Nothing)
            objSharedSettings = Nothing

            Dim EDIMissingDataList As New List(Of EDIMissingDataObject)
            Dim objEDIMissingDataObject As EDIMissingDataObject

            If drSql.HasRows Then

                While drSql.Read()

                    objEDIMissingDataObject = New EDIMissingDataObject()

                    objEDIMissingDataObject.Name = Convert.ToString(drSql("Name"), Nothing).Trim()
               
                    objEDIMissingDataObject.ClientId = If((DBNull.Value.Equals(drSql("ClientId"))), objEDIMissingDataObject.ClientId, Convert.ToInt32(drSql("ClientId")))
                    objEDIMissingDataObject.StateClientId = If((DBNull.Value.Equals(drSql("StateClientId"))), objEDIMissingDataObject.StateClientId,
                                                               Convert.ToInt32(drSql("StateClientId")))

                    objEDIMissingDataObject.SocialSecurityNo = Convert.ToString(drSql("SocialSecurityNumber"), Nothing).Trim()
                    objEDIMissingDataObject.MissingData = Convert.ToString(drSql("Missing_Data"), Nothing).Trim()

                    EDIMissingDataList.Add(objEDIMissingDataObject)

                End While
                objEDIMissingDataObject = Nothing

            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return EDIMissingDataList

        End Function

        Public Function GenerateEDIFile(ConVisitel As SqlConnection, ByRef Sds As DataSet, ContractNumber As String, SaveLocation As String, IsResubmission As Int16, _
                                   ReceiverId As Int64, PayerId As Int64, UserId As Int64) As String

            Dim ReturnResult As String

            Dim parameters As New HybridDictionary()

            parameters.Add("@ContractNumber", ContractNumber)
            parameters.Add("@SaveLocation", SaveLocation)
            parameters.Add("@IsResubmission", IsResubmission)
            parameters.Add("@ReceiverId", ReceiverId)
            parameters.Add("@gPayer", PayerId)
            parameters.Add("@UserId", UserId)

            Dim objSharedSettings As New SharedSettings()

            ReturnResult = objSharedSettings.GetDataSet("", "[TurboDB.EDIFileGeneration]", Sds, ConVisitel, parameters, "GenerateEDIFile")

            objSharedSettings = Nothing
            parameters = Nothing

            Return ReturnResult

        End Function

    End Class

    Public Class EDILoginDataObject
        Inherits BaseDataObject

        Private m_IdNumber As Int64
        Public Property IdNumber() As Int64
            Get
                Return m_IdNumber
            End Get
            Set(value As Int64)
                m_IdNumber = value
            End Set
        End Property

        Private m_Name As String
        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(value As String)
                m_Name = value
            End Set
        End Property

        Private m_FTPAddress As String
        Public Property FTPAddress() As String
            Get
                Return m_FTPAddress
            End Get
            Set(value As String)
                m_FTPAddress = value
            End Set
        End Property

        Private m_Directory As String
        Public Property Directory() As String
            Get
                Return m_Directory
            End Get
            Set(value As String)
                m_Directory = value
            End Set
        End Property

        Private m_SubmitterId As String
        Public Property SubmitterId() As String
            Get
                Return m_SubmitterId
            End Get
            Set(value As String)
                m_SubmitterId = value
            End Set
        End Property

        Private m_Password As String
        Public Property Password() As String
            Get
                Return m_Password
            End Get
            Set(value As String)
                m_Password = value
            End Set
        End Property

        Private m_Status As String
        Public Property Status() As String
            Get
                Return m_Status
            End Get
            Set(value As String)
                m_Status = value
            End Set
        End Property

        Private m_UpdateDate As String
        Public Property UpdateDate() As String
            Get
                Return m_UpdateDate
            End Get
            Set(value As String)
                m_UpdateDate = value
            End Set
        End Property

        Private m_UpdateBy As String
        Public Property UpdateBy() As String
            Get
                Return m_UpdateBy
            End Get
            Set(value As String)
                m_UpdateBy = value
            End Set
        End Property

        Private m_CompanyId As Int64
        Public Property CompanyId() As Int64
            Get
                Return m_CompanyId
            End Get
            Set(value As Int64)
                m_CompanyId = value
            End Set
        End Property

        Private m_UserId As Int64
        Public Property UserId() As Int64
            Get
                Return m_UserId
            End Get
            Set(value As Int64)
                m_UserId = value
            End Set
        End Property

        Private m_Remarks As String
        Public Property Remarks() As String
            Get
                Return m_Remarks
            End Get
            Set(value As String)
                m_Remarks = value
            End Set
        End Property

        Public Sub New()

            Me.IdNumber = InlineAssignHelper(Me.CompanyId, InlineAssignHelper(Me.UserId, Int64.MinValue))

            Me.Name = InlineAssignHelper(Me.FTPAddress, InlineAssignHelper(Me.Directory, InlineAssignHelper(Me.SubmitterId,
                      InlineAssignHelper(Me.Password, InlineAssignHelper(Me.Status, InlineAssignHelper(Me.UpdateDate,
                      InlineAssignHelper(Me.UpdateBy, String.Empty)))))))

        End Sub

    End Class

    Public Class EDISubmissionDataObject
        Inherits BaseDataObject

        Private m_Id As Integer
        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(value As Integer)
                m_Id = value
            End Set
        End Property

        Private m_ClientId As Int64
        Public Property ClientId() As Int64
            Get
                Return m_ClientId
            End Get
            Set(value As Int64)
                m_ClientId = value
            End Set
        End Property

        Private m_FirstName As String
        Public Property FirstName() As String
            Get
                Return m_FirstName
            End Get
            Set(value As String)
                m_FirstName = value
            End Set
        End Property

        Private m_MiddleNameInitial As String
        Public Property MiddleNameInitial() As String
            Get
                Return m_MiddleNameInitial
            End Get
            Set(value As String)
                m_MiddleNameInitial = value
            End Set
        End Property

        Private m_LastName As String
        Public Property LastName() As String
            Get
                Return m_LastName
            End Get
            Set(value As String)
                m_LastName = value
            End Set
        End Property

        Private m_Address As String
        Public Property Address() As String
            Get
                Return m_Address
            End Get
            Set(value As String)
                m_Address = value
            End Set
        End Property

        Private m_ApartmentNumber As String
        Public Property ApartmentNumber() As String
            Get
                Return m_ApartmentNumber
            End Get
            Set(value As String)
                m_ApartmentNumber = value
            End Set
        End Property

        Private m_City As String
        Public Property City() As String
            Get
                Return m_City
            End Get
            Set(value As String)
                m_City = value
            End Set
        End Property

        Private m_State As String
        Public Property State() As String
            Get
                Return m_State
            End Get
            Set(value As String)
                m_State = value
            End Set
        End Property

        Private m_Zip As String
        Public Property Zip() As String
            Get
                Return m_Zip
            End Get
            Set(value As String)
                m_Zip = value
            End Set
        End Property

        Private m_DateOfBirth As String
        Public Property DateOfBirth() As String
            Get
                Return m_DateOfBirth
            End Get
            Set(value As String)
                m_DateOfBirth = value
            End Set
        End Property

        Private m_Gender As String
        Public Property Gender() As String
            Get
                Return m_Gender
            End Get
            Set(value As String)
                m_Gender = value
            End Set
        End Property

        Private m_Name As String
        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(value As String)
                m_Name = value
            End Set
        End Property

        Private m_StateClientId As Int64
        Public Property StateClientId() As Int64
            Get
                Return m_StateClientId
            End Get
            Set(value As Int64)
                m_StateClientId = value
            End Set
        End Property

        Private m_StartDate As String
        Public Property StartDate() As String
            Get
                Return m_StartDate
            End Get
            Set(value As String)
                m_StartDate = value
            End Set
        End Property

        Private m_EndDate As String
        Public Property EndDate() As String
            Get
                Return m_EndDate
            End Get
            Set(value As String)
                m_EndDate = value
            End Set
        End Property

        Private m_ActualMinutes As String
        Public Property ActualMinutes() As String
            Get
                Return m_ActualMinutes
            End Get
            Set(value As String)
                m_ActualMinutes = value
            End Set
        End Property

        Private m_UnitRate As String
        Public Property UnitRate() As String
            Get
                Return m_UnitRate
            End Get
            Set(value As String)
                m_UnitRate = value
            End Set
        End Property

        Private m_ClientType As Integer
        Public Property ClientType() As Integer
            Get
                Return m_ClientType
            End Get
            Set(value As Integer)
                m_ClientType = value
            End Set
        End Property

        Private m_ContractNumber As String
        Public Property ContractNumber() As String
            Get
                Return m_ContractNumber
            End Get
            Set(value As String)
                m_ContractNumber = value
            End Set
        End Property

        Private m_ReceiverId As Integer
        Public Property ReceiverId() As Integer
            Get
                Return m_ReceiverId
            End Get
            Set(value As Integer)
                m_ReceiverId = value
            End Set
        End Property

        Private m_ActualHours As String
        Public Property ActualHours() As String
            Get
                Return m_ActualHours
            End Get
            Set(value As String)
                m_ActualHours = value
            End Set
        End Property

        Private m_MonetaryAmount As String
        Public Property MonetaryAmount() As String
            Get
                Return m_MonetaryAmount
            End Get
            Set(value As String)
                m_MonetaryAmount = value
            End Set
        End Property

        Private m_PlaceOfServiceId As String
        Public Property PlaceOfServiceId() As String
            Get
                Return m_PlaceOfServiceId
            End Get
            Set(value As String)
                m_PlaceOfServiceId = value
            End Set
        End Property

        Private m_DiagnosisCodeOne As String
        Public Property DiagnosisCodeOne() As String
            Get
                Return m_DiagnosisCodeOne
            End Get
            Set(value As String)
                m_DiagnosisCodeOne = value
            End Set
        End Property

        Private m_DiagnosisCodeTwo As String
        Public Property DiagnosisCodeTwo() As String
            Get
                Return m_DiagnosisCodeTwo
            End Get
            Set(value As String)
                m_DiagnosisCodeTwo = value
            End Set
        End Property

        Private m_DiagnosisCodeThree As String
        Public Property DiagnosisCodeThree() As String
            Get
                Return m_DiagnosisCodeThree
            End Get
            Set(value As String)
                m_DiagnosisCodeThree = value
            End Set
        End Property

        Private m_DiagnosisCodeFour As String
        Public Property DiagnosisCodeFour() As String
            Get
                Return m_DiagnosisCodeFour
            End Get
            Set(value As String)
                m_DiagnosisCodeFour = value
            End Set
        End Property

        Private m_ProcedureId As String
        Public Property ProcedureId() As String
            Get
                Return m_ProcedureId
            End Get
            Set(value As String)
                m_ProcedureId = value
            End Set
        End Property

        Private m_ClientFullName As String
        Public Property ClientFullName() As String
            Get
                Return m_ClientFullName
            End Get
            Set(value As String)
                m_ClientFullName = value
            End Set
        End Property

        Private m_ClientFullAddress As String
        Public Property ClientFullAddress() As String
            Get
                Return m_ClientFullAddress
            End Get
            Set(value As String)
                m_ClientFullAddress = value
            End Set
        End Property

        Private m_BillHours As String
        Public Property BillHours() As String
            Get
                Return m_BillHours
            End Get
            Set(value As String)
                m_BillHours = value
            End Set
        End Property

        Private m_BillHoursTwo As String
        Public Property BillHoursTwo() As String
            Get
                Return m_BillHoursTwo
            End Get
            Set(value As String)
                m_BillHoursTwo = value
            End Set
        End Property

        Private m_ModifierOne As String
        Public Property ModifierOne() As String
            Get
                Return m_ModifierOne
            End Get
            Set(value As String)
                m_ModifierOne = value
            End Set
        End Property

        Private m_ModifierTwo As String
        Public Property ModifierTwo() As String
            Get
                Return m_ModifierTwo
            End Get
            Set(value As String)
                m_ModifierTwo = value
            End Set
        End Property

        Private m_ModifierThree As String
        Public Property ModifierThree() As String
            Get
                Return m_ModifierThree
            End Get
            Set(value As String)
                m_ModifierThree = value
            End Set
        End Property

        Private m_ModifierFour As String
        Public Property ModifierFour() As String
            Get
                Return m_ModifierFour
            End Get
            Set(value As String)
                m_ModifierFour = value
            End Set
        End Property

        Private m_CLM0503ClmFrequencyTypeCode As String
        Public Property CLM0503ClmFrequencyTypeCode() As String
            Get
                Return m_CLM0503ClmFrequencyTypeCode
            End Get
            Set(value As String)
                m_CLM0503ClmFrequencyTypeCode = value
            End Set
        End Property

        Private m_CLM06ProviderSignatureOnFile As String
        Public Property CLM06ProviderSignatureOnFile() As String
            Get
                Return m_CLM06ProviderSignatureOnFile
            End Get
            Set(value As String)
                m_CLM06ProviderSignatureOnFile = value
            End Set
        End Property

        Private m_CLM07MedicareAssignmentCode As String
        Public Property CLM07MedicareAssignmentCode() As String
            Get
                Return m_CLM07MedicareAssignmentCode
            End Get
            Set(value As String)
                m_CLM07MedicareAssignmentCode = value
            End Set
        End Property

        Private m_CLM08AssignmentOfBenefitsIndicator As String
        Public Property CLM08AssignmentOfBenefitsIndicator() As String
            Get
                Return m_CLM08AssignmentOfBenefitsIndicator
            End Get
            Set(value As String)
                m_CLM08AssignmentOfBenefitsIndicator = value
            End Set
        End Property

        Private m_CLM09ReleaseOfInfoCode As String
        Public Property CLM09ReleaseOfInfoCode() As String
            Get
                Return m_CLM09ReleaseOfInfoCode
            End Get
            Set(value As String)
                m_CLM09ReleaseOfInfoCode = value
            End Set
        End Property

        Private m_CLM10PatientSignatureCode As String
        Public Property CLM10PatientSignatureCode() As String
            Get
                Return m_CLM10PatientSignatureCode
            End Get
            Set(value As String)
                m_CLM10PatientSignatureCode = value
            End Set
        End Property

        Private m_AuthorizationNumber As String
        Public Property AuthorizationNumber() As String
            Get
                Return m_AuthorizationNumber
            End Get
            Set(value As String)
                m_AuthorizationNumber = value
            End Set
        End Property

        Private m_UpdateDate As String
        Public Property UpdateDate() As String
            Get
                Return m_UpdateDate
            End Get
            Set(value As String)
                m_UpdateDate = value
            End Set
        End Property

        Private m_UpdateBy As String
        Public Property UpdateBy() As String
            Get
                Return m_UpdateBy
            End Get
            Set(value As String)
                m_UpdateBy = value
            End Set
        End Property

        Private m_MissingData As Boolean
        Public Property MissingData() As Boolean
            Get
                Return m_MissingData
            End Get
            Set(value As Boolean)
                m_MissingData = value
            End Set
        End Property

        Private m_Remarks As String
        Public Property Remarks() As String
            Get
                Return m_Remarks
            End Get
            Set(value As String)
                m_Remarks = value
            End Set
        End Property

        Public Sub New()

            Me.Id = InlineAssignHelper(Me.ClientType, InlineAssignHelper(Me.ReceiverId, Integer.MinValue))

            Me.ClientId = InlineAssignHelper(Me.StateClientId, Int64.MinValue)

            Me.FirstName = InlineAssignHelper(Me.MiddleNameInitial, InlineAssignHelper(Me.LastName, InlineAssignHelper(Me.Address,
                           InlineAssignHelper(Me.ApartmentNumber, InlineAssignHelper(Me.Zip, InlineAssignHelper(Me.DateOfBirth,
                           InlineAssignHelper(Me.Gender, InlineAssignHelper(Me.Name, InlineAssignHelper(Me.StartDate,
                           InlineAssignHelper(Me.EndDate, InlineAssignHelper(Me.ActualMinutes, InlineAssignHelper(Me.UnitRate,
                           InlineAssignHelper(Me.ContractNumber, InlineAssignHelper(Me.ActualHours, InlineAssignHelper(Me.MonetaryAmount,
                           InlineAssignHelper(Me.PlaceOfServiceId, InlineAssignHelper(Me.DiagnosisCodeOne, InlineAssignHelper(Me.DiagnosisCodeTwo,
                           InlineAssignHelper(Me.DiagnosisCodeThree, InlineAssignHelper(Me.DiagnosisCodeFour, InlineAssignHelper(Me.ProcedureId,
                           InlineAssignHelper(Me.ClientFullName, InlineAssignHelper(Me.ClientFullAddress, InlineAssignHelper(Me.BillHours,
                           InlineAssignHelper(Me.BillHoursTwo, InlineAssignHelper(Me.ModifierOne, InlineAssignHelper(Me.ModifierTwo,
                           InlineAssignHelper(Me.ModifierThree, InlineAssignHelper(Me.ModifierFour, InlineAssignHelper(Me.CLM0503ClmFrequencyTypeCode,
                           InlineAssignHelper(Me.CLM06ProviderSignatureOnFile, InlineAssignHelper(Me.CLM07MedicareAssignmentCode,
                           InlineAssignHelper(Me.CLM08AssignmentOfBenefitsIndicator, InlineAssignHelper(Me.CLM09ReleaseOfInfoCode,
                           InlineAssignHelper(Me.CLM10PatientSignatureCode, InlineAssignHelper(Me.AuthorizationNumber,
                           InlineAssignHelper(Me.UpdateDate, InlineAssignHelper(Me.UpdateBy, InlineAssignHelper(Me.Remarks,
                           InlineAssignHelper(Me.City, InlineAssignHelper(Me.State,
                                String.Empty)))))))))))))))))))))))))))))))))))))))))

            Me.MissingData = False

        End Sub
    End Class

    Public Class EDIMissingDataObject
        Inherits BaseDataObject

        Private m_Name As String
        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(value As String)
                m_Name = value
            End Set
        End Property

        Private m_ClientId As Int64
        Public Property ClientId() As Int64
            Get
                Return m_ClientId
            End Get
            Set(value As Int64)
                m_ClientId = value
            End Set
        End Property

        Private m_StateClientId As Int64
        Public Property StateClientId() As Int64
            Get
                Return m_StateClientId
            End Get
            Set(value As Int64)
                m_StateClientId = value
            End Set
        End Property

        Private m_SocialSecurityNo As String
        Public Property SocialSecurityNo() As String
            Get
                Return m_SocialSecurityNo
            End Get
            Set(value As String)
                m_SocialSecurityNo = value
            End Set
        End Property

        Private m_MissingData As String
        Public Property MissingData() As String
            Get
                Return m_MissingData
            End Get
            Set(value As String)
                m_MissingData = value
            End Set
        End Property

        Private m_Remarks As String
        Public Property Remarks() As String
            Get
                Return m_Remarks
            End Get
            Set(value As String)
                m_Remarks = value
            End Set
        End Property

        Public Sub New()

            Me.ClientId = InlineAssignHelper(Me.StateClientId, Int64.MinValue)
            Me.SocialSecurityNo = InlineAssignHelper(Me.Name, InlineAssignHelper(Me.MissingData, InlineAssignHelper(Me.Remarks, String.Empty)))

        End Sub

    End Class


End Namespace

