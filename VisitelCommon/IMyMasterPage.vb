
Namespace VisitelCommon
    Public Interface IMyMasterPage
        Sub DisplayHeaderMessage(pstrMsg As String)
        Sub DisplayHeaderError(pstrMsg As String)
        Property PageHeaderTitle() As String
    End Interface
End Namespace