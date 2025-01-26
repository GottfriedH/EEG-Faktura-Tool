# EEG-Faktura-Tool
Tool zum Erstellen von SEPA-Datenträgern für die EEG-Faktura

ACHTUNG: Der Autor übernimmt keine KEINE Haftung für dieses Tool. Die Verwendung erfolgt auf eigene Gefahr!

A) VORAUSSETZUNGEN
	1) Microsoft Excel (R) muss installiert sein!

B) INSTALLATION:

	1) Die Dateien "EEG-Faktura-Tool.exe" und "EEG-Faktura-Tool.exe.config" in einen Ordner kopieren (zB C:\EEG-Faktura-Tool\)

	2) Die Datei "EEG-Faktura-Tool.exe.config" mit einem Editor öffnen und an die eigenen Gegebenheiten anpassen (Vereinsname, IBAN, BIC, ....).

	2.1) Wenn für alle Einzüge ein fixes Mandat verwendet werden soll, dieses unter "FixesLastschriftMandat" eintragen. Schreibweise genau beachten !!!
		<setting name="FixesLastschriftMandat" serializeAs="String">
	                <value>Mein fixes Mandat</value>
	        </setting>
	     ACHTUNG: Bei Firmenlastschriften muss für jedes Mitglied eine eindeutige Mandatsreferenz (zB die Mitgliedsnummer) verwendet werden!

	2.2) In der Einstellung "BelegeZusammenfassen" kann eingestellt werden, ob für ein Mitglied die Gutschrift und Rechnung in eine Transaktion zusammengefasst werden soll (True) oder ob für Rechnung und Gutschrift je eine eigene Transaktion erstellt werden soll (False)


C) DATEI FÜR MANDATSREFERENZEN

	1) Musterdatei "Mandate.XLS" herunterladen
 
	2) Mandate laut Musterzeile einfügen
 
	3) ACHTUNG: Das Mandatsausstellungsdatum muss mit dem Datum auf dem unterschriebenen SEPA-Mandat übereinstimmen!
 
	4) Bei SEPA-Firmenlastschriften in die Spalte "Firmenlastschrift" ein "Ja" eintragen.
	   Bei Privaten Mitgliedern die Spalte leer lassen

D) SEPA-Datenträger erstellen

	1) EEG-Faktura Abrechnung herunterladen (RCxxxxxx_abrechnung_Abr_YQ-2024-4_export.XLSX)
 
	2) EEG-Faktura-Tool.EXE starten
 	   Sollte sich die Anwendung nicht öffnen, bitte prüfen, ob sich in der Datei "EEG-Faktura-Tool.exe.config" ein Fehler eingeschlichen hat!   
	2.1) Die Einstellungen im unteren Bereich der Maske prüfen!
 
 	3) Menüpunkt "Datei / EEG-Faktura-Datei importieren" aufrufen und die Excel-Datei auswählen
  
	4) OPTIONAL: Wenn keine fixe Mandatsreferenz verwendet wird, den Menüpunkt "Datei / Sepa-Mandate importieren" aufrufen
	4.1) Es kommt eine Meldung mit der Anzahl der importierten Mandaten: Es wurde(n) xxx Mandat(e) importiert.
	4.2) Die Mandate werden über die Mitgliedsnummer zugeordnet. Wenn alles passt kommt die Meldung: Es konnten alle Mandate zugeordnet werden.
	     Ansonsten die Mandate ergänzen / korrigieren und den Vorgang wiederholen
      
	5) Menüpunkt "Datei / Sepa-Datenträger erstellen" aufrufen
 
	6) Für die Lastschriften und Überweisungen wird je eine Datei erstellt
	   In der Datei für die Lastschriften wir für "SEPA-Lastschriften" (Private) und "SEPA-Firmenlastschriften" je ein Bestand erstellt
	   Es wird eine Meldung mit dem Betrag der Lastschriften und der Überweisungen angezeigt und die Differenz daraus.
	   WICHTIG: Den Betrag, der unter Differenz ausgewiesen wird, muss mit der Summe der Rechnungen abzüglich der Gutschrifen aus der EEG-Faktura übereinstimmen.

E) IMPORT DER DATEI IN DIE TELEBANKING-SOFTWARE

	1) Bitte die Anleitung der jeweiligen Software beachten
 
	2) Bisher wurde die Datei in ELBA-Business und ELBA-Infinity importiert
	    
