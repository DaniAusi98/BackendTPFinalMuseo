using Domain.RecursoMuseo.Entities;
using static Domain.RecursoMuseo.Enums.Enums;

namespace Domain.RecursoMuseo.Factories
{
    public static class RecursoFactory
    {
        public static List<Recurso> CrearRecursosPorDefecto()
        {
            return
            [
                // =====================================================
                // MOBILIARIO
                // =====================================================

                new Recurso(
                    "Mesa plegable rectangular",
                    TipoRecurso.Mobiliario,
                    "Mesa plegable de estructura metálica y superficie laminada, aproximadamente 180 x 75 cm, destinada a montaje de eventos, talleres y actividades educativas.",
                    10
                ),

                new Recurso(
                    "Silla apilable",
                    TipoRecurso.Mobiliario,
                    "Silla de estructura metálica con asiento y respaldo plástico, apilable para facilitar almacenamiento y montaje de salas.",
                    80
                ),

                new Recurso(
                    "Mesa alta tipo cóctel",
                    TipoRecurso.Mobiliario,
                    "Mesa alta para recepción y eventos, con estructura metálica y superficie circular de aproximadamente 70 cm de diámetro.",
                    6
                ),

                new Recurso(
                    "Mantel blanco rectangular",
                    TipoRecurso.Mobiliario,
                    "Mantel textil blanco para mesas rectangulares de aproximadamente 180 x 75 cm.",
                    20
                ),

                new Recurso(
                    "Mantel negro rectangular",
                    TipoRecurso.Mobiliario,
                    "Mantel textil negro para mesas rectangulares de aproximadamente 180 x 75 cm.",
                    10
                ),

                new Recurso(
                    "Vaso descartable",
                    TipoRecurso.Mobiliario,
                    "Vaso descartable de aproximadamente 200 ml para servicio de refrigerios durante actividades y eventos.",
                    200
                ),

                new Recurso(
                    "Taza de cerámica",
                    TipoRecurso.Mobiliario,
                    "Taza de cerámica de aproximadamente 250 ml para servicio de café, té y otros refrigerios.",
                    50
                ),

                new Recurso(
                    "Jarra para bebidas",
                    TipoRecurso.Mobiliario,
                    "Jarra reutilizable de aproximadamente 1,5 litros para servicio de bebidas durante eventos.",
                    10
                ),

                new Recurso(
                    "Atril de conferencia",
                    TipoRecurso.Mobiliario,
                    "Atril de estructura metálica para exposiciones, conferencias y presentaciones, con superficie inclinada para apoyo de documentos.",
                    3
                ),

                new Recurso(
                    "Perchero móvil",
                    TipoRecurso.Mobiliario,
                    "Perchero móvil metálico con ruedas para guardar prendas durante eventos y actividades con público.",
                    3
                ),

                // =====================================================
                // TECNOLÓGICO
                // =====================================================

                new Recurso(
                    "Notebook HP ProBook 450 G10",
                    TipoRecurso.Tecnologico,
                    "Notebook de 15,6 pulgadas con procesador Intel Core i5-1335U, memoria DDR4 de hasta 32 GB, almacenamiento SSD PCIe NVMe y conectividad Wi-Fi 6E y Bluetooth 5.3.",
                    5
                ),

                new Recurso(
                    "Notebook Lenovo ThinkPad E14 Gen 5",
                    TipoRecurso.Tecnologico,
                    "Notebook empresarial de 14 pulgadas orientada a tareas administrativas, presentaciones y actividades educativas.",
                    3
                ),

                new Recurso(
                    "Tablet Samsung Galaxy Tab",
                    TipoRecurso.Tecnologico,
                    "Tablet Android con pantalla táctil para actividades educativas, consulta de contenidos digitales y utilización de aplicaciones interactivas.",
                    6
                ),

                new Recurso(
                    "Monitor Dell 24 pulgadas",
                    TipoRecurso.Tecnologico,
                    "Monitor LED de 24 pulgadas con resolución Full HD para conexión a notebooks y equipos de escritorio.",
                    3
                ),

                new Recurso(
                    "PC de escritorio HP",
                    TipoRecurso.Tecnologico,
                    "Computadora de escritorio para tareas administrativas, gestión de contenidos y actividades educativas digitales.",
                    3
                ),

                new Recurso(
                    "Cargador USB-C universal",
                    TipoRecurso.Tecnologico,
                    "Cargador USB-C multipropósito compatible con notebooks, tablets y dispositivos electrónicos que admiten carga mediante USB-C.",
                    8
                ),

                new Recurso(
                    "Hub USB-C multipuerto",
                    TipoRecurso.Tecnologico,
                    "Adaptador USB-C multipuerto con conexiones USB, HDMI y alimentación para ampliar la conectividad de notebooks durante eventos y presentaciones.",
                    5
                ),

                new Recurso(
                    "Router TP-Link Archer",
                    TipoRecurso.Tecnologico,
                    "Router inalámbrico de doble banda para proporcionar conectividad Wi-Fi durante actividades y eventos que requieren acceso a red.",
                    2
                ),

                // =====================================================
                // AUDIOVISUAL
                // =====================================================

                new Recurso(
                    "Proyector Epson PowerLite FH52+",
                    TipoRecurso.Audiovisual,
                    "Proyector 3LCD con resolución nativa Full HD 1920 x 1080, brillo de 4.000 lúmenes en color y blanco, relación de aspecto 16:9 y entradas HDMI.",
                    2
                ),

                new Recurso(
                    "Pantalla de proyección portátil 100 pulgadas",
                    TipoRecurso.Audiovisual,
                    "Pantalla portátil para proyección frontal con superficie aproximada de 100 pulgadas, estructura plegable y soporte independiente.",
                    2
                ),

                new Recurso(
                    "Sistema inalámbrico Shure BLX24/SM58",
                    TipoRecurso.Audiovisual,
                    "Sistema de micrófono inalámbrico compuesto por receptor BLX4 y transmisor de mano BLX2 con cápsula dinámica SM58, con alcance operativo de hasta 100 metros.",
                    4
                ),

                new Recurso(
                    "Sistema inalámbrico Shure BLX24/B58",
                    TipoRecurso.Audiovisual,
                    "Sistema de micrófono inalámbrico compuesto por receptor BLX4 y transmisor BLX2 con cápsula Beta 58A para presentaciones y conferencias.",
                    2
                ),

                new Recurso(
                    "Parlante activo JBL EON715",
                    TipoRecurso.Audiovisual,
                    "Parlante activo de 15 pulgadas con potencia de 650 W RMS y 1.300 W pico, Bluetooth, DSP integrado, entradas XLR y presión sonora máxima de 128 dB.",
                    4
                ),

                new Recurso(
                    "Consola de sonido Yamaha MG10XU",
                    TipoRecurso.Audiovisual,
                    "Consola de mezcla de audio de 10 canales con entradas XLR, alimentación phantom, efectos SPX y conectividad USB para reproducción y grabación.",
                    1
                ),

                new Recurso(
                    "Cámara Canon EOS R50",
                    TipoRecurso.Audiovisual,
                    "Cámara mirrorless con sensor CMOS APS-C de aproximadamente 24,2 megapíxeles, montura Canon RF y sistema Dual Pixel CMOS AF.",
                    2
                ),

                new Recurso(
                    "Cámara web Logitech Brio 4K",
                    TipoRecurso.Audiovisual,
                    "Cámara web con resolución hasta 4K a 30 fps, sensor de 13 MP, enfoque automático, HDR, campo de visión configurable y conexión USB.",
                    3
                ),

                new Recurso(
                    "Grabador de audio Zoom H4n Pro",
                    TipoRecurso.Audiovisual,
                    "Grabador portátil de audio digital con micrófonos estéreo integrados, entradas para micrófonos externos y grabación multipista.",
                    2
                ),

                new Recurso(
                    "Trípode para cámara Manfrotto",
                    TipoRecurso.Audiovisual,
                    "Trípode fotográfico de aluminio con cabezal ajustable y montaje estándar de 1/4 de pulgada para cámaras y videocámaras.",
                    3
                ),

                new Recurso(
                    "Proyector láser Epson",
                    TipoRecurso.Audiovisual,
                    "Proyector láser de tecnología 3LCD para presentaciones audiovisuales, con resolución Full HD y conectividad HDMI.",
                    1
                )
            ];
        }
    }
}