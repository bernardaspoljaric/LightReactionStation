:loop
if exist off.txt goto end
taskkill /F /IM explorer.exe
start "" /wait "C:\UnityProjectsLive\HNB\hnb_rfid_registracija\Build\rfid_registracija_v_1.0.3\RFID Registracija.exe"
goto loop
:end