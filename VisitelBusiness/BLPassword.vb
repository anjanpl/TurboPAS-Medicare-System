Imports System.Text
Imports System.Security.Cryptography

Namespace VisitelBusiness
    Public Class BLPassword
        Private m_password As String
        Private m_salt As Integer

        Public Sub New(Password As String, Salt As Integer)
            m_password = Password
            m_salt = Salt
        End Sub

        Public Shared Function CreateRandomPassword(PasswordLength As Integer) As String
            Dim AllowedChars As [String] = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ23456789"
            Dim RandomBytes As [Byte]() = New [Byte](PasswordLength - 1) {}
            Dim objRNGCryptoServiceProvider As New RNGCryptoServiceProvider()
            objRNGCryptoServiceProvider.GetBytes(RandomBytes)
            Dim chars As Char() = New Char(PasswordLength - 1) {}
            Dim AllowedCharCount As Integer = AllowedChars.Length

            For i As Integer = 0 To PasswordLength - 1
                chars(i) = AllowedChars(CInt(RandomBytes(i)) Mod AllowedCharCount)
            Next

            Return New String(chars)
        End Function

        Public Shared Function CreateRandomSalt() As Integer
            Dim SaltBytes As [Byte]() = New [Byte](3) {}
            Dim objRNGCryptoServiceProvider As New RNGCryptoServiceProvider()
            objRNGCryptoServiceProvider.GetBytes(SaltBytes)

            Return ((CInt(SaltBytes(0)) << 24) + (CInt(SaltBytes(1)) << 16) + (CInt(SaltBytes(2)) << 8) + CInt(SaltBytes(3)))
        End Function

        Public Function ComputeSaltedHash() As String
            ' Create Byte array of password string
            Dim encoder As New ASCIIEncoding()
            Dim SecretBytes As [Byte]() = encoder.GetBytes(m_password)

            ' Create a new salt
            'Dim SaltBytes As [Byte]() = New [Byte](3) {}
            'SaltBytes(0) = CByte(m_salt >> 24)
            'SaltBytes(1) = CByte(m_salt >> 16)
            'SaltBytes(2) = CByte(m_salt >> 8)
            'SaltBytes(3) = CByte(m_salt)


            Dim SaltBytes As Byte() = Encoding.UTF8.GetBytes(m_salt)


            ' append the two arrays
            Dim toHash As [Byte]() = New [Byte](SecretBytes.Length + (SaltBytes.Length - 1)) {}
            Array.Copy(SecretBytes, 0, toHash, 0, SecretBytes.Length)
            Array.Copy(SaltBytes, 0, toHash, SecretBytes.Length, SaltBytes.Length)

            Dim sha1__1 As SHA1 = SHA1.Create()
            Dim computedHash As [Byte]() = sha1__1.ComputeHash(toHash)

            Return encoder.GetString(computedHash)
        End Function
    End Class
End Namespace