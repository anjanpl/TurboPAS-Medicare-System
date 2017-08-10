
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: EDI Codes Save & Delete
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
Imports VisitelBusiness.VisitelBusiness.DataObject

Namespace VisitelBusiness

    Public Class BLEDICodes
        Inherits BLCommon

        Private m_ShowFooter As Boolean
        Public Property ShowFooter() As Boolean
            Get
                Return m_ShowFooter
            End Get
            Set(value As Boolean)
                m_ShowFooter = value
            End Set
        End Property


        Public Sub InsertEDICodeInfo(ConVisitel As SqlConnection, objEDICodesDataObject As EDICodesDataObject)

            Dim parameters As New HybridDictionary()

            SetParameters(parameters, objEDICodesDataObject)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertEDICodeInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateEDICodeInfo(ConVisitel As SqlConnection, objEDICodesDataObject As EDICodesDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objEDICodesDataObject.Id)

            SetParameters(parameters, objEDICodesDataObject)

            parameters.Add("@UpdateBy", objEDICodesDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateEDICodeInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteEDICodeInfo(ConVisitel As SqlConnection, Id As Int64, DeletedBy As String)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", Id)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteEDICodeInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Function SelectEDICodes(ByRef ConVisitel As SqlConnection, CodeTable As String, CodeColumn As String, EdiCodesType As String) As List(Of EDICodesDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            Select Case EdiCodesType
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesType.EDICentral)
                    parameters.Add("@EdiCodesType", EdiCodesType)
                    parameters.Add("@CodeTable", CodeTable)
                    parameters.Add("@CodeColumn", CodeColumn)
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EdiCodesType.EDICorrectedClaims)
                    parameters.Add("@EdiCodesType", EdiCodesType)
                    Exit Select

            End Select

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectEdiCodes]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim EDICodesList As New List(Of EDICodesDataObject)

            If drSql.HasRows Then
                Dim objEDICodesDataObject As EDICodesDataObject
                While drSql.Read()

                    objEDICodesDataObject = New EDICodesDataObject()

                    objEDICodesDataObject.CodeDefinition = Convert.ToString(drSql("CodeDefinition"), Nothing)
                    objEDICodesDataObject.Code = Convert.ToString(drSql("Code"), Nothing)

                    EDICodesList.Add(objEDICodesDataObject)

                End While
                objEDICodesDataObject = Nothing
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return EDICodesList

        End Function

        Public Function SelectEDICodes(ByRef ConVisitel As SqlConnection) As List(Of EDICodesDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectEdiCodesInfo]", drSql, ConVisitel, Nothing)
            objSharedSettings = Nothing

            Dim EDICodesList As New List(Of EDICodesDataObject)
            Dim objEDICodesDataObject As EDICodesDataObject

            If drSql.HasRows Then
                Me.ShowFooter = True
                While drSql.Read()

                    objEDICodesDataObject = New EDICodesDataObject()

                    objEDICodesDataObject.Id = If((DBNull.Value.Equals(drSql("Id"))), objEDICodesDataObject.Id, Convert.ToInt32(drSql("Id")))
                    objEDICodesDataObject.Code = Convert.ToString(drSql("Code"), Nothing)
                    objEDICodesDataObject.CodeDefinition = Convert.ToString(drSql("Definition"), Nothing)
                    objEDICodesDataObject.CodeTable = Convert.ToString(drSql("Table"), Nothing)
                    objEDICodesDataObject.CodeColumn = Convert.ToString(drSql("Column"), Nothing)
                    objEDICodesDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing)

                    'objEDICodesDataObject.UpdateDate = If((Not String.IsNullOrEmpty(objEDICodesDataObject.UpdateDate)),
                    '                                      Convert.ToDateTime(objEDICodesDataObject.UpdateDate, GetCultureInfo()).ToString(DateFormat),
                    '                                      objEDICodesDataObject.UpdateDate)

                    objEDICodesDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing)

                    EDICodesList.Add(objEDICodesDataObject)

                End While
                objEDICodesDataObject = Nothing

            Else
                'objEDICodesDataObject = New EDICodesDataObject()
                'EDICodesList.Add(objEDICodesDataObject)
                'objEDICodesDataObject = Nothing
                Me.ShowFooter = False
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return EDICodesList

        End Function

        Private Sub SetParameters(parameters As HybridDictionary, objEDICodesDataObject As EDICodesDataObject)

            parameters.Add("@Code", objEDICodesDataObject.Code)
            parameters.Add("@Definition", objEDICodesDataObject.CodeDefinition)
            parameters.Add("@Codetable", objEDICodesDataObject.CodeTable)
            parameters.Add("@Codecolumn", objEDICodesDataObject.CodeColumn)

        End Sub


    End Class

    Public Class EDICodesDataObject

        Private m_Id As Integer
        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(value As Integer)
                m_Id = value
            End Set
        End Property

        Private m_Code As String
        Public Property Code() As String
            Get
                Return m_Code
            End Get
            Set(value As String)
                m_Code = value
            End Set
        End Property

        Private m_CodeDefinition As String
        Public Property CodeDefinition() As String
            Get
                Return m_CodeDefinition
            End Get
            Set(value As String)
                m_CodeDefinition = value
            End Set
        End Property

        Private m_CodeTable As String
        Public Property CodeTable() As String
            Get
                Return m_CodeTable
            End Get
            Set(value As String)
                m_CodeTable = value
            End Set
        End Property

        Private m_CodeColumn As String
        Public Property CodeColumn() As String
            Get
                Return m_CodeColumn
            End Get
            Set(value As String)
                m_CodeColumn = value
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

            Me.Id = Integer.MinValue

            Me.Code = String.Empty
            Me.CodeDefinition = String.Empty
            Me.CodeTable = String.Empty
            Me.CodeColumn = String.Empty
            Me.Remarks = String.Empty

        End Sub
    End Class
End Namespace

