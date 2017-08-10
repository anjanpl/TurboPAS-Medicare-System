#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Common Methods Incorporation
' Author: Anjan Kumar Paul
' Start Date: 06 Dec 2014
' End Date: 
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                06 Dec 2014    Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Globalization

Namespace VisitelBusiness
    Public Class BLCommon

        'Protected DateFormat As String = "MMMM d, yyyy"
        Protected DateFormat As String = "M/d/yy"

        Protected Function GetCultureInfo() As CultureInfo
            Dim webCultureInfo1 As New CultureInfo("en-us")

            Return webCultureInfo1
        End Function

        ''' <summary>
        ''' Formatted the mobile number as (xxx) xxx-xxxx
        ''' </summary>
        ''' <param name="MobileNumber"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function GetFormattedMobileNumber(MobileNumber As String) As String
            Dim result As String = String.Empty
            If (Not String.IsNullOrEmpty(MobileNumber)) Then
                result = "(" + MobileNumber.Substring(0, 3) + ") " + MobileNumber.Substring(3, 3) + "-" + MobileNumber.Substring(6, 4)
            End If
            Return result
        End Function

        ''' <summary>
        ''' Formatted the social security number number as xxx-xx-xxxx
        ''' </summary>
        ''' <param name="SocialSecurityNumber"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function GetFormattedSocialSecurityNumber(SocialSecurityNumber As String) As String
            Dim result As String = String.Empty
            If (Not String.IsNullOrEmpty(SocialSecurityNumber)) Then
                result = SocialSecurityNumber.Substring(0, 3) + "-" + SocialSecurityNumber.Substring(3, 2) + "-" + SocialSecurityNumber.Substring(5, 4)
            End If
            Return result
        End Function

        Protected Function GetQueryStringFormattedDate(DateString As String) As String

            If (Not String.IsNullOrEmpty(DateString)) Then
                DateString = DateString.Substring(0, 4) & "/" & DateString.Substring(4, 2) & "/" & DateString.Substring(6, 2)
                DateString = Convert.ToDateTime(DateString, GetCultureInfo()).ToString(DateFormat)
            End If

            Return DateString

        End Function

    End Class
End Namespace
