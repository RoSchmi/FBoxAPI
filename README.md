# FBoxAPI

Dieses Projekt ist ein .NET Bibliothek f�r die TR-064 Schnittstelle der AVM Fritz!Box. 
Das Projekt ist eine Ausgliederung aus meinem Addin f�r Microsoft Outlook Fritz!Box Telefon-dingsbums V5

Dieses Addin ist in meiner Freizeit entstanden. Ich erwarte keine Gegenleistung. Ein Danke ist ausreichend. Wer mir dennoch etwas Gutes zukommen lassen m�chte kann dies gerne tun:

[![Donate](https://img.shields.io/badge/Spenden-PayPal-green.svg)](https://www.paypal.com/paypalme/gertmichael)

### Grundlagen
Die Schnittstelle basiert auf der [AVM-Dokumentation](https://avm.de/service/schnittstellen). 

### Nutzung
Die Verwendung ist recht einfach angedacht. Es muss eine neue `FBoxAPI.FritzBoxTR64`-Klasse instanziiert werden. Hierf�r sind zwei Parameter erforderlich: IP-Adresse der Fritz!Box und die 
Anmeldeinformationen. Nutzername und Passwort werden in einer neuen Instanz der `System.Net.NetworkCredential`-Klasse hinterlegt und �bergeben.
Im folgenen ist ein kleines Beispiel aufgef�hrt, wie die SessionID der Fritz!Box abgefragt werden kann. 

```vbnet
Private Function GetSessionID() As String
    ' Bereitstellung der Variable(n), in die das Ergebnis gesetzt werden sollen.
    Dim SessionID As String = "0000000000000000"

    ' Erstelle Anmeldeinformationen f�r die Fritz!Box bereit
    Dim Nutzername As String = "Fritz"
    Dim Passwort As String = "Box"

    ' ANmeldeinformationen k�nnen Nothing sein, falls nur Actions ausgef�hrt werden, die keine Anmeldung erfordern.
    Dim Anmeldeinformationen As New Net.NetworkCredential(Nutzername, Passwort)

    ' Starte die TR-064 Schnittstelle zur Fritz!Box
    Using FBoxTR064 As New FBoxAPI.FritzBoxTR64("192.168.178.1", Anmeldeinformationen)

        ' Auwahl des Service
        With FBoxTR064.Deviceconfig

            ' Action ausf�hren
            If .GetSessionID(SessionID) Then
                ' Alles OK
            Else
                ' Ein Fehler ist aufgetreten
            End If
        End With

    End Using

    Return SessionID

End Function
```

Sobald eine neue `FBoxAPI.FritzBoxTR64`-Klasse erstellt wurde, kann auch auf das Event `Status` abgefragt werden. 
Die FBoxAPI.LogMessage beinhaltet eine Eigenschaft f�r ein `LogLevel` (Enum) und eine Eigenschaft f�r eine `Message` (String).
```vbnet
    Friend Sub FBoxAPIMessage(sender As Object, e As FBoxAPI.NotifyEventArgs(Of FBoxAPI.LogMessage))
        ' Nlog:
        NLogger.Log(LogLevel.FromOrdinal(e.Value.Level), e.Value.Message)
    End Sub
```
### Umsetzung

folgende Services werden derzeit unterst�tzt. Es ist geplant weitere Services und deren Actions zu erg�nzen.

* [X_AVM-DE_OnTel](https://avm.de/fileadmin/user_upload/Global/Service/Schnittstellen/x_contactSCPD.pdf)	
* [X_AVM-DE_TAM](https://avm.de/fileadmin/user_upload/Global/Service/Schnittstellen/x_tam.pdf)
* [X_VoIP](https://avm.de/fileadmin/user_upload/Global/Service/Schnittstellen/x_voip-avm.pdf)			
* [DeviceInfo](https://avm.de/fileadmin/user_upload/Global/Service/Schnittstellen/deviceinfoSCPD.pdf)
* [DeviceConfig](https://avm.de/fileadmin/user_upload/Global/Service/Schnittstellen/deviceconfigSCPD.pdf)
* [LANConfigSecurity](https://avm.de/fileadmin/user_upload/Global/Service/Schnittstellen/lanconfigsecuritySCPD.pdf)
* [WANCommonInterfaceConfig](https://avm.de/fileadmin/user_upload/Global/Service/Schnittstellen/wancommonifconfigSCPD.pdf)

### Markenrecht
Dieses Outlook-Addin wird vom Autor privat in der Freizeit als Hobby gepflegt. Mit der Bereitstellung des Outlook-Addins werden keine gewerblichen Interessen verfolgt. Es wird aus rein ideellen Gr�nden zum Gemeinwohl aller Nutzer einer Fritz!Box betrieben. 
Die Erstellung dieser Software erfolgt nicht im Auftrag oder mit Wissen der Firmen AVM GmbH bzw. Microsoft Corporation. Diese Software wurde unabh�ngig erstellt. Der Autor pflegt im Zusammenhang mit dieser Software keine Beziehungen zur Firma AVM GmbH oder Microsoft Corporation.