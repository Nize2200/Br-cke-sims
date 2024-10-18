# XInputRemapper WS(Windows Service) ver.

## Installation
Installera genom cmd (administator) exe filen som ligger i "bin" katalog.
```bash
sc create service_test1 binPath="C:\...\bin\Release\sims_ws_test2.exe"
```

## Start och stopp
### Start
Du kan starta programmet genom cmd.
```bash
sc start sims_ws_test2
```
Eller genom "Services" på Windows kan man starta och stoppa

### Stopp
Du kan stoppa programmet genom cmd.
```bash
st stop sims_ws_test2
```

#### För test, genererar programmet en textfil i "C:\\temp\\scan\\service_check.txt;" så att man kan kolla resultaten i text.
