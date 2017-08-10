Namespace VisitelCommon

    Public Enum WebServiceStatusEnum
        Success = 0
        Failed = 1
        NotAuthorized = 2
        NoSession = 3
    End Enum

    ''' <summary>
    ''' Response returned by a web service that contains only status information
    ''' </summary>
    Public Class WebServiceResponse

        Private m_Status As WebServiceStatusEnum
        Public Property Status() As WebServiceStatusEnum
            Get
                Return m_Status
            End Get
            Set(value As WebServiceStatusEnum)
                m_Status = value
            End Set
        End Property

        Private m_StatusString As String
        Public Property StatusString() As String
            Get
                Return m_StatusString
            End Get
            Set(value As String)
                m_StatusString = value
            End Set
        End Property

        Public Shared Function GetNoSessionResponse() As WebServiceResponse
            Return New WebServiceResponse() With { _
             .Status = WebServiceStatusEnum.NoSession, _
             .StatusString = "You are currently not logged in." _
            }
        End Function

        Public Shared Function GetNotAuthorizedResponse() As WebServiceResponse
            Return New WebServiceResponse() With { _
             .Status = WebServiceStatusEnum.NotAuthorized, _
             .StatusString = "Your account is not authorized to perform the requested action." _
            }
        End Function

    End Class

    ''' <summary>
    ''' Response returned by a web service that includes status information
    ''' </summary>
    Public Class WebServiceResponse(Of T)
        Inherits WebServiceResponse

        Private m_ReturnValue As T
        Public Property ReturnValue() As T
            Get
                Return m_ReturnValue
            End Get
            Set(value As T)
                m_ReturnValue = Value
            End Set
        End Property

        Public Sub New()
        End Sub

        Public Sub New(returnValue As T, statusEnum As WebServiceStatusEnum, statusString As String)
            Me.ReturnValue = returnValue
            Me.Status = statusEnum
            Me.StatusString = statusString
        End Sub

        Public Sub New(returnValue As T)
            ' created this constructor for success cases - status will always be success and statusString is irrelevant
            Me.ReturnValue = returnValue
            Me.Status = WebServiceStatusEnum.Success
            Me.StatusString = ""
        End Sub

        Public Sub New(statusEnum As WebServiceStatusEnum, statusString As String)
            ' created this constructor for failed cases - we do not want to return an object in this case.
            Me.Status = statusEnum
            Me.StatusString = statusString
        End Sub

        Public Shared Shadows Function GetNoSessionResponse() As WebServiceResponse(Of T)
            Return New WebServiceResponse(Of T)(WebServiceStatusEnum.NoSession, "You are currently not logged in.")
        End Function

        Public Shared Shadows Function GetNotAuthorizedResponse() As WebServiceResponse(Of T)
            Return New WebServiceResponse(Of T)(WebServiceStatusEnum.NotAuthorized, "Your account is not authorized to perform the requested action.")
        End Function

    End Class
End Namespace
