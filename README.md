# DualSense GameBar Listener 🎮

Una aplicación nativa de Windows (Bandeja del Sistema) escrita en C# que permite abrir la **Xbox Game Bar** directamente utilizando el **Botón PlayStation** de un control DualSense (PS5) o DualSense Edge.

A diferencia de scripts de AutoHotkey o emuladores pesados (como DS4Windows o Steam), este programa es extremadamente ligero, consume ~0% de CPU y se comunica directamente con la API COM de Windows para invocar la Game Bar sin simular atajos de teclado.

## Características ✨
* **100% Segundo Plano:** Sin ventanas molestas. Vive en la bandeja del sistema (System Tray).
* **Soporte USB y Bluetooth:** Funciona sin importar cómo conectes tu control gracias a la integración nativa con DirectInput.
* **Extremadamente Ligero:** Escrito en WinForms puro. El hilo descansa por 2 segundos si no hay controles, y chequea eficientemente a 33Hz cuando hay uno conectado.
* **Sin Atajos de Teclado:** Utiliza `ApplicationActivationManager` (COM) para abrir la aplicación UWP de Xbox Game Bar de forma directa, esquivando el bloqueo de Windows (Foreground Lock) inyectando la tecla fantasma F24.
* **Conectar y Jugar:** Soporta desconexiones y reconexiones en tiempo real. Reconoce instantáneamente tu control sin necesidad de reiniciar la app.

## ¿Cómo instalar? 📦
Si solo quieres usar la aplicación, descarga el instalador desde la pestaña de **Releases** de este repositorio e instálalo como cualquier otro programa de Windows.

## ¿Cómo compilar desde el código fuente? 🛠️
No necesitas instalar Visual Studio. Tu PC con Windows ya tiene el compilador necesario.

1. Clona o descarga este repositorio.
2. Reemplaza `icon.ico` con el icono que prefieras (opcional).
3. Dale doble clic al archivo `compilar.bat`.
4. ¡Listo! Se generará un nuevo `DualSenseGameBar.exe`.

Para generar el instalador formal `.exe`, necesitas instalar **Inno Setup 7** y compilar el archivo `setup.iss`.

## Detalles Técnicos (Auditoría) 🔬
* El botón de PS es reconocido en los identificadores de bits `4096` y `8192` (Botones 13 y 14 de DirectInput).
* Se ejecuta un hilo secundario MTA/STA al invocar COM para prevenir cuelgues si la Game Bar se congela.
* Emplea un `Mutex` global para evitar que abras la aplicación múltiples veces por error.
