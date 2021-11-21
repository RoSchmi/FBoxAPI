﻿Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Xml.Serialization

Friend Module Serializer

#Region "XML"
#Region "CheckXMLData"
    ''' <summary>
    ''' Überprüft, ob die einzulesenden Daten überhaupt eine XML sind. Dazu wird versucht die XML Daten einzulesen. 
    ''' Wenn die Daten eingelesen werden können, werden sie als <see cref="XmlDocument"/> zur weiteren Verarbeitung in <paramref name="xDoc"/> bereitgestellt.
    ''' </summary>
    ''' <param name="InputData">Die einzulesenden Daten</param>
    ''' <param name="IsPfad">Angabe, ob ein Dateipfad oder XML-Daten geprüft werden sollen.</param>
    ''' <param name="xDoc">XML-Daten zur weiteren Verwendung</param>
    ''' <returns>Boolean</returns>
    Private Function CheckXMLData(InputData As String, IsPfad As Boolean, ByRef xDoc As XmlDocument) As Boolean

        If InputData.IsNotStringNothingOrEmpty Then
            Try
                ' Versuche die Datei zu laden, wenn es keine Exception gibt, ist alles ok

                With xDoc
                    ' Verhindere, dass etwaige HTML-Seiten validiert werden. Hier friert der Prozess ein.
                    .XmlResolver = Nothing

                    If IsPfad Then
                        .Load(InputData)
                    Else
                        .LoadXml(InputData)
                    End If
                End With

                Return True

            Catch ex As XmlException
                'NLogger.Fatal(ex, $"Die XML-Datan weist einen Lade- oder Analysefehler auf: '{InputData}'")

                Return False

            Catch ex As FileNotFoundException
                'NLogger.Fatal(ex, $"Die XML-Datan kann nicht gefunden werden: '{InputData}'")

                Return False

            End Try
        Else
            'NLogger.Fatal("Die übergebenen XML-Datan sind leer.")

            Return False
        End If
    End Function

#End Region

#Region "XML Deserialisieren"
    ''' <summary>
    ''' Deserialisiert die XML-Datei, die unter <paramref name="Data"/> gespeichert ist.
    ''' </summary>
    ''' <typeparam name="T">Zieltdatentyp</typeparam>
    ''' <param name="Data">Speicherort</param>
    ''' <param name="IsPath">Angabe, ob es sich um einen Pfad handelt.</param>
    ''' <param name="ReturnObj">Deserialisiertes Datenobjekt vom Type <typeparamref name="T"/>.</param>
    ''' <returns>True oder False, je nach Ergebnis der Deserialisierung</returns>
    Friend Function DeserializeXML(Of T)(Data As String, IsPath As Boolean, ByRef ReturnObj As T) As Boolean

        Dim xDoc As New XmlDocument
        If CheckXMLData(Data, IsPath, xDoc) Then

            Dim Serializer As New XmlSerializer(GetType(T))

            ' Erstelle einen XMLReader zum Deserialisieren des XML-Documentes
            Using Reader As New XmlNodeReader(xDoc)

                If Serializer.CanDeserialize(Reader) Then
                    Try
                        ReturnObj = CType(Serializer.Deserialize(Reader, New XmlDeserializationEvents With {.OnUnknownAttribute = AddressOf On_UnknownAttribute,
                                                                                                            .OnUnknownElement = AddressOf On_UnknownElement,
                                                                                                            .OnUnknownNode = AddressOf On_UnknownNode,
                                                                                                            .OnUnreferencedObject = AddressOf On_UnreferencedObject}), T)

                        Return True

                    Catch ex As InvalidOperationException

                        'NLogger.Fatal(ex, $"Bei der Deserialisierung ist ein Fehler aufgetreten.")
                        Return False
                    End Try
                Else
                    'NLogger.Fatal($"Fehler beim Deserialisieren.")
                    Return False
                End If


            End Using

        Else
            'NLogger.Fatal($"Fehler beim Deserialisieren: {Data} kann nicht deserialisert werden.")
            Return False

        End If
        xDoc = Nothing
    End Function

#End Region

#Region "XmlDeserializationEvents"
    Private Sub On_UnknownAttribute(sender As Object, e As XmlAttributeEventArgs)
        'NLogger.Warn($"Unknown Attribute: {e.Attr.Name} in {e.ObjectBeingDeserialized}")
    End Sub

    Private Sub On_UnknownElement(sender As Object, e As XmlElementEventArgs)
        'NLogger.Warn($"Unknown Element: {e.Element.Name} in {e.ObjectBeingDeserialized}")
    End Sub

    Private Sub On_UnknownNode(sender As Object, e As XmlNodeEventArgs)
        'NLogger.Warn($"Unknown Node: {e.Name} in {e.ObjectBeingDeserialized}")
    End Sub

    Private Sub On_UnreferencedObject(sender As Object, e As UnreferencedObjectEventArgs)
        'NLogger.Warn($"Unreferenced Object: {e.UnreferencedId}")
    End Sub
#End Region

#End Region

End Module