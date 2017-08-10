
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: EDI Central Data Fetching and Saving Contract Number wise
' Author: Anjan Kumar Paul
' Start Date: 18 Apr 2015
' End Date: 
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                18 Apr 2015     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA
Imports System.Web.UI.WebControls


Namespace VisitelBusiness

    Public Class BLEDICentral
        Inherits BLCommon

        Public Sub SelectContractInfo(VisitelConnectionString As String, ByRef SqlDataSourceContractInfo As SqlDataSource)

            SqlDataSourceContractInfo.ProviderName = "System.Data.SqlClient"
            SqlDataSourceContractInfo.ConnectionString = VisitelConnectionString

            SqlDataSourceContractInfo.SelectCommandType = SqlDataSourceCommandType.StoredProcedure

            SqlDataSourceContractInfo.SelectCommand = "[TurboDB.SelectContractInfo]"
            SqlDataSourceContractInfo.DataBind()


        End Sub


        Public Sub SelectApplicationReceiverCodeInfo(VisitelConnectionString As String, ByRef SqlDataSourceApplicationReceiverCode As SqlDataSource)

            SqlDataSourceApplicationReceiverCode.ProviderName = "System.Data.SqlClient"
            SqlDataSourceApplicationReceiverCode.ConnectionString = VisitelConnectionString

            SqlDataSourceApplicationReceiverCode.SelectCommandType = SqlDataSourceCommandType.StoredProcedure

            SqlDataSourceApplicationReceiverCode.SelectCommand = "[TurboDB.SelectApplicationReceiverCode]"
            SqlDataSourceApplicationReceiverCode.DataBind()


        End Sub

        Public Sub SelectProviderCodeInfo(VisitelConnectionString As String, ByRef SqlDataSourceProviderCode As SqlDataSource)

            SqlDataSourceProviderCode.ProviderName = "System.Data.SqlClient"
            SqlDataSourceProviderCode.ConnectionString = VisitelConnectionString

            SqlDataSourceProviderCode.SelectCommandType = SqlDataSourceCommandType.StoredProcedure

            SqlDataSourceProviderCode.SelectCommand = "[TurboDB.SelectProviderCode]"
            SqlDataSourceProviderCode.DataBind()


        End Sub

        Public Sub SelectReferenceIdQualifierInfo(VisitelConnectionString As String, ByRef SqlDataSourceRefIdQualifier As SqlDataSource)

            SqlDataSourceRefIdQualifier.ProviderName = "System.Data.SqlClient"
            SqlDataSourceRefIdQualifier.ConnectionString = VisitelConnectionString

            SqlDataSourceRefIdQualifier.SelectCommandType = SqlDataSourceCommandType.StoredProcedure

            SqlDataSourceRefIdQualifier.SelectCommand = "[TurboDB.SelectReferenceIdQualifier]"
            SqlDataSourceRefIdQualifier.DataBind()


        End Sub


#Region "Interchange Control Header"

        Public Sub InsertInterchangeControlHeaderInfo(ConVisitel As SqlConnection, objInterchangeControlHeaderDataObject As InterchangeControlHeaderDataObject)

            Dim parameters As New HybridDictionary()

            SetInterchangeControlHeaderInfoParameters(parameters, objInterchangeControlHeaderDataObject)

            parameters.Add("@CompanyId", objInterchangeControlHeaderDataObject.CompanyId)
            parameters.Add("@UserId", objInterchangeControlHeaderDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.SaveInterchangeControlHeaderInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateInterchangeControlHeaderInfo(ConVisitel As SqlConnection, objInterchangeControlHeaderDataObject As InterchangeControlHeaderDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objInterchangeControlHeaderDataObject.Id)

            SetInterchangeControlHeaderInfoParameters(parameters, objInterchangeControlHeaderDataObject)

            parameters.Add("@UpdateBy", objInterchangeControlHeaderDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateInterchangeControlHeaderInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteInterchangeControlHeaderInfo(ConVisitel As SqlConnection, InterchangeControlHeaderId As Integer, DeletedBy As Integer)

            Dim parameters As New HybridDictionary()
            parameters.Add("@Id", InterchangeControlHeaderId)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteInterchangeControlHeaderInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetInterchangeControlHeaderInfoParameters(parameters As HybridDictionary, objInterchangeControlHeaderDataObject As InterchangeControlHeaderDataObject)

            parameters.Add("@AuthorizationInformationQualifier", objInterchangeControlHeaderDataObject.AuthorizationInformationQualifier)
            parameters.Add("@AuthorizationInformation", objInterchangeControlHeaderDataObject.AuthorizationInformation)
            parameters.Add("@SecurityInformationQualifier", objInterchangeControlHeaderDataObject.SecurityInformationQualifier)
            parameters.Add("@SecurityInformation", objInterchangeControlHeaderDataObject.SecurityInformation)
            parameters.Add("@InterchangeIdQualifier", objInterchangeControlHeaderDataObject.InterchangeIdQualifier)
            parameters.Add("@SubmitterId", objInterchangeControlHeaderDataObject.SubmitterId)
            parameters.Add("@ReceiverId", objInterchangeControlHeaderDataObject.ReceiverId)
            parameters.Add("@ControlVersionId", objInterchangeControlHeaderDataObject.ControlVersionId)
            parameters.Add("@ControlNumber", objInterchangeControlHeaderDataObject.ControlNumber)
            parameters.Add("@AcknowledgementRequested", objInterchangeControlHeaderDataObject.AcknowledgementRequested)
            parameters.Add("@UsageIndicator", objInterchangeControlHeaderDataObject.UsageIndicator)
            parameters.Add("@ContractNo", objInterchangeControlHeaderDataObject.ContractNo)

        End Sub

        Public Function SelectInterchangeControlHeaderInfo(ByRef ConVisitel As SqlConnection, ContractNumber As String) As InterchangeControlHeaderDataObject

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()
            parameters.Add("@ContractNumber", ContractNumber)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectInterchangeControlHeaderInfo]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim objInterchangeControlHeaderDataObject As New InterchangeControlHeaderDataObject()

            If drSql.HasRows Then

                If drSql.Read() Then

                    objInterchangeControlHeaderDataObject.Id = Convert.ToInt64(drSql("Id"), Nothing)

                    objInterchangeControlHeaderDataObject.AuthorizationInformationQualifier = Convert.ToString(drSql("AuthorizationInformationQualifier"), Nothing).Trim()
                    objInterchangeControlHeaderDataObject.AuthorizationInformation = Convert.ToString(drSql("AuthorizationInformation"), Nothing).Trim()
                    objInterchangeControlHeaderDataObject.SecurityInformationQualifier = Convert.ToString(drSql("SecurityInformationQualifier"), Nothing).Trim()
                    objInterchangeControlHeaderDataObject.SecurityInformation = Convert.ToString(drSql("SecurityInformation"), Nothing).Trim()
                    objInterchangeControlHeaderDataObject.InterchangeIdQualifier = Convert.ToString(drSql("InterchangeIdQualifier"), Nothing).Trim()
                    objInterchangeControlHeaderDataObject.SubmitterId = Convert.ToString(drSql("SubmitterId"), Nothing).Trim()
                    objInterchangeControlHeaderDataObject.ReceiverId = Convert.ToString(drSql("ReceiverId"), Nothing).Trim()
                    objInterchangeControlHeaderDataObject.ControlVersionId = Convert.ToString(drSql("ControlVersionId"), Nothing).Trim()
                    objInterchangeControlHeaderDataObject.ControlNumber = Convert.ToString(drSql("ControlNumber"), Nothing).Trim()
                    objInterchangeControlHeaderDataObject.AcknowledgementRequested = Convert.ToString(drSql("AcknowledgementRequested"), Nothing).Trim()
                    objInterchangeControlHeaderDataObject.UsageIndicator = Convert.ToString(drSql("UsageIndicator"), Nothing).Trim()
                    objInterchangeControlHeaderDataObject.ContractNo = Convert.ToString(drSql("ContractNo"), Nothing).Trim()

                    objInterchangeControlHeaderDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing).Trim()
                    objInterchangeControlHeaderDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing).Trim()

                End If

            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return objInterchangeControlHeaderDataObject

        End Function

        Public Class InterchangeControlHeaderDataObject

            Private m_Id As Int64
            Public Property Id() As Int64
                Get
                    Return m_Id
                End Get
                Set(value As Int64)
                    m_Id = value
                End Set
            End Property

            Private m_AuthorizationInformationQualifier As String
            Public Property AuthorizationInformationQualifier() As String
                Get
                    Return m_AuthorizationInformationQualifier
                End Get
                Set(value As String)
                    m_AuthorizationInformationQualifier = value
                End Set
            End Property

            Private m_AuthorizationInformation As String
            Public Property AuthorizationInformation() As String
                Get
                    Return m_AuthorizationInformation
                End Get
                Set(value As String)
                    m_AuthorizationInformation = value
                End Set
            End Property

            Private m_SecurityInformationQualifier As String
            Public Property SecurityInformationQualifier() As String
                Get
                    Return m_SecurityInformationQualifier
                End Get
                Set(value As String)
                    m_SecurityInformationQualifier = value
                End Set
            End Property

            Private m_SecurityInformation As String
            Public Property SecurityInformation() As String
                Get
                    Return m_SecurityInformation
                End Get
                Set(value As String)
                    m_SecurityInformation = value
                End Set
            End Property

            Private m_InterchangeIdQualifier As String
            Public Property InterchangeIdQualifier() As String
                Get
                    Return m_InterchangeIdQualifier
                End Get
                Set(value As String)
                    m_InterchangeIdQualifier = value
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

            Private m_ReceiverId As String
            Public Property ReceiverId() As String
                Get
                    Return m_ReceiverId
                End Get
                Set(value As String)
                    m_ReceiverId = value
                End Set
            End Property

            Private m_ControlVersionId As String
            Public Property ControlVersionId() As String
                Get
                    Return m_ControlVersionId
                End Get
                Set(value As String)
                    m_ControlVersionId = value
                End Set
            End Property

            Private m_ControlNumber As String
            Public Property ControlNumber() As String
                Get
                    Return m_ControlNumber
                End Get
                Set(value As String)
                    m_ControlNumber = value
                End Set
            End Property

            Private m_AcknowledgementRequested As String
            Public Property AcknowledgementRequested() As String
                Get
                    Return m_AcknowledgementRequested
                End Get
                Set(value As String)
                    m_AcknowledgementRequested = value
                End Set
            End Property

            Private m_UsageIndicator As String
            Public Property UsageIndicator() As String
                Get
                    Return m_UsageIndicator
                End Get
                Set(value As String)
                    m_UsageIndicator = value
                End Set
            End Property

            Private m_ContractNo As String
            Public Property ContractNo() As String
                Get
                    Return m_ContractNo
                End Get
                Set(value As String)
                    m_ContractNo = value
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

            Public Sub New()

                Me.Id = Int64.MinValue

                Me.AuthorizationInformationQualifier = String.Empty
                Me.AuthorizationInformation = String.Empty
                Me.SecurityInformationQualifier = String.Empty
                Me.SecurityInformation = String.Empty
                Me.InterchangeIdQualifier = String.Empty
                Me.SubmitterId = String.Empty
                Me.ReceiverId = String.Empty
                Me.ControlVersionId = String.Empty
                Me.ControlNumber = String.Empty
                Me.AcknowledgementRequested = String.Empty
                Me.UsageIndicator = String.Empty
                Me.ContractNo = String.Empty
                Me.UpdateDate = String.Empty
                Me.UpdateBy = String.Empty

                Me.CompanyId = Int64.MinValue
                Me.UserId = Int64.MinValue

            End Sub

        End Class

#End Region

#Region "Functional Group Header"

        Public Sub InsertFunctionalGroupHeaderInfo(ConVisitel As SqlConnection, objFunctionalGroupHeaderDataObject As FunctionalGroupHeaderDataObject)

            Dim parameters As New HybridDictionary()

            SetFunctionalGroupHeaderInfoParameters(parameters, objFunctionalGroupHeaderDataObject)

            parameters.Add("@CompanyId", objFunctionalGroupHeaderDataObject.CompanyId)
            parameters.Add("@UserId", objFunctionalGroupHeaderDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertFunctionalGroupHeaderInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateFunctionalGroupHeaderInfo(ConVisitel As SqlConnection, objFunctionalGroupHeaderDataObject As FunctionalGroupHeaderDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objFunctionalGroupHeaderDataObject.Id)

            SetFunctionalGroupHeaderInfoParameters(parameters, objFunctionalGroupHeaderDataObject)

            parameters.Add("@UpdateBy", objFunctionalGroupHeaderDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateFunctionalGroupHeaderInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteFunctionalGroupHeaderInfo(ConVisitel As SqlConnection, FunctionalGroupHeaderId As Integer, DeletedBy As Integer)

            Dim parameters As New HybridDictionary()
            parameters.Add("@Id", FunctionalGroupHeaderId)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteFunctionalGroupHeaderInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetFunctionalGroupHeaderInfoParameters(parameters As HybridDictionary, objFunctionalGroupHeaderDataObject As FunctionalGroupHeaderDataObject)

            parameters.Add("@FunctionalIdentifierCode", objFunctionalGroupHeaderDataObject.FunctionalIdentifierCode)
            parameters.Add("@ApplicationSenderCode", objFunctionalGroupHeaderDataObject.ApplicationSenderCode)
            parameters.Add("@ApplicationReceiverCode", objFunctionalGroupHeaderDataObject.ApplicationReceiverCode)
            parameters.Add("@GroupControlNumber", objFunctionalGroupHeaderDataObject.GroupControlNumber)
            parameters.Add("@ResponsibleAgencyCode", objFunctionalGroupHeaderDataObject.ResponsibleAgencyCode)
            parameters.Add("@IndustryIdentifierCode", objFunctionalGroupHeaderDataObject.IndustryIdentifierCode)
            parameters.Add("@ContractNo", objFunctionalGroupHeaderDataObject.ContractNo)

        End Sub

        Public Function SelectFunctionalGroupHeaderInfo(ByRef ConVisitel As SqlConnection, ContractNumber As String) As FunctionalGroupHeaderDataObject

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()
            parameters.Add("@ContractNumber", ContractNumber)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectFunctionalGroupHeaderInfo]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim objFunctionalGroupHeaderDataObject As New FunctionalGroupHeaderDataObject

            If drSql.HasRows Then

                If drSql.Read() Then

                    objFunctionalGroupHeaderDataObject.Id = Convert.ToInt64(drSql("Id"), Nothing)

                    objFunctionalGroupHeaderDataObject.FunctionalIdentifierCode = Convert.ToString(drSql("FunctionalIdentifierCode"), Nothing).Trim()
                    objFunctionalGroupHeaderDataObject.ApplicationSenderCode = Convert.ToString(drSql("ApplicationSenderCode"), Nothing).Trim()
                    objFunctionalGroupHeaderDataObject.ApplicationReceiverCode = Convert.ToString(drSql("ApplicationReceiverCode"), Nothing).Trim()
                    objFunctionalGroupHeaderDataObject.GroupControlNumber = Convert.ToString(drSql("GroupControlNumber"), Nothing).Trim()
                    objFunctionalGroupHeaderDataObject.ResponsibleAgencyCode = Convert.ToString(drSql("ResponsibleAgencyCode"), Nothing).Trim()
                    objFunctionalGroupHeaderDataObject.IndustryIdentifierCode = Convert.ToString(drSql("IndustryIdentifierCode"), Nothing).Trim()
                    objFunctionalGroupHeaderDataObject.ContractNo = Convert.ToString(drSql("ContractNo"), Nothing).Trim()

                    objFunctionalGroupHeaderDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing).Trim()
                    objFunctionalGroupHeaderDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing).Trim()

                End If

            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return objFunctionalGroupHeaderDataObject

        End Function

        Public Class FunctionalGroupHeaderDataObject

            Private m_Id As Int64
            Public Property Id() As Int64
                Get
                    Return m_Id
                End Get
                Set(value As Int64)
                    m_Id = value
                End Set
            End Property

            Private m_FunctionalIdentifierCode As String
            Public Property FunctionalIdentifierCode() As String
                Get
                    Return m_FunctionalIdentifierCode
                End Get
                Set(value As String)
                    m_FunctionalIdentifierCode = value
                End Set
            End Property

            Private m_ApplicationSenderCode As String
            Public Property ApplicationSenderCode() As String
                Get
                    Return m_ApplicationSenderCode
                End Get
                Set(value As String)
                    m_ApplicationSenderCode = value
                End Set
            End Property

            Private m_ApplicationReceiverCode As String
            Public Property ApplicationReceiverCode() As String
                Get
                    Return m_ApplicationReceiverCode
                End Get
                Set(value As String)
                    m_ApplicationReceiverCode = value
                End Set
            End Property

            Private m_GroupControlNumber As String
            Public Property GroupControlNumber() As String
                Get
                    Return m_GroupControlNumber
                End Get
                Set(value As String)
                    m_GroupControlNumber = value
                End Set
            End Property

            Private m_ResponsibleAgencyCode As String
            Public Property ResponsibleAgencyCode() As String
                Get
                    Return m_ResponsibleAgencyCode
                End Get
                Set(value As String)
                    m_ResponsibleAgencyCode = value
                End Set
            End Property

            Private m_IndustryIdentifierCode As String
            Public Property IndustryIdentifierCode() As String
                Get
                    Return m_IndustryIdentifierCode
                End Get
                Set(value As String)
                    m_IndustryIdentifierCode = value
                End Set
            End Property

            Private m_ContractNo As String
            Public Property ContractNo() As String
                Get
                    Return m_ContractNo
                End Get
                Set(value As String)
                    m_ContractNo = value
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

            Public Sub New()

                Me.Id = Int64.MinValue
                Me.FunctionalIdentifierCode = String.Empty
                Me.ApplicationSenderCode = String.Empty
                Me.ApplicationReceiverCode = String.Empty
                Me.GroupControlNumber = String.Empty
                Me.ResponsibleAgencyCode = String.Empty
                Me.IndustryIdentifierCode = String.Empty
                Me.ContractNo = String.Empty
                Me.UpdateDate = String.Empty
                Me.UpdateBy = String.Empty

                Me.CompanyId = Int64.MinValue
                Me.UserId = Int64.MinValue

            End Sub
        End Class

#End Region

#Region "Referring Provider"

        Public Sub InsertReferringProviderInfo(ConVisitel As SqlConnection, objReferringProviderDataObject As ReferringProviderDataObject)

            Dim parameters As New HybridDictionary()

            SetReferringProviderInfoParameters(parameters, objReferringProviderDataObject)

            parameters.Add("@CompanyId", objReferringProviderDataObject.CompanyId)
            parameters.Add("@UserId", objReferringProviderDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertReferringProviderInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateReferringProviderInfo(ConVisitel As SqlConnection, objReferringProviderDataObject As ReferringProviderDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objReferringProviderDataObject.Id)

            SetReferringProviderInfoParameters(parameters, objReferringProviderDataObject)

            parameters.Add("@UpdateBy", objReferringProviderDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateReferringProviderInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteReferringProviderInfo(ConVisitel As SqlConnection, ReferringProviderId As Integer, DeletedBy As Integer)

            Dim parameters As New HybridDictionary()
            parameters.Add("@Id", ReferringProviderId)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteReferringProviderInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetReferringProviderInfoParameters(parameters As HybridDictionary, objReferringProviderDataObject As ReferringProviderDataObject)

            parameters.Add("@EntityIdCode", objReferringProviderDataObject.EntityIdCode)
            parameters.Add("@EntityTypeQualifier", objReferringProviderDataObject.EntityTypeQualifier)
            parameters.Add("@LastOrOrginizationName", objReferringProviderDataObject.LastOrOrginizationName)
            parameters.Add("@FirstName", objReferringProviderDataObject.FirstName)
            parameters.Add("@MiddleName", objReferringProviderDataObject.MiddleName)
            parameters.Add("@Prefix", objReferringProviderDataObject.Prefix)
            parameters.Add("@Suffix", objReferringProviderDataObject.Suffix)
            parameters.Add("@Address", objReferringProviderDataObject.Address)
            parameters.Add("@City", objReferringProviderDataObject.City)
            parameters.Add("@State", objReferringProviderDataObject.State)
            parameters.Add("@Zip", objReferringProviderDataObject.Zip)
            parameters.Add("@ReferenceIdQualifier", objReferringProviderDataObject.ReferenceIdQualifier)
            parameters.Add("@EINOrSSN", objReferringProviderDataObject.EINOrSSN)
            parameters.Add("@IdCodeQualifier", objReferringProviderDataObject.IdCodeQualifier)
            parameters.Add("@ReferringProviderIdentifier", objReferringProviderDataObject.ReferringProviderIdentifier)
            parameters.Add("@ContractNo", objReferringProviderDataObject.ContractNo)

        End Sub

        Public Function SelectReferringProviderInfo(ByRef ConVisitel As SqlConnection, ContractNumber As String) As ReferringProviderDataObject

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()
            parameters.Add("@ContractNumber", ContractNumber)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectReferringProviderInfo]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim objReferringProviderDataObject As New ReferringProviderDataObject
            If drSql.HasRows Then

                If drSql.Read() Then

                    objReferringProviderDataObject.Id = Convert.ToInt64(drSql("Id"), Nothing)

                    objReferringProviderDataObject.EntityIdCode = Convert.ToString(drSql("EntityIdCode"), Nothing).Trim()
                    objReferringProviderDataObject.EntityTypeQualifier = Convert.ToString(drSql("EntityTypeQualifier"), Nothing).Trim()
                    objReferringProviderDataObject.LastOrOrginizationName = Convert.ToString(drSql("LastOrOrginizationName"), Nothing).Trim()
                    objReferringProviderDataObject.FirstName = Convert.ToString(drSql("FirstName"), Nothing).Trim()
                    objReferringProviderDataObject.MiddleName = Convert.ToString(drSql("MiddleName"), Nothing).Trim()
                    objReferringProviderDataObject.Prefix = Convert.ToString(drSql("Prefix"), Nothing).Trim()
                    objReferringProviderDataObject.Suffix = Convert.ToString(drSql("Suffix"), Nothing).Trim()
                    objReferringProviderDataObject.Address = Convert.ToString(drSql("Address"), Nothing).Trim()
                    objReferringProviderDataObject.City = Convert.ToString(drSql("City"), Nothing).Trim()
                    objReferringProviderDataObject.State = Convert.ToString(drSql("State"), Nothing).Trim()
                    objReferringProviderDataObject.Zip = Convert.ToString(drSql("Zip"), Nothing).Trim()
                    objReferringProviderDataObject.ReferenceIdQualifier = Convert.ToString(drSql("ReferenceIdQualifier"), Nothing).Trim()
                    objReferringProviderDataObject.EINOrSSN = Convert.ToString(drSql("EINOrSSN"), Nothing).Trim()
                    objReferringProviderDataObject.IdCodeQualifier = Convert.ToString(drSql("IdCodeQualifier"), Nothing).Trim()
                    objReferringProviderDataObject.ReferringProviderIdentifier = Convert.ToString(drSql("ReferringProviderIdentifier"), Nothing).Trim()
                    objReferringProviderDataObject.ContractNo = Convert.ToString(drSql("ContractNo"), Nothing).Trim()

                    objReferringProviderDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing).Trim()
                    objReferringProviderDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing).Trim()

                End If

            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return objReferringProviderDataObject

        End Function

        Public Class ReferringProviderDataObject

            Private m_Id As Int64
            Public Property Id() As Int64
                Get
                    Return m_Id
                End Get
                Set(value As Int64)
                    m_Id = value
                End Set
            End Property

            Private m_EntityIdCode As String
            Public Property EntityIdCode() As String
                Get
                    Return m_EntityIdCode
                End Get
                Set(value As String)
                    m_EntityIdCode = value
                End Set
            End Property

            Private m_EntityTypeQualifier As String
            Public Property EntityTypeQualifier() As String
                Get
                    Return m_EntityTypeQualifier
                End Get
                Set(value As String)
                    m_EntityTypeQualifier = value
                End Set
            End Property

            Private m_LastOrOrginizationName As String
            Public Property LastOrOrginizationName() As String
                Get
                    Return m_LastOrOrginizationName
                End Get
                Set(value As String)
                    m_LastOrOrginizationName = value
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

            Private m_MiddleName As String
            Public Property MiddleName() As String
                Get
                    Return m_MiddleName
                End Get
                Set(value As String)
                    m_MiddleName = value
                End Set
            End Property

            Private m_Prefix As String
            Public Property Prefix() As String
                Get
                    Return m_Prefix
                End Get
                Set(value As String)
                    m_Prefix = value
                End Set
            End Property

            Private m_Suffix As String
            Public Property Suffix() As String
                Get
                    Return m_Suffix
                End Get
                Set(value As String)
                    m_Suffix = value
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

            Private m_ReferenceIdQualifier As String
            Public Property ReferenceIdQualifier() As String
                Get
                    Return m_ReferenceIdQualifier
                End Get
                Set(value As String)
                    m_ReferenceIdQualifier = value
                End Set
            End Property

            Private m_EINOrSSN As String
            Public Property EINOrSSN() As String
                Get
                    Return m_EINOrSSN
                End Get
                Set(value As String)
                    m_EINOrSSN = value
                End Set
            End Property

            Private m_IdCodeQualifier As String
            Public Property IdCodeQualifier() As String
                Get
                    Return m_IdCodeQualifier
                End Get
                Set(value As String)
                    m_IdCodeQualifier = value
                End Set
            End Property

            Private m_ReferringProviderIdentifier As String
            Public Property ReferringProviderIdentifier() As String
                Get
                    Return m_ReferringProviderIdentifier
                End Get
                Set(value As String)
                    m_ReferringProviderIdentifier = value
                End Set
            End Property

            Private m_ContractNo As String
            Public Property ContractNo() As String
                Get
                    Return m_ContractNo
                End Get
                Set(value As String)
                    m_ContractNo = value
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

            Public Sub New()

                Me.Id = Int64.MinValue

                Me.EntityIdCode = String.Empty
                Me.EntityTypeQualifier = String.Empty
                Me.LastOrOrginizationName = String.Empty
                Me.FirstName = String.Empty
                Me.MiddleName = String.Empty
                Me.Prefix = String.Empty
                Me.Suffix = String.Empty
                Me.Address = String.Empty
                Me.City = String.Empty
                Me.State = String.Empty
                Me.Zip = String.Empty
                Me.ReferenceIdQualifier = String.Empty
                Me.EINOrSSN = String.Empty
                Me.IdCodeQualifier = String.Empty
                Me.ReferringProviderIdentifier = String.Empty
                Me.ContractNo = String.Empty
                Me.UpdateDate = String.Empty
                Me.UpdateBy = String.Empty

                Me.CompanyId = Int64.MinValue
                Me.UserId = Int64.MinValue

            End Sub
        End Class
#End Region

#Region "Billing Provider"

        Public Sub InsertBillingProviderInfo(ConVisitel As SqlConnection, objBillingProviderDataObject As BillingProviderDataObject)

            Dim parameters As New HybridDictionary()

            SetBillingProviderInfoParameters(parameters, objBillingProviderDataObject)

            parameters.Add("@CompanyId", objBillingProviderDataObject.CompanyId)
            parameters.Add("@UserId", objBillingProviderDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertBillingProviderInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateBillingProviderInfo(ConVisitel As SqlConnection, objBillingProviderDataObject As BillingProviderDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objBillingProviderDataObject.Id)

            SetBillingProviderInfoParameters(parameters, objBillingProviderDataObject)

            parameters.Add("@UpdateBy", objBillingProviderDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateBillingProviderInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteBillingProviderInfo(ConVisitel As SqlConnection, BillingProviderId As Integer, DeletedBy As Integer)

            Dim parameters As New HybridDictionary()
            parameters.Add("@Id", BillingProviderId)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteBillingProviderInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetBillingProviderInfoParameters(parameters As HybridDictionary, objBillingProviderDataObject As BillingProviderDataObject)

            parameters.Add("@EntityIdCode", objBillingProviderDataObject.EntityIdCode)
            parameters.Add("@EntityTypeQualifier", objBillingProviderDataObject.EntityTypeQualifier)
            parameters.Add("@LastOrOrginizationName", objBillingProviderDataObject.LastOrOrginizationName)
            parameters.Add("@FirstName", objBillingProviderDataObject.FirstName)
            parameters.Add("@MiddleName", objBillingProviderDataObject.MiddleName)
            parameters.Add("@Prefix", objBillingProviderDataObject.Prefix)
            parameters.Add("@Suffix", objBillingProviderDataObject.Suffix)
            parameters.Add("@Address", objBillingProviderDataObject.Address)
            parameters.Add("@City", objBillingProviderDataObject.City)
            parameters.Add("@State", objBillingProviderDataObject.State)
            parameters.Add("@Zip", objBillingProviderDataObject.Zip)
            parameters.Add("@ReferenceIdQualifier", objBillingProviderDataObject.ReferenceIdQualifier)
            parameters.Add("@EINOrSSN", objBillingProviderDataObject.EINOrSSN)
            parameters.Add("@IdCodeQualifier", objBillingProviderDataObject.IdCodeQualifier)
            parameters.Add("@NPI", objBillingProviderDataObject.NPI)
            parameters.Add("@ContractNo", objBillingProviderDataObject.ContractNo)

        End Sub

        Public Function SelectBillingProviderInfo(ByRef ConVisitel As SqlConnection, ContractNumber As String) As BillingProviderDataObject

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()
            parameters.Add("@ContractNumber", ContractNumber)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectBillingProviderInfo]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim objBillingProviderDataObject As New BillingProviderDataObject

            If drSql.HasRows Then

                If drSql.Read() Then

                    objBillingProviderDataObject.Id = Convert.ToInt64(drSql("Id"), Nothing)

                    objBillingProviderDataObject.EntityIdCode = Convert.ToString(drSql("EntityIdCode"), Nothing).Trim()
                    objBillingProviderDataObject.EntityTypeQualifier = Convert.ToString(drSql("EntityTypeQualifier"), Nothing).Trim()
                    objBillingProviderDataObject.LastOrOrginizationName = Convert.ToString(drSql("LastOrOrginizationName"), Nothing).Trim()
                    objBillingProviderDataObject.FirstName = Convert.ToString(drSql("FirstName"), Nothing).Trim()
                    objBillingProviderDataObject.MiddleName = Convert.ToString(drSql("MiddleName"), Nothing).Trim()
                    objBillingProviderDataObject.Prefix = Convert.ToString(drSql("Prefix"), Nothing).Trim()
                    objBillingProviderDataObject.Suffix = Convert.ToString(drSql("Suffix"), Nothing).Trim()
                    objBillingProviderDataObject.Address = Convert.ToString(drSql("Address"), Nothing).Trim()
                    objBillingProviderDataObject.City = Convert.ToString(drSql("City"), Nothing).Trim()
                    objBillingProviderDataObject.State = Convert.ToString(drSql("State"), Nothing).Trim()
                    objBillingProviderDataObject.Zip = Convert.ToString(drSql("Zip"), Nothing).Trim()
                    objBillingProviderDataObject.ReferenceIdQualifier = Convert.ToString(drSql("ReferenceIdQualifier"), Nothing).Trim()
                    objBillingProviderDataObject.EINOrSSN = Convert.ToString(drSql("EINOrSSN"), Nothing).Trim()
                    objBillingProviderDataObject.IdCodeQualifier = Convert.ToString(drSql("IdCodeQualifier"), Nothing).Trim()
                    objBillingProviderDataObject.NPI = Convert.ToString(drSql("NPI"), Nothing).Trim()
                    objBillingProviderDataObject.ContractNo = Convert.ToString(drSql("ContractNo"), Nothing).Trim()

                    objBillingProviderDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing).Trim()
                    objBillingProviderDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing).Trim()

                End If

            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return objBillingProviderDataObject

        End Function

        Public Class BillingProviderDataObject

            Private m_Id As Int64
            Public Property Id() As Int64
                Get
                    Return m_Id
                End Get
                Set(value As Int64)
                    m_Id = value
                End Set
            End Property

            Private m_EntityIdCode As String
            Public Property EntityIdCode() As String
                Get
                    Return m_EntityIdCode
                End Get
                Set(value As String)
                    m_EntityIdCode = value
                End Set
            End Property

            Private m_EntityTypeQualifier As String
            Public Property EntityTypeQualifier() As String
                Get
                    Return m_EntityTypeQualifier
                End Get
                Set(value As String)
                    m_EntityTypeQualifier = value
                End Set
            End Property

            Private m_LastOrOrginizationName As String
            Public Property LastOrOrginizationName() As String
                Get
                    Return m_LastOrOrginizationName
                End Get
                Set(value As String)
                    m_LastOrOrginizationName = value
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

            Private m_MiddleName As String
            Public Property MiddleName() As String
                Get
                    Return m_MiddleName
                End Get
                Set(value As String)
                    m_MiddleName = value
                End Set
            End Property

            Private m_Prefix As String
            Public Property Prefix() As String
                Get
                    Return m_Prefix
                End Get
                Set(value As String)
                    m_Prefix = value
                End Set
            End Property

            Private m_Suffix As String
            Public Property Suffix() As String
                Get
                    Return m_Suffix
                End Get
                Set(value As String)
                    m_Suffix = value
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

            Private m_ReferenceIdQualifier As String
            Public Property ReferenceIdQualifier() As String
                Get
                    Return m_ReferenceIdQualifier
                End Get
                Set(value As String)
                    m_ReferenceIdQualifier = value
                End Set
            End Property

            Private m_EINOrSSN As String
            Public Property EINOrSSN() As String
                Get
                    Return m_EINOrSSN
                End Get
                Set(value As String)
                    m_EINOrSSN = value
                End Set
            End Property

            Private m_IdCodeQualifier As String
            Public Property IdCodeQualifier() As String
                Get
                    Return m_IdCodeQualifier
                End Get
                Set(value As String)
                    m_IdCodeQualifier = value
                End Set
            End Property

            Private m_NPI As String
            Public Property NPI() As String
                Get
                    Return m_NPI
                End Get
                Set(value As String)
                    m_NPI = value
                End Set
            End Property

            Private m_ContractNo As String
            Public Property ContractNo() As String
                Get
                    Return m_ContractNo
                End Get
                Set(value As String)
                    m_ContractNo = value
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

            Public Sub New()

                Me.Id = Int64.MinValue

                Me.EntityIdCode = String.Empty
                Me.EntityTypeQualifier = String.Empty
                Me.LastOrOrginizationName = String.Empty
                Me.FirstName = String.Empty
                Me.MiddleName = String.Empty
                Me.Prefix = String.Empty
                Me.Suffix = String.Empty
                Me.Address = String.Empty
                Me.City = String.Empty
                Me.State = String.Empty
                Me.Zip = String.Empty
                Me.ReferenceIdQualifier = String.Empty
                Me.EINOrSSN = String.Empty
                Me.IdCodeQualifier = String.Empty
                Me.NPI = String.Empty
                Me.ContractNo = String.Empty
                Me.UpdateDate = String.Empty
                Me.UpdateBy = String.Empty

                Me.CompanyId = Int64.MinValue
                Me.UserId = Int64.MinValue

            End Sub
        End Class
#End Region

#Region "Rendering Provider"

        Public Sub InsertRenderingProviderInfo(ConVisitel As SqlConnection, objRenderingProviderDataObject As RenderingProviderDataObject)

            Dim parameters As New HybridDictionary()

            SetRenderingProviderInfoParameters(parameters, objRenderingProviderDataObject)

            parameters.Add("@CompanyId", objRenderingProviderDataObject.CompanyId)
            parameters.Add("@UserId", objRenderingProviderDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertRenderingProviderInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateRenderingProviderInfo(ConVisitel As SqlConnection, objRenderingProviderDataObject As RenderingProviderDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objRenderingProviderDataObject.Id)

            SetRenderingProviderInfoParameters(parameters, objRenderingProviderDataObject)

            parameters.Add("@UpdateBy", objRenderingProviderDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateRenderingProviderInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteRenderingProviderInfo(ConVisitel As SqlConnection, RenderingProviderId As Integer, DeletedBy As Integer)

            Dim parameters As New HybridDictionary()
            parameters.Add("@Id", RenderingProviderId)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteRenderingProviderInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetRenderingProviderInfoParameters(parameters As HybridDictionary, objRenderingProviderDataObject As RenderingProviderDataObject)

            parameters.Add("@EntityIdCode", objRenderingProviderDataObject.EntityIdCode)
            parameters.Add("@EntityTypeQualifier", objRenderingProviderDataObject.EntityTypeQualifier)
            parameters.Add("@LastOrOrganizationName", objRenderingProviderDataObject.LastOrOrganizationName)
            parameters.Add("@FirstName", objRenderingProviderDataObject.FirstName)
            parameters.Add("@MiddleName", objRenderingProviderDataObject.MiddleName)
            parameters.Add("@Prefix", objRenderingProviderDataObject.Prefix)
            parameters.Add("@Suffix", objRenderingProviderDataObject.Suffix)
            parameters.Add("@Address", objRenderingProviderDataObject.Address)
            parameters.Add("@City", objRenderingProviderDataObject.City)
            parameters.Add("@State", objRenderingProviderDataObject.State)
            parameters.Add("@Zip", objRenderingProviderDataObject.Zip)
            parameters.Add("@ReferenceIdQualifier", objRenderingProviderDataObject.ReferenceIdQualifier)
            parameters.Add("@EINOrSSN", objRenderingProviderDataObject.EINOrSSN)
            parameters.Add("@IdCodeQualifier", objRenderingProviderDataObject.IdCodeQualifier)
            parameters.Add("@RenderingProviderIdentifier", objRenderingProviderDataObject.RenderingProviderIdentifier)
            parameters.Add("@ContractNo", objRenderingProviderDataObject.ContractNo)
            parameters.Add("@ProviderCode", objRenderingProviderDataObject.ProviderCode)
            parameters.Add("@ProviderReferenceIdQualifier", objRenderingProviderDataObject.ProviderReferenceIdQualifier)
            parameters.Add("@TaxonomyCode", objRenderingProviderDataObject.TaxonomyCode)

        End Sub

        Public Function SelectRenderingProviderInfo(ByRef ConVisitel As SqlConnection, ContractNumber As String) As RenderingProviderDataObject

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()
            parameters.Add("@ContractNumber", ContractNumber)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectRenderingProviderInfo]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim objRenderingProviderDataObject As New RenderingProviderDataObject

            If drSql.HasRows Then

                If drSql.Read() Then

                    objRenderingProviderDataObject.Id = Convert.ToInt64(drSql("Id"), Nothing)

                    objRenderingProviderDataObject.EntityIdCode = Convert.ToString(drSql("EntityIdCode"), Nothing).Trim()
                    objRenderingProviderDataObject.EntityTypeQualifier = Convert.ToString(drSql("EntityTypeQualifier"), Nothing).Trim()
                    objRenderingProviderDataObject.LastOrOrganizationName = Convert.ToString(drSql("LastOrOrganizationName"), Nothing).Trim()
                    objRenderingProviderDataObject.FirstName = Convert.ToString(drSql("FirstName"), Nothing).Trim()
                    objRenderingProviderDataObject.MiddleName = Convert.ToString(drSql("MiddleName"), Nothing).Trim()
                    objRenderingProviderDataObject.Prefix = Convert.ToString(drSql("Prefix"), Nothing).Trim()
                    objRenderingProviderDataObject.Suffix = Convert.ToString(drSql("Suffix"), Nothing).Trim()
                    objRenderingProviderDataObject.Address = Convert.ToString(drSql("Address"), Nothing).Trim()
                    objRenderingProviderDataObject.City = Convert.ToString(drSql("City"), Nothing).Trim()
                    objRenderingProviderDataObject.State = Convert.ToString(drSql("State"), Nothing).Trim()
                    objRenderingProviderDataObject.Zip = Convert.ToString(drSql("Zip"), Nothing).Trim()
                    objRenderingProviderDataObject.ReferenceIdQualifier = Convert.ToString(drSql("ReferenceIdQualifier"), Nothing).Trim()
                    objRenderingProviderDataObject.EINOrSSN = Convert.ToString(drSql("EINOrSSN"), Nothing).Trim()
                    objRenderingProviderDataObject.IdCodeQualifier = Convert.ToString(drSql("IdCodeQualifier"), Nothing).Trim()
                    objRenderingProviderDataObject.RenderingProviderIdentifier = Convert.ToString(drSql("RenderingProviderIdentifier"), Nothing).Trim()
                    objRenderingProviderDataObject.ContractNo = Convert.ToString(drSql("ContractNo"), Nothing).Trim()
                    objRenderingProviderDataObject.ProviderCode = Convert.ToString(drSql("ProviderCode"), Nothing).Trim()
                    objRenderingProviderDataObject.ProviderReferenceIdQualifier = Convert.ToString(drSql("ProviderReferenceIdQualifier"), Nothing).Trim()
                    objRenderingProviderDataObject.TaxonomyCode = Convert.ToString(drSql("TaxonomyCode"), Nothing).Trim()

                    objRenderingProviderDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing).Trim()
                    objRenderingProviderDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing).Trim()

                End If

            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return objRenderingProviderDataObject

        End Function

        Public Class RenderingProviderDataObject

            Private m_Id As Int64
            Public Property Id() As Int64
                Get
                    Return m_Id
                End Get
                Set(value As Int64)
                    m_Id = value
                End Set
            End Property

            Private m_EntityIdCode As String
            Public Property EntityIdCode() As String
                Get
                    Return m_EntityIdCode
                End Get
                Set(value As String)
                    m_EntityIdCode = value
                End Set
            End Property

            Private m_EntityTypeQualifier As String
            Public Property EntityTypeQualifier() As String
                Get
                    Return m_EntityTypeQualifier
                End Get
                Set(value As String)
                    m_EntityTypeQualifier = value
                End Set
            End Property

            Private m_LastOrOrganizationName As String
            Public Property LastOrOrganizationName() As String
                Get
                    Return m_LastOrOrganizationName
                End Get
                Set(value As String)
                    m_LastOrOrganizationName = value
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

            Private m_MiddleName As String
            Public Property MiddleName() As String
                Get
                    Return m_MiddleName
                End Get
                Set(value As String)
                    m_MiddleName = value
                End Set
            End Property

            Private m_Prefix As String
            Public Property Prefix() As String
                Get
                    Return m_Prefix
                End Get
                Set(value As String)
                    m_Prefix = value
                End Set
            End Property

            Private m_Suffix As String
            Public Property Suffix() As String
                Get
                    Return m_Suffix
                End Get
                Set(value As String)
                    m_Suffix = value
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

            Private m_ReferenceIdQualifier As String
            Public Property ReferenceIdQualifier() As String
                Get
                    Return m_ReferenceIdQualifier
                End Get
                Set(value As String)
                    m_ReferenceIdQualifier = value
                End Set
            End Property

            Private m_EINOrSSN As String
            Public Property EINOrSSN() As String
                Get
                    Return m_EINOrSSN
                End Get
                Set(value As String)
                    m_EINOrSSN = value
                End Set
            End Property

            Private m_IdCodeQualifier As String
            Public Property IdCodeQualifier() As String
                Get
                    Return m_IdCodeQualifier
                End Get
                Set(value As String)
                    m_IdCodeQualifier = value
                End Set
            End Property

            Private m_RenderingProviderIdentifier As String
            Public Property RenderingProviderIdentifier() As String
                Get
                    Return m_RenderingProviderIdentifier
                End Get
                Set(value As String)
                    m_RenderingProviderIdentifier = value
                End Set
            End Property

            Private m_ContractNo As String
            Public Property ContractNo() As String
                Get
                    Return m_ContractNo
                End Get
                Set(value As String)
                    m_ContractNo = value
                End Set
            End Property

            Private m_ProviderCode As String
            Public Property ProviderCode() As String
                Get
                    Return m_ProviderCode
                End Get
                Set(value As String)
                    m_ProviderCode = value
                End Set
            End Property

            Private m_ProviderReferenceIdQualifier As String
            Public Property ProviderReferenceIdQualifier() As String
                Get
                    Return m_ProviderReferenceIdQualifier
                End Get
                Set(value As String)
                    m_ProviderReferenceIdQualifier = value
                End Set
            End Property

            Private m_TaxonomyCode As String
            Public Property TaxonomyCode() As String
                Get
                    Return m_TaxonomyCode
                End Get
                Set(value As String)
                    m_TaxonomyCode = value
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

            Public Sub New()

                Me.Id = Int64.MinValue

                Me.EntityIdCode = String.Empty
                Me.EntityTypeQualifier = String.Empty
                Me.LastOrOrganizationName = String.Empty
                Me.FirstName = String.Empty
                Me.MiddleName = String.Empty
                Me.Prefix = String.Empty
                Me.Suffix = String.Empty
                Me.Address = String.Empty
                Me.City = String.Empty
                Me.State = String.Empty
                Me.Zip = String.Empty
                Me.ReferenceIdQualifier = String.Empty
                Me.EINOrSSN = String.Empty
                Me.IdCodeQualifier = String.Empty
                Me.RenderingProviderIdentifier = String.Empty
                Me.ContractNo = String.Empty
                Me.ProviderCode = String.Empty
                Me.ProviderReferenceIdQualifier = String.Empty
                Me.TaxonomyCode = String.Empty
                Me.UpdateDate = String.Empty
                Me.UpdateBy = String.Empty

                Me.CompanyId = Int64.MinValue
                Me.UserId = Int64.MinValue

            End Sub
        End Class
#End Region

#Region "Submitter"

        Public Sub InsertSubmitterInfo(ConVisitel As SqlConnection, objSubmitterDataObject As SubmitterDataObject)

            Dim parameters As New HybridDictionary()

            SetSubmitterInfoParameters(parameters, objSubmitterDataObject)

            parameters.Add("@CompanyId", objSubmitterDataObject.CompanyId)
            parameters.Add("@UserId", objSubmitterDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertSubmitterInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateSubmitterInfo(ConVisitel As SqlConnection, objSubmitterDataObject As SubmitterDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objSubmitterDataObject.Id)

            SetSubmitterInfoParameters(parameters, objSubmitterDataObject)

            parameters.Add("@UpdateBy", objSubmitterDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateSubmitterInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteSubmitterInfo(ConVisitel As SqlConnection, SubmitterId As Integer, DeletedBy As Integer)

            Dim parameters As New HybridDictionary()
            parameters.Add("@Id", SubmitterId)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteSubmitterInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetSubmitterInfoParameters(parameters As HybridDictionary, objSubmitterDataObject As SubmitterDataObject)

            parameters.Add("@EntityIdentifierCode", objSubmitterDataObject.EntityIdentifierCode)
            parameters.Add("@EntityTypeQualifier", objSubmitterDataObject.EntityTypeQualifier)
            parameters.Add("@NameLastOrOrganizationName", objSubmitterDataObject.NameLastOrOrganizationName)
            parameters.Add("@FirstName", objSubmitterDataObject.FirstName)
            parameters.Add("@MiddleName", objSubmitterDataObject.MiddleName)
            parameters.Add("@Prefix", objSubmitterDataObject.Prefix)
            parameters.Add("@Suffix", objSubmitterDataObject.Suffix)
            parameters.Add("@PrimaryIdentificationNumber", objSubmitterDataObject.PrimaryIdentificationNumber)
            parameters.Add("@ContractName", objSubmitterDataObject.ContractName)
            parameters.Add("@Phone", objSubmitterDataObject.Phone)
            parameters.Add("@ContractNo", objSubmitterDataObject.ContractNo)

        End Sub

        Public Function SelectSubmitterInfo(ByRef ConVisitel As SqlConnection, ContractNumber As String) As SubmitterDataObject

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()
            parameters.Add("@ContractNumber", ContractNumber)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectSubmitterInfo]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim objSubmitterDataObject As New SubmitterDataObject

            If drSql.HasRows Then

                If drSql.Read() Then

                    objSubmitterDataObject.Id = Convert.ToInt64(drSql("Id"), Nothing)

                    objSubmitterDataObject.EntityIdentifierCode = Convert.ToString(drSql("EntityIdentifierCode"), Nothing).Trim()
                    objSubmitterDataObject.EntityTypeQualifier = Convert.ToString(drSql("EntityTypeQualifier"), Nothing).Trim()
                    objSubmitterDataObject.NameLastOrOrganizationName = Convert.ToString(drSql("NameLastOrOrganizationName"), Nothing).Trim()
                    objSubmitterDataObject.FirstName = Convert.ToString(drSql("FirstName"), Nothing).Trim()
                    objSubmitterDataObject.MiddleName = Convert.ToString(drSql("MiddleName"), Nothing).Trim()
                    objSubmitterDataObject.Prefix = Convert.ToString(drSql("Prefix"), Nothing).Trim()
                    objSubmitterDataObject.Suffix = Convert.ToString(drSql("Suffix"), Nothing).Trim()
                    objSubmitterDataObject.PrimaryIdentificationNumber = Convert.ToString(drSql("PrimaryIdentificationNumber"), Nothing).Trim()
                    objSubmitterDataObject.ContractName = Convert.ToString(drSql("ContractName"), Nothing).Trim()
                    objSubmitterDataObject.Phone = Convert.ToString(drSql("Phone"), Nothing).Trim()
                    objSubmitterDataObject.ContractNo = Convert.ToString(drSql("ContractNo"), Nothing).Trim()

                    objSubmitterDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing).Trim()
                    objSubmitterDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing).Trim()

                End If

            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return objSubmitterDataObject

        End Function

        Public Class SubmitterDataObject

            Private m_Id As Int64
            Public Property Id() As Int64
                Get
                    Return m_Id
                End Get
                Set(value As Int64)
                    m_Id = value
                End Set
            End Property

            Private m_EntityIdentifierCode As String
            Public Property EntityIdentifierCode() As String
                Get
                    Return m_EntityIdentifierCode
                End Get
                Set(value As String)
                    m_EntityIdentifierCode = value
                End Set
            End Property

            Private m_EntityTypeQualifier As String
            Public Property EntityTypeQualifier() As String
                Get
                    Return m_EntityTypeQualifier
                End Get
                Set(value As String)
                    m_EntityTypeQualifier = value
                End Set
            End Property

            Private m_NameLastOrOrganizationName As String
            Public Property NameLastOrOrganizationName() As String
                Get
                    Return m_NameLastOrOrganizationName
                End Get
                Set(value As String)
                    m_NameLastOrOrganizationName = value
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

            Private m_MiddleName As String
            Public Property MiddleName() As String
                Get
                    Return m_MiddleName
                End Get
                Set(value As String)
                    m_MiddleName = value
                End Set
            End Property

            Private m_Prefix As String
            Public Property Prefix() As String
                Get
                    Return m_Prefix
                End Get
                Set(value As String)
                    m_Prefix = value
                End Set
            End Property

            Private m_Suffix As String
            Public Property Suffix() As String
                Get
                    Return m_Suffix
                End Get
                Set(value As String)
                    m_Suffix = value
                End Set
            End Property

            Private m_PrimaryIdentificationNumber As String
            Public Property PrimaryIdentificationNumber() As String
                Get
                    Return m_PrimaryIdentificationNumber
                End Get
                Set(value As String)
                    m_PrimaryIdentificationNumber = value
                End Set
            End Property

            Private m_ContractName As String
            Public Property ContractName() As String
                Get
                    Return m_ContractName
                End Get
                Set(value As String)
                    m_ContractName = value
                End Set
            End Property

            Private m_Phone As String
            Public Property Phone() As String
                Get
                    Return m_Phone
                End Get
                Set(value As String)
                    m_Phone = value
                End Set
            End Property

            Private m_ContractNo As String
            Public Property ContractNo() As String
                Get
                    Return m_ContractNo
                End Get
                Set(value As String)
                    m_ContractNo = value
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

            Public Sub New()

                Me.Id = Int64.MinValue

                Me.EntityIdentifierCode = String.Empty
                Me.EntityTypeQualifier = String.Empty
                Me.NameLastOrOrganizationName = String.Empty
                Me.FirstName = String.Empty
                Me.MiddleName = String.Empty
                Me.Prefix = String.Empty
                Me.Suffix = String.Empty
                Me.PrimaryIdentificationNumber = String.Empty
                Me.ContractName = String.Empty
                Me.Phone = String.Empty
                Me.ContractNo = String.Empty
                Me.UpdateDate = String.Empty
                Me.UpdateBy = String.Empty

                Me.CompanyId = Int64.MinValue
                Me.UserId = Int64.MinValue

            End Sub
        End Class
#End Region

#Region "Bill Or Pay To Provider"

        Public Sub InsertBillOrPayToProviderInfo(ConVisitel As SqlConnection, objBillOrPayToProviderDataObject As BillOrPayToProviderDataObject)

            Dim parameters As New HybridDictionary()

            SetBillOrPayToProviderInfoParameters(parameters, objBillOrPayToProviderDataObject)

            parameters.Add("@CompanyId", objBillOrPayToProviderDataObject.CompanyId)
            parameters.Add("@UserId", objBillOrPayToProviderDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertBillOrPayToProviderInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateBillOrPayToProviderInfo(ConVisitel As SqlConnection, objBillOrPayToProviderDataObject As BillOrPayToProviderDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objBillOrPayToProviderDataObject.Id)

            SetBillOrPayToProviderInfoParameters(parameters, objBillOrPayToProviderDataObject)

            parameters.Add("@UpdateBy", objBillOrPayToProviderDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateBillOrPayToProviderInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteBillOrPayToProviderInfo(ConVisitel As SqlConnection, BillOrPayToProviderId As Integer, DeletedBy As Integer)

            Dim parameters As New HybridDictionary()
            parameters.Add("@Id", BillOrPayToProviderId)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteBillOrPayToProviderInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetBillOrPayToProviderInfoParameters(parameters As HybridDictionary, objBillOrPayToProviderDataObject As BillOrPayToProviderDataObject)

            parameters.Add("@HierarchicalIdNumber", objBillOrPayToProviderDataObject.HierarchicalIdNumber)
            parameters.Add("@HierarchicalLevelCode", objBillOrPayToProviderDataObject.HierarchicalLevelCode)
            parameters.Add("@HierarchicalChildCode", objBillOrPayToProviderDataObject.HierarchicalChildCode)
            parameters.Add("@ProviderCode", objBillOrPayToProviderDataObject.ProviderCode)
            parameters.Add("@ReferenceIdQualifier", objBillOrPayToProviderDataObject.ReferenceIdQualifier)
            parameters.Add("@TaxonomyCode", objBillOrPayToProviderDataObject.TaxonomyCode)
            parameters.Add("@ContractNo", objBillOrPayToProviderDataObject.ContractNo)

        End Sub

        Public Function SelectBillOrPayToProviderInfo(ByRef ConVisitel As SqlConnection, ContractNumber As String) As BillOrPayToProviderDataObject

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()
            parameters.Add("@ContractNumber", ContractNumber)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectBillOrPayToProviderInfo]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim objBillOrPayToProviderDataObject As New BillOrPayToProviderDataObject

            If drSql.HasRows Then

                If drSql.Read() Then

                    objBillOrPayToProviderDataObject.Id = Convert.ToInt64(drSql("Id"), Nothing)

                    objBillOrPayToProviderDataObject.HierarchicalIdNumber = Convert.ToString(drSql("HierarchicalIdNumber"), Nothing).Trim()
                    objBillOrPayToProviderDataObject.HierarchicalLevelCode = Convert.ToString(drSql("HierarchicalLevelCode"), Nothing).Trim()
                    objBillOrPayToProviderDataObject.HierarchicalChildCode = Convert.ToString(drSql("HierarchicalChildCode"), Nothing).Trim()
                    objBillOrPayToProviderDataObject.ProviderCode = Convert.ToString(drSql("ProviderCode"), Nothing).Trim()
                    objBillOrPayToProviderDataObject.ReferenceIdQualifier = Convert.ToString(drSql("ReferenceIdQualifier"), Nothing).Trim()
                    objBillOrPayToProviderDataObject.TaxonomyCode = Convert.ToString(drSql("TaxonomyCode"), Nothing).Trim()
                    objBillOrPayToProviderDataObject.ContractNo = Convert.ToString(drSql("ContractNo"), Nothing).Trim()

                    objBillOrPayToProviderDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing).Trim()
                    objBillOrPayToProviderDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing).Trim()

                End If

            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return objBillOrPayToProviderDataObject

        End Function

        Public Class BillOrPayToProviderDataObject

            Private m_Id As Int64
            Public Property Id() As Int64
                Get
                    Return m_Id
                End Get
                Set(value As Int64)
                    m_Id = value
                End Set
            End Property

            Private m_HierarchicalIdNumber As String
            Public Property HierarchicalIdNumber() As String
                Get
                    Return m_HierarchicalIdNumber
                End Get
                Set(value As String)
                    m_HierarchicalIdNumber = value
                End Set
            End Property

            Private m_HierarchicalLevelCode As String
            Public Property HierarchicalLevelCode() As String
                Get
                    Return m_HierarchicalLevelCode
                End Get
                Set(value As String)
                    m_HierarchicalLevelCode = value
                End Set
            End Property

            Private m_HierarchicalChildCode As Int64
            Public Property HierarchicalChildCode() As Int64
                Get
                    Return m_HierarchicalChildCode
                End Get
                Set(value As Int64)
                    m_HierarchicalChildCode = value
                End Set
            End Property

            Private m_ProviderCode As String
            Public Property ProviderCode() As String
                Get
                    Return m_ProviderCode
                End Get
                Set(value As String)
                    m_ProviderCode = value
                End Set
            End Property

            Private m_ReferenceIdQualifier As String
            Public Property ReferenceIdQualifier() As String
                Get
                    Return m_ReferenceIdQualifier
                End Get
                Set(value As String)
                    m_ReferenceIdQualifier = value
                End Set
            End Property

            Private m_TaxonomyCode As String
            Public Property TaxonomyCode() As String
                Get
                    Return m_TaxonomyCode
                End Get
                Set(value As String)
                    m_TaxonomyCode = value
                End Set
            End Property

            Private m_ContractNo As String
            Public Property ContractNo() As String
                Get
                    Return m_ContractNo
                End Get
                Set(value As String)
                    m_ContractNo = value
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

            Public Sub New()

                Me.Id = Int64.MinValue

                Me.HierarchicalIdNumber = String.Empty
                Me.HierarchicalLevelCode = String.Empty
                Me.HierarchicalChildCode = Int64.MinValue
                Me.ProviderCode = String.Empty
                Me.ReferenceIdQualifier = String.Empty
                Me.TaxonomyCode = String.Empty
                Me.ContractNo = String.Empty
                Me.UpdateDate = String.Empty
                Me.UpdateBy = String.Empty

                Me.CompanyId = Int64.MinValue
                Me.UserId = Int64.MinValue

            End Sub

        End Class
#End Region

#Region "Subscriber"

        Public Sub InsertSubscriberInfo(ConVisitel As SqlConnection, objSubscriberInfoDataObject As SubscriberInfoDataObject)

            Dim parameters As New HybridDictionary()

            SetSubscriberInfoParameters(parameters, objSubscriberInfoDataObject)

            parameters.Add("@CompanyId", objSubscriberInfoDataObject.CompanyId)
            parameters.Add("@UserId", objSubscriberInfoDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertSubscriberInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateSubscriberInfo(ConVisitel As SqlConnection, objSubscriberInfoDataObject As SubscriberInfoDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objSubscriberInfoDataObject.Id)

            SetSubscriberInfoParameters(parameters, objSubscriberInfoDataObject)

            parameters.Add("@UpdateBy", objSubscriberInfoDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateSubscriberInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteSubscriberInfo(ConVisitel As SqlConnection, SubscriberId As Integer, DeletedBy As Integer)

            Dim parameters As New HybridDictionary()
            parameters.Add("@Id", SubscriberId)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteSubscriberInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetSubscriberInfoParameters(parameters As HybridDictionary, objSubscriberInfoDataObject As SubscriberInfoDataObject)

            parameters.Add("@PayerResponsibilitySequenceNumberCode", objSubscriberInfoDataObject.PayerResponsibilitySequenceNumberCode)
            parameters.Add("@RelationshipCode", objSubscriberInfoDataObject.RelationshipCode)
            parameters.Add("@GroupOrPolicyNumber", objSubscriberInfoDataObject.GroupOrPolicyNumber)
            parameters.Add("@GroupOrPlanName", objSubscriberInfoDataObject.GroupOrPlanName)
            parameters.Add("@InsuranceTypeCode", objSubscriberInfoDataObject.InsuranceTypeCode)
            parameters.Add("@ClaimFilingIndicatorCode", objSubscriberInfoDataObject.ClaimFilingIndicatorCode)
            parameters.Add("@EntityIdentificationCode", objSubscriberInfoDataObject.EntityIdentificationCode)
            parameters.Add("@EntityTypeQualifier", objSubscriberInfoDataObject.EntityTypeQualifier)
            parameters.Add("@IdCodeQualifier", objSubscriberInfoDataObject.IdCodeQualifier)
            parameters.Add("@ContractNo", objSubscriberInfoDataObject.ContractNo)

        End Sub

        Public Function SelectSubscriberInfo(ByRef ConVisitel As SqlConnection, ContractNumber As String) As SubscriberInfoDataObject

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()
            parameters.Add("@ContractNumber", ContractNumber)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectSubscriberInfo]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim objSubscriberInfoDataObject As New SubscriberInfoDataObject

            If drSql.HasRows Then

                If drSql.Read() Then

                    objSubscriberInfoDataObject.Id = Convert.ToInt64(drSql("Id"), Nothing)

                    objSubscriberInfoDataObject.PayerResponsibilitySequenceNumberCode = Convert.ToString(drSql("PayerResponsibilitySequenceNumberCode"), Nothing).Trim()
                    objSubscriberInfoDataObject.RelationshipCode = Convert.ToString(drSql("RelationshipCode"), Nothing).Trim()
                    objSubscriberInfoDataObject.GroupOrPolicyNumber = Convert.ToString(drSql("GroupOrPolicyNumber"), Nothing).Trim()
                    objSubscriberInfoDataObject.GroupOrPlanName = Convert.ToString(drSql("GroupOrPlanName"), Nothing).Trim()
                    objSubscriberInfoDataObject.InsuranceTypeCode = Convert.ToString(drSql("InsuranceTypeCode"), Nothing).Trim()
                    objSubscriberInfoDataObject.ClaimFilingIndicatorCode = Convert.ToString(drSql("ClaimFilingIndicatorCode"), Nothing).Trim()
                    objSubscriberInfoDataObject.EntityIdentificationCode = Convert.ToString(drSql("EntityIdentificationCode"), Nothing).Trim()
                    objSubscriberInfoDataObject.EntityTypeQualifier = Convert.ToString(drSql("EntityTypeQualifier"), Nothing).Trim()
                    objSubscriberInfoDataObject.IdCodeQualifier = Convert.ToString(drSql("IdCodeQualifier"), Nothing).Trim()
                    objSubscriberInfoDataObject.ContractNo = Convert.ToString(drSql("ContractNo"), Nothing).Trim()

                    objSubscriberInfoDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing).Trim()
                    objSubscriberInfoDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing).Trim()

                End If

            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return objSubscriberInfoDataObject

        End Function

        Public Class SubscriberInfoDataObject

            Private m_Id As Int64
            Public Property Id() As Int64
                Get
                    Return m_Id
                End Get
                Set(value As Int64)
                    m_Id = value
                End Set
            End Property

            Private m_PayerResponsibilitySequenceNumberCode As String
            Public Property PayerResponsibilitySequenceNumberCode() As String
                Get
                    Return m_PayerResponsibilitySequenceNumberCode
                End Get
                Set(value As String)
                    m_PayerResponsibilitySequenceNumberCode = value
                End Set
            End Property

            Private m_RelationshipCode As String
            Public Property RelationshipCode() As String
                Get
                    Return m_RelationshipCode
                End Get
                Set(value As String)
                    m_RelationshipCode = value
                End Set
            End Property

            Private m_GroupOrPolicyNumber As String
            Public Property GroupOrPolicyNumber() As String
                Get
                    Return m_GroupOrPolicyNumber
                End Get
                Set(value As String)
                    m_GroupOrPolicyNumber = value
                End Set
            End Property

            Private m_GroupOrPlanName As String
            Public Property GroupOrPlanName() As String
                Get
                    Return m_GroupOrPlanName
                End Get
                Set(value As String)
                    m_GroupOrPlanName = value
                End Set
            End Property

            Private m_InsuranceTypeCode As String
            Public Property InsuranceTypeCode() As String
                Get
                    Return m_InsuranceTypeCode
                End Get
                Set(value As String)
                    m_InsuranceTypeCode = value
                End Set
            End Property

            Private m_ClaimFilingIndicatorCode As String
            Public Property ClaimFilingIndicatorCode() As String
                Get
                    Return m_ClaimFilingIndicatorCode
                End Get
                Set(value As String)
                    m_ClaimFilingIndicatorCode = value
                End Set
            End Property

            Private m_EntityIdentificationCode As String
            Public Property EntityIdentificationCode() As String
                Get
                    Return m_EntityIdentificationCode
                End Get
                Set(value As String)
                    m_EntityIdentificationCode = value
                End Set
            End Property

            Private m_EntityTypeQualifier As String
            Public Property EntityTypeQualifier() As String
                Get
                    Return m_EntityTypeQualifier
                End Get
                Set(value As String)
                    m_EntityTypeQualifier = value
                End Set
            End Property

            Private m_IdCodeQualifier As String
            Public Property IdCodeQualifier() As String
                Get
                    Return m_IdCodeQualifier
                End Get
                Set(value As String)
                    m_IdCodeQualifier = value
                End Set
            End Property

            Private m_ContractNo As String
            Public Property ContractNo() As String
                Get
                    Return m_ContractNo
                End Get
                Set(value As String)
                    m_ContractNo = value
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

            Public Sub New()

                Me.Id = Int64.MinValue

                Me.PayerResponsibilitySequenceNumberCode = String.Empty
                Me.RelationshipCode = String.Empty
                Me.GroupOrPolicyNumber = String.Empty
                Me.GroupOrPlanName = String.Empty
                Me.InsuranceTypeCode = String.Empty
                Me.ClaimFilingIndicatorCode = String.Empty
                Me.EntityIdentificationCode = String.Empty
                Me.EntityTypeQualifier = String.Empty
                Me.IdCodeQualifier = String.Empty
                Me.ContractNo = String.Empty
                Me.UpdateDate = String.Empty
                Me.UpdateBy = String.Empty

                Me.CompanyId = Int64.MinValue
                Me.UserId = Int64.MinValue

            End Sub

        End Class
#End Region

    End Class

    
End Namespace

